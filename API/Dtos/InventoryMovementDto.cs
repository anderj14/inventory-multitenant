
namespace API.Dtos
{
    public class InventoryMovementDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        public DateTime Date { get; set; }

        public required string MovementType { get; set; }

        public required string Comment { get; set; }

        public int ProductId { get; set; }

        public string AppUserId { get; set; }
    }
}