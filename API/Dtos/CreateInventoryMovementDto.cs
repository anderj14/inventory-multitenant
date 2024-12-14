
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class CreateInventoryMovementDto
    {
        public int Quantity { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.UtcNow;

        public required string MovementType { get; set; }

        public required string Comment { get; set; }

        public int ProductId { get; set; }
    }
}