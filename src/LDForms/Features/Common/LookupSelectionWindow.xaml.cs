using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using LD.FormsX.Model.Lookup;

namespace LD.FormsX.Features.Common
{
    public partial class LookupSelectionWindow : Window
    {
        private const int MaxVisibleItems = 5;

        private readonly List<LookupItem> _allItems = new();
        private List<PropertyInfo> _visibleProperties = new();

        private readonly HashSet<string> _hiddenColumns;
        private readonly HashSet<string> _searchColumns;

        public LookupItem? SelectedLookupItem { get; private set; }

        public LookupSelectionWindow(
            IEnumerable<LookupItem> items,
            IEnumerable<string>? hiddenColumns = null,
            IEnumerable<string>? searchColumns = null,
            double? x = null,
            double? y = null)
        {
            InitializeComponent();

            _allItems = items?.ToList() ?? new List<LookupItem>();

            _hiddenColumns = new HashSet<string>(
                (hiddenColumns ?? Enumerable.Empty<string>())
                .Concat(GetDefaultHiddenColumns()),
                StringComparer.OrdinalIgnoreCase);

            _searchColumns = new HashSet<string>(
                searchColumns ?? Enumerable.Empty<string>(),
                StringComparer.OrdinalIgnoreCase);

            if (x.HasValue && y.HasValue)
            {
                WindowStartupLocation = WindowStartupLocation.Manual;
                Left = x.Value;
                Top = y.Value;
            }

            dgProductos.AlternationCount = 2;

            BuildColumns();
            LoadGrid(_allItems);

            Loaded += LookupSelectionWindow_Loaded;
        }

        private void LookupSelectionWindow_Loaded(object sender, RoutedEventArgs e)
        {
            txtBuscar.Focus();
            txtBuscar.SelectAll();

            if (dgProductos.Items.Count > 0)
                dgProductos.SelectedIndex = 0;
        }

        private IEnumerable<string> GetDefaultHiddenColumns()
        {
            return new[]
            {
                "Activo",
                "IsActive",
                "Deleted",
                "IsDeleted",
                "RowVersion",
                "CreatedAt",
                "CreatedDate",
                "UpdatedAt",
                "UpdatedDate",
                "Password",
                "PasswordHash",
                "PasswordSalt"
            };
        }

        private void BuildColumns()
        {
            dgProductos.Columns.Clear();
            _visibleProperties.Clear();

            var sample = _allItems
                .Select(x => x.Data)
                .FirstOrDefault(x => x != null);

            if (sample == null)
            {
                dgProductos.Columns.Add(new DataGridTextColumn
                {
                    Header = "Código",
                    Binding = new Binding(nameof(LookupItem.Code)),
                    Width = 160
                });

                dgProductos.Columns.Add(new DataGridTextColumn
                {
                    Header = "Descripción",
                    Binding = new Binding(nameof(LookupItem.Description)),
                    Width = new DataGridLength(1, DataGridLengthUnitType.Star)
                });

                return;
            }

            var type = sample.GetType();

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(IsSimpleProperty)
                .Where(p => !_hiddenColumns.Contains(p.Name))
                .ToList();

            _visibleProperties = properties;

            foreach (var prop in properties)
            {
                dgProductos.Columns.Add(new DataGridTextColumn
                {
                    Header = GetDisplayName(prop),
                    Binding = new Binding($"Data.{prop.Name}")
                    {
                        TargetNullValue = string.Empty,
                        StringFormat = GetStringFormat(prop.PropertyType)
                    },
                    Width = GetColumnWidth(prop)
                });
            }
        }

        private static bool IsSimpleProperty(PropertyInfo prop)
        {
            var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

            if (type.IsPrimitive || type.IsEnum)
                return true;

            return type == typeof(string)
                || type == typeof(decimal)
                || type == typeof(DateTime)
                || type == typeof(DateTimeOffset)
                || type == typeof(TimeSpan)
                || type == typeof(Guid);
        }

        private static string GetDisplayName(PropertyInfo prop)
        {
            return prop.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? prop.Name;
        }

        private static string? GetStringFormat(Type propertyType)
        {
            var type = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

            if (type == typeof(DateTime))
                return "dd/MM/yyyy";

            return null;
        }

        private static DataGridLength GetColumnWidth(PropertyInfo prop)
        {
            var name = prop.Name.ToLowerInvariant();

            if (name == "id" || name.EndsWith("id"))
                return new DataGridLength(90);

            if (name.Contains("numero") || name.Contains("part") || name.Contains("code"))
                return new DataGridLength(170);

            if (name.Contains("descripcion") || name.Contains("description") || name.Contains("nombre"))
                return new DataGridLength(270);

            return new DataGridLength(140);
        }

        private void LoadGrid(IEnumerable<LookupItem> items)
        {
            var list = items
                .Take(MaxVisibleItems)
                .ToList();

            dgProductos.ItemsSource = null;
            dgProductos.ItemsSource = list;

            if (list.Count > 0)
                dgProductos.SelectedIndex = 0;
        }

        private void ApplyFilter()
        {
            var text = txtBuscar.Text?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(text))
            {
                LoadGrid(_allItems);
                return;
            }

            var filtered = _allItems
                .Where(x => MatchesSearch(x, text));

            LoadGrid(filtered);
        }

        private bool MatchesSearch(LookupItem item, string searchText)
        {
            if (!string.IsNullOrWhiteSpace(item.Code) &&
                item.Code.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!string.IsNullOrWhiteSpace(item.Description) &&
                item.Description.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                return true;

            if (item.Data == null)
                return false;

            IEnumerable<PropertyInfo> propsToSearch = _visibleProperties;

            if (_searchColumns.Count > 0)
                propsToSearch = _visibleProperties.Where(p => _searchColumns.Contains(p.Name));

            foreach (var prop in propsToSearch)
            {
                var value = prop.GetValue(item.Data);
                if (value?.ToString()?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true)
                    return true;
            }

            return false;
        }

        private void ConfirmSelection()
        {
            if (dgProductos.SelectedItem is not LookupItem selected)
                return;

            SelectedLookupItem = selected;
            DialogResult = true;
            Close();
        }

        private void txtBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilter();
        }

        private void dgProductos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ConfirmSelection();
        }

        private void txtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                dgProductos.Focus();

                if (dgProductos.Items.Count > 0 && dgProductos.SelectedIndex < 0)
                    dgProductos.SelectedIndex = 0;

                e.Handled = true;
                return;
            }

            if (e.Key == Key.Enter)
            {
                ConfirmSelection();
                e.Handled = true;
                return;
            }

            if (e.Key == Key.Escape)
            {
                DialogResult = false;
                Close();
                e.Handled = true;
            }
        }

        private void dgProductos_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ConfirmSelection();
                e.Handled = true;
                return;
            }

            if (e.Key == Key.Escape)
            {
                DialogResult = false;
                Close();
                e.Handled = true;
                return;
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                DialogResult = false;
                Close();
                e.Handled = true;
            }
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
                return;

            DragMove();
        }
    }
}