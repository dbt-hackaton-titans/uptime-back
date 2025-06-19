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
        }
    }
}
