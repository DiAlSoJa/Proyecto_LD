using LD.Application.Common.Interfaces.Persistence;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.AvailableInventory;
using LD.Domain.Entities;
using LD.Domain.Enums;
using MediatR;

namespace LD.Application.Features.AvailableInventories.Commands;

public class ChangeInventoryStatusCommand : ChangeInventoryStatusRequest, IRequest<Result<string>>
{
    public string UserId { get; set; } = string.Empty;
}

public class ChangeInventoryStatusCommandHandler : IRequestHandler<ChangeInventoryStatusCommand, Result<string>>
{
    private readonly IRepository<AvailableInventory> _availableInventoryRepository;
    private readonly IRepository<LD.Domain.Entities.InventoryMovement> _inventoryMovementRepository;
    private readonly IRepository<LD.Domain.Entities.InventaryStatus> _statusRepository;
    private readonly ITransactionManager _transactionManager;

    public ChangeInventoryStatusCommandHandler(
        IRepository<AvailableInventory> availableInventoryRepository,
        IRepository<LD.Domain.Entities.InventoryMovement> inventoryMovementRepository,
        IRepository<LD.Domain.Entities.InventaryStatus> statusRepository,
        ITransactionManager transactionManager)
    {
        _availableInventoryRepository = availableInventoryRepository;
        _inventoryMovementRepository = inventoryMovementRepository;
        _statusRepository = statusRepository;
        _transactionManager = transactionManager;
    }

    public async Task<Result<string>> Handle(ChangeInventoryStatusCommand request, CancellationToken cancellationToken)
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

            if (string.IsNullOrWhiteSpace(request.StatusDestino))
                return Result<string>.Failure("El estatus destino es requerido.", new());

            var destinationStatus = await ResolveDestinationStatusAsync(request.StatusDestino);
            if (destinationStatus is null)
                return Result<string>.Failure("No existe el estatus destino indicado.", new(), 404);

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

            var alreadyInDestination = availableInventories
                .Where(x => string.Equals(x.StatusId?.Trim(), destinationStatus.InventoryStatusIdS.Trim(), StringComparison.OrdinalIgnoreCase))
                .Select(x => x.StandardId!.Value)
                .ToList();

            if (alreadyInDestination.Any())
            {
                return Result<string>.Failure(
                    $"El inventario ya tiene el estatus destino para los StandardId: {string.Join(", ", alreadyInDestination)}.",
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
                        availableInventory.StatusId ?? string.Empty);

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
                        destinationStatus.InventoryStatusIdS);

                    var purchaseCreated = await _inventoryMovementRepository.CreateAsync(purchaseMovement);
                    if (!purchaseCreated)
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        return Result<string>.Failure($"No se pudo crear el movimiento de compra para el StandardId {availableInventory.StandardId}.", new());
                    }

                    availableInventory.StatusId = destinationStatus.InventoryStatusIdS;
                    var updated = await _availableInventoryRepository.UpdateAsync(availableInventory);
                    if (!updated)
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        return Result<string>.Failure($"No se pudo actualizar el estatus del inventario disponible para el StandardId {availableInventory.StandardId}.", new());
                    }
                }

                await transaction.CommitAsync(cancellationToken);
                return Result<string>.Success(string.Join(", ", standardIds), "Estatus actualizado correctamente.");
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al cambiar el estatus.", new() { ex.Message });
        }
    }

    private async Task<LD.Domain.Entities.InventaryStatus?> ResolveDestinationStatusAsync(string statusDestino)
    {
        var value = statusDestino.Trim();
        var statuses = await _statusRepository.GetManyAsync() ?? new List<LD.Domain.Entities.InventaryStatus>();

        return statuses.FirstOrDefault(x =>
            string.Equals(x.InventoryStatusIdS?.Trim(), value, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(x.FullName?.Trim(), value, StringComparison.OrdinalIgnoreCase));
    }

    private static LD.Domain.Entities.InventoryMovement CreateMovementFromInventory(
        AvailableInventory inventory,
        DateTime movementDate,
        string userId,
        string documentId,
        MovementType_e movementType,
        string statusId)
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
            StatusId = statusId,
            LocationId = inventory.LocationId,
            Qty = inventory.Qty,
            StandardId = inventory.StandardId
        };

        if (movementType == MovementType_e.Venta && movement.Qty.HasValue)
            movement.Qty = -Math.Abs(movement.Qty.Value);

        return movement;
    }
}
