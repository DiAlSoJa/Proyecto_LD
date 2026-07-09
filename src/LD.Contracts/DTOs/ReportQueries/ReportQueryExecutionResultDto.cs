using System.Collections.Generic;

namespace LD.Contracts.DTOs.ReportQueries;

public class ReportQueryExecutionResultDto
{
    public int ReportQueryId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public List<string> Columns { get; set; } = [];

    public List<Dictionary<string, string?>> Rows { get; set; } = [];

    public int RowCount => Rows.Count;
}
