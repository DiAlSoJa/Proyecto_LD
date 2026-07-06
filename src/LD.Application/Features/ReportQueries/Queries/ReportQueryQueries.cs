using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.DTOs.ReportQueries;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.ReportQueries.Queries;

public class ReportQueryQuery : IRequest<Result<List<ReportQueryDto>?>>
{
}

public class ReportQueryQueryHandler : IRequestHandler<ReportQueryQuery, Result<List<ReportQueryDto>?>>
{
    private readonly IReportQueryRepository _repository;
    private readonly IMapper _mapper;

    public ReportQueryQueryHandler(IReportQueryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<List<ReportQueryDto>?>> Handle(ReportQueryQuery request, CancellationToken cancellationToken)
    {
        var queries = await _repository.GetManyAsync() ?? [];
        var result = queries
            .Where(x => x.IsActive)
            .OrderBy(x => x.Name)
            .ThenBy(x => x.ReportQueryId)
            .ToList();

        return Result<List<ReportQueryDto>?>.Success(
            _mapper.Map<List<ReportQueryDto>>(result),
            "Consultas obtenidas correctamente");
    }
}

public class ReportQuerySummaryQuery : IRequest<Result<List<ReportQuerySummaryDto>?>>
{
}

public class ReportQuerySummaryQueryHandler : IRequestHandler<ReportQuerySummaryQuery, Result<List<ReportQuerySummaryDto>?>>
{
    private readonly IReportQueryRepository _repository;
    private readonly IMapper _mapper;

    public ReportQuerySummaryQueryHandler(IReportQueryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<List<ReportQuerySummaryDto>?>> Handle(ReportQuerySummaryQuery request, CancellationToken cancellationToken)
    {
        var queries = await _repository.GetManyAsync() ?? [];
        var result = queries
            .Where(x => x.IsActive)
            .OrderBy(x => x.Name)
            .ThenBy(x => x.ReportQueryId)
            .ToList();

        return Result<List<ReportQuerySummaryDto>?>.Success(
            _mapper.Map<List<ReportQuerySummaryDto>>(result),
            "Consultas obtenidas correctamente");
    }
}

public class ReportQueryByIdQuery : IRequest<Result<ReportQueryDto?>>
{
    public ReportQueryByIdQuery(int reportQueryId)
    {
        ReportQueryId = reportQueryId;
    }

    public int ReportQueryId { get; }
}

public class ReportQueryByIdQueryHandler : IRequestHandler<ReportQueryByIdQuery, Result<ReportQueryDto?>>
{
    private readonly IReportQueryRepository _repository;
    private readonly IMapper _mapper;

    public ReportQueryByIdQueryHandler(IReportQueryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<ReportQueryDto?>> Handle(ReportQueryByIdQuery request, CancellationToken cancellationToken)
    {
        var query = await _repository.GetByIdAsync(request.ReportQueryId);
        if (query is null)
            return Result<ReportQueryDto?>.Failure("No se encontró la consulta.", new List<string> { "La consulta solicitada no existe." }, 404);

        return Result<ReportQueryDto?>.Success(
            _mapper.Map<ReportQueryDto>(query),
            "Consulta obtenida correctamente");
    }
}

public class GetReportQueryParametersQuery : IRequest<Result<List<ReportQueryParameterDto>?>>
{
    public GetReportQueryParametersQuery(int reportQueryId)
    {
        ReportQueryId = reportQueryId;
    }

    public int ReportQueryId { get; }
}

public class GetReportQueryParametersQueryHandler : IRequestHandler<GetReportQueryParametersQuery, Result<List<ReportQueryParameterDto>?>>
{
    private readonly IReportQueryRepository _repository;

    public GetReportQueryParametersQueryHandler(IReportQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<ReportQueryParameterDto>?>> Handle(GetReportQueryParametersQuery request, CancellationToken cancellationToken)
    {
        var query = await _repository.GetByIdAsync(request.ReportQueryId);
        if (query is null)
            return Result<List<ReportQueryParameterDto>?>.Failure("No se encontró la consulta.", new List<string> { "La consulta solicitada no existe." }, 404);

        var parameters = ReportQuerySqlParser.GetParameterNames(query.SqlQuery)
            .Select(name => new ReportQueryParameterDto { Nombre = name })
            .ToList();

        return Result<List<ReportQueryParameterDto>?>.Success(
            parameters,
            "Parámetros obtenidos correctamente");
    }
}
