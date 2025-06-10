using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Task.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        // Foreign Key property
        public int UserId { get; set; }

        // Navigation property to the User (the "one" in the one-to-many)
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
