using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VgcCollege.Models;

namespace VgcCollege.Data
{
    // This class represents the database context
    // It connects all models to the database using Entity Framework
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        // Constructor to pass database options
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Tables in the database
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentProfile> StudentProfiles { get; set; }
        public DbSet<FacultyProfile> FacultyProfiles { get; set; }
        public DbSet<FacultyCourseAssignment> FacultyCourseAssignments { get; set; }
        public DbSet<CourseEnrolment> CourseEnrolments { get; set; }
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<AssignmentResult> AssignmentResults { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamResult> ExamResults { get; set; }

        // Configure relationships and constraints
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Prevent a student from enrolling in the same course twice
            builder.Entity<CourseEnrolment>()
                .HasIndex(e => new { e.StudentProfileId, e.CourseId })
                .IsUnique();

            // Prevent a faculty member from being assigned to the same course twice
            builder.Entity<FacultyCourseAssignment>()
                .HasIndex(f => new { f.FacultyProfileId, f.CourseId })
                .IsUnique();

            // Link StudentProfile to Identity user
            builder.Entity<StudentProfile>()
                .HasOne(s => s.IdentityUser)
                .WithMany()
                .HasForeignKey(s => s.IdentityUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Link FacultyProfile to Identity user
            builder.Entity<FacultyProfile>()
                .HasOne(f => f.IdentityUser)
                .WithMany()
                .HasForeignKey(f => f.IdentityUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}