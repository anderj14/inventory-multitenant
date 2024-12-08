
using API.Data;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class TenantContextService(TenantDbcontext _context) : ITenantContextService
    {
        public string? TenantId { get; set; }
        public string? ConnectionString { get; set; }

        public async Task<bool> SetTenantAsync(string tenant)
        {
            var tenantInfo = await _context.Tenants.Where(x => x.Id == tenant).FirstOrDefaultAsync();

            if (tenantInfo != null)
            {
                TenantId = tenantInfo.Id;
                ConnectionString = tenantInfo.ConnectionString;

                return true;
            }

            return false;
        }
    }
}