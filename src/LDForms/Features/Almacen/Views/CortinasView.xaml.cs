using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LD.Contracts.DTOs.Security;
using LD.Contracts.Warehouse;
using LD.FormsX.Features.Almacen.ViewModels;
using LD.FormsX.Features.Common;
using LD.FormsX.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace LD.FormsX.Views.Almacen;

public partial class CortinasView : Window
{
    private readonly IServiceProvider _serviceProvider;
    private readonly WpfGridFilter<CortinaDto> _gridFilter;
    private bool _loaded;

    private CortinasViewModel ViewModel => (CortinasViewModel)DataContext;

    public CortinasView(CortinasViewModel viewModel, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        DataContext = viewModel;
        _serviceProvider = serviceProvider;

        _gridFilter = new WpfGridFilter<CortinaDto>(dgCortinas, ldBuscar.TextBoxElement);
        _gridFilter.SetHiddenColumns(nameof(CortinaDto.WarehouseId));
        _gridFilter.SetColumnOrder(
            nameof(CortinaDto.CortinaId),
            nameof(CortinaDto.Numero),
            nameof(CortinaDto.Descripcion),
            nameof(CortinaDto.EstaDisponible),
            nameof(CortinaDto.WarehouseId));
        _gridFilter.SetColumnWidths(new Dictionary<string, double>
        {
            { nameof(CortinaDto.CortinaId), 110 },
            { nameof(CortinaDto.Numero), 130 },
            { nameof(CortinaDto.Descripcion), 360 },
            { nameof(CortinaDto.EstaDisponible), 120 },
            { nameof(CortinaDto.WarehouseId), 120 }
        });

        viewModel.OnDataLoaded += data => _gridFilter.SetData(data);
    }

    public void SetWarehouse(WarehouseDto? warehouse) => ViewModel.SetWarehouse(warehouse);

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        if (_loaded)
            return;

        _loaded = true;
        await ViewModel.CargarDatosAsync();
    }

    private void dgCortinas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        => ViewModel.SelectedCortina = _gridFilter.SelectedItem;

    private async void BtnNuevo_Click(object sender, RoutedEventArgs e)
    {
        var dialog = _serviceProvider.GetRequiredService<NuevaCortinaView>();
        WindowOwnerHelper.AttachOwnerOrCenter(dialog, WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));
        dialog.SetWarehouse(ViewModel.SelectedWarehouse);

        if (dialog.ShowDialog() == true)
            await ViewModel.CargarDatosAsync();
    }

    private async void BtnEditar_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedCortina is null)
        {
            DialogHelper.ShowWarning("Selecciona una cortina para editarla.");
            return;
        }

        var dialog = _serviceProvider.GetRequiredService<NuevaCortinaView>();
        WindowOwnerHelper.AttachOwnerOrCenter(dialog, WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));
        dialog.SetWarehouse(ViewModel.SelectedWarehouse);
        dialog.SetCortina(ViewModel.SelectedCortina);

        if (dialog.ShowDialog() == true)
            await ViewModel.CargarDatosAsync();
    }

    private async void BtnEliminar_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedCortina is null)
        {
            DialogHelper.ShowWarning("Selecciona una cortina para eliminarla.");
            return;
        }

        if (!DialogHelper.ShowConfirm($"Eliminar la cortina {ViewModel.SelectedCortina.Numero}?"))
            return;

        if (await ViewModel.DeleteSelectedAsync())
            await ViewModel.CargarDatosAsync();
    }

    private void BtnClose_Click(object sender, RoutedEventArgs e) => Close();

    private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
            DragMove();
    }
}
