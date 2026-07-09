using System;
using System.Collections.Generic;

namespace LD.Contracts.Requests;

public class ReportQueryExecutionRequest
{
    public Dictionary<string, string?> Parameters { get; set; } = new(StringComparer.OrdinalIgnoreCase);
}
