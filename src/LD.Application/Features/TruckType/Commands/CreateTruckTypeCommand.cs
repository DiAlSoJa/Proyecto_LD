using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using MediatR;
using TruckTypeEntity = LD.Domain.Entities.TruckType;

namespace LD.Application.Features.TruckType.Commands;

public class CreateTruckTypeCommand : TruckTypeRequest, IRequest<Result<string>>
{
}

public class CreateTruckTypeCommandHandler : IRequestHandler<CreateTruckTypeCommand, Result<string>>
{
    private readonly IRepository<TruckTypeEntity> _truckTypeRepository;
    private readonly IMapper _mapper;

    public CreateTruckTypeCommandHandler(IRepository<TruckTypeEntity> truckTypeRepository, IMapper mapper)
    {
        _truckTypeRepository = truckTypeRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateTruckTypeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _truckTypeRepository.CreateAsync(_mapper.Map<TruckTypeEntity>(request));
            return result
                ? Result<string>.Success("Tipo de camion creado con exito", "")
                : Result<string>.Failure("Hubo un error al crear el tipo de camion", new());
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al crear el tipo de camion", new List<string> { ex.Message });
        }
    }
}
