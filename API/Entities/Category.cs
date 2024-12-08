
using API.Interfaces;

namespace API.Entities
{
    public class Category : BaseEntity, ITenantEntity
    {
        public required string CategoryName { get; set; }
        public required string CategoryDescription { get; set; }
        public required string TenantId { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}