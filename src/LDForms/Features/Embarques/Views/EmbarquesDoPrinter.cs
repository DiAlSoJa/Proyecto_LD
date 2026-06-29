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
using LD.FormsX.Features.Surtidos.Views;

namespace LD.FormsX.Features.Embarques.Views;

internal static class EmbarquesDoPrinter
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

    public static void Print(KittingDto? kitting, IReadOnlyList<ListaSurtidoPrinter.ListaSurtidoRow> rows)
    {
        if (rows.Count == 0)
            throw new InvalidOperationException("No hay partidas para mostrar en vista previa del DO.");

        var document = BuildDocument(kitting, rows);
        var preview = new PrintPreviewWindow(document, "Vista previa - Orden de entrega DO", "Orden de entrega DO");

        WindowOwnerHelper.AttachOwnerOrCenter(preview, Application.Current?.MainWindow);

        preview.ShowDialog();
    }

    private static FixedDocument BuildDocument(
        KittingDto? kitting,
        IReadOnlyList<ListaSurtidoPrinter.ListaSurtidoRow> rows)
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

            var page = BuildPage(kitting, pageRows, pageNumber, pageCount, printedAt);
            page.Measure(new Size(PageWidth, PageHeight));
            page.Arrange(new Rect(0, 0, PageWidth, PageHeight));
            page.UpdateLayout();

            var pageContent = new PageContent();
            ((IAddChild)pageContent).AddChild(page);
            document.Pages.Add(pageContent);
        }

        return document;
    }

    private static FixedPage BuildPage(
        KittingDto? kitting,
        IReadOnlyList<ListaSurtidoPrinter.ListaSurtidoRow> rows,
        int pageNumber,
        int pageCount,
        DateTime printedAt)
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
        BuildAddressBlock(canvas, kitting);
        BuildTransportBlock(canvas, kitting, printedAt);
        BuildRowsTable(canvas, rows);
        BuildSignatures(canvas);
        BuildPageFooter(canvas, printedAt, pageNumber, pageCount);

        return page;
    }

    private static void BuildHeader(Canvas canvas, KittingDto? kitting)
    {
        AddBaselineText(canvas, "LOGISTICA FLEXIBLE", 9.1, 762.6, 12, FontWeights.Black, Arial);
        AddBaselineText(canvas, "ALMACEN B5", 12.7, 743.8, 8, FontWeights.Normal, Times);
        AddBaselineText(canvas, "CARRETERA LA VENTA NEXTIPAC NO. 3020", 15, 729.9, 8, FontWeights.Normal, Times);
        AddBaselineText(canvas, "LA VENTA DEL ASTILLERO", 15, 721.1, 8, FontWeights.Normal, Times);
        AddBaselineText(canvas, "ZAPOPAN JALISCO", 15, 712.2, 8, FontWeights.Normal, Times);

        AddTitleBox(canvas);
        AddLogo(canvas);
        AddBaselineText(canvas, FormatValue(kitting?.Project), 333, 709.4, 14, FontWeights.Bold, Times, 37, TextAlignment.Center);
        AddHorizontalLine(canvas, 8, 98, 572, 3.2, Brushes.Black);
    }

    private static void AddTitleBox(Canvas canvas)
    {
        var title = new TextBlock
        {
            Text = "ORDEN DE ENTREGA",
            FontFamily = Arial,
            FontSize = Pt(14),
            FontWeight = FontWeights.Black,
            TextAlignment = TextAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center
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

    private static void BuildAddressBlock(Canvas canvas, KittingDto? kitting)
    {
        AddBaselineText(canvas, "Cliente", 9.6, 669.5, 10, FontWeights.Bold, Times);
        AddBaselineText(canvas, "Embarcar a", 355.9, 669.5, 10, FontWeights.Bold, Times);

        AddAddressColumn(
            canvas,
            163,
            new[]
            {
                FormatValue(kitting?.Client),
                FormatValue(kitting?.Direccion),
                FormatValue(kitting?.Colonia),
                FormatPostalCode(kitting?.CodigoPostal),
                FormatValue(kitting?.Ciudad)
            },
            new[] { 652.8, 643.9, 635, 626.2, 617.3 });

        AddAddressColumn(
            canvas,
            373.5,
            new[]
            {
                FormatValue(kitting?.Client),
                FormatValue(kitting?.Direccion),
                FormatValue(kitting?.Colonia),
                FormatValue(kitting?.Ciudad),
                FormatShipToTail(kitting)
            },
            new[] { 652.8, 643.9, 635, 617.3, 608.5 });
    }

    private static void AddAddressColumn(Canvas canvas, double x, IReadOnlyList<string> lines, IReadOnlyList<double> baselines)
    {
        for (var i = 0; i < lines.Count && i < baselines.Count; i++)
        {
            if (!string.IsNullOrWhiteSpace(lines[i]))
                AddBaselineText(canvas, lines[i], x, baselines[i], 8, FontWeights.Normal, Times, 190);
        }
    }

    private static void BuildTransportBlock(Canvas canvas, KittingDto? kitting, DateTime printedAt)
    {
        AddBaselineText(canvas, "Transporte", 163, 588.3, 10, FontWeights.Bold, Times);

        AddInfoLine(canvas, 163, 209.0, "Linea:", kitting?.TransportLine);
        AddInfoLine(canvas, 163, 223.1, "Tipo:", kitting?.VehicleType);
        AddInfoLine(canvas, 163, 237.0, "Chofer:", kitting?.DriverName);
        AddInfoLine(canvas, 163, 251.1, "Placas:", kitting?.VehiclePlate);
        AddInfoLine(canvas, 163, 265.0, "Sello:", kitting?.SealNumber);

        AddRightInfoLine(canvas, 373.4, 209.0, "Orden de Entrega:", FormatOrderNumber(kitting?.KittingCode));
        AddRightInfoLine(canvas, 373.4, 223.1, "Fecha:", FormatMainDate(printedAt));
        AddRightInfoLine(canvas, 373.4, 237.0, "Hora:", FormatMainTime(printedAt));
        AddRightInfoLine(canvas, 373.4, 251.1, "Bultos:", FormatPackages(kitting?.PackagesQty));
        AddRightInfoLine(canvas, 373.4, 265.0, "Factura:", kitting?.InvoiceNumber);

        AddHorizontalLine(canvas, 8, 284, 572, 3.2, Brushes.Black);
    }

    private static void AddInfoLine(Canvas canvas, double x, double top, string label, string? value)
    {
        var baseline = PdfPageHeight - top - 9;
        AddBaselineText(canvas, label, x, baseline, 9, FontWeights.Bold, Arial, 37);
        AddBaselineText(canvas, FormatValue(value), 209.6, baseline, 9, FontWeights.Normal, Arial, 120);
    }

    private static void AddRightInfoLine(Canvas canvas, double x, double top, string label, string? value)
    {
        var baseline = PdfPageHeight - top - 9;
        AddBaselineText(canvas, label, x, baseline, 9, FontWeights.Bold, Arial, 90);
        AddBaselineText(canvas, FormatValue(value), 417.5, baseline, 9, FontWeights.Normal, Arial, 132, TextAlignment.Right);
    }

    private static void BuildRowsTable(Canvas canvas, IReadOnlyList<ListaSurtidoPrinter.ListaSurtidoRow> rows)
    {
        const double tableLeft = 9;
        const double tableTop = 290;
        const double headerHeight = 16;
        const double rowHeight = 17;

        var columns = new[]
        {
            new TableColumn("No Parte", 87, TextAlignment.Center),
            new TableColumn("Descripción", 130, TextAlignment.Center),
            new TableColumn("Cantidad", 47, TextAlignment.Center),
            new TableColumn("Status", 34, TextAlignment.Center),
            new TableColumn("No Lote", 72, TextAlignment.Center),
            new TableColumn("SD", 19, TextAlignment.Center)
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

            AddRowText(canvas, row.PartNumber, 10.6, currentTop + 4, 8, 80, TextAlignment.Left);
            AddRowText(canvas, row.Description, 98, currentTop + 4, 8, 120, TextAlignment.Left);
            AddRowText(canvas, FormatQuantity(row.Quantity), 226, currentTop + 4, 8, 44, TextAlignment.Right);
            AddRowText(canvas, row.Status, 273, currentTop + 4, 8, 34, TextAlignment.Center);
            AddRowText(canvas, row.LotNumber, 307, currentTop + 4, 8, 70, TextAlignment.Right);
            AddRowText(canvas, row.SD, 380, currentTop + 4, 8, 16, TextAlignment.Center, FontWeights.Bold);
            AddHorizontalLine(canvas, 10, currentTop + rowHeight - 1, 388, 0.55, Brushes.LightGray);
        }

        var totalTop = rowTop + (rows.Count * rowHeight);
        AddRowText(canvas, FormatQuantity(rows.Sum(x => x.Quantity)), 226, totalTop + 4, 8, 44, TextAlignment.Right, FontWeights.Bold);
        AddHorizontalLine(canvas, 10, totalTop + rowHeight - 1, 388, 0.55, Brushes.LightGray);
        AddHorizontalLine(canvas, 10, totalTop + (rowHeight * 2) - 1, 370, 0.55, Brushes.LightGray);
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
            TextWrapping = TextWrapping.NoWrap
        };

        Place(canvas, block, x, y);
    }

    private static void BuildSignatures(Canvas canvas)
    {
        AddHorizontalLine(canvas, 31, 727, 188, 2, Brushes.Black);
        AddHorizontalLine(canvas, 373, 727, 188, 2, Brushes.Black);

        AddTopText(canvas, "Firma del Supervisor del Almacén", 52, 735, 10, FontWeights.Bold, Times, 146, TextAlignment.Center);
        AddTopText(canvas, "Firma del Chofer", 428, 735, 10, FontWeights.Bold, Times, 75, TextAlignment.Center);
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

    private static string FormatQuantity(decimal quantity) =>
        quantity % 1 == 0
            ? quantity.ToString("0", CultureInfo.InvariantCulture)
            : quantity.ToString("0.##", CultureInfo.InvariantCulture);

    private static string FormatValue(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
    }

    private static string FormatOrderNumber(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        var trimmed = value.Trim();
        if (long.TryParse(trimmed.Replace(",", string.Empty), NumberStyles.Any, CultureInfo.InvariantCulture, out var number))
            return number.ToString("#,0", CultureInfo.InvariantCulture);

        return trimmed;
    }

    private static string FormatMainDate(DateTime value) =>
        value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

    private static string FormatMainTime(DateTime value) =>
        value.ToString("hh:mm:ss tt", CultureInfo.InvariantCulture);

    private static string FormatFooterDateTime(DateTime value) =>
        value.ToString("MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);

    private static string FormatPackages(int? value) =>
        value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;

    private static string FormatShipToTail(KittingDto? kitting)
    {
        var tail = FormatValue(kitting?.Telefono);
        var postal = FormatPostalCode(kitting?.CodigoPostal);
        if (string.IsNullOrWhiteSpace(postal) && string.IsNullOrWhiteSpace(tail))
            return string.Empty;

        if (string.IsNullOrWhiteSpace(postal))
            return tail;

        return string.IsNullOrWhiteSpace(tail) ? postal : $"{postal}   {tail}";
    }

    private static string FormatPostalCode(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        return $"C.P. {value.Trim()}";
    }

    private readonly record struct TableColumn(string Header, double Width, TextAlignment Alignment);
}
