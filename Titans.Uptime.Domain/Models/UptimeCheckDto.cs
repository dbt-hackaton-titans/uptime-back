using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Titans.Uptime.Domain.Models
{
    public class UptimeCheckDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int SystemId { get; set; }
        public string SystemName { get; set; } = string.Empty;
        public int? ComponentId { get; set; }
        public string? ComponentName { get; set; }
        public string CheckUrl { get; set; } = string.Empty;
        public CheckType CheckType { get; set; }
        public int CheckInterval { get; set; }
        public double CheckTimeout { get; set; }
        public string? RequestHeaders { get; set; }
        public ResponseStringType? ResponseStringType { get; set; }
        public string? ResponseStringValue { get; set; }
        public string AlertEmails { get; set; } = string.Empty;
        public string AlertMessage { get; set; } = string.Empty;
        public int DownAlertDelay { get; set; }
        public int DownAlertResend { get; set; }
        public CheckStatus Status { get; set; }
        public DateTime? LastChecked { get; set; }
        public DateTime? LastStatusChange { get; set; }
        public string? LastError { get; set; }
        public double? LastResponseTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
