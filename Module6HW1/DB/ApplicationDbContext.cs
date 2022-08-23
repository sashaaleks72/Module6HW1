using Microsoft.EntityFrameworkCore;
using Module6HW1.Models;

namespace Module6HW1.DB
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Teapot> Teapots { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) {
            Database.EnsureCreated();
        }
    }
}
