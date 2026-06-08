using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Common.Interfaces.Storage;
using LD.Application.Features.DamageReports.Commands;
using LD.Application.Features.DamageReports.Queries;
using LD.Contracts.Constants;
using LD.Contracts.DamageReports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class DamageReportController : CommonController
{
    private const string DamageReportsSubfolder = "damage-reports";
    private readonly IFileStorageService _fileStorage;

    public DamageReportController(IFileStorageService fileStorage)
    {
        _fileStorage = fileStorage;
    }

    [HttpPost("upload-image")]
    [Consumes("multipart/form-data")]
    [Permission(PermissionKeys.DamageReport_Create)]
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
            DamageReportsSubfolder,
            $"damage_report_photo{normalizedPhotoNumber}",
            extension);

        if (string.IsNullOrWhiteSpace(relativePath))
            return BadRequest("No se pudo guardar la imagen.");

        var imageUrl = Url.ActionLink(nameof(GetImage), values: new { path = relativePath }) ?? string.Empty;

        return Ok(new
        {
            isSuccess = true,
            data = new DamageReportImageUploadDto
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

    [HttpGet]
    [Permission(PermissionKeys.DamageReport_View)]
    public async Task<IActionResult> GetDamageReports(
        [FromQuery] DateTime? desde,
        [FromQuery] DateTime? hasta,
        [FromQuery] int? standardId,
        [FromQuery] int? warehouseId,
        [FromQuery] string? warehouse,
        [FromQuery] string? partNumber,
        [FromQuery] string? damageType)
    {
        return ResultExtensions.ToActionResult(await Mediator.Send(new DamageReportQuery
        {
            Desde = desde,
            Hasta = hasta,
            StandardId = standardId,
            WarehouseId = warehouseId,
            Warehouse = warehouse,
            PartNumber = partNumber,
            DamageType = damageType
        }));
    }

    [HttpGet("{damageReportId}")]
    [Permission(PermissionKeys.DamageReport_View)]
    public async Task<IActionResult> GetDamageReportById(int damageReportId)
        => ResultExtensions.ToActionResult(await Mediator.Send(new DamageReportByIdQuery(damageReportId)));

    [HttpPost]
    [Permission(PermissionKeys.DamageReport_Create)]
    public async Task<IActionResult> CreateDamageReport([FromBody] CreateDamageReportCommand command)
    {
        return ResultExtensions.ToActionResult(await Mediator.Send(command));
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
