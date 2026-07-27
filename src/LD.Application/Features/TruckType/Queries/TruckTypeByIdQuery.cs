using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using MediatR;
using TruckTypeEntity = LD.Domain.Entities.TruckType;

namespace LD.Application.Features.TruckType.Queries;

public record TruckTypeByIdQuery(int TruckTypeId)
    : IRequest<Result<TruckTypeRequest?>>;

public class TruckTypeByIdQueryHandler : IRequestHandler<TruckTypeByIdQuery, Result<TruckTypeRequest?>>
{
    private readonly IRepository<TruckTypeEntity> _truckTypeRepository;
    private readonly IMapper _mapper;

    public TruckTypeByIdQueryHandler(IRepository<TruckTypeEntity> truckTypeRepository, IMapper mapper)
    {
        _truckTypeRepository = truckTypeRepository;
        _mapper = mapper;
    }

    public async Task<Result<TruckTypeRequest?>> Handle(TruckTypeByIdQuery request, CancellationToken cancellationToken)
    {
        var truckTypeDb = await _truckTypeRepository.GetByIdAsync(request.TruckTypeId);
        if (truckTypeDb is null)
            return Result<TruckTypeRequest?>.Failure("Tipo de camion no encontrado", new(), 404);

        return Result<TruckTypeRequest?>.Success(_mapper.Map<TruckTypeRequest>(truckTypeDb), "Tipo de camion obtenido con exito");
    }
}
