using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.EquipmentType.Queries;

public record EquipmentTypeByIdQuery(int EquipmentTypeId)
    : IRequest<Result<EquipmentTypeRequest?>>;

public class EquipmentTypeByIdQueryHandler : IRequestHandler<EquipmentTypeByIdQuery, Result<EquipmentTypeRequest?>>
{
    private readonly IRepository<LD.Domain.Entities.EquipmentType> _equipmentTypeRepository;
    private readonly IMapper _mapper;

    public EquipmentTypeByIdQueryHandler(IRepository<LD.Domain.Entities.EquipmentType> equipmentTypeRepository, IMapper mapper)
    {
        _equipmentTypeRepository = equipmentTypeRepository;
        _mapper = mapper;
    }

    public async Task<Result<EquipmentTypeRequest?>> Handle(EquipmentTypeByIdQuery request, CancellationToken cancellationToken)
    {
        var equipmentTypeDb = await _equipmentTypeRepository.GetByIdAsync(request.EquipmentTypeId);
        if (equipmentTypeDb == null) return Result<EquipmentTypeRequest?>.Failure("Tipo de equipo no encontrado", new(), 404);

        return Result<EquipmentTypeRequest?>.Success(_mapper.Map<EquipmentTypeRequest>(equipmentTypeDb), "Tipo de equipo obtenido con exito");
    }
}
