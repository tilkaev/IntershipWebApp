using IntershipWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace IntershipWebApp.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions options) : base(options)
        {
                
        }

        public DbSet<Status> Statuses { get; set; } = null!;

        public DbSet<_Task> Tasks { get; set; }

    }

}
