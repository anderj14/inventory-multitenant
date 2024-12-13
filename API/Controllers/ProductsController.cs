
using API.Dtos;
using API.Entities;
using API.Helpers.Mapping;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class ProductsController(IGenericRepository<Product> _productRepo) : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] ProductDto productDto)
        {
            if (productDto == null) return BadRequest("Invalid product data");

            var tenantHeader = Request.Headers["tenant"].ToString();
            var product = productDto.ToEntity(tenantHeader);

            await _productRepo.CreateAsync(product);

            var createdProduct = product.ToDto();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, createdProduct);
        }

        [HttpGet]
        public async Task<ActionResult<ProductDto>> GetProducts()
        {
            var products = await _productRepo.GetAllAsync();
            if (products == null || products.Count == 0) return NotFound("No products found.");

            return Ok(products.Select(x => x.ToDto()).ToList());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var products = await _productRepo.GetByIdAsync(id);
            if (products == null) return NotFound("No product found.");

            return Ok(products.ToDto());
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] ProductDto productDto)
        {
            if (productDto == null) return BadRequest("Product data is required.");

            var product = await _productRepo.GetByIdAsync(id);
            if (product == null) return NotFound("No product found.");

            product.UpdateFromDto(productDto);

            await _productRepo.UpdateAsync(product);

            return Ok(product.ToDto());
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);

            if (product == null) return NotFound($"Product with ID {id} not found.");

            await _productRepo.DeleteAsync(id);

            return NoContent();
        }

    }
}