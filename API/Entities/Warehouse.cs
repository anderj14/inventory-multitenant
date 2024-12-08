
using API.Interfaces;

namespace API.Entities
{
    public class Warehouse: BaseEntity, ITenantEntity
    {
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string TenantId { get; set; }

        public ICollection<Product> Products { get; set; }
        public ICollection<InventoryMovement> InventoryMovements { get; set; }
    }
}