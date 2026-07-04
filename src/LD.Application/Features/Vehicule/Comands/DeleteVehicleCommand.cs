using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using MediatR;

namespace LD.Application.Features.Vehicule.Comands;

public class DeleteVehicleCommand : IRequest<Result<string>>
{
    public string Plates { get; set; } = string.Empty;
}

public class DeleteVehicleCommandHandler : IRequestHandler<DeleteVehicleCommand, Result<string>>
{
    private readonly IRepository<LD.Domain.Entities.Vehicle> _vehicleRepository;

    public DeleteVehicleCommandHandler(IRepository<LD.Domain.Entities.Vehicle> vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<Result<string>> Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(request.Plates);
            if (vehicle is null)
            {
                return Result<string>.Failure(
                    "No existe el vehículo",
                    new List<string> { "No se encontró el vehículo para eliminar" },
                    404);
            }

            var deleted = await _vehicleRepository.DeleteAsync(vehicle);
            if (!deleted)
            {
                return Result<string>.Failure(
                    "Hubo un error al eliminar el vehículo",
                    new List<string> { "No se pudo eliminar el vehículo" });
            }

            return Result<string>.Success(request.Plates, "Vehículo eliminado con éxito");
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(
                "Hubo un error al eliminar el vehículo",
                new List<string> { ex.Message });
        }
    }
}
