
using API.Dtos;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TenantsController(ITenantService _tenantService) : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> CreateTenant([FromBody] TenantCreateDto tenantCreateDto)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid Model");

            try
            {
                if (await _tenantService.TenantExistsAsync(tenantCreateDto.Id))
                    return BadRequest($"Tenant '{tenantCreateDto.Name}' already exists.");

                var tenant = await _tenantService.CreateAsync(tenantCreateDto);
                return CreatedAtAction(nameof(GetTenantById), new { tenantId = tenant.Id }, tenant);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var tenants = await _tenantService.GetAllTenantsAsync();
                return Ok(tenants);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("{tenantId}")]
        public async Task<IActionResult> GetTenantById(string tenantId)
        {
            try
            {
                var tenant = await _tenantService.GetTenantByIdAsync(tenantId);

                return Ok(tenant);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{tenantId}")]
        public async Task<IActionResult> DeleteTenant(string tenantId)
        {
            if (string.IsNullOrWhiteSpace(tenantId))
                return BadRequest(new { error = "TenantId is required" });

            try
            {
                var result = await _tenantService.DeleteTenantAsync(tenantId);
                if (!result)
                    return NotFound(new { message = $"Tenant with ID '{tenantId}' not found." });

                return Ok(new { message = $"Tenant '{tenantId}' delete successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}