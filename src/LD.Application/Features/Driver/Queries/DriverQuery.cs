using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Driver;
using MediatR;

namespace LD.Application.Features.Driver.Queries;

public class DriverQuery : IRequest<Result<List<DriverDto>?>>
{
}

public class DriverQueryHandler : IRequestHandler<DriverQuery, Result<List<DriverDto>?>>
{
    private readonly IRepository<LD.Domain.Entities.Driver> _driverRepository;
    private readonly IMapper _mapper;

    public DriverQueryHandler(IRepository<LD.Domain.Entities.Driver> driverRepository, IMapper mapper)
    {
        _driverRepository = driverRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<DriverDto>?>> Handle(DriverQuery request, CancellationToken cancellationToken)
    {
        var drivers = await _driverRepository.GetManyAsync();
        var driverDtos = _mapper.Map<List<DriverDto>>(drivers ?? []);
        return Result<List<DriverDto>?>.Success(driverDtos, "Choferes obtenidos correctamente");
    }
}
