using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titans.Uptime.Application.Interfaces;
using Titans.Uptime.Domain.Models;
using Titans.Uptime.Domain;
using System.Net.Http;

namespace Titans.Uptime.Application.Services
{
    public class MonitoringService : IMonitoringService
    {
        private readonly IUptimeEventService _uptimeEventService;
        private readonly IEmailService _emailService;
        private readonly IHttpClientFactory _httpClientFactory;

        public MonitoringService(
            IUptimeEventService uptimeEventService,
            IEmailService emailService,
            IHttpClientFactory httpClientFactory)
        {
            _uptimeEventService = uptimeEventService;
            _emailService = emailService;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<CheckResult> PerformCheckAsync(UptimeCheckDto uptimeCheck)
        {
            var result = new CheckResult();

            try
            {
                var client = _httpClientFactory.CreateClient();
                var request = new HttpRequestMessage(HttpMethod.Get, uptimeCheck.CheckUrl);
                var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                var response = await client.SendAsync(request);
                stopwatch.Stop();

                result.IsUp = response.IsSuccessStatusCode;
                result.ResponseTime = stopwatch.Elapsed.TotalMilliseconds;
                result.Error = result.IsUp ? null : $"Status: {response.StatusCode}";
                result.CheckTime = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                result.IsUp = false;
                result.Error = ex.Message;
                result.ResponseTime = 0;
                result.CheckTime = DateTime.UtcNow;
            }

            return result;
        }

        public async Task ProcessCheckResultAsync(UptimeCheckDto uptimeCheck, CheckResult result)
        {
            var eventType = result.IsUp ? EventType.Up : EventType.Down;

            var eventDto = new UptimeEventDto
            {
                UptimeCheckId = uptimeCheck.Id,
                EventType = eventType,
                StartTime = result.CheckTime,
                ResponseTime = result.ResponseTime,
                ErrorMessage = result.Error
            };

            await _uptimeEventService.CreateAsync(eventDto);

            if (eventType == EventType.Down)
            {
                var downEvent = new UptimeEvent
                {
                    UptimeCheckId = uptimeCheck.Id,
                    EventType = EventType.Down,
                    StartTime = result.CheckTime,
                    ErrorMessage = result.Error,
                    ResponseTime = result.ResponseTime
                };
                await _emailService.SendDownAlertAsync(uptimeCheck, downEvent);
            }
            else if (eventType == EventType.Up)
            {
                var upEvent = new UptimeEvent
                {
                    UptimeCheckId = uptimeCheck.Id,
                    EventType = EventType.Up,
                    StartTime = result.CheckTime,
                    ResponseTime = result.ResponseTime
                };
                await _emailService.SendUpAlertAsync(uptimeCheck, upEvent);
            }
        }
    }
}
