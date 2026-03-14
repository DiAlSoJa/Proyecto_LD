using Microsoft.AspNetCore.Authorization;

namespace LD.Api.Authorization
{
    public class PermissionAttribute : AuthorizeAttribute
    {

        public PermissionAttribute(string permission)
        {
            Policy = permission;
        }
    
    }
}
