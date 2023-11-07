using Microsoft.EntityFrameworkCore;
using Project.Services.EmailAPI.Models;

namespace Project.Services.EmailAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<EmailLogger> EmailLoggers { get; set; }
    }
}
