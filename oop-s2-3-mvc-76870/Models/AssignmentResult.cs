using System.ComponentModel.DataAnnotations;

namespace VgcCollege.Models
{
    // This class represents a student's result for an assignment
    public class AssignmentResult
    {
        // Primary key (unique ID for each result)
        public int Id { get; set; }

        // Foreign key linking to Assignment
        [Required]
        public int AssignmentId { get; set; }

        // Navigation property: the assignment this result belongs to
        public Assignment? Assignment { get; set; }

        // Foreign key linking to StudentProfile
        [Required]
        public int StudentProfileId { get; set; }

        // Navigation property: the student who received this result
        public StudentProfile? StudentProfile { get; set; }

        // Score achieved by the student (between 0 and 1000)
        [Range(0, 1000)]
        public double Score { get; set; }

        // Optional feedback for the assignment (max 500 characters)
        [StringLength(500)]
        public string? Feedback { get; set; }
    }
}