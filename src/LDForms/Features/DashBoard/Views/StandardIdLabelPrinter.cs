using LD.Contracts.ASN;
using LD.FormsX.Views.ASN;

namespace LDForms;

internal static class StandardIdLabelPrinter
{
    public static void PrintLabels(IReadOnlyList<string> standardIds)
    {
        var receiptDetails = standardIds
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => new AsnReceiptDetailDto
            {
                StandardId = x.Trim()
            })
            .ToList();

        if (receiptDetails.Count == 0)
            return;

        AsnReceiptLabelPrinter.PrintLabels(receiptDetails, null, null);
    }
}
