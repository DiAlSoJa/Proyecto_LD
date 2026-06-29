using System.Collections.Generic;
using System.Linq;

namespace LD.Contracts.DTOs.DeliveryOrder;

public class FinishDeliveryOrderLoadingResultDto
{
    public string DeliveryOrderCode { get; set; } = string.Empty;

    public string DeliveryOrderStatus { get; set; } = string.Empty;

    public int TotalKittings { get; set; }

    public int LoadedKittings { get; set; }

    public int PartialKittings { get; set; }

    public List<FinishDeliveryOrderKittingResultDto> Kittings { get; set; } = new();

    public bool HasPartials => PartialKittings > 0 || Kittings.Any(x => x.IsPartial);
}

public class FinishDeliveryOrderKittingResultDto
{
    public int KittingId { get; set; }

    public string KittingCode { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public int TotalIssues { get; set; }

    public int LoadedIssues { get; set; }

    public List<string> MissingLabels { get; set; } = new();

    public bool IsPartial => MissingLabels.Count > 0;
}
