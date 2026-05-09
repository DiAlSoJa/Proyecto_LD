using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.AvailableInventories.Commands;
using LD.Application.Features.AvailableInventories.Queries;
using LD.Contracts.AvailableInventory;
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
    public async Task<IActionResult> GetAvailableInventory([FromQuery] int? standardId = null)
    {
        return ResultExtensions.ToActionResult(await Mediator.Send(new AvailableInventoryQuery
        {
            StandardId = standardId
        }));
    }

    [HttpPost("change-location")]
    [Permission(PermissionKeys.WarehouseStaff_LocationChange_Execute)]
    public async Task<IActionResult> ChangeLocation([FromBody] ChangeInventoryLocationRequest request)
    {
        var command = new ChangeInventoryLocationCommand
        {
            StandardId = request.StandardId,
            StandardIds = request.StandardIds,
            UbicacionDestino = request.UbicacionDestino,
            UserId = CurrentUserId
        };

        return ResultExtensions.ToActionResult(await Mediator.Send(command));
    }

    [HttpPost("change-status")]
    [Permission(PermissionKeys.Inventory_View)]
    public async Task<IActionResult> ChangeStatus([FromBody] ChangeInventoryStatusRequest request)
    {
        var command = new ChangeInventoryStatusCommand
        {
            StandardId = request.StandardId,
            StandardIds = request.StandardIds,
            StatusDestino = request.StatusDestino,
            UserId = CurrentUserId
        };

        return ResultExtensions.ToActionResult(await Mediator.Send(command));
    }

    [HttpPost("change-warehouse")]
    [Permission(PermissionKeys.Inventory_View)]
    public async Task<IActionResult> ChangeWarehouse([FromBody] ChangeInventoryWarehouseRequest request)
    {
        var command = new ChangeInventoryWarehouseCommand
        {
            StandardId = request.StandardId,
            StandardIds = request.StandardIds,
            WarehouseId = request.WarehouseId,
            UbicacionDestino = request.UbicacionDestino,
            UserId = CurrentUserId
        };

        return ResultExtensions.ToActionResult(await Mediator.Send(command));
    }
}
