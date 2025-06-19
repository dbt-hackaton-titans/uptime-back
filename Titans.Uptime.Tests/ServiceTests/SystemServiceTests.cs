using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titans.Uptime.Application.Interfaces;
using Titans.Uptime.Application.Services;
using Titans.Uptime.Domain.Contracts;

namespace Titans.Uptime.Tests.ServiceTests
{
    public class SystemServiceIntegrationTests
    {
        [Fact]
        public async Task CreateAsync_ShouldAddSystem_AndGetById_ReturnsIt()
        {
            // Arrange
            var dbContext = TestHelpers.CreateInMemoryContext();
            ISystemService service = new SystemService(dbContext);

            var createRequest = new CreateSystemRequest
            {
                Name = "Sistema 1",
                Description = "Sistema principal"
            };

            // Act
            var created = await service.CreateAsync(createRequest);

            // Assert
            Assert.NotNull(created);
            Assert.True(created.Id > 0);
            Assert.Equal("Sistema 1", created.Name);
        }

        [Fact]
        public async Task GetById_ShouldCreatedFirst_Then_ReturnsIt()
        {
            // Arrange
            var dbContext = TestHelpers.CreateInMemoryContext();
            ISystemService service = new SystemService(dbContext);

            var createRequest = new CreateSystemRequest
            {
                Name = "Sistema 1",
                Description = "Sistema principal"
            };

            // Act
            var created = await service.CreateAsync(createRequest);

            // Assert
            Assert.NotNull(created);
            Assert.True(created.Id > 0);
            Assert.Equal("Sistema 1", created.Name);

            // Act 2: Buscar el sistema por ID
            var found = await service.GetByIdAsync(created.Id);

            // Assert 2
            Assert.NotNull(found);
            Assert.Equal(created.Id, found.Id);
            Assert.Equal("Sistema principal", found.Description);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllSystems()
        {
            // Arrange
            var dbContext = TestHelpers.CreateInMemoryContext();
            ISystemService service = new SystemService(dbContext);

            // Agrega varios sistemas
            await service.CreateAsync(new CreateSystemRequest { Name = "A" });
            await service.CreateAsync(new CreateSystemRequest { Name = "B" });

            // Act
            var all = (await service.GetAllAsync()).ToList();

            // Assert
            Assert.Equal(2, all.Count);
            Assert.Contains(all, s => s.Name == "A");
            Assert.Contains(all, s => s.Name == "B");
        }
    }
}
