

using API.Dtos;
using API.Dtos.Creates;
using API.Entities;
using API.Helpers.Mapping;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CategoriesController(IGenericRepository<Category> _categoryRepo) : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> CreateCategory(CategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                return BadRequest("Invalid category data");
            }
            var tenantHeader = Request.Headers["tenant"].ToString();
            var category = categoryDto.ToEntity(tenantHeader);

            await _categoryRepo.CreateAsync(category);

            var createdCategoryDto = category.ToDto();

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, createdCategoryDto);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CategoryDto>>> GetCategories()
        {
            var categories = await _categoryRepo.GetAllAsync();

            if (categories == null || categories.Count == 0)
            {
                return NotFound("No categories found.");
            }

            var categoryDtos = categories.Select(x => x.ToDto()).ToList();

            return Ok(categoryDtos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            var categoryDto = category.ToDto();

            return Ok(categoryDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateCategory(int id, [FromBody] CategoryDto categoryDto)
        {

            if (categoryDto == null)
            {
                return BadRequest("Category data is required.");
            }

            var category = await _categoryRepo.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            category.UpdateFromDto(categoryDto);

            await _categoryRepo.UpdateAsync(category);

            var updatedCategoryDto = category.ToDto();

            return Ok(updatedCategoryDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            await _categoryRepo.DeleteAsync(id);

            return NoContent();
        }
    }
}