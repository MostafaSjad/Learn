
using System.ComponentModel.DataAnnotations;
namespace Task.Models
{
    public class User
    {
        [Key] // Defines this property as the primary key
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        // Navigation property for the one-to-many relationship
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
