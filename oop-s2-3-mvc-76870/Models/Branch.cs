using System.ComponentModel.DataAnnotations;

namespace VgcCollege.Models
{
    // Represents a college branch
    public class Branch
    {
        // Primary key (unique ID)
        public int Id { get; set; }

        // Branch name (required, max 100 characters)
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        // Branch address (required, max 200 characters)
        [Required]
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;

        // One branch can have many courses
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}