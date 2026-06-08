using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Common.Interfaces.Storage;
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
    private const string ChecklistsSubfolder = "checklists";
    private const long MaxFotoBytes = 10 * 1024 * 1024; // 10 MB

    private readonly IFileStorageService _fileStorage;

    private static readonly HashSet<string> _extensionesPermitidas =
        new(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".webp" };

    private static readonly HashSet<string> _sidesPermitidos =
        new(StringComparer.Ordinal) { "left", "right", "custom" };

    public ChecklistController(IFileStorageService fileStorage)
    {
        _fileStorage = fileStorage;
    }

    // Verifica si el usuario tiene equipo asignado y si ya completó su checklist en las últimas 24 horas.
    [HttpGet("daily-status")]
    [Permission(PermissionKeys.ForkliftChecklist_Execute)]
    public async Task<IActionResult> GetDailyStatus()
        => ResultExtensions.ToActionResult(
            await Mediator.Send(new GetChecklistDailyStatusQuery(CurrentUserId)));

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
            From = from,
            To = to,
            EquipmentTypeId = equipmentTypeId,
            EquipmentId = equipmentId
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

    [HttpPost("upload-photo")]
    [Consumes("multipart/form-data")]
    [Permission(PermissionKeys.Checklist_Submit)]
    public async Task<IActionResult> UploadPhoto(IFormFile file, [FromForm] string side)
    {
        if (file is null || file.Length == 0)
            return BadRequest("Archivo inválido.");

        if (file.Length > MaxFotoBytes)
            return BadRequest("El archivo excede el límite de 10 MB.");

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!_extensionesPermitidas.Contains(ext))
            return BadRequest("Extensión no permitida. Use jpg, jpeg, png o webp.");

        var normalizedSide = side?.ToLowerInvariant();
        if (normalizedSide is null || !_sidesPermitidos.Contains(normalizedSide))
            return BadRequest("Valor de 'side' no válido. Use: left, right, custom.");

        await using var ms = new MemoryStream();
        await file.CopyToAsync(ms);

        var relativePath = await _fileStorage.SaveAsync(
            ms.ToArray(),
            ChecklistsSubfolder,
            $"checklist_{normalizedSide}",
            ext);

        if (string.IsNullOrWhiteSpace(relativePath))
            return BadRequest("No se pudo guardar la foto.");

        var photoUrl = Url.ActionLink(nameof(GetPhoto), values: new { path = relativePath }) ?? string.Empty;

        return Ok(new
        {
            isSuccess = true,
            data = new EquipmentImageUploadDto
            {
                RelativePath = relativePath,
                ImageUrl = photoUrl
            },
            code = 200,
            message = "Foto cargada correctamente"
        });
    }

    // Requiere autenticación — el cliente HTTP adjunta el JWT automáticamente.
    [HttpGet("photo")]
    [Permission(PermissionKeys.Checklist_ViewSummary)]
    public async Task<IActionResult> GetPhoto([FromQuery] string path)
    {
        if (!TryNormalizeBlobPath(path, out var normalizedPath))
            return NotFound();

        var stream = await _fileStorage.OpenReadAsync(normalizedPath);
        if (stream is null)
            return NotFound();

        var contentType = GetContentType(normalizedPath);
        return File(stream, contentType);
    }

    private static bool TryNormalizeBlobPath(string path, out string normalizedPath)
    {
        normalizedPath = string.Empty;

        if (string.IsNullOrWhiteSpace(path) || path.Contains("..") || path.Contains('\0'))
            return false;

        var candidate = path.Replace('\\', '/').Trim('/');
        if (!candidate.StartsWith("uploads/", StringComparison.OrdinalIgnoreCase))
            return false;

        var ext = Path.GetExtension(candidate).ToLowerInvariant();
        if (!_extensionesPermitidas.Contains(ext))
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
            _ => "image/jpeg"
        };
    }
}
