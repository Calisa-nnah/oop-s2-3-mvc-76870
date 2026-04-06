using System.ComponentModel.DataAnnotations;

namespace VgcCollege.Models
{
    // This class represents a student's attendance record
    public class AttendanceRecord
    {
        // Primary key (unique ID for each record)
        public int Id { get; set; }

        // Foreign key linking to CourseEnrolment
        [Required]
        public int CourseEnrolmentId { get; set; }

        // Navigation property: the enrolment this attendance belongs to
        public CourseEnrolment? CourseEnrolment { get; set; }

        // Week number of the class (must be between 1 and 52)
        [Range(1, 52)]
        public int WeekNumber { get; set; }

        // Indicates if the student was present or not
        public bool Present { get; set; }
    }
}