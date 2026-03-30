using Microsoft.EntityFrameworkCore;
using phase_1.Models; // Để nhận diện được class Product

namespace phase_1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Users> Users { get; set; }
    }
}