using System.Windows;
using System.Windows.Controls;
using LD.FormsX.Features.Surtidos.ViewModels;
using LD.FormsX.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;

namespace LD.FormsX.Features.Surtidos.Views;

public partial class CapturaFoliosView : UserControl
{
    private readonly CapturaFoliosViewModel _viewModel;
    private bool _loaded;

    public CapturaFoliosView(CapturaFoliosViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        if (_loaded)
            return;

        _loaded = true;
        await _viewModel.LoadAsync();
    }

    private async void BtnGenerar_Click(object sender, RoutedEventArgs e)
    {
        if (!_viewModel.CanGenerate)
        {
            DialogHelper.ShowWarning("No tienes permiso para generar Kitting.", "Permiso requerido");
            return;
        }

        var dialog = new OpenFileDialog
        {
            Title = "Selecciona el archivo de texto",
            Filter = "Archivos de texto (*.6;*.txt)|*.6;*.txt|Todos los archivos (*.*)|*.*"
        };

        if (dialog.ShowDialog() != true)
            return;

        await _viewModel.GenerateFromFileAsync(dialog.FileName);
    }

    private void LookupCliente_SelectionConfirmed(object sender, RoutedEventArgs e)
    {
        if (sender is not LD.FormsX.Features.Common.InlineLookupEditor editor || editor.SelectedLookupItem is not LD.FormsX.Model.Lookup.LookupItem lookupItem)
            return;

        if (lookupItem.Data is not LD.Contracts.DTOs.DropDownDto selectedClient)
            return;

        _viewModel.SelectedClientId = int.TryParse(selectedClient.Key, out var clientId) ? clientId : 0;
        _viewModel.SelectedClientText = selectedClient.Value ?? string.Empty;
    }

    private void LookupProyecto_SelectionConfirmed(object sender, RoutedEventArgs e)
    {
        if (sender is not LD.FormsX.Features.Common.InlineLookupEditor editor || editor.SelectedLookupItem is not LD.FormsX.Model.Lookup.LookupItem lookupItem)
            return;

        if (lookupItem.Data is not LD.Contracts.DTOs.DropDownDto selectedProject)
            return;

        _viewModel.SelectedProjectId = int.TryParse(selectedProject.Key, out var projectId) ? projectId : 0;
        _viewModel.SelectedProjectText = selectedProject.Value ?? string.Empty;
    }
}
