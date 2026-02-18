using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Linq.Expressions;
using LD.Contracts.Client;

namespace LD.Forms.Classes
{
    public class GridFilter<T>
    {
        private DataGridView _grid;
        private BindingSource _binding;
        private DataGridView _filterGrid;
        private Dictionary<string, PropertyInfo> _propertyCache;
        private String valorSelFiltro;
        private String valorColFiltro;
        private List<T> _originalData;
        private readonly Dictionary<string, object> _activeFilters = new();


        private System.Windows.Forms.ContextMenuStrip menuDer;
        private System.Windows.Forms.ToolStripMenuItem SearchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OrderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FilterToolStrimMenuItem;
        private System.Windows.Forms.ToolStripMenuItem NoFilterToolStrimMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CopyToolStrimMenuItem;


        // private List<T> _originalData;
        public GridFilter(DataGridView grid, BindingSource binding)
        {
            _grid = grid;
            _binding = binding;

            _propertyCache = typeof(T)
                .GetProperties()
                .ToDictionary(p => p.Name, p => p);

            //  EnsureContainer();

            CreateFilterGrid();
            startMenuStrip();
             SyncColumns();
        }
        public void SetData(List<T> data)
        {
            _originalData = data;
            _binding.DataSource = data;

            
        }

        private void startMenuStrip()
        {
            this.menuDer = new System.Windows.Forms.ContextMenuStrip();
            this.SearchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OrderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FilterToolStrimMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NoFilterToolStrimMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CopyToolStrimMenuItem = new System.Windows.Forms.ToolStripMenuItem();


            this.menuDer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CopyToolStrimMenuItem,
            this.SearchToolStripMenuItem,
            this.FilterToolStrimMenuItem,
            this.NoFilterToolStrimMenuItem,
            this.OrderToolStripMenuItem});
            this.menuDer.Name = "menuDer";
            this.menuDer.Size = new System.Drawing.Size(181, 114);

            // 
            // SearchToolStripMenuItem
            // 
            this.SearchToolStripMenuItem.Image = Properties.Resources.search2;
            this.SearchToolStripMenuItem.Name = "SearchToolStripMenuItem";
            this.SearchToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.SearchToolStripMenuItem.Text = "Buscar (Ctrl + G)";
            this.SearchToolStripMenuItem.Click += new System.EventHandler(this.SearchToolStripMenuItem_Click);
            // 
            // OrderToolStripMenuItem
            // 
            this.OrderToolStripMenuItem.Image = Properties.Resources.reordenar;
            this.OrderToolStripMenuItem.Name = "OrderToolStripMenuItem";
            this.OrderToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.OrderToolStripMenuItem.Text = "Ordenar columnas";
            this.OrderToolStripMenuItem.Click += new System.EventHandler(this.OrderToolStripMenuItem_Click);
            // 
            // FilterToolStrimMenuItem
            // 
            this.FilterToolStrimMenuItem.Image = Properties.Resources.filtro;
            this.FilterToolStrimMenuItem.Name = "FilterToolStrimMenuItem";
            this.FilterToolStrimMenuItem.Size = new System.Drawing.Size(180, 22);
            this.FilterToolStrimMenuItem.Text = "Filtrar";
            this.FilterToolStrimMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.FilterToolStrimMenuItem.Click += new System.EventHandler(this.FilterToolStrimMenuItem_Click);
            // 
            // NoFilterToolStrimMenuItem
            // 
            this.NoFilterToolStrimMenuItem.Image = Properties.Resources.quitarFiltro;
            this.NoFilterToolStrimMenuItem.Name = "NoFilterToolStrimMenuItem";
            this.NoFilterToolStrimMenuItem.Size = new System.Drawing.Size(180, 22);
            this.NoFilterToolStrimMenuItem.Text = "Quitar Filtros";
            this.NoFilterToolStrimMenuItem.Click += new System.EventHandler(this.NoFilterToolStrimMenuItem_Click);

            // 
            // CopyToolStrimMenuItem
            // 
            this.CopyToolStrimMenuItem.Image = Properties.Resources.copiar;
            this.CopyToolStrimMenuItem.Name = "CopyToolStrimMenuItem";
            this.CopyToolStrimMenuItem.Size = new System.Drawing.Size(180, 22);
            this.CopyToolStrimMenuItem.Text = "Copiar";
            this.CopyToolStrimMenuItem.Click += new System.EventHandler(this.CopyToolStripMenuItem_Click);

        }


        public DataGridView BuildFilterColumns()
        {
         

            _propertyCache = typeof(T)
                .GetProperties()
                .ToDictionary(p => p.Name, p => p);
            
              DataGridViewColumn[] columns;
            DataGridViewColumn[]  columnsFilter = new DataGridViewColumn[this._grid.Columns.Count];
            for (int x = 0; x < _grid.Columns.Count; x++)
            {
                DataGridViewTextBoxColumn c = new DataGridViewTextBoxColumn();
                c.DataPropertyName = _grid.Columns[x].DataPropertyName;
                c.HeaderText = _grid.Columns[x].HeaderText;
                c.MinimumWidth = _grid.Columns[x].MinimumWidth; ;
                c.Name = _grid.Columns[x].Name;
                c.Width = _grid.Columns[x].Width;
                columnsFilter[x] = c;
            }
            this._filterGrid.Columns.Clear();
            this._filterGrid.Columns.AddRange(columnsFilter);
            this._filterGrid.Rows.Add(new DataGridViewRow());
            return this._grid;
        }


        private void SyncColumns()
        {
            this._grid.AllowUserToAddRows = false;
            this._grid.AllowUserToDeleteRows = false;
            this._grid.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView_ColumnWidthChanged);
            this._grid.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDown);
            this._grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyDown);
            this._grid.Sorted += new System.EventHandler(this.dg_Sorted);
            this._grid.VisibleChanged += new System.EventHandler(this.dg_VisibleChanged);
            this._grid.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dg_Scroll);
            //this._filterGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgFilter_CellValueChanged);
            this._filterGrid.Sorted += new System.EventHandler(this.dg_SortedFilter);
            this._filterGrid.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgFilter_CellMouseDown);
            this._filterGrid.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgFilter_ColumnWidthChanged);
            this._grid.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView_DataError);
            this._filterGrid.CellValueChanged += (_, __) => ApplyFilters();
            this._filterGrid.EditingControlShowing += Filter_EditingControlShowing;

        }



        private void CreateFilterGrid()
        {
            if (this._filterGrid == null)
            {

                System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
                this._filterGrid = new DataGridView();
                this._filterGrid.AllowUserToAddRows = false;
                this._filterGrid.AllowUserToDeleteRows = false;
                this._filterGrid.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
                this._filterGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
                this._filterGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                this._filterGrid.ColumnHeadersHeight = 25;
                dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
                dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLightLight;
                dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
                dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.ControlLightLight;
                dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
                dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
                this._filterGrid.DefaultCellStyle = dataGridViewCellStyle1;
                this._filterGrid.Dock = System.Windows.Forms.DockStyle.Top;
                this._filterGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
                this._filterGrid.Location = new System.Drawing.Point(8, 47);
                this._filterGrid.RowHeadersWidth = 51;
                this._filterGrid.ScrollBars = System.Windows.Forms.ScrollBars.None;
                this._filterGrid.Size = new System.Drawing.Size(1238, 55);
                this._filterGrid.TabIndex = 5;
                this._filterGrid.Visible = false;


                //            this.groupBox3.Controls.Add(this.dtEncFilter);
                this._grid.ColumnHeadersHeight = 55;
                this._grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                this._grid.Controls.Add(this._filterGrid);
            }

            
            
        }



        private void dataGridView_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            this._filterGrid.Columns[e.Column.Name].Width = e.Column.Width;
        }
        private void _filterGrid_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {           
            this._grid.Columns[e.Column.Name].Width = e.Column.Width;
        }

        // click derecho
        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex != -1 && e.ColumnIndex != -1)
                {
                    this._grid.CurrentCell = this._grid.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    this.menuDer.Show(this._grid, Cursor.Position);
                    this.menuDer.Show(Cursor.Position);
                    this.valorColFiltro = this._grid.Columns[e.ColumnIndex].Name;
                    this.valorSelFiltro = this._grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();                    
                    FilterToolStrimMenuItem.Text = "Es Igual a " + this.valorSelFiltro;
                    FilterToolStrimMenuItem.Visible = true;
                }
                else
                {
                    FilterToolStrimMenuItem.Visible = false;
                    this.menuDer.Show(Cursor.Position);
                }
            }
        }
        // click pra ordenar
        private void OrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
           /*falta
            * 
            * FrmOrderColumn order = new FrmOrderColumn(this.Usuario, this.nombreFormulario);
            order.ShowDialog();
            if (order.getRespuesta())// si contesta correcto
            {
                this.actualizaColumnas(this._grid, this.bs, this.Usuario, this.nombreFormulario);
                this.pinta_grid();
            }*/

        }
        // click para quitar filtro        
        private void NoFilterToolStrimMenuItem_Click(object sender, EventArgs e)
        {
            this._binding.Filter = null;
            foreach (DataGridViewColumn col in _filterGrid.Columns)
            {
                _filterGrid.Rows[0].Cells[col.Index].Value = null;
            }

            ApplyFilters();
            pintaDG();
         
        }
        // click para mostrar filtro        
        private void FilterToolStrimMenuItem_Click(object sender, EventArgs e)
        {
            int x = 0;
            this._filterGrid.Rows[0].Cells[valorColFiltro].Value = this.valorSelFiltro;
            ApplyFilters();
            pintaDG();
          
        }


        // click para filtrrar
        private void SearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFormFilter();
        }
        // click para copiar
        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Clipboard.SetText(this.valorSelFiltro);

        }

        // Control + G  ( Abre para buscar )
        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.G)
            {
                this.valorColFiltro = "";
                openFormFilter();
            }
        }


        public void openFormFilter()
        {
            /* frmFilter = new FrmFilter(this.Usuario, this.nombreFormulario, this.valorColFiltro);
              frmFilter.ShowDialog();
              if(frmFilter.getRespuesta())
              {
                  filtrar();
              }*/
            this._filterGrid.Visible = !this._filterGrid.Visible;
            if (this._filterGrid.Visible)
            {
                //  this._grid.ColumnHeadersVisible = false;

                this._grid.ColumnHeadersHeight = 55;
                this._grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;                
            }
            else
            {
                this._grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                //this._grid.ColumnHeadersVisible = true;
            }
        }


        private void dg_Sorted(object sender, EventArgs e)
        {
            pintaDG();
        }

        private void dg_VisibleChanged(object sender, EventArgs e)
        {
            pintaDG();
        }

        private void dg_Scroll(object sender, ScrollEventArgs e)
        {

            this._filterGrid.HorizontalScrollingOffset = this._grid.HorizontalScrollingOffset;

        }

        private void dgFilter_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

            ApplyFilters();

        }



        private void dg_SortedFilter(object sender, EventArgs e)
        {
            


            var columnName = this._filterGrid.SortedColumn.Name;
            var ascending = this._filterGrid.SortOrder == SortOrder.Ascending;

            var list = (List<T>)_binding.DataSource;

            if (ascending)
                list = list.OrderBy(x => GetPropertyValue(x, columnName)).ToList();
            else
                list = list.OrderByDescending(x => GetPropertyValue(x, columnName)).ToList();

            _binding.DataSource = list;


            if (this._filterGrid.SortOrder == SortOrder.Ascending)
                _binding.Sort = columnName + " ASC";
            else
                _binding.Sort = columnName + " DESC";
        }

        private object GetPropertyValue(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName)?.GetValue(obj, null);
        }




        private void dgFilter_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                CopyToolStrimMenuItem.Visible = false;
                FilterToolStrimMenuItem.Visible = false;
                OrderToolStripMenuItem.Visible = false;
                SearchToolStripMenuItem.Visible = false;
                this.menuDer.Show(Cursor.Position);

            }
        }

        private void dgFilter_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
           /* Encryptor enc = new Encryptor();


            if (this.nombreFormulario.Equals(""))
            {
                return;
            }
            Formularios f = new Formularios();
            f.ActualizaColumnaForm(this.Usuario.Id, this.nombreFormulario, e.Column.Name, e.Column.Width.ToString());
            if (f.Valido == false)
            {
                FrmError frmError = new FrmError(f.Error);
                frmError.ShowDialog();
            }*/
            this._grid.Columns[e.Column.Name].Width = e.Column.Width;
        }



        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {
                throw e.Exception;
            }
            catch (Exception ex)
            {

            }
        }


        public void pintaDG()
        {
            
            _grid.RowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(252)))), ((int)(((byte)(213)))));
            _grid.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            _grid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }


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

        /* private void ApplyFilters()
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
                     string strVal = filter.Value.ToString().ToLower();
                     bool boolValue = false;
                     if ((strVal=="1") || strVal.ToLower()=="true")
                     {
                         boolValue = true;
                     }

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
         }*/



        private void ApplyFilters()
        {
            if (_originalData == null)
                return;

            _activeFilters.Clear();

            foreach (DataGridViewColumn col in _filterGrid.Columns)
            {
                var value = _filterGrid.Rows[0]
                    .Cells[col.Index].Value?.ToString();

                if (!string.IsNullOrWhiteSpace(value))
                    _activeFilters[col.Name] = value.Trim();
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

                Expression condition = BuildCondition(member, prop, filter.Value?.ToString());

                if (condition == null)
                    continue;

                body = body == null
                    ? condition
                    : Expression.AndAlso(body, condition);
            }

            var lambda =
                Expression.Lambda<Func<T, bool>>(body, parameter).Compile();

            _binding.DataSource = _originalData.Where(lambda).ToList();

            HighlightFilteredColumns();
        }

        // ===============================
        // Motor de condiciones avanzado
        // ===============================

        private Expression BuildCondition(
    MemberExpression member,
    PropertyInfo prop,
    string input)
        {
            var values = input.Split(',')
                              .Select(v => v.Trim())
                              .Where(v => !string.IsNullOrWhiteSpace(v))
                              .ToList();

            Expression finalExpression = null;

            foreach (var val in values)
            {
                Expression condition = null;

                // STRING
                if (prop.PropertyType == typeof(string))
                {
                    var toLower = Expression.Call(member, "ToLower", null);
                    var value = val.Replace("*", "").ToLower();
                    var constant = Expression.Constant(value);
                    condition = Expression.Call(toLower, "Contains", null, constant);
                }

                // DATE RANGE
                else if (prop.PropertyType == typeof(DateTime)
                      || prop.PropertyType == typeof(DateTime?))
                {
                    if (val.Contains(".."))
                    {
                        var parts = val.Split("..");

                        if (DateTime.TryParse(parts[0], out DateTime start)
                         && DateTime.TryParse(parts[1], out DateTime end))
                        {
                            var startConst = Expression.Constant(start.Date);
                            var endConst = Expression.Constant(end.Date);
                            var memberDate = Expression.Property(member, "Date");

                            var greater =
                                Expression.GreaterThanOrEqual(memberDate, startConst);
                            var less =
                                Expression.LessThanOrEqual(memberDate, endConst);

                            condition = Expression.AndAlso(greater, less);
                        }
                    }
                    else if (DateTime.TryParse(val, out DateTime dt))
                    {
                        var constant = Expression.Constant(dt.Date);
                        var memberDate = Expression.Property(member, "Date");
                        condition = Expression.Equal(memberDate, constant);
                    }
                }

                // NUMERIC
                else if (IsNumeric(prop.PropertyType))
                {
                    var op = GetOperator(val);

                    var cleanValue = val.Replace(">=", "")
                                        .Replace("<=", "")
                                        .Replace(">", "")
                                        .Replace("<", "")
                                        .Replace("=", "");

                    if (double.TryParse(cleanValue, out double num))
                    {
                        var constant =
                            Expression.Constant(Convert.ChangeType(num,
                                prop.PropertyType));

                        condition = op switch
                        {
                            ">=" => Expression.GreaterThanOrEqual(member, constant),
                            "<=" => Expression.LessThanOrEqual(member, constant),
                            ">" => Expression.GreaterThan(member, constant),
                            "<" => Expression.LessThan(member, constant),
                            _ => Expression.Equal(member, constant)
                        };
                    }
                }

                // BOOL
                else if (prop.PropertyType == typeof(bool)
                      || prop.PropertyType == typeof(bool?))
                {
                    if (bool.TryParse(val, out bool b))
                        condition = Expression.Equal(member,
                            Expression.Constant(b));
                }

                if (condition == null)
                    continue;

                finalExpression = finalExpression == null
                    ? condition
                    : Expression.OrElse(finalExpression, condition);
            }

            return finalExpression;
        }



        private static bool IsNumeric(Type type)
        {
            return type == typeof(int)
                || type == typeof(decimal)
                || type == typeof(double)
                || type == typeof(float)
                || type == typeof(long);
        }

        private static string GetOperator(string input)
        {
            if (input.StartsWith(">=")) return ">=";
            if (input.StartsWith("<=")) return "<=";
            if (input.StartsWith(">")) return ">";
            if (input.StartsWith("<")) return "<";
            return "=";
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
