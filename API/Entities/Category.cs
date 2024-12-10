
using API.Interfaces;

namespace API.Entities
{
    public class Category : BaseEntity, ITenantEntity
    {
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public string TenantId { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}