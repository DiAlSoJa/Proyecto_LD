using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LD.Contracts.ASN;
using QRCoder;

namespace LD.FormsX.Views.ASN;

internal static class AsnReceiptLabelPrinter
{
    private const double Dpi = 96d;
    private const double LabelWidth = 6d / 2.54d * Dpi;
    private const double LabelHeight = 15d / 2.54d * Dpi;
    private const double MarginSize = 8d;

    private static readonly IReadOnlyDictionary<char, string> ItfPatterns = new Dictionary<char, string>
    {
        ['0'] = "nnwwn",
        ['1'] = "wnnnw",
        ['2'] = "nwnnw",
        ['3'] = "wwnnn",
        ['4'] = "nnwnw",
        ['5'] = "wnwnn",
        ['6'] = "nwwnn",
        ['7'] = "nnnww",
        ['8'] = "wnnwn",
        ['9'] = "nwnwn"
    };

    public static void PrintLabels(
        IReadOnlyList<AsnReceiptDetailDto> receiptDetails,
        AsnDto? asn,
        AsnDetailDto? detail)
    {
        var printDialog = new PrintDialog();
        if (printDialog.ShowDialog() != true)
            return;

        var document = new FixedDocument
        {
            DocumentPaginator = { PageSize = new Size(LabelWidth, LabelHeight) }
        };

        foreach (var receiptDetail in receiptDetails)
        {
            var fixedPage = BuildLabelPage(receiptDetail, asn, detail);
            fixedPage.Measure(new Size(LabelWidth, LabelHeight));
            fixedPage.Arrange(new Rect(0, 0, LabelWidth, LabelHeight));
            fixedPage.UpdateLayout();

            var pageContent = new PageContent();
            ((IAddChild)pageContent).AddChild(fixedPage);
            document.Pages.Add(pageContent);
        }

        printDialog.PrintDocument(document.DocumentPaginator, "Etiquetas ASN");
    }

    private static FixedPage BuildLabelPage(
        AsnReceiptDetailDto receiptDetail,
        AsnDto? asn,
        AsnDetailDto? detail)
    {
        var page = new FixedPage
        {
            Width = LabelWidth,
            Height = LabelHeight,
            Background = Brushes.White
        };

        var root = new Grid
        {
            Width = LabelWidth,
            Height = LabelHeight,
            Margin = new Thickness(MarginSize)
        };

        root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        var topBarcode = BuildBarcodeBlock(receiptDetail.StandardId);
        Grid.SetRow(topBarcode, 0);
        root.Children.Add(topBarcode);

        var topBrandBlock = BuildBrandBlock(receiptDetail, 4);
        Grid.SetRow(topBrandBlock, 1);
        root.Children.Add(topBrandBlock);

        var separator = new Border
        {
            Margin = new Thickness(0, 4, 0, 6),
            Height = 2,
            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#24344D"))
        };
        Grid.SetRow(separator, 2);
        root.Children.Add(separator);

        var infoPanel = BuildInfoBlock(receiptDetail, asn, detail);
        Grid.SetRow(infoPanel, 3);
        root.Children.Add(infoPanel);

        var bottomBarcode = BuildBarcodeBlock(receiptDetail.StandardId);
        bottomBarcode.Margin = new Thickness(0, 8, 0, 0);
        Grid.SetRow(bottomBarcode, 4);
        root.Children.Add(bottomBarcode);

        var bottomBrandBlock = BuildBrandBlock(receiptDetail, 3);
        Grid.SetRow(bottomBrandBlock, 5);
        root.Children.Add(bottomBrandBlock);

        page.Children.Add(root);
        return page;
    }

    private static FrameworkElement BuildBarcodeBlock(string? standardId)
    {
        var container = new StackPanel
        {
            Orientation = Orientation.Vertical,
            HorizontalAlignment = HorizontalAlignment.Stretch
        };

        var barcodeCanvas = BuildInterleavedTwoOfFiveBarcode(standardId, LabelWidth - (MarginSize * 2), 78);
        container.Children.Add(barcodeCanvas);

        container.Children.Add(new TextBlock
        {
            Text = string.IsNullOrWhiteSpace(standardId) ? "SIN ESTANDAR" : standardId,
            FontSize = 14,
            FontWeight = FontWeights.SemiBold,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(0, 2, 0, 0),
            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#475569"))
        });

        return container;
    }

    private static FrameworkElement BuildBrandBlock(AsnReceiptDetailDto receiptDetail, double topMargin)
    {
        var panel = new Grid
        {
            Margin = new Thickness(0, topMargin, 0, 0)
        };

        panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        panel.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

        var logo = new Image
        {
            Width = 64,
            Height = 28,
            Stretch = Stretch.Uniform,
            HorizontalAlignment = HorizontalAlignment.Left,
            Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/logo.png"))
        };

        Grid.SetColumn(logo, 0);
        panel.Children.Add(logo);

        var qrImage = new Image
        {
            Width = 34,
            Height = 34,
            Stretch = Stretch.Uniform,
            HorizontalAlignment = HorizontalAlignment.Right,
            Margin = new Thickness(0, 0, 16, 0),
            Source = BuildQrImage(receiptDetail)
        };

        Grid.SetColumn(qrImage, 1);
        panel.Children.Add(qrImage);
        return panel;
    }

    private static FrameworkElement BuildInfoBlock(
        AsnReceiptDetailDto receiptDetail,
        AsnDto? asn,
        AsnDetailDto? detail)
    {
        var panel = new StackPanel
        {
            Orientation = Orientation.Vertical
        };

        AddInfoLine(panel, "N.P.", receiptDetail.PartNumber);
        AddInfoLine(panel, "Pallet", receiptDetail.PalletNumber > 0 ? receiptDetail.PalletNumber.ToString(CultureInfo.InvariantCulture) : "-");
        AddInfoLine(panel, "Descripción", receiptDetail.Description);
        AddInfoLine(panel, "Qty.", FormatQuantity(receiptDetail.ReceivedQuantity ?? detail?.Quantity));
        AddInfoLine(panel, "Lote", receiptDetail.LotNumber);
        AddInfoLine(panel, "Cliente", asn?.Client);
        AddInfoLine(panel, "Proyecto", asn?.Project);
        AddInfoLine(panel, "ASN", asn?.AsnCode);
        AddInfoLine(panel, "Fecha", FormatDate(receiptDetail.ExpirationDate, asn?.Eta));

        return panel;
    }

    private static void AddInfoLine(Panel panel, string label, string? value)
    {
        panel.Children.Add(new TextBlock
        {
            Text = $"{label}: {FormatValue(value)}",
            FontSize = 12,
            Margin = new Thickness(0, 0, 0, 2),
            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F2937"))
        });
    }

    private static string FormatQuantity(decimal? quantity)
    {
        return quantity.HasValue
            ? quantity.Value.ToString("0.##", CultureInfo.InvariantCulture)
            : "-";
    }

    private static string FormatDate(DateTime? primaryDate, DateTime? fallbackDate)
    {
        var value = primaryDate ?? fallbackDate;
        return value?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) ?? "-";
    }

    private static string FormatValue(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? "-" : value.Trim();
    }

    private static ImageSource BuildQrImage(AsnReceiptDetailDto receiptDetail)
    {
        var qrValue = BuildQrValue(receiptDetail);
        if (string.IsNullOrWhiteSpace(qrValue))
            return CreateEmptyImage();

        using var generator = new QRCodeGenerator();
        using var data = generator.CreateQrCode(qrValue, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(data);
        var qrBytes = qrCode.GetGraphic(4, darkColorRgba: [0, 0, 0, 255], lightColorRgba: [255, 255, 255, 255], drawQuietZones: false);

        var image = new BitmapImage();
        using var stream = new MemoryStream(qrBytes);
        image.BeginInit();
        image.CacheOption = BitmapCacheOption.OnLoad;
        image.StreamSource = stream;
        image.EndInit();
        image.Freeze();
        return image;
    }

    private static string BuildQrValue(AsnReceiptDetailDto receiptDetail)
    {
        var values = new[]
        {
            receiptDetail.StandardId,
            receiptDetail.PalletNumber > 0 ? receiptDetail.PalletNumber.ToString(CultureInfo.InvariantCulture) : string.Empty,
            receiptDetail.PartNumber,
            receiptDetail.LotNumber,
            receiptDetail.LocationCode
        };

        return string.Join("|", values.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x!.Trim()));
    }

    private static ImageSource CreateEmptyImage()
    {
        var drawing = new GeometryDrawing(Brushes.Transparent, null, new RectangleGeometry(new Rect(0, 0, 1, 1)));
        var image = new DrawingImage(drawing);
        image.Freeze();
        return image;
    }

    private static Canvas BuildInterleavedTwoOfFiveBarcode(string? value, double width, double height)
    {
        var canvas = new Canvas
        {
            Width = width,
            Height = height
        };

        var digits = new string((value ?? string.Empty).Where(char.IsDigit).ToArray());
        if (string.IsNullOrWhiteSpace(digits))
        {
            DrawPlaceholder(canvas, width, height, value);
            return canvas;
        }

        if (digits.Length % 2 != 0)
            digits = $"0{digits}";

        var sequence = new List<(bool IsBar, int WidthUnits)>();
        sequence.Add((true, 1));
        sequence.Add((false, 1));
        sequence.Add((true, 1));
        sequence.Add((false, 1));

        for (var i = 0; i < digits.Length; i += 2)
        {
            var bars = ItfPatterns[digits[i]];
            var spaces = ItfPatterns[digits[i + 1]];

            for (var j = 0; j < 5; j++)
            {
                sequence.Add((true, bars[j] == 'w' ? 3 : 1));
                sequence.Add((false, spaces[j] == 'w' ? 3 : 1));
            }
        }

        sequence.Add((true, 3));
        sequence.Add((false, 1));
        sequence.Add((true, 1));

        const double quietZoneUnits = 10d;
        var totalUnits = sequence.Sum(x => x.WidthUnits) + (quietZoneUnits * 2);
        var narrowWidth = width / totalUnits;
        var currentX = quietZoneUnits * narrowWidth;

        foreach (var item in sequence)
        {
            var elementWidth = item.WidthUnits * narrowWidth;
            if (item.IsBar)
            {
                canvas.Children.Add(new Rectangle
                {
                    Width = Math.Max(1, elementWidth),
                    Height = height,
                    Fill = Brushes.Black
                });

                Canvas.SetLeft(canvas.Children[^1], currentX);
                Canvas.SetTop(canvas.Children[^1], 0);
            }

            currentX += elementWidth;
        }

        return canvas;
    }

    private static void DrawPlaceholder(Canvas canvas, double width, double height, string? value)
    {
        canvas.Children.Add(new Border
        {
            Width = width,
            Height = height,
            BorderBrush = Brushes.Black,
            BorderThickness = new Thickness(1),
            Child = new TextBlock
            {
                Text = string.IsNullOrWhiteSpace(value) ? "SIN CODIGO" : value,
                FontSize = 12,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.Wrap
            }
        });
    }
}
