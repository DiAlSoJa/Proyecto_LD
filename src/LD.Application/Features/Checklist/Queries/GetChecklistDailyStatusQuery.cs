using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Checklist;
using LD.Contracts.Equipment;
using MediatR;

namespace LD.Application.Features.Checklist.Queries;

public record GetChecklistDailyStatusQuery(string UserId) : IRequest<Result<ChecklistDailyStatusDto>>;

public class GetChecklistDailyStatusQueryHandler
    : IRequestHandler<GetChecklistDailyStatusQuery, Result<ChecklistDailyStatusDto>>
{
    private readonly IRepository<LD.Domain.Entities.Equipment> _equipmentRepository;
    private readonly IRepository<LD.Domain.Entities.EquipmentType> _equipmentTypeRepository;
    private readonly IRepository<LD.Domain.Entities.EquipmentSupplier> _supplierRepository;
    private readonly IChecklistRepository _checklistRepository;
    private readonly IApplicationUserManager _userManager;
    private readonly IMapper _mapper;

    public GetChecklistDailyStatusQueryHandler(
        IRepository<LD.Domain.Entities.Equipment> equipmentRepository,
        IRepository<LD.Domain.Entities.EquipmentType> equipmentTypeRepository,
        IRepository<LD.Domain.Entities.EquipmentSupplier> supplierRepository,
        IChecklistRepository checklistRepository,
        IApplicationUserManager userManager,
        IMapper mapper)
    {
        _equipmentRepository     = equipmentRepository;
        _equipmentTypeRepository = equipmentTypeRepository;
        _supplierRepository      = supplierRepository;
        _checklistRepository     = checklistRepository;
        _userManager             = userManager;
        _mapper                  = mapper;
    }

    public async Task<Result<ChecklistDailyStatusDto>> Handle(
        GetChecklistDailyStatusQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userManager.GetUserByIdAsync(request.UserId);
            var userName = user?.Username ?? string.Empty;

            if (string.IsNullOrWhiteSpace(userName))
                return Result<ChecklistDailyStatusDto>.Success(
                    new ChecklistDailyStatusDto { HasAssignedEquipment = false },
                    "Sin usuario válido");

            var equipments = await _equipmentRepository.GetManyAsync()
                             ?? new List<LD.Domain.Entities.Equipment>();

            // Mismo criterio que GetAssignedEquipmentQuery: Turn1/2/3 == username
            var equipoAsignado = equipments.FirstOrDefault(e =>
                string.Equals(e.Turn1, userName, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(e.Turn2, userName, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(e.Turn3, userName, StringComparison.OrdinalIgnoreCase));

            if (equipoAsignado is null)
                return Result<ChecklistDailyStatusDto>.Success(
                    new ChecklistDailyStatusDto { HasAssignedEquipment = false },
                    "Sin equipo asignado");

            var types     = await _equipmentTypeRepository.GetManyAsync() ?? new List<LD.Domain.Entities.EquipmentType>();
            var suppliers = await _supplierRepository.GetManyAsync()      ?? new List<LD.Domain.Entities.EquipmentSupplier>();

            var typeMap     = types.ToDictionary(x => x.EquipmentTypeId,    x => x.EquipmentName);
            var supplierMap = suppliers.ToDictionary(x => x.EquipmentSupplierId, x => x.EquipmentSupplierName);

            var equipDto  = _mapper.Map<EquipmentDto>(equipoAsignado);
            equipDto.Tipo     = typeMap.TryGetValue(equipDto.EquipmentTypeId, out var tipo)   ? tipo : string.Empty;
            equipDto.Proveedor = supplierMap.TryGetValue(equipDto.EquipmentSupplierId, out var prov) ? prov : string.Empty;

            var since = DateTime.UtcNow.AddHours(-24);
            var (hasCompleted, lastAt) = await _checklistRepository.GetDailyStatusAsync(
                request.UserId, equipoAsignado.EquipmentId, since);

            return Result<ChecklistDailyStatusDto>.Success(new ChecklistDailyStatusDto
            {
                HasAssignedEquipment = true,
                HasCompletedToday    = hasCompleted,
                LastChecklistAt      = lastAt,
                Equipment            = equipDto
            }, "Estado diario obtenido");
        }
        catch (Exception ex)
        {
            return Result<ChecklistDailyStatusDto>.Failure(
                "Error al obtener el estado diario.", new List<string> { ex.Message });
        }
    }
}
