using LD.Application.Common.Interfaces.Persistence;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.AvailableInventory;
using LD.Contracts.Constants;
using LD.Domain.Entities;
using LD.Domain.Enums;
using MediatR;

namespace LD.Application.Features.AvailableInventories.Commands;

public class ChangeInventoryLocationCommand : ChangeInventoryLocationRequest, IRequest<Result<string>>
{
    public string UserId { get; set; } = string.Empty;
}

public class ChangeInventoryLocationCommandHandler : IRequestHandler<ChangeInventoryLocationCommand, Result<string>>
{
    private readonly IRepository<AvailableInventory> _availableInventoryRepository;
    private readonly IRepository<LD.Domain.Entities.InventoryMovement> _inventoryMovementRepository;
    private readonly IRepository<KittingIssueDetail> _kittingIssueRepository;
    private readonly IRepository<KittingDetail> _kittingDetailRepository;
    private readonly IRepository<LD.Domain.Entities.Kitting> _kittingRepository;
    private readonly IRepository<Location> _locationRepository;
    private readonly ITransactionManager _transactionManager;

    public ChangeInventoryLocationCommandHandler(
        IRepository<AvailableInventory> availableInventoryRepository,
        IRepository<LD.Domain.Entities.InventoryMovement> inventoryMovementRepository,
        IRepository<KittingIssueDetail> kittingIssueRepository,
        IRepository<KittingDetail> kittingDetailRepository,
        IRepository<LD.Domain.Entities.Kitting> kittingRepository,
        IRepository<Location> locationRepository,
        ITransactionManager transactionManager)
    {
        _availableInventoryRepository = availableInventoryRepository;
        _inventoryMovementRepository = inventoryMovementRepository;
        _kittingIssueRepository = kittingIssueRepository;
        _kittingDetailRepository = kittingDetailRepository;
        _kittingRepository = kittingRepository;
        _locationRepository = locationRepository;
        _transactionManager = transactionManager;
    }

    public async Task<Result<string>> Handle(ChangeInventoryLocationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.UserId))
                return Result<string>.Failure("No se pudo identificar el usuario actual.", new(), 401);

            var standardIds = (request.StandardIds ?? new List<int>())
                .Append(request.StandardId)
                .Where(x => x > 0)
                .Distinct()
                .ToList();

            if (!standardIds.Any())
                return Result<string>.Failure("El StandardId es requerido.", new());

            if (string.IsNullOrWhiteSpace(request.UbicacionDestino))
                return Result<string>.Failure("La ubicacion destino es requerida.", new());

            var availableInventories = (await _availableInventoryRepository.GetManyAsync() ?? new List<AvailableInventory>())
                .Where(x => x.StandardId.HasValue && standardIds.Contains(x.StandardId.Value))
                .ToList();

            var foundStandardIds = availableInventories
                .Where(x => x.StandardId.HasValue)
                .Select(x => x.StandardId!.Value)
                .ToHashSet();

            var missingStandardIds = standardIds
                .Where(x => !foundStandardIds.Contains(x))
                .ToList();

            if (missingStandardIds.Any())
            {
                return Result<string>.Failure(
                    $"No existe inventario disponible para los StandardId: {string.Join(", ", missingStandardIds)}.",
                    new(),
                    404);
            }

            var destinationLocation = await ResolveDestinationLocationAsync(request.UbicacionDestino);
            if (destinationLocation is null)
                return Result<string>.Failure("No existe la ubicacion destino indicada.", new(), 404);

            var alreadyInDestination = availableInventories
                .Where(x => x.LocationId == destinationLocation.LocationId)
                .Select(x => x.StandardId!.Value)
                .ToList();

            if (alreadyInDestination.Any())
            {
                return Result<string>.Failure(
                    $"El inventario ya se encuentra en la ubicacion destino para los StandardId: {string.Join(", ", alreadyInDestination)}.",
                    new());
            }

            await using var transaction = await _transactionManager.BeginTransactionAsync(cancellationToken);

            try
            {
                var now = DateTime.Now;

                foreach (var availableInventory in availableInventories)
                {
                    var documentId = string.IsNullOrWhiteSpace(availableInventory.DocumentId)
                        ? availableInventory.StandardId?.ToString() ?? string.Empty
                        : availableInventory.DocumentId;

                    var saleMovement = CreateMovementFromInventory(
                        availableInventory,
                        now,
                        request.UserId,
                        documentId,
                        MovementType_e.Venta,
                        availableInventory.LocationId);

                    var saleCreated = await _inventoryMovementRepository.CreateAsync(saleMovement);
                    if (!saleCreated)
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        return Result<string>.Failure($"No se pudo crear el movimiento de venta para el StandardId {availableInventory.StandardId}.", new());
                    }

                    var purchaseMovement = CreateMovementFromInventory(
                        availableInventory,
                        now,
                        request.UserId,
                        documentId,
                        MovementType_e.Compra,
                        destinationLocation.LocationId);

                    var purchaseCreated = await _inventoryMovementRepository.CreateAsync(purchaseMovement);
                    if (!purchaseCreated)
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        return Result<string>.Failure($"No se pudo crear el movimiento de compra para el StandardId {availableInventory.StandardId}.", new());
                    }

                    availableInventory.LocationId = destinationLocation.LocationId;
                    var updated = await _availableInventoryRepository.UpdateAsync(availableInventory);
                    if (!updated)
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        return Result<string>.Failure($"No se pudo actualizar la ubicacion del inventario disponible para el StandardId {availableInventory.StandardId}.", new());
                    }
                }

                var kittingFinalized = false;
                if (request.KittingReceiptDetailId.GetValueOrDefault() > 0)
                {
                    var kittingResult = await UpdateKittingFlowAsync(request, destinationLocation, now);
                    if (kittingResult.Failure is not null)
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        return kittingResult.Failure;
                    }

                    kittingFinalized = kittingResult.Finalized;
                }

                await transaction.CommitAsync(cancellationToken);
                var successMessage = kittingFinalized
                    ? "Ubicacion actualizada correctamente. Kitting surtido correctamente."
                    : "Ubicacion actualizada correctamente.";

                return Result<string>.Success(string.Join(", ", standardIds), successMessage);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al cambiar la ubicacion.", new() { ex.Message });
        }
    }

    private async Task<Location?> ResolveDestinationLocationAsync(string ubicacionDestino)
    {
        var value = ubicacionDestino.Trim();
        var locations = await _locationRepository.GetManyAsync() ?? new List<Location>();

        if (int.TryParse(value, out var locationId))
        {
            var locationById = locations.FirstOrDefault(x => x.LocationId == locationId);
            if (locationById is not null)
                return locationById;
        }

        return locations.FirstOrDefault(x =>
            string.Equals(x.LocationName?.Trim(), value, StringComparison.OrdinalIgnoreCase));
    }

    private static LD.Domain.Entities.InventoryMovement CreateMovementFromInventory(
        AvailableInventory inventory,
        DateTime movementDate,
        string userId,
        string documentId,
        MovementType_e movementType,
        int? locationId)
    {
        var movement = new LD.Domain.Entities.InventoryMovement
        {
            ProductId = inventory.ProductId,
            ClientId = inventory.ClientId,
            ProjectId = inventory.ProjectId,
            PartNumber = inventory.PartNumber,
            Description = inventory.Description,
            Fecha = movementDate,
            Hora = movementDate.TimeOfDay,
            UserId = userId,
            LotNumber = inventory.LotNumber,
            Reference = inventory.Reference,
            PurchaseOrder = inventory.PurchaseOrder,
            CustomsDeclarationNumber = inventory.CustomsDeclarationNumber,
            ExpirationDate = inventory.ExpirationDate,
            DocumentType = DocumentType_e.Transferencia,
            MovementType = movementType,
            DocumentId = documentId,
            StatusId = inventory.StatusId,
            LocationId = locationId,
            Qty = inventory.Qty,
            StandardId = inventory.StandardId
        };

        if (movementType == MovementType_e.Venta && movement.Qty.HasValue)
            movement.Qty = -Math.Abs(movement.Qty.Value);

        return movement;
    }

    private async Task<(Result<string>? Failure, bool Finalized)> UpdateKittingFlowAsync(
        ChangeInventoryLocationCommand request,
        Location destinationLocation,
        DateTime now)
    {
        var kittingReceiptDetailId = request.KittingReceiptDetailId.GetValueOrDefault();
        if (kittingReceiptDetailId <= 0)
            return (null, false);

        var issueDetail = await _kittingIssueRepository.GetByIdAsync(kittingReceiptDetailId);
        if (issueDetail is null)
        {
            return (Result<string>.Failure(
                    "No existe la linea issue del Kitting.",
                    new List<string> { "No existe la linea issue del Kitting." },
                    404),
                false);
        }

        var kittingDetail = await _kittingDetailRepository.GetByIdAsync(issueDetail.KittingDetailId);
        if (kittingDetail is null)
        {
            return (Result<string>.Failure(
                    "No existe el detalle del Kitting.",
                    new List<string> { "No existe el detalle del Kitting." },
                    404),
                false);
        }

        if (request.KittingId.GetValueOrDefault() > 0 && request.KittingId.Value != kittingDetail.KittingId)
        {
            return (Result<string>.Failure(
                    "La linea issue no pertenece al Kitting seleccionado.",
                    new List<string> { "La linea issue no pertenece al Kitting seleccionado." },
                    400),
                false);
        }

        issueDetail.LocationId = destinationLocation.LocationId;
        issueDetail.LocationCode = destinationLocation.LocationName?.Trim() ?? request.UbicacionDestino.Trim();
        issueDetail.SupplyStatus = KittingStatusNames.Validacion;
        issueDetail.LastModifiedAt = now;
        issueDetail.LastModifiedByUserId = request.UserId;

        var issueUpdated = await _kittingIssueRepository.UpdateAsync(issueDetail);
        if (!issueUpdated)
        {
            return (Result<string>.Failure(
                    "No se pudo actualizar la linea issue del Kitting.",
                    new List<string> { "No se pudo actualizar la linea issue del Kitting." }),
                false);
        }

        kittingDetail.CantidadSurtida = await CalculateCantidadSurtidaAsync(kittingDetail.KittingDetailId);
        kittingDetail.LastModifiedAt = now;
        kittingDetail.LastModifiedByUserId = request.UserId;

        var detailUpdated = await _kittingDetailRepository.UpdateAsync(kittingDetail);
        if (!detailUpdated)
        {
            return (Result<string>.Failure(
                    "No se pudo actualizar el detalle del Kitting.",
                    new List<string> { "No se pudo actualizar el detalle del Kitting." }),
                false);
        }

        var effectiveKittingId = request.KittingId.GetValueOrDefault() > 0
            ? request.KittingId!.Value
            : kittingDetail.KittingId;

        if (effectiveKittingId <= 0)
            return (null, false);

        var finalized = await AreAllIssueLinesValidacionAsync(effectiveKittingId);
        if (!finalized)
            return (null, false);

        var kitting = await _kittingRepository.GetByIdAsync(effectiveKittingId);
        if (kitting is null)
        {
            return (Result<string>.Failure(
                    "No existe el Kitting.",
                    new List<string> { "No existe el Kitting." },
                    404),
                false);
        }

        if (!KittingStatusNames.IsValidation(kitting.Status))
        {
            kitting.Status = KittingStatusNames.Validacion;
            kitting.LastModifiedAt = now;
            kitting.LastModifiedByUserId = request.UserId;

            var kittingUpdated = await _kittingRepository.UpdateAsync(kitting);
            if (!kittingUpdated)
            {
                return (Result<string>.Failure(
                        "No se pudo actualizar el estatus del Kitting.",
                        new List<string> { "No se pudo actualizar el estatus del Kitting." }),
                    false);
            }
        }

        return (null, true);
    }

    private async Task<decimal> CalculateCantidadSurtidaAsync(int kittingDetailId)
    {
        var issueDetails = await _kittingIssueRepository.GetManyAsync() ?? new List<KittingIssueDetail>();
        return issueDetails
            .Where(x => x.KittingDetailId == kittingDetailId)
            .Sum(x => x.ReceivedQuantity ?? 0m);
    }

    private async Task<bool> AreAllIssueLinesValidacionAsync(int kittingId)
    {
        var detailIds = (await _kittingDetailRepository.GetManyAsync() ?? new List<KittingDetail>())
            .Where(x => x.KittingId == kittingId)
            .Select(x => x.KittingDetailId)
            .ToList();

        if (detailIds.Count == 0)
            return false;

        var issueDetails = (await _kittingIssueRepository.GetManyAsync() ?? new List<KittingIssueDetail>())
            .Where(x => detailIds.Contains(x.KittingDetailId))
            .ToList();

        if (issueDetails.Count == 0)
            return false;

        return issueDetails.All(x => KittingStatusNames.IsValidation(x.SupplyStatus));
    }
}
