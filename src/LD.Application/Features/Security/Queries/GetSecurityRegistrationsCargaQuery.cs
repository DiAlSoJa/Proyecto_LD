using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.DTOs.Security;
using MediatR;

namespace LD.Application.Features.Security.Queries;

public class GetSecurityRegistrationsCargaQuery : IRequest<Result<List<SecurityRegistrationDto>>>
{
}

public class GetSecurityRegistrationsCargaQueryHandler
    : IRequestHandler<GetSecurityRegistrationsCargaQuery, Result<List<SecurityRegistrationDto>>>
{
    private readonly ISecurityRegistrationRepository _repository;
    private readonly IMapper _mapper;

    public GetSecurityRegistrationsCargaQueryHandler(
        ISecurityRegistrationRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper     = mapper;
    }

    public async Task<Result<List<SecurityRegistrationDto>>> Handle(
        GetSecurityRegistrationsCargaQuery request,
        CancellationToken cancellationToken)
    {
        var registros = await _repository.GetManyWithCortinaAsync();
        var carga = registros
            .Where(r => string.Equals(r.Tipo?.Trim(), "Carga", StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(r => r.CreatedAt)
            .ToList();

        var dtos = _mapper.Map<List<SecurityRegistrationDto>>(carga);
        return Result<List<SecurityRegistrationDto>>.Success(dtos, "Vehiculos de carga obtenidos correctamente");
    }
}
