using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.DTOs.Security;
using MediatR;

namespace LD.Application.Features.Security.Queries;

public class SecurityRegistrationsByCreatedAtQuery : IRequest<Result<List<SecurityRegistrationDto>?>>
{
    public int? Hours { get; set; }
    public int? Days { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
}

public class SecurityRegistrationsByCreatedAtQueryHandler
    : IRequestHandler<SecurityRegistrationsByCreatedAtQuery, Result<List<SecurityRegistrationDto>?>>
{
    private readonly ISecurityRegistrationRepository _repository;
    private readonly IMapper _mapper;

    public SecurityRegistrationsByCreatedAtQueryHandler(
        ISecurityRegistrationRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<List<SecurityRegistrationDto>?>> Handle(
        SecurityRegistrationsByCreatedAtQuery request,
        CancellationToken cancellationToken)
    {
        if (request.Hours is <= 0)
            return Result<List<SecurityRegistrationDto>?>.Failure("El filtro de horas debe ser mayor a cero", new());

        if (request.Days is <= 0)
            return Result<List<SecurityRegistrationDto>?>.Failure("El filtro de dias debe ser mayor a cero", new());

        if (request.Hours.HasValue && request.Days.HasValue)
            return Result<List<SecurityRegistrationDto>?>.Failure("Usa hours o days, no ambos", new());

        if (request.From.HasValue && request.To.HasValue && request.From > request.To)
            return Result<List<SecurityRegistrationDto>?>.Failure("La fecha inicial no puede ser mayor que la fecha final", new());

        var now = DateTime.UtcNow;
        var from = request.From
            ?? (request.Days.HasValue
                ? now.AddDays(-request.Days.Value)
                : now.AddHours(-(request.Hours ?? 24)));
        var to = request.To ?? now;

        var registrations = await _repository.GetByCreatedAtRangeAsync(from, to);
        var dtos = _mapper.Map<List<SecurityRegistrationDto>>(registrations);
        return Result<List<SecurityRegistrationDto>?>.Success(dtos, "Registros de seguridad obtenidos correctamente");
    }
}
