using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Common.Interfaces.Storage;
using LD.Application.Features.Equipment.Commands;
using LD.Application.Features.Equipment.Queries;
using LD.Contracts.Constants;
using LD.Contracts.Equipment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class EquipmentController : CommonController
{
    private const string EquipmentSubfolder = "equipos";
    private readonly IFileStorageService _fileStorage;

    private static readonly HashSet<string> _extensionesPermitidas =
        new(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".webp" };

    public EquipmentController(IFileStorageService fileStorage)
    {
        _fileStorage = fileStorage;
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

    // Devuelve el equipo asignado al usuario autenticado (200 con Data=null si no tiene).
    [HttpGet("assigned-to-me")]
    public async Task<IActionResult> GetAssignedToMe()
        => ResultExtensions.ToActionResult(
               await Mediator.Send(new GetAssignedEquipmentQuery(CurrentUserId)));

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
    [Consumes("multipart/form-data")]
    [Permission(PermissionKeys.ForkliftChecklist_Create)]
    public async Task<IActionResult> UploadImage(IFormFile file, [FromForm] string side)
    {
        if (file is null || file.Length == 0)
            return BadRequest("Archivo inválido.");

        var normalizedSide = string.Equals(side, "right", StringComparison.OrdinalIgnoreCase)
            ? "right"
            : "left";

        var extension = Path.GetExtension(file.FileName);
        if (string.IsNullOrWhiteSpace(extension))
            extension = ".jpg";

        if (!_extensionesPermitidas.Contains(extension))
            return BadRequest("Extensión no permitida. Use jpg, jpeg, png o webp.");

        await using var ms = new MemoryStream();
        await file.CopyToAsync(ms);

        var relativePath = await _fileStorage.SaveAsync(
            ms.ToArray(),
            EquipmentSubfolder,
            $"equipo_{normalizedSide}",
            extension);

        if (string.IsNullOrWhiteSpace(relativePath))
            return BadRequest("No se pudo guardar la imagen.");

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

    [HttpGet("image")]
    [Permission(PermissionKeys.ForkliftChecklist_View)]
    public async Task<IActionResult> GetImage([FromQuery] string path)
    {
        if (!TryNormalizeBlobPath(path, out var normalizedPath))
            return NotFound();

        var stream = await _fileStorage.OpenReadAsync(normalizedPath);
        if (stream is null)
            return NotFound();

        return File(stream, GetContentType(normalizedPath));
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

        var stream = await _fileStorage.OpenReadAsync(relativePath);
        if (stream is null)
            return NotFound();

        return File(stream, GetContentType(relativePath));
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
