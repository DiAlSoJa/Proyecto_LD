using System.Collections.Generic;

namespace LD.Contracts.Requests;

public class CreateDeliveryOrderRequest
{
    public List<int> KittingIds { get; set; } = new();
}
