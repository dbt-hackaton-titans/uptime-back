using System.ComponentModel.DataAnnotations;

namespace Titans.Uptime.Domain.Contracts
{
    public class CreateUptimeCheckRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public int SystemId { get; set; }
        public int? ComponentId { get; set; }
        [Required]
        public string CheckUrl { get; set; } = string.Empty;
        public CheckType CheckType { get; set; }
        [Range(1, 60)]
        public int CheckInterval { get; set; } = 5;
        [Range(0.01, 30)]
        public double CheckTimeout { get; set; } = 10.0;
        public string? RequestHeaders { get; set; }
        public ResponseStringType? ResponseStringType { get; set; }
        public string? ResponseStringValue { get; set; }
        [Required]
        public string AlertEmails { get; set; } = string.Empty;
        public string AlertMessage { get; set; } = string.Empty;
        [Range(0, 60)]
        public int DownAlertDelay { get; set; } = 0;
        [Range(0, 60)]
        public int DownAlertResend { get; set; } = 0;
    }
}
