using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Common.Results;
using LD.Application.Features.CyclicInventory.Commands;
using LD.Application.Features.CyclicInventory.Queries;
using LD.Contracts.Constants;
using LD.Contracts.InventarioCiclico;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using LD.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace LD.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class CyclicInventoryController : CommonController
{
    private readonly LdProyectDbContext _context;

    public CyclicInventoryController(LdProyectDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Permission(PermissionKeys.CycleCount_View)]
    public async Task<IActionResult> GetCyclicInventories(
        [FromQuery] DateTime? desde,
        [FromQuery] DateTime? hasta,
        [FromQuery] string? estatus,
        [FromQuery] string? auditorUserId)
    {
        return ResultExtensions.ToActionResult(await Mediator.Send(new CyclicInventoryQuery
        {
            Desde = desde,
            Hasta = hasta,
            Estatus = estatus,
            AuditorUserId = auditorUserId
        }));
    }

    [HttpGet("{cyclicInventoryId:int}")]
    [Permission(PermissionKeys.CycleCount_View)]
    public async Task<IActionResult> GetCyclicInventoryById(int cyclicInventoryId)
        => ResultExtensions.ToActionResult(await Mediator.Send(new CyclicInventoryByIdQuery(cyclicInventoryId)));

    [HttpGet("{cyclicInventoryId:int}/details/{cyclicInventoryDetailId:int}/scans")]
    [Permission(PermissionKeys.CycleCount_View)]
    public async Task<IActionResult> GetCyclicInventoryScans(int cyclicInventoryId, int cyclicInventoryDetailId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return ResultExtensions.ToActionResult(
                    Result<List<CyclicInventoryScanDto>>.Failure(
                        "No se pudo identificar al usuario actual.",
                        new List<string> { "No se pudo identificar al usuario actual." },
                        401));
            }

            var inventory = await _context.CyclicInventories
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.CyclicInventoryId == cyclicInventoryId &&
                    x.AuditorUserId == CurrentUserId);

            if (inventory is null)
            {
                return ResultExtensions.ToActionResult(
                    Result<List<CyclicInventoryScanDto>>.Failure(
                        "Inventario ciclico no encontrado.",
                        new List<string> { "Inventario ciclico no encontrado." },
                        404));
            }

            var detail = await _context.CyclicInventoryDetails
                .AsNoTracking()
                .Include(x => x.Location)
                .FirstOrDefaultAsync(x =>
                    x.CyclicInventoryDetailId == cyclicInventoryDetailId &&
                    x.CyclicInventoryId == cyclicInventoryId);

            if (detail is null)
            {
                return ResultExtensions.ToActionResult(
                    Result<List<CyclicInventoryScanDto>>.Failure(
                        "Detalle del inventario ciclico no encontrado.",
                        new List<string> { "Detalle del inventario ciclico no encontrado." },
                        404));
            }

            var locationName = detail.Location?.LocationName ?? string.Empty;

            var scans = await _context.CyclicInventoryScans
                .AsNoTracking()
                .Where(x =>
                    x.CyclicInventoryId == cyclicInventoryId &&
                    x.CyclicInventoryDetailId == cyclicInventoryDetailId &&
                    x.IsActive)
                .OrderBy(x => x.ScannedAt)
                .ThenBy(x => x.CyclicInventoryScanId)
                .Select(x => new CyclicInventoryScanDto
                {
                    CyclicInventoryScanId = x.CyclicInventoryScanId,
                    CyclicInventoryId = x.CyclicInventoryId,
                    CyclicInventoryDetailId = x.CyclicInventoryDetailId,
                    LocationId = x.LocationId,
                    Ubicacion = locationName,
                    StandardId = x.StandardId,
                    ScannedAt = x.ScannedAt,
                    IsCorrectScan = x.IsCorrectScan,
                    CurrentLocation = x.CurrentLocation,
                    CurrentLocationId = x.CurrentLocationId,
                    IsInAnotherLocation = x.IsInAnotherLocation,
                    InventoryNotAvailable = x.InventoryNotAvailable
                })
                .ToListAsync();

            return ResultExtensions.ToActionResult(
                Result<List<CyclicInventoryScanDto>>.Success(scans, "Escaneos obtenidos correctamente."));
        }
        catch (Exception ex)
        {
            return ResultExtensions.ToActionResult(
                Result<List<CyclicInventoryScanDto>>.Failure(
                    "Hubo un error al consultar los escaneos del inventario ciclico.",
                    new List<string> { ex.Message }));
        }
    }

    [HttpPost("{cyclicInventoryId:int}/details/{cyclicInventoryDetailId:int}/scans")]
    [Permission(PermissionKeys.CycleCount_Update)]
    public async Task<IActionResult> CreateCyclicInventoryScan(
        int cyclicInventoryId,
        int cyclicInventoryDetailId,
        [FromBody] CreateCyclicInventoryScanRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return ResultExtensions.ToActionResult(
                    Result<CyclicInventoryScanDto>.Failure(
                        "No se pudo identificar al usuario actual.",
                        new List<string> { "No se pudo identificar al usuario actual." },
                        401));
            }

            var inventory = await _context.CyclicInventories
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.CyclicInventoryId == cyclicInventoryId &&
                    x.AuditorUserId == CurrentUserId);

            if (inventory is null)
            {
                return ResultExtensions.ToActionResult(
                    Result<CyclicInventoryScanDto>.Failure(
                        "Inventario ciclico no encontrado.",
                        new List<string> { "Inventario ciclico no encontrado." },
                        404));
            }

            var detail = await _context.CyclicInventoryDetails
                .Include(x => x.Location)
                .FirstOrDefaultAsync(x =>
                    x.CyclicInventoryDetailId == cyclicInventoryDetailId &&
                    x.CyclicInventoryId == cyclicInventoryId);

            if (detail is null)
            {
                return ResultExtensions.ToActionResult(
                    Result<CyclicInventoryScanDto>.Failure(
                        "Detalle del inventario ciclico no encontrado.",
                        new List<string> { "Detalle del inventario ciclico no encontrado." },
                        404));
            }

            if (detail.Scanned)
            {
                return ResultExtensions.ToActionResult(
                    Result<CyclicInventoryScanDto>.Failure(
                        "La ubicacion ya esta cerrada. No se pueden agregar mas escaneos.",
                        new List<string> { "La ubicacion ya esta cerrada. No se pueden agregar mas escaneos." },
                        409));
            }

            var standardId = request.StandardId?.Trim();
            if (string.IsNullOrWhiteSpace(standardId))
            {
                return ResultExtensions.ToActionResult(
                    Result<CyclicInventoryScanDto>.Failure(
                        "StandardId es obligatorio.",
                        new List<string> { "StandardId es obligatorio." }));
            }

            if (standardId.Length < 12 || !standardId.All(char.IsDigit))
            {
                return ResultExtensions.ToActionResult(
                    Result<CyclicInventoryScanDto>.Failure(
                        "La etiqueta debe tener al menos 12 digitos numericos.",
                        new List<string> { "La etiqueta debe tener al menos 12 digitos numericos." }));
            }

            var duplicateScanExists = await _context.CyclicInventoryScans
                .AsNoTracking()
                .AnyAsync(x =>
                    x.CyclicInventoryId == cyclicInventoryId &&
                    x.IsActive &&
                    x.StandardId == standardId);

            if (duplicateScanExists)
            {
                return ResultExtensions.ToActionResult(
                    Result<CyclicInventoryScanDto>.Failure(
                        $"La etiqueta {standardId} ya fue escaneada.",
                        new List<string> { $"La etiqueta {standardId} ya fue escaneada." },
                        409));
            }

            var scan = new CyclicInventoryScan
            {
                CyclicInventoryId = cyclicInventoryId,
                CyclicInventoryDetailId = cyclicInventoryDetailId,
                LocationId = detail.LocationId,
                StandardId = Truncate(standardId, 100),
                ScannedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
                CreatedByUserId = CurrentUserId,
                IsActive = true
            };

            await ApplyScanComparisonAsync(scan, detail);

            _context.CyclicInventoryScans.Add(scan);
            var saved = await _context.SaveChangesAsync() > 0;

            if (!saved)
            {
                return ResultExtensions.ToActionResult(
                    Result<CyclicInventoryScanDto>.Failure(
                        "No se pudo guardar el escaneo.",
                        new List<string> { "No se pudo guardar el escaneo." }));
            }

            return ResultExtensions.ToActionResult(
                Result<CyclicInventoryScanDto>.Success(
                    ToDto(scan, detail.Location?.LocationName ?? string.Empty),
                    "Escaneo guardado correctamente."));
        }
        catch (Exception ex)
        {
            return ResultExtensions.ToActionResult(
                Result<CyclicInventoryScanDto>.Failure(
                    "Hubo un error al guardar el escaneo.",
                    new List<string> { ex.Message }));
        }
    }

    [HttpDelete("{cyclicInventoryId:int}/details/{cyclicInventoryDetailId:int}/scans/{cyclicInventoryScanId:int}")]
    [Permission(PermissionKeys.CycleCount_Update)]
    public async Task<IActionResult> DeleteCyclicInventoryScan(
        int cyclicInventoryId,
        int cyclicInventoryDetailId,
        int cyclicInventoryScanId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure(
                        "No se pudo identificar al usuario actual.",
                        new List<string> { "No se pudo identificar al usuario actual." },
                        401));
            }

            var inventory = await _context.CyclicInventories
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.CyclicInventoryId == cyclicInventoryId &&
                    x.AuditorUserId == CurrentUserId);

            if (inventory is null)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure(
                        "Inventario ciclico no encontrado.",
                        new List<string> { "Inventario ciclico no encontrado." },
                        404));
            }

            var detail = await _context.CyclicInventoryDetails
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.CyclicInventoryDetailId == cyclicInventoryDetailId &&
                    x.CyclicInventoryId == cyclicInventoryId);

            if (detail is null)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure(
                        "Detalle del inventario ciclico no encontrado.",
                        new List<string> { "Detalle del inventario ciclico no encontrado." },
                        404));
            }

            if (detail.Scanned)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure(
                        "La ubicacion ya esta cerrada. No se pueden eliminar escaneos.",
                        new List<string> { "La ubicacion ya esta cerrada. No se pueden eliminar escaneos." },
                        409));
            }

            var scan = await _context.CyclicInventoryScans
                .FirstOrDefaultAsync(x =>
                    x.CyclicInventoryScanId == cyclicInventoryScanId &&
                    x.CyclicInventoryId == cyclicInventoryId &&
                    x.CyclicInventoryDetailId == cyclicInventoryDetailId &&
                    x.IsActive);

            if (scan is null)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure(
                        "No se encontro el escaneo activo para eliminar.",
                        new List<string> { "No se encontro el escaneo activo para eliminar." },
                        404));
            }

            scan.IsActive = false;
            scan.DeletedAt = DateTime.Now;
            scan.DeletedByUserId = CurrentUserId;
            scan.LastModifiedAt = DateTime.Now;
            scan.LastModifiedByUserId = CurrentUserId;

            var saved = await _context.SaveChangesAsync() > 0;
            return ResultExtensions.ToActionResult(
                saved
                    ? Result<string>.Success("OK", "Escaneo eliminado correctamente.")
                    : Result<string>.Failure(
                        "No se pudo eliminar el escaneo.",
                        new List<string> { "No se pudo eliminar el escaneo." }));
        }
        catch (Exception ex)
        {
            return ResultExtensions.ToActionResult(
                Result<string>.Failure(
                    "Hubo un error al eliminar el escaneo.",
                    new List<string> { ex.Message }));
        }
    }

    [HttpPost("{cyclicInventoryId:int}/details/{cyclicInventoryDetailId:int}/finish-location")]
    [Permission(PermissionKeys.CycleCount_Update)]
    public async Task<IActionResult> FinishCyclicInventoryLocation(int cyclicInventoryId, int cyclicInventoryDetailId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return ResultExtensions.ToActionResult(
                    Result<List<CyclicInventoryScanDto>>.Failure(
                        "No se pudo identificar al usuario actual.",
                        new List<string> { "No se pudo identificar al usuario actual." },
                        401));
            }

            var inventory = await _context.CyclicInventories
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.CyclicInventoryId == cyclicInventoryId &&
                    x.AuditorUserId == CurrentUserId);

            if (inventory is null)
            {
                return ResultExtensions.ToActionResult(
                    Result<List<CyclicInventoryScanDto>>.Failure(
                        "Inventario ciclico no encontrado.",
                        new List<string> { "Inventario ciclico no encontrado." },
                        404));
            }

            var detail = await _context.CyclicInventoryDetails
                .Include(x => x.Location)
                .FirstOrDefaultAsync(x =>
                    x.CyclicInventoryDetailId == cyclicInventoryDetailId &&
                    x.CyclicInventoryId == cyclicInventoryId);

            if (detail is null)
            {
                return ResultExtensions.ToActionResult(
                    Result<List<CyclicInventoryScanDto>>.Failure(
                        "Detalle del inventario ciclico no encontrado.",
                        new List<string> { "Detalle del inventario ciclico no encontrado." },
                        404));
            }

            if (detail.Scanned)
            {
                return ResultExtensions.ToActionResult(
                    Result<List<CyclicInventoryScanDto>>.Failure(
                        "La ubicacion ya esta cerrada.",
                        new List<string> { "La ubicacion ya esta cerrada." },
                        409));
            }

            detail.TakeNumber = NormalizeTakeNumber(detail.TakeNumber);
            detail.Counted = true;
            detail.Scanned = true;
            detail.LastModifiedAt = DateTime.Now;
            detail.LastModifiedByUserId = CurrentUserId;

            await RebuildAvailableInventorySnapshotAsync(cyclicInventoryId, detail);
            await _context.SaveChangesAsync();

            var scans = await _context.CyclicInventoryScans
                .Where(x =>
                    x.CyclicInventoryId == cyclicInventoryId &&
                    x.CyclicInventoryDetailId == cyclicInventoryDetailId &&
                    x.IsActive)
                .OrderBy(x => x.ScannedAt)
                .ThenBy(x => x.CyclicInventoryScanId)
                .ToListAsync();

            foreach (var scan in scans)
            {
                await ApplyScanComparisonAsync(scan, detail);
                scan.LastModifiedAt = DateTime.Now;
                scan.LastModifiedByUserId = CurrentUserId;
            }

            await ApplyDetailCountResultsAsync(detail, scans);
            await _context.SaveChangesAsync();

            var locationName = detail.Location?.LocationName ?? string.Empty;
            var result = scans
                .Select(x => ToDto(x, locationName))
                .ToList();

            return ResultExtensions.ToActionResult(
                Result<List<CyclicInventoryScanDto>>.Success(
                    result,
                    "Ubicacion terminada correctamente."));
        }
        catch (Exception ex)
        {
            return ResultExtensions.ToActionResult(
                Result<List<CyclicInventoryScanDto>>.Failure(
                    "Hubo un error al terminar la ubicacion.",
                    new List<string> { ex.Message }));
        }
    }

    [HttpPost]
    [Permission(PermissionKeys.CycleCount_Create)]
    public async Task<IActionResult> CreateCyclicInventory([FromBody] CreateCyclicInventoryCommand command)
    {
        return ResultExtensions.ToActionResult(await Mediator.Send(command));
    }

    [HttpPut("{cyclicInventoryId:int}")]
    [Permission(PermissionKeys.CycleCount_Update)]
    public async Task<IActionResult> UpdateCyclicInventory(int cyclicInventoryId, [FromBody] UpdateCyclicInventoryCommand command)
    {
        command.InventarioCiclicoId = cyclicInventoryId;
        return ResultExtensions.ToActionResult(await Mediator.Send(command));
    }

    private async Task RebuildAvailableInventorySnapshotAsync(int cyclicInventoryId, CyclicInventoryDetail detail)
    {
        var takeNumber = NormalizeTakeNumber(detail.TakeNumber);
        var now = DateTime.Now;

        var previousRows = await _context.CyclicInventoryAvailableInventories
            .Where(x =>
                x.CyclicInventoryId == cyclicInventoryId &&
                x.CyclicInventoryDetailId == detail.CyclicInventoryDetailId &&
                x.TakeNumber == takeNumber &&
                x.IsActive)
            .ToListAsync();

        foreach (var row in previousRows)
        {
            row.IsActive = false;
            row.DeletedAt = now;
            row.DeletedByUserId = CurrentUserId;
            row.LastModifiedAt = now;
            row.LastModifiedByUserId = CurrentUserId;
        }

        var availableInventories = await _context.AvailableInventories
            .AsNoTracking()
            .Include(x => x.StandardLabel)
            .Where(x =>
                x.IsActive &&
                x.LocationId == detail.LocationId &&
                (x.FinalAvailable > 0m || (x.Qty.HasValue && x.Qty.Value > 0m)))
            .ToListAsync();

        foreach (var inventory in availableInventories)
        {
            _context.CyclicInventoryAvailableInventories.Add(new CyclicInventoryAvailableInventory
            {
                CyclicInventoryId = cyclicInventoryId,
                CyclicInventoryDetailId = detail.CyclicInventoryDetailId,
                LocationId = detail.LocationId,
                TakeNumber = takeNumber,
                AvailableInventoryId = inventory.AvailableInventoryId,
                StandardId = inventory.StandardId,
                StandardIdCode = Truncate(ResolveStandardIdCode(inventory), 100),
                ProductId = inventory.ProductId,
                ClientId = inventory.ClientId,
                ProjectId = inventory.ProjectId,
                PartNumber = Truncate(inventory.PartNumber, 100),
                Description = Truncate(inventory.Description ?? string.Empty, 250),
                LotNumber = Truncate(inventory.LotNumber ?? string.Empty, 50),
                Reference = Truncate(inventory.Reference ?? string.Empty, 100),
                AvailableReference = Truncate(inventory.AvailableReference ?? string.Empty, 30),
                PurchaseOrder = Truncate(inventory.PurchaseOrder ?? string.Empty, 50),
                CustomsDeclarationNumber = Truncate(inventory.CustomsDeclarationNumber ?? string.Empty, 50),
                ExpirationDate = inventory.ExpirationDate,
                DocumentId = Truncate(inventory.DocumentId ?? string.Empty, 50),
                StatusId = Truncate(inventory.StatusId ?? string.Empty, 30),
                SD = Truncate(inventory.SD ?? string.Empty, 50),
                AvailableStatus = Truncate(inventory.AvailableStatus ?? string.Empty, 150),
                Qty = inventory.Qty,
                Supply = inventory.Supply,
                FinalAvailable = inventory.FinalAvailable,
                CreatedAt = now,
                CreatedByUserId = CurrentUserId,
                IsActive = true
            });
        }
    }

    private async Task ApplyScanComparisonAsync(CyclicInventoryScan scan, CyclicInventoryDetail detail)
    {
        var standardId = (scan.StandardId ?? string.Empty).Trim();

        if (string.IsNullOrWhiteSpace(standardId))
        {
            scan.IsCorrectScan = false;
            scan.IsInAnotherLocation = false;
            scan.InventoryNotAvailable = true;
            scan.CurrentLocation = null;
            scan.CurrentLocationId = null;
            return;
        }

        var internalStandardId = TryParseInternalStandardId(standardId);
        var availableInventories = await GetAvailableInventoriesByStandardIdAsync(standardId, internalStandardId);
        var existsInAvailableInventory = availableInventories.Count > 0;
        var existsInDetailLocation = availableInventories.Any(x => x.LocationId == detail.LocationId);
        var currentLocation = existsInAvailableInventory
            ? ResolveCurrentLocation(availableInventories, detail.LocationId)
            : null;
        var isInAnotherLocation =
            existsInAvailableInventory &&
            !existsInDetailLocation &&
            currentLocation?.Id.HasValue == true &&
            currentLocation.Id.Value != detail.LocationId;

        scan.IsCorrectScan = existsInAvailableInventory && existsInDetailLocation;
        scan.InventoryNotAvailable = !existsInAvailableInventory;
        scan.IsInAnotherLocation = isInAnotherLocation;
        scan.CurrentLocation = isInAnotherLocation ? currentLocation?.Name : null;
        scan.CurrentLocationId = isInAnotherLocation ? currentLocation?.Id : null;
    }

    private async Task ApplyDetailCountResultsAsync(
        CyclicInventoryDetail detail,
        IReadOnlyCollection<CyclicInventoryScan> scans)
    {
        var takeNumber = NormalizeTakeNumber(detail.TakeNumber);
        var theoreticalLabels = await _context.CyclicInventoryAvailableInventories
            .AsNoTracking()
            .Where(x =>
                x.CyclicInventoryDetailId == detail.CyclicInventoryDetailId &&
                x.LocationId == detail.LocationId &&
                x.TakeNumber == takeNumber &&
                x.IsActive)
            .Select(x => new
            {
                x.StandardIdCode,
                x.StandardId,
                x.AvailableInventoryId
            })
            .ToListAsync();

        var theoreticalQty = theoreticalLabels
            .Select(x => ResolveInventoryLabelKey(x.StandardIdCode, x.StandardId, x.AvailableInventoryId))
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Count();

        var physicalQty = scans
            .Where(x => x.IsActive)
            .Select(x => x.StandardId?.Trim())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Count();

        var sameLocationQty = scans
            .Where(x =>
                x.IsActive &&
                x.LocationId == detail.LocationId &&
                x.IsCorrectScan)
            .Select(x => x.StandardId?.Trim())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Count();

        var anotherLocationQty = scans
            .Where(x =>
                x.IsActive &&
                x.IsInAnotherLocation &&
                x.CurrentLocationId.HasValue &&
                x.CurrentLocationId.Value != detail.LocationId)
            .Select(x => x.StandardId?.Trim())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Count();

        detail.TheoreticalQty = theoreticalQty;
        detail.PhysicalQty = physicalQty;
        detail.SameLocationQty = sameLocationQty;
        detail.AnotherLocationQty = anotherLocationQty;
        SetCountResultByTakeNumber(
            detail,
            takeNumber,
            FormatCountPercentage(theoreticalQty, sameLocationQty));
        detail.LastModifiedAt = DateTime.Now;
        detail.LastModifiedByUserId = CurrentUserId;
    }

    private async Task<List<AvailableInventory>> GetAvailableInventoriesByStandardIdAsync(
        string standardId,
        int? internalStandardId)
    {
        return await _context.AvailableInventories
            .AsNoTracking()
            .Include(x => x.Location)
            .Include(x => x.StandardLabel)
            .Where(x =>
                x.IsActive &&
                (x.FinalAvailable > 0m || (x.Qty.HasValue && x.Qty.Value > 0m)) &&
                ((x.StandardLabel != null && x.StandardLabel.StandarIdStr == standardId) ||
                 (internalStandardId.HasValue && x.StandardId == internalStandardId.Value)))
            .ToListAsync();
    }

    private static CurrentLocationInfo? ResolveCurrentLocation(List<AvailableInventory> inventories, int detailLocationId)
    {
        var inventory = inventories
            .OrderByDescending(x => x.LocationId != detailLocationId)
            .ThenByDescending(GetAvailableQuantity)
            .FirstOrDefault();

        if (inventory is null)
            return null;

        return new CurrentLocationInfo(
            inventory.LocationId,
            inventory.Location?.LocationName?.Trim() ?? "Sin ubicacion");
    }

    private static string ResolveStandardIdCode(AvailableInventory inventory)
    {
        if (!string.IsNullOrWhiteSpace(inventory.StandardLabel?.StandarIdStr))
        {
            return inventory.StandardLabel.StandarIdStr.Trim();
        }

        return inventory.StandardId.HasValue && inventory.StandardId.Value > 0
            ? inventory.StandardId.Value.ToString()
            : string.Empty;
    }

    private static int NormalizeTakeNumber(int takeNumber)
    {
        return takeNumber <= 0 ? 1 : takeNumber;
    }

    private static int? TryParseInternalStandardId(string standardId)
    {
        return int.TryParse(standardId, out var value)
            ? value
            : null;
    }

    private static decimal GetAvailableQuantity(AvailableInventory inventory)
    {
        return inventory.FinalAvailable > 0m
            ? inventory.FinalAvailable
            : inventory.Qty ?? 0m;
    }

    private static string ResolveInventoryLabelKey(string? standardIdCode, int? standardId, int? availableInventoryId)
    {
        if (!string.IsNullOrWhiteSpace(standardIdCode))
        {
            return standardIdCode.Trim();
        }

        if (standardId.HasValue && standardId.Value > 0)
        {
            return standardId.Value.ToString(CultureInfo.InvariantCulture);
        }

        return availableInventoryId.HasValue
            ? availableInventoryId.Value.ToString(CultureInfo.InvariantCulture)
            : string.Empty;
    }

    private static void SetCountResultByTakeNumber(
        CyclicInventoryDetail detail,
        int takeNumber,
        string result)
    {
        switch (NormalizeTakeNumber(takeNumber))
        {
            case 1:
                detail.FirstCountResult = result;
                break;
            case 2:
                detail.SecondCountResult = result;
                break;
            case 3:
                detail.ThirdCountResult = result;
                break;
            case 4:
                detail.FourthCountResult = result;
                break;
        }
    }

    private static string FormatCountPercentage(decimal theoreticalQty, decimal sameLocationQty)
    {
        if (theoreticalQty == 0m)
        {
            return "0";
        }

        return ((sameLocationQty / theoreticalQty) * 100m).ToString("0.##", CultureInfo.InvariantCulture);
    }

    private static string Truncate(string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
            return value;

        return value[..maxLength];
    }

    private static CyclicInventoryScanDto ToDto(CyclicInventoryScan scan, string ubicacion)
    {
        return new CyclicInventoryScanDto
        {
            CyclicInventoryScanId = scan.CyclicInventoryScanId,
            CyclicInventoryId = scan.CyclicInventoryId,
            CyclicInventoryDetailId = scan.CyclicInventoryDetailId,
            LocationId = scan.LocationId,
            Ubicacion = ubicacion,
            StandardId = scan.StandardId,
            ScannedAt = scan.ScannedAt,
            IsCorrectScan = scan.IsCorrectScan,
            CurrentLocation = scan.CurrentLocation,
            CurrentLocationId = scan.CurrentLocationId,
            IsInAnotherLocation = scan.IsInAnotherLocation,
            InventoryNotAvailable = scan.InventoryNotAvailable
        };
    }

    private sealed record CurrentLocationInfo(int? Id, string Name);
}
