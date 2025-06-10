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
    public class UsersController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public UsersController(ApiDbContext context)
        {
            _context = context; // Dependency Injection
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _context.Users
                .Select(u => new UserDto { Id = u.Id, Username = u.Username, Email = u.Email })
                .ToListAsync();
            return Ok(users);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new UserDto { Id = u.Id, Username = u.Username, Email = u.Email })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // GET: api/Users/5/products
        [HttpGet("{id}/products")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetUserProducts(int id)
        {
            if (!_context.Users.Any(u => u.Id == id))
            {
                return NotFound("User not found.");
            }

            var products = await _context.Products
                .Where(p => p.UserId == id)
                .Select(p => new ProductDto { Id = p.Id, Name = p.Name, Price = p.Price, UserId = p.UserId })
                .ToListAsync();

            return Ok(products);
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<UserDto>> PostUser(CreateUserDto createUserDto)
        {
            var user = new User
            {
                Username = createUserDto.Username,
                Email = createUserDto.Email
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userDto = new UserDto { Id = user.Id, Username = user.Username, Email = user.Email };

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, userDto);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent(); // Success, no content to return
        }

        // PUT: api/Users/5 (Implement this as an exercise!)
        //كتله سويلي ال PUT 🙂
        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UpdateUserDto updateUserDto)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound(); // إذا لم يتم العثور على المستخدم، أرجع 404
            }

            // التحقق مما إذا كان البريد الإلكتروني أو اسم المستخدم الجديد مستخدماً بالفعل من قبل شخص آخر
            var usernameExists = await _context.Users.AnyAsync(u => u.Username == updateUserDto.Username && u.Id != id);
            var emailExists = await _context.Users.AnyAsync(u => u.Email == updateUserDto.Email && u.Id != id);

            if (usernameExists)
            {
                return BadRequest("Username is already taken by another user.");
            }

            if (emailExists)
            {
                return BadRequest("Email is already taken by another user.");
            }

            // تحديث بيانات المستخدم
            user.Username = updateUserDto.Username;
            user.Email = updateUserDto.Email;

            try
            {
                await _context.SaveChangesAsync(); // حفظ التغييرات في قاعدة البيانات
            }
            catch (DbUpdateConcurrencyException)
            {
                // هذا للتعامل مع حالات نادرة من التضارب
                if (!_context.Users.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // أرجع 204 No Content للإشارة إلى نجاح العملية
        }


    }
}
