﻿using Microsoft.EntityFrameworkCore;
using Titans.Uptime.Domain.Models;

namespace Titans.Uptime.Persistence
{
    public class UptimeMonitorContext : DbContext
    {
        public UptimeMonitorContext(DbContextOptions<UptimeMonitorContext> options) : base(options) { }

        public DbSet<SystemEntity> Systems { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<UptimeCheck> UptimeChecks { get; set; }
        public DbSet<UptimeEvent> UptimeEvents { get; set; }

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

            // UptimeEvent configuration
            modelBuilder.Entity<UptimeEvent>(entity =>
            {
                entity.ToTable("Events");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.EventType).HasConversion<string>();
                entity.Property(e => e.Category).HasConversion<string>();
                entity.Property(e => e.MaintenanceType).HasConversion<string>();
                entity.Property(e => e.ErrorMessage).HasColumnType("TEXT");
                entity.Property(e => e.Notes).HasColumnType("TEXT");
                entity.Property(e => e.TicketCode).HasMaxLength(50);

                entity.HasOne(e => e.UptimeCheck)
                      .WithMany(u => u.Events)
                      .HasForeignKey(e => e.UptimeCheckId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.StartTime);
                entity.HasIndex(e => e.EventType);
                entity.HasIndex(e => e.IsResolved);
                entity.HasIndex(e => new { e.UptimeCheckId, e.StartTime });
            });
        }
    }
}
