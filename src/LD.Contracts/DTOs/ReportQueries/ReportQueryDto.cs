using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.DTOs.ReportQueries;

public class ReportQueryDto
{
    public int ReportQueryId { get; set; }

    [DisplayName("Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [DisplayName("Query")]
    public string Query { get; set; } = string.Empty;
}
