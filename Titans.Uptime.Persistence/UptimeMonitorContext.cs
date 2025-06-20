using Microsoft.EntityFrameworkCore;
using Titans.Uptime.Domain.Models;

namespace Titans.Uptime.Persistence
{
    public class UptimeMonitorContext : DbContext
    {
        public UptimeMonitorContext(DbContextOptions<UptimeMonitorContext> options) : base(options) { }

        public DbSet<SystemEntity> Systems { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<UptimeCheck> UptimeChecks { get; set; }

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

            // UptimeCheck configuration
            modelBuilder.Entity<UptimeCheck>(entity =>
            {
                entity.ToTable("UptimeChecks");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CheckUrl).IsRequired().HasMaxLength(500);
                entity.Property(e => e.CheckType).HasConversion<string>();
                entity.Property(e => e.Status).HasConversion<string>();
                entity.Property(e => e.ResponseStringType).HasConversion<string>();
                entity.Property(e => e.RequestHeaders).HasColumnType("TEXT");
                entity.Property(e => e.AlertEmails).HasColumnType("TEXT");
                entity.Property(e => e.AlertMessage).HasColumnType("TEXT");
                entity.Property(e => e.LastError).HasColumnType("TEXT");

                entity.HasOne(e => e.System)
                      .WithMany(s => s.UptimeChecks)
                      .HasForeignKey(e => e.SystemId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Component)
                      .WithMany(c => c.UptimeChecks)
                      .HasForeignKey(e => e.ComponentId)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasIndex(e => e.Name).IsUnique();
                entity.HasIndex(e => e.IsActive);
                entity.HasIndex(e => e.Status);
            });

            // Seed data for development
            modelBuilder.Entity<SystemEntity>().HasData(
                new SystemEntity { Id = 1, Name = "Web Platform", Description = "Main web application platform", CreatedAt = DateTime.UtcNow },
                new SystemEntity { Id = 2, Name = "API Gateway", Description = "External API gateway services", CreatedAt = DateTime.UtcNow }
            );
        }
    }
}
