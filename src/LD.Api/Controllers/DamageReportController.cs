using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
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
    [HttpPost("upload-image")]
    [Consumes("multipart/form-data")]
    [Permission(PermissionKeys.DamageReport_Create)]
    public async Task<IActionResult> UploadImage(IFormFile file, [FromForm] int photoNumber)
    {
        if (file is null || file.Length == 0)
            return BadRequest("Archivo invalido.");

        var normalizedPhotoNumber = photoNumber is >= 1 and <= 4 ? photoNumber : 1;
        var uploadsFolder = @"C:\LD\Uploads\DamageReports";
        Directory.CreateDirectory(uploadsFolder);

        var extension = Path.GetExtension(file.FileName);
        if (string.IsNullOrWhiteSpace(extension))
            extension = ".jpg";

        var fileName = $"damage_report_photo{normalizedPhotoNumber}_{Guid.NewGuid():N}{extension}";
        var fullPath = Path.Combine(uploadsFolder, fileName);

        await using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var relativePath = Path.Combine("uploads", "damage-reports", fileName).Replace("\\", "/");
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
    public IActionResult GetImage([FromQuery] string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return NotFound();

        var fileName = Path.GetFileName(path);
        if (string.IsNullOrWhiteSpace(fileName))
            return NotFound();

        var fullPath = Path.Combine(@"C:\LD", "Uploads", "DamageReports", fileName);
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
}
