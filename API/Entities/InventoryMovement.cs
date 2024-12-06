
namespace API.Entities
{
    public class InventoryMovement : ITenantEntity
    {
        public int Quantity { get; set; }

        public DateTime Date { get; set; }

        public required string MovementType { get; set; }

        public required string Comment { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public required string TenantId { get; set; }
    }
}