using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.Security.Commands;
using LD.Application.Features.Security.Queries;
using LD.Contracts.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers.Mobile;

[Authorize]
[Route("api/[controller]")]
public class SecurityController : CommonController
{
    [HttpPost]
    [Permission(PermissionKeys.Vehicle_Create)]
    public async Task<IActionResult> Register([FromBody] CreateSecurityRegistrationCommand command)
    {
        return ResultExtensions.ToActionResult(await Mediator.Send(command));
    }

    [HttpGet("sin-salida")]
    [Permission(PermissionKeys.Security_View)]
    public async Task<IActionResult> GetSinSalida()
    {
        return ResultExtensions.ToActionResult(await Mediator.Send(new GetSecurityRegistrationsSinSalidaQuery()));
    }
}
