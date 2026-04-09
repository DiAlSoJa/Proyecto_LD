using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;

namespace LD.FormsX.Features.Common
{
    public partial class FilterableLookupComboBox : UserControl, INotifyPropertyChanged
    {
        private TextBox? _editableTextBox;
        private bool _suppressTextChanged;
        private bool _loadedOnce;
        public ObservableCollection<object> FilteredItems { get; } = new();

        public event PropertyChangedEventHandler? PropertyChanged;
        public event Action<object?>? SelectionConfirmed;

        public FilterableLookupComboBox()
        {
            InitializeComponent();
            DataContext = this;
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                nameof(ItemsSource),
                typeof(IEnumerable),
                typeof(FilterableLookupComboBox),
                new PropertyMetadata(null, OnItemsSourceChanged));

        public IEnumerable? ItemsSource
        {
            get => (IEnumerable?)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(
                nameof(SelectedItem),
                typeof(object),
                typeof(FilterableLookupComboBox),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnSelectedItemChanged));

        public object? SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public static readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register(
                nameof(DisplayMemberPath),
                typeof(string),
                typeof(FilterableLookupComboBox),
                new PropertyMetadata(string.Empty));

        public string DisplayMemberPath
        {
            get => (string)GetValue(DisplayMemberPathProperty);
            set => SetValue(DisplayMemberPathProperty, value);
        }

        public static readonly DependencyProperty SearchMemberPathsProperty =
            DependencyProperty.Register(
                nameof(SearchMemberPaths),
                typeof(string),
                typeof(FilterableLookupComboBox),
                new PropertyMetadata(string.Empty));

        public string SearchMemberPaths
        {
            get => (string)GetValue(SearchMemberPathsProperty);
            set => SetValue(SearchMemberPathsProperty, value);
        }

        public static readonly DependencyProperty SecondaryMemberPathProperty =
            DependencyProperty.Register(
                nameof(SecondaryMemberPath),
                typeof(string),
                typeof(FilterableLookupComboBox),
                new PropertyMetadata(string.Empty, OnLookupTemplatePropertyChanged));

        public string SecondaryMemberPath
        {
            get => (string)GetValue(SecondaryMemberPathProperty);
            set => SetValue(SecondaryMemberPathProperty, value);
        }

        public static readonly DependencyProperty PrimaryColumnWidthProperty =
            DependencyProperty.Register(
                nameof(PrimaryColumnWidth),
                typeof(double),
                typeof(FilterableLookupComboBox),
                new PropertyMetadata(140d, OnLookupTemplatePropertyChanged));

        public double PrimaryColumnWidth
        {
            get => (double)GetValue(PrimaryColumnWidthProperty);
            set => SetValue(PrimaryColumnWidthProperty, value);
        }

        public static readonly DependencyProperty SecondaryColumnWidthProperty =
            DependencyProperty.Register(
                nameof(SecondaryColumnWidth),
                typeof(double),
                typeof(FilterableLookupComboBox),
                new PropertyMetadata(220d, OnLookupTemplatePropertyChanged));

        public double SecondaryColumnWidth
        {
            get => (double)GetValue(SecondaryColumnWidthProperty);
            set => SetValue(SecondaryColumnWidthProperty, value);
        }

        public static readonly DependencyProperty MaxResultsProperty =
            DependencyProperty.Register(
                nameof(MaxResults),
                typeof(int),
                typeof(FilterableLookupComboBox),
                new PropertyMetadata(30));

        public int MaxResults
        {
            get => (int)GetValue(MaxResultsProperty);
            set => SetValue(MaxResultsProperty, value);
        }

        public static readonly DependencyProperty InitialTextProperty =
            DependencyProperty.Register(
                nameof(InitialText),
                typeof(string),
                typeof(FilterableLookupComboBox),
                new PropertyMetadata(string.Empty, OnInitialTextChanged));

        public string InitialText
        {
            get => (string)GetValue(InitialTextProperty);
            set => SetValue(InitialTextProperty, value);
        }

        public static readonly DependencyProperty OpenDropDownOnLoadProperty =
            DependencyProperty.Register(
                nameof(OpenDropDownOnLoad),
                typeof(bool),
                typeof(FilterableLookupComboBox),
                new PropertyMetadata(true));

        public bool OpenDropDownOnLoad
        {
            get => (bool)GetValue(OpenDropDownOnLoadProperty);
            set => SetValue(OpenDropDownOnLoadProperty, value);
        }

        public static readonly DependencyProperty SelectAllTextOnLoadProperty =
            DependencyProperty.Register(
                nameof(SelectAllTextOnLoad),
                typeof(bool),
                typeof(FilterableLookupComboBox),
                new PropertyMetadata(true));

        public bool SelectAllTextOnLoad
        {
            get => (bool)GetValue(SelectAllTextOnLoadProperty);
            set => SetValue(SelectAllTextOnLoadProperty, value);
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FilterableLookupComboBox control)
                control.ApplyFilter(control._editableTextBox?.Text ?? string.Empty);
        }

        private static void OnLookupTemplatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FilterableLookupComboBox control)
                control.ApplyItemTemplate();
        }

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not FilterableLookupComboBox control)
                return;

            if (control._editableTextBox == null)
                return;

            if (e.NewValue == null)
                return;

            var text = control.GetDisplayValue(e.NewValue);

            control._suppressTextChanged = true;
            control._editableTextBox.Text = text;

            if (control.SelectAllTextOnLoad)
                control._editableTextBox.SelectAll();

            control._suppressTextChanged = false;
        }

        private static void OnInitialTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not FilterableLookupComboBox control)
                return;

            if (control._editableTextBox == null || control.SelectedItem != null)
                return;

            control._suppressTextChanged = true;
            control._editableTextBox.Text = e.NewValue?.ToString() ?? string.Empty;

            if (control.SelectAllTextOnLoad)
                control._editableTextBox.SelectAll();

            control._suppressTextChanged = false;
            control.ApplyFilter(control._editableTextBox.Text);
        }

        private void cmbLookup_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                cmbLookup.ApplyTemplate();

                _editableTextBox = cmbLookup.Template.FindName("PART_EditableTextBox", cmbLookup) as TextBox;
                if (_editableTextBox != null)
                {
                    _editableTextBox.TextChanged -= EditableTextBox_TextChanged;
                    _editableTextBox.TextChanged += EditableTextBox_TextChanged;
                    _editableTextBox.PreviewKeyDown -= EditableTextBox_PreviewKeyDown;
                    _editableTextBox.PreviewKeyDown += EditableTextBox_PreviewKeyDown;

                    _editableTextBox.Focus();

                    if (SelectAllTextOnLoad)
                        _editableTextBox.SelectAll();
                }

                ApplyItemTemplate();
                ApplyInitialText();
                ApplyFilter(_editableTextBox?.Text ?? string.Empty);

                if (OpenDropDownOnLoad)
                    cmbLookup.IsDropDownOpen = true;

                _loadedOnce = true;
            }), DispatcherPriority.Background);
        }

        private void ApplyInitialText()
        {
            if (_editableTextBox == null)
                return;

            if (SelectedItem != null)
            {
                _suppressTextChanged = true;
                _editableTextBox.Text = GetDisplayValue(SelectedItem);

                if (SelectAllTextOnLoad)
                    _editableTextBox.SelectAll();

                _suppressTextChanged = false;
                return;
            }

            if (!string.IsNullOrWhiteSpace(InitialText))
            {
                _suppressTextChanged = true;
                _editableTextBox.Text = InitialText;

                if (SelectAllTextOnLoad)
                    _editableTextBox.SelectAll();

                _suppressTextChanged = false;
            }
        }

        private void EditableTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_suppressTextChanged)
                return;

            ApplyFilter(_editableTextBox?.Text ?? string.Empty);

            if (!cmbLookup.IsDropDownOpen)
                cmbLookup.IsDropDownOpen = true;
        }

        private void ApplyFilter(string text)
        {
            FilteredItems.Clear();

            var allItems = ItemsSource?.Cast<object>().ToList() ?? new List<object>();
            IEnumerable<object> filtered;

            if (string.IsNullOrWhiteSpace(text))
            {
                filtered = allItems.Take(MaxResults);
            }
            else
            {
                filtered = allItems
                    .Where(x => MatchesSearch(x, text))
                    .Take(MaxResults);
            }

            foreach (var item in filtered)
                FilteredItems.Add(item);

            cmbLookup.SelectedItem = FilteredItems.Count > 0 ? FilteredItems[0] : null;

            OnPropertyChanged(nameof(FilteredItems));
        }

        private void EditableTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ConfirmCurrentSelection();
                e.Handled = true;
                return;
            }

            if (e.Key == Key.Down)
            {
                if (!cmbLookup.IsDropDownOpen)
                    cmbLookup.IsDropDownOpen = true;

                if (cmbLookup.SelectedItem == null && FilteredItems.Count > 0)
                    cmbLookup.SelectedItem = FilteredItems[0];

                e.Handled = true;
                return;
            }
        }

        private void ConfirmCurrentSelection()
        {
            if (cmbLookup.SelectedItem == null && FilteredItems.Count > 0)
                cmbLookup.SelectedItem = FilteredItems[0];

            if (cmbLookup.SelectedItem == null)
                return;

            SelectionConfirmed?.Invoke(cmbLookup.SelectedItem);
        }

        private void ApplyItemTemplate()
        {
            cmbLookup.DisplayMemberPath = string.Empty;

            if (string.IsNullOrWhiteSpace(DisplayMemberPath))
            {
                cmbLookup.ItemTemplate = null;
                return;
            }

            if (string.IsNullOrWhiteSpace(SecondaryMemberPath))
            {
                cmbLookup.ItemTemplate = null;
                cmbLookup.DisplayMemberPath = DisplayMemberPath;
                return;
            }

            var templateXaml =
                "<DataTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>" +
                "<Grid Margin='2,0'>" +
                "<Grid.ColumnDefinitions>" +
                $"<ColumnDefinition Width='{PrimaryColumnWidth}'/>" +
                "<ColumnDefinition Width='12'/>" +
                $"<ColumnDefinition Width='{SecondaryColumnWidth}'/>" +
                "</Grid.ColumnDefinitions>" +
                $"<TextBlock Grid.Column='0' Text='{{Binding {DisplayMemberPath}}}' VerticalAlignment='Center' TextTrimming='CharacterEllipsis'/>" +
                $"<TextBlock Grid.Column='2' Text='{{Binding {SecondaryMemberPath}}}' VerticalAlignment='Center' Foreground='#5B6574' TextTrimming='CharacterEllipsis'/>" +
                "</Grid>" +
                "</DataTemplate>";

            cmbLookup.ItemTemplate = (DataTemplate)XamlReader.Parse(templateXaml);
        }

        private bool MatchesSearch(object item, string searchText)
        {
            var paths = (SearchMemberPaths ?? string.Empty)
                .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            if (!paths.Any())
            {
                var display = GetDisplayValue(item);
                return display.Contains(searchText, StringComparison.OrdinalIgnoreCase);
            }

            foreach (var path in paths)
            {
                var value = GetPropertyValue(item, path)?.ToString();
                if (!string.IsNullOrWhiteSpace(value) &&
                    value.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        private string GetDisplayValue(object item)
        {
            if (item == null)
                return string.Empty;

            if (string.IsNullOrWhiteSpace(DisplayMemberPath))
                return item.ToString() ?? string.Empty;

            return GetPropertyValue(item, DisplayMemberPath)?.ToString() ?? string.Empty;
        }

        private object? GetPropertyValue(object item, string propertyName)
        {
            return item.GetType()
                .GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
                ?.GetValue(item);
        }

        private void cmbLookup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_loadedOnce)
                return;
        }

        private void cmbLookup_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ConfirmCurrentSelection();
                e.Handled = true;
                return;
            }

            if (e.Key == Key.Down)
            {
                if (!cmbLookup.IsDropDownOpen)
                    cmbLookup.IsDropDownOpen = true;

                if (cmbLookup.SelectedItem == null && FilteredItems.Count > 0)
                    cmbLookup.SelectedItem = FilteredItems[0];

                e.Handled = true;
                return;
            }

            if (e.Key == Key.Escape)
            {
                cmbLookup.IsDropDownOpen = false;
            }
        }

        private void cmbLookup_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
