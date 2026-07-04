using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using MediatR;

namespace LD.Application.Features.Driver.Queries;

public record DriverByIdQuery(int DriverId) : IRequest<Result<DriverRequest?>>;

public class DriverByIdQueryHandler : IRequestHandler<DriverByIdQuery, Result<DriverRequest?>>
{
    private readonly IRepository<LD.Domain.Entities.Driver> _driverRepository;
    private readonly IMapper _mapper;

    public DriverByIdQueryHandler(IRepository<LD.Domain.Entities.Driver> driverRepository, IMapper mapper)
    {
        _driverRepository = driverRepository;
        _mapper = mapper;
    }

    public async Task<Result<DriverRequest?>> Handle(DriverByIdQuery request, CancellationToken cancellationToken)
    {
        var driverDb = await _driverRepository.GetByIdAsync(request.DriverId);
        if (driverDb is null)
        {
            return Result<DriverRequest?>.Failure(
                "Chofer no encontrado",
                new List<string> { "No se encontró el chofer" },
                404);
        }

        return Result<DriverRequest?>.Success(_mapper.Map<DriverRequest>(driverDb), "Chofer obtenido con éxito");
    }
}
