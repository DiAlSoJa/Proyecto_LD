namespace LD.Contracts.Requests;

public class TruckTypeRequest
{
    public int TruckTypeId { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool TieneCaja { get; set; }
}
