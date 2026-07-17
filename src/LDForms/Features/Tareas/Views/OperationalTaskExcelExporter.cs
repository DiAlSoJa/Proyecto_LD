using LD.Contracts.DTOs.OperationalTasks;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace LD.FormsX.Views.Tareas;

internal static class OperationalTaskExcelExporter
{
    private sealed record PhotoSlot(string Title, string? Path, int FromCol, int ToCol, int FromRow, int ToRow, byte[]? Bytes = null, string? Extension = null);

    private sealed record SheetDefinition(string Name, string WorksheetXml, IReadOnlyList<PhotoSlot> PhotoSlots);

    private sealed record PhotoAsset(PhotoSlot Slot, string MediaName);

    public static async Task ExportAsync(
        OperationalTaskDto task,
        string filePath,
        Func<string?, Task<byte[]>> imageBytesProvider)
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(filePath);
        ArgumentNullException.ThrowIfNull(imageBytesProvider);

        var mainPhotos = await LoadPhotoSlotsAsync(BuildMainPhotoSlots(task), imageBytesProvider);
        var sheets = new List<SheetDefinition>
        {
            BuildMainSheet(task, mainPhotos)
        };

        if (HasResolutionContent(task))
        {
            var resolutionPhotos = await LoadPhotoSlotsAsync(BuildResolutionPhotoSlots(task), imageBytesProvider);
            sheets.Add(BuildResolutionSheet(task, resolutionPhotos));
        }

        WriteExcelFile(filePath, sheets);
    }

    private static SheetDefinition BuildMainSheet(OperationalTaskDto task, IReadOnlyList<PhotoSlot> photoSlots)
    {
        var worksheetXml = BuildWorksheetXml(
            title: "REPORTE DE TAREA",
            metaLine1Left: $"Folio: {task.OperationalTaskId}",
            metaLine1Right: $"Usuario: {FormatValue(task.CreatedByUserName)}",
            metaLine2Left: $"Almacen: {FormatValue(task.WarehouseName)}",
            metaLine2Right: $"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}",
            infoSectionTitle: "INFORMACION DE LA TAREA",
            infoRows: BuildGeneralRows(task),
            photoSectionTitle: "EVIDENCIA INICIAL",
            photoSlots: photoSlots,
            infoSectionRow: 4,
            photoSectionRow: 13,
            topPhotoTitleRow: 14,
            bottomPhotoTitleRow: 30,
            includeDrawing: photoSlots.Any(x => x.Bytes is { Length: >0 }));

        return new SheetDefinition("Tarea", worksheetXml, photoSlots);
    }

    private static SheetDefinition BuildResolutionSheet(OperationalTaskDto task, IReadOnlyList<PhotoSlot> photoSlots)
    {
        var worksheetXml = BuildWorksheetXml(
            title: "RESOLUCION DE TAREA",
            metaLine1Left: $"Folio: {task.OperationalTaskId}",
            metaLine1Right: $"Usuario: {FormatValue(task.CreatedByUserName)}",
            metaLine2Left: $"Almacen: {FormatValue(task.WarehouseName)}",
            metaLine2Right: $"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}",
            infoSectionTitle: "INFORMACION DE RESOLUCION",
            infoRows: BuildResolutionRows(task),
            photoSectionTitle: "FOTOS DE RESOLUCION",
            photoSlots: photoSlots,
            infoSectionRow: 4,
            photoSectionRow: 12,
            topPhotoTitleRow: 13,
            bottomPhotoTitleRow: 29,
            includeDrawing: photoSlots.Any(x => x.Bytes is { Length: >0 }));

        return new SheetDefinition("Resolucion", worksheetXml, photoSlots);
    }

    private static async Task<IReadOnlyList<PhotoSlot>> LoadPhotoSlotsAsync(
        IEnumerable<PhotoSlot> slots,
        Func<string?, Task<byte[]>> imageBytesProvider)
    {
        var tasks = slots.Select(async slot =>
        {
            if (string.IsNullOrWhiteSpace(slot.Path))
                return slot;

            byte[] bytes;
            try
            {
                bytes = await imageBytesProvider(slot.Path);
            }
            catch
            {
                bytes = Array.Empty<byte>();
            }

            if (bytes.Length == 0)
                return slot;

            return slot with
            {
                Bytes = bytes,
                Extension = DetectImageExtension(bytes)
            };
        });

        return await Task.WhenAll(tasks);
    }

    private static PhotoSlot[] BuildMainPhotoSlots(OperationalTaskDto task) =>
    [
        new("Foto inicial 1", task.Photo1Path, 0, 2, 14, 27),
        new("Foto inicial 2", task.Photo2Path, 3, 5, 14, 27),
        new("Foto inicial 3", task.Photo3Path, 0, 2, 30, 43),
        new("Foto inicial 4", task.Photo4Path, 3, 5, 30, 43)
    ];

    private static PhotoSlot[] BuildResolutionPhotoSlots(OperationalTaskDto task) =>
    [
        new("Foto final 1", task.ResolvedPhoto1Path, 0, 2, 13, 26),
        new("Foto final 2", task.ResolvedPhoto2Path, 3, 5, 13, 26),
        new("Foto final 3", task.ResolvedPhoto3Path, 0, 2, 29, 42),
        new("Foto final 4", task.ResolvedPhoto4Path, 3, 5, 29, 42)
    ];

    private static string BuildWorksheetXml(
        string title,
        string metaLine1Left,
        string metaLine1Right,
        string metaLine2Left,
        string metaLine2Right,
        string infoSectionTitle,
        IReadOnlyList<(string Label, string Value)> infoRows,
        string photoSectionTitle,
        IReadOnlyList<PhotoSlot> photoSlots,
        int infoSectionRow,
        int photoSectionRow,
        int topPhotoTitleRow,
        int bottomPhotoTitleRow,
        bool includeDrawing)
    {
        var rows = new StringBuilder();
        var merges = new List<string>();

        rows.Append(Row(1, Cell("A1", title, 2)));
        merges.Add("A1:F1");

        rows.Append(Row(2, Cell("A2", metaLine1Left, 1), Cell("D2", metaLine1Right, 1)));
        merges.Add("A2:C2");
        merges.Add("D2:F2");

        rows.Append(Row(3, Cell("A3", metaLine2Left, 1), Cell("D3", metaLine2Right, 1)));
        merges.Add("A3:C3");
        merges.Add("D3:F3");

        rows.Append(Row(infoSectionRow, Cell($"A{infoSectionRow}", infoSectionTitle, 3)));
        merges.Add($"A{infoSectionRow}:F{infoSectionRow}");

        var rowIndex = 5;
        foreach (var item in infoRows)
        {
            rows.Append(Row(
                rowIndex,
                Cell($"A{rowIndex}", item.Label, 4),
                Cell($"B{rowIndex}", item.Value, 5)));
            rowIndex++;
        }

        rows.Append(Row(photoSectionRow, Cell($"A{photoSectionRow}", photoSectionTitle, 3)));
        merges.Add($"A{photoSectionRow}:F{photoSectionRow}");

        rows.Append(Row(
            topPhotoTitleRow,
            Cell($"A{topPhotoTitleRow}", photoSlots[0].Title, 6),
            Cell($"D{topPhotoTitleRow}", photoSlots[1].Title, 6)));
        merges.Add($"A{topPhotoTitleRow}:C{topPhotoTitleRow}");
        merges.Add($"D{topPhotoTitleRow}:F{topPhotoTitleRow}");

        rows.Append(Row(
            bottomPhotoTitleRow,
            Cell($"A{bottomPhotoTitleRow}", photoSlots[2].Title, 6),
            Cell($"D{bottomPhotoTitleRow}", photoSlots[3].Title, 6)));
        merges.Add($"A{bottomPhotoTitleRow}:C{bottomPhotoTitleRow}");
        merges.Add($"D{bottomPhotoTitleRow}:F{bottomPhotoTitleRow}");

        var drawing = includeDrawing ? "  <drawing r:id=\"rId1\"/>" : string.Empty;

        return $"""
            <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
            <worksheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships">
              <sheetViews>
                <sheetView workbookViewId="0"/>
              </sheetViews>
              <sheetFormatPr defaultRowHeight="18"/>
              <cols>
                <col min="1" max="1" width="18" customWidth="1"/>
                <col min="2" max="2" width="38" customWidth="1"/>
                <col min="3" max="3" width="5" customWidth="1"/>
                <col min="4" max="4" width="18" customWidth="1"/>
                <col min="5" max="5" width="38" customWidth="1"/>
                <col min="6" max="6" width="5" customWidth="1"/>
              </cols>
              <sheetData>
            {rows}
              </sheetData>
              <mergeCells count="{merges.Count}">
            {BuildMergeCells(merges)}
              </mergeCells>
            {drawing}
            </worksheet>
            """;
    }

    private static void WriteExcelFile(string filePath, IReadOnlyList<SheetDefinition> sheets)
    {
        if (File.Exists(filePath))
            File.Delete(filePath);

        var drawingCount = sheets.Count(sheet => sheet.PhotoSlots.Any(slot => slot.Bytes is { Length: >0 }));
        var hasPng = sheets.SelectMany(sheet => sheet.PhotoSlots)
            .Any(slot => slot.Extension == "png");
        var hasJpg = sheets.SelectMany(sheet => sheet.PhotoSlots)
            .Any(slot => slot.Extension is null || slot.Extension == "jpg");

        using var archive = ZipFile.Open(filePath, ZipArchiveMode.Create);

        AddZipEntry(archive, "[Content_Types].xml", BuildContentTypesXml(sheets.Count, drawingCount, hasPng, hasJpg));
        AddZipEntry(archive, "_rels/.rels", RootRelationshipsXml);
        AddZipEntry(archive, "xl/_rels/workbook.xml.rels", BuildWorkbookRelationshipsXml(sheets.Count));
        AddZipEntry(archive, "xl/workbook.xml", BuildWorkbookXml(sheets));
        AddZipEntry(archive, "xl/styles.xml", ExcelStylesXml);

        var nextDrawingIndex = 0;
        var nextMediaIndex = 0;

        for (var i = 0; i < sheets.Count; i++)
        {
            var sheetIndex = i + 1;
            var sheet = sheets[i];
            AddZipEntry(archive, $"xl/worksheets/sheet{sheetIndex}.xml", sheet.WorksheetXml);

            var imageSlots = sheet.PhotoSlots
                .Where(slot => slot.Bytes is { Length: >0 })
                .ToList();

            if (imageSlots.Count == 0)
                continue;

            nextDrawingIndex++;

            var assets = new List<PhotoAsset>();
            foreach (var slot in imageSlots)
            {
                nextMediaIndex++;
                var extension = slot.Extension ?? DetectImageExtension(slot.Bytes!);
                var mediaName = $"image{nextMediaIndex}.{extension}";
                assets.Add(new PhotoAsset(slot, mediaName));
                AddBinaryZipEntry(archive, $"xl/media/{mediaName}", slot.Bytes!);
            }

            AddZipEntry(archive, $"xl/drawings/drawing{nextDrawingIndex}.xml", BuildDrawingXml(assets));
            AddZipEntry(archive, $"xl/drawings/_rels/drawing{nextDrawingIndex}.xml.rels", BuildDrawingRelationshipsXml(assets));
            AddZipEntry(archive, $"xl/worksheets/_rels/sheet{sheetIndex}.xml.rels", WorksheetRelationshipsXml(nextDrawingIndex));
        }

        var timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
        AddZipEntry(archive, "docProps/core.xml", BuildCoreXml(timestamp));
        AddZipEntry(archive, "docProps/app.xml", AppXml);
    }

    private static string BuildContentTypesXml(int sheetCount, int drawingCount, bool hasPng, bool hasJpg)
    {
        var builder = new StringBuilder();
        builder.AppendLine("""<?xml version="1.0" encoding="UTF-8" standalone="yes"?>""");
        builder.AppendLine("""<Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types">""");
        builder.AppendLine("""  <Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml"/>""");
        builder.AppendLine("""  <Default Extension="xml" ContentType="application/xml"/>""");

        if (hasPng)
            builder.AppendLine("""  <Default Extension="png" ContentType="image/png"/>""");

        if (hasJpg)
            builder.AppendLine("""  <Default Extension="jpg" ContentType="image/jpeg"/>""");

        builder.AppendLine("""  <Override PartName="/xl/workbook.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml"/>""");

        for (var i = 1; i <= sheetCount; i++)
            builder.AppendLine($"""  <Override PartName="/xl/worksheets/sheet{i}.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml"/>""");

        builder.AppendLine("""  <Override PartName="/xl/styles.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml"/>""");

        for (var i = 1; i <= drawingCount; i++)
            builder.AppendLine($"""  <Override PartName="/xl/drawings/drawing{i}.xml" ContentType="application/vnd.openxmlformats-officedocument.drawing+xml"/>""");

        builder.AppendLine("""  <Override PartName="/docProps/core.xml" ContentType="application/vnd.openxmlformats-package.core-properties+xml"/>""");
        builder.AppendLine("""  <Override PartName="/docProps/app.xml" ContentType="application/vnd.openxmlformats-officedocument.extended-properties+xml"/>""");
        builder.AppendLine("""</Types>""");

        return builder.ToString();
    }

    private static string BuildWorkbookRelationshipsXml(int sheetCount)
    {
        var builder = new StringBuilder();
        builder.AppendLine("""<?xml version="1.0" encoding="UTF-8" standalone="yes"?>""");
        builder.AppendLine("""<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">""");

        for (var i = 1; i <= sheetCount; i++)
        {
            builder.AppendLine($"""  <Relationship Id="rId{i}" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet" Target="worksheets/sheet{i}.xml"/>""");
        }

        builder.AppendLine($"""  <Relationship Id="rId{sheetCount + 1}" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles" Target="styles.xml"/>""");
        builder.AppendLine("""</Relationships>""");

        return builder.ToString();
    }

    private static string BuildWorkbookXml(IReadOnlyList<SheetDefinition> sheets)
    {
        var builder = new StringBuilder();
        builder.AppendLine("""<?xml version="1.0" encoding="UTF-8" standalone="yes"?>""");
        builder.AppendLine("""<workbook xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships">""");
        builder.AppendLine("""  <sheets>""");

        for (var i = 0; i < sheets.Count; i++)
        {
            builder.AppendLine($"""    <sheet name="{SecurityElement.Escape(sheets[i].Name)}" sheetId="{i + 1}" r:id="rId{i + 1}"/>""");
        }

        builder.AppendLine("""  </sheets>""");
        builder.AppendLine("""</workbook>""");
        return builder.ToString();
    }

    private static string BuildDrawingXml(IReadOnlyList<PhotoAsset> assets)
    {
        var builder = new StringBuilder();
        builder.AppendLine("""<?xml version="1.0" encoding="UTF-8" standalone="yes"?>""");
        builder.AppendLine("""<xdr:wsDr xmlns:xdr="http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing" xmlns:a="http://schemas.openxmlformats.org/drawingml/2006/main">""");

        for (var i = 0; i < assets.Count; i++)
        {
            var asset = assets[i];
            builder.AppendLine($"""
              <xdr:twoCellAnchor editAs="oneCell">
                <xdr:from>
                  <xdr:col>{asset.Slot.FromCol}</xdr:col>
                  <xdr:colOff>0</xdr:colOff>
                  <xdr:row>{asset.Slot.FromRow}</xdr:row>
                  <xdr:rowOff>0</xdr:rowOff>
                </xdr:from>
                <xdr:to>
                  <xdr:col>{asset.Slot.ToCol}</xdr:col>
                  <xdr:colOff>0</xdr:colOff>
                  <xdr:row>{asset.Slot.ToRow}</xdr:row>
                  <xdr:rowOff>0</xdr:rowOff>
                </xdr:to>
                <xdr:pic>
                  <xdr:nvPicPr>
                    <xdr:cNvPr id="{i + 1}" name="Picture {i + 1}"/>
                    <xdr:cNvPicPr/>
                    <xdr:nvPr/>
                  </xdr:nvPicPr>
                  <xdr:blipFill>
                    <a:blip r:embed="rId{i + 1}" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships"/>
                    <a:stretch>
                      <a:fillRect/>
                    </a:stretch>
                  </xdr:blipFill>
                  <xdr:spPr>
                    <a:xfrm>
                      <a:off x="0" y="0"/>
                      <a:ext cx="0" cy="0"/>
                    </a:xfrm>
                    <a:prstGeom prst="rect">
                      <a:avLst/>
                    </a:prstGeom>
                  </xdr:spPr>
                </xdr:pic>
                <xdr:clientData/>
              </xdr:twoCellAnchor>
            """);
        }

        builder.AppendLine("""</xdr:wsDr>""");
        return builder.ToString();
    }

    private static string BuildDrawingRelationshipsXml(IReadOnlyList<PhotoAsset> assets)
    {
        var builder = new StringBuilder();
        builder.AppendLine("""<?xml version="1.0" encoding="UTF-8" standalone="yes"?>""");
        builder.AppendLine("""<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">""");

        for (var i = 0; i < assets.Count; i++)
        {
            builder.AppendLine($"""  <Relationship Id="rId{i + 1}" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/image" Target="../media/{assets[i].MediaName}"/>""");
        }

        builder.AppendLine("""</Relationships>""");
        return builder.ToString();
    }

    private static string WorksheetRelationshipsXml(int drawingIndex) =>
        $"""
        <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
        <Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
          <Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/drawing" Target="../drawings/drawing{drawingIndex}.xml"/>
        </Relationships>
        """;

    private static IReadOnlyList<(string Label, string Value)> BuildGeneralRows(OperationalTaskDto task) =>
    [
        ("Folio", FormatValue(task.OperationalTaskId.ToString(CultureInfo.InvariantCulture))),
        ("Almacen", FormatValue(task.WarehouseName)),
        ("Prioridad", FormatValue(task.Priority)),
        ("Actividad", FormatValue(task.Activity)),
        ("Nombre", FormatValue(task.Name)),
        ("Descripcion", FormatValue(task.Description)),
        ("Creado el", FormatDateTime(task.CreatedAt)),
        ("Estatus", task.Completed ? "Finalizada" : "Pendiente")
    ];

    private static IReadOnlyList<(string Label, string Value)> BuildResolutionRows(OperationalTaskDto task) =>
    [
        ("Folio", FormatValue(task.OperationalTaskId.ToString(CultureInfo.InvariantCulture))),
        ("Finalizado", task.Completed ? "Si" : "No"),
        ("Finalizo", FormatValue(task.CompletedByName)),
        ("Fecha finalizacion", FormatDateTime(task.CompletedAt)),
        ("Observaciones", FormatValue(task.ResolutionObservations)),
        ("Estatus", task.Completed ? "Finalizada" : "Pendiente")
    ];

    private static bool HasResolutionContent(OperationalTaskDto task)
    {
        return task.Completed
            || task.CompletedAt.HasValue
            || !string.IsNullOrWhiteSpace(task.CompletedByName)
            || !string.IsNullOrWhiteSpace(task.ResolutionObservations)
            || BuildResolutionPhotoSlots(task).Any(photo => !string.IsNullOrWhiteSpace(photo.Path));
    }

    private static string BuildMergeCells(IReadOnlyList<string> merges)
    {
        var builder = new StringBuilder();
        foreach (var merge in merges)
            builder.AppendLine($"    <mergeCell ref=\"{merge}\"/>");

        return builder.ToString();
    }

    private static string Row(int index, params string[] cells) =>
        $"    <row r=\"{index}\">{string.Join(string.Empty, cells)}</row>{Environment.NewLine}";

    private static string Cell(string reference, string value, int style)
    {
        var escaped = SecurityElement.Escape(value) ?? string.Empty;
        return $"<c r=\"{reference}\" t=\"inlineStr\" s=\"{style}\"><is><t>{escaped}</t></is></c>";
    }

    private static string FormatValue(string? value) => string.IsNullOrWhiteSpace(value) ? "-" : value.Trim();

    private static string FormatDateTime(DateTime? value) => value?.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture) ?? "-";

    private static string DetectImageExtension(byte[] bytes)
    {
        if (bytes.Length >= 4 && bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47)
            return "png";

        return "jpg";
    }

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

    private static string RootRelationshipsXml =>
        """<?xml version="1.0" encoding="UTF-8" standalone="yes"?><Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships"><Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="xl/workbook.xml"/><Relationship Id="rId2" Type="http://schemas.openxmlformats.org/package/2006/relationships/metadata/core-properties" Target="docProps/core.xml"/><Relationship Id="rId3" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/extended-properties" Target="docProps/app.xml"/></Relationships>""";

    private static string ExcelStylesXml =>
        """
        <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
        <styleSheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main">
          <fonts count="4">
            <font><sz val="11"/><name val="Calibri"/></font>
            <font><b/><sz val="11"/><name val="Calibri"/></font>
            <font><b/><color rgb="FFFFFFFF"/><sz val="16"/><name val="Calibri"/></font>
            <font><b/><color rgb="FFFFFFFF"/><sz val="11"/><name val="Calibri"/></font>
          </fonts>
          <fills count="5">
            <fill><patternFill patternType="none"/></fill>
            <fill><patternFill patternType="gray125"/></fill>
            <fill><patternFill patternType="solid"><fgColor rgb="FF233167"/><bgColor indexed="64"/></patternFill></fill>
            <fill><patternFill patternType="solid"><fgColor rgb="FFEAF2FB"/><bgColor indexed="64"/></patternFill></fill>
            <fill><patternFill patternType="solid"><fgColor rgb="FFF2F4F7"/><bgColor indexed="64"/></patternFill></fill>
          </fills>
          <borders count="2">
            <border><left/><right/><top/><bottom/><diagonal/></border>
            <border><left style="thin"><color rgb="FFD9D9D9"/></left><right style="thin"><color rgb="FFD9D9D9"/></right><top style="thin"><color rgb="FFD9D9D9"/></top><bottom style="thin"><color rgb="FFD9D9D9"/></bottom><diagonal/></border>
          </borders>
          <cellStyleXfs count="1"><xf numFmtId="0" fontId="0" fillId="0" borderId="0"/></cellStyleXfs>
          <cellXfs count="7">
            <xf numFmtId="0" fontId="0" fillId="0" borderId="0" xfId="0"/>
            <xf numFmtId="0" fontId="1" fillId="0" borderId="0" xfId="0" applyFont="1"/>
            <xf numFmtId="0" fontId="2" fillId="2" borderId="0" xfId="0" applyFont="1" applyFill="1" applyAlignment="1">
              <alignment horizontal="center" vertical="center" wrapText="1"/>
            </xf>
            <xf numFmtId="0" fontId="3" fillId="2" borderId="0" xfId="0" applyFont="1" applyFill="1" applyAlignment="1">
              <alignment horizontal="left" vertical="center" wrapText="1"/>
            </xf>
            <xf numFmtId="0" fontId="1" fillId="3" borderId="1" xfId="0" applyFont="1" applyFill="1" applyBorder="1"/>
            <xf numFmtId="0" fontId="0" fillId="0" borderId="1" xfId="0" applyBorder="1"/>
            <xf numFmtId="0" fontId="1" fillId="4" borderId="1" xfId="0" applyFont="1" applyFill="1" applyBorder="1"/>
          </cellXfs>
          <cellStyles count="1"><cellStyle name="Normal" xfId="0" builtinId="0"/></cellStyles>
          <dxfs count="0"/>
          <tableStyles count="0" defaultTableStyle="TableStyleMedium2" defaultPivotStyle="PivotStyleLight16"/>
        </styleSheet>
        """;

    private static string AppXml =>
        """<?xml version="1.0" encoding="UTF-8" standalone="yes"?><Properties xmlns="http://schemas.openxmlformats.org/officeDocument/2006/extended-properties" xmlns:vt="http://schemas.openxmlformats.org/officeDocument/2006/docPropsVTypes"><Application>LD.FormsX</Application></Properties>""";

    private static string BuildCoreXml(string timestamp) =>
        $"""<?xml version="1.0" encoding="UTF-8" standalone="yes"?><cp:coreProperties xmlns:cp="http://schemas.openxmlformats.org/package/2006/metadata/core-properties" xmlns:dc="http://purl.org/dc/elements/1.1/" xmlns:dcterms="http://purl.org/dc/terms/" xmlns:dcmitype="http://purl.org/dc/dcmitype/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"><dc:creator>LD.FormsX</dc:creator><cp:lastModifiedBy>LD.FormsX</cp:lastModifiedBy><dcterms:created xsi:type="dcterms:W3CDTF">{timestamp}</dcterms:created><dcterms:modified xsi:type="dcterms:W3CDTF">{timestamp}</dcterms:modified></cp:coreProperties>""";
}
