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
        public DbSet<Login> logins { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Category> categories { get; set; } 
        public DbSet<Cart> carts { get; set; }  
        public DbSet<Cartitems> cartitems { get; set; }
    }
}
