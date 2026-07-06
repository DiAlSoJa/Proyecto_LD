using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.DTOs.ReportQueries;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LD.Application.Features.ReportQueries.Commands;

public class CreateReportQueryCommand : ReportQueryRequest, IRequest<Result<string>>
{
}

public class CreateReportQueryCommandHandler : IRequestHandler<CreateReportQueryCommand, Result<string>>
{
    private readonly IReportQueryRepository _repository;
    private readonly IMapper _mapper;

    public CreateReportQueryCommandHandler(IReportQueryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateReportQueryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var normalizedName = request.Nombre?.Trim() ?? string.Empty;
            var normalizedQuery = request.Query?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(normalizedName))
                return Result<string>.Failure("El nombre de la consulta es obligatorio.", new List<string> { "Ingresa un nombre." });

            if (string.IsNullOrWhiteSpace(normalizedQuery))
                return Result<string>.Failure("La consulta SQL es obligatoria.", new List<string> { "Ingresa la consulta SQL." });

            if (!ReportQuerySqlParser.IsReadOnlyQuery(normalizedQuery))
                return Result<string>.Failure("La consulta debe ser de solo lectura.", new List<string> { "Solo se permiten consultas SELECT o WITH." });

            var existingQueries = await _repository.GetManyAsync() ?? [];
            if (existingQueries.Any(x => x.IsActive && string.Equals(x.Name?.Trim(), normalizedName, StringComparison.OrdinalIgnoreCase)))
                return Result<string>.Failure("Ya existe una consulta con ese nombre.", new List<string> { "El nombre debe ser único." }, 409);

            var entity = _mapper.Map<ReportQuery>(request);
            entity.Name = normalizedName;
            entity.SqlQuery = normalizedQuery;

            var result = await _repository.CreateAsync(entity);
            return result
                ? Result<string>.Success("Consulta creada correctamente", "")
                : Result<string>.Failure("No se pudo crear la consulta.", new List<string> { "Ocurrió un error al guardar la consulta." });
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("No se pudo crear la consulta.", new List<string> { ex.Message });
        }
    }
}

public class UpdateReportQueryCommand : ReportQueryRequest, IRequest<Result<string>>
{
    public int ReportQueryId { get; set; }
}

public class UpdateReportQueryCommandHandler : IRequestHandler<UpdateReportQueryCommand, Result<string>>
{
    private readonly IReportQueryRepository _repository;
    private readonly IMapper _mapper;

    public UpdateReportQueryCommandHandler(IReportQueryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(UpdateReportQueryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(request.ReportQueryId);
            if (entity is null)
                return Result<string>.Failure("No se encontró la consulta.", new List<string> { "La consulta solicitada no existe." }, 404);

            var normalizedName = request.Nombre?.Trim() ?? string.Empty;
            var normalizedQuery = request.Query?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(normalizedName))
                return Result<string>.Failure("El nombre de la consulta es obligatorio.", new List<string> { "Ingresa un nombre." });

            if (string.IsNullOrWhiteSpace(normalizedQuery))
                return Result<string>.Failure("La consulta SQL es obligatoria.", new List<string> { "Ingresa la consulta SQL." });

            if (!ReportQuerySqlParser.IsReadOnlyQuery(normalizedQuery))
                return Result<string>.Failure("La consulta debe ser de solo lectura.", new List<string> { "Solo se permiten consultas SELECT o WITH." });

            var existingQueries = await _repository.GetManyAsync() ?? [];
            if (existingQueries.Any(x => x.IsActive
                    && x.ReportQueryId != request.ReportQueryId
                    && string.Equals(x.Name?.Trim(), normalizedName, StringComparison.OrdinalIgnoreCase)))
            {
                return Result<string>.Failure("Ya existe una consulta con ese nombre.", new List<string> { "El nombre debe ser único." }, 409);
            }

            _mapper.Map(request, entity);
            entity.Name = normalizedName;
            entity.SqlQuery = normalizedQuery;

            var result = await _repository.UpdateAsync(entity);
            return result
                ? Result<string>.Success("Consulta actualizada correctamente", "")
                : Result<string>.Failure("No se pudo actualizar la consulta.", new List<string> { "Ocurrió un error al actualizar la consulta." });
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("No se pudo actualizar la consulta.", new List<string> { ex.Message });
        }
    }
}

public class DeleteReportQueryCommand : IRequest<Result<string>>
{
    public DeleteReportQueryCommand(int reportQueryId)
    {
        ReportQueryId = reportQueryId;
    }

    public int ReportQueryId { get; }
}

public class DeleteReportQueryCommandHandler : IRequestHandler<DeleteReportQueryCommand, Result<string>>
{
    private readonly IReportQueryRepository _repository;

    public DeleteReportQueryCommandHandler(IReportQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<string>> Handle(DeleteReportQueryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(request.ReportQueryId);
            if (entity is null)
                return Result<string>.Failure("No se encontró la consulta.", new List<string> { "La consulta solicitada no existe." }, 404);

            var result = await _repository.DeleteAsync(entity);
            return result
                ? Result<string>.Success("Consulta eliminada correctamente", "")
                : Result<string>.Failure("No se pudo eliminar la consulta.", new List<string> { "Ocurrió un error al eliminar la consulta." });
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("No se pudo eliminar la consulta.", new List<string> { ex.Message });
        }
    }
}

public class ExecuteReportQueryCommand : IRequest<Result<ReportQueryExecutionResultDto>>
{
    public ExecuteReportQueryCommand(int reportQueryId, Dictionary<string, string?>? parameters = null)
    {
        ReportQueryId = reportQueryId;
        Parameters = parameters is null
            ? new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase)
            : new Dictionary<string, string?>(parameters, StringComparer.OrdinalIgnoreCase);
    }

    public int ReportQueryId { get; }

    public Dictionary<string, string?> Parameters { get; }
}

public class ExecuteReportQueryCommandHandler : IRequestHandler<ExecuteReportQueryCommand, Result<ReportQueryExecutionResultDto>>
{
    private readonly IReportQueryRepository _repository;

    public ExecuteReportQueryCommandHandler(IReportQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<ReportQueryExecutionResultDto>> Handle(ExecuteReportQueryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(request.ReportQueryId);
            if (entity is null)
                return Result<ReportQueryExecutionResultDto>.Failure("No se encontró la consulta.", new List<string> { "La consulta solicitada no existe." }, 404);

            if (!ReportQuerySqlParser.IsReadOnlyQuery(entity.SqlQuery))
                return Result<ReportQueryExecutionResultDto>.Failure("La consulta no es válida para ejecución.", new List<string> { "Solo se permiten consultas de solo lectura." });

            var expectedParameters = ReportQuerySqlParser.GetParameterNames(entity.SqlQuery);
            var suppliedParameters = request.Parameters ?? new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

            var missingParameters = expectedParameters
                .Where(name => !suppliedParameters.ContainsKey(name))
                .ToList();

            if (missingParameters.Count > 0)
            {
                return Result<ReportQueryExecutionResultDto>.Failure(
                    "Faltan parámetros requeridos.",
                    missingParameters.Select(name => $"Falta el parámetro @{name}.").ToList());
            }

            var executionResult = await _repository.ExecuteAsync(entity.SqlQuery, suppliedParameters, cancellationToken);
            executionResult.ReportQueryId = entity.ReportQueryId;
            executionResult.Nombre = entity.Name;

            return Result<ReportQueryExecutionResultDto>.Success(
                executionResult,
                "Consulta ejecutada correctamente");
        }
        catch (Exception ex)
        {
            return Result<ReportQueryExecutionResultDto>.Failure("No se pudo ejecutar la consulta.", new List<string> { ex.Message });
        }
    }
}
