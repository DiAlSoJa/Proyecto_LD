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
using System.Windows.Threading;
using LD.Contracts.Requests;
using LD.FormsX.Model.Lookup;

namespace LD.FormsX.Features.Common
{
    public partial class InlineLookupEditor : UserControl, INotifyPropertyChanged, IDataGridEditingControl
    {
        private bool _isUpdatingText;
        private bool _loadedOnce;

        public ObservableCollection<object> FilteredItems { get; } = new();
        public object? SelectedLookupItem { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public static readonly RoutedEvent SelectionConfirmedEvent =
            EventManager.RegisterRoutedEvent(
                nameof(SelectionConfirmed),
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(InlineLookupEditor));

        public event RoutedEventHandler SelectionConfirmed
        {
            add => AddHandler(SelectionConfirmedEvent, value);
            remove => RemoveHandler(SelectionConfirmedEvent, value);
        }

        public InlineLookupEditor()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                nameof(ItemsSource),
                typeof(IEnumerable),
                typeof(InlineLookupEditor),
                new PropertyMetadata(null, OnItemsSourceChanged));

        public IEnumerable? ItemsSource
        {
            get => (IEnumerable?)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register(
                nameof(DisplayMemberPath),
                typeof(string),
                typeof(InlineLookupEditor),
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
                typeof(InlineLookupEditor),
                new PropertyMetadata(string.Empty));

        public string SearchMemberPaths
        {
            get => (string)GetValue(SearchMemberPathsProperty);
            set => SetValue(SearchMemberPathsProperty, value);
        }

        public static readonly DependencyProperty InitialTextProperty =
            DependencyProperty.Register(
                nameof(InitialText),
                typeof(string),
                typeof(InlineLookupEditor),
                new PropertyMetadata(string.Empty));

        public string InitialText
        {
            get => (string)GetValue(InitialTextProperty);
            set => SetValue(InitialTextProperty, value);
        }

        public static readonly DependencyProperty SelectedCodeProperty =
            DependencyProperty.Register(
                nameof(SelectedCode),
                typeof(string),
                typeof(InlineLookupEditor),
                new FrameworkPropertyMetadata(
                    string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnSelectedCodeChanged));

        public string SelectedCode
        {
            get => (string)GetValue(SelectedCodeProperty);
            set => SetValue(SelectedCodeProperty, value);
        }

        private static void OnSelectedCodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not InlineLookupEditor control || !control._loadedOnce)
                return;

            var newValue = e.NewValue?.ToString() ?? string.Empty;
            if (string.Equals(control.txtLookup.Text, newValue, StringComparison.Ordinal))
                return;

            control.SetEditorText(newValue, false);
            control.ApplyFilter(newValue);
        }

        public static readonly DependencyProperty SelectedDescriptionProperty =
            DependencyProperty.Register(
                nameof(SelectedDescription),
                typeof(string),
                typeof(InlineLookupEditor),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string SelectedDescription
        {
            get => (string)GetValue(SelectedDescriptionProperty);
            set => SetValue(SelectedDescriptionProperty, value);
        }

        public static readonly DependencyProperty SelectedIdProperty =
            DependencyProperty.Register(
                nameof(SelectedId),
                typeof(object),
                typeof(InlineLookupEditor),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public object? SelectedId
        {
            get => GetValue(SelectedIdProperty);
            set => SetValue(SelectedIdProperty, value);
        }

        public static readonly DependencyProperty RequireSelectionMatchProperty =
            DependencyProperty.Register(
                nameof(RequireSelectionMatch),
                typeof(bool),
                typeof(InlineLookupEditor),
                new PropertyMetadata(false));

        public bool RequireSelectionMatch
        {
            get => (bool)GetValue(RequireSelectionMatchProperty);
            set => SetValue(RequireSelectionMatchProperty, value);
        }

        public static readonly DependencyProperty MaxResultsProperty =
            DependencyProperty.Register(
                nameof(MaxResults),
                typeof(int),
                typeof(InlineLookupEditor),
                new PropertyMetadata(30));

        public int MaxResults
        {
            get => (int)GetValue(MaxResultsProperty);
            set => SetValue(MaxResultsProperty, value);
        }

        public static readonly DependencyProperty OpenDropDownOnLoadProperty =
            DependencyProperty.Register(
                nameof(OpenDropDownOnLoad),
                typeof(bool),
                typeof(InlineLookupEditor),
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
                typeof(InlineLookupEditor),
                new PropertyMetadata(true));

        public bool SelectAllTextOnLoad
        {
            get => (bool)GetValue(SelectAllTextOnLoadProperty);
            set => SetValue(SelectAllTextOnLoadProperty, value);
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is InlineLookupEditor control)
                control.ApplyFilter(control.txtLookup.Text);
        }

        private void txtLookup_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                var initialValue = !string.IsNullOrWhiteSpace(SelectedCode) ? SelectedCode : InitialText;
                SetEditorText(initialValue ?? string.Empty, SelectAllTextOnLoad);
                ApplyHeadersFromTag();
                ApplyFilter(txtLookup.Text);

                if (OpenDropDownOnLoad)
                {
                    popupLookup.IsOpen = true;
                    txtLookup.Focus();

                    if (SelectAllTextOnLoad)
                        txtLookup.SelectAll();
                }

                _loadedOnce = true;
            }), DispatcherPriority.Background);
        }

        private void txtLookup_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isUpdatingText)
                return;

            ApplyFilter(txtLookup.Text);

            if (!popupLookup.IsOpen)
                popupLookup.IsOpen = true;
        }

        private void txtLookup_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            ApplyFilter(txtLookup.Text);
            popupLookup.IsOpen = true;
        }

        private void txtLookup_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                MoveSelection(1);
                popupLookup.IsOpen = true;
                e.Handled = true;
                return;
            }

            if (e.Key == Key.Up)
            {
                MoveSelection(-1);
                e.Handled = true;
                return;
            }

            if (e.Key == Key.Enter)
            {
                ConfirmCurrentSelection();
                e.Handled = true;
                return;
            }

            if (e.Key == Key.Escape)
            {
                popupLookup.IsOpen = false;
                e.Handled = true;
            }
        }

        private void lstLookup_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ConfirmCurrentSelection();
                e.Handled = true;
                return;
            }
        }

        private void lstLookup_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var element = e.OriginalSource as DependencyObject;
            var itemContainer = ItemsControl.ContainerFromElement(lstLookup, element) as ListBoxItem;
            if (itemContainer?.DataContext == null)
                return;

            lstLookup.SelectedItem = itemContainer.DataContext;
            ConfirmCurrentSelection();
            e.Handled = true;
        }

        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnLostKeyboardFocus(e);

            if (!IsKeyboardFocusWithin)
            {
                if (RequireSelectionMatch)
                    RestoreValidSelectionText();

                popupLookup.IsOpen = false;
            }
        }

        private void ApplyFilter(string text)
        {
            FilteredItems.Clear();

            var allItems = ItemsSource?.Cast<object>().ToList() ?? new List<object>();
            IEnumerable<object> filtered = string.IsNullOrWhiteSpace(text)
                ? allItems.Take(MaxResults)
                : allItems.Where(x => MatchesSearch(x, text)).Take(MaxResults);

            foreach (var item in filtered)
                FilteredItems.Add(item);

            lstLookup.SelectedIndex = string.IsNullOrWhiteSpace(text)
                ? -1
                : (FilteredItems.Count > 0 ? 0 : -1);
            OnPropertyChanged(nameof(FilteredItems));
        }

        private void MoveSelection(int delta)
        {
            if (FilteredItems.Count == 0)
                return;

            var nextIndex = lstLookup.SelectedIndex;
            if (nextIndex < 0)
                nextIndex = 0;
            else
                nextIndex = Math.Max(0, Math.Min(FilteredItems.Count - 1, nextIndex + delta));

            lstLookup.SelectedIndex = nextIndex;
            lstLookup.ScrollIntoView(lstLookup.SelectedItem);
        }

        public bool TryCommitSelection()
            => ConfirmCurrentSelection(false);

        public bool TryCommitSelectionFromKeyboard()
            => ConfirmCurrentSelection(false);

        private bool ConfirmCurrentSelection(bool raiseEvent = true)
        {
            if (lstLookup.SelectedItem == null)
            {
                if (RequireSelectionMatch)
                {
                    RestoreValidSelectionText();
                    return false;
                }

                CommitFreeTextValue(raiseEvent);
                return true;
            }

            var selected = lstLookup.SelectedItem;
            SelectedLookupItem = selected;
            SetEditorText(GetDisplayValue(selected), false);

            if (selected is LookupItem lookupItem)
            {
                SelectedId = lookupItem.Id;
                SelectedCode = lookupItem.Code;
                SelectedDescription = lookupItem.Description;
            }

            popupLookup.IsOpen = false;
            if (raiseEvent)
                RaiseEvent(new RoutedEventArgs(SelectionConfirmedEvent, this));

            return true;
        }

        private void CommitFreeTextValue(bool raiseEvent)
        {
            var freeText = txtLookup.Text?.Trim() ?? string.Empty;

            SelectedLookupItem = null;
            SelectedId = 0;
            SelectedCode = freeText;
            SelectedDescription = string.Empty;

            SetEditorText(freeText, false);
            popupLookup.IsOpen = false;
            if (raiseEvent)
                RaiseEvent(new RoutedEventArgs(SelectionConfirmedEvent, this));
        }

        private void RestoreValidSelectionText()
        {
            if (SelectedLookupItem is LookupItem lookupItem)
            {
                SelectedId = lookupItem.Id;
                SelectedCode = lookupItem.Code;
                SelectedDescription = lookupItem.Description;
                SetEditorText(lookupItem.Code, false);
                return;
            }

            SelectedId = 0;
            SelectedCode = string.Empty;
            SelectedDescription = string.Empty;
            SetEditorText(string.Empty, false);
        }

        private void ApplyHeadersFromTag()
        {
            var tagValue = Tag?.ToString() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(tagValue))
                return;

            var parts = tagValue.Split('|');
            if (parts.Length > 0 && !string.IsNullOrWhiteSpace(parts[0]))
                txtPrimaryHeader.Text = parts[0];

            if (parts.Length > 1 && !string.IsNullOrWhiteSpace(parts[1]))
                txtSecondaryHeader.Text = parts[1];
        }

        private void SetEditorText(string text, bool selectAll)
        {
            _isUpdatingText = true;
            txtLookup.Text = text;

            if (selectAll)
                txtLookup.SelectAll();
            else
                txtLookup.CaretIndex = txtLookup.Text.Length;

            _isUpdatingText = false;
        }

        private bool MatchesSearch(object item, string searchText)
        {
            var paths = (SearchMemberPaths ?? string.Empty)
                .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            if (!paths.Any())
                return GetDisplayValue(item).Contains(searchText, StringComparison.OrdinalIgnoreCase);

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

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void FocusEditor()
        {
            txtLookup.Focus();
            txtLookup.SelectAll();
            Keyboard.Focus(txtLookup);
        }

        public void ClearSelection()
        {
            SelectedLookupItem = null;
            SelectedId = 0;
            SelectedCode = string.Empty;
            SelectedDescription = string.Empty;
            SetEditorText(string.Empty, false);
            ApplyFilter(string.Empty);
            popupLookup.IsOpen = false;
        }
    }
}
