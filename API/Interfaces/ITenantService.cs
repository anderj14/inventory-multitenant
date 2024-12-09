
using API.Dtos;
using API.Entities;

namespace API.Interfaces
{
    public interface ITenantService
    {
        Task<Tenant> CreateAsync(TenantCreateDto tenantCreateDto);
        Task<ICollection<Tenant>> GetAllTenantsAsync();
        Task<Tenant> GetTenantByIdAsync(string tenantId);
        Task<bool> DeleteTenantAsync(string tenantId);
    }
}