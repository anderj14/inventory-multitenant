using API.Interfaces;

namespace API.Middleware
{
    public class TenantResolver(RequestDelegate _next)
    {
        public async Task InvokeAsync(HttpContext context, ITenantContextService tenantContextService)
        {
            context.Request.Headers.TryGetValue("tenant", out var tenantFromHeader);

            if (string.IsNullOrEmpty(tenantFromHeader) == false)
            {
                await tenantContextService.SetTenantAsync(tenantFromHeader);
            }

            await _next(context);
        }
    }
}