using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titans.Uptime.Application.Interfaces;
using Titans.Uptime.Application.Services;
using Titans.Uptime.Domain.Contracts;
using Titans.Uptime.Domain;

namespace Titans.Uptime.Tests.ServiceTests
{
    public class UptimeCheckServiceTests
    {
        [Fact]
        public async Task CreateAsync_ShouldAddUptimeCheck_AndGetById()
        {
            // Arrange
            var dbContext = TestHelpers.CreateInMemoryContext();
            ISystemService systemService = new SystemService(dbContext);
            IUptimeCheckService uptimeCheckService = new UptimeCheckService(dbContext);

            var system = await systemService.CreateAsync(new CreateSystemRequest { Name = "Sys" });

            var checkRequest = new CreateUptimeCheckRequest
            {
                Name = "Ping API",
                SystemId = system.Id,
                CheckUrl = "8.8.8.8",
                CheckType = CheckType.Ping,
                AlertEmails = "admin@mail.com"
            };

            // Act
            var check = await uptimeCheckService.CreateAsync(checkRequest);

            // Assert
            Assert.NotNull(check);
            Assert.Equal("Ping API", check.Name);
            Assert.Equal(system.Id, check.SystemId);

            // Buscar por id
            var found = await uptimeCheckService.GetByIdAsync(check.Id);
            Assert.NotNull(found);
            Assert.Equal("Ping API", found.Name);
        }
    }
}
