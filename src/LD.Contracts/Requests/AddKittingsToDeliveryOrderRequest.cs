using System.Collections.Generic;

namespace LD.Contracts.Requests;

public class AddKittingsToDeliveryOrderRequest
{
    public string? DeliveryOrderCode { get; set; }

    public List<int> KittingIds { get; set; } = new();
}
