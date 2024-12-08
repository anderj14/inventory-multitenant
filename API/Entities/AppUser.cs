
using API.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUser : IdentityUser, ITenantEntity
    {
        public required string TenantId { get; set; }
        public ICollection<InventoryMovement> InventoryMovements { get; set; }
    }
}