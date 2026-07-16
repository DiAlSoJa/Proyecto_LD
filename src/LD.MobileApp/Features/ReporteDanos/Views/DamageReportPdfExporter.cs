using LD.Client.Configuration;
using LD.Contracts.DamageReports;
using LD.Contracts.Requests;
using Microsoft.Maui.Storage;
using SkiaSharp;
using System.Globalization;
using System.Text;

namespace MauiAppLogin;

internal static class DamageReportPdfExporter
{
    private const float PageWidth = 612f;
    private const float PageHeight = 792f;
    private const float Margin = 25.5f;
    private const float ContentWidth = 375f;
    private const float LabelWidth = 116.25f;
    private const float HeaderLogoWidth = 78.75f;
    private const float HeaderLogoHeight = 51f;
    private const float HeaderSeparatorGap = 10.5f;
    private const float HeaderContentHeight = 52f;
    private const float SectionTitleTopGap = 19.5f;
    private const float SectionTitleBottomGap = 4.5f;
    private const float PhotoTopGap = 16.5f;
    private const float PhotoBlockHeight = 150f;
    private const float FooterHeight = 39f;
    private const float FooterTopLineHeight = 9f;
    private const float FooterBarHeight = 30f;
    private const float FooterLogoWidth = 64.5f;
    private const float FooterLogoHeight = 28.5f;
    private const float FooterLogoRightMargin = 31.5f;
    private const float TablePaddingX = 3f;
    private const float TablePaddingY = 3f;
    private const float BaseFontSize = 9f;
    private const float SectionTitleFontSize = 13.5f;
    private const float CompanyTopFontSize = 12.75f;
    private const float CompanySmallFontSize = 7.5f;
    private const float RightHeaderTitleFontSize = 15f;
    private const float RightHeaderCodeFontSize = 7.5f;
    private static readonly SKTypeface TypeFace = SKTypeface.FromFamilyName("Arial") ?? SKTypeface.Default;

    public static async Task<string> CreateAsync(DamageReportRequest report, IReadOnlyList<string> photoPaths)
    {
        ArgumentNullException.ThrowIfNull(report);
        ArgumentNullException.ThrowIfNull(photoPaths);

        var reportCode = EnsureReportCode(report);
        var fileName = $"reporte-danos-{reportCode}-{DateTime.Now:yyyyMMdd-HHmmss}.pdf";
        var filePath = Path.Combine(FileSystem.CacheDirectory, fileName);
        var photoPath = GetFirstPhotoPath(photoPaths);

        using var output = File.Create(filePath);
        using var document = SKDocument.CreatePdf(output);

        using var logoBitmap = await LoadPackageBitmapAsync("logo.png");
        using var photoBitmap = await LoadBitmapAsync(photoPath);

        var canvas = document.BeginPage(PageWidth, PageHeight);
        try
        {
            canvas.Clear(SKColors.White);
            DrawPage(canvas, report, reportCode, logoBitmap, photoBitmap);
        }
        finally
        {
            document.EndPage();
            document.Close();
        }

        return filePath;
    }

    private static string EnsureReportCode(DamageReportRequest report)
    {
        var reportCode = DamageReportCodeGenerator.Normalize(report.DamageReportCode);
        if (string.IsNullOrWhiteSpace(reportCode))
        {
            reportCode = DamageReportCodeGenerator.Generate(
                report.ReportDate == default ? DateTime.Now : report.ReportDate);
            report.DamageReportCode = reportCode;
        }

        return reportCode;
    }

    private static async Task<SKBitmap?> LoadPackageBitmapAsync(string assetName)
    {
        try
        {
            await using var stream = await FileSystem.OpenAppPackageFileAsync(assetName);
            return SKBitmap.Decode(stream);
        }
        catch
        {
            return null;
        }
    }

    private static Task<SKBitmap?> LoadBitmapAsync(string? path)
    {
        return Task.Run(() =>
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                return (SKBitmap?)null;

            try
            {
                return SKBitmap.Decode(path);
            }
            catch
            {
                return null;
            }
        });
    }

    private static void DrawPage(SKCanvas canvas, DamageReportRequest report, string reportCode, SKBitmap? logoBitmap, SKBitmap? photoBitmap)
    {
        var y = Margin;

        DrawHeader(canvas, report, reportCode, logoBitmap, y);
        y += HeaderContentHeight + HeaderSeparatorGap;

        DrawHorizontalLine(canvas, Margin, y, PageWidth - (Margin * 2), 2.25f, new SKColor(0, 0, 0));
        y += 13.5f;

        y = DrawTable(canvas, y, BuildGeneralRows(report));
        y += SectionTitleTopGap;

        DrawCenteredText(canvas, "Reporte de Daño", Margin, y, PageWidth - (Margin * 2), SectionTitleFontSize, true);
        y += SectionTitleFontSize + SectionTitleBottomGap;

        y = DrawTable(canvas, y, BuildDamageRows(report));
        y += PhotoTopGap;

        DrawPhotoBlock(canvas, y, photoBitmap);
        DrawFooter(canvas, logoBitmap);
    }

    private static void DrawHeader(SKCanvas canvas, DamageReportRequest report, string reportCode, SKBitmap? logoBitmap, float top)
    {
        var logoX = Margin;
        var logoY = top;
        if (logoBitmap is not null)
        {
            DrawBitmapContain(canvas, logoBitmap, SKRect.Create(logoX, logoY, HeaderLogoWidth, HeaderLogoHeight));
        }

        var companyX = Margin + HeaderLogoWidth + 9f;
        DrawText(canvas, "LOGISTICA FLEXIBLE", companyX, logoY, CompanyTopFontSize, true);
        DrawText(canvas, "ALMACEN B1", companyX, logoY + 15f, CompanySmallFontSize, true);
        DrawText(canvas, "CALZADA JUAN GIL PRECIADO NO. 2450", companyX, logoY + 24f, CompanySmallFontSize, false);
        DrawText(canvas, "NAVE 23", companyX, logoY + 33f, CompanySmallFontSize, false);
        DrawText(canvas, "EL TIGRE", companyX, logoY + 42f, CompanySmallFontSize, false);
        DrawText(canvas, "ZAPOPAN, JALISCO", companyX, logoY + 51f, CompanySmallFontSize, false);

        var rightBoxWidth = 168f;
        var rightBoxX = PageWidth - Margin - rightBoxWidth;
        var rightBoxY = top - 1f;
        DrawRectangle(canvas, SKRect.Create(rightBoxX, rightBoxY, rightBoxWidth, 31f), 1.5f, SKColors.Black);
        DrawCenteredText(canvas, "Reporte de Daño", rightBoxX, rightBoxY + 5f, rightBoxWidth, RightHeaderTitleFontSize, true);
        DrawCenteredText(canvas, $"No. RD: {BuildReportReference(report, reportCode)}", rightBoxX, rightBoxY + 39f, rightBoxWidth, RightHeaderCodeFontSize, false);
    }

    private static float DrawTable(SKCanvas canvas, float startY, IReadOnlyList<(string Label, string Value)> rows)
    {
        var y = startY;
        foreach (var row in rows)
        {
            var rowHeight = MeasureRowHeight(row.Label, row.Value);
            DrawCell(canvas, SKRect.Create(Margin, y, LabelWidth, rowHeight), row.Label, true);
            DrawCell(canvas, SKRect.Create(Margin + LabelWidth, y, ContentWidth - LabelWidth, rowHeight), row.Value, false);
            y += rowHeight;
        }

        return y;
    }

    private static void DrawPhotoBlock(SKCanvas canvas, float top, SKBitmap? photoBitmap)
    {
        var outerRect = SKRect.Create(Margin, top, ContentWidth, PhotoBlockHeight);
        DrawRectangle(canvas, outerRect, 1f, SKColors.Black);

        var leftRect = SKRect.Create(outerRect.Left, outerRect.Top, outerRect.Width / 2f, outerRect.Height);
        var dividerPaint = CreateStrokePaint(SKColors.Black, 1f);
        canvas.DrawLine(leftRect.Right, leftRect.Top, leftRect.Right, leftRect.Bottom, dividerPaint);

        if (photoBitmap is not null)
        {
            canvas.Save();
            canvas.ClipRect(leftRect);
            DrawBitmapCover(canvas, photoBitmap, leftRect);
            canvas.Restore();
        }
    }

    private static void DrawFooter(SKCanvas canvas, SKBitmap? logoBitmap)
    {
        var footerTop = PageHeight - FooterHeight;
        DrawRectangle(canvas, SKRect.Create(0, footerTop, PageWidth, FooterTopLineHeight), 0f, SKColor.Parse("#B2B9C3"), true);
        DrawRectangle(canvas, SKRect.Create(0, footerTop + FooterTopLineHeight, PageWidth, FooterBarHeight), 0f, SKColor.Parse("#253070"), true);

        if (logoBitmap is not null)
        {
            var logoRect = SKRect.Create(
                PageWidth - Margin - FooterLogoRightMargin - FooterLogoWidth,
                footerTop + FooterTopLineHeight + ((FooterBarHeight - FooterLogoHeight) / 2f),
                FooterLogoWidth,
                FooterLogoHeight);

            DrawBitmapContain(canvas, logoBitmap, logoRect);
        }
    }

    private static void DrawCell(SKCanvas canvas, SKRect rect, string text, bool bold)
    {
        DrawRectangle(canvas, rect, 1f, SKColors.Black);
        DrawWrappedText(canvas, rect, text, BaseFontSize, bold);
    }

    private static void DrawRectangle(SKCanvas canvas, SKRect rect, float strokeWidth, SKColor color, bool fill = false)
    {
        using var paint = fill ? CreateFillPaint(color) : CreateStrokePaint(color, strokeWidth);
        if (fill)
            canvas.DrawRect(rect, paint);
        else
            canvas.DrawRect(rect, paint);
    }

    private static SKPaint CreateStrokePaint(SKColor color, float strokeWidth)
    {
        return new SKPaint
        {
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = strokeWidth,
            Color = color
        };
    }

    private static SKPaint CreateFillPaint(SKColor color)
    {
        return new SKPaint
        {
            IsAntialias = true,
            Style = SKPaintStyle.Fill,
            Color = color
        };
    }

    private static void DrawHorizontalLine(SKCanvas canvas, float x, float y, float width, float thickness, SKColor color)
    {
        using var paint = CreateStrokePaint(color, thickness);
        canvas.DrawLine(x, y, x + width, y, paint);
    }

    private static void DrawCenteredText(SKCanvas canvas, string text, float x, float top, float width, float fontSize, bool bold)
    {
        using var paint = CreateTextPaint(fontSize, bold, SKTextAlign.Center, SKColors.Black);
        var baseline = top - paint.FontMetrics.Ascent;
        canvas.DrawText(text, x + (width / 2f), baseline, paint);
    }

    private static void DrawText(SKCanvas canvas, string text, float x, float top, float fontSize, bool bold)
    {
        using var paint = CreateTextPaint(fontSize, bold, SKTextAlign.Left, SKColors.Black);
        var baseline = top - paint.FontMetrics.Ascent;
        canvas.DrawText(text, x, baseline, paint);
    }

    private static void DrawWrappedText(SKCanvas canvas, SKRect rect, string text, float fontSize, bool bold)
    {
        using var paint = CreateTextPaint(fontSize, bold, SKTextAlign.Left, SKColors.Black);
        var lines = WrapText(text, paint, rect.Width - (TablePaddingX * 2f));
        var lineHeight = fontSize * 1.3f;
        var y = rect.Top + TablePaddingY;

        foreach (var line in lines)
        {
            var baseline = y - paint.FontMetrics.Ascent;
            canvas.DrawText(line, rect.Left + TablePaddingX, baseline, paint);
            y += lineHeight;
        }
    }

    private static SKPaint CreateTextPaint(float fontSize, bool bold, SKTextAlign align, SKColor color)
    {
        return new SKPaint
        {
            IsAntialias = true,
            Color = color,
            Typeface = TypeFace,
            TextSize = fontSize,
            TextAlign = align,
            FakeBoldText = bold
        };
    }

    private static float MeasureRowHeight(string label, string value)
    {
        using var labelPaint = CreateTextPaint(BaseFontSize, true, SKTextAlign.Left, SKColors.Black);
        using var valuePaint = CreateTextPaint(BaseFontSize, false, SKTextAlign.Left, SKColors.Black);

        var labelLines = WrapText(label, labelPaint, LabelWidth - (TablePaddingX * 2f));
        var valueLines = WrapText(value, valuePaint, ContentWidth - LabelWidth - (TablePaddingX * 2f));
        var lineHeight = BaseFontSize * 1.3f;
        var labelHeight = (labelLines.Count * lineHeight) + (TablePaddingY * 2f);
        var valueHeight = (valueLines.Count * lineHeight) + (TablePaddingY * 2f);

        return Math.Max(labelHeight, valueHeight);
    }

    private static List<string> WrapText(string text, SKPaint paint, float maxWidth)
    {
        var normalized = NormalizeText(text);
        var lines = new List<string>();

        foreach (var paragraph in normalized.Replace("\r", string.Empty).Split('\n'))
        {
            if (string.IsNullOrWhiteSpace(paragraph))
            {
                lines.Add(string.Empty);
                continue;
            }

            var words = paragraph.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var current = new StringBuilder();

            foreach (var word in words)
            {
                var candidate = current.Length == 0 ? word : $"{current} {word}";
                if (paint.MeasureText(candidate) <= maxWidth)
                {
                    current.Clear();
                    current.Append(candidate);
                    continue;
                }

                if (current.Length > 0)
                {
                    lines.Add(current.ToString());
                    current.Clear();
                }

                if (paint.MeasureText(word) <= maxWidth)
                {
                    current.Append(word);
                    continue;
                }

                lines.AddRange(BreakLongWord(word, paint, maxWidth));
            }

            if (current.Length > 0)
                lines.Add(current.ToString());
        }

        return lines.Count == 0 ? new List<string> { string.Empty } : lines;
    }

    private static IEnumerable<string> BreakLongWord(string word, SKPaint paint, float maxWidth)
    {
        var current = new StringBuilder();
        foreach (var ch in word)
        {
            var candidate = current.Length == 0 ? ch.ToString() : current + ch.ToString();
            if (paint.MeasureText(candidate) <= maxWidth)
            {
                current.Append(ch);
                continue;
            }

            if (current.Length > 0)
            {
                yield return current.ToString();
                current.Clear();
            }

            current.Append(ch);
        }

        if (current.Length > 0)
            yield return current.ToString();
    }

    private static string NormalizeText(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? "-"
            : value.Trim();
    }

    private static void DrawBitmapCover(SKCanvas canvas, SKBitmap bitmap, SKRect rect)
    {
        var scale = Math.Max(rect.Width / bitmap.Width, rect.Height / bitmap.Height);
        var scaledWidth = bitmap.Width * scale;
        var scaledHeight = bitmap.Height * scale;
        var offsetX = rect.Left + ((rect.Width - scaledWidth) / 2f);
        var offsetY = rect.Top + ((rect.Height - scaledHeight) / 2f);
        var dest = SKRect.Create(offsetX, offsetY, scaledWidth, scaledHeight);
        canvas.DrawBitmap(bitmap, dest);
    }

    private static void DrawBitmapContain(SKCanvas canvas, SKBitmap bitmap, SKRect rect)
    {
        var scale = Math.Min(rect.Width / bitmap.Width, rect.Height / bitmap.Height);
        var scaledWidth = bitmap.Width * scale;
        var scaledHeight = bitmap.Height * scale;
        var offsetX = rect.Left + ((rect.Width - scaledWidth) / 2f);
        var offsetY = rect.Top + ((rect.Height - scaledHeight) / 2f);
        var dest = SKRect.Create(offsetX, offsetY, scaledWidth, scaledHeight);
        canvas.DrawBitmap(bitmap, dest);
    }

    private static string BuildReportReference(DamageReportRequest report, string reportCode)
    {
        return report.DamageReportId > 0
            ? report.DamageReportId.ToString(CultureInfo.InvariantCulture)
            : reportCode;
    }

    private static IReadOnlyList<(string Label, string Value)> BuildGeneralRows(DamageReportRequest report) =>
    [
        ("Cliente:", FormatValue(report.Client)),
        ("Proyecto:", FormatValue(report.Project)),
        ("No. ASN:", FormatValue(report.Asn)),
        ("No. Recepcion:", FormatValue(report.AvailableInventoryId?.ToString(CultureInfo.InvariantCulture))),
        ("Fecha Recepcion:", FormatDate(report.ReceptionDate)),
        ("EstandarID:", FormatValue(report.StandardIdCode ?? report.StandardId?.ToString(CultureInfo.InvariantCulture))),
        ("No. Parte:", FormatValue(report.PartNumber)),
        ("QTY:", FormatQuantity(report.AvailableQuantity ?? report.ReceivedQuantity)),
        ("Ubicacion:", FormatValue(report.Location)),
        ("Status:", FormatValue(report.CurrentStatus))
    ];

    private static IReadOnlyList<(string Label, string Value)> BuildDamageRows(DamageReportRequest report) =>
    [
        ("Tipo de Daño:", FormatValue(report.DamageType)),
        ("Categoria:", FormatValue(report.Category)),
        ("Nuevo Status:", FormatValue(report.NewStatus)),
        ("Comentarios:", FormatValue(report.Comments)),
        ("Usuario Captura:", FormatValue(report.ReportedByName ?? UserData.Name ?? UserData.UserName)),
        ("Hora Captura:", FormatDateTime(report.ReportDate))
    ];

    private static string FormatValue(string? value) => string.IsNullOrWhiteSpace(value) ? "-" : value.Trim();

    private static string FormatDate(DateTime? value) => value?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) ?? "-";

    private static string FormatDateTime(DateTime? value) => value?.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture) ?? "-";

    private static string FormatQuantity(decimal? value) => value?.ToString(CultureInfo.InvariantCulture) ?? "-";

    private static string GetFirstPhotoPath(IReadOnlyList<string> photoPaths)
    {
        return photoPaths.FirstOrDefault(path => !string.IsNullOrWhiteSpace(path) && File.Exists(path)) ?? string.Empty;
    }
}
