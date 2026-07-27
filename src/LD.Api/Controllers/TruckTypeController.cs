using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.TruckType.Commands;
using LD.Application.Features.TruckType.Queries;
using LD.Contracts.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class TruckTypeController : CommonController
{
    [HttpGet]
    [Permission(PermissionKeys.Catalog_View)]
    public async Task<IActionResult> GetTruckTypes()
        => ResultExtensions.ToActionResult(await Mediator.Send(new TruckTypeQuery()));

    [HttpGet("{truckTypeId}")]
    [Permission(PermissionKeys.Catalog_View)]
    public async Task<IActionResult> GetTruckTypeById(int truckTypeId)
        => ResultExtensions.ToActionResult(await Mediator.Send(new TruckTypeByIdQuery(truckTypeId)));

    [HttpPost]
    [Permission(PermissionKeys.Catalog_View)]
    public async Task<IActionResult> CreateTruckType([FromBody] CreateTruckTypeCommand command)
        => ResultExtensions.ToActionResult(await Mediator.Send(command));

    [HttpPut("{truckTypeId}")]
    [Permission(PermissionKeys.Catalog_View)]
    public async Task<IActionResult> UpdateTruckType(int truckTypeId, [FromBody] UpdateTruckTypeCommand command)
    {
        command.TruckTypeId = truckTypeId;
        return ResultExtensions.ToActionResult(await Mediator.Send(command));
    }

    [HttpDelete("{truckTypeId}")]
    [Permission(PermissionKeys.Catalog_View)]
    public async Task<IActionResult> DeleteTruckType(int truckTypeId)
        => ResultExtensions.ToActionResult(await Mediator.Send(new DeleteTruckTypeCommand { TruckTypeId = truckTypeId }));
}
