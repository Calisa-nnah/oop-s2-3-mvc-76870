using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VgcCollege.Data;
using VgcCollege.Models;

namespace VgcCollege.Controllers
{
    [Authorize(Roles = "Admin,Faculty")]
    public class AssignmentResultsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AssignmentResultsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AssignmentResults
        public async Task<IActionResult> Index()
        {
            var results = _context.AssignmentResults
                .Include(r => r.StudentProfile)
                .Include(r => r.Assignment)
                    .ThenInclude(a => a.Course)
                        .ThenInclude(c => c.Branch);

            return View(await results.ToListAsync());
        }

        // GET: AssignmentResults/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var result = await _context.AssignmentResults
                .Include(r => r.StudentProfile)
                .Include(r => r.Assignment)
                    .ThenInclude(a => a.Course)
                        .ThenInclude(c => c.Branch)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (result == null) return NotFound();

            return View(result);
        }

        // GET: AssignmentResults/Create
        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        // POST: AssignmentResults/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AssignmentResult result)
        {
            bool duplicateExists = await _context.AssignmentResults
                .AnyAsync(r => r.StudentProfileId == result.StudentProfileId && r.AssignmentId == result.AssignmentId);

            if (duplicateExists)
            {
                ModelState.AddModelError("", "Result already exists for this student and assignment.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(result);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateDropdowns(result.StudentProfileId, result.AssignmentId);
            return View(result);
        }

        // GET: AssignmentResults/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var result = await _context.AssignmentResults.FindAsync(id);
            if (result == null) return NotFound();

            PopulateDropdowns(result.StudentProfileId, result.AssignmentId);
            return View(result);
        }

        // POST: AssignmentResults/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AssignmentResult result)
        {
            if (id != result.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(result);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.AssignmentResults.Any(e => e.Id == result.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            PopulateDropdowns(result.StudentProfileId, result.AssignmentId);
            return View(result);
        }

        // GET: AssignmentResults/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var result = await _context.AssignmentResults
                .Include(r => r.StudentProfile)
                .Include(r => r.Assignment)
                    .ThenInclude(a => a.Course)
                        .ThenInclude(c => c.Branch)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (result == null) return NotFound();

            return View(result);
        }

        // POST: AssignmentResults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _context.AssignmentResults.FindAsync(id);
            if (result != null)
            {
                _context.AssignmentResults.Remove(result);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private void PopulateDropdowns(object? selectedStudent = null, object? selectedAssignment = null)
        {
            ViewData["StudentProfileId"] = new SelectList(
                _context.StudentProfiles,
                "Id",
                "Name",
                selectedStudent);

            ViewData["AssignmentId"] = new SelectList(
                _context.Assignments
                    .Include(a => a.Course)
                    .Select(a => new
                    {
                        a.Id,
                        Display = a.Title + " (" + a.Course!.Name + ")"
                    })
                    .ToList(),
                "Id",
                "Display",
                selectedAssignment);
        }
    }
}