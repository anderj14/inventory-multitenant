
using API.Dtos;
using API.Entities;

namespace API.Helpers.Mapping
{
    public static class WarehouseMapping
    {
        public static WarehouseDto? ToDto(this Warehouse warehouse)
        {
            if (warehouse == null) return null;

            return new WarehouseDto
            {
                Id = warehouse.Id,
                Name = warehouse.Name,
                Address = warehouse.Address
            };
        }

        public static Warehouse ToEntity(this WarehouseDto warehouseDto, string tenantId)
        {
            if (warehouseDto == null) throw new ArgumentException(nameof(warehouseDto));

            return new Warehouse
            {
                Name = warehouseDto.Name,
                Address = warehouseDto.Address,
                TenantId = tenantId
            };
        }

        public static void UpdateFromDto(this Warehouse warehouse, WarehouseDto warehouseDto)
        {
            if (warehouseDto == null) throw new ArgumentException(nameof(warehouseDto));
            if (warehouse == null) throw new ArgumentException(nameof(warehouse));

            warehouse.Name = warehouseDto.Name;
            warehouse.Address = warehouseDto.Address;
        }
    }
}