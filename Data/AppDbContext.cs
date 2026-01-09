using GenMLMPlanApp.Models;
using Microsoft.EntityFrameworkCore;

namespace GenMLMPlanApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Self-Referencing Relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.Sponsor)
                .WithMany(u => u.DirectReferrals)
                .HasForeignKey(u => u.SponsorId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            // Unique Constraint on Email or UserId can be added here
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
