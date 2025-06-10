using System.ComponentModel.DataAnnotations;
namespace Task.DTOs
{
    public class CreateUserDto
    {
        [Required] public string Username { get; set; }
        [Required][EmailAddress] public string Email { get; set; }
    }
}
