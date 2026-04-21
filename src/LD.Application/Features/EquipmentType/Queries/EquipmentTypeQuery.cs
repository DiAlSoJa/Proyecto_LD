using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.EquipmentType;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.EquipmentType.Queries;

public class EquipmentTypeQuery : IRequest<Result<List<EquipmentTypeDto>?>>
{
}

public class EquipmentTypeQueryHandler : IRequestHandler<EquipmentTypeQuery, Result<List<EquipmentTypeDto>?>>
{
    private readonly IRepository<LD.Domain.Entities.EquipmentType> _equipmentTypeRepository;
    private readonly IMapper _mapper;

    public EquipmentTypeQueryHandler(IRepository<LD.Domain.Entities.EquipmentType> equipmentTypeRepository, IMapper mapper)
    {
        _equipmentTypeRepository = equipmentTypeRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<EquipmentTypeDto>?>> Handle(EquipmentTypeQuery request, CancellationToken cancellationToken)
    {
        var equipmentTypes = await _equipmentTypeRepository.GetManyAsync();
        var equipmentTypeDtos = _mapper.Map<List<EquipmentTypeDto>>(equipmentTypes);
        return Result<List<EquipmentTypeDto>?>.Success(equipmentTypeDtos, "Tipos de equipo obtenidos correctamente");
    }
}
