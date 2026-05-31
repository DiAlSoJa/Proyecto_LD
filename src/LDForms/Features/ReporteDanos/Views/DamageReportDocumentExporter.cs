using LD.Contracts.DamageReports;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LD.FormsX.Views.ReporteDanos;

internal static class DamageReportDocumentExporter
{
    private const double Dpi = 96d;
    private const double PageWidth = 8.5d * Dpi;
    private const double PageHeight = 11d * Dpi;
    private const double MarginSize = 34d;
    private const double ReportWidth = 500d;

    public static void Print(DamageReportDto report, Func<string?, string?> imageUrlFactory)
    {
        var printDialog = new PrintDialog();
        if (printDialog.ShowDialog() != true)
            return;

        var document = new FixedDocument
        {
            DocumentPaginator = { PageSize = new Size(PageWidth, PageHeight) }
        };

        var page = BuildPrintPage(report, imageUrlFactory);
        page.Measure(new Size(PageWidth, PageHeight));
        page.Arrange(new Rect(0, 0, PageWidth, PageHeight));
        page.UpdateLayout();

        var pageContent = new PageContent();
        ((IAddChild)pageContent).AddChild(page);
        document.Pages.Add(pageContent);

        printDialog.PrintDocument(document.DocumentPaginator, $"Reporte de da\u00f1o {report.DamageReportId}");
    }

    public static async Task ExportExcelAsync(
        DamageReportDto report,
        string filePath,
        Func<string?, Task<byte[]?>> imageBytesProvider)
    {
        if (File.Exists(filePath))
            File.Delete(filePath);

        var firstPhotoPath = GetFirstPhotoPath(report);
        var photoBytes = string.IsNullOrWhiteSpace(firstPhotoPath)
            ? null
            : await TryGetImageBytesAsync(firstPhotoPath, imageBytesProvider);

        WriteExcelFile(filePath, report, photoBytes);
    }

    private static FixedPage BuildPrintPage(DamageReportDto report, Func<string?, string?> imageUrlFactory)
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
        root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        AddToGrid(root, BuildHeader(report), 0);

        AddToGrid(root, new Border
        {
            Height = 3,
            Background = Brushes.Black,
            Margin = new Thickness(0, 0, 0, 18)
        }, 1);

        AddToGrid(root, BuildFieldTable(BuildGeneralRows(report)), 2);

        AddToGrid(root, Text("Reporte de Da\u00f1o", 18, FontWeights.Bold, TextAlignment.Center, "Arial"), 3);
        root.Children[^1].SetValue(FrameworkElement.MarginProperty, new Thickness(0, 26, 0, 6));

        AddToGrid(root, BuildFieldTable(BuildDamageRows(report)), 4);
        AddToGrid(root, BuildPhotoBlock(report, imageUrlFactory), 5);
        AddToGrid(root, BuildFooter(), 7);

        page.Children.Add(root);
        return page;
    }

    private static FrameworkElement BuildHeader(DamageReportDto report)
    {
        var header = new Grid
        {
            Margin = new Thickness(0, 0, 0, 14)
        };

        header.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });
        header.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(250) });
        header.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

        var logo = new Image
        {
            Width = 105,
            Height = 68,
            Stretch = Stretch.Uniform,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Source = LoadLogo()
        };
        Grid.SetColumn(logo, 0);
        header.Children.Add(logo);

        var company = new StackPanel();
        company.Children.Add(Text("LOGISTICA FLEXIBLE", 17, FontWeights.Bold));
        company.Children.Add(Text("ALMACEN B1", 10, FontWeights.Bold));
        company.Children.Add(Text("CALZADA JUAN GIL PRECIADO NO. 2450", 10));
        company.Children.Add(Text("NAVE 23", 10));
        company.Children.Add(Text("EL TIGRE", 10));
        company.Children.Add(Text("ZAPOPAN, JALISCO", 10));
        Grid.SetColumn(company, 1);
        header.Children.Add(company);

        var right = new StackPanel
        {
            HorizontalAlignment = HorizontalAlignment.Stretch
        };

        right.Children.Add(new Border
        {
            BorderBrush = Brushes.Black,
            BorderThickness = new Thickness(2),
            Padding = new Thickness(8, 4, 8, 4),
            Child = Text("Reporte de Da\u00f1o", 20, FontWeights.Bold, TextAlignment.Center, "Arial")
        });

        right.Children.Add(Text($"No. RD: {report.DamageReportId}", 10, FontWeights.Normal, TextAlignment.Center));
        right.Children[^1].SetValue(FrameworkElement.MarginProperty, new Thickness(0, 36, 0, 0));

        Grid.SetColumn(right, 2);
        header.Children.Add(right);

        return header;
    }

    private static FrameworkElement BuildFieldTable(IReadOnlyList<(string Label, string Value)> rows)
    {
        var table = new Grid
        {
            Width = ReportWidth,
            HorizontalAlignment = HorizontalAlignment.Left
        };

        table.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(155) });
        table.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(ReportWidth - 155) });

        for (var rowIndex = 0; rowIndex < rows.Count; rowIndex++)
        {
            table.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            AddCell(table, rows[rowIndex].Label, rowIndex, 0, true);
            AddCell(table, rows[rowIndex].Value, rowIndex, 1, false);
        }

        return table;
    }

    private static FrameworkElement BuildPhotoBlock(DamageReportDto report, Func<string?, string?> imageUrlFactory)
    {
        var photoPath = GetFirstPhotoPath(report);
        var imageSource = LoadImage(photoPath, imageUrlFactory);

        var outer = new Border
        {
            Width = ReportWidth,
            Height = 200,
            BorderBrush = Brushes.Black,
            BorderThickness = new Thickness(1),
            Margin = new Thickness(0, 22, 0, 0),
            HorizontalAlignment = HorizontalAlignment.Left,
            ClipToBounds = true
        };

        var grid = new Grid();
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

        var left = new Border
        {
            BorderBrush = Brushes.Black,
            BorderThickness = new Thickness(0, 0, 1, 0),
            ClipToBounds = true
        };

        if (imageSource is not null)
        {
            left.Child = new Image
            {
                Source = imageSource,
                Stretch = Stretch.UniformToFill
            };
        }

        grid.Children.Add(left);
        outer.Child = grid;
        return outer;
    }

    private static FrameworkElement BuildFooter()
    {
        var footer = new Grid
        {
            Height = 52,
            Margin = new Thickness(-MarginSize, 0, -MarginSize, -MarginSize)
        };

        footer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(12) });
        footer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });

        var topLine = new Border { Background = new SolidColorBrush(Color.FromRgb(178, 185, 195)) };
        Grid.SetRow(topLine, 0);
        footer.Children.Add(topLine);

        var bar = new Border { Background = new SolidColorBrush(Color.FromRgb(37, 48, 112)) };
        Grid.SetRow(bar, 1);
        footer.Children.Add(bar);

        var logo = new Image
        {
            Width = 86,
            Height = 38,
            Source = LoadLogo(),
            Stretch = Stretch.Uniform,
            HorizontalAlignment = HorizontalAlignment.Right,
            Margin = new Thickness(0, 0, 42, 0)
        };
        Grid.SetRow(logo, 1);
        footer.Children.Add(logo);

        return footer;
    }

    private static void AddCell(Grid table, string value, int row, int column, bool isLabel)
    {
        var border = new Border
        {
            BorderBrush = Brushes.Black,
            BorderThickness = new Thickness(0.8),
            Padding = new Thickness(4, 2, 4, 2),
            Child = Text(value, 12, isLabel ? FontWeights.Bold : FontWeights.Normal)
        };

        Grid.SetRow(border, row);
        Grid.SetColumn(border, column);
        table.Children.Add(border);
    }

    private static IReadOnlyList<(string Label, string Value)> BuildGeneralRows(DamageReportDto report) =>
    [
        ("Cliente:", FormatValue(report.Client)),
        ("Proyecto:", FormatValue(report.Project)),
        ("No. ASN:", FormatValue(report.Asn)),
        ("No. Recepci\u00f3n:", FormatValue(report.AvailableInventoryId)),
        ("Fecha Recepci\u00f3n:", FormatDate(report.ReceptionDate)),
        ("EstandarID:", FormatValue(report.StandardIdCode ?? report.StandardId?.ToString(CultureInfo.InvariantCulture))),
        ("No. Parte:", FormatValue(report.PartNumber)),
        ("QTY:", FormatQuantity(report.AvailableQuantity ?? report.ReceivedQuantity)),
        ("Ubicaci\u00f3n:", FormatValue(report.Location)),
        ("Status:", FormatValue(report.CurrentStatus))
    ];

    private static IReadOnlyList<(string Label, string Value)> BuildDamageRows(DamageReportDto report) =>
    [
        ("Tipo de Da\u00f1o:", FormatValue(report.DamageType)),
        ("Categor\u00eda:", FormatValue(report.Category)),
        ("Nuevo Status:", FormatValue(report.NewStatus)),
        ("Comentarios:", FormatValue(report.Comments)),
        ("Usuario Captura:", FormatValue(report.ReportedByName ?? report.CreatedByUserName)),
        ("Hora Captura:", FormatDateTime(report.ReportDate))
    ];

    private static void WriteExcelFile(string filePath, DamageReportDto report, byte[]? photoBytes)
    {
        using var archive = ZipFile.Open(filePath, ZipArchiveMode.Create);
        var hasPhoto = photoBytes is { Length: > 0 };
        var extension = hasPhoto && IsPng(photoBytes!) ? "png" : "jpg";

        AddZipEntry(archive, "[Content_Types].xml", BuildContentTypesXml(hasPhoto, extension));
        AddZipEntry(archive, "_rels/.rels", RootRelationshipsXml);
        AddZipEntry(archive, "xl/_rels/workbook.xml.rels", WorkbookRelationshipsXml);
        AddZipEntry(archive, "xl/workbook.xml", WorkbookXml);
        AddZipEntry(archive, "xl/styles.xml", ExcelStylesXml);
        AddZipEntry(archive, "xl/worksheets/sheet1.xml", BuildWorksheetXml(report, hasPhoto));

        if (hasPhoto)
        {
            AddZipEntry(archive, "xl/worksheets/_rels/sheet1.xml.rels", WorksheetRelationshipsXml);
            AddZipEntry(archive, "xl/drawings/drawing1.xml", DrawingXml);
            AddZipEntry(archive, "xl/drawings/_rels/drawing1.xml.rels", $"<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\"><Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/image\" Target=\"../media/image1.{extension}\"/></Relationships>");
            AddBinaryZipEntry(archive, $"xl/media/image1.{extension}", photoBytes!);
        }

        var timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
        AddZipEntry(archive, "docProps/core.xml", BuildCoreXml(timestamp));
        AddZipEntry(archive, "docProps/app.xml", AppXml);
    }

    private static string BuildWorksheetXml(DamageReportDto report, bool hasPhoto)
    {
        var rows = new StringBuilder();
        rows.Append(Row(1, Cell("A1", "LOGISTICA FLEXIBLE", 3), Cell("D1", "Reporte de Da\u00f1o", 2)));
        rows.Append(Row(2, Cell("A2", "ALMACEN B1", 1), Cell("D2", $"No. RD: {report.DamageReportId}", 0)));
        rows.Append(Row(3, Cell("A3", "CALZADA JUAN GIL PRECIADO NO. 2450", 0)));
        rows.Append(Row(4, Cell("A4", "NAVE 23", 0)));
        rows.Append(Row(5, Cell("A5", "EL TIGRE, ZAPOPAN, JALISCO", 0)));

        var rowIndex = 7;
        foreach (var item in BuildGeneralRows(report))
        {
            rows.Append(Row(rowIndex, Cell($"A{rowIndex}", item.Label, 4), Cell($"B{rowIndex}", item.Value, 5)));
            rowIndex++;
        }

        rowIndex += 1;
        rows.Append(Row(rowIndex, Cell($"A{rowIndex}", "Reporte de Da\u00f1o", 2)));
        rowIndex++;

        foreach (var item in BuildDamageRows(report))
        {
            rows.Append(Row(rowIndex, Cell($"A{rowIndex}", item.Label, 4), Cell($"B{rowIndex}", item.Value, 5)));
            rowIndex++;
        }

        rowIndex += 1;
        rows.Append(Row(rowIndex, Cell($"A{rowIndex}", "Fotograf\u00eda", 1)));

        var drawing = hasPhoto ? "  <drawing r:id=\"rId1\"/>" : string.Empty;
        var mergeCells = """
          <mergeCells count="3">
            <mergeCell ref="A1:C1"/>
            <mergeCell ref="D1:F1"/>
            <mergeCell ref="A26:C26"/>
          </mergeCells>
        """;

        return $"""
            <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
            <worksheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships">
              <sheetViews><sheetView workbookViewId="0"/></sheetViews>
              <sheetFormatPr defaultRowHeight="18"/>
              <cols>
                <col min="1" max="1" width="22" customWidth="1"/>
                <col min="2" max="2" width="40" customWidth="1"/>
                <col min="3" max="3" width="4" customWidth="1"/>
                <col min="4" max="4" width="22" customWidth="1"/>
                <col min="5" max="5" width="28" customWidth="1"/>
                <col min="6" max="6" width="14" customWidth="1"/>
              </cols>
              <sheetData>
            {rows}
              </sheetData>
            {mergeCells}
            {drawing}
            </worksheet>
            """;
    }

    private static string BuildContentTypesXml(bool hasPhoto, string imageExtension)
    {
        var imageDefault = hasPhoto
            ? imageExtension == "png"
                ? "  <Default Extension=\"png\" ContentType=\"image/png\"/>"
                : "  <Default Extension=\"jpg\" ContentType=\"image/jpeg\"/>"
            : string.Empty;

        var drawingOverride = hasPhoto
            ? "  <Override PartName=\"/xl/drawings/drawing1.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.drawing+xml\"/>"
            : string.Empty;

        return $"""
            <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
            <Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types">
              <Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml"/>
              <Default Extension="xml" ContentType="application/xml"/>
            {imageDefault}
              <Override PartName="/xl/workbook.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml"/>
              <Override PartName="/xl/worksheets/sheet1.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml"/>
              <Override PartName="/xl/styles.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml"/>
            {drawingOverride}
              <Override PartName="/docProps/core.xml" ContentType="application/vnd.openxmlformats-package.core-properties+xml"/>
              <Override PartName="/docProps/app.xml" ContentType="application/vnd.openxmlformats-officedocument.extended-properties+xml"/>
            </Types>
            """;
    }

    private static string BuildCoreXml(string timestamp) =>
        $"""
        <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
        <cp:coreProperties xmlns:cp="http://schemas.openxmlformats.org/package/2006/metadata/core-properties" xmlns:dc="http://purl.org/dc/elements/1.1/" xmlns:dcterms="http://purl.org/dc/terms/" xmlns:dcmitype="http://purl.org/dc/dcmitype/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
          <dc:creator>LD</dc:creator>
          <cp:lastModifiedBy>LD</cp:lastModifiedBy>
          <dcterms:created xsi:type="dcterms:W3CDTF">{timestamp}</dcterms:created>
          <dcterms:modified xsi:type="dcterms:W3CDTF">{timestamp}</dcterms:modified>
        </cp:coreProperties>
        """;

    private static async Task<byte[]?> TryGetImageBytesAsync(
        string photoPath,
        Func<string?, Task<byte[]?>> imageBytesProvider)
    {
        try
        {
            return await imageBytesProvider(photoPath);
        }
        catch
        {
            return null;
        }
    }

    private static ImageSource? LoadImage(string? photoPath, Func<string?, string?> imageUrlFactory)
    {
        if (string.IsNullOrWhiteSpace(photoPath))
            return null;

        try
        {
            var url = imageUrlFactory(photoPath);
            if (string.IsNullOrWhiteSpace(url))
                return null;

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(url, UriKind.Absolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            bitmap.DecodePixelWidth = 900;
            bitmap.EndInit();
            bitmap.Freeze();
            return bitmap;
        }
        catch
        {
            return null;
        }
    }

    private static ImageSource? LoadLogo()
    {
        try
        {
            var bitmap = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/logo.png"));
            bitmap.Freeze();
            return bitmap;
        }
        catch
        {
            return null;
        }
    }

    private static string? GetFirstPhotoPath(DamageReportDto report) =>
        new[] { report.Photo1Path, report.Photo2Path, report.Photo3Path, report.Photo4Path }
            .FirstOrDefault(path => !string.IsNullOrWhiteSpace(path));

    private static void AddToGrid(Grid grid, UIElement element, int row)
    {
        Grid.SetRow(element, row);
        grid.Children.Add(element);
    }

    private static TextBlock Text(
        string text,
        double fontSize,
        FontWeight fontWeight = default,
        TextAlignment textAlignment = TextAlignment.Left,
        string fontFamily = "Times New Roman")
    {
        return new TextBlock
        {
            Text = text,
            FontSize = fontSize,
            FontWeight = fontWeight == default ? FontWeights.Normal : fontWeight,
            TextAlignment = textAlignment,
            TextWrapping = TextWrapping.Wrap,
            FontFamily = new FontFamily(fontFamily),
            Foreground = Brushes.Black,
            VerticalAlignment = VerticalAlignment.Center
        };
    }

    private static string FormatDate(DateTime? value) =>
        value?.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CurrentCulture) ?? "-";

    private static string FormatDateTime(DateTime value) =>
        value.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CurrentCulture);

    private static string FormatQuantity(decimal? value) =>
        value?.ToString("0.##", CultureInfo.CurrentCulture) ?? "-";

    private static string FormatValue(object? value)
    {
        if (value is null)
            return "-";

        var text = Convert.ToString(value, CultureInfo.CurrentCulture);
        return string.IsNullOrWhiteSpace(text) ? "-" : text.Trim();
    }

    private static bool IsPng(byte[] bytes) =>
        bytes.Length > 8
        && bytes[0] == 0x89
        && bytes[1] == 0x50
        && bytes[2] == 0x4E
        && bytes[3] == 0x47;

    private static void AddZipEntry(ZipArchive archive, string entryName, string content)
    {
        var entry = archive.CreateEntry(entryName);
        using var stream = entry.Open();
        using var writer = new StreamWriter(stream, new UTF8Encoding(false));
        writer.Write(content);
    }

    private static void AddBinaryZipEntry(ZipArchive archive, string entryName, byte[] content)
    {
        var entry = archive.CreateEntry(entryName);
        using var stream = entry.Open();
        stream.Write(content, 0, content.Length);
    }

    private static string Row(int index, params string[] cells) =>
        $"    <row r=\"{index}\">{string.Concat(cells)}</row>{Environment.NewLine}";

    private static string Cell(string reference, string? value, int style)
    {
        var escaped = SecurityElement.Escape(value ?? string.Empty) ?? string.Empty;
        return $"<c r=\"{reference}\" s=\"{style}\" t=\"inlineStr\"><is><t>{escaped}</t></is></c>";
    }

    private const string RootRelationshipsXml =
        """
        <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
        <Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
          <Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="xl/workbook.xml"/>
          <Relationship Id="rId2" Type="http://schemas.openxmlformats.org/package/2006/relationships/metadata/core-properties" Target="docProps/core.xml"/>
          <Relationship Id="rId3" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/extended-properties" Target="docProps/app.xml"/>
        </Relationships>
        """;

    private const string WorkbookRelationshipsXml =
        """
        <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
        <Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
          <Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet" Target="worksheets/sheet1.xml"/>
          <Relationship Id="rId2" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles" Target="styles.xml"/>
        </Relationships>
        """;

    private const string WorkbookXml =
        """
        <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
        <workbook xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships">
          <sheets>
            <sheet name="Reporte de Dano" sheetId="1" r:id="rId1"/>
          </sheets>
        </workbook>
        """;

    private const string WorksheetRelationshipsXml =
        """
        <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
        <Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
          <Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/drawing" Target="../drawings/drawing1.xml"/>
        </Relationships>
        """;

    private const string DrawingXml =
        """
        <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
        <xdr:wsDr xmlns:xdr="http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing" xmlns:a="http://schemas.openxmlformats.org/drawingml/2006/main" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships">
          <xdr:twoCellAnchor editAs="oneCell">
            <xdr:from><xdr:col>0</xdr:col><xdr:colOff>0</xdr:colOff><xdr:row>25</xdr:row><xdr:rowOff>0</xdr:rowOff></xdr:from>
            <xdr:to><xdr:col>3</xdr:col><xdr:colOff>0</xdr:colOff><xdr:row>38</xdr:row><xdr:rowOff>0</xdr:rowOff></xdr:to>
            <xdr:pic>
              <xdr:nvPicPr>
                <xdr:cNvPr id="2" name="Foto de dano"/>
                <xdr:cNvPicPr><a:picLocks noChangeAspect="1"/></xdr:cNvPicPr>
              </xdr:nvPicPr>
              <xdr:blipFill>
                <a:blip r:embed="rId1"/>
                <a:stretch><a:fillRect/></a:stretch>
              </xdr:blipFill>
              <xdr:spPr>
                <a:prstGeom prst="rect"><a:avLst/></a:prstGeom>
              </xdr:spPr>
            </xdr:pic>
            <xdr:clientData/>
          </xdr:twoCellAnchor>
        </xdr:wsDr>
        """;

    private const string AppXml =
        """
        <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
        <Properties xmlns="http://schemas.openxmlformats.org/officeDocument/2006/extended-properties" xmlns:vt="http://schemas.openxmlformats.org/officeDocument/2006/docPropsVTypes">
          <Application>LD</Application>
        </Properties>
        """;

    private const string ExcelStylesXml =
        """
        <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
        <styleSheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main">
          <fonts count="4">
            <font><sz val="11"/><name val="Calibri"/></font>
            <font><b/><sz val="11"/><name val="Calibri"/></font>
            <font><b/><sz val="16"/><name val="Calibri"/></font>
            <font><b/><sz val="18"/><name val="Calibri"/></font>
          </fonts>
          <fills count="4">
            <fill><patternFill patternType="none"/></fill>
            <fill><patternFill patternType="gray125"/></fill>
            <fill><patternFill patternType="solid"><fgColor rgb="FFEFEFEF"/><bgColor indexed="64"/></patternFill></fill>
            <fill><patternFill patternType="solid"><fgColor rgb="FFD9EAF7"/><bgColor indexed="64"/></patternFill></fill>
          </fills>
          <borders count="2">
            <border><left/><right/><top/><bottom/><diagonal/></border>
            <border><left style="thin"><color rgb="FF000000"/></left><right style="thin"><color rgb="FF000000"/></right><top style="thin"><color rgb="FF000000"/></top><bottom style="thin"><color rgb="FF000000"/></bottom><diagonal/></border>
          </borders>
          <cellStyleXfs count="1"><xf numFmtId="0" fontId="0" fillId="0" borderId="0"/></cellStyleXfs>
          <cellXfs count="6">
            <xf numFmtId="0" fontId="0" fillId="0" borderId="0" xfId="0"/>
            <xf numFmtId="0" fontId="1" fillId="0" borderId="0" xfId="0" applyFont="1"/>
            <xf numFmtId="0" fontId="2" fillId="0" borderId="1" xfId="0" applyFont="1" applyBorder="1" applyAlignment="1"><alignment horizontal="center" vertical="center"/></xf>
            <xf numFmtId="0" fontId="3" fillId="0" borderId="0" xfId="0" applyFont="1"/>
            <xf numFmtId="0" fontId="1" fillId="2" borderId="1" xfId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1"><alignment vertical="center" wrapText="1"/></xf>
            <xf numFmtId="0" fontId="0" fillId="0" borderId="1" xfId="0" applyBorder="1" applyAlignment="1"><alignment vertical="center" wrapText="1"/></xf>
          </cellXfs>
          <cellStyles count="1"><cellStyle name="Normal" xfId="0" builtinId="0"/></cellStyles>
          <dxfs count="0"/>
          <tableStyles count="0" defaultTableStyle="TableStyleMedium2" defaultPivotStyle="PivotStyleLight16"/>
        </styleSheet>
        """;
}
