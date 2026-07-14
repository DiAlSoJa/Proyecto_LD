using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.Security.Commands;
using LD.Application.Features.Security.Queries;
using LD.Contracts.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class CortinaController : CommonController
{
    [HttpGet]
    [Permission(PermissionKeys.Cortina_Assign)]
    public async Task<IActionResult> GetCortinas([FromQuery] int? warehouseId)
        => ResultExtensions.ToActionResult(await Mediator.Send(new CortinaQuery { WarehouseId = warehouseId }));

    [HttpGet("{cortinaId}")]
    [Permission(PermissionKeys.Cortina_Assign)]
    public async Task<IActionResult> GetCortinaById(int cortinaId)
        => ResultExtensions.ToActionResult(await Mediator.Send(new CortinaByIdQuery(cortinaId)));

    [HttpPost]
    [Permission(PermissionKeys.Cortina_Assign)]
    public async Task<IActionResult> CreateCortina([FromBody] CreateCortinaCommand command)
        => ResultExtensions.ToActionResult(await Mediator.Send(command));

    [HttpPut("{cortinaId}")]
    [Permission(PermissionKeys.Cortina_Assign)]
    public async Task<IActionResult> UpdateCortina(int cortinaId, [FromBody] UpdateCortinaCommand command)
    {
        command.CortinaId = cortinaId;
        return ResultExtensions.ToActionResult(await Mediator.Send(command));
    }

    [HttpDelete("{cortinaId}")]
    [Permission(PermissionKeys.Cortina_Assign)]
    public async Task<IActionResult> DeleteCortina(int cortinaId)
        => ResultExtensions.ToActionResult(await Mediator.Send(new DeleteCortinaCommand(cortinaId)));
}
