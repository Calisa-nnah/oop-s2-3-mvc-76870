using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VgcCollege.Data;
using VgcCollege.Models;

namespace VgcCollege.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AttendanceRecordsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttendanceRecordsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AttendanceRecords
        public async Task<IActionResult> Index()
        {
            var attendanceRecords = _context.AttendanceRecords
                .Include(a => a.CourseEnrolment)
                    .ThenInclude(e => e.StudentProfile)
                .Include(a => a.CourseEnrolment)
                    .ThenInclude(e => e.Course)
                        .ThenInclude(c => c.Branch);

            return View(await attendanceRecords.ToListAsync());
        }

        // GET: AttendanceRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var attendanceRecord = await _context.AttendanceRecords
                .Include(a => a.CourseEnrolment)
                    .ThenInclude(e => e.StudentProfile)
                .Include(a => a.CourseEnrolment)
                    .ThenInclude(e => e.Course)
                        .ThenInclude(c => c.Branch)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (attendanceRecord == null) return NotFound();

            return View(attendanceRecord);
        }

        // GET: AttendanceRecords/Create
        public IActionResult Create()
        {
            ViewData["CourseEnrolmentId"] = new SelectList(
                _context.CourseEnrolments
                    .Include(e => e.StudentProfile)
                    .Include(e => e.Course)
                    .Select(e => new
                    {
                        e.Id,
                        Display = e.StudentProfile!.Name + " - " + e.Course!.Name
                    }),
                "Id",
                "Display");

            return View();
        }

        // POST: AttendanceRecords/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AttendanceRecord attendanceRecord)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attendanceRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CourseEnrolmentId"] = new SelectList(
                _context.CourseEnrolments
                    .Include(e => e.StudentProfile)
                    .Include(e => e.Course)
                    .Select(e => new
                    {
                        e.Id,
                        Display = e.StudentProfile!.Name + " - " + e.Course!.Name
                    }),
                "Id",
                "Display",
                attendanceRecord.CourseEnrolmentId);

            return View(attendanceRecord);
        }

        // GET: AttendanceRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var attendanceRecord = await _context.AttendanceRecords.FindAsync(id);
            if (attendanceRecord == null) return NotFound();

            ViewData["CourseEnrolmentId"] = new SelectList(
                _context.CourseEnrolments
                    .Include(e => e.StudentProfile)
                    .Include(e => e.Course)
                    .Select(e => new
                    {
                        e.Id,
                        Display = e.StudentProfile!.Name + " - " + e.Course!.Name
                    }),
                "Id",
                "Display",
                attendanceRecord.CourseEnrolmentId);

            return View(attendanceRecord);
        }

        // POST: AttendanceRecords/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AttendanceRecord attendanceRecord)
        {
            if (id != attendanceRecord.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attendanceRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.AttendanceRecords.Any(e => e.Id == attendanceRecord.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["CourseEnrolmentId"] = new SelectList(
                _context.CourseEnrolments
                    .Include(e => e.StudentProfile)
                    .Include(e => e.Course)
                    .Select(e => new
                    {
                        e.Id,
                        Display = e.StudentProfile!.Name + " - " + e.Course!.Name
                    }),
                "Id",
                "Display",
                attendanceRecord.CourseEnrolmentId);

            return View(attendanceRecord);
        }

        // GET: AttendanceRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var attendanceRecord = await _context.AttendanceRecords
                .Include(a => a.CourseEnrolment)
                    .ThenInclude(e => e.StudentProfile)
                .Include(a => a.CourseEnrolment)
                    .ThenInclude(e => e.Course)
                        .ThenInclude(c => c.Branch)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (attendanceRecord == null) return NotFound();

            return View(attendanceRecord);
        }

        // POST: AttendanceRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attendanceRecord = await _context.AttendanceRecords.FindAsync(id);
            if (attendanceRecord != null)
            {
                _context.AttendanceRecords.Remove(attendanceRecord);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}