using Microsoft.EntityFrameworkCore;

namespace Titans.Uptime.Persistence
{
    public class UptimeMonitorContext : DbContext
    {
        public UptimeMonitorContext(DbContextOptions<UptimeMonitorContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
