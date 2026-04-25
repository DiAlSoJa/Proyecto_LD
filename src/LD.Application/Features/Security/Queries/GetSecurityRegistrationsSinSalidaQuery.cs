using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.DTOs.Security;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.Security.Queries;

public class GetSecurityRegistrationsSinSalidaQuery : IRequest<Result<List<SecurityRegistrationDto>>>
{
}

public class GetSecurityRegistrationsSinSalidaQueryHandler
    : IRequestHandler<GetSecurityRegistrationsSinSalidaQuery, Result<List<SecurityRegistrationDto>>>
{
    private readonly IRepository<SecurityRegistration> _repository;
    private readonly IMapper _mapper;

    public GetSecurityRegistrationsSinSalidaQueryHandler(
        IRepository<SecurityRegistration> repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<List<SecurityRegistrationDto>>> Handle(
        GetSecurityRegistrationsSinSalidaQuery request,
        CancellationToken cancellationToken)
    {
        var registros = await _repository.GetManyAsync();
        var enPatio = (registros ?? [])
            .Where(r => r.IsActive)
            .OrderByDescending(r => r.CreatedAt)
            .ToList();

        var dtos = _mapper.Map<List<SecurityRegistrationDto>>(enPatio);
        return Result<List<SecurityRegistrationDto>>.Success(dtos, "Vehículos en patio obtenidos correctamente");
    }
}
