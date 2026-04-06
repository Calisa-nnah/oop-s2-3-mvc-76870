using System.ComponentModel.DataAnnotations;

namespace VgcCollege.Models
{
    // This class represents an exam for a course
    public class Exam
    {
        // Primary key (unique ID for each exam)
        public int Id { get; set; }

        // Foreign key linking to Course
        [Required]
        public int CourseId { get; set; }

        // Navigation property: the course this exam belongs to
        public Course? Course { get; set; }

        // Exam title (required, max 150 characters)
        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        // Date of the exam
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        // Maximum score for the exam (between 0 and 1000)
        [Range(0, 1000)]
        public double MaxScore { get; set; }

        // Indicates if results have been released
        public bool ResultsReleased { get; set; }

        // Navigation property: one exam can have many results
        public ICollection<ExamResult> ExamResults { get; set; } = new List<ExamResult>();
    }
}