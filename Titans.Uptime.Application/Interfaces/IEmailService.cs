using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titans.Uptime.Domain.Models;

namespace Titans.Uptime.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendAlertAsync(string[] recipients, string subject, string message);
        Task SendDownAlertAsync(UptimeCheck uptimeCheck, UptimeEvent downEvent);
        Task SendUpAlertAsync(UptimeCheck uptimeCheck, UptimeEvent upEvent);
    }
}
