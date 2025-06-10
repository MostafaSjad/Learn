using System.ComponentModel.DataAnnotations;

namespace Task.DTOs
{
    public class UpdateProductDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(0.01, 1000000)]
        public decimal Price { get; set; }

        // قد لا ترغب في السماح بتغيير مالك المنتج، لكننا سنضيفها هنا كمثال
        [Required]
        public int UserId { get; set; }
    }
}
