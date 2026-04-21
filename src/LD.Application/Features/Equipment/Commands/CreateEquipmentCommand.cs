using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using MediatR;

namespace LD.Application.Features.Equipment.Commands;

public class CreateEquipmentCommand : EquipmentRequest, IRequest<Result<string>>
{
}

public class CreateEquipmentCommandHandler : IRequestHandler<CreateEquipmentCommand, Result<string>>
{
    private readonly IRepository<LD.Domain.Entities.Equipment> _equipmentRepository;
    private readonly IRepository<LD.Domain.Entities.Warehouse> _warehouseRepository;
    private readonly IRepository<LD.Domain.Entities.EquipmentSupplier> _supplierRepository;
    private readonly IMapper _mapper;

    public CreateEquipmentCommandHandler(
        IRepository<LD.Domain.Entities.Equipment> equipmentRepository,
        IRepository<LD.Domain.Entities.Warehouse> warehouseRepository,
        IRepository<LD.Domain.Entities.EquipmentSupplier> supplierRepository,
        IMapper mapper)
    {
        _equipmentRepository = equipmentRepository;
        _warehouseRepository = warehouseRepository;
        _supplierRepository = supplierRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateEquipmentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            request.WarehouseId = await ResolveWarehouseIdAsync(request.WarehouseId);
            request.EquipmentSupplierId = await ResolveSupplierIdAsync(request.EquipmentSupplierId);

            if (request.WarehouseId <= 0)
                return Result<string>.Failure("No hay almacenes configurados para registrar el equipo", new());

            if (request.EquipmentSupplierId <= 0)
                return Result<string>.Failure("No se pudo resolver el proveedor del equipo", new());

            var result = await _equipmentRepository.CreateAsync(_mapper.Map<LD.Domain.Entities.Equipment>(request));
            return result
                ? Result<string>.Success("Equipo creado con exito", "")
                : Result<string>.Failure("Hubo un error al crear el equipo", new());
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al crear el equipo", new List<string> { ex.Message });
        }
    }

    private async Task<int> ResolveWarehouseIdAsync(int warehouseId)
    {
        if (warehouseId > 0)
            return warehouseId;

        var warehouses = await _warehouseRepository.GetManyAsync() ?? new List<LD.Domain.Entities.Warehouse>();
        return warehouses.FirstOrDefault()?.WarehouseId ?? 0;
    }

    private async Task<int> ResolveSupplierIdAsync(int supplierId)
    {
        if (supplierId > 0)
            return supplierId;

        var suppliers = await _supplierRepository.GetManyAsync() ?? new List<LD.Domain.Entities.EquipmentSupplier>();
        var supplier = suppliers.FirstOrDefault();
        if (supplier is not null)
            return supplier.EquipmentSupplierId;

        var defaultSupplier = new LD.Domain.Entities.EquipmentSupplier
        {
            EquipmentSupplierName = "General"
        };

        var created = await _supplierRepository.CreateAsync(defaultSupplier);
        if (!created)
            return 0;

        var updatedSuppliers = await _supplierRepository.GetManyAsync() ?? new List<LD.Domain.Entities.EquipmentSupplier>();
        return updatedSuppliers
            .OrderByDescending(x => x.EquipmentSupplierId)
            .FirstOrDefault(x => x.EquipmentSupplierName == "General")?.EquipmentSupplierId ?? 0;
    }
}
