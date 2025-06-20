using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titans.Uptime.Domain.Models;
using Titans.Uptime.Domain;

namespace Titans.Uptime.Application.Interfaces
{
    public interface IUptimeEventService
    {
        Task<IEnumerable<UptimeEventDto>> GetByCheckIdAsync(int uptimeCheckId);
        Task<UptimeEventDto?> GetByIdAsync(int id);
        Task<UptimeEventDto> CreateAsync(UptimeEventDto dto);
        Task<IEnumerable<UptimeEventDto>> GetAllAsync();
        Task MarkAsFalsePositiveAsync(int eventId, bool isFalsePositive);
        Task CategorizeEventAsync(int eventId, EventCategory category);
    }
}
