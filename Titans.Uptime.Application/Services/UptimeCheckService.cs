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
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UptimeCheckDto>> GetActiveAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<UptimeCheckDto?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<UptimeCheckDto> CreateAsync(CreateUptimeCheckRequest request)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
