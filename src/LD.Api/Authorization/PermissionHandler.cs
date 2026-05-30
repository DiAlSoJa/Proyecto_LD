using LD.Api.Authorization;
using LD.Application.Common.Interfaces.Auth;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LD.Infrastructure.Authorization
{
    public class PermissionHandler
        : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IPermissionService _permissionService;

        public PermissionHandler(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return;

            var hasPermission = await _permissionService
                .HasPermissionAsync(userId, requirement.Permission);

            if (hasPermission)
                context.Succeed(requirement);
        }
    }
}
