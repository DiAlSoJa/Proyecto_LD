using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.DamageReports;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.DamageReports.Queries;

public class DamageReportQuery : IRequest<Result<List<DamageReportDto>?>>
{
    public DateTime? Desde { get; set; }
    public DateTime? Hasta { get; set; }
    public int? StandardId { get; set; }
    public string? PartNumber { get; set; }
}

public class DamageReportQueryHandler : IRequestHandler<DamageReportQuery, Result<List<DamageReportDto>?>>
{
    private readonly IRepository<DamageReport> _repository;
    private readonly IMapper _mapper;

    public DamageReportQueryHandler(IRepository<DamageReport> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<List<DamageReportDto>?>> Handle(DamageReportQuery request, CancellationToken cancellationToken)
    {
        var reports = await _repository.GetManyAsync() ?? new List<DamageReport>();
        var query = reports.AsEnumerable();

        if (request.Desde.HasValue)
            query = query.Where(x => x.ReportDate.Date >= request.Desde.Value.Date);

        if (request.Hasta.HasValue)
            query = query.Where(x => x.ReportDate.Date <= request.Hasta.Value.Date);

        if (request.StandardId.HasValue)
            query = query.Where(x => x.StandardId == request.StandardId.Value);

        if (!string.IsNullOrWhiteSpace(request.PartNumber))
            query = query.Where(x => string.Equals(x.PartNumber, request.PartNumber.Trim(), StringComparison.OrdinalIgnoreCase));

        var result = query
            .OrderByDescending(x => x.ReportDate)
            .ThenByDescending(x => x.DamageReportId)
            .ToList();

        return Result<List<DamageReportDto>?>.Success(
            _mapper.Map<List<DamageReportDto>>(result),
            "Reportes de daños obtenidos correctamente");
    }
}
