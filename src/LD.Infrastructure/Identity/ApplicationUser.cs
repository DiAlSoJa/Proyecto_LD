using Microsoft.AspNetCore.Identity;


namespace LD.Infrastructure
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
