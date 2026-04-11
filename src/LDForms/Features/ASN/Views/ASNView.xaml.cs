using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LD.Client.Services;
using LD.Contracts.ASN;
using LD.FormsX.Helpers;
using LD.FormsX.Views.Dialogs;
using LD.FormsX.Views.Familias;
using LD.FormsX.Views.Ubicaciones;
using Microsoft.Extensions.DependencyInjection;

namespace LD.FormsX.Views.ASN
{
    /// <summary>
    /// Lógica de interacción para ASNView.xaml
    /// </summary>
    public partial class ASNView : UserControl
    {
        private readonly AsnService _asnService;
        private readonly AsnDetailService _asnDetailService;
        private readonly AsnReceiptService _asnReceiptService;
        private readonly IServiceProvider _serviceProvider;

        private readonly WpfGridFilter<AsnDto> _gridFilter;
        private readonly WpfGridFilter<AsnDetailDto> _gridFilterDet;
        private readonly WpfGridFilter<AsnReceiptDetailDto> _gridFilterRec;

        private AsnDto? _selectedX;
        private AsnDetailDto? _selectedDetail;
        private bool _loaded;

        public ASNView(
            AsnService asnService,
            AsnDetailService asnDetailService,
            AsnReceiptService asnReceiptService,
            IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _asnService = asnService;
            _asnDetailService = asnDetailService;
            _asnReceiptService = asnReceiptService;
            _serviceProvider = serviceProvider;

            _gridFilter = new WpfGridFilter<AsnDto>(dgASN);
            _gridFilterDet = new WpfGridFilter<AsnDetailDto>(dgDetalleASN);
            _gridFilterRec = new WpfGridFilter<AsnReceiptDetailDto>(dgRecepcionASN);
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loaded) return;
            _loaded = true;

            await CargarDatosConLoaderAsync("Trayendo ASN...");
        }

        private async Task CargarDatosConLoaderAsync(string mensaje)
        {
            try
            {
                MostrarLoader(true, mensaje);
                await CargarDatosAsync();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                MostrarLoader(false);
            }
        }

        private void MostrarLoader(bool mostrar, string mensaje = "Cargando...")
        {
            TxtLoadingASN.Text = mensaje;
            LoadingOverlayASN.Visibility = mostrar ? Visibility.Visible : Visibility.Collapsed;
        }

        private async Task CargarDatosAsync()
        {
            var result = await _asnService.GetAsn();

            if (!result.IsSuccess)
            {
                DialogHelper.ShowWarning(result.Message);
                return;
            }

            _gridFilter.SetData(result.Data);
            _selectedX = null;
            _selectedDetail = null;
            _gridFilterDet.SetData(null);
            _gridFilterRec.SetData(null);
            txtStatusDetalle.Text = "Sin detalle para mostrar";
            txtStatusRecepcion.Text = "Sin partidas recibidas";
        }


        private async Task CargarDatosAsyncDet()
        {
            _selectedDetail = null;
            _gridFilterRec.SetData(null);
            txtStatusRecepcion.Text = "Sin partidas recibidas";

            if (_selectedX == null)
            {
                _gridFilterDet.SetData(null);
                txtStatusDetalle.Text = "Sin detalle para mostrar";
                return;
            }

            var result = await _asnDetailService.GetAsnDetailsByAsn(_selectedX.AsnId);

            if (!result.IsSuccess || result.Data == null)
            {
                _gridFilterDet.SetData(null);
                txtStatusDetalle.Text = "Sin detalle para mostrar";
                return;
            }

            _gridFilterDet.SetData(result.Data);
            txtStatusDetalle.Text = $"Registros: {result.Data?.Count ?? 0}";
        }

        private async Task CargarDatosAsyncRecepcion()
        {
            if (_selectedDetail == null || _selectedDetail.AsnDetailId <= 0)
            {
                _gridFilterRec.SetData(null);
                txtStatusRecepcion.Text = "Sin partidas recibidas";
                return;
            }

            var result = await _asnReceiptService.GetAsnReceipts();
            if (!result.IsSuccess || result.Data == null)
            {
                _gridFilterRec.SetData(null);
                txtStatusRecepcion.Text = "Sin partidas recibidas";
                return;
            }

            var filtered = result.Data
                .Where(x => x.AsnDetailId == _selectedDetail.AsnDetailId)
                .ToList();

            _gridFilterRec.SetData(filtered);
            txtStatusRecepcion.Text = filtered.Count > 0
                ? $"Registros: {filtered.Count}"
                : "Sin partidas recibidas";
        }

        private async void BtnActualizar_Click(object sender, RoutedEventArgs e)
        {
            await CargarDatosConLoaderAsync("Trayendo ASN...");
        }

        private void BtnNuevoASN_Click(object sender, RoutedEventArgs e)
        {
            var dialog = _serviceProvider.GetRequiredService<NuevoASNView>();
            dialog.Owner = Window.GetWindow(this);

            var result = dialog.ShowDialog();

            if (result == true)
            {
                // recargar datos
                _ = CargarDatosAsync();
            }
        }
        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedX is null)
                return;

            var dialog = _serviceProvider.GetRequiredService<NuevoASNView>();
            dialog.Owner = Window.GetWindow(this);
            dialog.SetAsn(_selectedX);

            var result = dialog.ShowDialog();

            if (result == true)
            {
                CargarDatosConLoaderAsync("Trayendo asns...");
            }
        }

        private void BtnRegistrarArribo_Click(object sender, RoutedEventArgs e) { }

        private void BtnEscanear_Click(object sender, RoutedEventArgs e) { }

        private async void dgASN_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _selectedX = _gridFilter.SelectedItem;
                await CargarDatosAsyncDet();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async void dgDetalleASN_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _selectedDetail = _gridFilterDet.SelectedItem;
                await CargarDatosAsyncRecepcion();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

    }
}
