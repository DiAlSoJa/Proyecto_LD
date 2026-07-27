using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using MediatR;
using TruckTypeEntity = LD.Domain.Entities.TruckType;

namespace LD.Application.Features.TruckType.Commands;

public class UpdateTruckTypeCommand : TruckTypeRequest, IRequest<Result<string>>
{
}

public class UpdateTruckTypeCommandHandler : IRequestHandler<UpdateTruckTypeCommand, Result<string>>
{
    private readonly IRepository<TruckTypeEntity> _truckTypeRepository;
    private readonly IMapper _mapper;

    public UpdateTruckTypeCommandHandler(IRepository<TruckTypeEntity> truckTypeRepository, IMapper mapper)
    {
        _truckTypeRepository = truckTypeRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(UpdateTruckTypeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var truckType = await _truckTypeRepository.GetByIdAsync(request.TruckTypeId);
            if (truckType is null)
                return Result<string>.Failure("No existe el tipo de camion", new List<string> { "Hubo un error al obtener el tipo de camion" }, 404);

            _mapper.Map(request, truckType);

            var result = await _truckTypeRepository.UpdateAsync(truckType);
            return result
                ? Result<string>.Success("Tipo de camion actualizado con exito", "")
                : Result<string>.Failure("Hubo un error al actualizar el tipo de camion", new List<string> { "No se encontro el tipo de camion" });
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al actualizar el tipo de camion", new List<string> { ex.Message });
        }
    }
}
