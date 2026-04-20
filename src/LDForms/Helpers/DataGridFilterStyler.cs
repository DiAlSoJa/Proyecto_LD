using System.Windows.Controls;

namespace LD.FormsX.Helpers
{
    public static class DataGridFilterStyler
    {
        public static void Apply(DataGrid dataGrid)
        {
            dataGrid.RowHeight = 28;
            dataGrid.ColumnHeaderHeight = 72;
        }
    }
}
