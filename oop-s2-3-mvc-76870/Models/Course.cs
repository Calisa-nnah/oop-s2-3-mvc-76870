using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgcCollege.Models
{
    // This class represents a course in the college
    public class Course
    {
        // Primary key (unique ID for each course)
        public int Id { get; set; }

        // Course name (required, max 150 characters)
        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        // Foreign key linking to Branch
        [Required]
        public int BranchId { get; set; }

        // Navigation property: this course belongs to one branch
        [ForeignKey("BranchId")]
        public Branch? Branch { get; set; }

        // Course start date
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        // Course end date
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        // Navigation property: one course can have many enrolments
        public ICollection<CourseEnrolment> CourseEnrolments { get; set; } = new List<CourseEnrolment>();

        // Navigation property: one course can have many assignments
        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

        // Navigation property: one course can have many exams
        public ICollection<Exam> Exams { get; set; } = new List<Exam>();

        // Navigation property: links course with faculty members
        public ICollection<FacultyCourseAssignment> FacultyCourseAssignments { get; set; } = new List<FacultyCourseAssignment>();
    }
}