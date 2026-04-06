using System.ComponentModel.DataAnnotations;

namespace VgcCollege.Models
{
    // This class represents a student profile in the system
    public class StudentProfile
    {
        // Primary key (unique ID for each student)
        public int Id { get; set; }

        // Links this profile to the ASP.NET Identity user
        [Required]
        public string IdentityUserId { get; set; } = string.Empty;

        // Navigation property: related Identity user
        public ApplicationUser? IdentityUser { get; set; }

        // Student full name (required, max 100 characters)
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        // Student email (required and must be valid format)
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        // Optional phone number
        [Phone]
        public string? Phone { get; set; }

        // Optional address (max 200 characters)
        [StringLength(200)]
        public string? Address { get; set; }

        // Student number (max 50 characters)
        [StringLength(50)]
        public string StudentNumber { get; set; } = string.Empty;

        // Navigation property: student can be enrolled in many courses
        public ICollection<CourseEnrolment> CourseEnrolments { get; set; } = new List<CourseEnrolment>();

        // Navigation property: student can have many assignment results
        public ICollection<AssignmentResult> AssignmentResults { get; set; } = new List<AssignmentResult>();

        // Navigation property: student can have many exam results
        public ICollection<ExamResult> ExamResults { get; set; } = new List<ExamResult>();
    }
}