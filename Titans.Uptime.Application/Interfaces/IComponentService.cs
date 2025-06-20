using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titans.Uptime.Domain.Contracts;
using Titans.Uptime.Domain.Models;

namespace Titans.Uptime.Application.Interfaces
{
    public interface IComponentService
    {
        Task<IEnumerable<ComponentDto>> GetAllAsync();
        Task<IEnumerable<ComponentDto>> GetBySystemIdAsync(int systemId);
        Task<ComponentDto?> GetByIdAsync(int id);
        Task<ComponentDto> CreateAsync(CreateComponentRequest request);
        Task<ComponentDto?> UpdateAsync(int id, CreateComponentRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
