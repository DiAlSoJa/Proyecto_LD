using LD.Contracts.DTOs.ReportQueries;
using LD.Domain.Entities;

namespace LD.Application.Common.Interfaces.Repository;

public interface IReportQueryRepository : IRepository<ReportQuery>
{
    Task<ReportQueryExecutionResultDto> ExecuteAsync(
        string sqlQuery,
        IReadOnlyDictionary<string, string?> parameters,
        CancellationToken cancellationToken = default);
}
