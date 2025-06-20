using Titans.Uptime.Domain.Models;
using Titans.Uptime.Domain;
using Titans.Uptime.Domain.Contracts;

namespace Titans.Uptime.Application.Interfaces
{
    public interface IUptimeCheckService
    {
        Task<IEnumerable<UptimeCheckDto>> GetAllAsync();
        Task<IEnumerable<UptimeCheckDto>> GetActiveAsync();
        Task<UptimeCheckDto?> GetByIdAsync(int id);
        Task<UptimeCheckDto> CreateAsync(CreateUptimeCheckRequest request);
        Task<UptimeCheckDto?> UpdateAsync(int id, CreateUptimeCheckRequest request);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateStatusAsync(int id, CheckStatus status, string? error = null, double? responseTime = null);
    }
}
