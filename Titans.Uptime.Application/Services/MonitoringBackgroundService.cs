using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titans.Uptime.Application.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace Titans.Uptime.Application.Services
{
    public class MonitoringBackgroundService : BackgroundService
    {
        private readonly IMonitoringService _monitoringService;
        private readonly IUptimeCheckService _uptimeCheckService;
        private readonly int _monitoringIntervalSeconds;

        public MonitoringBackgroundService(
            IMonitoringService monitoringService,
            IUptimeCheckService uptimeCheckService,
            IConfiguration configuration)
        {
            _monitoringService = monitoringService;
            _uptimeCheckService = uptimeCheckService;
            _monitoringIntervalSeconds = configuration.GetValue<int>("Monitoring:IntervalSeconds", 60);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var uptimeChecks = await _uptimeCheckService.GetAllAsync();

                    var tasks = uptimeChecks
                        .Where(c => c.IsActive) // O la condición que prefieras
                        .Select(async check =>
                        {
                            var result = await _monitoringService.PerformCheckAsync(check);
                            await _monitoringService.ProcessCheckResultAsync(check, result);
                        });

                    await Task.WhenAll(tasks);
                }
                catch (Exception ex)
                {
                    // Loguea el error; nunca permitas que una excepción detenga el loop.
                    Console.WriteLine($"[MonitoringBackgroundService] Error: {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromSeconds(_monitoringIntervalSeconds), stoppingToken);
            }
        }
    }
}
