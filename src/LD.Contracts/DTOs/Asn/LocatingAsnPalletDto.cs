namespace LD.Contracts.ASN;

public class LocatingAsnPalletDto
{
    public int AsnId { get; set; }
    public string AsnCode { get; set; } = string.Empty;
    public int PalletsPorMover { get; set; }
    public string LocationCode { get; set; } = string.Empty;
}
