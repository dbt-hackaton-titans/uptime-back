using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titans.Uptime.Domain.Contracts;
using Titans.Uptime.Domain.Models;

namespace Titans.Uptime.Application.Interfaces
{
    public interface ISystemService
    {
        Task<IEnumerable<SystemDto>> GetAllAsync();
        Task<SystemDto?> GetByIdAsync(int id);
        Task<SystemDto> CreateAsync(CreateSystemRequest request);
        Task<SystemDto?> UpdateAsync(int id, CreateSystemRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
