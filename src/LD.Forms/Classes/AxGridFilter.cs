using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;


namespace LD.Forms.Classes
{
   
    public class AxGridFilter<T>
    {
        private readonly DataGridView _grid;
        private readonly BindingSource _binding;

        private DataGridView _filterGrid;
        private Panel _container;

        private List<T> _originalData;

        private readonly Dictionary<string, PropertyInfo> _propertyCache;
        private readonly Dictionary<string, object> _activeFilters = new();

        public AxGridFilter(DataGridView grid, BindingSource binding)
        {
            _grid = grid;
            _binding = binding;

            _propertyCache = typeof(T)
                .GetProperties()
                .ToDictionary(p => p.Name, p => p);

          //  EnsureContainer();
            CreateFilterGrid();
            SyncScroll();
            SyncColumns();
        }

        // ==============================
        // PUBLIC
        // ==============================

        public void SetData(List<T> data)
        {
            _originalData = data;
            _binding.DataSource = data;

            BuildFilterColumns();
        }

        public void ClearFilters()
        {
            _activeFilters.Clear();
            _binding.DataSource = _originalData;

            foreach (DataGridViewCell cell in _filterGrid.Rows[0].Cells)
                cell.Value = null;

            ResetHeaderStyle();
        }

        // ==============================
        // LAYOUT AX
        // ==============================

        private void EnsureContainer()
        {
            if (_grid.Parent is Panel)
            {
                _container = (Panel)_grid.Parent;
                return;
            }

            _container = new Panel
            {
                Dock = _grid.Dock,
                Location = _grid.Location,
                Size = _grid.Size
            };

            var parent = _grid.Parent;
            parent.Controls.Remove(_grid);
            _container.Controls.Add(_grid);
            parent.Controls.Add(_container);

            _grid.Dock = DockStyle.Fill;
        }

        private void CreateFilterGrid()
        {
            _filterGrid = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 26,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                MultiSelect = false,
                RowHeadersVisible = false,
                ColumnHeadersVisible = false,
                ScrollBars = ScrollBars.None,
                BorderStyle = BorderStyle.None,
                BackgroundColor = _grid.BackgroundColor,
                EnableHeadersVisualStyles = false,
                ColumnHeadersHeight=102,
                RowHeadersWidth=52,
                Size = new System.Drawing.Size(1238, 55),
                Location = new System.Drawing.Point(8, 47),
                TabIndex = 5,
                


            };
            

            _filterGrid.DefaultCellStyle.BackColor = Color.White;
            _filterGrid.DefaultCellStyle.SelectionBackColor = Color.White;
            _filterGrid.DefaultCellStyle.SelectionForeColor = Color.Black;
            _filterGrid.DefaultCellStyle.Padding = new Padding(2);
            _filterGrid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            _filterGrid.GridColor = Color.LightGray;            
            _grid.Controls.Add(_filterGrid);
            _filterGrid.BringToFront();

            _filterGrid.CellValueChanged += (_, __) => ApplyFilters();
            _filterGrid.EditingControlShowing += Filter_EditingControlShowing;
        }

        private void BuildFilterColumns()
        {
            _filterGrid.Columns.Clear();

            foreach (DataGridViewColumn col in _grid.Columns)
            {
                if (string.IsNullOrWhiteSpace(col.DataPropertyName))
                    continue;

                var filterCol = new DataGridViewTextBoxColumn
                {
                    Name = col.DataPropertyName,
                    Width = col.Width,
                    HeaderText = col.DataPropertyName,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                };

                _filterGrid.Columns.Add(filterCol);
            }

         //   _filterGrid.Rows.Clear();
            _filterGrid.Rows.Add();
        }

        // ==============================
        // SYNC AX
        // ==============================

        private void SyncScroll()
        {
            _grid.Scroll += (s, e) =>
            {
                if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
                {
                    _filterGrid.HorizontalScrollingOffset =
                        _grid.HorizontalScrollingOffset;
                }
            };
        }

        private void SyncColumns()
        {
            _grid.ColumnWidthChanged += (s, e) =>
            {
                if (_filterGrid.Columns.Contains(e.Column.DataPropertyName))
                {
                    _filterGrid.Columns[e.Column.DataPropertyName].Width =
                        e.Column.Width;
                }
            };

            _grid.ColumnDisplayIndexChanged += (s, e) =>
            {
                if (_filterGrid.Columns.Contains(e.Column.DataPropertyName))
                {
                    _filterGrid.Columns[e.Column.DataPropertyName].DisplayIndex =
                        e.Column.DisplayIndex;
                }
            };
        }

        // ==============================
        // FILTRO
        // ==============================

        private void Filter_EditingControlShowing(object sender,
            DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is TextBox tb)
            {
                tb.TextChanged -= FilterTextChanged;
                tb.TextChanged += FilterTextChanged;
            }
        }

        private void FilterTextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (_originalData == null)
                return;

            _activeFilters.Clear();

            foreach (DataGridViewColumn col in _filterGrid.Columns)
            {
                var value = _filterGrid.Rows[0].Cells[col.Index].Value?.ToString();

                if (!string.IsNullOrWhiteSpace(value))
                    _activeFilters[col.Name] = value;
            }

            if (_activeFilters.Count == 0)
            {
                _binding.DataSource = _originalData;
                ResetHeaderStyle();
                return;
            }

            var parameter = Expression.Parameter(typeof(T), "x");
            Expression body = null;

            foreach (var filter in _activeFilters)
            {
                var prop = _propertyCache[filter.Key];
                var member = Expression.Property(parameter, prop);

                Expression condition;

                if (prop.PropertyType == typeof(string))
                {
                    var toLower = Expression.Call(member, "ToLower", null);
                    var filterValue = filter.Value.ToString()
                        .Replace("*", "")
                        .ToLower();

                    var constant = Expression.Constant(filterValue);
                    condition = Expression.Call(toLower,
                        "Contains", null, constant);
                }
                else if (prop.PropertyType == typeof(bool)
                      || prop.PropertyType == typeof(bool?))
                {
                    var boolValue = bool.Parse(filter.Value.ToString());
                    var constant = Expression.Constant(boolValue);
                    condition = Expression.Equal(member, constant);
                }
                else if (prop.PropertyType == typeof(DateTime)
                      || prop.PropertyType == typeof(DateTime?))
                {
                    if (DateTime.TryParse(filter.Value.ToString(),
                        out DateTime dt))
                    {
                        var constant = Expression.Constant(dt.Date);
                        var memberDate = Expression.Property(member, "Date");
                        condition = Expression.Equal(memberDate, constant);
                    }
                    else continue;
                }
                else
                {
                    var constant = Expression.Constant(
                        Convert.ChangeType(filter.Value,
                        prop.PropertyType));

                    condition = Expression.Equal(member, constant);
                }

                body = body == null
                    ? condition
                    : Expression.AndAlso(body, condition);
            }

            var lambda =
                Expression.Lambda<Func<T, bool>>(body, parameter).Compile();

            var filtered = _originalData.Where(lambda).ToList();

            _binding.DataSource = filtered;

            HighlightFilteredColumns();
        }

        // ==============================
        // VISUAL
        // ==============================

        private void HighlightFilteredColumns()
        {
            foreach (DataGridViewColumn col in _grid.Columns)
            {
                col.HeaderCell.Style.ForeColor =
                    _activeFilters.ContainsKey(col.DataPropertyName)
                    ? Color.Blue
                    : Color.Black;
            }
        }

        private void ResetHeaderStyle()
        {
            foreach (DataGridViewColumn col in _grid.Columns)
                col.HeaderCell.Style.ForeColor = Color.Black;
        }
    }
}