using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titans.Uptime.Application.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Titans.Uptime.Application.Services
{
    public class MonitoringBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly int _monitoringIntervalSeconds;

        public MonitoringBackgroundService(
            IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _monitoringIntervalSeconds = configuration.GetValue<int>("Monitoring:IntervalSeconds", 60);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var monitoringService = scope.ServiceProvider.GetRequiredService<IMonitoringService>();
                        var uptimeCheckService = scope.ServiceProvider.GetRequiredService<IUptimeCheckService>();

                        var uptimeChecks = await uptimeCheckService.GetAllAsync();

                        var tasks = uptimeChecks
                            .Where(c => c.IsActive) // O la condición que prefieras
                            .Select(async check =>
                            {
                                var result = await monitoringService.PerformCheckAsync(check);
                                await monitoringService.ProcessCheckResultAsync(check, result);
                            });

                        await Task.WhenAll(tasks);
                    }
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
