using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Titans.Uptime.Domain.Models
{
    public class UptimeEventDto
    {
        public int Id { get; set; }
        public int UptimeCheckId { get; set; }
        public string UptimeCheckName { get; set; } = string.Empty;
        public string SystemName { get; set; } = string.Empty;
        public string? ComponentName { get; set; }
        public EventType EventType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TimeSpan? Duration { get; set; }
        public string? ErrorMessage { get; set; }
        public double? ResponseTime { get; set; }
        public bool IsFalsePositive { get; set; }
        public EventCategory? Category { get; set; }
        public MaintenanceType? MaintenanceType { get; set; }
        public string? Notes { get; set; }
        public string? TicketCode { get; set; }
        public bool IsResolved { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
