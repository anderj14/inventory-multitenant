
using API.Interfaces;

namespace API.Entities
{
    public class InventoryMovement : BaseEntity, ITenantEntity
    {
        public int Quantity { get; set; }

        public DateTime Date { get; set; }

        public required string MovementType { get; set; }

        public required string Comment { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public required string TenantId { get; set; }
    }
}