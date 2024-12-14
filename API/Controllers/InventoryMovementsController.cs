using API.Dtos;
using API.Entities;
using API.Extensions;
using API.Helpers.Mapping;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class InventoryMovementsController(UserManager<AppUser> _userManager, IGenericRepository<InventoryMovement> _inventoryMovRepo, IGenericRepository<Product> _productRepo) : BaseApiController
    {
        protected async Task<AppUser> GetAuthenticatedUserAsync()
        {
            var email = User.GetEmail();

            if (string.IsNullOrEmpty(email))
            {
                return null;
            }

            var tenantHeader = GetTenantHeader();

            return await _userManager.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email && u.TenantId == tenantHeader);
        }

        private string GetTenantHeader()
        {
            var tenantHeader = Request.Headers["tenant"].ToString();
            if (string.IsNullOrEmpty(tenantHeader))
                throw new UnauthorizedAccessException("Tenant header is missing");

            return tenantHeader;
        }


        [HttpPost]
        public async Task<ActionResult<InventoryMovementDto>> CreateInventoryMovement([FromBody] CreateInventoryMovementDto inventoryMovementDto)
        {
            if (inventoryMovementDto == null)
                return BadRequest(new { Errors = new[] { "Invalid product data" } });

            try
            {
                var user = await GetAuthenticatedUserAsync();

                if (user == null)
                    return Unauthorized(new { Errors = new[] { "Unauthorized access" } });

                var product = await _productRepo.GetByIdAsync(inventoryMovementDto.ProductId);

                var tenantHeader = GetTenantHeader();

                if (product == null || product.TenantId != tenantHeader)
                {
                    return NotFound(new { Errors = new[] { "Product not found or does not belong to the tenant" } });
                }

                if (inventoryMovementDto.MovementType == "Output" && inventoryMovementDto.Quantity > product.Quantity)
                {
                    return BadRequest(new { Errors = new[] { "Insufficient stock available for the requested movement" } });
                }

                var inventoryMovement = inventoryMovementDto.ToEntity(tenantHeader);
                inventoryMovement.AppUserId = user.Id;

                if (inventoryMovementDto.MovementType == "Output")
                {
                    product.Quantity -= inventoryMovement.Quantity;
                }

                if (inventoryMovementDto.MovementType == "Input")
                {
                    product.Quantity += inventoryMovement.Quantity;
                }


                await _inventoryMovRepo.CreateAsync(inventoryMovement);

                var createdInventoryMovement = inventoryMovement.ToDto();

                return CreatedAtAction(nameof(GetInventoryMovement), new { id = inventoryMovement.Id }, createdInventoryMovement);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Errors = new[] { ex.Message } });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Errors = new[] { "An unexpected error occurred.", ex.Message } });
            }

        }

        [HttpGet]
        public async Task<ActionResult<InventoryMovementDto>> GetInventoryMovements()
        {
            var inventoryMovements = await _inventoryMovRepo.GetAllAsync();

            if (inventoryMovements == null || inventoryMovements.Count == 0) return NotFound("No products found.");

            return Ok(inventoryMovements.Select(x => x.ToDto()).ToList());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<InventoryMovementDto>> GetInventoryMovement(int id)
        {
            var inventoryMovement = await _inventoryMovRepo.GetByIdAsync(id);

            if (inventoryMovement == null) return NotFound("No product found.");

            return Ok(inventoryMovement.ToDto());
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<InventoryMovementDto>> UpdateInventoryMovement(int id, [FromBody] CreateInventoryMovementDto inventoryMovementDto)
        {
            if (inventoryMovementDto == null)
                return BadRequest(new { Errors = new[] { "Invalid data provided" } });

            try
            {
                var user = await GetAuthenticatedUserAsync();

                if (user == null)
                    return Unauthorized(new { Errors = new[] { "Unauthorized access" } });

                var tenantHeader = GetTenantHeader();

                var existingInventoryMovement = await _inventoryMovRepo.GetByIdAsync(id);

                if (existingInventoryMovement == null || existingInventoryMovement.TenantId != tenantHeader)
                {
                    return NotFound(new { Errors = new[] { "Inventory movement not found or does not belong to the tenant" } });
                }

                var product = await _productRepo.GetByIdAsync(existingInventoryMovement.ProductId);

                if (product == null || product.TenantId != tenantHeader)
                {
                    return NotFound(new { Errors = new[] { "Product not found or does not belong to the tenant" } });
                }

                if (inventoryMovementDto.MovementType == "Output")
                {
                    product.Quantity += existingInventoryMovement.Quantity;
                }

                if (inventoryMovementDto.MovementType == "Input")
                {
                    product.Quantity -= existingInventoryMovement.Quantity;
                }

                if (inventoryMovementDto.MovementType == "Output" && inventoryMovementDto.Quantity > product.Quantity)
                {
                    return BadRequest(new { Errors = new[] { "Insufficient stock available for the requested movement" } });
                }

                existingInventoryMovement.UpdateFromDto(inventoryMovementDto);

                if (inventoryMovementDto.MovementType == "Output")
                {
                    product.Quantity -= inventoryMovementDto.Quantity;
                }
                else if (inventoryMovementDto.MovementType == "Input")
                {
                    product.Quantity += inventoryMovementDto.Quantity;
                }

                await _inventoryMovRepo.UpdateAsync(existingInventoryMovement);

                var updatedInventoryMovement = existingInventoryMovement.ToDto();

                return Ok(updatedInventoryMovement);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Errors = new[] { ex.Message } });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Errors = new[] { "An unexpected error occurred.", ex.Message } });
            }
        }
    }
}