using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.DamageReports;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.DamageReports.Queries;

public record DamageReportByIdQuery(int DamageReportId) : IRequest<Result<DamageReportDto?>>;

public class DamageReportByIdQueryHandler : IRequestHandler<DamageReportByIdQuery, Result<DamageReportDto?>>
{
    private readonly IRepository<DamageReport> _repository;
    private readonly IApplicationUserManager _applicationUserManager;
    private readonly IMapper _mapper;

    public DamageReportByIdQueryHandler(
        IRepository<DamageReport> repository,
        IApplicationUserManager applicationUserManager,
        IMapper mapper)
    {
        _repository = repository;
        _applicationUserManager = applicationUserManager;
        _mapper = mapper;
    }

    public async Task<Result<DamageReportDto?>> Handle(DamageReportByIdQuery request, CancellationToken cancellationToken)
    {
        var damageReport = await _repository.GetByIdAsync(request.DamageReportId);
        if (damageReport is null)
            return Result<DamageReportDto?>.Failure("Reporte de danos no encontrado", new(), 404);

        var damageReportDto = _mapper.Map<DamageReportDto>(damageReport);
        if (!string.IsNullOrWhiteSpace(damageReportDto.CreatedByUserId))
        {
            var user = await _applicationUserManager.GetUserByIdAsync(damageReportDto.CreatedByUserId);
            damageReportDto.CreatedByUserName = user?.Username ?? user?.Name ?? damageReportDto.CreatedByUserId;
        }

        return Result<DamageReportDto?>.Success(
            damageReportDto,
            "Reporte de danos obtenido con exito");
    }
}
