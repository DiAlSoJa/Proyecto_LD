using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.DTOs.Security;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.Security.Queries;

public class CortinaQuery : IRequest<Result<List<CortinaDto>?>>
{
    public int? WarehouseId { get; set; }
}

public class CortinaQueryHandler : IRequestHandler<CortinaQuery, Result<List<CortinaDto>?>>
{
    private readonly IRepository<Cortina> _cortinaRepository;
    private readonly IMapper _mapper;

    public CortinaQueryHandler(IRepository<Cortina> cortinaRepository, IMapper mapper)
    {
        _cortinaRepository = cortinaRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<CortinaDto>?>> Handle(CortinaQuery request, CancellationToken cancellationToken)
    {
        var cortinas = await _cortinaRepository.GetManyAsync();
        var filtered = (cortinas ?? [])
            .Where(c => request.WarehouseId == null || c.WarehouseId == request.WarehouseId)
            .OrderBy(c => c.Numero)
            .ToList();

        return Result<List<CortinaDto>?>.Success(_mapper.Map<List<CortinaDto>>(filtered), "Cortinas obtenidas correctamente");
    }
}
