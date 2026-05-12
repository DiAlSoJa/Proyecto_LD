using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.DTOs.Security;
using LD.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

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
    private readonly IMemoryCache _cache;

    public GetCortinasDisponiblesQueryHandler(
        IRepository<Cortina> repository,
        IMapper mapper,
        IMemoryCache cache)
    {
        _repository = repository;
        _mapper     = mapper;
        _cache      = cache;
    }

    public async Task<Result<List<CortinaDto>>> Handle(
        GetCortinasDisponiblesQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"security:cortinas:disponibles:{request.WarehouseId?.ToString() ?? "all"}";

        if (!_cache.TryGetValue(cacheKey, out List<CortinaDto>? dtos))
        {
            var todas = await _repository.GetManyAsync();
            var disponibles = (todas ?? [])
                .Where(c => c.IsActive && c.EstaDisponible)
                .Where(c => request.WarehouseId == null || c.WarehouseId == request.WarehouseId)
                .OrderBy(c => c.Numero)
                .ToList();

            dtos = _mapper.Map<List<CortinaDto>>(disponibles);

            _cache.Set(cacheKey, dtos, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            });
        }

        return Result<List<CortinaDto>>.Success(dtos ?? [], "Cortinas disponibles obtenidas");
    }
}
