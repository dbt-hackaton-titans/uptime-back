using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<SystemDto>> GetAllAsync()
        {
            var systems = new List<SystemDto>
            {
                new SystemDto { Id = 1, Name = "A", Description = "Sistema A" },
                new SystemDto { Id = 2, Name = "B", Description = "Sistema B" }
            };

            return await Task.FromResult(systems);
        }
        public async Task<SystemDto?> GetByIdAsync(int id)
        {
            var system = await _context.Systems
            .Include(s => s.Components)
            .FirstOrDefaultAsync(s => s.Id == id);

            if (system == null) return null;

            return new SystemDto
            {
                Id = system.Id,
                Name = system.Name,
                Description = system.Description,
                CreatedAt = system.CreatedAt,
                Components = system.Components.Select(c => new ComponentDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    SystemId = c.SystemId,
                    SystemName = system.Name,
                    CreatedAt = c.CreatedAt
                }).ToList()
            };
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
