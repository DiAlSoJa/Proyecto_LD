namespace LD.Contracts.Driver;

public class DriverDto
{
    public int DriverId { get; set; }
    public string DriverNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Licence { get; set; } = string.Empty;
    public string IMSS { get; set; } = string.Empty;
}
