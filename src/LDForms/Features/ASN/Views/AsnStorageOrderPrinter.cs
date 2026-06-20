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
using LD.Contracts.ASN;
using LD.FormsX.Features.Common;

namespace LD.FormsX.Views.ASN;

internal static class AsnStorageOrderPrinter
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

    public static void PrintOrder(
        AsnDto asn,
        IReadOnlyList<AsnDetailDto> details,
        IReadOnlyList<AsnReceiptDetailDto> receiptDetails)
    {
        var rows = BuildRows(details, receiptDetails);
        if (rows.Count == 0)
            throw new InvalidOperationException("No hay partidas para mostrar en vista previa de la orden de almacenamiento.");

        var document = BuildDocument(asn, rows, rows.Sum(x => x.Received));
        ShowPreview(document, "Vista previa - Orden de almacenamiento", "Orden de almacenamiento ASN");
    }

    private static FixedDocument BuildDocument(
        AsnDto asn,
        IReadOnlyList<StorageOrderRow> rows,
        decimal totalReceived)
    {
        var document = new FixedDocument
        {
            DocumentPaginator = { PageSize = new Size(PageWidth, PageHeight) }
        };

        var pageCount = (int)Math.Ceiling(rows.Count / (double)RowsPerPage);
        var printedAt = DateTime.Now;

        for (var pageNumber = 1; pageNumber <= pageCount; pageNumber++)
        {
            var pageRows = rows
                .Skip((pageNumber - 1) * RowsPerPage)
                .Take(RowsPerPage)
                .ToList();

            var page = BuildPage(asn, pageRows, pageRows.Sum(x => x.Received), totalReceived, printedAt, pageNumber, pageCount);
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
        var preview = new PrintPreviewWindow(document, title, jobName)
        {
            Owner = Application.Current?.MainWindow
        };

        preview.ShowDialog();
    }

    private static FixedPage BuildPage(
        AsnDto asn,
        IReadOnlyList<StorageOrderRow> rows,
        decimal pageTotal,
        decimal totalReceived,
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

        BuildHeader(canvas, asn);
        BuildClientBlock(canvas, asn);
        BuildTransportBlock(canvas, asn);
        BuildRowsTable(canvas, rows, pageTotal, totalReceived);
        BuildSignatures(canvas);
        BuildPageFooter(canvas, printedAt, pageNumber, pageCount);

        return page;
    }

    private static void BuildHeader(Canvas canvas, AsnDto asn)
    {
        AddBaselineText(canvas, "LOGISTICA FLEXIBLE", 9.1, 762.6, 12, FontWeights.Black, Arial);
        AddBaselineText(canvas, "ALMACEN B5", 12.7, 743.8, 8, FontWeights.Normal, Times);
        AddBaselineText(canvas, "CARRETERA LA VENTA NEXTIPAC NO. 3020", 15, 729.9, 8, FontWeights.Normal, Times);
        AddBaselineText(canvas, "LA VENTA DEL ASTILLERO", 15, 721.1, 8, FontWeights.Normal, Times);
        AddBaselineText(canvas, "ZAPOPAN JALISCO", 15, 712.2, 8, FontWeights.Normal, Times);

        AddTitleBox(canvas);
        AddLogo(canvas);
        AddBaselineText(canvas, FormatValue(asn.Project), 333, 709.4, 14, FontWeights.Bold, Times, 37, TextAlignment.Center);
        AddHorizontalLine(canvas, 8, 98, 572, 3.2, Brushes.Black);
    }

    private static void AddTitleBox(Canvas canvas)
    {
        var title = new TextBlock
        {
            Text = "ORDEN DE ALMACENAMIENTO",
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

    private static void BuildClientBlock(Canvas canvas, AsnDto asn)
    {
        AddBaselineText(canvas, "Cliente", 9.6, 665.5, 10, FontWeights.Bold, Times);

        AddAddressLine(canvas, asn.Client, 648.8);
        AddAddressLine(canvas, asn.Project, 641.5);
    }

    private static void AddAddressLine(Canvas canvas, string? text, double baseline)
    {
        if (!string.IsNullOrWhiteSpace(text))
            AddBaselineText(canvas, text.Trim(), 163, baseline, 8, FontWeights.Normal, Times, 190);
    }

    private static void BuildTransportBlock(Canvas canvas, AsnDto asn)
    {
        AddBaselineText(canvas, "Transporte", 355.9, 659.8, 10, FontWeights.Bold, Times);
        AddTransportLine(canvas, "Chofer:", asn.DriverName, 641.5);
        AddTransportLine(canvas, "Placas:", asn.VehiclePlate, 622.6);
        AddTransportLine(canvas, "Sello:", asn.SealNumber, 603.8);
        AddFolioBlock(canvas, asn);
        AddHorizontalLine(canvas, 8, 228, 572, 3.2, Brushes.Black);
    }

    private static void AddTransportLine(Canvas canvas, string label, string? value, double baseline)
    {
        AddBaselineText(canvas, label, 374.7, baseline, 10, FontWeights.Bold, Times, 45);
        AddBaselineText(canvas, FormatValue(value), 433.4, baseline, 8, FontWeights.Normal, Times, 120);
    }

    private static void AddFolioBlock(Canvas canvas, AsnDto asn)
    {
        var box = new Border
        {
            Width = Pt(220),
            Height = Pt(28),
            BorderBrush = Brushes.Black,
            BorderThickness = new Thickness(1)
        };

        Place(canvas, box, 333, 195);
        AddTopText(canvas, $"Folio: {asn.AsnId}", 348.5, 203.5, 10, FontWeights.Normal, Times, 72);
        AddTopText(canvas, FormatValue(asn.AsnCode), 493.9, 203.5, 10, FontWeights.Bold, Times, 45);
    }

    private static void BuildRowsTable(
        Canvas canvas,
        IReadOnlyList<StorageOrderRow> rows,
        decimal pageTotal,
        decimal totalReceived)
    {
        const double tableLeft = 13;
        const double tableTop = 234;
        const double headerHeight = 16;
        const double rowHeight = 15.85;

        var columns = new[]
        {
            new TableColumn("No Parte", 87, TextAlignment.Center),
            new TableColumn("Descripción", 130, TextAlignment.Center),
            new TableColumn("Recibida", 47, TextAlignment.Center),
            new TableColumn("Status", 35, TextAlignment.Center),
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

            AddRowText(canvas, row.PartNumber, 15, currentTop + 4, 7, 82, TextAlignment.Left);
            AddRowText(canvas, row.Description, 102.4, currentTop + 4, 8, 120, TextAlignment.Left);
            AddRowText(canvas, FormatQuantity(row.Received), 229, currentTop + 4, 8, 45, TextAlignment.Right);
            AddRowText(canvas, FormatStatus(row.Status), 277, currentTop + 4, 8, 30, TextAlignment.Center);
            AddRowText(canvas, row.LotNumber, 310, currentTop + 4, 8, 72, TextAlignment.Right);
            AddRowText(canvas, row.SD, 383, currentTop + 4, 8, 18, TextAlignment.Center, FontWeights.Bold);
            AddRowText(canvas, row.LocationCode, 404, currentTop + 4, 8, 52, TextAlignment.Right);
            AddHorizontalLine(canvas, 13, currentTop + rowHeight - 1, 445, 0.55, Brushes.LightGray);
        }

        var totalTop = rowTop + (rows.Count * rowHeight);
        AddRowText(canvas, FormatQuantity(pageTotal), 229, totalTop + 4, 8, 45, TextAlignment.Right, FontWeights.Bold);
        AddHorizontalLine(canvas, 13, totalTop + rowHeight - 1, 445, 0.55, Brushes.LightGray);

        AddRowText(canvas, FormatQuantity(totalReceived), 229, totalTop + rowHeight + 4, 8, 45, TextAlignment.Right, FontWeights.Bold);
        AddHorizontalLine(canvas, 13, totalTop + (rowHeight * 2) - 1, 370, 0.55, Brushes.LightGray);
        AddHorizontalLine(canvas, 404, totalTop + (rowHeight * 2) - 1, 54, 0.55, Brushes.LightGray);
        AddHorizontalLine(canvas, 13, totalTop + (rowHeight * 3) - 1, 370, 0.55, Brushes.LightGray);
        AddHorizontalLine(canvas, 404, totalTop + (rowHeight * 3) - 1, 54, 0.55, Brushes.LightGray);
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

    private static void BuildSignatures(Canvas canvas)
    {
        AddHorizontalLine(canvas, 31, 727, 188, 2, Brushes.Black);
        AddHorizontalLine(canvas, 373, 727, 188, 2, Brushes.Black);

        AddTopText(canvas, "Nombre del Supervisor del Almacén", 47.7, 735, 10, FontWeights.Bold, Times, 155, TextAlignment.Center);
        AddTopText(canvas, "Nombre del Almacenista", 412.7, 735, 10, FontWeights.Bold, Times, 106, TextAlignment.Center);
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

    private static List<StorageOrderRow> BuildRows(
        IReadOnlyList<AsnDetailDto> details,
        IReadOnlyList<AsnReceiptDetailDto> receiptDetails)
    {
        if (receiptDetails.Count > 0)
        {
            var rows = receiptDetails
                .Select(x => new StorageOrderRow
                {
                    PartNumber = x.PartNumber,
                    Description = x.Description,
                    Received = x.ReceivedQuantity ?? 0m,
                    Status = x.Status,
                    LotNumber = x.LotNumber,
                    SD = x.SD,
                    LocationCode = x.LocationCode
                })
                .ToList();

            var receivedDetailIds = receiptDetails
                .Select(x => x.AsnDetailId)
                .ToHashSet();

            rows.AddRange(details
                .Where(x => !receivedDetailIds.Contains(x.AsnDetailId))
                .Select(x => new StorageOrderRow
                {
                    PartNumber = x.PartNumber,
                    Description = x.Description,
                    Received = x.Quantity,
                    Status = x.Status,
                    LotNumber = x.LotNumber,
                    SD = x.SD,
                    LocationCode = string.Empty
                }));

            return rows
                .OrderBy(x => x.PartNumber)
                .ThenBy(x => x.LotNumber)
                .ThenBy(x => x.LocationCode)
                .ToList();
        }

        return details
            .OrderBy(x => x.PartNumber)
            .ThenBy(x => x.LotNumber)
            .Select(x => new StorageOrderRow
            {
                PartNumber = x.PartNumber,
                Description = x.Description,
                Received = x.Quantity,
                Status = x.Status,
                LotNumber = x.LotNumber,
                SD = x.SD,
                LocationCode = string.Empty
            })
            .ToList();
    }

    private static string FormatQuantity(decimal quantity)
    {
        return quantity.ToString("0.##", CultureInfo.InvariantCulture);
    }

    private static string FormatStatus(string? value)
    {
        var trimmed = FormatValue(value);
        return trimmed.Length <= 1 ? trimmed : trimmed[..1];
    }

    private static string FormatValue(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
    }

    private static string FormatFooterDateTime(DateTime value) =>
        value.ToString("MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);

    private sealed class StorageOrderRow
    {
        public string PartNumber { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public decimal Received { get; init; }
        public string Status { get; init; } = string.Empty;
        public string LotNumber { get; init; } = string.Empty;
        public string SD { get; init; } = string.Empty;
        public string LocationCode { get; init; } = string.Empty;
    }

    private readonly record struct TableColumn(string Header, double Width, TextAlignment Alignment);
}
