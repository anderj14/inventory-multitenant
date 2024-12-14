
using API.Data;
using API.Dtos;
using API.Entities;
using API.Helpers.Mapping;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class WarehouseController(IGenericRepository<Warehouse> _warehouseRepo, TenantDbcontext _context) : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult<WarehouseDto>> CreateWarehouse([FromBody] WarehouseDto warehouseDto)
        {
            if (warehouseDto == null) return BadRequest("Invalid warehouse data");

            var tenantHeader = Request.Headers["tenant"].ToString();

            var tenantExists = await _context.Tenants
                 .AsNoTracking()
                 .AnyAsync(t => t.Id == tenantHeader);

            if (!tenantExists)
            {
                return BadRequest(new { Errors = new[] { $"Tenant '{tenantHeader}' does not exist" } });
            }

            var warehouse = warehouseDto.ToEntity(tenantHeader);

            await _warehouseRepo.CreateAsync(warehouse);

            var createdWarehouse = warehouse.ToDto();

            return CreatedAtAction(nameof(GetWarehouse), new { id = warehouse.Id }, createdWarehouse);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<WarehouseDto>>> GetWarehouses()
        {
            var warehouses = await _warehouseRepo.GetAllAsync();

            if (warehouses == null || warehouses.Count == 0) return NotFound("No warehouses found.");

            return Ok(warehouses.Select(x => x.ToDto()).ToList());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<WarehouseDto>> GetWarehouse(int id)
        {
            var warehouse = await _warehouseRepo.GetByIdAsync(id);

            if (warehouse == null) return NotFound("No warehouse found.");

            return Ok(warehouse.ToDto());
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<WarehouseDto>> UpdateWarehouse(int id, [FromBody] WarehouseDto warehouseDto)
        {
            var warehouse = await _warehouseRepo.GetByIdAsync(id);

            if (warehouse == null) return NotFound("No warehouse found.");

            warehouse.UpdateFromDto(warehouseDto);

            await _warehouseRepo.UpdateAsync(warehouse);

            return Ok(warehouse.ToDto());
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<WarehouseDto>> DeleteWarehouse(int id)
        {
            var warehouse = await _warehouseRepo.GetByIdAsync(id);

            if (warehouse == null) return NotFound("No warehouse found.");

            await _warehouseRepo.DeleteAsync(id);

            return NoContent();
        }
    }
}