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
using System.IO;

namespace LD.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class ChecklistController : CommonController
{
    private const string UploadsRoot        = @"C:\LD\";
    private const string ChecklistsSubfolder = @"uploads\checklists";
    private const long   MaxFotoBytes        = 10 * 1024 * 1024; // 10 MB

    private static readonly HashSet<string> _extensionesPermitidas =
        new(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".webp" };

    private static readonly HashSet<string> _sidesPermitidos =
        new(StringComparer.Ordinal) { "left", "right", "custom" };

    // Valida que relativePath sea seguro y resuelve la ruta física completa.
    // Rechaza traversal (..), nulos, rutas absolutas y extensiones no permitidas.
    // Devuelve false → llamador debe responder 404 (sin revelar el motivo).
    private static bool EsRutaSegura(string relativePath, out string rutaCompleta)
    {
        rutaCompleta = string.Empty;

        if (string.IsNullOrWhiteSpace(relativePath))
            return false;

        if (relativePath.Contains("..") ||
            relativePath.Contains('\0') ||
            Path.IsPathRooted(relativePath))
            return false;

        var ext = Path.GetExtension(relativePath).ToLowerInvariant();
        if (!_extensionesPermitidas.Contains(ext))
            return false;

        // Usar solo el nombre del archivo; descarta cualquier subdirectorio en el input
        var nombre = Path.GetFileName(relativePath);
        if (string.IsNullOrWhiteSpace(nombre))
            return false;

        var baseCanonica = Path.GetFullPath(Path.Combine(UploadsRoot, ChecklistsSubfolder));
        var candidato    = Path.GetFullPath(Path.Combine(baseCanonica, nombre));

        if (!candidato.StartsWith(baseCanonica + Path.DirectorySeparatorChar,
                                   StringComparison.OrdinalIgnoreCase))
            return false;

        rutaCompleta = candidato;
        return true;
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

    [HttpPost("upload-photo")]
    [Permission(PermissionKeys.Checklist_Submit)]
    public async Task<IActionResult> UploadPhoto([FromForm] IFormFile file, [FromForm] string side)
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

        var uploadsFolder = Path.Combine(UploadsRoot, ChecklistsSubfolder);
        Directory.CreateDirectory(uploadsFolder);

        var fileName = $"checklist_{normalizedSide}_{Guid.NewGuid():N}{ext}";
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

    // Requiere autenticación — el cliente HTTP adjunta el JWT automáticamente.
    [HttpGet("photo")]
    [Permission(PermissionKeys.Checklist_ViewSummary)]
    public IActionResult GetPhoto([FromQuery] string path)
    {
        if (!EsRutaSegura(path, out var fullPath))
            return NotFound();

        if (!System.IO.File.Exists(fullPath))
            return NotFound();

        var ext = System.IO.Path.GetExtension(fullPath).ToLowerInvariant();
        var contentType = ext switch
        {
            ".png"  => "image/png",
            ".webp" => "image/webp",
            _       => "image/jpeg"
        };

        return PhysicalFile(fullPath, contentType);
    }
}
