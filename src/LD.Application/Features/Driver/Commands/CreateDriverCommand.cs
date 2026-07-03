using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using MediatR;

namespace LD.Application.Features.Driver.Commands;

public class CreateDriverCommand : DriverRequest, IRequest<Result<string>>
{
}

public class CreateDriverCommandHandler : IRequestHandler<CreateDriverCommand, Result<string>>
{
    private readonly IRepository<LD.Domain.Entities.Driver> _driverRepository;
    private readonly IMapper _mapper;

    public CreateDriverCommandHandler(IRepository<LD.Domain.Entities.Driver> driverRepository, IMapper mapper)
    {
        _driverRepository = driverRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateDriverCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = _mapper.Map<LD.Domain.Entities.Driver>(request);
            var created = await _driverRepository.CreateAsync(entity);

            return created
                ? Result<string>.Success(entity.DriverId.ToString(), "Chofer creado con éxito")
                : Result<string>.Failure("Hubo un error al crear el chofer", new List<string> { "No se pudo crear el chofer" });
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al crear el chofer", new List<string> { ex.Message });
        }
    }
}
