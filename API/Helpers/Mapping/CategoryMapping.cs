
using API.Dtos;
using API.Entities;

namespace API.Helpers.Mapping
{
    public static class CategoryMapping
    {
        public static CategoryDto? ToDto(this Category? category)
        {
            if (category == null) return null;

            return new CategoryDto
            {
                Id = category.Id,
                CategoryName = category.CategoryName,
                CategoryDescription = category.CategoryDescription,
            };
        }

        public static Category ToEntity(this CategoryDto? categoryDto, string tenantId)
        {

            if (categoryDto == null) throw new ArgumentException(nameof(categoryDto));

            return new Category
            {
                CategoryName = categoryDto.CategoryName,
                CategoryDescription = categoryDto.CategoryDescription,
                TenantId = tenantId
            };
        }

        public static void UpdateFromDto(this Category category, CategoryDto categoryDto)
        {
            if (categoryDto == null) throw new ArgumentException(nameof(categoryDto));
            if (category == null) throw new ArgumentException(nameof(category));

            category.CategoryName = categoryDto.CategoryName;
            category.CategoryDescription = categoryDto.CategoryDescription;
        }
    }
}