using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.InventarioCiclico;
using MediatR;

namespace LD.Application.Features.CyclicInventory.Queries;

public class CyclicInventoryQuery : IRequest<Result<List<CyclicInventoryDto>?>>
{
    public DateTime? Desde { get; set; }
    public DateTime? Hasta { get; set; }
    public string? Estatus { get; set; }
    public string? AuditorUserId { get; set; }
}

public class CyclicInventoryQueryHandler : IRequestHandler<CyclicInventoryQuery, Result<List<CyclicInventoryDto>?>>
{
    private readonly IInventarioCiclicoRepository _repository;
    private readonly IMapper _mapper;

    public CyclicInventoryQueryHandler(IInventarioCiclicoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<List<CyclicInventoryDto>?>> Handle(CyclicInventoryQuery request, CancellationToken cancellationToken)
    {
        var inventarios = await _repository.GetAllWithRelationsAsync(
            request.Desde,
            request.Hasta,
            request.Estatus,
            request.AuditorUserId);
        return Result<List<CyclicInventoryDto>?>.Success(
            _mapper.Map<List<CyclicInventoryDto>>(inventarios),
            "Inventarios ciclicos obtenidos correctamente");
    }
}
