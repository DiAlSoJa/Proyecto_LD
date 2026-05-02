namespace LD.Contracts.DTOs
{
    public class UserProjectClientDto
    {
        public int ClientId { get; set; }
        public int ProjectId { get; set; }
        public int WarehouseId { get; set; }
        public string Client { get; set; } = string.Empty;
        public string Project { get; set; } = string.Empty;
        public string Warehouse { get; set; } = string.Empty;
    }
}
