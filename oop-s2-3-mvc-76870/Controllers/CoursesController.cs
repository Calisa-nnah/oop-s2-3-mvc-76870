using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VgcCollege.Data;
using VgcCollege.Models;

namespace VgcCollege.Controllers
{
    // Only Admin users can access this controller
    [Authorize(Roles = "Admin")]
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructor to inject database context
        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Courses (list all courses)
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Courses.Include(c => c.Branch);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Courses/Details/5 (view course details)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var course = await _context.Courses
                .Include(c => c.Branch)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (course == null) return NotFound();

            return View(course);
        }

        // GET: Courses/Create (show create form)
        public IActionResult Create()
        {
            // Populate dropdown for branches
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Name");
            return View();
        }

        // POST: Courses/Create (save new course)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Reload dropdown if validation fails
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Name", course.BranchId);
            return View(course);
        }

        // GET: Courses/Edit/5 (show edit form)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            // Populate dropdown
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Name", course.BranchId);
            return View(course);
        }

        // POST: Courses/Edit/5 (update course)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Course course)
        {
            if (id != course.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Check if course still exists
                    if (!_context.Courses.Any(e => e.Id == course.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            // Reload dropdown if validation fails
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Name", course.BranchId);
            return View(course);
        }

        // GET: Courses/Delete/5 (confirm delete page)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var course = await _context.Courses
                .Include(c => c.Branch)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (course == null) return NotFound();

            return View(course);
        }

        // POST: Courses/Delete/5 (delete course)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}