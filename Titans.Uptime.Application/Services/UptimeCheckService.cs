using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titans.Uptime.Application.Interfaces;
using Titans.Uptime.Domain.Contracts;
using Titans.Uptime.Domain.Models;
using Titans.Uptime.Domain;
using Titans.Uptime.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Titans.Uptime.Application.Services
{
    public class UptimeCheckService : IUptimeCheckService
    {
        private readonly UptimeMonitorContext _context;

        public UptimeCheckService(UptimeMonitorContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UptimeCheckDto>> GetAllAsync()
        {
            List<UptimeCheckDto> list = new List<UptimeCheckDto>();
            UptimeCheckDto dto = new UptimeCheckDto();
            dto.Name = "Check1";

            list.Add(dto);
            dto = new UptimeCheckDto();
            dto.Name = "Check2";
            list.Add(dto);

            return list;
        }

        public async Task<IEnumerable<UptimeCheckDto>> GetActiveAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<UptimeCheckDto?> GetByIdAsync(int id)
        {
            var uptimeCheck = await _context.UptimeChecks
            .Include(u => u.System)
            .Include(u => u.Component)
            .FirstOrDefaultAsync(u => u.Id == id);

            return uptimeCheck == null ? null : MapToDto(uptimeCheck);
        }

        public async Task<UptimeCheckDto> CreateAsync(CreateUptimeCheckRequest request)
        {
            // Verify system exists
            var system = await _context.Systems.FindAsync(request.SystemId);
            if (system == null)
                throw new ArgumentException($"System with ID {request.SystemId} not found");

            // Verify component exists if provided
            Component? component = null;
            if (request.ComponentId.HasValue)
            {
                component = await _context.Components.FindAsync(request.ComponentId.Value);
                if (component == null)
                    throw new ArgumentException($"Component with ID {request.ComponentId} not found");
            }

            var uptimeCheck = new UptimeCheck
            {
                Name = request.Name,
                SystemId = request.SystemId,
                ComponentId = request.ComponentId,
                CheckUrl = request.CheckUrl,
                CheckType = request.CheckType,
                CheckInterval = request.CheckInterval,
                CheckTimeout = request.CheckTimeout,
                RequestHeaders = request.RequestHeaders,
                ResponseStringType = request.ResponseStringType,
                ResponseStringValue = request.ResponseStringValue,
                AlertEmails = request.AlertEmails,
                AlertMessage = request.AlertMessage,
                DownAlertDelay = request.DownAlertDelay,
                DownAlertResend = request.DownAlertResend,
                Status = CheckStatus.Unknown,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.UptimeChecks.Add(uptimeCheck);
            await _context.SaveChangesAsync();

            // Load related entities for DTO
            await _context.Entry(uptimeCheck).Reference(u => u.System).LoadAsync();
            if (uptimeCheck.ComponentId.HasValue)
            {
                await _context.Entry(uptimeCheck).Reference(u => u.Component).LoadAsync();
            }

            return MapToDto(uptimeCheck);
        }

        public async Task<UptimeCheckDto?> UpdateAsync(int id, CreateUptimeCheckRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateStatusAsync(int id, CheckStatus status, string? error = null, double? responseTime = null)
        {
            throw new NotImplementedException();
        }

        private static UptimeCheckDto MapToDto(UptimeCheck uptimeCheck)
        {
            return new UptimeCheckDto
            {
                Id = uptimeCheck.Id,
                Name = uptimeCheck.Name,
                SystemId = uptimeCheck.SystemId,
                SystemName = uptimeCheck.System.Name,
                ComponentId = uptimeCheck.ComponentId,
                ComponentName = uptimeCheck.Component?.Name,
                CheckUrl = uptimeCheck.CheckUrl,
                CheckType = uptimeCheck.CheckType,
                CheckInterval = uptimeCheck.CheckInterval,
                CheckTimeout = uptimeCheck.CheckTimeout,
                RequestHeaders = uptimeCheck.RequestHeaders,
                ResponseStringType = uptimeCheck.ResponseStringType,
                ResponseStringValue = uptimeCheck.ResponseStringValue,
                AlertEmails = uptimeCheck.AlertEmails,
                AlertMessage = uptimeCheck.AlertMessage,
                DownAlertDelay = uptimeCheck.DownAlertDelay,
                DownAlertResend = uptimeCheck.DownAlertResend,
                Status = uptimeCheck.Status,
                LastChecked = uptimeCheck.LastChecked,
                LastStatusChange = uptimeCheck.LastStatusChange,
                LastError = uptimeCheck.LastError,
                LastResponseTime = uptimeCheck.LastResponseTime,
                IsActive = uptimeCheck.IsActive,
                CreatedAt = uptimeCheck.CreatedAt
            };
        }
    }
}
