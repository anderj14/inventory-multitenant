
using API.Dtos;
using API.Entities;

namespace API.Helpers.Mapping
{
    public static class InventoryMovementMapping
    {
        public static InventoryMovementDto? ToDto(this InventoryMovement? inventoryMovement)
        {
            if (inventoryMovement == null) return null;

            return new InventoryMovementDto
            {
                Id = inventoryMovement.Id,
                Quantity = inventoryMovement.Quantity,
                Date = inventoryMovement.Date,
                MovementType = inventoryMovement.MovementType,
                Comment = inventoryMovement.Comment,
                ProductId = inventoryMovement.ProductId,
                AppUserId = inventoryMovement.AppUserId,
            };
        }

        public static InventoryMovement ToEntity(this CreateInventoryMovementDto inventoryMovementDto, string tenantId)
        {
            if (inventoryMovementDto == null) throw new ArgumentException(nameof(inventoryMovementDto));

            return new InventoryMovement
            {
                Quantity = inventoryMovementDto.Quantity,
                Date = inventoryMovementDto.Date,
                MovementType = inventoryMovementDto.MovementType,
                Comment = inventoryMovementDto.Comment,
                ProductId = inventoryMovementDto.ProductId,
                TenantId = tenantId
            };
        }

        public static void UpdateFromDto(this InventoryMovement inventoryMovement, CreateInventoryMovementDto inventoryMovementDto)
        {
            if (inventoryMovementDto == null) throw new ArgumentException(nameof(inventoryMovementDto));
            if (inventoryMovement == null) throw new ArgumentException(nameof(inventoryMovement));

            inventoryMovement.Quantity = inventoryMovementDto.Quantity;
            inventoryMovement.Date = inventoryMovementDto.Date;
            inventoryMovement.MovementType = inventoryMovementDto.MovementType;
            inventoryMovement.Comment = inventoryMovementDto.Comment;
            inventoryMovement.ProductId = inventoryMovementDto.ProductId;
        }
    }
}