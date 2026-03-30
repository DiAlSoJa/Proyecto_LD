using LD.Client.Services;
using LD.Contracts.Category;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.FormsX.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LD.FormsX.Views.Categorias
{
    public partial class NuevaCategoriaView : Window
    {
        private readonly CategoryService _categoryService;
        private readonly LookupService _lookupService;

        private CategoryDto? CategorySelect;
        private bool _cargandoDatos = false;

        public bool ResponseForm { get; private set; }

        public NuevaCategoriaView(CategoryService categoryService, LookupService lookupService)
        {
            InitializeComponent();
            _categoryService = categoryService;
            _lookupService = lookupService;
        }

        public async void SetCategory(CategoryDto category)
        {
            CategorySelect = category;
            await CargarDatosInicialesAsync();
        }

        protected override async void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            if (cmbCliente.Items.Count == 0)
                await CargarDatosInicialesAsync();
        }

        private async Task CargarDatosInicialesAsync()
        {
            try
            {
                _cargandoDatos = true;

                await SetCombos();

                if (CategorySelect != null)
                    await CargarDatosAsync();
            }
            finally
            {
                _cargandoDatos = false;
            }
        }

        private async Task SetCombos()
        {
            var clientes = await _lookupService.GetClientLookup();

            if (clientes.IsSuccess && clientes.Data != null)
            {
                cmbCliente.ItemsSource = clientes.Data;
                cmbCliente.DisplayMemberPath = "Value";
                cmbCliente.SelectedValuePath = "Key";
                cmbCliente.SelectedIndex = -1;
            }
        }

        private async Task SetCombosProjects(string projectSel = "")
        {
            if (cmbCliente.SelectedValue == null)
            {
                cmbProyecto.ItemsSource = null;
                return;
            }

            if (!int.TryParse(cmbCliente.SelectedValue.ToString(), out int clienteId) || clienteId <= 0)
            {
                cmbProyecto.ItemsSource = null;
                return;
            }

            var proyectos = await _lookupService.GetProjectClientLookup(clienteId);

            if (!proyectos.IsSuccess || proyectos.Data == null)
            {
                cmbProyecto.ItemsSource = null;
                return;
            }

            cmbProyecto.DisplayMemberPath = "Value";
            cmbProyecto.SelectedValuePath = "Key";
            cmbProyecto.ItemsSource = proyectos.Data;

            if (!string.IsNullOrWhiteSpace(projectSel))
                cmbProyecto.SelectedValue = projectSel;
            else if (proyectos.Data.Count > 1)
                cmbProyecto.SelectedIndex = -1;
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                var response = await _categoryService.GetCategoryById(CategorySelect?.CategoriaId ?? 0);

                if (!response.IsSuccess)
                {
                    DialogHelper.ShowError(response.Message ?? "No se pudo cargar la categoría.");
                    return;
                }

                var item = response.Data;

                txtId.Text = item.CategoryName;
                txtNombre.Text = item.Description;
                txtFrecuencia.Text = item.Frecuency?.ToString() ?? "";
                txtId.IsEnabled = CategorySelect == null;

                cmbCliente.SelectedValue = item.ClientId.ToString();
                await SetCombosProjects(item.ProjectId.ToString());
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private CategoryRequest BuildRequest()
        {
            return new CategoryRequest
            {
                CategoryName = CategorySelect != null ? CategorySelect.Categoria : txtId.Text.Trim(),
                Description = txtNombre.Text.Trim(),
                Frecuency = int.TryParse(txtFrecuencia.Text, out int frec) ? frec : null,
                ClientId = int.TryParse(cmbCliente.SelectedValue?.ToString(), out int clienteId) ? clienteId : 0,
                ProjectId = int.TryParse(cmbProyecto.SelectedValue?.ToString(), out int projectId) ? projectId : 0
            };
        }

        private Task<ApiResponseDto<string>> CreateCategory(CategoryRequest request) =>
            _categoryService.CreateCategory(request);

        private Task<ApiResponseDto<string>> EditCategory(int categoryId, CategoryRequest request) =>
            _categoryService.UpdateCategory(categoryId, request);

        private async Task<ApiResponseDto<string>> SaveCategory(CategoryRequest request)
        {
            return CategorySelect != null
                ? await EditCategory(CategorySelect.CategoriaId, request)
                : await CreateCategory(request);
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnSave.IsEnabled = false;

                var request = BuildRequest();
                var result = await SaveCategory(request);

                if (result.IsSuccess)
                {
                    DialogHelper.ShowSuccess(result.Data ?? "Guardado correctamente.");
                    ResponseForm = true;
                    this.DialogResult = true;
                    Close();
                }
                else
                {
                    DialogHelper.ShowError(result.ErrorMessage ?? "Hubo un error al guardar.");
                }
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                btnSave.IsEnabled = true;
            }
        }

        private async void cmbCliente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_cargandoDatos)
                return;

            await SetCombosProjects();
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}