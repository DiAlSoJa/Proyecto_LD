using Microsoft.AspNetCore.Authorization;

namespace LD.Api.Authorization;

public class AnyPermissionAttribute : AuthorizeAttribute
{
    public AnyPermissionAttribute(params string[] permissions)
    {
        Policy = AnyPermissionRequirement.BuildPolicyName(permissions);
    }
}
