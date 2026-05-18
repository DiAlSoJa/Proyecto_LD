using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
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
    public int? WarehouseId { get; set; }
    public string? Warehouse { get; set; }
    public string? PartNumber { get; set; }
    public string? DamageType { get; set; }
}

public class DamageReportQueryHandler : IRequestHandler<DamageReportQuery, Result<List<DamageReportDto>?>>
{
    private readonly IRepository<DamageReport> _repository;
    private readonly IApplicationUserManager _applicationUserManager;
    private readonly IMapper _mapper;

    public DamageReportQueryHandler(
        IRepository<DamageReport> repository,
        IApplicationUserManager applicationUserManager,
        IMapper mapper)
    {
        _repository = repository;
        _applicationUserManager = applicationUserManager;
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

        if (request.WarehouseId.HasValue || !string.IsNullOrWhiteSpace(request.Warehouse))
        {
            var warehouse = request.Warehouse?.Trim();
            query = query.Where(x =>
                request.WarehouseId.HasValue && x.WarehouseId == request.WarehouseId.Value
                || !string.IsNullOrWhiteSpace(warehouse)
                    && !string.IsNullOrWhiteSpace(x.Warehouse)
                    && x.Warehouse.Contains(warehouse, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(request.PartNumber))
            query = query.Where(x => !string.IsNullOrWhiteSpace(x.PartNumber)
                && x.PartNumber.Contains(request.PartNumber.Trim(), StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(request.DamageType))
            query = query.Where(x => !string.IsNullOrWhiteSpace(x.DamageType)
                && x.DamageType.Contains(request.DamageType.Trim(), StringComparison.OrdinalIgnoreCase));

        var result = query
            .OrderByDescending(x => x.ReportDate)
            .ThenByDescending(x => x.DamageReportId)
            .ToList();

        var damageReportDtos = _mapper.Map<List<DamageReportDto>>(result);
        await FillUserNamesAsync(damageReportDtos);

        return Result<List<DamageReportDto>?>.Success(
            damageReportDtos,
            "Reportes de danos obtenidos correctamente");
    }

    private async Task FillUserNamesAsync(List<DamageReportDto> reports)
    {
        var userIds = reports
            .Select(x => x.CreatedByUserId)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x!)
            .Distinct()
            .ToList();

        var users = new Dictionary<string, string>();
        foreach (var userId in userIds)
        {
            var user = await _applicationUserManager.GetUserByIdAsync(userId);
            users[userId] = user?.Username ?? user?.Name ?? userId;
        }

        foreach (var report in reports)
        {
            report.CreatedByUserName = !string.IsNullOrWhiteSpace(report.CreatedByUserId)
                && users.TryGetValue(report.CreatedByUserId, out var userName)
                    ? userName
                    : report.CreatedByUserId;
        }
    }
}
