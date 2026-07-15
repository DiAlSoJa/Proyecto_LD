using LD.Application.Common.Interfaces.Persistence;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Interfaces.StandarLabel;
using LD.Application.Common.Results;
using LD.Domain.Entities;
using LD.Domain.Enums;
using MediatR;

namespace LD.Application.Features.Asn.Commands;

public class LocateAsnCommand : IRequest<Result<string>>
{
    public int AsnId { get; set; }
    public string UserId { get; set; } = string.Empty;
}

// ============================================================================
// VERSION ANTERIOR (antes de generar tareas de entrada al ubicar el ASN).
// Se conserva comentada por si se necesita revertir o comparar.
// Solo marcaba el ASN como "Ubicando"; no generaba WarehouseTasks.
// ----------------------------------------------------------------------------
// public class LocateAsnCommandHandler : IRequestHandler<LocateAsnCommand, Result<string>>
// {
//     private readonly IAsnRepository _asnRepository;
//     private readonly IRepository<LD.Domain.Entities.AsnDetail> _asnDetailRepository;
//     private readonly IRepository<AsnReceiptDetail> _asnReceiptDetailRepository;
//     private readonly IStandarIdService _standarIdService;
//
//     public LocateAsnCommandHandler(
//         IAsnRepository asnRepository,
//         IRepository<LD.Domain.Entities.AsnDetail> asnDetailRepository,
//         IRepository<AsnReceiptDetail> asnReceiptDetailRepository,
//         IStandarIdService standarIdService)
//     {
//         _asnRepository = asnRepository;
//         _asnDetailRepository = asnDetailRepository;
//         _asnReceiptDetailRepository = asnReceiptDetailRepository;
//         _standarIdService = standarIdService;
//     }
//
//     public async Task<Result<string>> Handle(LocateAsnCommand request, CancellationToken cancellationToken)
//     {
//         try
//         {
//             if (string.IsNullOrWhiteSpace(request.UserId))
//                 return Result<string>.Failure("No se pudo identificar el usuario actual.", new(), 401);
//
//             var asn = await _asnRepository.GetByIdAsync(request.AsnId);
//
//             if (asn == null)
//                 return Result<string>.Failure("ASN no encontrado.", new(), 404);
//
//             if (string.Equals(asn.Status?.Trim(), "Confirmado", StringComparison.OrdinalIgnoreCase))
//                 return Result<string>.Failure("El ASN ya esta confirmado y no se puede ubicar.", new());
//
//             if (string.Equals(asn.Status?.Trim(), "Cancelado", StringComparison.OrdinalIgnoreCase))
//                 return Result<string>.Failure("El ASN esta cancelado y no se puede ubicar.", new());
//
//             if (string.Equals(asn.Status?.Trim(), "Ubicando", StringComparison.OrdinalIgnoreCase))
//                 return Result<string>.Failure("El ASN ya esta en estatus Ubicando.", new());
//
//             var asnDetails = await _asnDetailRepository.GetManyAsync() ?? new List<LD.Domain.Entities.AsnDetail>();
//             var asnDetailIds = asnDetails
//                 .Where(x => x.AsnId == request.AsnId)
//                 .Select(x => x.AsnDetailId)
//                 .ToHashSet();
//
//             var details = (await _asnReceiptDetailRepository.GetManyAsync() ?? new List<AsnReceiptDetail>())
//                 .Where(x => asnDetailIds.Contains(x.AsnDetailId))
//                 .ToList();
//
//             if (!details.Any())
//                 return Result<string>.Failure("El ASN debe tener al menos un ASN Receipt Detail.", new());
//
//             var invalidDetails = details
//                 .Where(x =>
//                     (x.LocationId is null && string.IsNullOrWhiteSpace(x.LocationCode)) ||
//                     string.IsNullOrWhiteSpace(x.Status) ||
//                     string.IsNullOrWhiteSpace(x.SD))
//                 .Select(x => string.IsNullOrWhiteSpace(x.PartNumber)
//                     ? x.AsnReceiptDetailId.ToString()
//                     : x.PartNumber)
//                 .Distinct()
//                 .ToList();
//
//             if (invalidDetails.Any())
//             {
//                 return Result<string>.Failure(
//                     $"Antes de confirmar, todos los asnreceiptdetails deben tener ubicacion, estatus y SD. Lineas con problema: {string.Join(", ", invalidDetails)}.",
//                     new());
//             }
//
//             var detailIdsWithoutStandardId = details
//                 .Where(x => !x.StandardId.HasValue)
//                 .Select(x => x.AsnReceiptDetailId)
//                 .ToList();
//
//             if (detailIdsWithoutStandardId.Any())
//                 await _standarIdService.AssignStandarIdsToReceiptDetailsAsync(detailIdsWithoutStandardId, request.UserId);
//
//             asn.Status = "Ubicando";
//             asn.LastModifiedAt = DateTime.Now;
//             asn.LastModifiedByUserId = request.UserId;
//
//             var updated = await _asnRepository.UpdateAsync(asn);
//             if (!updated)
//                 return Result<string>.Failure("No se pudo actualizar el ASN a Ubicando.", new());
//
//             return Result<string>.Success(asn.AsnId.ToString(), "ASN marcado como Ubicando correctamente.");
//         }
//         catch (Exception ex)
//         {
//             return Result<string>.Failure("Hubo un error al marcar el ASN como Ubicando.", new() { ex.Message });
//         }
//     }
// }
// ============================================================================

public class LocateAsnCommandHandler : IRequestHandler<LocateAsnCommand, Result<string>>
{
    private readonly IAsnRepository _asnRepository;
    private readonly IRepository<LD.Domain.Entities.AsnDetail> _asnDetailRepository;
    private readonly IRepository<AsnReceiptDetail> _asnReceiptDetailRepository;
    private readonly IRepository<Location> _locationRepository;
    private readonly IRepository<AvailableInventory> _availableInventoryRepository;
    private readonly IRepository<WarehouseTask> _warehouseTaskRepository;
    private readonly IStandarIdService _standarIdService;
    private readonly ITransactionManager _transactionManager;

    public LocateAsnCommandHandler(
        IAsnRepository asnRepository,
        IRepository<LD.Domain.Entities.AsnDetail> asnDetailRepository,
        IRepository<AsnReceiptDetail> asnReceiptDetailRepository,
        IRepository<Location> locationRepository,
        IRepository<AvailableInventory> availableInventoryRepository,
        IRepository<WarehouseTask> warehouseTaskRepository,
        IStandarIdService standarIdService,
        ITransactionManager transactionManager)
    {
        _asnRepository = asnRepository;
        _asnDetailRepository = asnDetailRepository;
        _asnReceiptDetailRepository = asnReceiptDetailRepository;
        _locationRepository = locationRepository;
        _availableInventoryRepository = availableInventoryRepository;
        _warehouseTaskRepository = warehouseTaskRepository;
        _standarIdService = standarIdService;
        _transactionManager = transactionManager;
    }

    public async Task<Result<string>> Handle(LocateAsnCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.UserId))
                return Result<string>.Failure("No se pudo identificar el usuario actual.", new(), 401);

            var asn = await _asnRepository.GetByIdAsync(request.AsnId);

            if (asn == null)
                return Result<string>.Failure("ASN no encontrado.", new(), 404);

            if (string.Equals(asn.Status?.Trim(), "Confirmado", StringComparison.OrdinalIgnoreCase))
                return Result<string>.Failure("El ASN ya esta confirmado y no se puede ubicar.", new());

            if (string.Equals(asn.Status?.Trim(), "Cancelado", StringComparison.OrdinalIgnoreCase))
                return Result<string>.Failure("El ASN esta cancelado y no se puede ubicar.", new());

            if (string.Equals(asn.Status?.Trim(), "Ubicando", StringComparison.OrdinalIgnoreCase))
                return Result<string>.Failure("El ASN ya esta en estatus Ubicando.", new());

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

            // Re-consultar tras asignar StandardIds para trabajar con datos frescos.
            var receiptDetails = (await _asnReceiptDetailRepository.GetManyAsync() ?? new List<AsnReceiptDetail>())
                .Where(x => asnDetailIds.Contains(x.AsnDetailId))
                .ToList();

            var pallets = receiptDetails
                .Where(x => x.StandardId.HasValue)
                .GroupBy(x => x.StandardId!.Value)
                .ToList();

            if (!pallets.Any())
                return Result<string>.Failure("El ASN no tiene tarimas (StandardId) para ubicar.", new());

            // Ubicaciones y catalogo de inventario para elegir destinos vacios.
            var locations = await _locationRepository.GetManyAsync() ?? new List<Location>();
            var occupiedByInventory = (await _availableInventoryRepository.GetManyAsync() ?? new List<AvailableInventory>())
                .Where(x => x.LocationId.HasValue)
                .Select(x => x.LocationId!.Value)
                .ToHashSet();

            var reservedInBatch = new HashSet<int>();

            await using var transaction = await _transactionManager.BeginTransactionAsync(cancellationToken);

            try
            {
                var createdTasks = 0;
                var unplacedPallets = new List<string>();

                foreach (var pallet in pallets)
                {
                    var standardId = pallet.Key;
                    var lines = pallet.ToList();
                    var head = lines[0];

                    // Almacen origen: derivado de la ubicacion de recepcion de la tarima.
                    var sourceLocation = ResolveLocation(locations, head.LocationId, head.LocationCode);
                    if (sourceLocation is null)
                    {
                        unplacedPallets.Add($"{standardId} (sin ubicacion de origen)");
                        continue;
                    }

                    var warehouseId = sourceLocation.WarehouseId;

                    // Ubicacion destino: vacia (sin inventario), no ocupada, no reservada en este lote.
                    var destination = PickEmptyDestination(
                        locations, warehouseId, occupiedByInventory, reservedInBatch, sourceLocation.LocationId);

                    if (destination is null)
                    {
                        unplacedPallets.Add($"{standardId} (sin ubicacion vacia disponible)");
                        continue;
                    }

                    // Reservar la ubicacion para que otra tarima no la tome.
                    reservedInBatch.Add(destination.LocationId);
                    destination.Ocupado = true;
                    var locationReserved = await _locationRepository.UpdateAsync(destination);
                    if (!locationReserved)
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        return Result<string>.Failure(
                            $"No se pudo reservar la ubicacion destino para la tarima {standardId}.", new());
                    }

                    var partNumbers = string.Join(", ", lines
                        .Select(x => x.PartNumber)
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .Distinct());

                    var task = new WarehouseTask
                    {
                        WarehouseId = warehouseId,
                        AsnId = asn.AsnId,
                        StandardId = standardId,
                        SourceLocationId = sourceLocation.LocationId,
                        DestinationLocationId = destination.LocationId,
                        DestinationLocationCode = destination.LocationName,
                        Priority = "Media",
                        Activity = "Entrada",
                        Name = $"Ubicar tarima {standardId}",
                        Description = $"Mover la tarima {standardId} ({partNumbers}) de {sourceLocation.LocationName} a {destination.LocationName}.",
                        Status = WarehouseTaskStatus.NoAsignada
                    };

                    var taskCreated = await _warehouseTaskRepository.CreateAsync(task);
                    if (!taskCreated)
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        return Result<string>.Failure(
                            $"No se pudo generar la tarea de entrada para la tarima {standardId}.", new());
                    }

                    createdTasks++;
                }

                if (unplacedPallets.Any())
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return Result<string>.Failure(
                        $"No se pudieron ubicar todas las tarimas. Sin ubicacion: {string.Join(", ", unplacedPallets)}.",
                        new());
                }

                asn.Status = "Ubicando";
                asn.LastModifiedAt = DateTime.Now;
                asn.LastModifiedByUserId = request.UserId;

                var updated = await _asnRepository.UpdateAsync(asn);
                if (!updated)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return Result<string>.Failure("No se pudo actualizar el ASN a Ubicando.", new());
                }

                await transaction.CommitAsync(cancellationToken);

                return Result<string>.Success(
                    asn.AsnId.ToString(),
                    $"ASN marcado como Ubicando. Se generaron {createdTasks} tarea(s) de entrada.");
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al marcar el ASN como Ubicando.", new() { ex.Message });
        }
    }

    private static Location? ResolveLocation(IReadOnlyCollection<Location> locations, int? locationId, string? locationCode)
    {
        if (locationId.HasValue)
        {
            var byId = locations.FirstOrDefault(x => x.LocationId == locationId.Value);
            if (byId is not null)
                return byId;
        }

        if (!string.IsNullOrWhiteSpace(locationCode))
        {
            var value = locationCode.Trim();
            return locations.FirstOrDefault(x =>
                string.Equals(x.LocationName?.Trim(), value, StringComparison.OrdinalIgnoreCase));
        }

        return null;
    }

    private static Location? PickEmptyDestination(
        IEnumerable<Location> locations,
        int warehouseId,
        HashSet<int> occupiedByInventory,
        HashSet<int> reservedInBatch,
        int sourceLocationId)
    {
        return locations
            .Where(x => x.WarehouseId == warehouseId)
            .Where(x => x.LocationId != sourceLocationId)
            .Where(x => !x.Ocupado)
            .Where(x => !occupiedByInventory.Contains(x.LocationId))
            .Where(x => !reservedInBatch.Contains(x.LocationId))
            // Excluir ubicaciones que no son de almacenamiento (andenes/cortinas/embarque).
            .Where(x => !x.HasCortina && !x.IsEmbarque && !x.IsReciboYEmbarque)
            .OrderBy(x => x.LocationName)
            .FirstOrDefault();
    }
}
