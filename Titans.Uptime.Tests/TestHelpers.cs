using Titans.Uptime.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Titans.Uptime.Tests
{
    public static class TestHelpers
    {
        public static UptimeMonitorContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<UptimeMonitorContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new UptimeMonitorContext(options);
        }
    }

}
