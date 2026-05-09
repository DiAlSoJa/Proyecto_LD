using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media;

namespace LD.FormsX.Helpers
{
    public static class DataGridExportExtensions
    {
        public static string ExportVisibleRowsToExcel(this DataGrid dataGrid, string? filePrefix = null)
        {
            var visibleColumns = dataGrid.Columns
                .Where(c => c.Visibility == System.Windows.Visibility.Visible)
                .OrderBy(c => c.DisplayIndex)
                .ToList();

            if (visibleColumns.Count == 0)
                throw new InvalidOperationException("No hay columnas visibles para exportar.");

            var rows = dataGrid.Items
                .Cast<object>()
                .Where(item => item != CollectionView.NewItemPlaceholder)
                .ToList();

            if (rows.Count == 0)
                throw new InvalidOperationException("No hay datos para exportar.");

            var exportFolder = Path.Combine(Path.GetTempPath(), "LD", "Exports");
            Directory.CreateDirectory(exportFolder);

            var safePrefix = SanitizeFileName(string.IsNullOrWhiteSpace(filePrefix) ? dataGrid.Name : filePrefix);
            var filePath = Path.Combine(exportFolder, $"{safePrefix}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");

            WriteExcelFile(filePath, visibleColumns, rows);

            Process.Start(new ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true
            });

            return filePath;
        }

        public static string ExportVisibleRowsToCsv(this DataGrid dataGrid, string? filePrefix = null) =>
            ExportVisibleRowsToExcel(dataGrid, filePrefix);

        private static string GetColumnHeaderText(DataGridColumn column)
        {
            var headerText = ExtractHeaderText(column.Header);
            if (!string.IsNullOrWhiteSpace(headerText))
                return NormalizeExcelText(headerText);

            if (!string.IsNullOrWhiteSpace(column.SortMemberPath))
                return NormalizeExcelText(column.SortMemberPath);

            if (column is DataGridBoundColumn boundColumn &&
                boundColumn.Binding is Binding binding &&
                !string.IsNullOrWhiteSpace(binding.Path?.Path))
            {
                return NormalizeExcelText(binding.Path.Path);
            }

            return "Columna";
        }

        private static string ExtractHeaderText(object? header)
        {
            if (header == null)
                return string.Empty;

            if (header is string text)
                return text;

            if (header is TextBlock textBlock)
                return textBlock.Text ?? string.Empty;

            if (header is ContentControl contentControl)
                return ExtractHeaderText(contentControl.Content);

            if (header is HeaderedContentControl headeredContentControl)
                return ExtractHeaderText(headeredContentControl.Header);

            if (header is FrameworkElement frameworkElement)
            {
                var nestedText = FindTextInVisualTree(frameworkElement);
                if (!string.IsNullOrWhiteSpace(nestedText))
                    return nestedText;
            }

            return header.ToString() ?? string.Empty;
        }

        private static string FindTextInVisualTree(DependencyObject parent)
        {
            var childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is TextBlock textBlock && !string.IsNullOrWhiteSpace(textBlock.Text))
                    return textBlock.Text;

                var nestedText = FindTextInVisualTree(child);
                if (!string.IsNullOrWhiteSpace(nestedText))
                    return nestedText;
            }

            return string.Empty;
        }

        private static string GetColumnValue(DataGridColumn column, object row)
        {
            if (column is DataGridBoundColumn boundColumn &&
                boundColumn.Binding is Binding binding)
            {
                var value = GetPropertyValue(row, binding.Path?.Path);

                if (value == null)
                    return string.Empty;

                if (!string.IsNullOrWhiteSpace(binding.StringFormat))
                    return NormalizeExcelText(string.Format(CultureInfo.CurrentCulture, binding.StringFormat, value));

                return FormatValue(value);
            }

            var fallbackValue = GetPropertyValue(row, column.SortMemberPath);
            return FormatValue(fallbackValue);
        }

        private static object? GetPropertyValue(object item, string? propertyPath)
        {
            if (item == null || string.IsNullOrWhiteSpace(propertyPath))
                return null;

            object? current = item;
            foreach (var segment in propertyPath.Split('.'))
            {
                if (current == null)
                    return null;

                var property = current.GetType().GetProperty(segment);
                if (property == null)
                    return null;

                current = property.GetValue(current);
            }

            return current;
        }

        private static string FormatValue(object? value)
        {
            if (value == null)
                return string.Empty;

            return value switch
            {
                DateTime dateTime => NormalizeExcelText(dateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)),
                TimeSpan timeSpan => NormalizeExcelText(timeSpan.ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture)),
                _ => NormalizeExcelText(Convert.ToString(value, CultureInfo.CurrentCulture) ?? string.Empty)
            };
        }

        private static string SanitizeFileName(string? fileName)
        {
            var safeFileName = string.IsNullOrWhiteSpace(fileName) ? "grid" : fileName;

            foreach (var invalidChar in Path.GetInvalidFileNameChars())
                safeFileName = safeFileName.Replace(invalidChar, '_');

            return string.IsNullOrWhiteSpace(safeFileName) ? "grid" : safeFileName;
        }

        private static void WriteExcelFile(string filePath, IReadOnlyList<DataGridColumn> columns, IReadOnlyList<object> rows)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);

            using var archive = ZipFile.Open(filePath, ZipArchiveMode.Create);

            AddZipEntry(
                archive,
                "[Content_Types].xml",
                """
                <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
                <Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types">
                  <Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml"/>
                  <Default Extension="xml" ContentType="application/xml"/>
                  <Override PartName="/xl/workbook.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml"/>
                  <Override PartName="/xl/worksheets/sheet1.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml"/>
                  <Override PartName="/xl/styles.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml"/>
                  <Override PartName="/docProps/core.xml" ContentType="application/vnd.openxmlformats-package.core-properties+xml"/>
                  <Override PartName="/docProps/app.xml" ContentType="application/vnd.openxmlformats-officedocument.extended-properties+xml"/>
                </Types>
                """);

            AddZipEntry(
                archive,
                "_rels/.rels",
                """
                <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
                <Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
                  <Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="xl/workbook.xml"/>
                  <Relationship Id="rId2" Type="http://schemas.openxmlformats.org/package/2006/relationships/metadata/core-properties" Target="docProps/core.xml"/>
                  <Relationship Id="rId3" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/extended-properties" Target="docProps/app.xml"/>
                </Relationships>
                """);

            AddZipEntry(
                archive,
                "xl/_rels/workbook.xml.rels",
                """
                <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
                <Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
                  <Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet" Target="worksheets/sheet1.xml"/>
                  <Relationship Id="rId2" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles" Target="styles.xml"/>
                </Relationships>
                """);

            AddZipEntry(
                archive,
                "xl/workbook.xml",
                """
                <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
                <workbook xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships">
                  <sheets>
                    <sheet name="Datos" sheetId="1" r:id="rId1"/>
                  </sheets>
                </workbook>
                """);

            AddZipEntry(archive, "xl/styles.xml", BuildExcelStylesXml());
            AddZipEntry(archive, "xl/worksheets/sheet1.xml", BuildWorksheetXml(columns, rows));

            var timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
            AddZipEntry(
                archive,
                "docProps/core.xml",
                $"""
                <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
                <cp:coreProperties xmlns:cp="http://schemas.openxmlformats.org/package/2006/metadata/core-properties" xmlns:dc="http://purl.org/dc/elements/1.1/" xmlns:dcterms="http://purl.org/dc/terms/" xmlns:dcmitype="http://purl.org/dc/dcmitype/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
                  <dc:creator>LD</dc:creator>
                  <cp:lastModifiedBy>LD</cp:lastModifiedBy>
                  <dcterms:created xsi:type="dcterms:W3CDTF">{timestamp}</dcterms:created>
                  <dcterms:modified xsi:type="dcterms:W3CDTF">{timestamp}</dcterms:modified>
                </cp:coreProperties>
                """);

            AddZipEntry(
                archive,
                "docProps/app.xml",
                """
                <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
                <Properties xmlns="http://schemas.openxmlformats.org/officeDocument/2006/extended-properties" xmlns:vt="http://schemas.openxmlformats.org/officeDocument/2006/docPropsVTypes">
                  <Application>LD</Application>
                </Properties>
                """);
        }

        private static string BuildWorksheetXml(IReadOnlyList<DataGridColumn> columns, IReadOnlyList<object> rows)
        {
            var sheetRows = new StringBuilder();

            sheetRows.Append(Row(
                1,
                columns
                    .Select((column, index) => TextCell($"{GetColumnName(index + 1)}1", GetColumnHeaderText(column), 1))
                    .ToArray()));

            for (var rowIndex = 0; rowIndex < rows.Count; rowIndex++)
            {
                var excelRow = rowIndex + 2;
                var rowStyle = excelRow % 2 == 0 ? 2 : 0;
                var cells = columns
                    .Select((column, columnIndex) => TextCell(
                        $"{GetColumnName(columnIndex + 1)}{excelRow}",
                        GetColumnValue(column, rows[rowIndex]),
                        rowStyle))
                    .ToArray();

                sheetRows.Append(Row(excelRow, cells));
            }

            return $"""
                <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
                <worksheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships">
                  <sheetViews>
                    <sheetView workbookViewId="0"/>
                  </sheetViews>
                  <sheetFormatPr defaultRowHeight="18"/>
                  <cols>
                {BuildColumnWidths(columns)}
                  </cols>
                  <sheetData>
                {sheetRows}
                  </sheetData>
                </worksheet>
                """;
        }

        private static string BuildColumnWidths(IReadOnlyList<DataGridColumn> columns)
        {
            var builder = new StringBuilder();

            for (var i = 0; i < columns.Count; i++)
            {
                var width = Math.Clamp(columns[i].ActualWidth / 7.0, 10.0, 60.0);
                builder.AppendLine(CultureInfo.InvariantCulture, $"    <col min=\"{i + 1}\" max=\"{i + 1}\" width=\"{width:0.##}\" customWidth=\"1\"/>");
            }

            return builder.ToString();
        }

        private static string BuildExcelStylesXml() =>
            """
            <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
            <styleSheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main">
              <fonts count="2">
                <font><sz val="11"/><name val="Calibri"/></font>
                <font><b/><color rgb="FFFFFFFF"/><sz val="11"/><name val="Calibri"/></font>
              </fonts>
              <fills count="4">
                <fill><patternFill patternType="none"/></fill>
                <fill><patternFill patternType="gray125"/></fill>
                <fill><patternFill patternType="solid"><fgColor rgb="FF1F4E78"/><bgColor indexed="64"/></patternFill></fill>
                <fill><patternFill patternType="solid"><fgColor rgb="FFEAF2F8"/><bgColor indexed="64"/></patternFill></fill>
              </fills>
              <borders count="2">
                <border><left/><right/><top/><bottom/><diagonal/></border>
                <border><left style="thin"><color rgb="FFD9D9D9"/></left><right style="thin"><color rgb="FFD9D9D9"/></right><top style="thin"><color rgb="FFD9D9D9"/></top><bottom style="thin"><color rgb="FFD9D9D9"/></bottom><diagonal/></border>
              </borders>
              <cellStyleXfs count="1"><xf numFmtId="0" fontId="0" fillId="0" borderId="0"/></cellStyleXfs>
              <cellXfs count="3">
                <xf numFmtId="0" fontId="0" fillId="0" borderId="1" xfId="0"/>
                <xf numFmtId="0" fontId="1" fillId="2" borderId="1" xfId="0" applyFont="1" applyFill="1"/>
                <xf numFmtId="0" fontId="0" fillId="3" borderId="1" xfId="0" applyFill="1"/>
              </cellXfs>
              <cellStyles count="1"><cellStyle name="Normal" xfId="0" builtinId="0"/></cellStyles>
              <dxfs count="0"/>
              <tableStyles count="0" defaultTableStyle="TableStyleMedium2" defaultPivotStyle="PivotStyleLight16"/>
            </styleSheet>
            """;

        private static void AddZipEntry(ZipArchive archive, string entryName, string content)
        {
            var entry = archive.CreateEntry(entryName);
            using var stream = entry.Open();
            using var writer = new StreamWriter(stream, new UTF8Encoding(false));
            writer.Write(content);
        }

        private static string Row(int index, params string[] cells) =>
            $"    <row r=\"{index}\">{string.Concat(cells)}</row>{Environment.NewLine}";

        private static string TextCell(string reference, string? value, int style = 0)
        {
            var escaped = SecurityElement.Escape(value ?? string.Empty) ?? string.Empty;
            return $"<c r=\"{reference}\" s=\"{style}\" t=\"inlineStr\"><is><t>{escaped}</t></is></c>";
        }

        private static string GetColumnName(int columnNumber)
        {
            var dividend = columnNumber;
            var columnName = string.Empty;

            while (dividend > 0)
            {
                var modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar('A' + modulo) + columnName;
                dividend = (dividend - modulo) / 26;
            }

            return columnName;
        }

        private static string NormalizeExcelText(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            if (!LooksLikeMojibake(value))
                return value;

            try
            {
                var bytes = Encoding.GetEncoding(1252).GetBytes(value);
                var repaired = Encoding.UTF8.GetString(bytes);
                return string.IsNullOrWhiteSpace(repaired) ? value : repaired;
            }
            catch
            {
                return value;
            }
        }

        private static bool LooksLikeMojibake(string value)
        {
            return value.Contains('Ã')
                || value.Contains('Â')
                || value.Contains('Ð')
                || value.Contains('Ñ');
        }
    }
}
