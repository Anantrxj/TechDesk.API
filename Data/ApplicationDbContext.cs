using Microsoft.EntityFrameworkCore;
using TechDesk.API.Models;

namespace TechDesk.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.CreatedBy)
                .WithMany() // or .WithMany(u => u.CreatedTickets) if User has that collection
                .HasForeignKey(t => t.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.AssignedEngineer)
                .WithMany() // or .WithMany(u => u.AssignedTickets) if User has that collection
                .HasForeignKey(t => t.AssignedEngineerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}