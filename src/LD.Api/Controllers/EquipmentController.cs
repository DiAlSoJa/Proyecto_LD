using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.Equipment.Commands;
using LD.Application.Features.Equipment.Queries;
using LD.Contracts.Equipment;
using LD.Contracts.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class EquipmentController : CommonController
{
    private readonly IWebHostEnvironment _environment;

    public EquipmentController(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

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

    [HttpPost("upload-image")]
    public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string side)
    {
        if (file is null || file.Length == 0)
            return BadRequest("Archivo inválido.");

        var normalizedSide = string.Equals(side, "right", StringComparison.OrdinalIgnoreCase)
            ? "right"
            : "left";

        var uploadsFolder = @"C:\LD\Uploads\Equipos";

        Directory.CreateDirectory(uploadsFolder);

        var extension = Path.GetExtension(file.FileName);
        var fileName = $"equipo_{normalizedSide}_{Guid.NewGuid():N}{extension}";
        var fullPath = Path.Combine(uploadsFolder, fileName);

        await using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var relativePath = Path.Combine("uploads", "equipos", fileName).Replace("\\", "/");
        var imageUrl = Url.ActionLink(nameof(GetImage), values: new { path = relativePath }) ?? string.Empty;

        return Ok(new
        {
            isSuccess = true,
            data = new EquipmentImageUploadDto
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
        var uploadsFolder = @"C:\LD\";
        var fullPath = Path.Combine(
           uploadsFolder,
            "uploads",
            "equipos",
            fileName);

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

    [HttpGet("{equipmentId}/image/{side}")]
    [Permission(PermissionKeys.ForkliftChecklist_View)]
    public async Task<IActionResult> GetEquipmentImage(int equipmentId, string side)
    {
        var result = await Mediator.Send(new EquipmentByIdQuery(equipmentId));
        if (result.IsFailure || result.Data is null)
            return NotFound();

        var relativePath = string.Equals(side, "right", StringComparison.OrdinalIgnoreCase)
            ? result.Data.ImagePathRight
            : result.Data.ImagePathLeft;

        if (string.IsNullOrWhiteSpace(relativePath))
            return NotFound();

        var fileName = Path.GetFileName(relativePath);
        if (string.IsNullOrWhiteSpace(fileName))
            return NotFound();
        var uploadsFolder = @"C:\LD\";
        var fullPath = Path.Combine(
           uploadsFolder,
            "uploads",
            "equipos",
            fileName);

        if (!System.IO.File.Exists(fullPath))
            return NotFound();

        var contentType = fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
            ? "image/png"
            : fileName.EndsWith(".gif", StringComparison.OrdinalIgnoreCase)
                ? "image/gif"
                : fileName.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase)
                    ? "image/bmp"
                    : "image/jpeg";

        var bytes = await System.IO.File.ReadAllBytesAsync(fullPath);
        return File(bytes, contentType);
    }
}
