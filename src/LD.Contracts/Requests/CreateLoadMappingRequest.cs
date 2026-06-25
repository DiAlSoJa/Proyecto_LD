namespace LD.Contracts.Requests;

public class CreateLoadMappingRequest
{
    public int ClientId { get; set; }

    public int ProjectId { get; set; }

    public string? DeliveryOrderCode { get; set; }
}
