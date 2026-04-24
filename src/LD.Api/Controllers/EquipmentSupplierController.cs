using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.EquipmentSupplier.Commands;
using LD.Application.Features.EquipmentSupplier.Queries;
using LD.Contracts.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class EquipmentSupplierController : CommonController
{
    [HttpGet]
    [Permission(PermissionKeys.ForkliftChecklist_View)]
    public async Task<IActionResult> GetEquipmentSuppliers()
    {
        return ResultExtensions.ToActionResult(await Mediator.Send(new EquipmentSupplierQuery()));
    }

    [HttpPost]
    [Permission(PermissionKeys.ForkliftChecklist_Create)]
    public async Task<IActionResult> CreateEquipmentSupplier([FromBody] CreateEquipmentSupplierCommand command)
    {
        return ResultExtensions.ToActionResult(await Mediator.Send(command));
    }
}
