using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VgcCollege.Data;
using VgcCollege.Models;

namespace VgcCollege.Controllers
{
    [Authorize]
    public class ExamResultsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExamResultsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===================== INDEX =====================
        public async Task<IActionResult> Index()
        {
            // Admin and Faculty can see all exam results
            if (User.IsInRole("Admin") || User.IsInRole("Faculty"))
            {
                var allResults = _context.ExamResults
                    .Include(r => r.StudentProfile)
                    .Include(r => r.Exam)
                        .ThenInclude(e => e.Course)
                            .ThenInclude(c => c.Branch);

                return View(await allResults.ToListAsync());
            }

            // Student can only see their own released exam results
            var userEmail = User.Identity?.Name;

            var studentResults = _context.ExamResults
                .Include(r => r.StudentProfile)
                .Include(r => r.Exam)
                    .ThenInclude(e => e.Course)
                        .ThenInclude(c => c.Branch)
                .Where(r => r.StudentProfile != null
                         && r.StudentProfile.Email == userEmail
                         && r.Exam != null
                         && r.Exam.ResultsReleased);

            return View(await studentResults.ToListAsync());
        }

        // ===================== DETAILS =====================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var result = await _context.ExamResults
                .Include(r => r.StudentProfile)
                .Include(r => r.Exam)
                    .ThenInclude(e => e.Course)
                        .ThenInclude(c => c.Branch)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (result == null) return NotFound();

            // Students can only view their own released result
            if (User.IsInRole("Student"))
            {
                var userEmail = User.Identity?.Name;

                if (result.StudentProfile?.Email != userEmail || result.Exam == null || !result.Exam.ResultsReleased)
                {
                    return Forbid();
                }
            }

            return View(result);
        }

        // ===================== CREATE GET =====================
        [Authorize(Roles = "Admin,Faculty")]
        public IActionResult Create()
        {
            ViewData["StudentProfileId"] = new SelectList(_context.StudentProfiles, "Id", "Email");
            ViewData["ExamId"] = new SelectList(_context.Exams, "Id", "Title");
            return View();
        }

        // ===================== CREATE POST =====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> Create(ExamResult examResult)
        {
            bool duplicateExists = await _context.ExamResults
                .AnyAsync(r => r.StudentProfileId == examResult.StudentProfileId && r.ExamId == examResult.ExamId);

            if (duplicateExists)
            {
                ModelState.AddModelError("", "Result already exists for this student and exam.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(examResult);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["StudentProfileId"] = new SelectList(_context.StudentProfiles, "Id", "Email", examResult.StudentProfileId);
            ViewData["ExamId"] = new SelectList(_context.Exams, "Id", "Title", examResult.ExamId);
            return View(examResult);
        }

     
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var result = await _context.ExamResults.FindAsync(id);
            if (result == null) return NotFound();

            ViewData["StudentProfileId"] = new SelectList(_context.StudentProfiles, "Id", "Email", result.StudentProfileId);
            ViewData["ExamId"] = new SelectList(_context.Exams, "Id", "Title", result.ExamId);

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> Edit(int id, ExamResult examResult)
        {
            if (id != examResult.Id) return NotFound();

            bool duplicateExists = await _context.ExamResults
                .AnyAsync(r => r.StudentProfileId == examResult.StudentProfileId
                            && r.ExamId == examResult.ExamId
                            && r.Id != examResult.Id);

            if (duplicateExists)
            {
                ModelState.AddModelError("", "Result already exists for this student and exam.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(examResult);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.ExamResults.Any(e => e.Id == examResult.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["StudentProfileId"] = new SelectList(_context.StudentProfiles, "Id", "Email", examResult.StudentProfileId);
            ViewData["ExamId"] = new SelectList(_context.Exams, "Id", "Title", examResult.ExamId);
            return View(examResult);
        }

        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var result = await _context.ExamResults
                .Include(r => r.StudentProfile)
                .Include(r => r.Exam)
                    .ThenInclude(e => e.Course)
                        .ThenInclude(c => c.Branch)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (result == null) return NotFound();

            return View(result);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _context.ExamResults.FindAsync(id);

            if (result != null)
            {
                _context.ExamResults.Remove(result);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}