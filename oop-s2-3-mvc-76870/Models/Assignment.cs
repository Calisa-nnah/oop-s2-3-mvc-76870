using System.ComponentModel.DataAnnotations;

namespace VgcCollege.Models
{
    // This class represents an assignment for a course
    public class Assignment
    {
        // Primary key (unique ID for each assignment)
        public int Id { get; set; }

        // Foreign key linking to Course
        [Required]
        public int CourseId { get; set; }

        // Navigation property: the course this assignment belongs to
        public Course? Course { get; set; }

        // Assignment title (required, max 150 characters)
        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        // Maximum score for the assignment (between 0 and 1000)
        [Range(0, 1000)]
        public double MaxScore { get; set; }

        // Assignment due date
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        // Navigation property: one assignment can have many results
        public ICollection<AssignmentResult> AssignmentResults { get; set; } = new List<AssignmentResult>();
    }
}