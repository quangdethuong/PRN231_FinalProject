using Microsoft.EntityFrameworkCore;
using Project.Services.ShoppingCartAPI.Models;

namespace Project.Services.ShoppingCartAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<CartHeader> CartHeaders { get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }

        
    }
}
