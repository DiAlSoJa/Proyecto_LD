using AutoMapper;
using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Common.Results;
using LD.Application.Common.Interfaces.Storage;
using LD.Contracts.Constants;
using LD.Contracts.DTOs.Kitting;
using LD.Contracts.Kitting;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using LD.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LD.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class KittingController : CommonController
{
    private const string KittingsSubfolder = "kittings";
    private readonly LdProyectDbContext _context;
    private readonly IMapper _mapper;
    private readonly IFileStorageService _fileStorage;

    public KittingController(LdProyectDbContext context, IMapper mapper, IFileStorageService fileStorage)
    {
        _context = context;
        _mapper = mapper;
        _fileStorage = fileStorage;
    }

    [HttpGet]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> GetKitting()
    {
        var entities = await _context.Kittings
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.Project)
                .ThenInclude(x => x.Warehouse)
            .OrderByDescending(x => x.KittingId)
            .ToListAsync();

        var dto = _mapper.Map<List<KittingDto>>(entities);
        return ResultExtensions.ToActionResult(Result<List<KittingDto>?>.Success(dto, "Kittings obtenidos correctamente"));
    }

    [HttpGet("{kittingId}")]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> GetKittingById(int kittingId)
    {
        var entity = await _context.Kittings
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.KittingId == kittingId);

        if (entity is null)
        {
            return ResultExtensions.ToActionResult(
                Result<KittingRequest?>.Failure("Kitting no encontrado.", new List<string> { "No existe el Kitting." }, 404));
        }

        var dto = _mapper.Map<KittingRequest>(entity);
        return ResultExtensions.ToActionResult(Result<KittingRequest?>.Success(dto, "Kitting obtenido correctamente"));
    }

    [HttpGet("{clientId}/{projectId}")]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> GetKittingByClient(int clientId, int projectId)
    {
        var entities = await _context.Kittings
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.Project)
                .ThenInclude(x => x.Warehouse)
            .Where(x => x.ClientId == clientId && x.ProjectId == projectId)
            .OrderByDescending(x => x.KittingId)
            .ToListAsync();

        var dto = _mapper.Map<List<KittingDto>>(entities);
        return ResultExtensions.ToActionResult(Result<List<KittingDto>>.Success(dto, "Kittings obtenidos correctamente"));
    }

    [HttpPost]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> CreateKitting([FromBody] KittingRequest request)
    {
        try
        {
            if (request.ClientId <= 0)
                return ResultExtensions.ToActionResult(Result<string>.Failure("Cliente es obligatorio.", new List<string> { "Cliente es obligatorio." }));

            if (request.ProjectId <= 0)
                return ResultExtensions.ToActionResult(Result<string>.Failure("Proyecto es obligatorio.", new List<string> { "Proyecto es obligatorio." }));

            var entity = _mapper.Map<Kitting>(request);
            entity.Status = NormalizeStatus(entity.Status);

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var project = await _context.Projects
                    .FirstOrDefaultAsync(p => p.ProjectId == entity.ProjectId);

                if (project is null)
                    throw new Exception("No se encontro el proyecto.");

                if (string.IsNullOrWhiteSpace(project.KittingPrefix))
                    throw new Exception("El proyecto no tiene configurado KittingPrefix.");

                var currentNumber = 1;
                if (!string.IsNullOrWhiteSpace(project.KittingNumber) &&
                    int.TryParse(project.KittingNumber, out var parsedNumber))
                {
                    currentNumber = parsedNumber;
                }

                entity.KittingCode = $"{project.KittingPrefix}{currentNumber:D5}";
                entity.PreKittingCode = entity.KittingCode;

                _context.Kittings.Add(entity);

                project.KittingNumber = (currentNumber + 1).ToString();
                _context.Projects.Update(project);

                var result = await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return ResultExtensions.ToActionResult(
                    result > 0
                        ? Result<string>.Success(entity.KittingId.ToString(), "Kitting creado con exito")
                        : Result<string>.Failure("Hubo un error al crear el Kitting.", new List<string> { "No se pudo guardar el Kitting." }));
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        catch (Exception ex)
        {
            return ResultExtensions.ToActionResult(
                Result<string>.Failure("Hubo un error al crear el Kitting.", new List<string> { ex.Message }));
        }
    }

    [HttpPut("{kittingId}")]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> UpdateKitting(int kittingId, [FromBody] KittingRequest request)
    {
        try
        {
            var kitting = await _context.Kittings.FirstOrDefaultAsync(x => x.KittingId == kittingId);
            if (kitting is null)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("No existe el Kitting.", new List<string> { "No existe el Kitting." }, 404));
            }

            if (IsTerminalStatus(kitting.Status))
            {
                if (!request.AllowRestrictedUpdate || IsEmbarcadoStatus(kitting.Status))
                {
                    return ResultExtensions.ToActionResult(
                        Result<string>.Failure("El Kitting ya esta finalizado y no se puede editar.", new List<string> { "El Kitting ya esta finalizado." }));
                }
            }

            request.KittingId = kitting.KittingId;
            request.KittingCode = kitting.KittingCode;
            request.PreKittingCode = kitting.PreKittingCode;
            request.Status = NormalizeStatus(request.Status) ?? NormalizeStatus(kitting.Status);

            if (request.AllowRestrictedUpdate)
            {
                ApplyTransportAndDeliveryUpdate(kitting, request);
            }
            else
            {
                _mapper.Map(request, kitting);
            }

            kitting.LastModifiedAt = DateTime.Now;
            kitting.LastModifiedByUserId = CurrentUserId;

            var updated = await _context.SaveChangesAsync() > 0;
            if (!updated)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("No se pudo actualizar el Kitting.", new List<string> { "No se pudo actualizar el Kitting." }));
            }

            return ResultExtensions.ToActionResult(
                Result<string>.Success(kitting.KittingId.ToString(), "Kitting actualizado"));
        }
        catch (Exception ex)
        {
            return ResultExtensions.ToActionResult(
                Result<string>.Failure("Hubo un error al actualizar el Kitting.", new List<string> { ex.Message }));
        }
    }

    [HttpPost("{kittingId}/confirm")]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> ConfirmKitting(int kittingId)
    {
        return ResultExtensions.ToActionResult(await ChangeKittingStatusAsync(kittingId, KittingStatusNames.Validacion, "Kitting en validación correctamente."));
    }

    [HttpPost("{kittingId}/cancel")]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> CancelKitting(int kittingId)
    {
        return ResultExtensions.ToActionResult(await ChangeKittingStatusAsync(kittingId, "Cancelado", "Kitting cancelado correctamente.", requireIssueValidation: false));
    }

    [HttpPost("{kittingId}/locate")]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> LocateKitting(int kittingId)
    {
        return ResultExtensions.ToActionResult(await ChangeKittingStatusAsync(kittingId, "Ubicando", "Kitting marcado como Ubicando correctamente."));
    }

    [HttpPost("{kittingId}/send-to-supply")]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> SendToSupplyKitting(int kittingId)
    {
        return ResultExtensions.ToActionResult(await ChangeKittingStatusAsync(kittingId, "Surtiendo", "Kitting enviado a surtir correctamente."));
    }

    [HttpPost("{kittingId}/upload-image")]
    [Consumes("multipart/form-data")]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> UploadImage(int kittingId, IFormFile file, [FromForm] int photoNumber)
    {
        if (file is null || file.Length == 0)
            return BadRequest("Archivo invalido.");

        var kitting = await _context.Kittings.FirstOrDefaultAsync(x => x.KittingId == kittingId);
        if (kitting is null)
        {
            return ResultExtensions.ToActionResult(
                Result<KittingImageUploadDto?>.Failure("Kitting no encontrado.", new List<string> { "No existe el Kitting." }, 404));
        }

        var normalizedPhotoNumber = photoNumber is >= 1 and <= 4 ? photoNumber : 1;

        await using var ms = new MemoryStream();
        await file.CopyToAsync(ms);

        var relativePath = await _fileStorage.SaveJpegAsync(
            ms.ToArray(),
            KittingsSubfolder,
            $"kitting_{kittingId}_photo{normalizedPhotoNumber}");

        if (string.IsNullOrWhiteSpace(relativePath))
        {
            return ResultExtensions.ToActionResult(
                Result<KittingImageUploadDto?>.Failure("No se pudo guardar la imagen.", new List<string> { "No se pudo guardar la imagen." }));
        }

        ApplyPhotoPath(kitting, normalizedPhotoNumber, relativePath);
        kitting.LastModifiedAt = DateTime.Now;
        kitting.LastModifiedByUserId = CurrentUserId;
        await _context.SaveChangesAsync();

        var imageUrl = Url.ActionLink(nameof(GetImage), values: new { path = relativePath }) ?? string.Empty;

        return ResultExtensions.ToActionResult(
            Result<KittingImageUploadDto?>.Success(new KittingImageUploadDto
            {
                RelativePath = relativePath,
                ImageUrl = imageUrl,
                PhotoNumber = normalizedPhotoNumber
            }, "Imagen cargada correctamente"));
    }

    [HttpGet("{kittingId}/validation-photos")]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> GetValidationPhotos(int kittingId)
    {
        var kitting = await _context.Kittings
            .AsNoTracking()
            .Include(x => x.ValidationPhotos)
            .FirstOrDefaultAsync(x => x.KittingId == kittingId);

        if (kitting is null)
        {
            return ResultExtensions.ToActionResult(
                Result<List<KittingValidationPhotoDto>?>.Failure("Kitting no encontrado.", new List<string> { "No existe el Kitting." }, 404));
        }

        var photos = BuildValidationPhotoDtos(kitting);
        return ResultExtensions.ToActionResult(Result<List<KittingValidationPhotoDto>?>.Success(photos, "Fotos obtenidas correctamente"));
    }

    [HttpPost("{kittingId}/validation-photos")]
    [Consumes("multipart/form-data")]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> UploadValidationPhoto(int kittingId, IFormFile file)
    {
        if (file is null || file.Length == 0)
            return BadRequest("Archivo invalido.");

        var kitting = await _context.Kittings
            .Include(x => x.ValidationPhotos)
            .FirstOrDefaultAsync(x => x.KittingId == kittingId);

        if (kitting is null)
        {
            return ResultExtensions.ToActionResult(
                Result<KittingValidationPhotoDto?>.Failure("Kitting no encontrado.", new List<string> { "No existe el Kitting." }, 404));
        }

        await using var ms = new MemoryStream();
        await file.CopyToAsync(ms);

        var nextOrder = kitting.ValidationPhotos.Count == 0
            ? 1
            : kitting.ValidationPhotos.Max(x => x.SortOrder) + 1;

        var relativePath = await _fileStorage.SaveJpegAsync(
            ms.ToArray(),
            KittingsSubfolder,
            $"kitting_{kittingId}_validation_{nextOrder}");

        if (string.IsNullOrWhiteSpace(relativePath))
        {
            return ResultExtensions.ToActionResult(
                Result<KittingValidationPhotoDto?>.Failure("No se pudo guardar la imagen.", new List<string> { "No se pudo guardar la imagen." }));
        }

        var photo = new KittingValidationPhoto
        {
            KittingId = kittingId,
            RelativePath = relativePath,
            SortOrder = nextOrder,
            LastModifiedAt = DateTime.Now,
            LastModifiedByUserId = CurrentUserId
        };

        _context.KittingValidationPhotos.Add(photo);
        kitting.LastModifiedAt = DateTime.Now;
        kitting.LastModifiedByUserId = CurrentUserId;
        await _context.SaveChangesAsync();

        var dto = BuildValidationPhotoDto(photo);
        return ResultExtensions.ToActionResult(
            Result<KittingValidationPhotoDto?>.Success(dto, "Imagen cargada correctamente"));
    }

    [HttpDelete("{kittingId}/validation-photos/{photoKey}")]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> DeleteValidationPhoto(int kittingId, string photoKey)
    {
        var kitting = await _context.Kittings
            .Include(x => x.ValidationPhotos)
            .FirstOrDefaultAsync(x => x.KittingId == kittingId);

        if (kitting is null)
        {
            return ResultExtensions.ToActionResult(
                Result<string>.Failure("Kitting no encontrado.", new List<string> { "No existe el Kitting." }, 404));
        }

        var legacy = TryResolveLegacyPhotoKey(photoKey);
        if (legacy is not null)
        {
            var legacyIndex = legacy.Value;
            var legacyPath = GetLegacyPhotoPath(kitting, legacyIndex);
            if (string.IsNullOrWhiteSpace(legacyPath))
            {
                return ResultExtensions.ToActionResult(Result<string>.Success(string.Empty, "Foto eliminada correctamente"));
            }

            await _fileStorage.DeleteAsync(legacyPath);
            ClearLegacyPhoto(kitting, legacyIndex);
            kitting.LastModifiedAt = DateTime.Now;
            kitting.LastModifiedByUserId = CurrentUserId;
            await _context.SaveChangesAsync();

            return ResultExtensions.ToActionResult(Result<string>.Success(string.Empty, "Foto eliminada correctamente"));
        }

        if (!int.TryParse(photoKey, out var validationPhotoId))
        {
            return BadRequest("Identificador de foto invalido.");
        }

        var photo = kitting.ValidationPhotos.FirstOrDefault(x => x.KittingValidationPhotoId == validationPhotoId);
        if (photo is null)
        {
            return ResultExtensions.ToActionResult(
                Result<string>.Failure("Foto no encontrada.", new List<string> { "Foto no encontrada." }, 404));
        }

        await _fileStorage.DeleteAsync(photo.RelativePath);
        _context.KittingValidationPhotos.Remove(photo);
        kitting.LastModifiedAt = DateTime.Now;
        kitting.LastModifiedByUserId = CurrentUserId;
        await _context.SaveChangesAsync();

        return ResultExtensions.ToActionResult(Result<string>.Success(string.Empty, "Foto eliminada correctamente"));
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

    private async Task<Result<string>> ChangeKittingStatusAsync(
        int kittingId,
        string targetStatus,
        string successMessage,
        bool requireIssueValidation = true)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(CurrentUserId))
                return Result<string>.Failure("No se pudo identificar el usuario actual.", new List<string> { "No se pudo identificar el usuario actual." }, 401);

            var kitting = await _context.Kittings.FirstOrDefaultAsync(x => x.KittingId == kittingId);
            if (kitting is null)
                return Result<string>.Failure("Kitting no encontrado.", new List<string> { "No existe el Kitting." }, 404);

            if (IsLoadingStatus(kitting.Status))
            {
                return Result<string>.Failure(
                    "El Kitting ya esta en Cargando y no se puede modificar.",
                    new List<string> { "El Kitting ya esta en Cargando." });
            }

            if (IsConfirmedStatus(kitting.Status) || IsValidationStatus(kitting.Status))
            {
                return Result<string>.Failure("El Kitting ya esta en Validación y no se puede modificar.", new List<string> { "El Kitting ya esta en Validación." });
            }

            if (string.Equals(kitting.Status?.Trim(), "Cancelado", StringComparison.OrdinalIgnoreCase))
            {
                return Result<string>.Failure("El Kitting ya esta cancelado y no se puede modificar.", new List<string> { "El Kitting ya esta cancelado." });
            }

            if (string.Equals(kitting.Status?.Trim(), targetStatus, StringComparison.OrdinalIgnoreCase))
            {
                return Result<string>.Failure(
                    $"El Kitting ya esta en estatus {targetStatus}.",
                    new List<string> { $"El Kitting ya esta en estatus {targetStatus}." });
            }

            var kittingDetails = new List<KittingDetail>();
            if (requireIssueValidation || IsValidationStatus(targetStatus))
            {
                kittingDetails = await GetKittingDetailsForKittingAsync(kittingId);
            }

            if (requireIssueValidation)
            {
                var actionLabel = GetStatusValidationActionLabel(targetStatus);
                var issueDetails = await GetIssueDetailsForKittingDetailsAsync(
                    kittingDetails.Select(x => x.KittingDetailId).ToList());

                if ((IsConfirmedStatus(targetStatus) || IsValidationStatus(targetStatus)) && kittingDetails.Count == 0)
                {
                    return Result<string>.Failure(
                        "El Kitting debe tener al menos un Kitting Detail antes de pasar a Validación.",
                        new List<string> { "El Kitting debe tener al menos un Kitting Detail antes de pasar a Validación." });
                }

                if (issueDetails.Count == 0)
                {
                    return Result<string>.Failure(
                        "El Kitting debe tener al menos un Kitting Issue Detail.",
                        new List<string> { "El Kitting debe tener al menos un Kitting Issue Detail." });
                }

                if (IsConfirmedStatus(targetStatus) || IsValidationStatus(targetStatus))
                {
                    var detailIdsWithIssues = issueDetails
                        .Select(x => x.KittingDetailId)
                        .Distinct()
                        .ToHashSet();

                    var detailsWithoutIssues = kittingDetails
                        .Where(x => !detailIdsWithIssues.Contains(x.KittingDetailId))
                        .Select(GetKittingDetailLabel)
                        .Distinct()
                        .ToList();

                    if (detailsWithoutIssues.Any())
                    {
                        return Result<string>.Failure(
                            $"Antes de {actionLabel}, cada linea del Kitting debe tener al menos un Kitting Issue Detail. Lineas sin issue detail: {string.Join(", ", detailsWithoutIssues)}.",
                            new List<string>());
                    }
                }

                var invalidDetails = issueDetails
                    .Where(x =>
                        (x.LocationId is null && string.IsNullOrWhiteSpace(x.LocationCode)) ||
                        string.IsNullOrWhiteSpace(x.Status) ||
                        string.IsNullOrWhiteSpace(x.SD))
                    .Select(x => string.IsNullOrWhiteSpace(x.PartNumber)
                        ? x.KittingReceiptDetailId.ToString()
                        : x.PartNumber)
                    .Distinct()
                    .ToList();

                if (invalidDetails.Any())
                {
                    return Result<string>.Failure(
                        $"Antes de {actionLabel}, todos los Kitting Issue Details deben tener ubicacion, estatus y SD. Lineas con problema: {string.Join(", ", invalidDetails)}.",
                        new List<string>());
                }
            }

            if (IsValidationStatus(targetStatus))
            {
                await UpdateIssueDetailsSupplyStatusAsync(
                    kittingDetails.Select(x => x.KittingDetailId).ToList(),
                    targetStatus);
            }

            kitting.Status = targetStatus;
            kitting.LastModifiedAt = DateTime.Now;
            kitting.LastModifiedByUserId = CurrentUserId;

            var updated = await _context.SaveChangesAsync() > 0;
            if (!updated)
                return Result<string>.Failure("No se pudo actualizar el Kitting.", new List<string> { "No se pudo actualizar el Kitting." });

            return Result<string>.Success(kitting.KittingId.ToString(), successMessage);
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al actualizar el Kitting.", new List<string> { ex.Message });
        }
    }

    private static void ApplyPhotoPath(Kitting kitting, int photoNumber, string relativePath)
    {
        switch (photoNumber)
        {
            case 1:
                kitting.Photo1Path = relativePath;
                break;
            case 2:
                kitting.Photo2Path = relativePath;
                break;
            case 3:
                kitting.Photo3Path = relativePath;
                break;
            case 4:
                kitting.Photo4Path = relativePath;
                break;
        }
    }

    private List<KittingValidationPhotoDto> BuildValidationPhotoDtos(Kitting kitting)
    {
        var photos = new List<KittingValidationPhotoDto>();

        AddLegacyPhoto(photos, 1, kitting.Photo1Path);
        AddLegacyPhoto(photos, 2, kitting.Photo2Path);
        AddLegacyPhoto(photos, 3, kitting.Photo3Path);
        AddLegacyPhoto(photos, 4, kitting.Photo4Path);

        foreach (var photo in kitting.ValidationPhotos.OrderBy(x => x.SortOrder).ThenBy(x => x.KittingValidationPhotoId))
        {
            photos.Add(BuildValidationPhotoDto(photo));
        }

        return photos;
    }

    private void AddLegacyPhoto(List<KittingValidationPhotoDto> photos, int slot, string? relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            return;

        photos.Add(new KittingValidationPhotoDto
        {
            PhotoKey = $"legacy-{slot}",
            SortOrder = slot,
            RelativePath = relativePath,
            ImageUrl = Url.ActionLink(nameof(GetImage), values: new { path = relativePath }) ?? string.Empty,
            IsLegacy = true
        });
    }

    private KittingValidationPhotoDto BuildValidationPhotoDto(KittingValidationPhoto photo)
    {
        var imageUrl = Url.ActionLink(nameof(GetImage), values: new { path = photo.RelativePath }) ?? string.Empty;
        return new KittingValidationPhotoDto
        {
            PhotoKey = photo.KittingValidationPhotoId.ToString(),
            KittingValidationPhotoId = photo.KittingValidationPhotoId,
            SortOrder = photo.SortOrder,
            RelativePath = photo.RelativePath,
            ImageUrl = imageUrl,
            IsLegacy = false
        };
    }

    private static int? TryResolveLegacyPhotoKey(string photoKey)
    {
        if (string.IsNullOrWhiteSpace(photoKey))
            return null;

        if (!photoKey.StartsWith("legacy-", StringComparison.OrdinalIgnoreCase))
            return null;

        var slotText = photoKey["legacy-".Length..];
        if (!int.TryParse(slotText, out var slot) || slot is < 1 or > 4)
            return null;

        return slot;
    }

    private static string? GetLegacyPhotoPath(Kitting kitting, int legacyIndex)
    {
        return legacyIndex switch
        {
            1 => kitting.Photo1Path,
            2 => kitting.Photo2Path,
            3 => kitting.Photo3Path,
            4 => kitting.Photo4Path,
            _ => null
        };
    }

    private static void ClearLegacyPhoto(Kitting kitting, int legacyIndex)
    {
        switch (legacyIndex)
        {
            case 1:
                kitting.Photo1Path = null;
                break;
            case 2:
                kitting.Photo2Path = null;
                break;
            case 3:
                kitting.Photo3Path = null;
                break;
            case 4:
                kitting.Photo4Path = null;
                break;
        }
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

    private async Task<List<KittingDetail>> GetKittingDetailsForKittingAsync(int kittingId)
    {
        return await _context.KittingDetails
            .AsNoTracking()
            .Where(x => x.KittingId == kittingId)
            .OrderBy(x => x.KittingDetailId)
            .ToListAsync();
    }

    private async Task<List<KittingIssueDetail>> GetIssueDetailsForKittingDetailsAsync(List<int> detailIds)
    {
        if (detailIds.Count == 0)
        {
            return new List<KittingIssueDetail>();
        }

        return await _context.KittingIssueDetails
            .AsNoTracking()
            .Where(x => detailIds.Contains(x.KittingDetailId))
            .ToListAsync();
    }

    private async Task UpdateIssueDetailsSupplyStatusAsync(List<int> detailIds, string supplyStatus)
    {
        if (detailIds.Count == 0)
        {
            return;
        }

        var issueDetails = await _context.KittingIssueDetails
            .Where(x => detailIds.Contains(x.KittingDetailId))
            .ToListAsync();

        if (issueDetails.Count == 0)
        {
            return;
        }

        var normalizedSupplyStatus = NormalizeStatus(supplyStatus);
        foreach (var issueDetail in issueDetails)
        {
            issueDetail.SupplyStatus = normalizedSupplyStatus;
            issueDetail.LastModifiedAt = DateTime.Now;
            issueDetail.LastModifiedByUserId = CurrentUserId;
        }
    }

    private static string GetKittingDetailLabel(KittingDetail detail)
    {
        return string.IsNullOrWhiteSpace(detail.PartNumber)
            ? detail.KittingDetailId.ToString()
            : detail.PartNumber;
    }

    private static string GetStatusValidationActionLabel(string targetStatus)
    {
        if (IsValidationStatus(targetStatus))
            return "validar";

        return IsConfirmedStatus(targetStatus)
            ? "confirmar"
            : $"marcar como {targetStatus}";
    }

    private static bool IsTerminalStatus(string? status) =>
        IsConfirmedStatus(status) || IsValidationStatus(status) || IsLoadingStatus(status) || IsCancelledStatus(status);

    private static bool IsConfirmedStatus(string? status) =>
        string.Equals(status?.Trim(), KittingStatusNames.Confirmado, StringComparison.OrdinalIgnoreCase);

    private static bool IsValidationStatus(string? status) =>
        KittingStatusNames.IsValidation(status);

    private static bool IsLoadingStatus(string? status) =>
        KittingStatusNames.IsLoading(status);

    private static bool IsCancelledStatus(string? status) =>
        string.Equals(status?.Trim(), KittingStatusNames.Cancelado, StringComparison.OrdinalIgnoreCase);

    private static bool IsEmbarcadoStatus(string? status) =>
        string.Equals(status?.Trim(), "Embarcado", StringComparison.OrdinalIgnoreCase);

    private static void ApplyTransportAndDeliveryUpdate(Kitting kitting, KittingRequest request)
    {
        kitting.Status = request.Status;
        kitting.TransportLine = NormalizeStatus(request.TransportLine);
        kitting.VehicleType = NormalizeStatus(request.VehicleType);
        kitting.DriverName = NormalizeStatus(request.DriverName);
        kitting.VehiclePlate = NormalizeStatus(request.VehiclePlate);
        kitting.SealNumber = NormalizeStatus(request.SealNumber);
        kitting.Contacto = NormalizeStatus(request.Contacto);
        kitting.Direccion = NormalizeStatus(request.Direccion);
        kitting.Colonia = NormalizeStatus(request.Colonia);
        kitting.Ciudad = NormalizeStatus(request.Ciudad);
        kitting.Telefono = NormalizeStatus(request.Telefono);
        kitting.CodigoPostal = NormalizeStatus(request.CodigoPostal);
        kitting.TipoEntrega = NormalizeStatus(request.TipoEntrega);
        kitting.FechaProgramada = request.FechaProgramada;
    }

    private static string? NormalizeStatus(string? status) =>
        KittingStatusNames.Normalize(status);
}
