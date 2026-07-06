using LD.FormsX.Features.ControlPatio.ViewModels;
using LD.FormsX.Helpers;
using System.Windows;
using System.Windows.Controls;

namespace LD.FormsX.Views.ControlPatio;

public partial class ControlPatioView : UserControl
{
    private readonly ControlPatioViewModel _viewModel;
    private readonly DataGridColumnFilterManager _patioGridManager;
    private bool _loaded;

    public ControlPatioView(ControlPatioViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = viewModel;
        _patioGridManager = new DataGridColumnFilterManager(dgPatioUnits);
        _patioGridManager.ApplyTo(_viewModel.PatioRowsView);
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        if (_loaded)
        {
            return;
        }

        _loaded = true;
        await _viewModel.CargarDatosAsync();
    }
}
