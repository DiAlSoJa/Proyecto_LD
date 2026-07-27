using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.TruckType;
using MediatR;
using TruckTypeEntity = LD.Domain.Entities.TruckType;

namespace LD.Application.Features.TruckType.Queries;

public class TruckTypeQuery : IRequest<Result<List<TruckTypeDto>?>>
{
}

public class TruckTypeQueryHandler : IRequestHandler<TruckTypeQuery, Result<List<TruckTypeDto>?>>
{
    private readonly IRepository<TruckTypeEntity> _truckTypeRepository;
    private readonly IMapper _mapper;

    public TruckTypeQueryHandler(IRepository<TruckTypeEntity> truckTypeRepository, IMapper mapper)
    {
        _truckTypeRepository = truckTypeRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<TruckTypeDto>?>> Handle(TruckTypeQuery request, CancellationToken cancellationToken)
    {
        var truckTypes = await _truckTypeRepository.GetManyAsync() ?? [];
        var orderedTruckTypes = truckTypes
            .OrderBy(x => x.Name)
            .ToList();

        var truckTypeDtos = _mapper.Map<List<TruckTypeDto>>(orderedTruckTypes);
        return Result<List<TruckTypeDto>?>.Success(truckTypeDtos, "Tipos de camion obtenidos correctamente");
    }
}
