using Microsoft.EntityFrameworkCore;
using Titans.Uptime.Domain.Models;

namespace Titans.Uptime.Persistence
{
    public class UptimeMonitorContext : DbContext
    {
        public UptimeMonitorContext(DbContextOptions<UptimeMonitorContext> options) : base(options) { }

        public DbSet<SystemEntity> Systems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SystemEntity>(entity =>
            {
                entity.ToTable("Systems");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.HasIndex(e => e.Name).IsUnique();
            });
        }
    }
}
