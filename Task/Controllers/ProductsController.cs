using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task.Data;
using Task.DTOs;
using Task.Models;
namespace Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public ProductsController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await _context.Products
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    UserId = p.UserId
                })
                .ToListAsync();

            return Ok(products);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _context.Products
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    UserId = p.UserId
                })
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<ProductDto>> PostProduct(CreateProductDto createProductDto)
        {
            // خطوة مهمة: التأكد من أن المستخدم الذي نربط به المنتج موجود فعلاً
            var userExists = await _context.Users.AnyAsync(u => u.Id == createProductDto.UserId);
            if (!userExists)
            {
                return BadRequest("Invalid User ID. The specified user does not exist.");
            }

            var product = new Product
            {
                Name = createProductDto.Name,
                Price = createProductDto.Price,
                UserId = createProductDto.UserId
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                UserId = product.UserId
            };

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, productDto);
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, UpdateProductDto updateProductDto)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            // التأكد من أن المستخدم الجديد موجود (إذا سمحنا بتغيير مالك المنتج)
            var userExists = await _context.Users.AnyAsync(u => u.Id == updateProductDto.UserId);
            if (!userExists)
            {
                return BadRequest("Invalid User ID. The specified user does not exist.");
            }

            product.Name = updateProductDto.Name;
            product.Price = updateProductDto.Price;
            product.UserId = updateProductDto.UserId; // تحديث مالك المنتج

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
