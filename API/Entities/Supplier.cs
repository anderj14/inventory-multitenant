
using API.Interfaces;

namespace API.Entities
{
    public class Supplier : BaseEntity, ITenantEntity
    {
        public required string Name { get; set; }
        public required string ContactInfo { get; set; }
        public required string Phone { get; set; }
        public required string Address { get; set; }
        public required string TenantId { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}