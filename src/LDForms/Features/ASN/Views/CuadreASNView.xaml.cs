using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using LD.Client.Services;
using LD.Contracts.ASN;
using LD.FormsX.Helpers;

namespace LD.FormsX.Views.Dialogs
{
    public partial class CuadreASNView : Window, INotifyPropertyChanged
    {
        private readonly AsnDetailService _asnDetailService;
        private readonly AsnReceiptService _asnReceiptService;
        private readonly WpfGridFilter<AsnReceiptDetailDto> _gridFilter;
        private AsnDto? _asn;
        private bool _loaded;
        private string _asnHeaderText = "Cuadre ASN";
        private string _asnMetaText = "Sin ASN seleccionado.";
        private string _summaryText = "Sin datos para mostrar.";
        private string _statusText = "Listo para cargar.";

        public event PropertyChangedEventHandler? PropertyChanged;

        public string AsnHeaderText
        {
            get => _asnHeaderText;
            private set
            {
                if (_asnHeaderText == value)
                    return;

                _asnHeaderText = value;
                OnPropertyChanged(nameof(AsnHeaderText));
            }
        }

        public string AsnMetaText
        {
            get => _asnMetaText;
            private set
            {
                if (_asnMetaText == value)
                    return;

                _asnMetaText = value;
                OnPropertyChanged(nameof(AsnMetaText));
            }
        }

        public string SummaryText
        {
            get => _summaryText;
            private set
            {
                if (_summaryText == value)
                    return;

                _summaryText = value;
                OnPropertyChanged(nameof(SummaryText));
            }
        }

        public string StatusText
        {
            get => _statusText;
            private set
            {
                if (_statusText == value)
                    return;

                _statusText = value;
                OnPropertyChanged(nameof(StatusText));
            }
        }

        public CuadreASNView(
            AsnDetailService asnDetailService,
            AsnReceiptService asnReceiptService)
        {
            InitializeComponent();

            _asnDetailService = asnDetailService;
            _asnReceiptService = asnReceiptService;
            _gridFilter = new WpfGridFilter<AsnReceiptDetailDto>(dgCuadre);

            DataContext = this;

            _gridFilter.SetHiddenColumns("AsnReceiptDetailId", "AsnDetailId", "ProductId", "LocationId");
            _gridFilter.SetColumnOrder(
                "PalletNumber",
                "StandardId",
                "PartNumber",
                "Description",
                "StandardQuantity",
                "MaximumQuantity",
                "ReceivedQuantity",
                "SD",
                "Status",
                "LocationCode",
                "LotNumber",
                "ExpirationDate",
                "Reference",
                "PurchaseOrder",
                "CustomsDeclarationNumber");
            _gridFilter.SetColumnWidths(new Dictionary<string, double>
            {
                ["PalletNumber"] = 90,
                ["StandardId"] = 120,
                ["PartNumber"] = 150,
                ["Description"] = 240,
                ["StandardQuantity"] = 95,
                ["MaximumQuantity"] = 95,
                ["ReceivedQuantity"] = 100,
                ["SD"] = 70,
                ["Status"] = 95,
                ["LocationCode"] = 120,
                ["LotNumber"] = 130,
                ["ExpirationDate"] = 130,
                ["Reference"] = 140,
                ["PurchaseOrder"] = 140,
                ["CustomsDeclarationNumber"] = 150
            });
        }

        public void SetAsn(AsnDto? asn)
        {
            _asn = asn;

            if (_asn == null)
            {
                AsnHeaderText = "Cuadre ASN";
                AsnMetaText = "Sin ASN seleccionado.";
                SummaryText = "Sin datos para mostrar.";
                StatusText = "Sin ASN seleccionado.";
                return;
            }

            var asnCode = string.IsNullOrWhiteSpace(_asn.AsnCode)
                ? $"ASN {_asn.AsnId}"
                : _asn.AsnCode.Trim();

            AsnHeaderText = asnCode;
            AsnMetaText = $"Cliente: {FormatValue(_asn.Client)} | Proyecto: {FormatValue(_asn.Project)}";
            SummaryText = "Preparando informacion del cuadre...";
            StatusText = "Preparando informacion del cuadre...";
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loaded)
                return;

            _loaded = true;
            await LoadCuadreAsync();
        }

        private async Task LoadCuadreAsync()
        {
            if (_asn == null || _asn.AsnId <= 0)
            {
                _gridFilter.SetData(Array.Empty<AsnReceiptDetailDto>());
                SummaryText = "Sin datos para mostrar.";
                StatusText = "Selecciona un ASN valido antes de abrir el cuadre.";
                return;
            }

            try
            {
                SetLoading(true, "Cargando cuadre...");

                var detailsResult = await _asnDetailService.GetAsnDetailsByAsn(_asn.AsnId);
                if (!detailsResult.IsSuccess || detailsResult.Data == null || detailsResult.Data.Count == 0)
                {
                    _gridFilter.SetData(Array.Empty<AsnReceiptDetailDto>());
                    SummaryText = "No se encontraron lineas de receipt para este ASN.";
                    StatusText = detailsResult.Message ?? detailsResult.ErrorMessage ?? "El ASN no tiene lineas para mostrar.";
                    return;
                }

                var receiptDetails = new List<AsnReceiptDetailDto>();
                foreach (var detail in detailsResult.Data.OrderBy(x => x.AsnDetailId))
                {
                    var receiptResult = await _asnReceiptService.GetAsnReceiptsByAsnDetailId(detail.AsnDetailId);
                    if (receiptResult.IsSuccess && receiptResult.Data != null)
                        receiptDetails.AddRange(receiptResult.Data);
                }

                var ordered = receiptDetails
                    .OrderBy(x => x.PalletNumber > 0 ? x.PalletNumber : int.MaxValue)
                    .ThenBy(x => x.StandardId)
                    .ThenBy(x => x.AsnReceiptDetailId)
                    .ToList();

                _gridFilter.SetData(ordered);

                var totalReceived = ordered.Sum(x => x.ReceivedQuantity ?? 0m);
                SummaryText = ordered.Count > 0
                    ? $"{ordered.Count} linea(s) | Total recibido: {totalReceived:N2}"
                    : "No se encontraron lineas de receipt para este ASN.";
                StatusText = ordered.Count > 0
                    ? "Cuadre cargado correctamente."
                    : "No se encontraron lineas de receipt para este ASN.";
            }
            catch (Exception ex)
            {
                _gridFilter.SetData(Array.Empty<AsnReceiptDetailDto>());
                SummaryText = "No se pudo cargar el cuadre.";
                StatusText = "Ocurrio un error al cargar el cuadre.";
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                SetLoading(false);
            }
        }

        private void SetLoading(bool loading, string message = "")
        {
            loadingOverlay.Visibility = loading ? Visibility.Visible : Visibility.Collapsed;
            txtLoading.Text = message;
            btnClose.IsEnabled = !loading;
            btnFooterClose.IsEnabled = !loading;
            dgCuadre.IsEnabled = !loading;

            if (!string.IsNullOrWhiteSpace(message))
                StatusText = message;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }

        private static string FormatValue(string? value) =>
            string.IsNullOrWhiteSpace(value) ? "-" : value.Trim();

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
