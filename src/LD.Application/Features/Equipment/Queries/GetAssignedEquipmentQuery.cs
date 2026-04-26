using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Equipment;
using MediatR;

namespace LD.Application.Features.Equipment.Queries;

public record GetAssignedEquipmentQuery(string UserId) : IRequest<Result<EquipmentDto?>>;

public class GetAssignedEquipmentQueryHandler : IRequestHandler<GetAssignedEquipmentQuery, Result<EquipmentDto?>>
{
    private readonly IRepository<LD.Domain.Entities.Equipment> _equipmentRepository;
    private readonly IRepository<LD.Domain.Entities.EquipmentType> _equipmentTypeRepository;
    private readonly IRepository<LD.Domain.Entities.EquipmentSupplier> _supplierRepository;
    private readonly IApplicationUserManager _userManager;
    private readonly IMapper _mapper;

    public GetAssignedEquipmentQueryHandler(
        IRepository<LD.Domain.Entities.Equipment> equipmentRepository,
        IRepository<LD.Domain.Entities.EquipmentType> equipmentTypeRepository,
        IRepository<LD.Domain.Entities.EquipmentSupplier> supplierRepository,
        IApplicationUserManager userManager,
        IMapper mapper)
    {
        _equipmentRepository     = equipmentRepository;
        _equipmentTypeRepository = equipmentTypeRepository;
        _supplierRepository      = supplierRepository;
        _userManager             = userManager;
        _mapper                  = mapper;
    }

    public async Task<Result<EquipmentDto?>> Handle(
        GetAssignedEquipmentQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.GetUserByIdAsync(request.UserId);
        var userName = user?.Username ?? string.Empty;

        if (string.IsNullOrWhiteSpace(userName))
            return Result<EquipmentDto?>.Success(null, "Sin equipo asignado");

        var equipments = await _equipmentRepository.GetManyAsync()
                         ?? new List<LD.Domain.Entities.Equipment>();

        // TODO: migrar a campo AssignedUserId cuando exista en la entidad
        var equipoAsignado = equipments.FirstOrDefault(e =>
            string.Equals(e.Turn1, userName, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(e.Turn2, userName, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(e.Turn3, userName, StringComparison.OrdinalIgnoreCase));

        if (equipoAsignado is null)
            return Result<EquipmentDto?>.Success(null, "Sin equipo asignado");

        var equipmentTypes = await _equipmentTypeRepository.GetManyAsync()
                             ?? new List<LD.Domain.Entities.EquipmentType>();
        var suppliers = await _supplierRepository.GetManyAsync()
                        ?? new List<LD.Domain.Entities.EquipmentSupplier>();

        var equipmentTypeMap = equipmentTypes.ToDictionary(x => x.EquipmentTypeId, x => x.EquipmentName);
        var supplierMap      = suppliers.ToDictionary(x => x.EquipmentSupplierId, x => x.EquipmentSupplierName);

        var dto = _mapper.Map<EquipmentDto>(equipoAsignado);
        dto.Tipo      = equipmentTypeMap.TryGetValue(dto.EquipmentTypeId, out var tipo) ? tipo : string.Empty;
        dto.Proveedor = supplierMap.TryGetValue(dto.EquipmentSupplierId, out var prov)  ? prov : string.Empty;

        return Result<EquipmentDto?>.Success(dto, "Equipo asignado encontrado");
    }
}
