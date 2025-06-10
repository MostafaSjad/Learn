
using System.ComponentModel.DataAnnotations;

namespace Task.DTOs
{
    public class CreateProductDto
    {
        [Required] public string Name { get; set; }
        [Required][Range(0.01, 1000000)] public decimal Price { get; set; }
        [Required] public int UserId { get; set; }
    }
}
