using LD.Application.Common.Interfaces.Persistence;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.AvailableInventory;
using LD.Domain.Entities;
using LD.Domain.Enums;
using MediatR;

namespace LD.Application.Features.AvailableInventories.Commands;

public class ChangeInventoryWarehouseCommand : ChangeInventoryWarehouseRequest, IRequest<Result<string>>
{
    public string UserId { get; set; } = string.Empty;
}

public class ChangeInventoryWarehouseCommandHandler : IRequestHandler<ChangeInventoryWarehouseCommand, Result<string>>
{
    private readonly IRepository<AvailableInventory> _availableInventoryRepository;
    private readonly IRepository<LD.Domain.Entities.InventoryMovement> _inventoryMovementRepository;
    private readonly IRepository<Warehouse> _warehouseRepository;
    private readonly IRepository<Location> _locationRepository;
    private readonly ITransactionManager _transactionManager;

    public ChangeInventoryWarehouseCommandHandler(
        IRepository<AvailableInventory> availableInventoryRepository,
        IRepository<LD.Domain.Entities.InventoryMovement> inventoryMovementRepository,
        IRepository<Warehouse> warehouseRepository,
        IRepository<Location> locationRepository,
        ITransactionManager transactionManager)
    {
        _availableInventoryRepository = availableInventoryRepository;
        _inventoryMovementRepository = inventoryMovementRepository;
        _warehouseRepository = warehouseRepository;
        _locationRepository = locationRepository;
        _transactionManager = transactionManager;
    }

    public async Task<Result<string>> Handle(ChangeInventoryWarehouseCommand request, CancellationToken cancellationToken)
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

            if (request.WarehouseId <= 0)
                return Result<string>.Failure("El almacen destino es requerido.", new());

            if (string.IsNullOrWhiteSpace(request.UbicacionDestino))
                return Result<string>.Failure("La ubicacion destino es requerida.", new());

            var destinationWarehouse = await _warehouseRepository.GetByIdAsync(request.WarehouseId);
            if (destinationWarehouse is null)
                return Result<string>.Failure("No existe el almacen destino indicado.", new(), 404);

            var destinationLocation = await ResolveDestinationLocationAsync(request.UbicacionDestino, destinationWarehouse.WarehouseId);
            if (destinationLocation is null)
                return Result<string>.Failure("No existe la ubicacion destino indicada para el almacen seleccionado.", new(), 404);

            var locations = await _locationRepository.GetManyAsync() ?? new List<Location>();
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

            var alreadyInDestinationLocation = availableInventories
                .Where(x => x.LocationId == destinationLocation.LocationId)
                .Select(x => x.StandardId!.Value)
                .ToList();

            if (alreadyInDestinationLocation.Any())
            {
                return Result<string>.Failure(
                    $"El inventario ya se encuentra en la ubicacion destino para los StandardId: {string.Join(", ", alreadyInDestinationLocation)}.",
                    new());
            }

            var alreadyInDestinationWarehouse = availableInventories
                .Where(x => locations.Any(location =>
                    location.LocationId == x.LocationId &&
                    location.WarehouseId == destinationWarehouse.WarehouseId))
                .Select(x => x.StandardId!.Value)
                .ToList();

            if (alreadyInDestinationWarehouse.Any())
            {
                return Result<string>.Failure(
                    $"El inventario ya se encuentra en el almacen destino para los StandardId: {string.Join(", ", alreadyInDestinationWarehouse)}.",
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
                        return Result<string>.Failure($"No se pudo actualizar el almacen del inventario disponible para el StandardId {availableInventory.StandardId}.", new());
                    }
                }

                await transaction.CommitAsync(cancellationToken);
                return Result<string>.Success(string.Join(", ", standardIds), "Almacen actualizado correctamente.");
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al cambiar el almacen.", new() { ex.Message });
        }
    }

    private async Task<Location?> ResolveDestinationLocationAsync(string ubicacionDestino, int warehouseId)
    {
        var value = ubicacionDestino.Trim();
        var locations = await _locationRepository.GetManyAsync() ?? new List<Location>();

        if (int.TryParse(value, out var locationId))
        {
            var locationById = locations.FirstOrDefault(x =>
                x.LocationId == locationId &&
                x.WarehouseId == warehouseId);
            if (locationById is not null)
                return locationById;
        }

        return locations.FirstOrDefault(x =>
            x.WarehouseId == warehouseId &&
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
}
