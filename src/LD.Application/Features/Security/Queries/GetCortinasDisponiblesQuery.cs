using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.DTOs.Security;
using LD.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace LD.Application.Features.Security.Queries;

public class GetCortinasDisponiblesQuery : IRequest<Result<List<CortinaDto>>>
{
}

public class GetCortinasDisponiblesQueryHandler
    : IRequestHandler<GetCortinasDisponiblesQuery, Result<List<CortinaDto>>>
{
    private readonly IRepository<Cortina> _cortinaRepository;
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IUserContextService _userContext;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache;

    public GetCortinasDisponiblesQueryHandler(
        IRepository<Cortina> cortinaRepository,
        IWarehouseRepository warehouseRepository,
        IUserContextService userContext,
        IMapper mapper,
        IMemoryCache cache)
    {
        _cortinaRepository = cortinaRepository;
        _warehouseRepository = warehouseRepository;
        _userContext = userContext;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<Result<List<CortinaDto>>> Handle(
        GetCortinasDisponiblesQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
            return Result<List<CortinaDto>>.Failure("No se pudo identificar al usuario.", new());

        var cacheKey = $"security:cortinas:disponibles:{userId}";
        if (_cache.TryGetValue(cacheKey, out List<CortinaDto>? dtos))
            return Result<List<CortinaDto>>.Success(dtos ?? [], "Cortinas disponibles obtenidas");

        var warehouses = await _warehouseRepository.GetLookupByUserId(userId);
        var warehouseIds = warehouses
            .Select(w => int.TryParse(w.Key, out var id) ? id : (int?)null)
            .Where(id => id.HasValue)
            .Select(id => id!.Value)
            .ToHashSet();

        if (warehouseIds.Count == 0)
        {
            dtos = [];
            _cache.Set(cacheKey, dtos, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            });
            return Result<List<CortinaDto>>.Success(dtos, "Cortinas disponibles obtenidas");
        }

        var cortinas = await _cortinaRepository.GetManyAsync() ?? [];
        var disponibles = cortinas
            .Where(cortina =>
                cortina.IsActive &&
                cortina.EstaDisponible &&
                warehouseIds.Contains(cortina.WarehouseId))
            .OrderBy(cortina => cortina.Numero)
            .ToList();

        dtos = _mapper.Map<List<CortinaDto>>(disponibles);

        _cache.Set(cacheKey, dtos, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
        });

        return Result<List<CortinaDto>>.Success(dtos, "Cortinas disponibles obtenidas");
    }
}
