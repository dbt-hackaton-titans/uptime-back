using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Titans.Uptime.Domain.Models
{
    public class UptimeEvent
    {
        public int Id { get; set; }

        public int UptimeCheckId { get; set; }
        public virtual UptimeCheck UptimeCheck { get; set; } = null!;

        public EventType EventType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TimeSpan? Duration => EndTime?.Subtract(StartTime);

        public string? ErrorMessage { get; set; }
        public double? ResponseTime { get; set; }

        // Manual fields for PSE to fill
        public bool IsFalsePositive { get; set; } = false;
        public EventCategory? Category { get; set; }
        public MaintenanceType? MaintenanceType { get; set; }
        public string? Notes { get; set; }
        public string? TicketCode { get; set; }

        public bool IsResolved { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
