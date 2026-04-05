namespace LD.FormsX.Model.Lookup
{
    public class LookupItem
    {
        public object? Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public object? Data { get; set; }

        public string DisplayText => string.IsNullOrWhiteSpace(Description)
            ? Code
            : $"{Code} - {Description}";

        public override string ToString()
        {
            return Code;
        }
    }
}