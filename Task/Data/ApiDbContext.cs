using Microsoft.EntityFrameworkCore;
using Task.Models;
namespace Task.Data
{
    public class ApiDbContext:DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Optional: Configure unique constraints if not handled by annotations
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
               .HasIndex(u => u.Username)
               .IsUnique();
        }
    }
}
