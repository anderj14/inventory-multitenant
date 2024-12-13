
using API.Dtos;
using API.Entities;
using API.Helpers.Mapping;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class SuppliersController(IGenericRepository<Supplier> _supplierRepo) : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult<SupplierDto>> CreateSupplier([FromBody] SupplierDto supplierDto)
        {
            if (supplierDto == null) return BadRequest("Invalid supplier data");

            var tenantHeader = Request.Headers["tenant"].ToString();
            var supplier = supplierDto.ToEntity(tenantHeader);

            await _supplierRepo.CreateAsync(supplier);

            var createdSupplierDto = supplier.ToDto();

            return CreatedAtAction(nameof(GetSupplier), new { id = supplier.Id }, createdSupplierDto);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<SupplierDto>>> GetSuppliers()
        {
            var suppliers = await _supplierRepo.GetAllAsync();

            if (suppliers == null || suppliers.Count == 0)
            {
                return NotFound("No suppliers found.");
            }

            var suppliersDto = suppliers.Select(x => x.ToDto()).ToList();

            return Ok(suppliersDto);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SupplierDto>> GetSupplier(int id)
        {
            var supplier = await _supplierRepo.GetByIdAsync(id);

            if (supplier == null)
            {
                return NotFound("No supplier found.");
            }

            return Ok(supplier.ToDto());
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateSupplier(int id, [FromBody] SupplierDto supplierDto)
        {
            if (supplierDto == null) return BadRequest("Supplier data is required.");

            var supplier = await _supplierRepo.GetByIdAsync(id);

            if (supplier == null) return NotFound($"Supplier with ID {id} not found.");

            supplier.UpdateFromDto(supplierDto);

            await _supplierRepo.UpdateAsync(supplier);

            return Ok(supplier.ToDto());
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteSupplier(int id)
        {
            var supplier = await _supplierRepo.GetByIdAsync(id);

            if (supplier == null) return NotFound($"Supplier with ID {id} not found.");

            await _supplierRepo.DeleteAsync(id);

            return NoContent();
        }
    }
}