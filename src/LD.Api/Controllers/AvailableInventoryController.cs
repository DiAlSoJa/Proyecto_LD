using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.AvailableInventories.Queries;
using LD.Contracts.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class AvailableInventoryController : CommonController
{
    [HttpGet]
    [Permission(PermissionKeys.Inventory_View)]
    public async Task<IActionResult> GetAvailableInventory()
    {
        return ResultExtensions.ToActionResult(await Mediator.Send(new AvailableInventoryQuery()));
    }
}
