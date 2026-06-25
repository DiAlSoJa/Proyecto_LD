using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using LD.Contracts.Client;
using LD.FormsX.Features.Clientes.ViewModels;
using LD.FormsX.Helpers;
using LD.FormsX.Views.Dialogs;
using Microsoft.Extensions.DependencyInjection;

namespace LD.FormsX.Views
{
    public partial class CatalogosClientesView : UserControl
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly WpfGridFilter<ClientDto> _gridFilter;
        private bool _loaded;

        private ClientesViewModel ViewModel => (ClientesViewModel)DataContext;

        public CatalogosClientesView(ClientesViewModel viewModel, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            DataContext = viewModel;
            _serviceProvider = serviceProvider;

            _gridFilter = new WpfGridFilter<ClientDto>(dgClientes, ldBuscar.TextBoxElement);
            _gridFilter.SetColumnWidths(new Dictionary<string, double>
            {
                { "Activo", 80 },
                { "Id", 70 },
                { "NombreComercial", 220 },
                { "DomicilioComercial", 250 },
                { "Telefono", 140 },
                { "Ciudad", 140 },
                { "CodigoPostal", 120 },
                { "RazonSocial", 250 },
                { "Rfc", 150 },
                { "IsProvider", 120 }
            });

            _gridFilter.SetColumnOrder(
                "Activo",
                "Id",
                "NombreComercial",
                "DomicilioComercial",
                "Telefono",
                "Ciudad",
                "CodigoPostal",
                "RazonSocial",
                "Rfc",
                "IsProvider"
            );

            viewModel.OnDataLoaded += data => _gridFilter.SetData(data);
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loaded) return;
            _loaded = true;

            await ViewModel.CargarDatosAsync();
        }

        private void DgClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SelectedClient = _gridFilter.SelectedItem;
        }

        private async void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = _serviceProvider.GetRequiredService<NuevoClienteView>();
            LD.FormsX.Features.Common.WindowOwnerHelper.AttachOwnerOrCenter(dialog, LD.FormsX.Features.Common.WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));

            if (dialog.ShowDialog() == true)
                await ViewModel.CargarDatosAsync();
        }

        private async void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            var dialog = _serviceProvider.GetRequiredService<NuevoClienteView>();
            LD.FormsX.Features.Common.WindowOwnerHelper.AttachOwnerOrCenter(dialog, LD.FormsX.Features.Common.WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));
            dialog.SetClient(ViewModel.SelectedClient);

            if (dialog.ShowDialog() == true)
                await ViewModel.CargarDatosAsync();
        }
    }
}
