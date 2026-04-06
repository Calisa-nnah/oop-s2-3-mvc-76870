using System.ComponentModel.DataAnnotations;

namespace VgcCollege.Models
{
    // This class represents a branch in the college
    public class Branch
    {
        // Primary key (unique ID for each branch)
        public int Id { get; set; }

        // Name of the branch (required, max 100 characters)
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        // Address of the branch (required, max 200 characters)
        [Required]
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;

        // Navigation property: one branch can have many courses
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}