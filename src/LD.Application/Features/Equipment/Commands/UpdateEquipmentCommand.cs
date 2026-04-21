using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using MediatR;

namespace LD.Application.Features.Equipment.Commands;

public class UpdateEquipmentCommand : EquipmentRequest, IRequest<Result<string>>
{
}

public class UpdateEquipmentCommandHandler : IRequestHandler<UpdateEquipmentCommand, Result<string>>
{
    private readonly IRepository<LD.Domain.Entities.Equipment> _equipmentRepository;
    private readonly IRepository<LD.Domain.Entities.Warehouse> _warehouseRepository;
    private readonly IRepository<LD.Domain.Entities.EquipmentSupplier> _supplierRepository;
    private readonly IMapper _mapper;

    public UpdateEquipmentCommandHandler(
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

    public async Task<Result<string>> Handle(UpdateEquipmentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var equipment = await _equipmentRepository.GetByIdAsync(request.EquipmentId);
            if (equipment is null)
                return Result<string>.Failure("No existe el equipo", new List<string> { "Hubo un error al obtener el equipo" }, 404);

            request.WarehouseId = await ResolveWarehouseIdAsync(request.WarehouseId, equipment.WarehouseId);
            request.EquipmentSupplierId = await ResolveSupplierIdAsync(request.EquipmentSupplierId, equipment.EquipmentSupplierId);

            _mapper.Map(request, equipment);

            var result = await _equipmentRepository.UpdateAsync(equipment);
            return result
                ? Result<string>.Success("Equipo actualizado con exito", "")
                : Result<string>.Failure("Hubo un error al actualizar el equipo", new List<string> { "No se encontro el equipo" });
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al actualizar el equipo", new List<string> { ex.Message });
        }
    }

    private async Task<int> ResolveWarehouseIdAsync(int warehouseId, int currentWarehouseId)
    {
        if (warehouseId > 0)
            return warehouseId;

        if (currentWarehouseId > 0)
            return currentWarehouseId;

        var warehouses = await _warehouseRepository.GetManyAsync() ?? new List<LD.Domain.Entities.Warehouse>();
        return warehouses.FirstOrDefault()?.WarehouseId ?? 0;
    }

    private async Task<int> ResolveSupplierIdAsync(int supplierId, int currentSupplierId)
    {
        if (supplierId > 0)
            return supplierId;

        if (currentSupplierId > 0)
            return currentSupplierId;

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
