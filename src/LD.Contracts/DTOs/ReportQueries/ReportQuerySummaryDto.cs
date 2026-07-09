using System.ComponentModel;

namespace LD.Contracts.DTOs.ReportQueries;

public class ReportQuerySummaryDto
{
    public int ReportQueryId { get; set; }

    [DisplayName("Nombre")]
    public string Nombre { get; set; } = string.Empty;
}
