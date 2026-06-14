using LD.Api.Authorization;
using LD.Api.Controllers.Common;
using LD.Api.Workers;
using LD.Contracts.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers.Dev;

[Authorize]
[Route("api/dev/task-generator")]
public sealed class DevTaskGeneratorController : CommonController
{
    private readonly TaskGeneratorState _state;

    public DevTaskGeneratorController(TaskGeneratorState state)
    {
        _state = state;
    }

    [HttpPost("start")]
    [Permission(PermissionKeys.Dev_TaskGenerator)]
    public IActionResult Start()
    {
        _state.Start();
        return Ok(new { message = "Generador de tareas dummy iniciado.", isRunning = true });
    }

    [HttpPost("stop")]
    [Permission(PermissionKeys.Dev_TaskGenerator)]
    public IActionResult Stop()
    {
        _state.Stop();
        return Ok(new { message = "Generador de tareas dummy detenido.", isRunning = false });
    }

    [HttpGet("status")]
    [Permission(PermissionKeys.Dev_TaskGenerator)]
    public IActionResult Status()
        => Ok(new { isRunning = _state.IsRunning });
}
