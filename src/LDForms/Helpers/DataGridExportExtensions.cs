using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media;

namespace LD.FormsX.Helpers
{
    public static class DataGridExportExtensions
    {
        public static string ExportVisibleRowsToCsv(this DataGrid dataGrid, string? filePrefix = null)
        {
            var separator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
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

            var builder = new StringBuilder();
            builder.AppendLine($"sep={separator}");
            builder.AppendLine(string.Join(separator, visibleColumns.Select(GetColumnHeaderText).Select(EscapeCsv)));

            foreach (var row in rows)
            {
                var values = visibleColumns.Select(column => EscapeCsv(GetColumnValue(column, row)));
                builder.AppendLine(string.Join(separator, values));
            }

            var exportFolder = Path.Combine(Path.GetTempPath(), "LD", "Exports");
            Directory.CreateDirectory(exportFolder);

            var safePrefix = string.IsNullOrWhiteSpace(filePrefix) ? dataGrid.Name : filePrefix;
            if (string.IsNullOrWhiteSpace(safePrefix))
                safePrefix = "grid";

            foreach (var invalidChar in Path.GetInvalidFileNameChars())
                safePrefix = safePrefix.Replace(invalidChar, '_');

            var filePath = Path.Combine(exportFolder, $"{safePrefix}_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
            File.WriteAllText(filePath, builder.ToString(), Encoding.Unicode);

            Process.Start(new ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true
            });

            return filePath;
        }

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

        private static string EscapeCsv(string? value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            if (ShouldForceExcelText(value))
                return $"=\"{value.Replace("\"", "\"\"")}\"";

            var escaped = value.Replace("\"", "\"\"");
            return $"\"{escaped}\"";
        }

        private static bool ShouldForceExcelText(string value)
        {
            var trimmed = value.Trim();

            if (trimmed.Length < 12)
                return false;

            return Regex.IsMatch(trimmed, @"^\d+$");
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
