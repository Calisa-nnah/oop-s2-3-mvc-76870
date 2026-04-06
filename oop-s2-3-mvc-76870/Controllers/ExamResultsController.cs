using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VgcCollege.Data;
using VgcCollege.Models;

namespace VgcCollege.Controllers
{
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
            var results = _context.ExamResults
                .Include(r => r.StudentProfile)
                .Include(r => r.Exam)
                    .ThenInclude(e => e.Course);

            return View(await results.ToListAsync());
        }

        // ===================== DETAILS =====================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var result = await _context.ExamResults
                .Include(r => r.StudentProfile)
                .Include(r => r.Exam)
                    .ThenInclude(e => e.Course)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (result == null) return NotFound();

            return View(result);
        }

    
        public IActionResult Create()
        {
            ViewData["StudentProfileId"] = new SelectList(_context.StudentProfiles, "Id", "Email");
            ViewData["ExamId"] = new SelectList(_context.Exams, "Id", "Title");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExamResult examResult)
        {
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
        public async Task<IActionResult> Edit(int id, ExamResult examResult)
        {
            if (id != examResult.Id) return NotFound();

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
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["StudentProfileId"] = new SelectList(_context.StudentProfiles, "Id", "Email", examResult.StudentProfileId);
            ViewData["ExamId"] = new SelectList(_context.Exams, "Id", "Title", examResult.ExamId);

            return View(examResult);
        }

      
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var result = await _context.ExamResults
                .Include(r => r.StudentProfile)
                .Include(r => r.Exam)
                    .ThenInclude(e => e.Course)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (result == null) return NotFound();

            return View(result);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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