using Microsoft.EntityFrameworkCore;
using TopSpeed.web.Models;

namespace TopSpeed.web.Data
{
    public class ApplicationDbContext : DbContext
    {
        private static DbContextOptions options;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Brand> Brand {  get; set; }
    }
}
