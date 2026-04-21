using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Equipment;
using MediatR;

namespace LD.Application.Features.Equipment.Queries;

public class EquipmentQuery : IRequest<Result<List<EquipmentDto>?>>
{
}

public class EquipmentQueryHandler : IRequestHandler<EquipmentQuery, Result<List<EquipmentDto>?>>
{
    private readonly IRepository<LD.Domain.Entities.Equipment> _equipmentRepository;
    private readonly IRepository<LD.Domain.Entities.EquipmentType> _equipmentTypeRepository;
    private readonly IRepository<LD.Domain.Entities.EquipmentSupplier> _supplierRepository;
    private readonly IMapper _mapper;

    public EquipmentQueryHandler(
        IRepository<LD.Domain.Entities.Equipment> equipmentRepository,
        IRepository<LD.Domain.Entities.EquipmentType> equipmentTypeRepository,
        IRepository<LD.Domain.Entities.EquipmentSupplier> supplierRepository,
        IMapper mapper)
    {
        _equipmentRepository = equipmentRepository;
        _equipmentTypeRepository = equipmentTypeRepository;
        _supplierRepository = supplierRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<EquipmentDto>?>> Handle(EquipmentQuery request, CancellationToken cancellationToken)
    {
        var equipments = await _equipmentRepository.GetManyAsync() ?? new List<LD.Domain.Entities.Equipment>();
        var equipmentTypes = await _equipmentTypeRepository.GetManyAsync() ?? new List<LD.Domain.Entities.EquipmentType>();
        var suppliers = await _supplierRepository.GetManyAsync() ?? new List<LD.Domain.Entities.EquipmentSupplier>();

        var equipmentTypeMap = equipmentTypes.ToDictionary(x => x.EquipmentTypeId, x => x.EquipmentName);
        var supplierMap = suppliers.ToDictionary(x => x.EquipmentSupplierId, x => x.EquipmentSupplierName);

        var equipmentDtos = _mapper.Map<List<EquipmentDto>>(equipments);

        foreach (var equipment in equipmentDtos)
        {
            equipment.Tipo = equipmentTypeMap.TryGetValue(equipment.EquipmentTypeId, out var typeName)
                ? typeName
                : string.Empty;

            equipment.Proveedor = supplierMap.TryGetValue(equipment.EquipmentSupplierId, out var supplierName)
                ? supplierName
                : string.Empty;
        }

        return Result<List<EquipmentDto>?>.Success(equipmentDtos, "Equipos obtenidos correctamente");
    }
}
