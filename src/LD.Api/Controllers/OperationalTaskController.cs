using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Common.Interfaces.Storage;
using LD.Application.Features.OperationalTasks.Commands;
using LD.Application.Features.OperationalTasks.Queries;
using LD.Contracts.Constants;
using LD.Contracts.OperationalTasks;
using LD.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class OperationalTaskController : CommonController
{
    private const string OperationalTasksSubfolder = "operational-tasks";
    private readonly IFileStorageService _fileStorage;

    public OperationalTaskController(IFileStorageService fileStorage)
    {
        _fileStorage = fileStorage;
    }

    [HttpGet]
    [Permission(PermissionKeys.WarehouseStaff_Tasks_View)]
    public async Task<IActionResult> GetTasks([FromQuery] bool soloPendientes = false, [FromQuery] int? warehouseId = null)
        => ResultExtensions.ToActionResult(await Mediator.Send(new OperationalTaskQuery
        {
            SoloPendientes = soloPendientes,
            WarehouseId = warehouseId
        }));

    [HttpGet("{taskId}")]
    [Permission(PermissionKeys.WarehouseStaff_Tasks_View)]
    public async Task<IActionResult> GetTaskById(int taskId)
        => ResultExtensions.ToActionResult(await Mediator.Send(new OperationalTaskByIdQuery { OperationalTaskId = taskId }));

    [HttpPost]
    [Permission(PermissionKeys.WarehouseStaff_Tasks_Manage)]
    public async Task<IActionResult> CreateTask([FromBody] CreateOperationalTaskCommand command)
        => ResultExtensions.ToActionResult(await Mediator.Send(command));

    [HttpPut("{taskId}/complete")]
    [Permission(PermissionKeys.WarehouseStaff_Tasks_Manage)]
    public async Task<IActionResult> CompleteTask(int taskId, [FromBody] CompleteOperationalTaskRequest request)
        => ResultExtensions.ToActionResult(await Mediator.Send(new CompleteOperationalTaskCommand
        {
            OperationalTaskId = taskId,
            CompletedBy = request.CompletedBy,
            ResolutionObservations = request.ResolutionObservations,
            ResolvedPhoto1Path = request.ResolvedPhoto1Path,
            ResolvedPhoto2Path = request.ResolvedPhoto2Path,
            ResolvedPhoto3Path = request.ResolvedPhoto3Path,
            ResolvedPhoto4Path = request.ResolvedPhoto4Path
        }));

    [HttpPost("upload-image")]
    [Consumes("multipart/form-data")]
    [Permission(PermissionKeys.WarehouseStaff_Tasks_Manage)]
    public async Task<IActionResult> UploadImage(IFormFile file, [FromForm] int photoNumber)
    {
        if (file is null || file.Length == 0)
            return BadRequest("Archivo invalido.");

        var normalizedPhotoNumber = photoNumber is >= 1 and <= 4 ? photoNumber : 1;
        var extension = Path.GetExtension(file.FileName);
        if (string.IsNullOrWhiteSpace(extension))
            extension = ".jpg";

        await using var ms = new MemoryStream();
        await file.CopyToAsync(ms);

        var relativePath = await _fileStorage.SaveAsync(
            ms.ToArray(),
            OperationalTasksSubfolder,
            $"operational_task_photo{normalizedPhotoNumber}",
            extension);

        if (string.IsNullOrWhiteSpace(relativePath))
            return BadRequest("No se pudo guardar la imagen.");

        var imageUrl = Url.ActionLink(nameof(GetImage), values: new { path = relativePath }) ?? string.Empty;

        return Ok(new
        {
            isSuccess = true,
            data = new OperationalTaskImageUploadDto
            {
                RelativePath = relativePath,
                ImageUrl = imageUrl
            },
            code = 200,
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
            ".png" => "image/png",
            ".webp" => "image/webp",
            ".gif" => "image/gif",
            ".bmp" => "image/bmp",
            _ => "image/jpeg"
        };
    }
}
