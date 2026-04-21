using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using MediatR;

namespace LD.Application.Features.Equipment.Queries;

public record EquipmentByIdQuery(int EquipmentId) : IRequest<Result<EquipmentRequest?>>;

public class EquipmentByIdQueryHandler : IRequestHandler<EquipmentByIdQuery, Result<EquipmentRequest?>>
{
    private readonly IRepository<LD.Domain.Entities.Equipment> _equipmentRepository;
    private readonly IMapper _mapper;

    public EquipmentByIdQueryHandler(IRepository<LD.Domain.Entities.Equipment> equipmentRepository, IMapper mapper)
    {
        _equipmentRepository = equipmentRepository;
        _mapper = mapper;
    }

    public async Task<Result<EquipmentRequest?>> Handle(EquipmentByIdQuery request, CancellationToken cancellationToken)
    {
        var equipmentDb = await _equipmentRepository.GetByIdAsync(request.EquipmentId);
        if (equipmentDb is null)
            return Result<EquipmentRequest?>.Failure("Equipo no encontrado", new(), 404);

        return Result<EquipmentRequest?>.Success(_mapper.Map<EquipmentRequest>(equipmentDb), "Equipo obtenido con exito");
    }
}
