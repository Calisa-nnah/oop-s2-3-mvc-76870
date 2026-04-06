using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VgcCollege.Data;
using VgcCollege.Models;

namespace VgcCollege.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StudentProfilesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentProfilesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StudentProfiles
        public async Task<IActionResult> Index()
        {
            var students = _context.StudentProfiles.Include(s => s.IdentityUser);
            return View(await students.ToListAsync());
        }

        // GET: StudentProfiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var studentProfile = await _context.StudentProfiles
                .Include(s => s.IdentityUser)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (studentProfile == null) return NotFound();

            return View(studentProfile);
        }

        // GET: StudentProfiles/Create
        public IActionResult Create()
        {
            ViewData["IdentityUserId"] = new SelectList(
                _context.Users
                    .Where(u => !_context.StudentProfiles.Select(s => s.IdentityUserId).Contains(u.Id)),
                "Id",
                "Email");
            return View();
        }

        // POST: StudentProfiles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentProfile studentProfile)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studentProfile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdentityUserId"] = new SelectList(
                _context.Users
                    .Where(u => !_context.StudentProfiles.Select(s => s.IdentityUserId).Contains(u.Id)),
                "Id",
                "Email",
                studentProfile.IdentityUserId);

            return View(studentProfile);
        }

        // GET: StudentProfiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var studentProfile = await _context.StudentProfiles.FindAsync(id);
            if (studentProfile == null) return NotFound();

            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Email", studentProfile.IdentityUserId);
            return View(studentProfile);
        }

        // POST: StudentProfiles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StudentProfile studentProfile)
        {
            if (id != studentProfile.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentProfile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentProfileExists(studentProfile.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Email", studentProfile.IdentityUserId);
            return View(studentProfile);
        }

        // GET: StudentProfiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var studentProfile = await _context.StudentProfiles
                .Include(s => s.IdentityUser)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (studentProfile == null) return NotFound();

            return View(studentProfile);
        }

        // POST: StudentProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studentProfile = await _context.StudentProfiles.FindAsync(id);
            if (studentProfile != null)
            {
                _context.StudentProfiles.Remove(studentProfile);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool StudentProfileExists(int id)
        {
            return _context.StudentProfiles.Any(e => e.Id == id);
        }
    }
}