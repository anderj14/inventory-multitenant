
namespace API.Dtos
{
    public class SupplierDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string ContactInfo { get; set; }
        public required string Phone { get; set; }
        public required string Address { get; set; }
    }
}