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

namespace LD.FormsX.Features.Surtidos.Views;

internal static class ListaSurtidoPrinter
{
    private const double Dpi = 96d;
    private const double PageWidth = 8.5d * Dpi;
    private const double PageHeight = 11d * Dpi;
    private const double MarginSize = 36d;
    private const double HeaderHeight = 220d;
    private const double FooterHeight = 42d;
    private const double RowHeight = 24d;

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
            throw new InvalidOperationException("No hay lineas surtidas para imprimir.");

        var printDialog = new PrintDialog();
        if (printDialog.ShowDialog() != true)
            return;

        var document = new FixedDocument
        {
            DocumentPaginator = { PageSize = new Size(PageWidth, PageHeight) }
        };

        var rowsPerPage = Math.Max(1, (int)((PageHeight - (MarginSize * 2) - HeaderHeight - FooterHeight) / RowHeight));
        var pageCount = (int)Math.Ceiling(rows.Count / (double)rowsPerPage);
        var totalQuantity = rows.Sum(x => x.Quantity);
        var totalPallets = rows.Count;

        var pageNumber = 1;
        foreach (var pageRows in rows.Chunk(rowsPerPage))
        {
            var page = BuildPage(kitting, pageRows.ToList(), pageNumber, pageCount, totalQuantity, totalPallets);
            page.Measure(new Size(PageWidth, PageHeight));
            page.Arrange(new Rect(0, 0, PageWidth, PageHeight));
            page.UpdateLayout();

            var pageContent = new PageContent();
            ((IAddChild)pageContent).AddChild(page);
            document.Pages.Add(pageContent);
            pageNumber++;
        }

        printDialog.PrintDocument(document.DocumentPaginator, "Lista de surtido");
    }

    private static FixedPage BuildPage(
        KittingDto? kitting,
        IReadOnlyList<ListaSurtidoRow> rows,
        int pageNumber,
        int pageCount,
        decimal totalQuantity,
        int totalPallets)
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
        root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        var header = BuildHeader(kitting);
        Grid.SetRow(header, 0);
        root.Children.Add(header);

        var projectBlock = BuildProjectBlock(kitting);
        Grid.SetRow(projectBlock, 1);
        root.Children.Add(projectBlock);

        var infoBlock = BuildInfoBlock(kitting);
        Grid.SetRow(infoBlock, 2);
        root.Children.Add(infoBlock);

        var table = BuildTable(rows);
        Grid.SetRow(table, 3);
        root.Children.Add(table);

        var footer = BuildFooter(rows.Sum(x => x.Quantity), totalQuantity, totalPallets, pageNumber, pageCount);
        Grid.SetRow(footer, 4);
        root.Children.Add(footer);

        page.Children.Add(root);
        return page;
    }

    private static FrameworkElement BuildHeader(KittingDto? kitting)
    {
        var grid = new Grid
        {
            Margin = new Thickness(0, 0, 0, 8)
        };

        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(360) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(84) });

        var company = new StackPanel
        {
            VerticalAlignment = VerticalAlignment.Center
        };
        company.Children.Add(Text("LOGISTICA FLEXIBLE", 20, FontWeights.Bold));
        Grid.SetColumn(company, 0);
        grid.Children.Add(company);

        var title = new Border
        {
            BorderBrush = Brushes.Black,
            BorderThickness = new Thickness(2),
            Padding = new Thickness(12, 6, 12, 6),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Center,
            Child = Text("LISTA DE SURTIDO", 19, FontWeights.Bold, TextAlignment.Center)
        };
        Grid.SetColumn(title, 1);
        grid.Children.Add(title);

        var logo = new Image
        {
            Width = 72,
            Height = 52,
            Stretch = Stretch.Uniform,
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Center,
            Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/logo.png"))
        };
        Grid.SetColumn(logo, 2);
        grid.Children.Add(logo);

        return grid;
    }

    private static FrameworkElement BuildProjectBlock(KittingDto? kitting)
    {
        var text = string.IsNullOrWhiteSpace(kitting?.Project)
            ? string.Empty
            : kitting!.Project.Trim();

        return new TextBlock
        {
            Text = text,
            FontSize = 18,
            FontWeight = FontWeights.Bold,
            HorizontalAlignment = HorizontalAlignment.Center,
            TextAlignment = TextAlignment.Center,
            Margin = new Thickness(0, 10, 0, 4),
            Foreground = Brushes.Black
        };
    }

    private static FrameworkElement BuildInfoBlock(KittingDto? kitting)
    {
        var grid = new Grid
        {
            Margin = new Thickness(0, 0, 0, 12)
        };

        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

        var picking = BuildInfoLine("Picking", kitting?.KittingCode, kitting?.KittingId.ToString(CultureInfo.InvariantCulture));
        Grid.SetColumn(picking, 0);
        grid.Children.Add(picking);

        var req = BuildInfoLine("REQ", kitting?.GuideNumber, null);
        Grid.SetColumn(req, 1);
        grid.Children.Add(req);

        var invoice = BuildInfoLine("Factura", kitting?.InvoiceNumber, null);
        Grid.SetColumn(invoice, 2);
        grid.Children.Add(invoice);

        return grid;
    }

    private static FrameworkElement BuildInfoLine(string label, string? primaryValue, string? fallbackValue)
    {
        var panel = new StackPanel
        {
            Orientation = Orientation.Vertical,
            HorizontalAlignment = HorizontalAlignment.Center
        };

        panel.Children.Add(Text($"{label}: {FormatValue(primaryValue, fallbackValue)}", 12.5, FontWeights.Bold));
        return panel;
    }

    private static FrameworkElement BuildTable(IReadOnlyList<ListaSurtidoRow> rows)
    {
        var table = new Grid();
        AddColumns(table);
        table.RowDefinitions.Add(new RowDefinition { Height = new GridLength(28) });

        var headers = new[]
        {
            "No Parte",
            "Descripcion",
            "Cantidad",
            "Status",
            "No Lote",
            "SD",
            "Ubicacion"
        };

        for (var i = 0; i < headers.Length; i++)
            AddCell(table, headers[i], 0, i, true);

        for (var i = 0; i < rows.Count; i++)
        {
            var rowIndex = i + 1;
            var row = rows[i];
            table.RowDefinitions.Add(new RowDefinition { Height = new GridLength(RowHeight) });

            AddCell(table, row.PartNumber, rowIndex, 0);
            AddCell(table, row.Description, rowIndex, 1);
            AddCell(table, FormatQuantity(row.Quantity), rowIndex, 2, alignRight: true);
            AddCell(table, row.Status, rowIndex, 3, textAlignment: TextAlignment.Center);
            AddCell(table, row.LotNumber, rowIndex, 4);
            AddCell(table, row.SD, rowIndex, 5, textAlignment: TextAlignment.Center);
            AddCell(table, row.LocationCode, rowIndex, 6);
        }

        return table;
    }

    private static FrameworkElement BuildFooter(
        decimal pageQuantity,
        decimal totalQuantity,
        int totalPallets,
        int pageNumber,
        int pageCount)
    {
        var grid = new Grid
        {
            Margin = new Thickness(0, 10, 0, 0)
        };

        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

        var leftPanel = new StackPanel
        {
            Orientation = Orientation.Vertical
        };
        leftPanel.Children.Add(new TextBlock
        {
            Text = $"Suma de cantidades: {FormatQuantity(pageQuantity)}",
            FontSize = 12,
            FontWeight = FontWeights.Bold,
            HorizontalAlignment = HorizontalAlignment.Left
        });
        leftPanel.Children.Add(new TextBlock
        {
            Text = $"Pallets: {totalPallets}",
            FontSize = 12,
            FontWeight = FontWeights.Bold,
            HorizontalAlignment = HorizontalAlignment.Left
        });
        Grid.SetColumn(leftPanel, 0);
        grid.Children.Add(leftPanel);

        var pagePanel = new StackPanel
        {
            Orientation = Orientation.Vertical,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        pagePanel.Children.Add(new TextBlock
        {
            Text = $"Pagina {pageNumber} de {pageCount}",
            FontSize = 11,
            HorizontalAlignment = HorizontalAlignment.Right,
            TextAlignment = TextAlignment.Right
        });
        pagePanel.Children.Add(new TextBlock
        {
            Text = $"Total general: {FormatQuantity(totalQuantity)}",
            FontSize = 11,
            HorizontalAlignment = HorizontalAlignment.Right,
            TextAlignment = TextAlignment.Right
        });
        Grid.SetColumn(pagePanel, 1);
        grid.Children.Add(pagePanel);

        return grid;
    }

    private static void AddColumns(Grid grid)
    {
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.5, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2.6, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.9, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.8, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.1, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.7, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.0, GridUnitType.Star) });
    }

    private static void AddCell(
        Grid grid,
        string? text,
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
            Background = isHeader ? new SolidColorBrush(Color.FromRgb(245, 245, 245)) : Brushes.White,
            Padding = new Thickness(4, 2, 4, 2)
        };

        var block = new TextBlock
        {
            Text = text ?? string.Empty,
            FontSize = isHeader ? 11.5 : 11,
            FontWeight = isHeader ? FontWeights.Bold : FontWeights.Normal,
            TextWrapping = TextWrapping.Wrap,
            TextAlignment = textAlignment,
            VerticalAlignment = VerticalAlignment.Center,
            Foreground = Brushes.Black
        };

        if (alignRight)
            block.HorizontalAlignment = HorizontalAlignment.Right;

        border.Child = block;
        Grid.SetRow(border, row);
        Grid.SetColumn(border, column);
        grid.Children.Add(border);
    }

    private static TextBlock Text(string text, double fontSize, FontWeight weight, TextAlignment alignment = TextAlignment.Left)
    {
        return new TextBlock
        {
            Text = text,
            FontSize = fontSize,
            FontWeight = weight,
            TextAlignment = alignment,
            Foreground = Brushes.Black
        };
    }

    private static string FormatQuantity(decimal quantity) =>
        quantity % 1 == 0 ? quantity.ToString("0", CultureInfo.InvariantCulture) : quantity.ToString("0.##", CultureInfo.InvariantCulture);

    private static string FormatValue(string? primaryValue, string? fallbackValue = null)
    {
        if (!string.IsNullOrWhiteSpace(primaryValue))
            return primaryValue.Trim();

        if (!string.IsNullOrWhiteSpace(fallbackValue))
            return fallbackValue.Trim();

        return string.Empty;
    }
}
