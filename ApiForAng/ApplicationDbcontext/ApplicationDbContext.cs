using ApiForAng.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiForAng.ApplicationDbcontext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<User> uses{ get; set; }
    }
}
