using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LD.Contracts.Kitting;
using LD.FormsX.Features.Common;

namespace LD.FormsX.Features.Surtidos.Views;

internal static class ListaSurtidoPrinter
{
    private const double Dpi = 96d;
    private const double PointToDip = Dpi / 72d;
    private const double PdfPageHeight = 792d;
    private const double PageWidth = 8.5d * Dpi;
    private const double PageHeight = 11d * Dpi;
    private const int RowsPerPage = 18;

    private static readonly FontFamily Arial = new("Arial");
    private static readonly FontFamily Times = new("Times New Roman");
    private static readonly FontFamily Courier = new("Courier New");

    internal sealed class ListaSurtidoRow
    {
        public string PartNumber { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public string Status { get; set; } = string.Empty;
        public string LotNumber { get; set; } = string.Empty;
        public string SD { get; set; } = string.Empty;
        public string LocationCode { get; set; } = string.Empty;
    }

    public static void Print(KittingDto? kitting, IReadOnlyList<ListaSurtidoRow> rows)
    {
        if (rows.Count == 0)
            throw new InvalidOperationException("No hay lineas surtidas para mostrar en vista previa.");

        var document = BuildDocument(kitting, rows);
        ShowPreview(document, "Vista previa - Lista de surtido", "Lista de surtido");
    }

    private static FixedDocument BuildDocument(KittingDto? kitting, IReadOnlyList<ListaSurtidoRow> rows)
    {
        var document = new FixedDocument
        {
            DocumentPaginator = { PageSize = new Size(PageWidth, PageHeight) }
        };

        var pageCount = (int)Math.Ceiling(rows.Count / (double)RowsPerPage);
        var totalQuantity = rows.Sum(x => x.Quantity);
        var totalPallets = rows.Count;
        var printedAt = DateTime.Now;

        for (var pageNumber = 1; pageNumber <= pageCount; pageNumber++)
        {
            var pageRows = rows
                .Skip((pageNumber - 1) * RowsPerPage)
                .Take(RowsPerPage)
                .ToList();

            var page = BuildPage(kitting, pageRows, pageRows.Sum(x => x.Quantity), totalQuantity, totalPallets, printedAt, pageNumber, pageCount);
            page.Measure(new Size(PageWidth, PageHeight));
            page.Arrange(new Rect(0, 0, PageWidth, PageHeight));
            page.UpdateLayout();

            var pageContent = new PageContent();
            ((IAddChild)pageContent).AddChild(page);
            document.Pages.Add(pageContent);
        }

        return document;
    }

    private static void ShowPreview(FixedDocument document, string title, string jobName)
    {
        var preview = new PrintPreviewWindow(document, title, jobName);

        WindowOwnerHelper.AttachOwnerOrCenter(preview, Application.Current?.MainWindow);

        preview.ShowDialog();
    }

    private static FixedPage BuildPage(
        KittingDto? kitting,
        IReadOnlyList<ListaSurtidoRow> rows,
        decimal pageQuantity,
        decimal totalQuantity,
        int totalPallets,
        DateTime printedAt,
        int pageNumber,
        int pageCount)
    {
        var page = new FixedPage
        {
            Width = PageWidth,
            Height = PageHeight,
            Background = Brushes.White
        };

        var canvas = new Canvas
        {
            Width = PageWidth,
            Height = PageHeight,
            Background = Brushes.White
        };

        page.Children.Add(canvas);

        BuildHeader(canvas, kitting);
        BuildRowsTable(canvas, rows, pageQuantity, totalQuantity, totalPallets);
        BuildSignatures(canvas, kitting);
        BuildPageFooter(canvas, printedAt, pageNumber, pageCount);

        return page;
    }

    private static void BuildHeader(Canvas canvas, KittingDto? kitting)
    {
        AddBaselineText(canvas, "LOGISTICA FLEXIBLE", 9.1, 762.6, 12, FontWeights.Black, Arial);
        AddTitleBox(canvas);
        AddLogo(canvas);

        var addressLines = SplitAddress(FormatValue(kitting?.Direccion));
        AddTopText(canvas, addressLines.ElementAtOrDefault(0) ?? string.Empty, 11, 64, 16, FontWeights.Bold, Times, 150, TextAlignment.Center);
        AddTopText(canvas, addressLines.ElementAtOrDefault(1) ?? string.Empty, 11, 82, 16, FontWeights.Bold, Times, 150, TextAlignment.Center);

        AddBaselineText(canvas, FormatValue(kitting?.Project), 327.8, 719.3, 10, FontWeights.Bold, Times, 55, TextAlignment.Center);
        AddBaselineText(canvas, BuildInfoLine(kitting), 304.7, 686.5, 10, FontWeights.Bold, Times, 260);
        AddHorizontalLine(canvas, 8, 119, 572, 3.2, Brushes.Black);
    }

    private static void AddTitleBox(Canvas canvas)
    {
        var title = new TextBlock
        {
            Text = "LISTA DE SURTIDO",
            FontFamily = Arial,
            FontSize = Pt(14),
            FontWeight = FontWeights.Black,
            TextAlignment = TextAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Stretch
        };

        var inner = new Border
        {
            BorderBrush = Brushes.Black,
            BorderThickness = new Thickness(1),
            Margin = new Thickness(Pt(1.8)),
            Child = title
        };

        var outer = new Border
        {
            Width = Pt(285),
            Height = Pt(29),
            BorderBrush = Brushes.Black,
            BorderThickness = new Thickness(1),
            Child = inner
        };

        Place(canvas, outer, 210, 18.5);
    }

    private static void AddLogo(Canvas canvas)
    {
        var logo = new Image
        {
            Width = Pt(105),
            Height = Pt(56),
            Opacity = 0.35,
            Stretch = Stretch.Uniform,
            Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/logo.png"))
        };

        Place(canvas, logo, 510, 14);
    }

    private static void BuildRowsTable(
        Canvas canvas,
        IReadOnlyList<ListaSurtidoRow> rows,
        decimal pageQuantity,
        decimal totalQuantity,
        int totalPallets)
    {
        const double tableLeft = 7;
        const double tableTop = 133;
        const double headerHeight = 16;
        const double rowHeight = 15.85;

        var columns = new[]
        {
            new TableColumn("No Parte", 87, TextAlignment.Center),
            new TableColumn("Descripción", 130, TextAlignment.Center),
            new TableColumn("Cantidad", 47, TextAlignment.Center),
            new TableColumn("Status", 34, TextAlignment.Center),
            new TableColumn("No Lote", 72, TextAlignment.Center),
            new TableColumn("SD", 18, TextAlignment.Center),
            new TableColumn("Ubicación", 56, TextAlignment.Center)
        };

        var x = tableLeft;
        foreach (var column in columns)
        {
            AddTableHeaderCell(canvas, column.Header, x, tableTop, column.Width, headerHeight, column.Alignment);
            x += column.Width;
        }

        var rowTop = tableTop + headerHeight;
        for (var i = 0; i < rows.Count; i++)
        {
            var row = rows[i];
            var currentTop = rowTop + (i * rowHeight);

            AddRowText(canvas, row.PartNumber, 9.1, currentTop + 4, 7, 82, TextAlignment.Left);
            AddRowText(canvas, row.Description, 96.4, currentTop + 4, 8, 120, TextAlignment.Left);
            AddRowText(canvas, FormatQuantity(row.Quantity), 223, currentTop + 4, 8, 45, TextAlignment.Right);
            AddRowText(canvas, FormatStatus(row.Status), 271, currentTop + 4, 8, 30, TextAlignment.Center);
            AddRowText(canvas, row.LotNumber, 304, currentTop + 4, 8, 72, TextAlignment.Right);
            AddRowText(canvas, row.SD, 377, currentTop + 4, 8, 18, TextAlignment.Center, FontWeights.Bold);
            AddRowText(canvas, row.LocationCode, 398, currentTop + 4, 8, 52, TextAlignment.Right);
            AddHorizontalLine(canvas, 8, currentTop + rowHeight - 1, 445, 0.55, Brushes.LightGray);
        }

        var totalTop = rowTop + (rows.Count * rowHeight);
        AddRowText(canvas, FormatQuantity(pageQuantity), 223, totalTop + 4, 8, 45, TextAlignment.Right, FontWeights.Bold);
        AddHorizontalLine(canvas, 8, totalTop + rowHeight - 1, 445, 0.55, Brushes.LightGray);

        AddRowText(canvas, FormatQuantity(totalQuantity), 223, totalTop + rowHeight + 4, 8, 45, TextAlignment.Right, FontWeights.Bold);
        AddHorizontalLine(canvas, 8, totalTop + (rowHeight * 2) - 1, 370, 0.55, Brushes.LightGray);
        AddHorizontalLine(canvas, 396, totalTop + (rowHeight * 2) - 1, 56, 0.55, Brushes.LightGray);

        AddRowText(canvas, $"Pallets: {totalPallets}", 189.6, totalTop + (rowHeight * 2) + 5, 12, 90, TextAlignment.Center, FontWeights.Bold);
        AddHorizontalLine(canvas, 8, totalTop + (rowHeight * 3) - 1, 370, 0.55, Brushes.LightGray);
        AddHorizontalLine(canvas, 396, totalTop + (rowHeight * 3) - 1, 56, 0.55, Brushes.LightGray);
        AddHorizontalLine(canvas, 8, totalTop + (rowHeight * 4) - 1, 370, 0.55, Brushes.LightGray);
        AddHorizontalLine(canvas, 396, totalTop + (rowHeight * 4) - 1, 56, 0.55, Brushes.LightGray);
    }

    private static void AddTableHeaderCell(
        Canvas canvas,
        string text,
        double x,
        double y,
        double width,
        double height,
        TextAlignment alignment)
    {
        var block = new TextBlock
        {
            Text = text,
            FontFamily = Arial,
            FontSize = Pt(10),
            FontWeight = FontWeights.Bold,
            TextAlignment = alignment,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Stretch
        };

        var border = new Border
        {
            Width = Pt(width),
            Height = Pt(height),
            BorderBrush = Brushes.Black,
            BorderThickness = new Thickness(1),
            Child = block
        };

        Place(canvas, border, x, y);
    }

    private static void AddRowText(
        Canvas canvas,
        string? text,
        double x,
        double y,
        double fontSize,
        double width,
        TextAlignment alignment,
        FontWeight? weight = null)
    {
        var block = new TextBlock
        {
            Text = FormatValue(text),
            Width = Pt(width),
            FontFamily = Courier,
            FontSize = Pt(fontSize),
            FontWeight = weight ?? FontWeights.Normal,
            TextAlignment = alignment,
            Foreground = Brushes.Black,
            TextWrapping = TextWrapping.NoWrap,
            ClipToBounds = true
        };

        Place(canvas, block, x, y);
    }

    private static void BuildSignatures(Canvas canvas, KittingDto? kitting)
    {
        AddHorizontalLine(canvas, 31, 727, 188, 2, Brushes.Black);
        AddHorizontalLine(canvas, 373, 727, 188, 2, Brushes.Black);

        AddTopText(canvas, "Surtido Por", 100, 735, 10, FontWeights.Bold, Times, 50, TextAlignment.Center);
        AddTopText(canvas, FormatWarehouseLabel(kitting?.Warehouse), 230, 737, 8, FontWeights.Bold, Times, 140, TextAlignment.Center);
        AddTopText(canvas, "Auditado Por", 436.4, 735, 10, FontWeights.Bold, Times, 58, TextAlignment.Center);
    }

    private static void BuildPageFooter(Canvas canvas, DateTime printedAt, int pageNumber, int pageCount)
    {
        AddTopText(canvas, $"Impresión: {FormatFooterDateTime(printedAt)}", 19.5, 756, 10, FontWeights.Normal, Times, 150);
        AddTopText(canvas, $"Transacción: {FormatFooterDateTime(printedAt)}", 203.1, 756, 10, FontWeights.Normal, Times, 160);
        AddTopText(canvas, $"Página {pageNumber} de {pageCount}", 475.7, 756, 10, FontWeights.Normal, Times, 70, TextAlignment.Center);
    }

    private static void AddBaselineText(
        Canvas canvas,
        string text,
        double x,
        double baseline,
        double fontSize,
        FontWeight weight,
        FontFamily family,
        double width = 220,
        TextAlignment alignment = TextAlignment.Left)
    {
        AddTopText(canvas, text, x, PdfPageHeight - baseline - fontSize, fontSize, weight, family, width, alignment);
    }

    private static void AddTopText(
        Canvas canvas,
        string text,
        double x,
        double y,
        double fontSize,
        FontWeight weight,
        FontFamily family,
        double width = 220,
        TextAlignment alignment = TextAlignment.Left)
    {
        var block = new TextBlock
        {
            Text = text,
            Width = Pt(width),
            FontFamily = family,
            FontSize = Pt(fontSize),
            FontWeight = weight,
            TextAlignment = alignment,
            Foreground = Brushes.Black,
            TextWrapping = TextWrapping.NoWrap
        };

        Place(canvas, block, x, y);
    }

    private static void AddHorizontalLine(Canvas canvas, double x, double y, double width, double height, Brush brush)
    {
        var line = new Border
        {
            Width = Pt(width),
            Height = Pt(height),
            Background = brush
        };

        Place(canvas, line, x, y);
    }

    private static void Place(Canvas canvas, UIElement element, double x, double y)
    {
        Canvas.SetLeft(element, Pt(x));
        Canvas.SetTop(element, Pt(y));
        canvas.Children.Add(element);
    }

    private static double Pt(double value) => value * PointToDip;

    private static IReadOnlyList<string> SplitAddress(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return new[] { string.Empty, string.Empty };

        var normalized = value.Trim();
        var marker = " BASE ";
        var splitIndex = normalized.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
        if (splitIndex > 0)
        {
            return new[]
            {
                normalized[..splitIndex].Trim(),
                normalized[(splitIndex + 1)..].Trim()
            };
        }

        const int targetLength = 16;
        if (normalized.Length <= targetLength)
            return new[] { normalized, string.Empty };

        var midpoint = normalized.LastIndexOf(' ', Math.Min(normalized.Length - 1, targetLength));
        if (midpoint <= 0)
            midpoint = targetLength;

        return new[]
        {
            normalized[..midpoint].Trim(),
            normalized[midpoint..].Trim()
        };
    }

    private static string BuildInfoLine(KittingDto? kitting)
    {
        var picking = FormatValue(kitting?.KittingCode, kitting?.KittingId.ToString(CultureInfo.InvariantCulture));
        var request = FormatValue(kitting?.GuideNumber);
        var invoice = FormatValue(kitting?.InvoiceNumber);

        return $"Picking: {picking}  REQ: {request}  Factura: {invoice}";
    }

    private static string FormatQuantity(decimal quantity) =>
        quantity % 1 == 0 ? quantity.ToString("0", CultureInfo.InvariantCulture) : quantity.ToString("0.##", CultureInfo.InvariantCulture);

    private static string FormatStatus(string? value)
    {
        var trimmed = FormatValue(value);
        return trimmed.Length <= 1 ? trimmed : trimmed[..1];
    }

    private static string FormatValue(string? primaryValue, string? fallbackValue = null)
    {
        if (!string.IsNullOrWhiteSpace(primaryValue))
            return primaryValue.Trim();

        if (!string.IsNullOrWhiteSpace(fallbackValue))
            return fallbackValue.Trim();

        return string.Empty;
    }

    private static string FormatFooterDateTime(DateTime value) =>
        value.ToString("MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);

    private static string FormatWarehouseLabel(string? warehouse)
    {
        var value = FormatValue(warehouse);
        if (string.IsNullOrWhiteSpace(value))
            return "ALMACEN";

        return value.StartsWith("ALMACEN", StringComparison.OrdinalIgnoreCase)
            ? value.ToUpperInvariant()
            : $"ALMACEN {value}".ToUpperInvariant();
    }

    private readonly record struct TableColumn(string Header, double Width, TextAlignment Alignment);
}
