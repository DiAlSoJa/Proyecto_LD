using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using LD.Contracts.Requests;
using LD.Contracts.TruckType;
using LD.FormsX.Features.Catalogos.TiposCamion.ViewModels;
using LD.FormsX.Helpers;

namespace LD.FormsX.Views.TiposCamion;

public partial class NuevoTipoCamionView : Window
{
    private NuevoTipoCamionViewModel ViewModel => (NuevoTipoCamionViewModel)DataContext;

    public bool ResponseForm => ViewModel.ResponseForm;

    public NuevoTipoCamionView(NuevoTipoCamionViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;

        viewModel.RequestClose = () =>
        {
            DialogResult = true;
            Close();
        };
    }

    public void SetTruckType(TruckTypeDto truckType)
    {
        ViewModel.SetTruckType(truckType);
    }

    protected override async void OnContentRendered(EventArgs e)
    {
        base.OnContentRendered(e);
        if (ViewModel.SelectedTruckType is not null)
            await CargarDatosAsync();
    }

    private async Task CargarDatosAsync()
    {
        var truckType = await ViewModel.GetTruckTypeAsync();
        if (truckType is null)
            return;

        txtNombre.Text = truckType.Name;
        chkTieneCaja.IsChecked = truckType.TieneCaja;
    }

    private TruckTypeRequest BuildRequest()
    {
        return new TruckTypeRequest
        {
            TruckTypeId = ViewModel.SelectedTruckType?.TruckTypeId ?? 0,
            Name = txtNombre.Text.Trim(),
            TieneCaja = chkTieneCaja.IsChecked == true
        };
    }

    private async void btnSave_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            btnSave.IsEnabled = false;

            var request = BuildRequest();
            await ViewModel.SaveAsync(request);
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
