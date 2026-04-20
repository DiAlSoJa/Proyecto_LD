using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace LD.Forms.Classes
{
    public static class DataGridViewExtensions
    {
        public static void ApplyColumnHeadersFromDisplayName<T>(this DataGridView grid)
        {
            foreach (DataGridViewColumn column in grid.Columns)
            {
                PropertyInfo prop = typeof(T).GetProperty(column.DataPropertyName ?? column.Name);
                if (prop == null) continue;

                var displayNameAttr = prop.GetCustomAttribute<DisplayNameAttribute>();
                if (displayNameAttr != null)
                {
                    column.HeaderText = displayNameAttr.DisplayName;
                }
            }
        }

        public static string ExportVisibleRowsToCsv(this DataGridView grid)
        {
            if (grid.Rows.Count == 0 || grid.Rows.Cast<DataGridViewRow>().All(r => r.IsNewRow))
            {
                throw new InvalidOperationException("No hay datos para exportar.");
            }

            var visibleColumns = grid.Columns
                .Cast<DataGridViewColumn>()
                .Where(c => c.Visible)
                .OrderBy(c => c.DisplayIndex)
                .ToList();

            if (visibleColumns.Count == 0)
            {
                throw new InvalidOperationException("No hay columnas visibles para exportar.");
            }

            var builder = new StringBuilder();
            builder.AppendLine(string.Join(",", visibleColumns.Select(c => EscapeCsv(c.HeaderText))));

            foreach (var row in grid.Rows.Cast<DataGridViewRow>().Where(r => !r.IsNewRow && r.Visible))
            {
                var values = visibleColumns.Select(column =>
                {
                    var value = row.Cells[column.Index].FormattedValue;
                    return EscapeCsv(FormatCellValue(value));
                });

                builder.AppendLine(string.Join(",", values));
            }

            var exportFolder = Path.Combine(Path.GetTempPath(), "LD", "Exports");
            Directory.CreateDirectory(exportFolder);

            var safeGridName = string.IsNullOrWhiteSpace(grid.Name) ? "grid" : grid.Name;
            foreach (var invalidChar in Path.GetInvalidFileNameChars())
            {
                safeGridName = safeGridName.Replace(invalidChar, '_');
            }

            var filePath = Path.Combine(
                exportFolder,
                $"{safeGridName}_{DateTime.Now:yyyyMMdd_HHmmss}.csv");

            File.WriteAllText(filePath, builder.ToString(), new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));

            Process.Start(new ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true
            });

            return filePath;
        }

        private static string FormatCellValue(object? value)
        {
            if (value is null)
            {
                return string.Empty;
            }

            if (value is DateTime dateTime)
            {
                return dateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            }

            return Convert.ToString(value, CultureInfo.CurrentCulture) ?? string.Empty;
        }

        private static string EscapeCsv(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            var escaped = value.Replace("\"", "\"\"");
            return $"\"{escaped}\"";
        }
    }
}
