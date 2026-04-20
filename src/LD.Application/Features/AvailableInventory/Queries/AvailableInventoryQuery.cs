using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.AvailableInventory;
using MediatR;

namespace LD.Application.Features.AvailableInventories.Queries;

public class AvailableInventoryQuery : IRequest<Result<List<AvailableInventoryDto>?>>
{
}

public class AvailableInventoryQueryHandler : IRequestHandler<AvailableInventoryQuery, Result<List<AvailableInventoryDto>?>>
{
    private readonly IAvailableInventoryRepository _availableInventoryRepository;
    private readonly IMapper _mapper;

    public AvailableInventoryQueryHandler(IAvailableInventoryRepository availableInventoryRepository, IMapper mapper)
    {
        _availableInventoryRepository = availableInventoryRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<AvailableInventoryDto>?>> Handle(AvailableInventoryQuery request, CancellationToken cancellationToken)
    {
        var availableInventories = await _availableInventoryRepository.GetAllWithRelationsAsync();
        var availableInventoryDtos = _mapper.Map<List<AvailableInventoryDto>>(availableInventories);
        return Result<List<AvailableInventoryDto>?>.Success(availableInventoryDtos, "Inventario disponible obtenido correctamente");
    }
}
