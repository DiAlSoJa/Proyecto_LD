using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using MediatR;

namespace LD.Application.Features.Driver.Commands;

public class DeleteDriverCommand : IRequest<Result<string>>
{
    public int DriverId { get; set; }
}

public class DeleteDriverCommandHandler : IRequestHandler<DeleteDriverCommand, Result<string>>
{
    private readonly IRepository<LD.Domain.Entities.Driver> _driverRepository;

    public DeleteDriverCommandHandler(IRepository<LD.Domain.Entities.Driver> driverRepository)
    {
        _driverRepository = driverRepository;
    }

    public async Task<Result<string>> Handle(DeleteDriverCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var driver = await _driverRepository.GetByIdAsync(request.DriverId);
            if (driver is null)
            {
                return Result<string>.Failure(
                    "No existe el chofer",
                    new List<string> { "No se encontró el chofer para eliminar" },
                    404);
            }

            var deleted = await _driverRepository.DeleteAsync(driver);
            if (!deleted)
            {
                return Result<string>.Failure(
                    "Hubo un error al eliminar el chofer",
                    new List<string> { "No se pudo eliminar el chofer" });
            }

            return Result<string>.Success(request.DriverId.ToString(), "Chofer eliminado con éxito");
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al eliminar el chofer", new List<string> { ex.Message });
        }
    }
}
