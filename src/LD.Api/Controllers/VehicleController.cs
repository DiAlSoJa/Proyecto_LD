using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.Vehicle.Comands;
using LD.Application.Features.Vehicle.Queries;
using LD.Application.Features.Vehicule.Comands;
using LD.Contracts.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class VehicleController : CommonController
{
    [HttpGet]
    [Permission(PermissionKeys.Vehicle_View)]
    public async Task<IActionResult> GetVehicle()
        => ResultExtensions.ToActionResult(await Mediator.Send(new VehicleQuery()));

    [HttpGet("{plates}")]
    [Permission(PermissionKeys.Vehicle_View)]
    public async Task<IActionResult> GeVehicleById(string plates)
        => ResultExtensions.ToActionResult(await Mediator.Send(new VehicleByIdQuery(plates)));

    [HttpPost]
    [Permission(PermissionKeys.Vehicle_Create)]
    public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleCommand command)
        => ResultExtensions.ToActionResult(await Mediator.Send(command));

    [HttpPut("{plates}")]
    [Permission(PermissionKeys.Vehicle_Update)]
    public async Task<IActionResult> UpdateVehicle(string plates, [FromBody] UpdateVehicleCommand command)
    {
        command.Plates = plates;
        return ResultExtensions.ToActionResult(await Mediator.Send(command));
    }

    [HttpDelete("{plates}")]
    [Permission(PermissionKeys.Vehicle_Delete)]
    public async Task<IActionResult> DeleteVehicle(string plates)
        => ResultExtensions.ToActionResult(await Mediator.Send(new DeleteVehicleCommand { Plates = plates }));
}
