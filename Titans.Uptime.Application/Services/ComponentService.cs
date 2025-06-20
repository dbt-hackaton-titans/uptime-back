using Titans.Uptime.Application.Interfaces;
using Titans.Uptime.Domain.Contracts;
using Titans.Uptime.Domain.Models;
using Titans.Uptime.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Titans.Uptime.Application.Services
{
    public class ComponentService : IComponentService
    {
        private readonly UptimeMonitorContext _context;

        public ComponentService(UptimeMonitorContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ComponentDto>> GetAllAsync()
        {
            return await _context.Components
                .Include(c => c.System)
                .Select(c => new ComponentDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    SystemId = c.SystemId,
                    SystemName = c.System.Name,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ComponentDto>> GetBySystemIdAsync(int systemId)
        {
            return await _context.Components
                .Include(c => c.System)
                .Where(c => c.SystemId == systemId)
                .Select(c => new ComponentDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    SystemId = c.SystemId,
                    SystemName = c.System.Name,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<ComponentDto?> GetByIdAsync(int id)
        {
            var component = await _context.Components
                .Include(c => c.System)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (component == null) return null;

            return new ComponentDto
            {
                Id = component.Id,
                Name = component.Name,
                Description = component.Description,
                SystemId = component.SystemId,
                SystemName = component.System.Name,
                CreatedAt = component.CreatedAt
            };
        }

        public async Task<ComponentDto> CreateAsync(CreateComponentRequest request)
        {
            // Verify system exists
            var system = await _context.Systems.FindAsync(request.SystemId);
            if (system == null)
                throw new ArgumentException($"System with ID {request.SystemId} not found");

            var component = new Component
            {
                Name = request.Name,
                Description = request.Description,
                SystemId = request.SystemId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Components.Add(component);
            await _context.SaveChangesAsync();

            return new ComponentDto
            {
                Id = component.Id,
                Name = component.Name,
                Description = component.Description,
                SystemId = component.SystemId,
                SystemName = system.Name,
                CreatedAt = component.CreatedAt
            };
        }

        public async Task<ComponentDto?> UpdateAsync(int id, CreateComponentRequest request)
        {
            var component = await _context.Components
                .Include(c => c.System)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (component == null) return null;

            // Verify new system exists if changed
            if (component.SystemId != request.SystemId)
            {
                var newSystem = await _context.Systems.FindAsync(request.SystemId);
                if (newSystem == null)
                    throw new ArgumentException($"System with ID {request.SystemId} not found");
            }

            component.Name = request.Name;
            component.Description = request.Description;
            component.SystemId = request.SystemId;

            await _context.SaveChangesAsync();

            // Reload to get updated system name
            await _context.Entry(component).Reference(c => c.System).LoadAsync();

            return new ComponentDto
            {
                Id = component.Id,
                Name = component.Name,
                Description = component.Description,
                SystemId = component.SystemId,
                SystemName = component.System.Name,
                CreatedAt = component.CreatedAt
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var component = await _context.Components.FindAsync(id);
            if (component == null) return false;

            _context.Components.Remove(component);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
