using LD.Application.Common.Interfaces.Persistence;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Domain.Entities;
using LD.Domain.Enums;
using MediatR;

namespace LD.Application.Features.Asn.Commands;

public class LocateAsnPalletCommand : IRequest<Result<string>>
{
    public int AsnId { get; set; }
    public int StandardId { get; set; }
    public string UbicacionDestino { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
}

public class LocateAsnPalletCommandHandler : IRequestHandler<LocateAsnPalletCommand, Result<string>>
{
    private readonly IAsnRepository _asnRepository;
    private readonly IRepository<LD.Domain.Entities.AsnDetail> _asnDetailRepository;
    private readonly IRepository<AsnReceiptDetail> _asnReceiptDetailRepository;
    private readonly IRepository<LD.Domain.Entities.InventoryMovement> _inventoryMovementRepository;
    private readonly IRepository<AvailableInventory> _availableInventoryRepository;
    private readonly IRepository<Location> _locationRepository;
    private readonly ITransactionManager _transactionManager;

    public LocateAsnPalletCommandHandler(
        IAsnRepository asnRepository,
        IRepository<LD.Domain.Entities.AsnDetail> asnDetailRepository,
        IRepository<AsnReceiptDetail> asnReceiptDetailRepository,
        IRepository<LD.Domain.Entities.InventoryMovement> inventoryMovementRepository,
        IRepository<AvailableInventory> availableInventoryRepository,
        IRepository<Location> locationRepository,
        ITransactionManager transactionManager)
    {
        _asnRepository = asnRepository;
        _asnDetailRepository = asnDetailRepository;
        _asnReceiptDetailRepository = asnReceiptDetailRepository;
        _inventoryMovementRepository = inventoryMovementRepository;
        _availableInventoryRepository = availableInventoryRepository;
        _locationRepository = locationRepository;
        _transactionManager = transactionManager;
    }

    public async Task<Result<string>> Handle(LocateAsnPalletCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.UserId))
                return Result<string>.Failure("No se pudo identificar el usuario actual.", new(), 401);

            if (request.StandardId <= 0)
                return Result<string>.Failure("El StandardId es requerido.", new());

            if (string.IsNullOrWhiteSpace(request.UbicacionDestino))
                return Result<string>.Failure("La ubicacion destino es requerida.", new());

            var asn = await _asnRepository.GetByIdAsync(request.AsnId);
            if (asn is null)
                return Result<string>.Failure("ASN no encontrado.", new(), 404);

            if (!string.Equals(asn.Status?.Trim(), "Ubicando", StringComparison.OrdinalIgnoreCase))
                return Result<string>.Failure("El ASN debe estar en estatus Ubicando para mover pallets.", new());

            var destinationLocation = await ResolveDestinationLocationAsync(request.UbicacionDestino);
            if (destinationLocation is null)
                return Result<string>.Failure("No existe la ubicacion destino indicada.", new(), 404);

            var asnDetails = await _asnDetailRepository.GetManyAsync() ?? new List<LD.Domain.Entities.AsnDetail>();
            var asnDetailIds = asnDetails
                .Where(x => x.AsnId == request.AsnId)
                .Select(x => x.AsnDetailId)
                .ToHashSet();

            var receiptDetails = (await _asnReceiptDetailRepository.GetManyAsync() ?? new List<AsnReceiptDetail>())
                .Where(x => asnDetailIds.Contains(x.AsnDetailId))
                .ToList();

            if (!receiptDetails.Any())
                return Result<string>.Failure("El ASN no tiene pallets recibidos.", new());

            var receiptDetail = receiptDetails.FirstOrDefault(x => x.StandardId == request.StandardId);
            if (receiptDetail is null)
                return Result<string>.Failure("El StandardId capturado no pertenece a este ASN.", new(), 404);

            var invalidMessage = ValidateReceiptDetail(receiptDetail);
            if (!string.IsNullOrWhiteSpace(invalidMessage))
                return Result<string>.Failure(invalidMessage, new());

            var existingAvailableInventory = await GetExistingAvailableInventoryAsync(request.StandardId);
            if (existingAvailableInventory is not null)
                return Result<string>.Failure("Este pallet ya fue ingresado al inventario disponible.", new());

            var existingMovement = await GetExistingMovementAsync(request.StandardId);
            if (existingMovement is not null)
                return Result<string>.Failure("Este pallet ya tiene movimiento de compra registrado.", new());

            await using var transaction = await _transactionManager.BeginTransactionAsync(cancellationToken);

            try
            {
                var now = DateTime.Now;
                var documentId = !string.IsNullOrWhiteSpace(asn.AsnCode)
                    ? asn.AsnCode
                    : asn.AsnId.ToString();

                receiptDetail.LocationId = destinationLocation.LocationId;
                receiptDetail.LocationCode = destinationLocation.LocationName;
                receiptDetail.LastModifiedAt = now;
                receiptDetail.LastModifiedByUserId = request.UserId;

                var detailUpdated = await _asnReceiptDetailRepository.UpdateAsync(receiptDetail);
                if (!detailUpdated)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return Result<string>.Failure("No se pudo actualizar la ubicacion del pallet recibido.", new());
                }

                var movement = CreatePurchaseMovement(asn, receiptDetail, now, request.UserId, documentId);
                var movementCreated = await _inventoryMovementRepository.CreateAsync(movement);
                if (!movementCreated)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return Result<string>.Failure("No se pudo generar el movimiento de compra del pallet.", new());
                }

                var availableInventory = CreateAvailableInventory(asn, receiptDetail, now, request.UserId, documentId);
                var availableInventoryCreated = await _availableInventoryRepository.CreateAsync(availableInventory);
                if (!availableInventoryCreated)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return Result<string>.Failure("No se pudo registrar el inventario disponible del pallet.", new());
                }

                if (await AllPalletsLocatedAsync(receiptDetails, request.StandardId))
                {
                    asn.Status = "Confirmado";
                    asn.LastModifiedAt = now;
                    asn.LastModifiedByUserId = request.UserId;

                    var asnUpdated = await _asnRepository.UpdateAsync(asn);
                    if (!asnUpdated)
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        return Result<string>.Failure("El pallet se proceso, pero no se pudo confirmar el ASN.", new());
                    }
                }

                await transaction.CommitAsync(cancellationToken);

                var message = string.Equals(asn.Status, "Confirmado", StringComparison.OrdinalIgnoreCase)
                    ? "Pallet ubicado y ASN confirmado correctamente."
                    : "Pallet ubicado correctamente.";

                return Result<string>.Success(request.StandardId.ToString(), message);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al ubicar el pallet del ASN.", new() { ex.Message });
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

    private async Task<AvailableInventory?> GetExistingAvailableInventoryAsync(int standardId)
    {
        var inventories = await _availableInventoryRepository.GetManyAsync() ?? new List<AvailableInventory>();
        return inventories.FirstOrDefault(x => x.StandardId == standardId);
    }

    private async Task<LD.Domain.Entities.InventoryMovement?> GetExistingMovementAsync(int standardId)
    {
        var movements = await _inventoryMovementRepository.GetManyAsync() ?? new List<LD.Domain.Entities.InventoryMovement>();
        return movements.FirstOrDefault(x =>
            x.StandardId == standardId &&
            x.DocumentType == DocumentType_e.Entrada &&
            x.MovementType == MovementType_e.Compra);
    }

    private async Task<bool> AllPalletsLocatedAsync(IEnumerable<AsnReceiptDetail> receiptDetails, int currentStandardId)
    {
        var standardIds = receiptDetails
            .Select(x => x.StandardId)
            .Where(x => x.HasValue)
            .Select(x => x!.Value)
            .Distinct()
            .ToHashSet();

        if (!standardIds.Any())
            return false;

        var inventories = await _availableInventoryRepository.GetManyAsync() ?? new List<AvailableInventory>();
        var locatedStandardIds = inventories
            .Where(x => x.StandardId.HasValue && standardIds.Contains(x.StandardId.Value))
            .Select(x => x.StandardId!.Value)
            .ToHashSet();

        locatedStandardIds.Add(currentStandardId);

        return standardIds.All(locatedStandardIds.Contains);
    }

    private static string? ValidateReceiptDetail(AsnReceiptDetail detail)
    {
        if (detail.ProductId is null)
            return $"La linea {detail.PartNumber} no tiene ProductId y no se puede generar el movimiento.";

        if (string.IsNullOrWhiteSpace(detail.Status))
            return $"La linea {detail.PartNumber} no tiene estatus.";

        if (string.IsNullOrWhiteSpace(detail.SD))
            return $"La linea {detail.PartNumber} no tiene SD.";

        return null;
    }

    private static LD.Domain.Entities.InventoryMovement CreatePurchaseMovement(
        LD.Domain.Entities.Asn asn,
        AsnReceiptDetail detail,
        DateTime now,
        string userId,
        string documentId)
    {
        return new LD.Domain.Entities.InventoryMovement
        {
            ProductId = detail.ProductId,
            ClientId = asn.ClientId,
            ProjectId = asn.ProjectId,
            PartNumber = detail.PartNumber,
            Description = detail.Description,
            Fecha = now,
            Hora = now.TimeOfDay,
            UserId = userId,
            LotNumber = detail.LotNumber,
            Reference = detail.Reference,
            PurchaseOrder = detail.PurchaseOrder,
            CustomsDeclarationNumber = detail.CustomsDeclarationNumber,
            ExpirationDate = detail.ExpirationDate,
            PalletNumber = detail.PalletNumber,
            DocumentType = DocumentType_e.Entrada,
            MovementType = MovementType_e.Compra,
            DocumentId = documentId,
            StatusId = detail.Status,
            LocationId = detail.LocationId,
            Qty = detail.ReceivedQuantity,
            StandardId = detail.StandardId
        };
    }

    private static AvailableInventory CreateAvailableInventory(
        LD.Domain.Entities.Asn asn,
        AsnReceiptDetail detail,
        DateTime now,
        string userId,
        string documentId)
    {
        return new AvailableInventory
        {
            ProductId = detail.ProductId,
            ClientId = asn.ClientId,
            ProjectId = asn.ProjectId,
            PartNumber = detail.PartNumber,
            Description = detail.Description,
            Fecha = now,
            Hora = now.TimeOfDay,
            UserId = userId,
            LotNumber = detail.LotNumber,
            Reference = detail.Reference,
            AvailableReference = documentId,
            PurchaseOrder = detail.PurchaseOrder,
            CustomsDeclarationNumber = detail.CustomsDeclarationNumber,
            ExpirationDate = detail.ExpirationDate,
            PalletNumber = detail.PalletNumber,
            DocumentId = documentId,
            StatusId = detail.Status,
            SD = detail.SD,
            AvailableStatus = "Disponible",
            LocationId = detail.LocationId,
            Qty = detail.ReceivedQuantity,
            Supply = 0,
            FinalAvailable = detail.ReceivedQuantity ?? 0,
            StandardId = detail.StandardId
        };
    }
}
