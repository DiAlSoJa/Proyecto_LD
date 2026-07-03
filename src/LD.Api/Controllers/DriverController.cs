using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.Driver.Commands;
using LD.Application.Features.Driver.Queries;
using LD.Contracts.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class DriverController : CommonController
{
    [HttpGet]
    [Permission(PermissionKeys.Vehicle_View)]
    public async Task<IActionResult> GetDrivers()
        => ResultExtensions.ToActionResult(await Mediator.Send(new DriverQuery()));

    [HttpGet("{driverId:int}")]
    [Permission(PermissionKeys.Vehicle_View)]
    public async Task<IActionResult> GetDriverById(int driverId)
        => ResultExtensions.ToActionResult(await Mediator.Send(new DriverByIdQuery(driverId)));

    [HttpPost]
    [Permission(PermissionKeys.Vehicle_Create)]
    public async Task<IActionResult> CreateDriver([FromBody] CreateDriverCommand command)
        => ResultExtensions.ToActionResult(await Mediator.Send(command));

    [HttpPut("{driverId:int}")]
    [Permission(PermissionKeys.Vehicle_Update)]
    public async Task<IActionResult> UpdateDriver(int driverId, [FromBody] UpdateDriverCommand command)
    {
        command.DriverId = driverId;
        return ResultExtensions.ToActionResult(await Mediator.Send(command));
    }

    [HttpDelete("{driverId:int}")]
    [Permission(PermissionKeys.Vehicle_Delete)]
    public async Task<IActionResult> DeleteDriver(int driverId)
        => ResultExtensions.ToActionResult(await Mediator.Send(new DeleteDriverCommand { DriverId = driverId }));
}
