using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
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
        var uploadsFolder = @"C:\LD\Uploads\OperationalTasks";
        Directory.CreateDirectory(uploadsFolder);

        var extension = Path.GetExtension(file.FileName);
        if (string.IsNullOrWhiteSpace(extension))
            extension = ".jpg";

        var fileName = $"operational_task_photo{normalizedPhotoNumber}_{Guid.NewGuid():N}{extension}";
        var fullPath = Path.Combine(uploadsFolder, fileName);

        await using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var relativePath = Path.Combine("uploads", "operational-tasks", fileName).Replace("\\", "/");
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
    public IActionResult GetImage([FromQuery] string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return NotFound();

        var fileName = Path.GetFileName(path);
        if (string.IsNullOrWhiteSpace(fileName))
            return NotFound();

        var fullPath = Path.Combine(@"C:\LD", "Uploads", "OperationalTasks", fileName);
        if (!System.IO.File.Exists(fullPath))
            return NotFound();

        var contentType = fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
            ? "image/png"
            : fileName.EndsWith(".gif", StringComparison.OrdinalIgnoreCase)
                ? "image/gif"
                : fileName.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase)
                    ? "image/bmp"
                    : "image/jpeg";

        return PhysicalFile(fullPath, contentType);
    }
}
