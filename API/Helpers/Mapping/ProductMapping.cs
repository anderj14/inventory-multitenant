
using API.Dtos;
using API.Entities;

namespace API.Helpers.Mapping
{
    public static class ProductMapping
    {
        public static ProductDto? ToDto(this Product product)
        {
            if (product == null) return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                SKU = product.SKU,
                Quantity = product.Quantity,
                UnitPrice = product.UnitPrice,
                IsActive = product.IsActive,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId,
                BrandId = product.BrandId,
                SupplierId = product.SupplierId
            };
        }
        public static Product ToEntity(this ProductDto productDto, string tenantId)
        {
            if (productDto == null) throw new ArgumentException(nameof(productDto));

            return new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                SKU = productDto.SKU,
                Quantity = productDto.Quantity,
                UnitPrice = productDto.UnitPrice,
                IsActive = productDto.IsActive,
                ImageUrl = productDto.ImageUrl,
                CategoryId = productDto.CategoryId,
                BrandId = productDto.BrandId,
                SupplierId = productDto.SupplierId,
                TenantId = tenantId
            };
        }

        public static void UpdateFromDto(this Product product, ProductDto productDto)
        {
            if (productDto == null) throw new ArgumentException(nameof(productDto));
            if (product == null) throw new ArgumentException(nameof(product));

            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.SKU = productDto.SKU;
            product.Quantity = productDto.Quantity;
            product.UnitPrice = productDto.UnitPrice;
            product.IsActive = productDto.IsActive;
            product.ImageUrl = productDto.ImageUrl;
            product.CategoryId = productDto.CategoryId;
            product.BrandId = productDto.BrandId;
            product.SupplierId = productDto.SupplierId;
        }
    }
}