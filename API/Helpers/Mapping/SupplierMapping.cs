
using API.Dtos;
using API.Entities;

namespace API.Helpers.Mapping
{
    public static class SupplierMapping
    {
        public static SupplierDto? ToDto(this Supplier? supplier)
        {
            if (supplier == null) return null;

            return new SupplierDto
            {
                Id = supplier.Id,
                Name = supplier.Name,
                ContactInfo = supplier.ContactInfo,
                Phone = supplier.Phone,
                Address = supplier.Address,
            };
        }

        public static Supplier ToEntity(this SupplierDto? supplierDto, string tenantId)
        {
            if (supplierDto == null) throw new ArgumentException(nameof(supplierDto));

            return new Supplier
            {
                Id = supplierDto.Id,
                Name = supplierDto.Name,
                ContactInfo = supplierDto.ContactInfo,
                Phone = supplierDto.Phone,
                Address = supplierDto.Address,
                TenantId = tenantId
            };
        }

        public static void UpdateFromDto(this Supplier supplier, SupplierDto supplierDto)
        {
            if (supplierDto == null) throw new ArgumentException(nameof(supplierDto));
            if (supplier == null) throw new ArgumentException(nameof(supplier));

            supplier.Name = supplierDto.Name;
            supplier.ContactInfo = supplierDto.ContactInfo;
            supplier.Phone = supplierDto.Phone;
            supplier.Address = supplierDto.Address;
        }
    }
}