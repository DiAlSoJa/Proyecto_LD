using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.Checklist.Commands;
using LD.Application.Features.Checklist.Queries;
using LD.Contracts.Checklist;
using LD.Contracts.Constants;
using LD.Contracts.Equipment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class ChecklistController : CommonController
{
    [HttpPost]
    [Permission(PermissionKeys.Checklist_Submit)]
    public async Task<IActionResult> Submit([FromBody] SubmitChecklistCommand command)
    {
        command.UserId = CurrentUserId;
        return ResultExtensions.ToActionResult(await Mediator.Send(command));
    }

    [HttpGet]
    [Permission(PermissionKeys.Checklist_ViewSummary)]
    public async Task<IActionResult> GetChecklists(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] int? equipmentTypeId,
        [FromQuery] int? equipmentId)
    {
        var query = new GetChecklistsQuery
        {
            From            = from,
            To              = to,
            EquipmentTypeId = equipmentTypeId,
            EquipmentId     = equipmentId
        };
        return ResultExtensions.ToActionResult(await Mediator.Send(query));
    }

    [HttpGet("{checklistId}")]
    [Permission(PermissionKeys.Checklist_ViewSummary)]
    public async Task<IActionResult> GetChecklistById(int checklistId)
    {
        var photoBaseUrl = Url.ActionLink(nameof(GetPhoto)) ?? string.Empty;
        return ResultExtensions.ToActionResult(
            await Mediator.Send(new GetChecklistByIdQuery(checklistId, photoBaseUrl)));
    }

    // Subida de foto: replica exactamente el patrón de EquipmentController.UploadImage
    [HttpPost("upload-photo")]
    [Permission(PermissionKeys.Checklist_Submit)]
    public async Task<IActionResult> UploadPhoto([FromForm] IFormFile file, [FromForm] string side)
    {
        if (file is null || file.Length == 0)
            return BadRequest("Archivo inválido.");

        var normalizedSide = side?.ToLowerInvariant() switch
        {
            "left"  => "left",
            "right" => "right",
            _       => "custom"
        };

        var uploadsFolder = @"C:\LD\Uploads\Checklists";
        Directory.CreateDirectory(uploadsFolder);

        var extension = Path.GetExtension(file.FileName);
        var fileName = $"checklist_{normalizedSide}_{Guid.NewGuid():N}{extension}";
        var fullPath = Path.Combine(uploadsFolder, fileName);

        await using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var relativePath = Path.Combine("uploads", "checklists", fileName).Replace("\\", "/");
        var photoUrl = Url.ActionLink(nameof(GetPhoto), values: new { path = relativePath }) ?? string.Empty;

        return Ok(new
        {
            isSuccess = true,
            data = new EquipmentImageUploadDto
            {
                RelativePath = relativePath,
                ImageUrl     = photoUrl
            },
            code    = 200,
            message = "Foto cargada correctamente"
        });
    }

    // Descarga de foto: replica el patrón de EquipmentController.GetImage
    [AllowAnonymous]
    [HttpGet("photo")]
    public IActionResult GetPhoto([FromQuery] string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return NotFound();

        var fileName = Path.GetFileName(path);
        if (string.IsNullOrWhiteSpace(fileName))
            return NotFound();

        var fullPath = Path.Combine(@"C:\LD\", "uploads", "checklists", fileName);
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
