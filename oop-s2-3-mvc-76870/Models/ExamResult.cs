using System.ComponentModel.DataAnnotations;

namespace VgcCollege.Models
{
    // This class represents a student's result for an exam
    public class ExamResult
    {
        // Primary key (unique ID for each result)
        public int Id { get; set; }

        // Foreign key linking to Exam
        [Required]
        public int ExamId { get; set; }

        // Navigation property: the exam this result belongs to
        public Exam? Exam { get; set; }

        // Foreign key linking to StudentProfile
        [Required]
        public int StudentProfileId { get; set; }

        // Navigation property: the student who received this result
        public StudentProfile? StudentProfile { get; set; }

        // Score achieved by the student (between 0 and 1000)
        [Range(0, 1000)]
        public double Score { get; set; }

        // Grade given to the student (max 10 characters)
        [StringLength(10)]
        public string Grade { get; set; } = string.Empty;
    }
}