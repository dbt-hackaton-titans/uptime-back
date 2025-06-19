using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titans.Uptime.Application.Interfaces;
using Titans.Uptime.Domain.Contracts;
using Titans.Uptime.Domain.Models;
using Titans.Uptime.Persistence;

namespace Titans.Uptime.Application.Services
{
    public class SystemService : ISystemService
    {
        private readonly UptimeMonitorContext _context;

        public SystemService(UptimeMonitorContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<SystemDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
        public Task<SystemDto?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<SystemDto> CreateAsync(CreateSystemRequest request)
        {
            throw new NotImplementedException();
        }
        public Task<SystemDto?> UpdateAsync(int id, CreateSystemRequest request)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
