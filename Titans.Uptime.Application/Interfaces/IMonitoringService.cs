using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titans.Uptime.Domain.Models;

namespace Titans.Uptime.Application.Interfaces
{
    public interface IMonitoringService
    {
        Task<CheckResult> PerformCheckAsync(UptimeCheckDto uptimeCheck);
        Task ProcessCheckResultAsync(UptimeCheckDto uptimeCheck, CheckResult result);
    }
}
