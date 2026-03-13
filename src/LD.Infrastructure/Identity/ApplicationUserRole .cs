using LD.Domain.Entities;
using Microsoft.AspNetCore.Identity;


namespace LD.Infrastructure
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
 
        public ApplicationUser User { get; set; }
        public ApplicationRole Role { get; set; }
        
    }
}
