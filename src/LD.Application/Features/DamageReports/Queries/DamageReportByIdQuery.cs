using AutoMapper;
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
    private readonly IMapper _mapper;

    public DamageReportByIdQueryHandler(IRepository<DamageReport> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<DamageReportDto?>> Handle(DamageReportByIdQuery request, CancellationToken cancellationToken)
    {
        var damageReport = await _repository.GetByIdAsync(request.DamageReportId);
        if (damageReport is null)
            return Result<DamageReportDto?>.Failure("Reporte de daños no encontrado", new(), 404);

        return Result<DamageReportDto?>.Success(
            _mapper.Map<DamageReportDto>(damageReport),
            "Reporte de daños obtenido con exito");
    }
}
