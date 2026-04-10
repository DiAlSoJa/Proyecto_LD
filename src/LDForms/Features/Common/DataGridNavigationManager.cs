using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace LD.FormsX.Features.Common
{
    public sealed class DataGridNavigationManager
    {
        private readonly DataGrid _grid;

        public DataGridNavigationManager(DataGrid grid)
        {
            _grid = grid;
        }

        public void CommitCurrentEdit()
        {
            _grid.CommitEdit(DataGridEditingUnit.Cell, true);
            _grid.CommitEdit(DataGridEditingUnit.Row, true);
        }

        public void HandleCurrentCellChanged()
        {
            if (_grid.CurrentCell.Column == null)
                return;

            if (_grid.CurrentCell.Item == null)
                return;

            if (_grid.CurrentCell.Column.IsReadOnly)
                return;

            _grid.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (!_grid.IsKeyboardFocusWithin)
                    _grid.Focus();

                _grid.BeginEdit();
            }), DispatcherPriority.Background);
        }

        public void HandleLookupSelectionConfirmed(object sender)
        {
            if (sender is not FrameworkElement editor)
                return;

            if (editor.DataContext == null)
                return;

            _grid.Dispatcher.BeginInvoke(new Action(() =>
            {
                CommitCurrentEdit();
                MoveFocusToNextCell(editor.DataContext);
            }), DispatcherPriority.Background);
        }

        public bool HandleEnterKeyNavigation()
        {
            if (_grid.CurrentCell.Column == null)
                return false;

            if (_grid.CurrentItem == null)
                return false;

            CommitCurrentEdit();
            MoveFocusToNextCell(_grid.CurrentItem);
            return true;
        }

        public void MoveFocusToNextCell(object rowItem)
        {
            if (_grid.CurrentCell.Column == null)
                return;

            int currentIndex = _grid.Columns.IndexOf(_grid.CurrentCell.Column);
            if (currentIndex < 0)
                return;

            var nextColumn = _grid.Columns
                .Skip(currentIndex + 1)
                .FirstOrDefault(c => !c.IsReadOnly);

            if (nextColumn == null)
                return;

            MoveFocusToCell(rowItem, nextColumn);
        }

        public void MoveFocusToFirstEditableCell(object rowItem)
        {
            var firstEditableColumn = _grid.Columns
                .OrderBy(c => c.DisplayIndex)
                .FirstOrDefault(c => !c.IsReadOnly);

            if (firstEditableColumn == null)
                return;

            MoveFocusToCell(rowItem, firstEditableColumn);
        }

        public DataGridColumn? GetLastEditableColumn()
        {
            return _grid.Columns
                .Where(c => !c.IsReadOnly)
                .OrderBy(c => c.DisplayIndex)
                .LastOrDefault();
        }

        private void MoveFocusToCell(object rowItem, DataGridColumn targetColumn)
        {
            _grid.CurrentCell = new DataGridCellInfo(rowItem, targetColumn);
            _grid.ScrollIntoView(rowItem, targetColumn);
            _grid.UpdateLayout();

            _grid.Dispatcher.BeginInvoke(new Action(() =>
            {
                var cell = GetDataGridCell(rowItem, targetColumn);
                if (cell != null)
                {
                    cell.Focus();
                    Keyboard.Focus(cell);
                }

                _grid.Dispatcher.BeginInvoke(new Action(() =>
                {
                    _grid.BeginEdit();

                    _grid.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        var refreshedCell = GetDataGridCell(rowItem, targetColumn);
                        if (refreshedCell != null)
                            FocusEditableContent(refreshedCell);
                    }), DispatcherPriority.Input);
                }), DispatcherPriority.Input);
            }), DispatcherPriority.Background);
        }

        public bool IsEventInsideControl<T>(DependencyObject? source) where T : DependencyObject
            => FindVisualParent<T>(source) != null;

        public DataGridCell? GetDataGridCell(object item, DataGridColumn column)
        {
            var rowContainer = _grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
            if (rowContainer == null)
                return null;

            var presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);
            if (presenter == null)
            {
                _grid.ScrollIntoView(item, column);
                rowContainer.UpdateLayout();
                presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);
            }

            return presenter?.ItemContainerGenerator.ContainerFromIndex(column.DisplayIndex) as DataGridCell;
        }

        public T? FindVisualChild<T>(DependencyObject parent) where T : class
        {
            if (parent == null)
                return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T correctlyTyped)
                    return correctlyTyped;

                var descendant = FindVisualChild<T>(child);
                if (descendant != null)
                    return descendant;
            }

            return null;
        }

        public void FocusEditableContent(DataGridCell cell)
        {
            if (FindVisualChild<InlineLookupEditor>(cell) is InlineLookupEditor inlineLookup)
            {
                inlineLookup.Focus();
                Keyboard.Focus(inlineLookup);
                return;
            }

            if (FindVisualChild<TextBox>(cell) is TextBox textBox)
            {
                textBox.Focus();
                textBox.SelectAll();
                Keyboard.Focus(textBox);
                return;
            }

            cell.Focus();
            Keyboard.Focus(cell);
        }

        private static T? FindVisualParent<T>(DependencyObject? child) where T : DependencyObject
        {
            while (child != null)
            {
                if (child is T parent)
                    return parent;

                child = VisualTreeHelper.GetParent(child);
            }

            return null;
        }
    }
}
