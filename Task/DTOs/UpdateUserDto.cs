using System.ComponentModel.DataAnnotations;

namespace Task.DTOs
{
    public class UpdateUserDto
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }
    }
}
