using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.DTOs.Security;
using LD.Domain.Enums;
using MediatR;

namespace LD.Application.Features.Security.Queries;

public class GetSecurityRegistrationsDescargaQuery : IRequest<Result<List<SecurityRegistrationDto>>>
{
}

public class GetSecurityRegistrationsDescargaQueryHandler
    : IRequestHandler<GetSecurityRegistrationsDescargaQuery, Result<List<SecurityRegistrationDto>>>
{
    private readonly ISecurityRegistrationRepository _repository;
    private readonly IMapper _mapper;

    public GetSecurityRegistrationsDescargaQueryHandler(
        ISecurityRegistrationRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper     = mapper;
    }

    public async Task<Result<List<SecurityRegistrationDto>>> Handle(
        GetSecurityRegistrationsDescargaQuery request,
        CancellationToken cancellationToken)
    {
        var registros = await _repository.GetManyWithCortinaAsync();
        var descarga = registros
            .Where(r => r.IsActive)
            .Where(r => string.Equals(r.Tipo?.Trim(), "Descarga", StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(r => r.CreatedAt)
            .ToList();

        var dtos = _mapper.Map<List<SecurityRegistrationDto>>(descarga);
        return Result<List<SecurityRegistrationDto>>.Success(dtos, "Vehiculos de descarga obtenidos correctamente");
    }
}
