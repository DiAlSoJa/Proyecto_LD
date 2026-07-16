using LD.Client.Configuration;
using LD.Contracts.DamageReports;
using LD.Contracts.Requests;
using Microsoft.Maui.Storage;
using System.Globalization;
using System.IO.Compression;
using System.Security;
using System.Text;

namespace MauiAppLogin;

internal static class DamageReportExcelExporter
{
    private const int PhotoAnchorStartRow = 26;
    private const int PhotoAnchorEndRow = 45;

    public static async Task<string> CreateAsync(DamageReportRequest report, IReadOnlyList<string> photoPaths)
    {
        ArgumentNullException.ThrowIfNull(report);
        ArgumentNullException.ThrowIfNull(photoPaths);

        var reportCode = EnsureReportCode(report);
        var fileName = $"reporte-danos-{reportCode}-{DateTime.Now:yyyyMMdd-HHmmss}.xlsx";
        var filePath = Path.Combine(FileSystem.CacheDirectory, fileName);

        var photoBytes = await GetFirstPhotoBytesAsync(photoPaths);
        WriteExcelFile(filePath, report, reportCode, photoBytes);

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

    private static async Task<byte[]?> GetFirstPhotoBytesAsync(IReadOnlyList<string> photoPaths)
    {
        var firstPhotoPath = photoPaths.FirstOrDefault(path => !string.IsNullOrWhiteSpace(path) && File.Exists(path));
        if (string.IsNullOrWhiteSpace(firstPhotoPath))
            return null;

        try
        {
            return await File.ReadAllBytesAsync(firstPhotoPath);
        }
        catch
        {
            return null;
        }
    }

    private static void WriteExcelFile(string filePath, DamageReportRequest report, string reportCode, byte[]? photoBytes)
    {
        if (File.Exists(filePath))
            File.Delete(filePath);

        using var archive = ZipFile.Open(filePath, ZipArchiveMode.Create);
        var hasPhoto = photoBytes is { Length: > 0 };
        var extension = hasPhoto && IsPng(photoBytes!) ? "png" : "jpg";

        AddZipEntry(archive, "[Content_Types].xml", BuildContentTypesXml(hasPhoto, extension));
        AddZipEntry(archive, "_rels/.rels", RootRelationshipsXml);
        AddZipEntry(archive, "xl/_rels/workbook.xml.rels", WorkbookRelationshipsXml);
        AddZipEntry(archive, "xl/workbook.xml", WorkbookXml);
        AddZipEntry(archive, "xl/styles.xml", ExcelStylesXml);
        AddZipEntry(archive, "xl/worksheets/sheet1.xml", BuildWorksheetXml(report, reportCode, hasPhoto));

        if (hasPhoto)
        {
            AddZipEntry(archive, "xl/worksheets/_rels/sheet1.xml.rels", WorksheetRelationshipsXml);
            AddZipEntry(archive, "xl/drawings/drawing1.xml", BuildDrawingXml(PhotoAnchorStartRow, PhotoAnchorEndRow));
            AddZipEntry(archive, "xl/drawings/_rels/drawing1.xml.rels", $"<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\"><Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/image\" Target=\"../media/image1.{extension}\"/></Relationships>");
            AddBinaryZipEntry(archive, $"xl/media/image1.{extension}", photoBytes!);
        }

        var timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
        AddZipEntry(archive, "docProps/core.xml", BuildCoreXml(timestamp));
        AddZipEntry(archive, "docProps/app.xml", AppXml);
    }

    private static string BuildWorksheetXml(DamageReportRequest report, string reportCode, bool hasPhoto)
    {
        var rows = new StringBuilder();
        rows.Append(Row(1, Cell("A1", "LOGISTICA FLEXIBLE", 3), Cell("D1", "Reporte de Daño", 2)));
        rows.Append(Row(2, Cell("A2", "ALMACEN B1", 1), Cell("D2", $"No. RD: {BuildReportReference(report, reportCode)}", 0)));
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
        rows.Append(Row(rowIndex, Cell($"A{rowIndex}", "Reporte de Daño", 2)));
        rowIndex++;

        foreach (var item in BuildDamageRows(report))
        {
            rows.Append(Row(rowIndex, Cell($"A{rowIndex}", item.Label, 4), Cell($"B{rowIndex}", item.Value, 5)));
            rowIndex++;
        }

        rowIndex += 1;
        rows.Append(Row(rowIndex, Cell($"A{rowIndex}", "Fotografia", 1)));

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
              <Override PartName="/docProps/core.xml" ContentType="application/vnd.openxmlformats-package.core-properties+xml"/>
              <Override PartName="/docProps/app.xml" ContentType="application/vnd.openxmlformats-officedocument.extended-properties+xml"/>
            {drawingOverride}
            </Types>
            """;
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

    private static string BuildReportReference(DamageReportRequest report, string reportCode)
    {
        return report.DamageReportId > 0
            ? report.DamageReportId.ToString(CultureInfo.InvariantCulture)
            : reportCode;
    }

    private static string FormatValue(string? value) => string.IsNullOrWhiteSpace(value) ? "-" : value.Trim();

    private static string FormatDate(DateTime? value) => value?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) ?? "-";

    private static string FormatDateTime(DateTime? value) => value?.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture) ?? "-";

    private static string FormatQuantity(decimal? value) => value?.ToString(CultureInfo.InvariantCulture) ?? "-";

    private static void AddZipEntry(ZipArchive archive, string entryName, string content)
    {
        var entry = archive.CreateEntry(entryName, CompressionLevel.Optimal);
        using var stream = entry.Open();
        using var writer = new StreamWriter(stream, new UTF8Encoding(false));
        writer.Write(content);
    }

    private static void AddBinaryZipEntry(ZipArchive archive, string entryName, byte[] content)
    {
        var entry = archive.CreateEntry(entryName, CompressionLevel.Optimal);
        using var stream = entry.Open();
        stream.Write(content, 0, content.Length);
    }

    private static bool IsPng(byte[] bytes) => bytes.Length > 4 && bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47;

    private static string Cell(string reference, string value, int style)
        => $"    <c r=\"{reference}\" t=\"inlineStr\" s=\"{style}\"><is><t>{SecurityElement.Escape(value)}</t></is></c>";

    private static string Row(int index, params string[] cells)
        => $"    <row r=\"{index}\">{string.Join(string.Empty, cells)}</row>\n";

    private static string RootRelationshipsXml =>
        """<?xml version="1.0" encoding="UTF-8" standalone="yes"?><Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships"><Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="xl/workbook.xml"/></Relationships>""";

    private static string WorkbookRelationshipsXml =>
        """<?xml version="1.0" encoding="UTF-8" standalone="yes"?><Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships"><Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet" Target="worksheets/sheet1.xml"/><Relationship Id="rId2" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles" Target="styles.xml"/></Relationships>""";

    private static string WorksheetRelationshipsXml =>
        """<?xml version="1.0" encoding="UTF-8" standalone="yes"?><Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships"><Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/drawing" Target="../drawings/drawing1.xml"/></Relationships>""";

    private static string WorkbookXml =>
        """<?xml version="1.0" encoding="UTF-8" standalone="yes"?><workbook xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships"><sheets><sheet name="Reporte" sheetId="1" r:id="rId1"/></sheets></workbook>""";

    private static string BuildDrawingXml(int fromRow, int toRow) =>
        $"""<?xml version="1.0" encoding="UTF-8" standalone="yes"?><xdr:wsDr xmlns:xdr="http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing" xmlns:a="http://schemas.openxmlformats.org/drawingml/2006/main"><xdr:twoCellAnchor editAs="oneCell"><xdr:from><xdr:col>0</xdr:col><xdr:colOff>0</xdr:colOff><xdr:row>{fromRow}</xdr:row><xdr:rowOff>0</xdr:rowOff></xdr:from><xdr:to><xdr:col>5</xdr:col><xdr:colOff>0</xdr:colOff><xdr:row>{toRow}</xdr:row><xdr:rowOff>0</xdr:rowOff></xdr:to><xdr:pic><xdr:nvPicPr><xdr:cNvPr id="1" name="Picture 1"/><xdr:cNvPicPr/><xdr:nvPr/></xdr:nvPicPr><xdr:blipFill><a:blip r:embed="rId1" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships"/><a:stretch><a:fillRect/></a:stretch></xdr:blipFill><xdr:spPr><a:xfrm><a:off x="0" y="0"/><a:ext cx="0" cy="0"/></a:xfrm><a:prstGeom prst="rect"><a:avLst/></a:prstGeom></xdr:spPr></xdr:pic><xdr:clientData/></xdr:twoCellAnchor></xdr:wsDr>""";

    private static string ExcelStylesXml =>
        """<?xml version="1.0" encoding="UTF-8" standalone="yes"?><styleSheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main"><fonts count="3"><font><sz val="11"/><name val="Calibri"/></font><font><b/><sz val="11"/><name val="Calibri"/></font><font><b/><sz val="16"/><name val="Calibri"/></font></fonts><fills count="2"><fill><patternFill patternType="none"/></fill><fill><patternFill patternType="gray125"/></fill></fills><borders count="2"><border><left/><right/><top/><bottom/><diagonal/></border><border><left style="thin"/><right style="thin"/><top style="thin"/><bottom style="thin"/><diagonal/></border></borders><cellStyleXfs count="1"><xf numFmtId="0" fontId="0" fillId="0" borderId="0"/></cellStyleXfs><cellXfs count="6"><xf numFmtId="0" fontId="0" fillId="0" borderId="0" xfId="0"/><xf numFmtId="0" fontId="1" fillId="0" borderId="1" xfId="0" applyFont="1" applyBorder="1"/><xf numFmtId="0" fontId="2" fillId="0" borderId="0" xfId="0" applyFont="1"/><xf numFmtId="0" fontId="1" fillId="0" borderId="1" xfId="0" applyFont="1" applyBorder="1"/><xf numFmtId="0" fontId="1" fillId="0" borderId="1" xfId="0" applyFont="1" applyBorder="1"/><xf numFmtId="0" fontId="0" fillId="0" borderId="1" xfId="0" applyBorder="1"/></cellXfs></styleSheet>""";

    private static string AppXml =>
        """<?xml version="1.0" encoding="UTF-8" standalone="yes"?><Properties xmlns="http://schemas.openxmlformats.org/officeDocument/2006/extended-properties" xmlns:vt="http://schemas.openxmlformats.org/officeDocument/2006/docPropsVTypes"><Application>MauiAppLogin</Application></Properties>""";

    private static string BuildCoreXml(string timestamp) =>
        $"""<?xml version="1.0" encoding="UTF-8" standalone="yes"?><cp:coreProperties xmlns:cp="http://schemas.openxmlformats.org/package/2006/metadata/core-properties" xmlns:dc="http://purl.org/dc/elements/1.1/" xmlns:dcterms="http://purl.org/dc/terms/" xmlns:dcmitype="http://purl.org/dc/dcmitype/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"><dc:creator>MauiAppLogin</dc:creator><cp:lastModifiedBy>MauiAppLogin</cp:lastModifiedBy><dcterms:created xsi:type="dcterms:W3CDTF">{timestamp}</dcterms:created><dcterms:modified xsi:type="dcterms:W3CDTF">{timestamp}</dcterms:modified></cp:coreProperties>""";
}
