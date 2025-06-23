using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Titans.Uptime.Domain.Models
{
    public class CheckResult
    {
        public bool IsUp { get; set; }
        public double ResponseTime { get; set; }
        public string? Error { get; set; }
        public DateTime CheckTime { get; set; } = DateTime.UtcNow;
    }
}
