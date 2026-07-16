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

    [HttpGet("patio-monitor")]
    [Permission(PermissionKeys.YardControl_View)]
    public async Task<IActionResult> GetPatioMonitor()
        => ResultExtensions.ToActionResult(await Mediator.Send(new GetPatioMonitorQuery()));

    [HttpGet("cortinas")]
    [Permission(PermissionKeys.Cortina_Assign)]
    public async Task<IActionResult> GetCortinas()
        => ResultExtensions.ToActionResult(await Mediator.Send(new GetCortinasDisponiblesQuery()));

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
    public async Task<IActionResult> GetTasks(
        [FromQuery] bool soloPendientes = false,
        [FromQuery] int? securityRegistrationId = null)
        => ResultExtensions.ToActionResult(await Mediator.Send(new GetSecurityTasksQuery
        {
            SoloPendientes = soloPendientes,
            SecurityRegistrationId = securityRegistrationId
        }));

    [HttpPut("tasks/{taskId}/abrir")]
    [Permission(PermissionKeys.Security_Tasks_Manage)]
    public async Task<IActionResult> AbrirCortina(int taskId, [FromBody] SecurityTaskActionRequest request)
        => ResultExtensions.ToActionResult(await Mediator.Send(new AbrirCortinaCommand
        {
            SecurityTaskId = taskId,
            RealizadaPor   = string.IsNullOrWhiteSpace(request.RealizadaPor) ? CurrentUserEmail : request.RealizadaPor,
            FotoBase64     = request.FotoBase64
        }));

    [HttpPut("tasks/{taskId}/cerrar")]
    [Permission(PermissionKeys.Security_Tasks_Manage)]
    public async Task<IActionResult> CerrarRegistro(int taskId, [FromBody] SecurityTaskActionRequest request)
        => ResultExtensions.ToActionResult(await Mediator.Send(new CerrarRegistroCommand
        {
            SecurityTaskId = taskId,
            RealizadaPor   = string.IsNullOrWhiteSpace(request.RealizadaPor) ? CurrentUserEmail : request.RealizadaPor,
            FotoBase64     = request.FotoBase64
        }));

    [HttpPut("tasks/{taskId}/iniciar-operacion")]
    [Permission(PermissionKeys.Security_Tasks_Manage)]
    public async Task<IActionResult> IniciarOperacion(int taskId, [FromBody] SecurityTaskActionRequest request)
        => ResultExtensions.ToActionResult(await Mediator.Send(new IniciarOperacionCommand
        {
            SecurityTaskId = taskId,
            RealizadaPor   = string.IsNullOrWhiteSpace(request.RealizadaPor) ? CurrentUserEmail : request.RealizadaPor,
            FotoBase64     = request.FotoBase64
        }));

    [HttpPut("tasks/{taskId}/finalizar-operacion")]
    [Permission(PermissionKeys.Security_Tasks_Manage)]
    public async Task<IActionResult> FinalizarOperacion(int taskId, [FromBody] SecurityTaskActionRequest request)
        => ResultExtensions.ToActionResult(await Mediator.Send(new FinalizarOperacionCommand
        {
            SecurityTaskId = taskId,
            RealizadaPor   = string.IsNullOrWhiteSpace(request.RealizadaPor) ? CurrentUserEmail : request.RealizadaPor,
            FotoBase64     = request.FotoBase64
        }));

    [HttpPost("{id}/generar-cierre-cortina")]
    [Permission(PermissionKeys.Security_Tasks_Manage)]
    public async Task<IActionResult> GenerarCierreCortina(int id)
        => ResultExtensions.ToActionResult(await Mediator.Send(new GenerarCierreCortinaCommand
        {
            SecurityRegistrationId = id
        }));
}
