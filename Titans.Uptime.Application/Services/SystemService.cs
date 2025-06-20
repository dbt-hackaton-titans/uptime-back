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
            return await _context.Systems
            .Include(s => s.Components)
            .Select(s => new SystemDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                CreatedAt = s.CreatedAt,
                Components = s.Components.Select(c => new ComponentDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    SystemId = c.SystemId,
                    SystemName = s.Name,
                    CreatedAt = c.CreatedAt
                }).ToList()
            })
            .ToListAsync();
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
        public async Task<SystemDto?> UpdateAsync(int id, CreateSystemRequest request)
        {
            var system = await _context.Systems.FindAsync(id);
            if (system == null) return null;

            system.Name = request.Name;
            system.Description = request.Description;

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
        public async Task<bool> DeleteAsync(int id)
        {
            var system = await _context.Systems.FindAsync(id);
            if (system == null) return false;

            _context.Systems.Remove(system);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
