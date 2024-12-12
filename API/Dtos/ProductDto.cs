
namespace API.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string SKU { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public bool IsActive { get; set; }
        public string? ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public int SupplierId { get; set; }
    }
}