using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using MediatR;

namespace LD.Application.Features.Driver.Commands;

public class UpdateDriverCommand : DriverRequest, IRequest<Result<string>>
{
}

public class UpdateDriverCommandHandler : IRequestHandler<UpdateDriverCommand, Result<string>>
{
    private readonly IRepository<LD.Domain.Entities.Driver> _driverRepository;
    private readonly IMapper _mapper;

    public UpdateDriverCommandHandler(IRepository<LD.Domain.Entities.Driver> driverRepository, IMapper mapper)
    {
        _driverRepository = driverRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(UpdateDriverCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var driver = await _driverRepository.GetByIdAsync(request.DriverId);
            if (driver is null)
            {
                return Result<string>.Failure(
                    "No existe el chofer",
                    new List<string> { "No se encontró el chofer" },
                    404);
            }

            _mapper.Map(request, driver);

            var updated = await _driverRepository.UpdateAsync(driver);
            return updated
                ? Result<string>.Success(driver.DriverId.ToString(), "Chofer actualizado con éxito")
                : Result<string>.Failure("Hubo un error al actualizar el chofer", new List<string> { "No se pudo actualizar el chofer" });
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al actualizar el chofer", new List<string> { ex.Message });
        }
    }
}
