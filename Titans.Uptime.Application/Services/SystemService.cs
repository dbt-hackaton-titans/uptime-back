using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titans.Uptime.Application.Interfaces;
using Titans.Uptime.Domain.Contracts;
using Titans.Uptime.Domain.Models;
using Titans.Uptime.Persistence;

namespace Titans.Uptime.Application.Services
{
    public class SystemService : ISystemService
    {
        private readonly UptimeMonitorContext _context;

        public SystemService(UptimeMonitorContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<SystemDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
        public Task<SystemDto?> GetByIdAsync(int id)
        {
            SystemDto result = new SystemDto();

            result.Id = 1;
            result.Name = "Sistema 1";
            result.Description = "Sistema principal";

            return Task.FromResult<SystemDto?>(result);
        }
        public async Task<SystemDto> CreateAsync(CreateSystemRequest request)
        {
            var system = new SystemEntity
            {
                Name = request.Name,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow
            };

            _context.Systems.Add(system);
            await _context.SaveChangesAsync();

            return new SystemDto
            {
                Id = system.Id,
                Name = system.Name,
                Description = system.Description,
                CreatedAt = system.CreatedAt,
                Components = new List<ComponentDto>()
            };
        }
        public Task<SystemDto?> UpdateAsync(int id, CreateSystemRequest request)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
