using LD.Application.Common.Interfaces.Persistence;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Interfaces.StandarLabel;
using LD.Application.Common.Results;
using LD.Domain.Entities;
using LD.Domain.Enums;
using MediatR;

namespace LD.Application.Features.Asn.Commands;

public class ConfirmAsnCommand : IRequest<Result<string>>
{
    public int AsnId { get; set; }
    public string UserId { get; set; } = string.Empty;
}

public class ConfirmAsnCommandHandler : IRequestHandler<ConfirmAsnCommand, Result<string>>
{
    private readonly IAsnRepository _asnRepository;
    private readonly IRepository<LD.Domain.Entities.AsnDetail> _asnDetailRepository;
    private readonly IRepository<AsnReceiptDetail> _asnReceiptDetailRepository;
    private readonly IRepository<LD.Domain.Entities.InventoryMovement> _inventoryMovementRepository;
    private readonly IRepository<AvailableInventory> _availableInventoryRepository;
    private readonly IStandarIdService _standarIdService;
    private readonly ITransactionManager _transactionManager;

    public ConfirmAsnCommandHandler(
        IAsnRepository asnRepository,
        IRepository<LD.Domain.Entities.AsnDetail> asnDetailRepository,
        IRepository<AsnReceiptDetail> asnReceiptDetailRepository,
        IRepository<LD.Domain.Entities.InventoryMovement> inventoryMovementRepository,
        IRepository<AvailableInventory> availableInventoryRepository,
        IStandarIdService standarIdService,
        ITransactionManager transactionManager)
    {
        _asnRepository = asnRepository;
        _asnDetailRepository = asnDetailRepository;
        _asnReceiptDetailRepository = asnReceiptDetailRepository;
        _inventoryMovementRepository = inventoryMovementRepository;
        _availableInventoryRepository = availableInventoryRepository;
        _standarIdService = standarIdService;
        _transactionManager = transactionManager;
    }

    public async Task<Result<string>> Handle(ConfirmAsnCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.UserId))
                return Result<string>.Failure("No se pudo identificar el usuario actual.", new(), 401);

            var asn = await _asnRepository.GetByIdAsync(request.AsnId);

            if (asn == null)
                return Result<string>.Failure("ASN no encontrado.", new(), 404);

            if (string.Equals(asn.Status, "Confirmado", StringComparison.OrdinalIgnoreCase))
                return Result<string>.Failure("El ASN ya esta confirmado.", new());

            if (string.Equals(asn.Status?.Trim(), "Cancelado", StringComparison.OrdinalIgnoreCase))
                return Result<string>.Failure("El ASN esta cancelado y no se puede confirmar.", new());

            var asnDetails = await _asnDetailRepository.GetManyAsync() ?? new List<LD.Domain.Entities.AsnDetail>();
            var asnDetailIds = asnDetails
                .Where(x => x.AsnId == request.AsnId)
                .Select(x => x.AsnDetailId)
                .ToHashSet();

            var details = (await _asnReceiptDetailRepository.GetManyAsync() ?? new List<AsnReceiptDetail>())
                .Where(x => asnDetailIds.Contains(x.AsnDetailId))
                .ToList();

            if (!details.Any())
                return Result<string>.Failure("El ASN debe tener al menos un ASN Receipt Detail.", new());

            var invalidDetails = details
                .Where(x =>
                    (x.LocationId is null && string.IsNullOrWhiteSpace(x.LocationCode)) ||
                    string.IsNullOrWhiteSpace(x.Status) ||
                    string.IsNullOrWhiteSpace(x.SD))
                .Select(x => string.IsNullOrWhiteSpace(x.PartNumber)
                    ? x.AsnReceiptDetailId.ToString()
                    : x.PartNumber)
                .Distinct()
                .ToList();

            if (invalidDetails.Any())
            {
                return Result<string>.Failure(
                    $"Antes de confirmar, todos los asnreceiptdetails deben tener ubicacion, estatus y SD. Lineas con problema: {string.Join(", ", invalidDetails)}.",
                    new());
            }

            var detailIdsWithoutStandardId = details
                .Where(x => !x.StandardId.HasValue)
                .Select(x => x.AsnReceiptDetailId)
                .ToList();

            if (detailIdsWithoutStandardId.Any())
                await _standarIdService.AssignStandarIdsToReceiptDetailsAsync(detailIdsWithoutStandardId, request.UserId);

            details = (await _asnReceiptDetailRepository.GetManyAsync() ?? new List<AsnReceiptDetail>())
                .Where(x => asnDetailIds.Contains(x.AsnDetailId))
                .ToList();

            await using var transaction = await _transactionManager.BeginTransactionAsync(cancellationToken);

            try
            {
                var now = DateTime.Now;
                var documentId = !string.IsNullOrWhiteSpace(asn.AsnCode)
                    ? asn.AsnCode
                    : asn.AsnId.ToString();

                var detailStandardIds = details
                    .Where(x => x.StandardId.HasValue)
                    .Select(x => x.StandardId!.Value)
                    .ToHashSet();

                var existingMovements = (await _inventoryMovementRepository.GetManyAsync() ?? new List<LD.Domain.Entities.InventoryMovement>())
                    .Where(x => x.StandardId.HasValue && detailStandardIds.Contains(x.StandardId.Value))
                    .ToList();
                var existingAvailableInventory = (await _availableInventoryRepository.GetManyAsync() ?? new List<AvailableInventory>())
                    .Where(x => x.StandardId.HasValue && detailStandardIds.Contains(x.StandardId.Value))
                    .ToList();

                foreach (var detail in details)
                {
                    if (detail.ProductId is null)
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        return Result<string>.Failure($"La linea {detail.PartNumber} no tiene ProductId y no se puede generar el movimiento.", new());
                    }

                    if (detail.StandardId.HasValue && existingMovements.Any(x => x.StandardId == detail.StandardId))
                        continue;

                    var movement = new LD.Domain.Entities.InventoryMovement
                    {
                        ProductId = detail.ProductId,
                        ClientId = asn.ClientId,
                        ProjectId = asn.ProjectId,
                        PartNumber = detail.PartNumber,
                        Description = detail.Description,
                        Fecha = now,
                        Hora = now.TimeOfDay,
                        UserId = request.UserId,
                        LotNumber = detail.LotNumber,
                        Reference = detail.Reference,
                        PurchaseOrder = detail.PurchaseOrder,
                        CustomsDeclarationNumber = detail.CustomsDeclarationNumber,
                        ExpirationDate = detail.ExpirationDate,
                        DocumentType = DocumentType_e.Entrada,
                        MovementType = MovementType_e.Compra,
                        DocumentId = documentId,
                        StatusId = detail.Status,
                        LocationId = detail.LocationId,
                        Qty = detail.ReceivedQuantity,
                        StandardId = detail.StandardId
                    };

                    var movementCreated = await _inventoryMovementRepository.CreateAsync(movement);
                    if (!movementCreated)
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        return Result<string>.Failure($"No se pudo generar el movimiento de inventario para la linea {detail.PartNumber}.", new());
                    }

                    if (detail.StandardId.HasValue)
                        existingMovements.Add(movement);

                    if (detail.StandardId.HasValue && existingAvailableInventory.Any(x => x.StandardId == detail.StandardId))
                        continue;

                    var availableInventory = new AvailableInventory
                    {
                        ProductId = detail.ProductId,
                        ClientId = asn.ClientId,
                        ProjectId = asn.ProjectId,
                        PartNumber = detail.PartNumber,
                        Description = detail.Description,
                        Fecha = now,
                        Hora = now.TimeOfDay,
                        UserId = request.UserId,
                        LotNumber = detail.LotNumber,
                        Reference = detail.Reference,
                        PurchaseOrder = detail.PurchaseOrder,
                        CustomsDeclarationNumber = detail.CustomsDeclarationNumber,
                        ExpirationDate = detail.ExpirationDate,
                        DocumentId = documentId,
                        StatusId = detail.Status,
                        LocationId = detail.LocationId,
                        Qty = detail.ReceivedQuantity,
                        StandardId = detail.StandardId
                    };

                    var availableInventoryCreated = await _availableInventoryRepository.CreateAsync(availableInventory);
                    if (!availableInventoryCreated)
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        return Result<string>.Failure($"No se pudo registrar el inventario disponible para la linea {detail.PartNumber}.", new());
                    }

                    if (detail.StandardId.HasValue)
                        existingAvailableInventory.Add(availableInventory);
                }

                asn.Status = "Confirmado";
                asn.LastModifiedAt = now;
                asn.LastModifiedByUserId = request.UserId;

                var updated = await _asnRepository.UpdateAsync(asn);
                if (!updated)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return Result<string>.Failure("No se pudo actualizar el ASN.", new());
                }

                await transaction.CommitAsync(cancellationToken);
                return Result<string>.Success(asn.AsnId.ToString(), "ASN confirmado correctamente.");
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al confirmar el ASN.", new() { ex.Message });
        }
    }
}
