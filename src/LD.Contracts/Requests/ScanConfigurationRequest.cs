namespace LD.Contracts.Requests
{
    public class ScanConfigurationRequest
    {
        public int SystemFieldId { get; set; }
        public string SystemFieldName { get; set; } = "";
        public string ClientField { get; set; } = "";
        public bool IsRequired { get; set; }
        public int? ScanTypeId { get; set; }
        public string? ScanValue { get; set; }
        public int? SaveTypeId { get; set; }
        public int SaveValue { get; set; }

        public int Order { get; set; }
    }
}
