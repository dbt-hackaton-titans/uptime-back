using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Titans.Uptime.Domain.Models
{
    public class UptimeCheck
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public int SystemId { get; set; }
        public virtual SystemEntity System { get; set; } = null!;

        public int? ComponentId { get; set; }
        public virtual Component? Component { get; set; }

        [Required]
        [MaxLength(500)]
        public string CheckUrl { get; set; } = string.Empty; // IP for Ping, URL for HTTPS

        public CheckType CheckType { get; set; }

        // Intervals in minutes: 1, 5, 15, 30, 60
        public int CheckInterval { get; set; } = 5;

        // Timeout in seconds: 0.01 to 30
        public double CheckTimeout { get; set; } = 10.0;

        // HTTP specific fields
        public string? RequestHeaders { get; set; } // One header per line
        public ResponseStringType? ResponseStringType { get; set; }
        public string? ResponseStringValue { get; set; }

        // Alert configuration
        public string AlertEmails { get; set; } = string.Empty; // One email per line
        public string AlertMessage { get; set; } = string.Empty;
        public int DownAlertDelay { get; set; } = 0; // Minutes to wait before alerting
        public int DownAlertResend { get; set; } = 0; // Number of cycles to resend alert

        // Status tracking
        public CheckStatus Status { get; set; } = CheckStatus.Unknown;
        public DateTime? LastChecked { get; set; }
        public DateTime? LastStatusChange { get; set; }
        public string? LastError { get; set; }
        public double? LastResponseTime { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<UptimeEvent> Events { get; set; } = new List<UptimeEvent>();
    }
}
