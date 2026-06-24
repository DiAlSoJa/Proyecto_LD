using AutoMapper;
using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Common.Results;
using LD.Contracts.Constants;
using LD.Contracts.Enums;
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
public class KittingIssueController : CommonController
{
    private const string AvailableStatusDisponible = "Disponible";
    private const string AvailableStatusSurtido = "Surtido";

    private readonly LdProyectDbContext _context;
    private readonly IMapper _mapper;

    private static string ResolveStandardIdText(int? standardId, string? standardIdStr)
    {
        if (!string.IsNullOrWhiteSpace(standardIdStr))
            return standardIdStr.Trim();

        return standardId.HasValue && standardId.Value > 0
            ? standardId.Value.ToString()
            : string.Empty;
    }

    private static string NormalizeAvailabilityText(string? value) =>
        string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();

    private static string Truncate(string? value, int maxLength)
    {
        var normalized = NormalizeAvailabilityText(value);
        return normalized.Length <= maxLength
            ? normalized
            : normalized[..maxLength];
    }

    private static string? ResolveIssueSd(string? requestedSd, string? detailSd, string? existingSd = null)
    {
        var candidate = !string.IsNullOrWhiteSpace(requestedSd)
            ? requestedSd
            : !string.IsNullOrWhiteSpace(existingSd)
                ? existingSd
                : detailSd;

        if (string.IsNullOrWhiteSpace(candidate))
            return null;

        return Truncate(candidate, 50);
    }

    public KittingIssueController(LdProyectDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> GetKittingIssue()
    {
        var entities = await _context.KittingIssueDetails
            .AsNoTracking()
            .Include(x => x.Product)
            .Include(x => x.Location)
            .Include(x => x.StandardLabel)
            .Include(x => x.DeliveryOrder)
            .OrderByDescending(x => x.KittingReceiptDetailId)
            .ToListAsync();

        var dto = _mapper.Map<List<KittingIssueDetailDto>>(entities);
        for (var i = 0; i < dto.Count; i++)
        {
            dto[i].StandardId = await ResolveIssueStandardIdTextAsync(entities[i]);
            dto[i].StandardIdStr = dto[i].StandardId;
        }

        return ResultExtensions.ToActionResult(Result<List<KittingIssueDetailDto>?>.Success(dto, "Kitting Issue Details obtenidos correctamente"));
    }

    [HttpGet("{kittingIssueDetailId}")]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> GetKittingIssueById(int kittingIssueDetailId)
    {
        var entity = await _context.KittingIssueDetails
            .AsNoTracking()
            .Include(x => x.Product)
            .Include(x => x.Location)
            .Include(x => x.StandardLabel)
            .FirstOrDefaultAsync(x => x.KittingReceiptDetailId == kittingIssueDetailId);

        if (entity is null)
        {
            return ResultExtensions.ToActionResult(
                Result<KittingIssueRequest?>.Failure("Kitting Issue Detail no encontrado.", new List<string> { "No existe el Kitting Issue Detail." }, 404));
        }

        var dto = _mapper.Map<KittingIssueRequest>(entity);
        dto.StandardId = await ResolveIssueStandardIdTextAsync(entity);
        return ResultExtensions.ToActionResult(Result<KittingIssueRequest?>.Success(dto, "Kitting Issue Detail obtenido correctamente"));
    }

    [HttpGet("kittingDetail/{kittingDetailId}")]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> GetKittingIssueByKittingDetailId(int kittingDetailId)
    {
        var entities = await _context.KittingIssueDetails
            .AsNoTracking()
            .Include(x => x.Product)
            .Include(x => x.Location)
            .Include(x => x.StandardLabel)
            .Include(x => x.DeliveryOrder)
            .Where(x => x.KittingDetailId == kittingDetailId)
            .OrderByDescending(x => x.KittingReceiptDetailId)
            .ToListAsync();

        var dto = _mapper.Map<List<KittingIssueDetailDto>>(entities);
        for (var i = 0; i < dto.Count; i++)
        {
            dto[i].StandardId = await ResolveIssueStandardIdTextAsync(entities[i]);
            dto[i].StandardIdStr = dto[i].StandardId;
        }

        return ResultExtensions.ToActionResult(Result<List<KittingIssueDetailDto>>.Success(dto, "Kitting Issue Details obtenidos correctamente"));
    }

    [HttpPost]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> CreateKittingIssue([FromBody] KittingIssueRequest request)
    {
        try
        {
            if (request.KittingDetailId <= 0)
                return ResultExtensions.ToActionResult(Result<string>.Failure("KittingDetailId es obligatorio.", new List<string> { "KittingDetailId es obligatorio." }));

            if (string.IsNullOrWhiteSpace(request.PartNumber))
                return ResultExtensions.ToActionResult(Result<string>.Failure("PartNumber es obligatorio.", new List<string> { "PartNumber es obligatorio." }));

            var validation = await EnsureKittingDetailEditableAsync(request.KittingDetailId);
            if (validation is not null)
                return ResultExtensions.ToActionResult(validation);

            var detail = await _context.KittingDetails
                .AsNoTracking()
                .Include(x => x.Kitting)
                .FirstOrDefaultAsync(x => x.KittingDetailId == request.KittingDetailId);

            if (detail is null)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("Kitting Detail no encontrado.", new List<string> { "No existe el Kitting Detail." }, 404));
            }

            if (detail.Kitting is null)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("No se encontro el Kitting relacionado.", new List<string> { "No se encontro el Kitting relacionado." }, 404));
            }

            var standardId = await ResolveIssueStandardIdAsync(request);
            if (!standardId.HasValue || standardId.Value <= 0)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("StandardId es obligatorio para surtir una etiqueta.", new List<string> { "StandardId es obligatorio para surtir una etiqueta." }));
            }

            request.StandardId = standardId.Value.ToString();

            var standardLabel = await ResolveStandardLabelAsync(request.StandardId);
            if (standardLabel is null)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("No existe la etiqueta indicada.", new List<string> { "No existe la etiqueta indicada." }, 404));
            }

            var availableInventory = await ResolveAvailableInventoryAsync(request, standardLabel.StandarId);
            if (availableInventory is null)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("No existe inventario disponible para esta etiqueta.", new List<string> { "No existe inventario disponible para esta etiqueta." }, 404));
            }

            var availableQuantity = GetAvailableQuantity(availableInventory);
            if (availableQuantity <= 0)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("La etiqueta seleccionada ya no tiene inventario disponible.", new List<string> { "La etiqueta seleccionada ya no tiene inventario disponible." }));
            }

            if (await IssueExistsForStandardIdAsync(request.KittingDetailId, standardLabel.StandarId))
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("La etiqueta seleccionada ya fue surtida en este embarque.", new List<string> { "La etiqueta seleccionada ya fue surtida en este embarque." }));
            }

            var movement = BuildInventoryMovement(request, detail, availableInventory, standardLabel.StandarId);
            if (movement is null)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("No se pudo preparar el movimiento de inventario.", new List<string> { "No se pudo preparar el movimiento de inventario." }));
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var inventoryToUpdate = await _context.AvailableInventories
                    .FirstOrDefaultAsync(x => x.AvailableInventoryId == availableInventory.AvailableInventoryId);

                if (inventoryToUpdate is null)
                {
                    await transaction.RollbackAsync();
                    return ResultExtensions.ToActionResult(
                        Result<string>.Failure("No se encontro el inventario disponible para actualizar.", new List<string> { "No se encontro el inventario disponible para actualizar." }, 404));
                }

                var entity = _mapper.Map<KittingIssueDetail>(request);
                entity.StandardId = standardLabel.StandarId;
                entity.Status = NormalizeStatus(availableInventory.StatusId) ?? NormalizeStatus(request.Status);
                entity.SupplyStatus = Truncate(request.SupplyStatus, 30);
                entity.ReceivedQuantity = availableQuantity;
                entity.SD = ResolveIssueSd(request.SD, detail.SD);
                entity.DeliveryOrderId = await ResolveDeliveryOrderIdForKittingAsync(detail.KittingId);

                _context.KittingIssueDetails.Add(entity);
                _context.InventoryMovements.Add(movement);

                var currentQty = inventoryToUpdate.Qty ?? 0m;
                inventoryToUpdate.Supply = Math.Min(currentQty, inventoryToUpdate.Supply + availableQuantity);
                inventoryToUpdate.FinalAvailable = Math.Max(currentQty - inventoryToUpdate.Supply, 0m);
                inventoryToUpdate.AvailableStatus = AvailableStatusSurtido;
                inventoryToUpdate.AvailableReference = Truncate(GetKittingReference(detail.Kitting), 30);
                inventoryToUpdate.LastModifiedAt = DateTime.Now;
                inventoryToUpdate.LastModifiedByUserId = CurrentUserId;

                var result = await _context.SaveChangesAsync();
                if (result <= 0)
                {
                    await transaction.RollbackAsync();
                    return ResultExtensions.ToActionResult(
                        Result<string>.Failure("No se pudo guardar el Kitting Issue Detail.", new List<string> { "No se pudo guardar el Kitting Issue Detail." }));
                }

                await UpdateCantidadSurtidaAsync(entity.KittingDetailId);
                await _context.SaveChangesAsync();

                var kittingValidated = await TryMarkKittingValidatedAsync(entity.KittingDetailId);
                if (kittingValidated)
                {
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return ResultExtensions.ToActionResult(
                    Result<string>.Success(entity.KittingReceiptDetailId.ToString(), "Kitting Issue Detail creado con exito"));
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
                Result<string>.Failure("Hubo un error al crear el Kitting Issue Detail.", new List<string> { ex.Message }));
        }
    }

    [HttpPut("{kittingIssueDetailId}")]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> UpdateKittingIssue(int kittingIssueDetailId, [FromBody] KittingIssueRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.PartNumber))
                return ResultExtensions.ToActionResult(Result<string>.Failure("PartNumber es obligatorio.", new List<string> { "PartNumber es obligatorio." }));

            var entity = await _context.KittingIssueDetails.FirstOrDefaultAsync(x => x.KittingReceiptDetailId == kittingIssueDetailId);
            if (entity is null)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("No existe el Kitting Issue Detail.", new List<string> { "No existe el Kitting Issue Detail." }, 404));
            }

            var validation = await EnsureKittingDetailEditableAsync(entity.KittingDetailId);
            if (validation is not null)
                return ResultExtensions.ToActionResult(validation);

            request.KittingReceiptDetailId = entity.KittingReceiptDetailId;
            request.KittingDetailId = entity.KittingDetailId;
            request.Status = NormalizeStatus(request.Status) ?? NormalizeStatus(entity.Status);

            var detail = await _context.KittingDetails
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.KittingDetailId == entity.KittingDetailId);

            var resolvedSd = ResolveIssueSd(request.SD, detail?.SD, entity.SD);

            var standardId = await ResolveIssueStandardIdAsync(request, entity);
            request.StandardId = standardId?.ToString();

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _mapper.Map(request, entity);
                entity.ProductId = request.ProductId > 0 ? request.ProductId : entity.ProductId;
                entity.SupplyStatus = Truncate(request.SupplyStatus, 30);
                entity.SD = resolvedSd;
                var updated = await _context.SaveChangesAsync() > 0;

                await UpdateCantidadSurtidaAsync(entity.KittingDetailId);
                await _context.SaveChangesAsync();

                var kittingValidated = await TryMarkKittingValidatedAsync(entity.KittingDetailId);
                if (kittingValidated)
                {
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                if (!updated)
                {
                    return ResultExtensions.ToActionResult(
                        Result<string>.Success(entity.KittingReceiptDetailId.ToString(), "Kitting Issue Detail actualizado"));
                }

                return ResultExtensions.ToActionResult(
                    Result<string>.Success(entity.KittingReceiptDetailId.ToString(), "Kitting Issue Detail actualizado"));
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
                Result<string>.Failure("Hubo un error al actualizar el Kitting Issue Detail.", new List<string> { ex.Message }));
        }
    }

    [HttpPost("{kittingIssueDetailId}/validate")]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> ValidateKittingIssue(int kittingIssueDetailId, [FromBody] KittingIssueValidateRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("No se pudo identificar el usuario actual.", new List<string> { "No se pudo identificar el usuario actual." }, 401));
            }

            if (string.IsNullOrWhiteSpace(request.StandardIdStr))
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("StandardIdStr es obligatorio.", new List<string> { "StandardIdStr es obligatorio." }));
            }

            var entity = await _context.KittingIssueDetails
                .Include(x => x.StandardLabel)
                .FirstOrDefaultAsync(x => x.KittingReceiptDetailId == kittingIssueDetailId);

            if (entity is null)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("No existe el Kitting Issue Detail.", new List<string> { "No existe el Kitting Issue Detail." }, 404));
            }

            var validation = await EnsureKittingDetailEditableAsync(entity.KittingDetailId);
            if (validation is not null)
                return ResultExtensions.ToActionResult(validation);

            var expectedStandardId = await ResolveIssueStandardIdTextAsync(entity);
            if (string.IsNullOrWhiteSpace(expectedStandardId))
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("No se pudo identificar el StandardId del issue.", new List<string> { "No se pudo identificar el StandardId del issue." }));
            }

            if (!string.Equals(expectedStandardId.Trim(), request.StandardIdStr.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure(
                        "El StandardId escaneado no corresponde al issue seleccionado.",
                        new List<string> { "El StandardId escaneado no corresponde al issue seleccionado." }));
            }

            if (KittingStatusNames.IsLoading(entity.SupplyStatus))
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Success(entity.KittingReceiptDetailId.ToString(), "Kitting Issue Detail ya estaba en Cargando."));
            }

            if (!KittingStatusNames.IsValidation(entity.SupplyStatus))
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure(
                        "Solo se puede validar un issue en estatus Validación.",
                        new List<string> { "Solo se puede validar un issue en estatus Validación." }));
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                entity.SupplyStatus = KittingStatusNames.Cargando;
                entity.LastModifiedAt = DateTime.Now;
                entity.LastModifiedByUserId = CurrentUserId;

                var updated = await _context.SaveChangesAsync() > 0;
                if (!updated)
                {
                    await transaction.RollbackAsync();
                    return ResultExtensions.ToActionResult(
                        Result<string>.Failure("No se pudo validar el Kitting Issue Detail.", new List<string> { "No se pudo validar el Kitting Issue Detail." }));
                }

                var kittingValidated = await TryMarkKittingValidatedAsync(entity.KittingDetailId);
                if (kittingValidated)
                {
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                return ResultExtensions.ToActionResult(
                    Result<string>.Success(
                        entity.KittingReceiptDetailId.ToString(),
                        kittingValidated
                    ? "Kitting Issue Detail actualizado a Cargando correctamente. Kitting actualizado a Cargando correctamente."
                    : "Kitting Issue Detail actualizado a Cargando correctamente."));
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
                Result<string>.Failure("Hubo un error al validar el Kitting Issue Detail.", new List<string> { ex.Message }));
        }
    }

    [HttpDelete("{kittingIssueDetailId}")]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> DeleteKittingIssue(int kittingIssueDetailId)
    {
        try
        {
            var entity = await _context.KittingIssueDetails
                .FirstOrDefaultAsync(x => x.KittingReceiptDetailId == kittingIssueDetailId);
            if (entity is null)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("No existe el Kitting Issue Detail.", new List<string> { "No existe el Kitting Issue Detail." }, 404));
            }

            var validation = await EnsureKittingDetailEditableAsync(entity.KittingDetailId);
            if (validation is not null)
                return ResultExtensions.ToActionResult(validation);

            if (!entity.StandardId.HasValue || entity.StandardId.Value <= 0)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("El Kitting Issue Detail no tiene StandardId asociado.", new List<string> { "El Kitting Issue Detail no tiene StandardId asociado." }));
            }

            if (!entity.ReceivedQuantity.HasValue || entity.ReceivedQuantity.Value <= 0)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("El Kitting Issue Detail no tiene cantidad recibida para liberar inventario.", new List<string> { "El Kitting Issue Detail no tiene cantidad recibida para liberar inventario." }));
            }

            var detail = await _context.KittingDetails
                .AsNoTracking()
                .Include(x => x.Kitting)
                .FirstOrDefaultAsync(x => x.KittingDetailId == entity.KittingDetailId);

            if (detail is null)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("Kitting Detail no encontrado.", new List<string> { "No existe el Kitting Detail." }, 404));
            }

            if (detail.Kitting is null)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("No se encontro el Kitting relacionado.", new List<string> { "No se encontro el Kitting relacionado." }, 404));
            }

            var availableInventory = await ResolveAvailableInventoryForIssueAsync(entity);
            if (availableInventory is null)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("No se encontro el inventario disponible asociado a esta etiqueta.", new List<string> { "No se encontro el inventario disponible asociado a esta etiqueta." }, 404));
            }

            var movement = BuildInventoryRestorationMovement(entity, detail, availableInventory, entity.StandardId.Value);
            if (movement is null)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("No se pudo preparar el movimiento de inventario.", new List<string> { "No se pudo preparar el movimiento de inventario." }));
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var inventoryToUpdate = await _context.AvailableInventories
                    .FirstOrDefaultAsync(x => x.AvailableInventoryId == availableInventory.AvailableInventoryId);

                if (inventoryToUpdate is null)
                {
                    await transaction.RollbackAsync();
                    return ResultExtensions.ToActionResult(
                        Result<string>.Failure("No se encontro el inventario disponible para restaurar.", new List<string> { "No se encontro el inventario disponible para restaurar." }, 404));
                }

                var currentQty = inventoryToUpdate.Qty ?? 0m;
                var restoredQuantity = entity.ReceivedQuantity ?? 0m;
                inventoryToUpdate.Supply = Math.Max(inventoryToUpdate.Supply - restoredQuantity, 0m);
                inventoryToUpdate.FinalAvailable = Math.Max(currentQty - inventoryToUpdate.Supply, 0m);
                inventoryToUpdate.AvailableStatus = inventoryToUpdate.Supply > 0m
                    ? AvailableStatusSurtido
                    : AvailableStatusDisponible;
                inventoryToUpdate.AvailableReference = inventoryToUpdate.Supply > 0m
                    ? Truncate(GetKittingReference(detail.Kitting), 30)
                    : Truncate(availableInventory.DocumentId, 30);
                inventoryToUpdate.LastModifiedAt = DateTime.Now;
                inventoryToUpdate.LastModifiedByUserId = CurrentUserId;

                _context.InventoryMovements.Add(movement);
                _context.KittingIssueDetails.Remove(entity);

                var saved = await _context.SaveChangesAsync();
                if (saved <= 0)
                {
                    await transaction.RollbackAsync();
                    return ResultExtensions.ToActionResult(
                        Result<string>.Failure("No se pudo eliminar el Kitting Issue Detail.", new List<string> { "No se pudo eliminar el Kitting Issue Detail." }));
                }

                await UpdateCantidadSurtidaAsync(entity.KittingDetailId);
                await _context.SaveChangesAsync();

                var kittingValidated = await TryMarkKittingValidatedAsync(entity.KittingDetailId);
                if (kittingValidated)
                {
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                return ResultExtensions.ToActionResult(
                    Result<string>.Success(kittingIssueDetailId.ToString(), "Kitting Issue Detail eliminado y inventario liberado."));
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
                Result<string>.Failure("Hubo un error al eliminar el Kitting Issue Detail.", new List<string> { ex.Message }));
        }
    }

    private async Task<Result<string>?> EnsureKittingDetailEditableAsync(int kittingDetailId)
    {
        var detail = await _context.KittingDetails
            .AsNoTracking()
            .Include(x => x.Kitting)
            .FirstOrDefaultAsync(x => x.KittingDetailId == kittingDetailId);

        if (detail is null)
        {
            return Result<string>.Failure("Kitting Detail no encontrado.", new List<string> { "No existe el Kitting Detail." }, 404);
        }

        if (detail.Kitting is null)
        {
            return Result<string>.Failure("No se encontro el Kitting relacionado.", new List<string> { "No se encontro el Kitting relacionado." }, 404);
        }

        if (IsTerminalStatus(detail.Kitting.Status))
        {
            return Result<string>.Failure(
                "El Kitting ya esta finalizado y no se puede modificar.",
                new List<string> { "El Kitting ya esta finalizado." });
        }

        return null;
    }

    private static bool IsTerminalStatus(string? status) =>
        IsConfirmedStatus(status) || IsCancelledStatus(status) || KittingStatusNames.IsLoading(status);

    private static bool IsConfirmedStatus(string? status) =>
        string.Equals(status?.Trim(), KittingStatusNames.Confirmado, StringComparison.OrdinalIgnoreCase);

    private static bool IsCancelledStatus(string? status) =>
        string.Equals(status?.Trim(), KittingStatusNames.Cancelado, StringComparison.OrdinalIgnoreCase);

    private static string? NormalizeStatus(string? status) =>
        string.IsNullOrWhiteSpace(status)
            ? null
            : status.Trim();

    private async Task UpdateCantidadSurtidaAsync(int kittingDetailId)
    {
        var detail = await _context.KittingDetails
            .FirstOrDefaultAsync(x => x.KittingDetailId == kittingDetailId);

        if (detail is null)
            return;

        detail.CantidadSurtida = await _context.KittingIssueDetails
            .AsNoTracking()
            .Where(x => x.KittingDetailId == kittingDetailId)
            .SumAsync(x => x.ReceivedQuantity ?? 0m);
    }

    private async Task<bool> TryMarkKittingValidatedAsync(int kittingDetailId)
    {
        var kittingId = await _context.KittingDetails
            .AsNoTracking()
            .Where(x => x.KittingDetailId == kittingDetailId)
            .Select(x => x.KittingId)
            .FirstOrDefaultAsync();

        if (kittingId <= 0)
            return false;

        var detailIds = await _context.KittingDetails
            .AsNoTracking()
            .Where(x => x.KittingId == kittingId)
            .Select(x => x.KittingDetailId)
            .ToListAsync();

        if (detailIds.Count == 0)
            return false;

        var issueStatuses = await _context.KittingIssueDetails
            .AsNoTracking()
            .Where(x => detailIds.Contains(x.KittingDetailId))
            .Select(x => x.SupplyStatus)
            .ToListAsync();

        if (issueStatuses.Count == 0)
            return false;

        if (issueStatuses.Any(x => !KittingStatusNames.IsLoading(x)))
            return false;

        var kitting = await _context.Kittings.FirstOrDefaultAsync(x => x.KittingId == kittingId);
        if (kitting is null)
            return false;

        if (KittingStatusNames.IsLoading(kitting.Status))
            return false;

        kitting.Status = KittingStatusNames.Cargando;
        kitting.LastModifiedAt = DateTime.Now;
        kitting.LastModifiedByUserId = CurrentUserId;
        return true;
    }

    private async Task<StandardLabel?> ResolveStandardLabelAsync(string standardIdText)
    {
        if (int.TryParse(standardIdText, out var standardId))
        {
            var byId = await _context.StandardLabels
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.StandarId == standardId);

            if (byId is not null)
                return byId;
        }

        return await _context.StandardLabels
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.StandarIdStr != null && x.StandarIdStr.Trim() == standardIdText);
    }

    private async Task<AvailableInventory?> ResolveAvailableInventoryAsync(KittingIssueRequest request, int standardId)
    {
        var inventories = _context.AvailableInventories
            .AsNoTracking()
            .Where(x =>
                (x.FinalAvailable > 0m || (x.Qty.HasValue && x.Qty.Value > 0)) &&
                x.StandardId == standardId &&
                (x.AvailableStatus == AvailableStatusDisponible ||
                 x.AvailableStatus == AvailableStatusSurtido ||
                 (string.IsNullOrWhiteSpace(x.AvailableStatus) &&
                  (x.StatusId == AvailableStatusDisponible || x.StatusId == AvailableStatusSurtido))));

        var exactMatch = await inventories.FirstOrDefaultAsync(x =>
            x.PartNumber == request.PartNumber &&
            x.ProductId == request.ProductId &&
            x.LocationId == request.LocationId &&
            x.LotNumber == request.LotNumber &&
            x.Reference == request.Reference &&
            x.PurchaseOrder == request.PurchaseOrder &&
            x.CustomsDeclarationNumber == request.CustomsDeclarationNumber);

        if (exactMatch is not null)
            return exactMatch;

        return await inventories.FirstOrDefaultAsync(x => x.PartNumber == request.PartNumber);
    }

    private async Task<AvailableInventory?> ResolveAvailableInventoryForIssueAsync(KittingIssueDetail issue)
    {
        if (!issue.StandardId.HasValue || issue.StandardId.Value <= 0)
            return null;

        var standardId = issue.StandardId.Value;
        var inventories = _context.AvailableInventories.Where(x => x.StandardId == standardId);

        return await inventories.FirstOrDefaultAsync(x =>
            x.PartNumber == issue.PartNumber &&
            x.ProductId == issue.ProductId &&
            x.LocationId == issue.LocationId &&
            x.LotNumber == issue.LotNumber &&
            x.Reference == issue.Reference &&
            x.PurchaseOrder == issue.PurchaseOrder &&
            x.CustomsDeclarationNumber == issue.CustomsDeclarationNumber);
    }

    private async Task<int?> ResolveIssueStandardIdAsync(KittingIssueRequest request, KittingIssueDetail? currentEntity = null)
    {
        var standardIdText = NormalizeStandardIdText(request.StandardId);
        if (standardIdText is not null)
        {
            var standardLabel = await ResolveStandardLabelAsync(standardIdText);
            if (standardLabel is not null)
                return standardLabel.StandarId;
        }

        if (currentEntity?.StandardId.HasValue == true && currentEntity.StandardId.Value > 0)
            return currentEntity.StandardId.Value;

        var exactMatch = await _context.AvailableInventories
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.PartNumber == request.PartNumber &&
                x.ProductId == request.ProductId &&
                x.LocationId == request.LocationId &&
                x.LotNumber == request.LotNumber &&
                x.Reference == request.Reference &&
                x.PurchaseOrder == request.PurchaseOrder &&
                x.CustomsDeclarationNumber == request.CustomsDeclarationNumber);

        if (exactMatch?.StandardId.HasValue == true && exactMatch.StandardId.Value > 0)
            return exactMatch.StandardId.Value;

        var partialMatch = await _context.AvailableInventories
            .AsNoTracking()
            .Where(x => x.PartNumber == request.PartNumber)
            .Where(x => !request.ProductId.HasValue || request.ProductId <= 0 || x.ProductId == request.ProductId)
            .Where(x => !request.LocationId.HasValue || request.LocationId <= 0 || x.LocationId == request.LocationId)
            .Where(x => string.IsNullOrWhiteSpace(request.LotNumber) || x.LotNumber == request.LotNumber)
            .Where(x => string.IsNullOrWhiteSpace(request.Reference) || x.Reference == request.Reference)
            .Where(x => string.IsNullOrWhiteSpace(request.PurchaseOrder) || x.PurchaseOrder == request.PurchaseOrder)
            .Where(x => string.IsNullOrWhiteSpace(request.CustomsDeclarationNumber) || x.CustomsDeclarationNumber == request.CustomsDeclarationNumber)
            .OrderByDescending(x => x.Fecha)
            .ThenByDescending(x => x.Hora)
            .FirstOrDefaultAsync();

        return partialMatch?.StandardId;
    }

    private async Task<string> ResolveIssueStandardIdTextAsync(KittingIssueDetail issue)
    {
        var directStandardId = ResolveStandardIdText(issue.StandardId, issue.StandardLabel?.StandarIdStr);
        if (!string.IsNullOrWhiteSpace(directStandardId))
            return directStandardId;

        var inventory = await _context.AvailableInventories
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.PartNumber == issue.PartNumber &&
                x.ProductId == issue.ProductId &&
                x.LocationId == issue.LocationId &&
                x.LotNumber == issue.LotNumber &&
                x.Reference == issue.Reference &&
                x.PurchaseOrder == issue.PurchaseOrder &&
                x.CustomsDeclarationNumber == issue.CustomsDeclarationNumber);

        if (inventory?.StandardId.HasValue == true && inventory.StandardId.Value > 0)
            return inventory.StandardId.Value.ToString();

        return string.Empty;
    }

    private async Task<bool> IssueExistsForStandardIdAsync(int kittingDetailId, int standardId)
    {
        return await _context.KittingIssueDetails.AsNoTracking().AnyAsync(x =>
            x.KittingDetailId == kittingDetailId &&
            x.StandardId == standardId);
    }

    private async Task<int?> ResolveDeliveryOrderIdForKittingAsync(int kittingId)
    {
        if (kittingId <= 0)
            return null;

        return await _context.DeliveryOrderKittings
            .AsNoTracking()
            .Where(x => x.KittingId == kittingId)
            .Select(x => (int?)x.DeliveryOrderId)
            .FirstOrDefaultAsync();
    }

    private InventoryMovement? BuildInventoryMovement(KittingIssueRequest request, KittingDetail detail, AvailableInventory availableInventory, int standardId)
    {
        var documentId = !string.IsNullOrWhiteSpace(detail.Kitting?.InvoiceNumber)
            ? detail.Kitting!.InvoiceNumber!.Trim()
            : !string.IsNullOrWhiteSpace(detail.Kitting?.KittingCode)
                ? detail.Kitting!.KittingCode!.Trim()
                : detail.Kitting?.KittingId > 0
                    ? detail.Kitting!.KittingId.ToString()
                    : string.Empty;

        if (string.IsNullOrWhiteSpace(documentId))
            return null;

        return new InventoryMovement
        {
            ProductId = request.ProductId ?? availableInventory.ProductId,
            ClientId = detail.Kitting!.ClientId,
            ProjectId = detail.Kitting.ProjectId,
            PartNumber = availableInventory.PartNumber.Trim(),
            Description = availableInventory.Description?.Trim() ?? request.Description?.Trim(),
            Fecha = DateTime.Now,
            Hora = DateTime.Now.TimeOfDay,
            UserId = CurrentUserId,
            LotNumber = availableInventory.LotNumber?.Trim(),
            Reference = availableInventory.Reference?.Trim(),
            PurchaseOrder = availableInventory.PurchaseOrder?.Trim(),
            CustomsDeclarationNumber = availableInventory.CustomsDeclarationNumber?.Trim(),
            ExpirationDate = availableInventory.ExpirationDate,
            DocumentType = (LD.Domain.Enums.DocumentType_e)(int)DocumentType_e.Transferencia,
            MovementType = (LD.Domain.Enums.MovementType_e)(int)MovementType_e.Picking,
            DocumentId = documentId,
            StatusId = NormalizeStatus(availableInventory.StatusId) ?? NormalizeStatus(request.Status),
            LocationId = availableInventory.LocationId,
            Qty = -Math.Abs(GetAvailableQuantity(availableInventory)),
            StandardId = standardId
        };
    }

    private InventoryMovement? BuildInventoryRestorationMovement(KittingIssueDetail issue, KittingDetail detail, AvailableInventory availableInventory, int standardId)
    {
        var documentId = !string.IsNullOrWhiteSpace(detail.Kitting?.InvoiceNumber)
            ? detail.Kitting!.InvoiceNumber!.Trim()
            : !string.IsNullOrWhiteSpace(detail.Kitting?.KittingCode)
                ? detail.Kitting!.KittingCode!.Trim()
                : detail.Kitting?.KittingId > 0
                    ? detail.Kitting!.KittingId.ToString()
                    : string.Empty;

        if (string.IsNullOrWhiteSpace(documentId))
            return null;

        return new InventoryMovement
        {
            ProductId = issue.ProductId ?? availableInventory.ProductId,
            ClientId = detail.Kitting!.ClientId,
            ProjectId = detail.Kitting.ProjectId,
            PartNumber = issue.PartNumber?.Trim() ?? availableInventory.PartNumber.Trim(),
            Description = issue.Description?.Trim() ?? availableInventory.Description?.Trim(),
            Fecha = DateTime.Now,
            Hora = DateTime.Now.TimeOfDay,
            UserId = CurrentUserId,
            LotNumber = issue.LotNumber?.Trim() ?? availableInventory.LotNumber?.Trim(),
            Reference = issue.Reference?.Trim() ?? availableInventory.Reference?.Trim(),
            PurchaseOrder = issue.PurchaseOrder?.Trim() ?? availableInventory.PurchaseOrder?.Trim(),
            CustomsDeclarationNumber = issue.CustomsDeclarationNumber?.Trim() ?? availableInventory.CustomsDeclarationNumber?.Trim(),
            ExpirationDate = issue.ExpirationDate ?? availableInventory.ExpirationDate,
            DocumentType = (LD.Domain.Enums.DocumentType_e)(int)DocumentType_e.Transferencia,
            MovementType = (LD.Domain.Enums.MovementType_e)(int)MovementType_e.Picking,
            DocumentId = documentId,
            StatusId = NormalizeStatus(issue.Status) ?? NormalizeStatus(availableInventory.StatusId),
            LocationId = issue.LocationId ?? availableInventory.LocationId,
            Qty = issue.ReceivedQuantity.HasValue ? Math.Abs(issue.ReceivedQuantity.Value) : null,
            StandardId = standardId
        };
    }

    private static decimal GetAvailableQuantity(AvailableInventory inventory)
    {
        if (inventory.FinalAvailable > 0m)
            return inventory.FinalAvailable;

        return inventory.Qty.GetValueOrDefault();
    }

    private static string? NormalizeStandardIdText(string? standardId)
    {
        if (string.IsNullOrWhiteSpace(standardId))
            return null;

        return standardId.Trim();
    }

    private static string GetKittingReference(Kitting? kitting)
    {
        if (kitting is null)
            return string.Empty;

        if (!string.IsNullOrWhiteSpace(kitting.KittingCode))
            return kitting.KittingCode.Trim();

        return kitting.KittingId > 0
            ? kitting.KittingId.ToString()
            : string.Empty;
    }
}
