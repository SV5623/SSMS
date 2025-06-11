using AutoSelect.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoSelect.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Console.WriteLine("AppDbContext initialized");
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Mechanic> Mechanics { get; set; }
        public DbSet<AutoSelect.Models.Task> Tasks { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Part> Parts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AutoSelect.Models.Task>().ToTable("Tasks");
        }
    }
}