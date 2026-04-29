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

namespace LD.FormsX.Views.ASN;

internal static class AsnStorageOrderPrinter
{
    private const double Dpi = 96d;
    private const double PageWidth = 8.5d * Dpi;
    private const double PageHeight = 11d * Dpi;
    private const double MarginSize = 44d;
    private const double HeaderHeight = 260d;
    private const double RowHeight = 24d;

    public static void PrintOrder(
        AsnDto asn,
        IReadOnlyList<AsnDetailDto> details,
        IReadOnlyList<AsnReceiptDetailDto> receiptDetails)
    {
        var rows = BuildRows(details, receiptDetails);
        if (rows.Count == 0)
            throw new InvalidOperationException("No hay partidas para imprimir en la orden de almacenamiento.");

        var printDialog = new PrintDialog();
        if (printDialog.ShowDialog() != true)
            return;

        var document = new FixedDocument
        {
            DocumentPaginator = { PageSize = new Size(PageWidth, PageHeight) }
        };

        var rowsPerPage = Math.Max(1, (int)((PageHeight - MarginSize - HeaderHeight - 70d) / RowHeight));
        var pageNumber = 1;
        var pageCount = (int)Math.Ceiling(rows.Count / (double)rowsPerPage);

        foreach (var pageRows in rows.Chunk(rowsPerPage))
        {
            var page = BuildPage(asn, pageRows.ToList(), pageNumber, pageCount, rows.Sum(x => x.Received));
            page.Measure(new Size(PageWidth, PageHeight));
            page.Arrange(new Rect(0, 0, PageWidth, PageHeight));
            page.UpdateLayout();

            var pageContent = new PageContent();
            ((IAddChild)pageContent).AddChild(page);
            document.Pages.Add(pageContent);
            pageNumber++;
        }

        printDialog.PrintDocument(document.DocumentPaginator, "Orden de almacenamiento ASN");
    }

    private static FixedPage BuildPage(
        AsnDto asn,
        IReadOnlyList<StorageOrderRow> rows,
        int pageNumber,
        int pageCount,
        decimal totalReceived)
    {
        var page = new FixedPage
        {
            Width = PageWidth,
            Height = PageHeight,
            Background = Brushes.White
        };

        var root = new Grid
        {
            Width = PageWidth - (MarginSize * 2),
            Height = PageHeight - (MarginSize * 2),
            Margin = new Thickness(MarginSize)
        };

        root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        var header = BuildHeader(asn, pageNumber, pageCount);
        Grid.SetRow(header, 0);
        root.Children.Add(header);

        var table = BuildTable(rows);
        Grid.SetRow(table, 1);
        root.Children.Add(table);

        var footer = BuildFooter(rows.Sum(x => x.Received), totalReceived, pageNumber, pageCount);
        Grid.SetRow(footer, 3);
        root.Children.Add(footer);

        page.Children.Add(root);
        return page;
    }

    private static FrameworkElement BuildHeader(AsnDto asn, int pageNumber, int pageCount)
    {
        var panel = new Grid
        {
            Margin = new Thickness(0, 0, 0, 8)
        };

        panel.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        panel.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        panel.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        panel.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        var top = new Grid();
        top.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        top.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(360) });
        top.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });

        var company = new StackPanel();
        company.Children.Add(Text("LOGISTICA FLEXIBLE", 20, FontWeights.Bold));
        company.Children.Add(Text("ALMACEN B5", 10, FontWeights.Bold));
        company.Children.Add(Text("CARRETERA A VENTA NEXTIPAC NO. 3020", 10, FontWeights.Normal));
        company.Children.Add(Text("LA VENTA DEL ASTILLERO", 10, FontWeights.Normal));
        company.Children.Add(Text("ZAPOPAN JALISCO", 10, FontWeights.Normal));
        Grid.SetColumn(company, 0);
        top.Children.Add(company);

        var title = new Border
        {
            BorderBrush = Brushes.Gray,
            BorderThickness = new Thickness(2),
            Padding = new Thickness(10, 4, 10, 4),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Top,
            Child = Text("ORDEN DE ALMACENAMIENTO", 20, FontWeights.Bold, TextAlignment.Center)
        };
        Grid.SetColumn(title, 1);
        top.Children.Add(title);

        var logo = new Image
        {
            Width = 92,
            Height = 54,
            Stretch = Stretch.Uniform,
            HorizontalAlignment = HorizontalAlignment.Right,
            Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/logo.png"))
        };
        Grid.SetColumn(logo, 2);
        top.Children.Add(logo);

        Grid.SetRow(top, 0);
        panel.Children.Add(top);

        var project = Text(FormatValue(asn.Project), 18, FontWeights.Bold, TextAlignment.Center);
        project.Margin = new Thickness(0, 18, 0, 8);
        Grid.SetRow(project, 1);
        panel.Children.Add(project);

        var line = new Border
        {
            Height = 3,
            Background = Brushes.Black,
            Margin = new Thickness(0, 0, 0, 16)
        };
        Grid.SetRow(line, 2);
        panel.Children.Add(line);

        var info = new Grid();
        info.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        info.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(290) });

        var clientPanel = new StackPanel();
        clientPanel.Children.Add(Text("Cliente", 12, FontWeights.Bold));
        clientPanel.Children.Add(Text(FormatValue(asn.Client), 11, FontWeights.Normal));
        clientPanel.Children.Add(Text(FormatValue(asn.Project), 11, FontWeights.Normal));
        Grid.SetColumn(clientPanel, 0);
        info.Children.Add(clientPanel);

        var transport = new StackPanel();
        transport.Children.Add(Text("Transporte", 12, FontWeights.Bold));
        transport.Children.Add(FieldLine("Chofer:", asn.DriverName));
        transport.Children.Add(FieldLine("Placas:", asn.VehiclePlate));
        transport.Children.Add(FieldLine("Sello:", asn.SealNumber));
        transport.Children.Add(BuildFolioBlock(asn, pageNumber, pageCount));
        Grid.SetColumn(transport, 1);
        info.Children.Add(transport);

        Grid.SetRow(info, 3);
        panel.Children.Add(info);

        return panel;
    }

    private static FrameworkElement BuildTable(IReadOnlyList<StorageOrderRow> rows)
    {
        var table = new Grid();
        AddColumns(table);
        table.RowDefinitions.Add(new RowDefinition { Height = new GridLength(24) });

        var headers = new[] { "No Parte", "Descripcion", "Recibida", "Status", "No Lote", "SD", "Ubicacion", "Nueva" };
        for (var i = 0; i < headers.Length; i++)
            AddCell(table, headers[i], 0, i, true);

        for (var i = 0; i < rows.Count; i++)
        {
            var rowIndex = i + 1;
            var row = rows[i];
            table.RowDefinitions.Add(new RowDefinition { Height = new GridLength(RowHeight) });

            AddCell(table, row.PartNumber, rowIndex, 0);
            AddCell(table, row.Description, rowIndex, 1);
            AddCell(table, FormatQuantity(row.Received), rowIndex, 2, alignRight: true);
            AddCell(table, row.Status, rowIndex, 3, textAlignment: TextAlignment.Center);
            AddCell(table, row.LotNumber, rowIndex, 4);
            AddCell(table, row.SD, rowIndex, 5, textAlignment: TextAlignment.Center);
            AddCell(table, row.LocationCode, rowIndex, 6);
            AddCell(table, string.Empty, rowIndex, 7);
        }

        return table;
    }

    private static FrameworkElement BuildFooter(decimal pageTotal, decimal totalReceived, int pageNumber, int pageCount)
    {
        var panel = new Grid
        {
            Margin = new Thickness(0, 8, 0, 0)
        };
        panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        panel.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

        var pages = Text($"Pagina {pageNumber} de {pageCount}", 10, FontWeights.Normal);
        Grid.SetColumn(pages, 0);
        panel.Children.Add(pages);

        var totals = new StackPanel
        {
            Width = 170,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        totals.Children.Add(Text($"Total pagina: {FormatQuantity(pageTotal)}", 12, FontWeights.Bold, TextAlignment.Right));
        totals.Children.Add(Text($"Total orden: {FormatQuantity(totalReceived)}", 12, FontWeights.Bold, TextAlignment.Right));
        Grid.SetColumn(totals, 1);
        panel.Children.Add(totals);

        return panel;
    }

    private static FrameworkElement BuildFolioBlock(AsnDto asn, int pageNumber, int pageCount)
    {
        var border = new Border
        {
            BorderBrush = Brushes.Gray,
            BorderThickness = new Thickness(1),
            Margin = new Thickness(0, 8, 0, 0),
            Padding = new Thickness(8, 4, 8, 4)
        };

        var grid = new Grid();
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

        var folio = Text($"Folio: {asn.AsnId} PRE", 11, FontWeights.Normal);
        Grid.SetColumn(folio, 0);
        grid.Children.Add(folio);

        var asnText = Text($"{FormatValue(asn.AsnCode)}  {pageNumber}/{pageCount}", 11, FontWeights.Bold, TextAlignment.Right);
        Grid.SetColumn(asnText, 1);
        grid.Children.Add(asnText);

        border.Child = grid;
        return border;
    }

    private static void AddColumns(Grid table)
    {
        table.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });
        table.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(175) });
        table.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(70) });
        table.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(58) });
        table.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(105) });
        table.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(42) });
        table.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(86) });
        table.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(42) });
    }

    private static void AddCell(
        Grid table,
        string? value,
        int row,
        int column,
        bool isHeader = false,
        bool alignRight = false,
        TextAlignment textAlignment = TextAlignment.Left)
    {
        var border = new Border
        {
            BorderBrush = Brushes.Black,
            BorderThickness = new Thickness(0.6),
            Padding = new Thickness(4, 2, 4, 2),
            Background = isHeader ? new SolidColorBrush(Color.FromRgb(238, 238, 238)) : Brushes.White,
            Child = Text(
                FormatValue(value),
                isHeader ? 11 : 9,
                isHeader ? FontWeights.Bold : FontWeights.Normal,
                alignRight ? TextAlignment.Right : textAlignment)
        };

        Grid.SetRow(border, row);
        Grid.SetColumn(border, column);
        table.Children.Add(border);
    }

    private static TextBlock FieldLine(string label, string? value)
    {
        return new TextBlock
        {
            Text = $"{label} {FormatValue(value)}",
            FontSize = 11,
            Margin = new Thickness(0, 2, 0, 0),
            FontFamily = new FontFamily("Times New Roman")
        };
    }

    private static TextBlock Text(
        string text,
        double fontSize,
        FontWeight fontWeight,
        TextAlignment textAlignment = TextAlignment.Left)
    {
        return new TextBlock
        {
            Text = text,
            FontSize = fontSize,
            FontWeight = fontWeight,
            TextAlignment = textAlignment,
            TextWrapping = TextWrapping.Wrap,
            FontFamily = new FontFamily("Times New Roman"),
            Foreground = Brushes.Black
        };
    }

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

    private static string FormatValue(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? "-" : value.Trim();
    }

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
}
