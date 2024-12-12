using API.Dtos;
using API.Entities;
using API.Helpers.Mapping;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BrandsController(IGenericRepository<Brand> _brandRepo) : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult<BrandDto>> CreateBrand(BrandDto brandDto)
        {
            if (brandDto == null)
            {
                return BadRequest("Invalid category data");
            }

            var tenantHeader = Request.Headers["tenant"].ToString();
            var brand = brandDto.ToEntity(tenantHeader);

            await _brandRepo.CreateAsync(brand);

            var createdBrandDto = brand.ToDto();

            return CreatedAtAction(nameof(GetBrand), new { id = brand.Id }, createdBrandDto);
        }

        [HttpGet]
        public async Task<ActionResult<BrandDto>> GetBrands()
        {
            var brands = await _brandRepo.GetAllAsync();

            if (brands == null || brands.Count == 0) return NotFound("No brands found.");

            return Ok(brands.Select(x => x.ToDto()).ToList());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BrandDto>> GetBrand(int id)
        {
            var brand = await _brandRepo.GetByIdAsync(id);

            if (brand == null) return NotFound("No category found.");

            return Ok(brand.ToDto());
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateBrand(int id, [FromBody] BrandDto brandDto)
        {
            if (brandDto == null) return BadRequest("Brand data is required.");

            var brand = await _brandRepo.GetByIdAsync(id);

            if (brand == null) return NotFound($"Brand with ID {id} not found.");

            brand.UpdateFromDto(brandDto);

            await _brandRepo.UpdateAsync(brand);

            return Ok(brand.ToDto());
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteBrand(int id)
        {
            var brand = await _brandRepo.GetByIdAsync(id);

            if (brand == null) return NotFound($"Brand with ID {id} not found.");

            await _brandRepo.DeleteAsync(id);

            return NoContent();
        }
    }
}