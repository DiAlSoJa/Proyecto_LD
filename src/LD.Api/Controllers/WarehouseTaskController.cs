using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Common.Interfaces.Storage;
using LD.Application.Features.WarehouseTasks.Commands;
using LD.Application.Features.WarehouseTasks.Queries;
using LD.Contracts.Constants;
using LD.Contracts.DTOs.WarehouseTasks;
using LD.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class WarehouseTaskController : CommonController
{
    private const string WarehouseTasksSubfolder = "warehouse-tasks";
    private readonly IFileStorageService _fileStorage;

    public WarehouseTaskController(IFileStorageService fileStorage)
    {
        _fileStorage = fileStorage;
    }

    [HttpGet("my-assigned")]
    [Permission(PermissionKeys.WarehouseTask_View)]
    public async Task<IActionResult> GetMyAssignedTask()
        => ResultExtensions.ToActionResult(await Mediator.Send(new MyAssignedWarehouseTaskQuery()));

    [HttpGet("connected-users")]
    [Permission(PermissionKeys.WarehouseTask_View)]
    public async Task<IActionResult> GetConnectedUsers()
        => ResultExtensions.ToActionResult(await Mediator.Send(new ConnectedUsersQuery()));

    [HttpGet("{taskId}")]
    [Permission(PermissionKeys.WarehouseTask_View)]
    public async Task<IActionResult> GetTaskById(int taskId)
        => ResultExtensions.ToActionResult(await Mediator.Send(new WarehouseTaskByIdQuery { WarehouseTaskId = taskId }));

    [HttpGet]
    [Permission(PermissionKeys.WarehouseTask_View)]
    public async Task<IActionResult> GetTasks(
        [FromQuery] bool soloPendientes = false,
        [FromQuery] int? warehouseId   = null)
        => ResultExtensions.ToActionResult(await Mediator.Send(new WarehouseTaskQuery
        {
            SoloPendientes = soloPendientes,
            WarehouseId    = warehouseId
        }));

    [HttpPost("mark-available")]
    [Permission(PermissionKeys.WarehouseTask_View)]
    public async Task<IActionResult> MarkAvailable()
        => ResultExtensions.ToActionResult(await Mediator.Send(new MarkUserAvailableCommand()));

    [HttpPost("cancel-waiting")]
    [Permission(PermissionKeys.WarehouseTask_View)]
    public async Task<IActionResult> CancelWaiting()
        => ResultExtensions.ToActionResult(await Mediator.Send(new CancelUserWaitingCommand()));

    [HttpPost]
    [Permission(PermissionKeys.WarehouseTask_Manage)]
    public async Task<IActionResult> CreateTask([FromBody] CreateWarehouseTaskCommand command)
        => ResultExtensions.ToActionResult(await Mediator.Send(command));

    // SOLO PARA PRUEBAS: genera tareas dummy NoAsignada para que el worker las auto-asigne.
    // Quitar antes de producción.
    [AllowAnonymous]
    [HttpPost("dummy")]
    public async Task<IActionResult> SeedDummyTasks([FromQuery] int warehouseId, [FromQuery] int count = 5)
        => ResultExtensions.ToActionResult(await Mediator.Send(new SeedDummyWarehouseTasksCommand
        {
            WarehouseId = warehouseId,
            Count       = count
        }));

    [HttpPut("{taskId}/complete")]
    [Permission(PermissionKeys.WarehouseTask_View)]
    public async Task<IActionResult> CompleteTask(int taskId, [FromBody] CompleteWarehouseTaskRequest request)
        => ResultExtensions.ToActionResult(await Mediator.Send(new CompleteWarehouseTaskCommand
        {
            WarehouseTaskId     = taskId,
            CompletedByName     = request.CompletedByName,
            ResolutionObservations = request.ResolutionObservations,
            ResolvedPhoto1Path  = request.ResolvedPhoto1Path,
            ResolvedPhoto2Path  = request.ResolvedPhoto2Path,
            ResolvedPhoto3Path  = request.ResolvedPhoto3Path,
            ResolvedPhoto4Path  = request.ResolvedPhoto4Path
        }));

    [HttpPost("upload-image")]
    [Consumes("multipart/form-data")]
    [Permission(PermissionKeys.WarehouseTask_View)]
    public async Task<IActionResult> UploadImage(IFormFile file, [FromForm] int photoNumber)
    {
        if (file is null || file.Length == 0)
            return BadRequest("Archivo inválido.");

        var normalizedPhotoNumber = photoNumber is >= 1 and <= 4 ? photoNumber : 1;
        var extension = Path.GetExtension(file.FileName);
        if (string.IsNullOrWhiteSpace(extension))
            extension = ".jpg";

        await using var ms = new MemoryStream();
        await file.CopyToAsync(ms);

        var relativePath = await _fileStorage.SaveAsync(
            ms.ToArray(),
            WarehouseTasksSubfolder,
            $"warehouse_task_photo{normalizedPhotoNumber}",
            extension);

        if (string.IsNullOrWhiteSpace(relativePath))
            return BadRequest("No se pudo guardar la imagen.");

        var imageUrl = Url.ActionLink(nameof(GetImage), values: new { path = relativePath }) ?? string.Empty;

        return Ok(new
        {
            isSuccess = true,
            data = new WarehouseTaskImageUploadDto
            {
                RelativePath = relativePath,
                ImageUrl     = imageUrl
            },
            code    = 200,
            message = "Imagen cargada correctamente"
        });
    }

    [AllowAnonymous]
    [HttpGet("image")]
    public async Task<IActionResult> GetImage([FromQuery] string path)
    {
        if (!TryNormalizeBlobPath(path, out var normalizedPath))
            return NotFound();

        var stream = await _fileStorage.OpenReadAsync(normalizedPath);
        if (stream is null)
            return NotFound();

        return File(stream, GetContentType(normalizedPath));
    }

    private static bool TryNormalizeBlobPath(string path, out string normalizedPath)
    {
        normalizedPath = string.Empty;

        if (string.IsNullOrWhiteSpace(path) || path.Contains("..") || path.Contains('\0'))
            return false;

        var candidate = path.Replace('\\', '/').Trim('/');
        if (!candidate.StartsWith("uploads/", StringComparison.OrdinalIgnoreCase))
            return false;

        normalizedPath = candidate;
        return true;
    }

    private static string GetContentType(string path)
    {
        var ext = Path.GetExtension(path).ToLowerInvariant();
        return ext switch
        {
            ".png"  => "image/png",
            ".webp" => "image/webp",
            ".gif"  => "image/gif",
            ".bmp"  => "image/bmp",
            _       => "image/jpeg"
        };
    }
}
