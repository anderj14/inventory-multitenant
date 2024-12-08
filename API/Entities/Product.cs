
using API.Interfaces;

namespace API.Entities
{
    public class Product : BaseEntity, ITenantEntity
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string SKU { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        // public bool IsActive { get; set; }
        // public string? ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public int SupplierId { get; set; }
        public required string TenantId { get; set; }

        public Category Category { get; set; }
        public Brand Brand { get; set; }
        public Supplier Supplier { get; set; }
    }
}