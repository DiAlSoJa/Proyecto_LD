using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.Equipment.Commands;
using LD.Application.Features.Equipment.Queries;
using LD.Contracts.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class EquipmentController : CommonController
{
    [HttpGet]
    [Permission(PermissionKeys.ForkliftChecklist_View)]
    public async Task<IActionResult> GetEquipment()
    {
        return ResultExtensions.ToActionResult(await Mediator.Send(new EquipmentQuery()));
    }

    [HttpGet("{equipmentId}")]
    [Permission(PermissionKeys.ForkliftChecklist_View)]
    public async Task<IActionResult> GeEquipmentById(int equipmentId)
        => ResultExtensions.ToActionResult(await Mediator.Send(new EquipmentByIdQuery(equipmentId)));

    [HttpPost]
    [Permission(PermissionKeys.ForkliftChecklist_Create)]
    public async Task<IActionResult> CreateEquipment([FromBody] CreateEquipmentCommand command)
    {
        return ResultExtensions.ToActionResult(await Mediator.Send(command));
    }

    [HttpPut("{equipmentId}")]
    [Permission(PermissionKeys.ForkliftChecklist_Update)]
    public async Task<IActionResult> UpdateEquipment(int equipmentId, UpdateEquipmentCommand command)
    {
        command.EquipmentId = equipmentId;
        var result = await Mediator.Send(command);
        return ResultExtensions.ToActionResult(result);
    }
}
