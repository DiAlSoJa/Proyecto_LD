using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Common.Results;
using LD.Contracts.Constants;
using LD.Contracts.DTOs.DeliveryOrder;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using LD.Domain.Enums;
using LD.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LD.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class DeliveryOrderController : CommonController
{
    private const string EmbarcadoStatus = "Embarcado";
    private const string AvailableStatusSalida = "Salida";

    private readonly LdProyectDbContext _context;

    public DeliveryOrderController(LdProyectDbContext context)
    {
        _context = context;
    }

    [HttpPost("from-kittings")]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> CreateFromKittings([FromBody] CreateDeliveryOrderRequest request)
    {
        try
        {
            var kittingIds = request.KittingIds
                .Where(x => x > 0)
                .Distinct()
                .ToList();

            if (kittingIds.Count == 0)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("Debes seleccionar al menos un kitting.", new List<string> { "Debes seleccionar al menos un kitting." }));
            }

            var kittings = await _context.Kittings
                .Include(x => x.Project)
                .Include(x => x.Client)
                .Where(x => kittingIds.Contains(x.KittingId))
                .ToListAsync();

            if (kittings.Count != kittingIds.Count)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("Uno o más kittings no existen.", new List<string> { "Uno o más kittings no existen." }));
            }

            var orderByRequest = kittings.ToDictionary(x => x.KittingId);
            var orderedKittings = new List<Kitting>();
            foreach (var kittingId in kittingIds)
            {
                if (orderByRequest.TryGetValue(kittingId, out var kitting))
                    orderedKittings.Add(kitting);
            }

            var first = orderedKittings.First();
            if (orderedKittings.Any(x => x.ProjectId != first.ProjectId))
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("Todos los kittings deben pertenecer al mismo proyecto.", new List<string> { "Todos los kittings deben pertenecer al mismo proyecto." }));
            }

            if (orderedKittings.Any(x => x.ClientId != first.ClientId))
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("Todos los kittings deben pertenecer al mismo cliente.", new List<string> { "Todos los kittings deben pertenecer al mismo cliente." }));
            }

            var project = first.Project;
            if (project is null)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("No se encontró el proyecto del kitting.", new List<string> { "No se encontró el proyecto del kitting." }));
            }

            var prefix = GetDeliveryOrderPrefix(project);
            if (string.IsNullOrWhiteSpace(prefix))
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("El proyecto no tiene configurado prefijo de orden de entrega.", new List<string> { "El proyecto no tiene configurado prefijo de orden de entrega." }));
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var currentNumber = GetDeliveryOrderNumber(project);
                var deliveryOrderCode = $"{prefix}{currentNumber:D5}";

                var deliveryOrder = new DeliveryOrder
                {
                    ClientId = first.ClientId,
                    ProjectId = first.ProjectId,
                    DeliveryOrderCode = deliveryOrderCode,
                    PreDeliveryOrderCode = deliveryOrderCode,
                    Status = KittingStatusNames.Cargando
                };

                _context.DeliveryOrders.Add(deliveryOrder);
                await _context.SaveChangesAsync();

                var existingRelations = await _context.DeliveryOrderKittings
                    .Where(x => kittingIds.Contains(x.KittingId))
                    .Select(x => x.KittingId)
                    .ToListAsync();

                if (existingRelations.Count > 0)
                {
                    throw new Exception("Uno o más kittings ya fueron cargados en una orden de entrega.");
                }

                var orderKittings = orderedKittings
                    .Select((kitting, index) => new DeliveryOrderKitting
                    {
                        DeliveryOrderId = deliveryOrder.DeliveryOrderId,
                        KittingId = kitting.KittingId,
                        SortOrder = index + 1
                    })
                    .ToList();

                _context.DeliveryOrderKittings.AddRange(orderKittings);
                await AssignDeliveryOrderToIssueDetailsAsync(kittingIds, deliveryOrder.DeliveryOrderId);

                SetDeliveryOrderNumber(project, currentNumber + 1);
                _context.Projects.Update(project);

                var result = await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return ResultExtensions.ToActionResult(
                    result > 0
                        ? Result<string>.Success(deliveryOrder.DeliveryOrderCode ?? deliveryOrder.DeliveryOrderId.ToString(), "Orden de entrega creada correctamente.")
                        : Result<string>.Failure("No se pudo crear la orden de entrega.", new List<string> { "No se pudo crear la orden de entrega." }));
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
                Result<string>.Failure("Hubo un error al crear la orden de entrega.", new List<string> { ex.Message }));
        }
    }

    [HttpPost("add-kittings")]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> AddKittingsToExistingOrder([FromBody] AddKittingsToDeliveryOrderRequest request)
    {
        try
        {
            var deliveryOrderCode = request.DeliveryOrderCode?.Trim();
            if (string.IsNullOrWhiteSpace(deliveryOrderCode))
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("Debes seleccionar una orden de entrega.", new List<string> { "Debes seleccionar una orden de entrega." }));
            }

            var kittingIds = request.KittingIds
                .Where(x => x > 0)
                .Distinct()
                .ToList();

            if (kittingIds.Count == 0)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("Debes seleccionar al menos un kitting.", new List<string> { "Debes seleccionar al menos un kitting." }));
            }

            var deliveryOrder = await _context.DeliveryOrders
                .Include(x => x.Client)
                .Include(x => x.Project)
                .FirstOrDefaultAsync(x =>
                    x.DeliveryOrderCode == deliveryOrderCode ||
                    x.PreDeliveryOrderCode == deliveryOrderCode);

            if (deliveryOrder is null)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("No se encontro la orden de entrega seleccionada.", new List<string> { "No se encontro la orden de entrega seleccionada." }));
            }

            var kittings = await _context.Kittings
                .Include(x => x.Project)
                .Include(x => x.Client)
                .Where(x => kittingIds.Contains(x.KittingId))
                .ToListAsync();

            if (kittings.Count != kittingIds.Count)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("Uno o mas kittings no existen.", new List<string> { "Uno o mas kittings no existen." }));
            }

            var orderByRequest = kittings.ToDictionary(x => x.KittingId);
            var orderedKittings = new List<Kitting>();
            foreach (var kittingId in kittingIds)
            {
                if (orderByRequest.TryGetValue(kittingId, out var kitting))
                    orderedKittings.Add(kitting);
            }

            var first = orderedKittings.First();
            if (orderedKittings.Any(x => x.ProjectId != first.ProjectId))
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("Todos los kittings deben pertenecer al mismo proyecto.", new List<string> { "Todos los kittings deben pertenecer al mismo proyecto." }));
            }

            if (orderedKittings.Any(x => x.ClientId != first.ClientId))
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("Todos los kittings deben pertenecer al mismo cliente.", new List<string> { "Todos los kittings deben pertenecer al mismo cliente." }));
            }

            if (deliveryOrder.ProjectId != first.ProjectId || deliveryOrder.ClientId != first.ClientId)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("La orden de entrega seleccionada no coincide con el cliente y proyecto de los kittings.", new List<string> { "La orden de entrega seleccionada no coincide con el cliente y proyecto de los kittings." }));
            }

            var existingRelations = await _context.DeliveryOrderKittings
                .AsNoTracking()
                .Where(x => kittingIds.Contains(x.KittingId))
                .Select(x => new { x.KittingId, x.DeliveryOrderId })
                .ToListAsync();

            if (existingRelations.Count > 0)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("Uno o mas kittings ya estan relacionados con una orden de entrega.", new List<string> { "Uno o mas kittings ya estan relacionados con una orden de entrega." }));
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var currentMaxSortOrder = await _context.DeliveryOrderKittings
                    .Where(x => x.DeliveryOrderId == deliveryOrder.DeliveryOrderId)
                    .Select(x => (int?)x.SortOrder)
                    .MaxAsync() ?? 0;

                var orderKittings = orderedKittings
                    .Select((kitting, index) => new DeliveryOrderKitting
                    {
                        DeliveryOrderId = deliveryOrder.DeliveryOrderId,
                        KittingId = kitting.KittingId,
                        SortOrder = currentMaxSortOrder + index + 1
                    })
                    .ToList();

                _context.DeliveryOrderKittings.AddRange(orderKittings);
                await AssignDeliveryOrderToIssueDetailsAsync(kittingIds, deliveryOrder.DeliveryOrderId);

                var result = await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return ResultExtensions.ToActionResult(
                    result > 0
                        ? Result<string>.Success(deliveryOrder.DeliveryOrderCode ?? deliveryOrder.PreDeliveryOrderCode ?? deliveryOrder.DeliveryOrderId.ToString(), "Kittings agregados correctamente a la orden de entrega.")
                        : Result<string>.Failure("No se pudieron agregar los kittings a la orden de entrega.", new List<string> { "No se pudieron agregar los kittings a la orden de entrega." }));
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
                Result<string>.Failure("Hubo un error al agregar los kittings a la orden de entrega.", new List<string> { ex.Message }));
        }
    }

    [HttpPost("dar-salida")]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> DarSalida([FromBody] FinishDeliveryOrderLoadingRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("No se pudo identificar al usuario actual.", new List<string> { "No se pudo identificar al usuario actual." }, 401));
            }

            var deliveryOrderCode = request.DeliveryOrderCode?.Trim();
            if (string.IsNullOrWhiteSpace(deliveryOrderCode))
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("Debes seleccionar una orden de entrega.", new List<string> { "Debes seleccionar una orden de entrega." }));
            }

            var deliveryOrder = await _context.DeliveryOrders
                .Include(x => x.DeliveryOrderKittings)
                    .ThenInclude(x => x.Kitting)
                .FirstOrDefaultAsync(x =>
                    x.DeliveryOrderCode == deliveryOrderCode ||
                    x.PreDeliveryOrderCode == deliveryOrderCode);

            if (deliveryOrder is null)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("No se encontro la orden de entrega seleccionada.", new List<string> { "No se encontro la orden de entrega seleccionada." }, 404));
            }

            var resolvedDeliveryOrderCode = deliveryOrder.DeliveryOrderCode ?? deliveryOrder.PreDeliveryOrderCode ?? deliveryOrder.DeliveryOrderId.ToString();

            var kittings = deliveryOrder.DeliveryOrderKittings
                .Where(x => x.Kitting != null)
                .OrderBy(x => x.SortOrder)
                .Select(x => x.Kitting!)
                .GroupBy(x => x.KittingId)
                .Select(group => group.First())
                .ToList();

            if (kittings.Count == 0)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("La orden de entrega no tiene kittings relacionados.", new List<string> { "La orden de entrega no tiene kittings relacionados." }));
            }

            if (kittings.Any(kitting => !KittingStatusNames.IsLoading(kitting.Status)))
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("Los embarques deben estar en estatus Cargando para dar salida.", new List<string> { "Los embarques deben estar en estatus Cargando para dar salida." }));
            }

            var deliveryMovementExists = await _context.InventoryMovements
                .AsNoTracking()
                .AnyAsync(x =>
                    x.DocumentType == DocumentType_e.Salida &&
                    x.DocumentId == resolvedDeliveryOrderCode);

            if (deliveryMovementExists)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("La orden de entrega ya tiene salida registrada.", new List<string> { "La orden de entrega ya tiene salida registrada." }, 409));
            }

            var issueDetails = await _context.KittingIssueDetails
                .AsNoTracking()
                .Where(x => x.DeliveryOrderId == deliveryOrder.DeliveryOrderId && !x.DeleteRow)
                .OrderBy(x => x.KittingDetailId)
                .ThenBy(x => x.KittingReceiptDetailId)
                .ToListAsync();

            if (issueDetails.Count == 0)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("La orden de entrega no tiene inventario relacionado para dar salida.", new List<string> { "La orden de entrega no tiene inventario relacionado para dar salida." }));
            }

            var resolvedIssues = new List<(KittingIssueDetail Issue, int AvailableInventoryId, decimal Quantity)>();
            var missingInventories = new List<string>();

            foreach (var issue in issueDetails)
            {
                var availableInventory = await ResolveAvailableInventoryForSalidaAsync(issue);
                if (availableInventory is null)
                {
                    missingInventories.Add(DescribeSalidaIssue(issue));
                    continue;
                }

                var quantity = ResolveSalidaQuantity(issue, availableInventory);
                if (quantity <= 0m)
                {
                    missingInventories.Add($"{DescribeSalidaIssue(issue)} | sin cantidad disponible.");
                    continue;
                }

                resolvedIssues.Add((issue, availableInventory.AvailableInventoryId, quantity));
            }

            if (missingInventories.Count > 0)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure(
                        "No se pudo localizar inventario disponible para dar salida.",
                        missingInventories.Distinct(StringComparer.OrdinalIgnoreCase).ToList()));
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var now = DateTime.Now;
                var inventoryIds = resolvedIssues
                    .Select(x => x.AvailableInventoryId)
                    .Distinct()
                    .ToList();

                var inventories = await _context.AvailableInventories
                    .Where(x => inventoryIds.Contains(x.AvailableInventoryId))
                    .ToListAsync();

                if (inventories.Count != inventoryIds.Count)
                {
                    await transaction.RollbackAsync();
                    return ResultExtensions.ToActionResult(
                        Result<string>.Failure("No se encontro el inventario disponible para actualizar.", new List<string> { "No se encontro el inventario disponible para actualizar." }, 404));
                }

                var inventoryById = inventories.ToDictionary(x => x.AvailableInventoryId);
                var updatedInventories = new HashSet<int>();

                foreach (var item in resolvedIssues)
                {
                    if (!inventoryById.TryGetValue(item.AvailableInventoryId, out var inventory))
                    {
                        await transaction.RollbackAsync();
                        return ResultExtensions.ToActionResult(
                            Result<string>.Failure("No se encontro el inventario disponible para actualizar.", new List<string> { "No se encontro el inventario disponible para actualizar." }, 404));
                    }

                    _context.InventoryMovements.Add(
                        BuildSalidaMovement(
                            item.Issue,
                            inventory,
                            deliveryOrder.ClientId,
                            deliveryOrder.ProjectId,
                            resolvedDeliveryOrderCode,
                            now,
                            CurrentUserId,
                            item.Quantity,
                            MovementType_e.Venta));

                    _context.InventoryMovements.Add(
                        BuildSalidaMovement(
                            item.Issue,
                            inventory,
                            deliveryOrder.ClientId,
                            deliveryOrder.ProjectId,
                            resolvedDeliveryOrderCode,
                            now,
                            CurrentUserId,
                            item.Quantity,
                            MovementType_e.Compra));

                    if (updatedInventories.Add(inventory.AvailableInventoryId))
                    {
                        inventory.AvailableReference = Truncate(resolvedDeliveryOrderCode, 30);
                        inventory.AvailableStatus = AvailableStatusSalida;
                        inventory.LastModifiedAt = now;
                        inventory.LastModifiedByUserId = CurrentUserId;
                    }
                }

                foreach (var kitting in kittings)
                {
                    kitting.Status = EmbarcadoStatus;
                    kitting.LastModifiedAt = now;
                    kitting.LastModifiedByUserId = CurrentUserId;
                }

                deliveryOrder.Status = EmbarcadoStatus;
                deliveryOrder.LastModifiedAt = now;
                deliveryOrder.LastModifiedByUserId = CurrentUserId;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return ResultExtensions.ToActionResult(
                    Result<string>.Success(
                        resolvedDeliveryOrderCode,
                        $"Salida generada correctamente para la orden {resolvedDeliveryOrderCode}."));
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
                Result<string>.Failure("Hubo un error al dar salida a la orden de entrega.", new List<string> { ex.Message }));
        }
    }

    [HttpPost("finish-loading")]
    [Permission(PermissionKeys.Auditing_View)]
    public async Task<IActionResult> FinishLoading([FromBody] FinishDeliveryOrderLoadingRequest request)
    {
        try
        {
            var deliveryOrderCode = request.DeliveryOrderCode?.Trim();
            if (string.IsNullOrWhiteSpace(deliveryOrderCode))
            {
                return ResultExtensions.ToActionResult(
                    Result<FinishDeliveryOrderLoadingResultDto>.Failure(
                        "Debes seleccionar una orden de entrega.",
                        new List<string> { "Debes seleccionar una orden de entrega." }));
            }

            var deliveryOrder = await _context.DeliveryOrders
                .Include(x => x.DeliveryOrderKittings)
                    .ThenInclude(x => x.Kitting)
                .FirstOrDefaultAsync(x =>
                    x.DeliveryOrderCode == deliveryOrderCode ||
                    x.PreDeliveryOrderCode == deliveryOrderCode);

            if (deliveryOrder is null)
            {
                return ResultExtensions.ToActionResult(
                    Result<FinishDeliveryOrderLoadingResultDto>.Failure(
                        "No se encontro la orden de entrega seleccionada.",
                        new List<string> { "No se encontro la orden de entrega seleccionada." },
                        404));
            }

            if (string.Equals(deliveryOrder.Status?.Trim(), EmbarcadoStatus, StringComparison.OrdinalIgnoreCase))
            {
                return ResultExtensions.ToActionResult(
                    Result<FinishDeliveryOrderLoadingResultDto>.Failure(
                        "La orden de entrega ya fue embarcada.",
                        new List<string> { "La orden de entrega ya fue embarcada." },
                        409));
            }

            var kittings = deliveryOrder.DeliveryOrderKittings
                .Where(x => x.Kitting != null)
                .OrderBy(x => x.SortOrder)
                .Select(x => x.Kitting!)
                .GroupBy(x => x.KittingId)
                .Select(group => group.First())
                .ToList();

            if (kittings.Count == 0)
            {
                return ResultExtensions.ToActionResult(
                    Result<FinishDeliveryOrderLoadingResultDto>.Failure(
                        "La orden de entrega no tiene kittings relacionados.",
                        new List<string> { "La orden de entrega no tiene kittings relacionados." }));
            }

            var kittingIds = kittings.Select(x => x.KittingId).Distinct().ToList();

            var loadMappingIds = await _context.LoadMappings
                .AsNoTracking()
                .Where(x => x.DeliveryOrderCode == deliveryOrderCode)
                .Select(x => x.LoadMappingId)
                .ToListAsync();

            var blockedScans = await _context.LoadMappingScans
                .AsNoTracking()
                .Where(x =>
                    x.IsActive &&
                    !x.IsSuccess &&
                    loadMappingIds.Contains(x.LoadMappingId))
                .OrderBy(x => x.ScannedAt)
                .ThenBy(x => x.LoadMappingScanId)
                .ToListAsync();

            if (blockedScans.Count > 0)
            {
                var scanErrors = blockedScans
                    .Select(DescribeBlockedScan)
                    .Where(message => !string.IsNullOrWhiteSpace(message))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                return ResultExtensions.ToActionResult(
                    Result<FinishDeliveryOrderLoadingResultDto>.Failure(
                        "Hay tarjetas de escaneo con error sin eliminar. No se cerró la orden de entrega.",
                        scanErrors));
            }

            var detailRows = await _context.KittingDetails
                .AsNoTracking()
                .Where(x => kittingIds.Contains(x.KittingId))
                .Select(x => new
                {
                    x.KittingDetailId,
                    x.KittingId
                })
                .ToListAsync();

            var detailIdToKittingId = detailRows
                .GroupBy(x => x.KittingDetailId)
                .ToDictionary(group => group.Key, group => group.First().KittingId);

            var issueDetails = await _context.KittingIssueDetails
                .AsNoTracking()
                .Include(x => x.StandardLabel)
                .Where(x => x.DeliveryOrderId == deliveryOrder.DeliveryOrderId)
                .ToListAsync();

            var validationResults = new List<FinishDeliveryOrderKittingResultDto>();
            var validationErrors = new List<string>();

            foreach (var kitting in kittings)
            {
                var kittingIssueDetails = issueDetails
                    .Where(issue => detailIdToKittingId.TryGetValue(issue.KittingDetailId, out var mappedKittingId) &&
                                    mappedKittingId == kitting.KittingId)
                    .ToList();

                var loadedIssues = kittingIssueDetails.Count(issue => KittingStatusNames.IsLoaded(issue.SupplyStatus));
                var missingLabels = kittingIssueDetails
                    .Where(issue => !KittingStatusNames.IsLoaded(issue.SupplyStatus))
                    .Select(ResolveMissingLabel)
                    .Where(label => !string.IsNullOrWhiteSpace(label))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                if (kittingIssueDetails.Count == 0)
                    missingLabels.Add("Sin etiquetas cargadas");

                validationResults.Add(new FinishDeliveryOrderKittingResultDto
                {
                    KittingId = kitting.KittingId,
                    KittingCode = kitting.KittingCode ?? kitting.PreKittingCode ?? kitting.KittingId.ToString(),
                    Status = missingLabels.Count > 0 ? KittingStatusNames.CargadoParcial : KittingStatusNames.Cargado,
                    TotalIssues = kittingIssueDetails.Count,
                    LoadedIssues = loadedIssues,
                    MissingLabels = missingLabels
                });

                if (missingLabels.Count > 0)
                {
                    // Se permite cerrar con parciales; no bloquear por etiquetas faltantes.
                }
            }

            if (validationErrors.Count > 0)
            {
                return ResultExtensions.ToActionResult(
                    Result<FinishDeliveryOrderLoadingResultDto>.Failure(
                        "Hay kittings con etiquetas pendientes. No se cerró la orden de entrega.",
                        validationErrors));
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var now = DateTime.Now;
                var validationByKittingId = validationResults.ToDictionary(x => x.KittingId);

                foreach (var kitting in kittings)
                {
                    if (!validationByKittingId.TryGetValue(kitting.KittingId, out var kittingValidation))
                        continue;

                    kitting.Status = kittingValidation.Status;
                    kitting.LastModifiedAt = now;
                    kitting.LastModifiedByUserId = CurrentUserId;
                }

                deliveryOrder.Status = KittingStatusNames.Cargado;
                deliveryOrder.LastModifiedAt = now;
                deliveryOrder.LastModifiedByUserId = CurrentUserId;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var response = new FinishDeliveryOrderLoadingResultDto
                {
                    DeliveryOrderCode = deliveryOrder.DeliveryOrderCode ?? deliveryOrder.PreDeliveryOrderCode ?? deliveryOrder.DeliveryOrderId.ToString(),
                    DeliveryOrderStatus = deliveryOrder.Status ?? string.Empty,
                    TotalKittings = validationResults.Count,
                    LoadedKittings = validationResults.Count(x => !x.IsPartial),
                    PartialKittings = validationResults.Count(x => x.IsPartial),
                    Kittings = validationResults
                };

                return ResultExtensions.ToActionResult(
                    Result<FinishDeliveryOrderLoadingResultDto>.Success(
                        response,
                        response.HasPartials
                            ? "Orden de entrega cerrada con parciales."
                            : "Orden de entrega cerrada correctamente."));
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
                Result<FinishDeliveryOrderLoadingResultDto>.Failure(
                    "Hubo un error al cerrar la orden de entrega.",
                    new List<string> { ex.Message }));
        }
    }

    private static string DescribeBlockedScan(LoadMappingScan scan)
    {
        var standardId = string.IsNullOrWhiteSpace(scan.StandardId) ? "Sin StandardId" : scan.StandardId;
        var kitting = string.IsNullOrWhiteSpace(scan.Kitting) ? "Sin kitting" : scan.Kitting;
        var partNumber = string.IsNullOrWhiteSpace(scan.PartNumber) ? string.Empty : $" - {scan.PartNumber}";
        var side = string.IsNullOrWhiteSpace(scan.Side) ? string.Empty : $"[{scan.Side}] ";
        var message = string.IsNullOrWhiteSpace(scan.Message) ? "Escaneo rechazado." : scan.Message.Trim();

        return $"{side}{standardId}{partNumber} | {kitting}: {message}";
    }

    private async Task AssignDeliveryOrderToIssueDetailsAsync(List<int> kittingIds, int deliveryOrderId)
    {
        if (kittingIds.Count == 0 || deliveryOrderId <= 0)
            return;

        var kittingDetailIds = await _context.KittingDetails
            .AsNoTracking()
            .Where(x => kittingIds.Contains(x.KittingId))
            .Select(x => x.KittingDetailId)
            .ToListAsync();

        if (kittingDetailIds.Count == 0)
            return;

        var issueDetails = await _context.KittingIssueDetails
            .Where(x => kittingDetailIds.Contains(x.KittingDetailId))
            .ToListAsync();

        foreach (var issueDetail in issueDetails)
        {
            issueDetail.DeliveryOrderId = deliveryOrderId;
        }
    }

    private async Task<AvailableInventory?> ResolveAvailableInventoryForSalidaAsync(KittingIssueDetail issue)
    {
        if (!issue.StandardId.HasValue || issue.StandardId.Value <= 0)
            return null;

        var inventories = _context.AvailableInventories
            .AsNoTracking()
            .Where(x => x.StandardId == issue.StandardId.Value);

        var exactMatch = await inventories.FirstOrDefaultAsync(x =>
            x.PartNumber == issue.PartNumber &&
            x.ProductId == issue.ProductId &&
            x.LocationId == issue.LocationId &&
            x.LotNumber == issue.LotNumber &&
            x.Reference == issue.Reference &&
            x.PurchaseOrder == issue.PurchaseOrder &&
            x.CustomsDeclarationNumber == issue.CustomsDeclarationNumber);

        if (exactMatch is not null)
            return exactMatch;

        return await inventories
            .Where(x => x.PartNumber == issue.PartNumber)
            .Where(x => !issue.ProductId.HasValue || issue.ProductId <= 0 || x.ProductId == issue.ProductId)
            .Where(x => !issue.LocationId.HasValue || issue.LocationId <= 0 || x.LocationId == issue.LocationId)
            .OrderByDescending(x => x.Fecha)
            .ThenByDescending(x => x.Hora)
            .ThenByDescending(x => x.AvailableInventoryId)
            .FirstOrDefaultAsync();
    }

    private static decimal ResolveSalidaQuantity(KittingIssueDetail issue, AvailableInventory inventory)
    {
        if (issue.ReceivedQuantity.HasValue && issue.ReceivedQuantity.Value > 0m)
            return issue.ReceivedQuantity.Value;

        if (inventory.Supply > 0m)
            return inventory.Supply;

        if (inventory.FinalAvailable > 0m)
            return inventory.FinalAvailable;

        return inventory.Qty.GetValueOrDefault();
    }

    private static InventoryMovement BuildSalidaMovement(
        KittingIssueDetail issue,
        AvailableInventory inventory,
        int clientId,
        int projectId,
        string deliveryOrderCode,
        DateTime now,
        string userId,
        decimal quantity,
        MovementType_e movementType)
    {
        return new InventoryMovement
        {
            ProductId = issue.ProductId ?? inventory.ProductId,
            ClientId = clientId,
            ProjectId = projectId,
            PartNumber = issue.PartNumber?.Trim() ?? inventory.PartNumber.Trim(),
            Description = issue.Description?.Trim() ?? inventory.Description?.Trim(),
            Fecha = now,
            Hora = now.TimeOfDay,
            UserId = userId,
            LotNumber = issue.LotNumber?.Trim() ?? inventory.LotNumber?.Trim(),
            PalletNumber = inventory.PalletNumber,
            Reference = issue.Reference?.Trim() ?? inventory.Reference?.Trim(),
            PurchaseOrder = issue.PurchaseOrder?.Trim() ?? inventory.PurchaseOrder?.Trim(),
            CustomsDeclarationNumber = issue.CustomsDeclarationNumber?.Trim() ?? inventory.CustomsDeclarationNumber?.Trim(),
            ExpirationDate = issue.ExpirationDate ?? inventory.ExpirationDate,
            DocumentType = DocumentType_e.Salida,
            MovementType = movementType,
            DocumentId = deliveryOrderCode.Trim(),
            StatusId = inventory.StatusId?.Trim(),
            LocationId = inventory.LocationId,
            Qty = movementType == MovementType_e.Venta
                ? -Math.Abs(quantity)
                : Math.Abs(quantity),
            StandardId = issue.StandardId ?? inventory.StandardId
        };
    }

    private static string DescribeSalidaIssue(KittingIssueDetail issue)
    {
        var standardId = issue.StandardId?.ToString() ?? "Sin StandardId";
        var partNumber = string.IsNullOrWhiteSpace(issue.PartNumber)
            ? "Sin parte"
            : issue.PartNumber.Trim();

        return $"{standardId} - {partNumber} ({issue.KittingReceiptDetailId})";
    }

    private static string Truncate(string? value, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        var trimmed = value.Trim();
        return trimmed.Length <= maxLength
            ? trimmed
            : trimmed[..maxLength];
    }

    private static string GetDeliveryOrderPrefix(Project project)
    {
        if (!string.IsNullOrWhiteSpace(project.DoPrefix))
            return project.DoPrefix.Trim();

        if (!string.IsNullOrWhiteSpace(project.DeliveryOrderPrefix))
            return project.DeliveryOrderPrefix.Trim();

        return string.Empty;
    }

    private static int GetDeliveryOrderNumber(Project project)
    {
        if (!string.IsNullOrWhiteSpace(project.DoNumber) &&
            int.TryParse(project.DoNumber, out var doNumber))
        {
            return doNumber;
        }

        if (!string.IsNullOrWhiteSpace(project.DeliveryOrderNumber) &&
            int.TryParse(project.DeliveryOrderNumber, out var deliveryOrderNumber))
        {
            return deliveryOrderNumber;
        }

        return 1;
    }

    private static void SetDeliveryOrderNumber(Project project, int nextNumber)
    {
        var value = nextNumber.ToString();
        project.DoNumber = value;
        project.DeliveryOrderNumber = value;
    }

    private static string ResolveMissingLabel(KittingIssueDetail issue)
    {
        var standardId = issue.StandardLabel?.StandarIdStr?.Trim();
        if (!string.IsNullOrWhiteSpace(standardId))
        {
            return string.IsNullOrWhiteSpace(issue.PartNumber)
                ? standardId
                : $"{standardId} ({issue.PartNumber.Trim()})";
        }

        if (issue.StandardId.HasValue && issue.StandardId.Value > 0)
        {
            return string.IsNullOrWhiteSpace(issue.PartNumber)
                ? issue.StandardId.Value.ToString()
                : $"{issue.StandardId.Value} ({issue.PartNumber.Trim()})";
        }

        return string.IsNullOrWhiteSpace(issue.PartNumber)
            ? issue.KittingReceiptDetailId.ToString()
            : issue.PartNumber.Trim();
    }
}
