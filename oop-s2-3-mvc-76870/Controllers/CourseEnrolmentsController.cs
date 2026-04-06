using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VgcCollege.Data;
using VgcCollege.Models;

namespace VgcCollege.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CourseEnrolmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourseEnrolmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CourseEnrolments
        public async Task<IActionResult> Index()
        {
            var enrolments = _context.CourseEnrolments
                .Include(e => e.StudentProfile)
                .Include(e => e.Course)
                    .ThenInclude(c => c.Branch);

            return View(await enrolments.ToListAsync());
        }

        // GET: CourseEnrolments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var enrolment = await _context.CourseEnrolments
                .Include(e => e.StudentProfile)
                .Include(e => e.Course)
                    .ThenInclude(c => c.Branch)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (enrolment == null) return NotFound();

            return View(enrolment);
        }

        // GET: CourseEnrolments/Create
        public IActionResult Create()
        {
            ViewData["StudentProfileId"] = new SelectList(_context.StudentProfiles, "Id", "Name");
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name");
            return View();
        }

        // POST: CourseEnrolments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseEnrolment enrolment)
        {
            bool duplicateExists = await _context.CourseEnrolments
                .AnyAsync(e => e.StudentProfileId == enrolment.StudentProfileId && e.CourseId == enrolment.CourseId);

            if (duplicateExists)
            {
                ModelState.AddModelError("", "This student is already enrolled in this course.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(enrolment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["StudentProfileId"] = new SelectList(_context.StudentProfiles, "Id", "Name", enrolment.StudentProfileId);
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", enrolment.CourseId);
            return View(enrolment);
        }

        // GET: CourseEnrolments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var enrolment = await _context.CourseEnrolments.FindAsync(id);
            if (enrolment == null) return NotFound();

            ViewData["StudentProfileId"] = new SelectList(_context.StudentProfiles, "Id", "Name", enrolment.StudentProfileId);
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", enrolment.CourseId);
            return View(enrolment);
        }

        // POST: CourseEnrolments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseEnrolment enrolment)
        {
            if (id != enrolment.Id) return NotFound();

            bool duplicateExists = await _context.CourseEnrolments
                .AnyAsync(e => e.StudentProfileId == enrolment.StudentProfileId
                            && e.CourseId == enrolment.CourseId
                            && e.Id != enrolment.Id);

            if (duplicateExists)
            {
                ModelState.AddModelError("", "This student is already enrolled in this course.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enrolment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.CourseEnrolments.Any(e => e.Id == enrolment.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["StudentProfileId"] = new SelectList(_context.StudentProfiles, "Id", "Name", enrolment.StudentProfileId);
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", enrolment.CourseId);
            return View(enrolment);
        }

        // GET: CourseEnrolments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var enrolment = await _context.CourseEnrolments
                .Include(e => e.StudentProfile)
                .Include(e => e.Course)
                    .ThenInclude(c => c.Branch)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (enrolment == null) return NotFound();

            return View(enrolment);
        }

        // POST: CourseEnrolments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enrolment = await _context.CourseEnrolments.FindAsync(id);
            if (enrolment != null)
            {
                _context.CourseEnrolments.Remove(enrolment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}