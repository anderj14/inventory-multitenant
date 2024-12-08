
using API.Interfaces;

namespace API.Entities
{
    public class Brand : BaseEntity, ITenantEntity
    {
        public required string BrandName { get; set; }
        public required string BrandDescription { get; set; }
        public required string TenantId { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}