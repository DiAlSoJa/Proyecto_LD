using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.Security.Commands;
using LD.Application.Features.Security.Queries;
using LD.Contracts.Constants;
using LD.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers.Mobile;

[Authorize]
[Route("api/[controller]")]
public class SecurityController : CommonController
{
    [HttpGet]
    [Permission(PermissionKeys.Security_View)]
    public async Task<IActionResult> GetByCreatedAt([FromQuery] SecurityRegistrationsByCreatedAtQuery query)
    {
        return ResultExtensions.ToActionResult(await Mediator.Send(query));
    }

    [HttpPost]
    [Permission(PermissionKeys.Vehicle_Create)]
    public async Task<IActionResult> Register([FromBody] CreateSecurityRegistrationCommand command)
        => ResultExtensions.ToActionResult(await Mediator.Send(command));

    [HttpGet("sin-salida")]
    [Permission(PermissionKeys.Security_View)]
    public async Task<IActionResult> GetSinSalida()
        => ResultExtensions.ToActionResult(await Mediator.Send(new GetSecurityRegistrationsSinSalidaQuery()));

    [HttpGet("cortinas")]
    [Permission(PermissionKeys.Cortina_Assign)]
    public async Task<IActionResult> GetCortinas([FromQuery] int? warehouseId)
        => ResultExtensions.ToActionResult(await Mediator.Send(new GetCortinasDisponiblesQuery { WarehouseId = warehouseId }));

    [HttpPut("{id}/asignar-cortina")]
    [Permission(PermissionKeys.Cortina_Assign)]
    public async Task<IActionResult> AsignarCortina(int id, [FromBody] AsignarCortinaRequest request)
        => ResultExtensions.ToActionResult(await Mediator.Send(new AsignarCortinaCommand
        {
            SecurityRegistrationId = id,
            CortinaId              = request.CortinaId
        }));

    [HttpGet("tasks")]
    [Permission(PermissionKeys.Security_Tasks_View)]
    public async Task<IActionResult> GetTasks([FromQuery] bool soloPendientes = false)
        => ResultExtensions.ToActionResult(await Mediator.Send(new GetSecurityTasksQuery { SoloPendientes = soloPendientes }));

    [HttpPut("tasks/{taskId}/abrir")]
    [Permission(PermissionKeys.Security_Tasks_Manage)]
    public async Task<IActionResult> AbrirCortina(int taskId, [FromQuery] string? realizadaPor)
        => ResultExtensions.ToActionResult(await Mediator.Send(new AbrirCortinaCommand
        {
            SecurityTaskId = taskId,
            RealizadaPor   = realizadaPor
        }));

    [HttpPut("tasks/{taskId}/cerrar")]
    [Permission(PermissionKeys.Security_Tasks_Manage)]
    public async Task<IActionResult> CerrarRegistro(int taskId, [FromQuery] string? realizadaPor)
        => ResultExtensions.ToActionResult(await Mediator.Send(new CerrarRegistroCommand
        {
            SecurityTaskId = taskId,
            RealizadaPor   = realizadaPor
        }));
}
