using Titans.Uptime.Application.Interfaces;
using Titans.Uptime.Domain.Models;
using Titans.Uptime.Domain;
using Titans.Uptime.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Titans.Uptime.Application.Services
{
    public class UptimeEventService : IUptimeEventService
    {
        private readonly UptimeMonitorContext _context;

        public UptimeEventService(UptimeMonitorContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UptimeEventDto>> GetByCheckIdAsync(int uptimeCheckId)
        {
            return await _context.UptimeEvents
                .Where(e => e.UptimeCheckId == uptimeCheckId)
                .OrderByDescending(e => e.StartTime)
                .Select(e => ToDto(e))
                .ToListAsync();
        }

        public async Task<UptimeEventDto?> GetByIdAsync(int id)
        {
            var entity = await _context.UptimeEvents
                .Include(e => e.UptimeCheck)
                .FirstOrDefaultAsync(e => e.Id == id);

            return entity == null ? null : ToDto(entity);
        }

        public async Task<UptimeEventDto> CreateAsync(UptimeEventDto dto)
        {
            var entity = FromDto(dto);
            _context.UptimeEvents.Add(entity);
            await _context.SaveChangesAsync();
            return ToDto(entity);
        }

        public async Task<IEnumerable<UptimeEventDto>> GetAllAsync()
        {
            return await _context.UptimeEvents
                .Include(e => e.UptimeCheck)
                .OrderByDescending(e => e.StartTime)
                .Select(e => ToDto(e))
                .ToListAsync();
        }

        public async Task MarkAsFalsePositiveAsync(int eventId, bool isFalsePositive)
        {
            var ev = await _context.UptimeEvents.FindAsync(eventId);
            if (ev != null)
            {
                ev.IsFalsePositive = isFalsePositive;
                await _context.SaveChangesAsync();
            }
        }

        public async Task CategorizeEventAsync(int eventId, EventCategory category)
        {
            var ev = await _context.UptimeEvents.FindAsync(eventId);
            if (ev != null)
            {
                ev.Category = category;
                await _context.SaveChangesAsync();
            }
        }

        // Conversión de entidad a DTO
        private static UptimeEventDto ToDto(UptimeEvent e) => new UptimeEventDto
        {
            Id = e.Id,
            UptimeCheckId = e.UptimeCheckId,
            EventType = e.EventType,
            StartTime = e.StartTime,
            EndTime = e.EndTime,
            ErrorMessage = e.ErrorMessage,
            ResponseTime = e.ResponseTime,
            IsFalsePositive = e.IsFalsePositive,
            Category = e.Category,
            MaintenanceType = e.MaintenanceType,
            Notes = e.Notes,
            Duration = e.Duration
        };

        // Conversión de DTO a entidad
        private static UptimeEvent FromDto(UptimeEventDto dto) => new UptimeEvent
        {
            Id = dto.Id,
            UptimeCheckId = dto.UptimeCheckId,
            EventType = dto.EventType,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            ErrorMessage = dto.ErrorMessage,
            ResponseTime = dto.ResponseTime,
            IsFalsePositive = dto.IsFalsePositive,
            Category = dto.Category,
            MaintenanceType = dto.MaintenanceType,
            Notes = dto.Notes
        };
    }
}
