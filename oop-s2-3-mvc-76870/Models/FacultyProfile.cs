using System.ComponentModel.DataAnnotations;

namespace VgcCollege.Models
{
    // This class represents a faculty (lecturer) profile
    public class FacultyProfile
    {
        // Primary key (unique ID for each faculty member)
        public int Id { get; set; }

        // Links this profile to the ASP.NET Identity user
        [Required]
        public string IdentityUserId { get; set; } = string.Empty;

        // Navigation property: related Identity user
        public ApplicationUser? IdentityUser { get; set; }

        // Faculty name (required, max 100 characters)
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        // Faculty email (required and must be valid format)
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        // Optional phone number
        [Phone]
        public string? Phone { get; set; }

        // Navigation property: faculty can teach many courses
        public ICollection<FacultyCourseAssignment> FacultyCourseAssignments { get; set; } = new List<FacultyCourseAssignment>();
    }
}