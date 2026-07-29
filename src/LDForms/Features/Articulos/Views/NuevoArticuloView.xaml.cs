using LD.Client.Services;
using LD.Contracts.Product;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.FormsX.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LD.FormsX.Views.Articulos
{
    public partial class NuevoArticuloView : Window
    {
        private readonly ProductService _itemService;
        private readonly LookupService _lookupService;
        private readonly CategoryService _categoryService;

        private ProductDto? ItemSelected;
        private bool _cargandoDatos = false;
        private int _selectedClientId;
        private int _selectedProjectId;
        private string _initialPartNumber = string.Empty;
        private List<CargaMasivaArticuloRow> _cargaMasivaItems = new();

        public bool ResponseForm { get; private set; }

        public NuevoArticuloView(
            ProductService itemService,
            LookupService lookupService,
            CategoryService categoryService)
        {
            InitializeComponent();
            _itemService = itemService;
            _lookupService = lookupService;
            _categoryService = categoryService;
        }

        public void SetContext(int clientId, int projectId)
        {
            _selectedClientId = clientId;
            _selectedProjectId = projectId;
        }

        public void SetInitialPartNumber(string? partNumber)
        {
            _initialPartNumber = partNumber?.Trim() ?? string.Empty;

            if (txtNoParte != null && ItemSelected == null && _cargaMasivaItems.Count == 0)
                txtNoParte.Text = _initialPartNumber;
        }

        public void SetCargaMasivaItems(IEnumerable<CargaMasivaArticuloRow> items)
        {
            _cargaMasivaItems = items?
                .Where(x => !string.IsNullOrWhiteSpace(x.PartNumber) || !string.IsNullOrWhiteSpace(x.Description))
                .Select(x => new CargaMasivaArticuloRow
                {
                    PartNumber = x.PartNumber,
                    Description = x.Description
                })
                .ToList() ?? new List<CargaMasivaArticuloRow>();

            var isMassiveMode = _cargaMasivaItems.Count > 0;
            NoParteRow.Visibility = isMassiveMode ? Visibility.Collapsed : Visibility.Visible;
            DescripcionRow.Visibility = isMassiveMode ? Visibility.Collapsed : Visibility.Visible;
        }

        public async void SetItem(ProductDto? item)
        {
            ItemSelected = item;
            await CargarDatosInicialesAsync();
        }

        protected override async void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            if (!_cargandoDatos)
                await CargarDatosInicialesAsync();
        }

        private async Task CargarDatosInicialesAsync()
        {
            await SetCombos();

            if (ItemSelected != null)
                await CargarDatosAsync();
            else if (_cargaMasivaItems.Count == 0 && !string.IsNullOrWhiteSpace(_initialPartNumber))
                txtNoParte.Text = _initialPartNumber;
        }

        private async Task SetCombos()
        {
            try
            {
                _cargandoDatos = true;

                var dimensioner = await _lookupService.GetDimensionerLookup();
                if (dimensioner.IsSuccess && dimensioner.Data != null)
                {
                    cmbDimension.ItemsSource = dimensioner.Data;
                    cmbDimension.DisplayMemberPath = "Key";
                    cmbDimension.SelectedValuePath = "Key";
                    cmbDimension.SelectedIndex = -1;
                }

                var unidades = await _lookupService.GetUnitLookup();
                if (unidades.IsSuccess && unidades.Data != null)
                {
                    cmbUnidadMinima.ItemsSource = unidades.Data.ToList();
                    cmbUnidadMedia.ItemsSource = unidades.Data.ToList();
                    cmbUnidadMaxima.ItemsSource = unidades.Data.ToList();
                    cmbUnidadProduccion.ItemsSource = unidades.Data.ToList();
                    cmbPaqueteEstandar.ItemsSource = unidades.Data.ToList();

                    ConfigurarCombo(cmbUnidadMinima);
                    ConfigurarCombo(cmbUnidadMedia);
                    ConfigurarCombo(cmbUnidadMaxima);
                    ConfigurarCombo(cmbUnidadProduccion);
                    ConfigurarCombo(cmbPaqueteEstandar);
                }

                cmbCategoria.ItemsSource = null;
                cmbFamilia.ItemsSource = null;

                if (HasValidContext())
                {
                    await SetCombosCategories();
                    await SetCombosFamilias();
                }
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                _cargandoDatos = false;
            }
        }

        private void ConfigurarCombo(ComboBox combo)
        {
            combo.DisplayMemberPath = "Value";
            combo.SelectedValuePath = "Key";
            combo.SelectedIndex = -1;
        }

        private async Task SetCombosCategories(string selectedValue = "")
        {
            try
            {
                _cargandoDatos = true;

                if (!HasValidContext())
                {
                    cmbCategoria.ItemsSource = null;
                    cmbFamilia.ItemsSource = null;
                    return;
                }

                var categorias = await _lookupService.GetCategoryClientLookup(_selectedClientId, _selectedProjectId);
                if (categorias.IsSuccess && categorias.Data != null)
                {
                    cmbCategoria.DisplayMemberPath = "Value";
                    cmbCategoria.SelectedValuePath = "Key";
                    cmbCategoria.ItemsSource = categorias.Data;

                    if (!string.IsNullOrWhiteSpace(selectedValue))
                        cmbCategoria.SelectedValue = selectedValue;
                    else if (categorias.Data.Count > 1)
                        cmbCategoria.SelectedIndex = -1;
                }
                else
                {
                    cmbCategoria.ItemsSource = null;
                }

            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                _cargandoDatos = false;
            }
        }
        private async Task SetCombosFamilias(string selectedValue = "")
        {
            try
            {
                _cargandoDatos = true;

                if (!HasValidContext())
                {
                    cmbCategoria.ItemsSource = null;
                    cmbFamilia.ItemsSource = null;
                    return;
                }

                var familias = await _lookupService.GetFamilyClientLookup(_selectedClientId, _selectedProjectId);
                if (familias.IsSuccess && familias.Data != null)
                {
                    cmbFamilia.DisplayMemberPath = "Value";
                    cmbFamilia.SelectedValuePath = "Key";
                    cmbFamilia.ItemsSource = familias.Data;

                    if (!string.IsNullOrWhiteSpace(selectedValue))
                        cmbFamilia.SelectedValue = selectedValue;
                    else if (familias.Data.Count > 1)
                        cmbFamilia.SelectedIndex = -1;
                }
                else
                {
                    cmbFamilia.ItemsSource = null;
                }
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                _cargandoDatos = false;
            }
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                _cargandoDatos = true;

                var response = await _itemService.GetItemById(ItemSelected?.ItemId ?? 0);

                if (!response.IsSuccess)
                {
                    DialogHelper.ShowError(response.Message ?? "No se pudo cargar el artículo.");
                    return;
                }

                var item = response.Data;
                if (item == null)
                {
                    DialogHelper.ShowError("No se pudo cargar el artículo.");
                    return;
                }

                if (_selectedClientId <= 0)
                    _selectedClientId = item.ClientId ?? 0;

                if (_selectedProjectId <= 0)
                    _selectedProjectId = item.ProjectId ?? 0;

                await SetCombosCategories(item.CategoryId?.ToString() ?? string.Empty);
                await SetCombosFamilias(item.FamilyId?.ToString() ?? string.Empty);

                // Encabezado
                // Generales
                txtNoParte.Text = item.PartNumber ?? string.Empty;
                txtDescripcion.Text = item.Description ?? string.Empty;
                cmbCategoria.SelectedValue = item.CategoryId?.ToString();
                cmbFamilia.SelectedValue = item.FamilyId?.ToString();

                chkActivo.IsChecked = item.IsActive;
                chkTemperatura.IsChecked = item.IsTemperatureControlled;
                chkVMI.IsChecked = item.IsVMI;
                chkBOM.IsChecked = item.IsBOM;

                // Tipo de almacenamiento
                rdFifo.IsChecked = item.StorageTypeId == 1;
                rdLifo.IsChecked = item.StorageTypeId == 2;
                rdLote.IsChecked = item.StorageTypeId == 3;
                rdCaducidad.IsChecked = item.StorageTypeId == 4;

                // Unidades
                cmbUnidadMinima.SelectedValue = item.MinUnitId;
                cmbUnidadMedia.SelectedValue = item.MediumUnitId;
                cmbUnidadMaxima.SelectedValue = item.MaxUnitId;
                cmbPaqueteEstandar.SelectedValue = item.StandardPackage;

                txtValorUnidadMedia.Text = item.MediumUnitValue?.ToString() ?? "";
                txtValorUnidadMax.Text = item.MaxUnitValue?.ToString() ?? "";
                txtValorPaqueteEst.Text = item.StandardPackageValue?.ToString() ?? "";

                // Solicitudes
                chkSolicitarLote.IsChecked = item.RequestLotNumber;
                chkSolicitarCaducidad.IsChecked = item.RequestExpirationDate;
                chkSolicitarPedimento.IsChecked = item.RequestDeclarationNumber;
                chkSolicitarTipoCambio.IsChecked = item.RequestExchangeRate;
                chkSolicitarOC.IsChecked = item.RequestPurchaseOrder;
                chkSolicitarReferencia.IsChecked = item.RequestReference;

                // Costos / Producción
                txtCostos.Text = item.Costs?.ToString() ?? "";
                txtFactorAlmacen.Text = item.WarehouseFactor?.ToString() ?? "";
                txtFactorProduccion.Text = item.ProductionFactor?.ToString() ?? "";

                cmbEstatusProduccion.SelectedValue = item.ProductionStatusId;
                cmbUnidadProduccion.SelectedValue = item.ProductionUnitId;

                // Inventario
                chkNotificacionMax.IsChecked = item.RequestNotificationMax;
                chkNotificacionMin.IsChecked = item.RequestNotificationMin;
                txtOrden.Text = item.Reorder?.ToString() ?? "";
                txtMaximos.Text = item.Maximums?.ToString() ?? "";
                txtMinimos.Text = item.Minimus?.ToString() ?? "";
                txtTiempoEntrega.Text = item.DeliveryTime?.ToString() ?? "";

                // Dimensiones
                txtAlto.Text = item.Height?.ToString() ?? "";
                txtLargo.Text = item.Length?.ToString() ?? "";
                txtAncho.Text = item.Width?.ToString() ?? "";
                txtPeso.Text = item.Weight?.ToString() ?? "";
                cmbDimension.SelectedValue = item.DimensionerId;

                // Avanzada
              /*  cmbProveedor.SelectedValue = item.SupplierId?.ToString();
                txtNoParteProv.Text = item.SupplierPartNumber ?? string.Empty;

                rdNotificacionEmail.IsChecked = item.NotificationTypeId == 1;
                rdNotificacionArchivos.IsChecked = item.NotificationTypeId == 2;

                cmbListaDistribucion.SelectedValue = item.DistributionListId?.ToString();
                cmbRutaNotificacion.SelectedValue = item.NotificationRouteId?.ToString();
                txtCorreoAlterno.Text = item.AlternateEmail ?? string.Empty;*/
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                _cargandoDatos = false;
            }
        }

        private ProductRequest BuildRequest(string? partNumberOverride = null, string? descriptionOverride = null)
        {
            int storageType =
                rdFifo.IsChecked == true ? 1 :
                rdLifo.IsChecked == true ? 2 :
                rdLote.IsChecked == true ? 3 :
                rdCaducidad.IsChecked == true ? 4 : 0;

            return new ProductRequest
            {
                ClientId = _selectedClientId,
                ProjectId = _selectedProjectId,
                PartNumber = (partNumberOverride ?? txtNoParte.Text).Trim(),
                Description = (descriptionOverride ?? txtDescripcion.Text).Trim(),
                CategoryId = int.TryParse(cmbCategoria.SelectedValue?.ToString(), out int categoryId) ? categoryId : 0,
                FamilyId = int.TryParse(cmbFamilia.SelectedValue?.ToString(), out int familyId) ? familyId : 0,
                IsActive = chkActivo.IsChecked == true,

                IsTemperatureControlled = chkTemperatura.IsChecked ?? false,
                IsVMI = chkVMI.IsChecked ?? false,
                IsBOM = chkBOM.IsChecked ?? false,

                StorageTypeId = storageType,

                MinUnitId = cmbUnidadMinima.SelectedValue?.ToString(),
                MediumUnitId = cmbUnidadMedia.SelectedValue?.ToString(),
                MaxUnitId = cmbUnidadMaxima.SelectedValue?.ToString(),
                StandardPackage = cmbPaqueteEstandar.SelectedValue?.ToString(),

                MediumUnitValue = decimal.TryParse(txtValorUnidadMedia.Text, out decimal valorMedia) ? valorMedia : 0,
                MaxUnitValue = decimal.TryParse(txtValorUnidadMax.Text, out decimal valorMax) ? valorMax : 0,
                StandardPackageValue = decimal.TryParse(txtValorPaqueteEst.Text, out decimal valorEstandar) ? valorEstandar : 0,

                RequestLotNumber = chkSolicitarLote.IsChecked ?? false,
                RequestExpirationDate = chkSolicitarCaducidad.IsChecked ?? false,
                RequestDeclarationNumber = chkSolicitarPedimento.IsChecked ?? false,
                RequestExchangeRate = chkSolicitarTipoCambio.IsChecked ?? false,
                RequestPurchaseOrder = chkSolicitarOC.IsChecked ?? false,
                RequestReference = chkSolicitarReferencia.IsChecked ?? false,

                Costs = decimal.TryParse(txtCostos.Text, out decimal costos) ? costos : 0,
                WarehouseFactor = decimal.TryParse(txtFactorAlmacen.Text, out decimal factorAlmacen) ? factorAlmacen : 0,
                ProductionStatusId = cmbEstatusProduccion.SelectedValue?.ToString(),
                ProductionUnitId = cmbUnidadProduccion.SelectedValue?.ToString(),

                RequestNotificationMax = chkNotificacionMax.IsChecked ?? false,
                RequestNotificationMin = chkNotificacionMin.IsChecked ?? false,
                Reorder = decimal.TryParse(txtOrden.Text, out decimal reorden) ? reorden : 0,
                Maximums = decimal.TryParse(txtMaximos.Text, out decimal maximos) ? maximos : 0,
                Minimus = decimal.TryParse(txtMinimos.Text, out decimal minimos) ? minimos : 0,
                DeliveryTime = int.TryParse(txtTiempoEntrega.Text, out int tiempoEntrega) ? tiempoEntrega : 0,

                Height = decimal.TryParse(txtAlto.Text, out decimal alto) ? alto : 0,
                Weight = decimal.TryParse(txtPeso.Text, out decimal peso) ? peso : 0,
                Length = decimal.TryParse(txtLargo.Text, out decimal largo) ? largo : 0,
                Width = decimal.TryParse(txtAncho.Text, out decimal ancho) ? ancho : 0,
                DimensionerId = cmbDimension.SelectedValue?.ToString(),
            };
        }

        private Task<ApiResponseDto<string>> CreateItem(ProductRequest request) =>
            _itemService.CreateItem(request);

        private Task<ApiResponseDto<string>> EditItem(int itemId, ProductRequest request) =>
            _itemService.UpdateItem(itemId, request);

        private async Task<ApiResponseDto<string>> SaveItem(ProductRequest request)
        {
            return ItemSelected != null
                ? await EditItem(ItemSelected.ItemId, request)
                : await CreateItem(request);
        }

        private async Task<(bool IsSuccess, string Message)> SaveCargaMasivaAsync()
        {
            var registros = _cargaMasivaItems
                .Where(x => !string.IsNullOrWhiteSpace(x.PartNumber) || !string.IsNullOrWhiteSpace(x.Description))
                .ToList();

            if (registros.Count == 0)
                return (false, "No hay artículos para guardar.");

            if (ItemSelected != null)
                return (false, "La carga masiva no está disponible en modo edición.");

            var errores = new List<string>();
            var guardados = 0;

            foreach (var row in registros)
            {
                var request = BuildRequest(row.PartNumber, row.Description);
                var result = await SaveItem(request);

                if (result.IsSuccess)
                {
                    guardados++;
                    continue;
                }

                errores.Add($"{row.PartNumber} - {result.ErrorMessage ?? result.Message ?? "Hubo un error al guardar."}");
            }

            if (errores.Count > 0)
            {
                var mensaje = guardados > 0
                    ? $"Se guardaron {guardados} artículos, pero algunos fallaron:{Environment.NewLine}{string.Join(Environment.NewLine, errores)}"
                    : $"No se pudo guardar la carga masiva:{Environment.NewLine}{string.Join(Environment.NewLine, errores)}";

                return (false, mensaje);
            }

            return (true, $"Se guardaron {guardados} artículos correctamente.");
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnSave.IsEnabled = false;

                if (_cargaMasivaItems.Count > 0)
                {
                    var massiveResult = await SaveCargaMasivaAsync();
                    if (massiveResult.IsSuccess)
                    {
                        DialogHelper.ShowSuccess(massiveResult.Message);
                        ResponseForm = true;
                        DialogResult = true;
                        Close();
                    }
                    else
                    {
                        DialogHelper.ShowError(massiveResult.Message);
                    }
                }
                else
                {
                    var request = BuildRequest();
                    var result = await SaveItem(request);

                    if (result.IsSuccess)
                    {
                        DialogHelper.ShowSuccess(result.Data ?? "Guardado correctamente.");
                        ResponseForm = true;
                        DialogResult = true;
                        Close();
                    }
                    else
                    {
                        DialogHelper.ShowError(result.ErrorMessage ?? "Hubo un error al guardar.");
                    }
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

        private bool HasValidContext() => _selectedClientId > 0 && _selectedProjectId > 0;

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
