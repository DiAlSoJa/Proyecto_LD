using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using MediatR;
using TruckTypeEntity = LD.Domain.Entities.TruckType;

namespace LD.Application.Features.TruckType.Commands;

public class DeleteTruckTypeCommand : IRequest<Result<string>>
{
    public int TruckTypeId { get; set; }
}

public class DeleteTruckTypeCommandHandler : IRequestHandler<DeleteTruckTypeCommand, Result<string>>
{
    private readonly IRepository<TruckTypeEntity> _truckTypeRepository;

    public DeleteTruckTypeCommandHandler(IRepository<TruckTypeEntity> truckTypeRepository)
    {
        _truckTypeRepository = truckTypeRepository;
    }

    public async Task<Result<string>> Handle(DeleteTruckTypeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var truckType = await _truckTypeRepository.GetByIdAsync(request.TruckTypeId);
            if (truckType is null)
                return Result<string>.Failure("No existe el tipo de camion", new List<string> { "El tipo de camion no fue encontrado" }, 404);

            var result = await _truckTypeRepository.DeleteAsync(truckType);
            return result
                ? Result<string>.Success("Tipo de camion eliminado con exito", "")
                : Result<string>.Failure("Hubo un error al eliminar el tipo de camion", new List<string> { "No se pudo eliminar el tipo de camion" });
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al eliminar el tipo de camion", new List<string> { ex.Message });
        }
    }
}
