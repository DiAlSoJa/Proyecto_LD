using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.DTOs.Security;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.Security.Queries;

public class GetCortinasDisponiblesQuery : IRequest<Result<List<CortinaDto>>>
{
    public int? WarehouseId { get; set; }
}

public class GetCortinasDisponiblesQueryHandler
    : IRequestHandler<GetCortinasDisponiblesQuery, Result<List<CortinaDto>>>
{
    private readonly IRepository<Cortina> _repository;
    private readonly IMapper _mapper;

    public GetCortinasDisponiblesQueryHandler(IRepository<Cortina> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper     = mapper;
    }

    public async Task<Result<List<CortinaDto>>> Handle(
        GetCortinasDisponiblesQuery request, CancellationToken cancellationToken)
    {
        var todas = await _repository.GetManyAsync();
        var disponibles = (todas ?? [])
            .Where(c => c.IsActive && c.EstaDisponible)
            .Where(c => request.WarehouseId == null || c.WarehouseId == request.WarehouseId)
            .OrderBy(c => c.Numero)
            .ToList();

        var dtos = _mapper.Map<List<CortinaDto>>(disponibles);
        return Result<List<CortinaDto>>.Success(dtos, "Cortinas disponibles obtenidas");
    }
}
