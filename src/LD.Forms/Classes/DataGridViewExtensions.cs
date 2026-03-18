using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    }
}