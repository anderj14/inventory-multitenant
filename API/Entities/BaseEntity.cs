
using API.Interfaces;

namespace API.Entities
{
    public class BaseEntity : ITenantEntity
    {
        public int Id { get; set; }
        public string TenantId { get; set; }
    }
}