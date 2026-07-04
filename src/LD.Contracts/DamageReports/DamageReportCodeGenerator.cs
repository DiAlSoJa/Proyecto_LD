using System.Globalization;

namespace LD.Contracts.DamageReports;

public static class DamageReportCodeGenerator
{
    public static string Generate(DateTime? reportDate = null, Guid? seed = null)
    {
        var datePart = (reportDate ?? DateTime.Now).ToString("yyyyMMdd", CultureInfo.InvariantCulture);
        var guidPart = (seed ?? Guid.NewGuid()).ToString("N")[..12].ToUpperInvariant();
        return $"{datePart}-{guidPart}";
    }

    public static string Normalize(string? code)
    {
        return string.IsNullOrWhiteSpace(code)
            ? string.Empty
            : code.Trim().ToUpperInvariant();
    }

    public static string BuildPublicUrl(string? code)
    {
        var normalizedCode = Normalize(code);
        return string.IsNullOrWhiteSpace(normalizedCode)
            ? string.Empty
            : $"https://rd.ld.com.mx/{Uri.EscapeDataString(normalizedCode)}";
    }
}
