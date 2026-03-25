using LD.Domain.Entities;
using Microsoft.AspNetCore.Identity;


namespace LD.Infrastructure
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();
        public ICollection<UserWarehouse> UserWarehouses { get; set; } = new List<UserWarehouse>();

    }

}
