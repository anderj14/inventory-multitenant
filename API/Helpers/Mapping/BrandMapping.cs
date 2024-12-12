
using API.Dtos;
using API.Entities;

namespace API.Helpers.Mapping
{
    public static class BrandMapping
    {
        public static BrandDto? ToDto(this Brand brand)
        {
            if (brand == null) return null;

            return new BrandDto
            {
                Id = brand.Id,
                BrandName = brand.BrandName,
                BrandDescription = brand.BrandDescription
            };
        }

        public static Brand ToEntity(this BrandDto? brandDto, string tenantId)
        {
            if (brandDto == null) throw new ArgumentException(nameof(brandDto));

            return new Brand
            {
                BrandName = brandDto.BrandName,
                BrandDescription = brandDto.BrandDescription,
                TenantId = tenantId
            };
        }

        public static void UpdateFromDto(this Brand brand, BrandDto brandDto)
        {
            if (brandDto == null) throw new ArgumentException(nameof(brandDto));
            if (brand == null) throw new ArgumentException(nameof(brand));

            brand.BrandName = brandDto.BrandName;
            brand.BrandDescription = brandDto.BrandDescription;
        }
    }
}