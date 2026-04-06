namespace VgcCollege.Models
{
    // This class links faculty members to courses (many-to-many relationship)
    public class FacultyCourseAssignment
    {
        // Primary key (unique ID for each assignment)
        public int Id { get; set; }

        // Foreign key linking to FacultyProfile
        public int FacultyProfileId { get; set; }

        // Navigation property: the faculty member assigned
        public FacultyProfile? FacultyProfile { get; set; }

        // Foreign key linking to Course
        public int CourseId { get; set; }

        // Navigation property: the course assigned
        public Course? Course { get; set; }
    }
}