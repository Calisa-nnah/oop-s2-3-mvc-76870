using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VgcCollege.Data;
using VgcCollege.Models;

namespace VgcCollege.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> MyResults()
        {
            var userEmail = User.Identity?.Name;

            var assignmentResults = await _context.AssignmentResults
                .Include(r => r.StudentProfile)
                .Include(r => r.Assignment)
                    .ThenInclude(a => a.Course)
                        .ThenInclude(c => c.Branch)
                .Where(r => r.StudentProfile != null && r.StudentProfile.Email == userEmail)
                .ToListAsync();

            var examResults = await _context.ExamResults
                .Include(r => r.StudentProfile)
                .Include(r => r.Exam)
                    .ThenInclude(e => e.Course)
                        .ThenInclude(c => c.Branch)
                .Where(r => r.StudentProfile != null
                         && r.StudentProfile.Email == userEmail
                         && r.Exam != null
                         && r.Exam.ResultsReleased)
                .ToListAsync();

            ViewBag.AssignmentResults = assignmentResults;
            ViewBag.ExamResults = examResults;

            return View();
        }
    }
}