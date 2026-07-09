using LD.Client.Services;
using LD.Contracts.DTOs.ReportQueries;
using LD.Contracts.Requests;
using LD.FormsX.Features.Common;
using LD.FormsX.Features.Reportes.ViewModels;
using LD.FormsX.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LD.FormsX.Views.Reportes;

public partial class ReportesView : UserControl
{
    private readonly IServiceProvider _serviceProvider;
    private readonly WpfGridFilter<ReportQueryDto> _gridFilter;
    private bool _loaded;

    private ReportesViewModel ViewModel => (ReportesViewModel)DataContext;

    public ReportesView(ReportesViewModel viewModel, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        DataContext = viewModel;
        _serviceProvider = serviceProvider;

        _gridFilter = new WpfGridFilter<ReportQueryDto>(dgQueries, ldBuscar.TextBoxElement);
        _gridFilter.SetHiddenColumns(GetHiddenColumns(viewModel));
        _gridFilter.SetColumnOrder(nameof(ReportQueryDto.Nombre), nameof(ReportQueryDto.Query));
        _gridFilter.SetColumnWidths(new Dictionary<string, double>
        {
            { nameof(ReportQueryDto.Nombre), 300 },
            { nameof(ReportQueryDto.Query), 900 }
        });

        viewModel.OnDataLoaded += HandleDataLoaded;
    }

    private static string[] GetHiddenColumns(ReportesViewModel viewModel)
    {
        return viewModel.CanViewSql
            ? [nameof(ReportQueryDto.ReportQueryId)]
            : [nameof(ReportQueryDto.ReportQueryId), nameof(ReportQueryDto.Query)];
    }

    private void HandleDataLoaded(List<ReportQueryDto> data)
    {
        _gridFilter.SetHiddenColumns(GetHiddenColumns(ViewModel));
        _gridFilter.SetData(data);
        ViewModel.SelectedQuery = null;
        UpdateSelectionState();
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        if (_loaded)
            return;

        _loaded = true;
        UpdateSelectionState();
        await ViewModel.CargarDatosAsync();
    }

    private void DgQueries_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.SelectedQuery = _gridFilter.SelectedItem;
        UpdateSelectionState();
    }

    private async void BtnNuevo_Click(object sender, RoutedEventArgs e)
    {
        if (!ViewModel.CanCreate)
            return;

        var dialog = _serviceProvider.GetRequiredService<ReportQueryEditorView>();
        WindowOwnerHelper.AttachOwnerOrCenter(dialog, WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));
        dialog.SetQuery(null);

        if (dialog.ShowDialog() == true)
            await ViewModel.CargarDatosAsync();
    }

    private async void BtnEditar_Click(object sender, RoutedEventArgs e)
    {
        var selectedQuery = GetSelectedQueryOrWarn("editar");
        if (selectedQuery is null)
            return;

        var dialog = _serviceProvider.GetRequiredService<ReportQueryEditorView>();
        WindowOwnerHelper.AttachOwnerOrCenter(dialog, WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));
        dialog.SetQuery(selectedQuery);

        if (dialog.ShowDialog() == true)
            await ViewModel.CargarDatosAsync();
    }

    private async void BtnEliminar_Click(object sender, RoutedEventArgs e)
    {
        var selectedQuery = GetSelectedQueryOrWarn("eliminar");
        if (selectedQuery is null)
            return;

        if (!DialogHelper.ShowConfirm(
                $"¿Deseas eliminar la consulta \"{selectedQuery.Nombre}\"?",
                "Eliminar consulta"))
        {
            return;
        }

        var service = _serviceProvider.GetRequiredService<ReportQueryService>();
        var response = await service.DeleteReportQueryAsync(selectedQuery.ReportQueryId);
        if (!response.IsSuccess)
        {
            DialogHelper.ShowError(response.ErrorMessage ?? response.Message ?? "No se pudo eliminar la consulta.");
            return;
        }

        DialogHelper.ShowSuccess(response.Data ?? response.Message ?? "Consulta eliminada correctamente.");
        await ViewModel.CargarDatosAsync();
    }

    private async void BtnEjecutar_Click(object sender, RoutedEventArgs e)
    {
        var selectedQuery = GetSelectedQueryOrWarn("ejecutar");
        if (selectedQuery is null)
            return;

        await ExecuteQueryAsync(selectedQuery);
    }

    private async void BtnActualizar_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.CargarDatosAsync();
    }

    private async Task ExecuteQueryAsync(ReportQueryDto query)
    {
        var service = _serviceProvider.GetRequiredService<ReportQueryService>();

        var parameterResponse = await service.GetReportQueryParametersAsync(query.ReportQueryId);
        if (!parameterResponse.IsSuccess || parameterResponse.Data is null)
        {
            DialogHelper.ShowWarning(parameterResponse.ErrorMessage ?? parameterResponse.Message ?? "No se pudieron cargar los parámetros.");
            return;
        }

        var parameters = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

        if (parameterResponse.Data.Count > 0)
        {
            var parametersDialog = _serviceProvider.GetRequiredService<ReportQueryParametersView>();
            WindowOwnerHelper.AttachOwnerOrCenter(parametersDialog, WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));
            parametersDialog.SetReportQuery(query);
            parametersDialog.SetParameters(parameterResponse.Data);

            if (parametersDialog.ShowDialog() != true)
                return;

            parameters = new Dictionary<string, string?>(parametersDialog.ResultParameters, StringComparer.OrdinalIgnoreCase);
        }

        var executionResponse = await service.ExecuteReportQueryAsync(query.ReportQueryId, new ReportQueryExecutionRequest
        {
            Parameters = parameters
        });

        if (!executionResponse.IsSuccess || executionResponse.Data is null)
        {
            DialogHelper.ShowError(executionResponse.ErrorMessage ?? executionResponse.Message ?? "No se pudo ejecutar la consulta.");
            return;
        }

        var resultDialog = _serviceProvider.GetRequiredService<ReportQueryResultView>();
        WindowOwnerHelper.AttachOwnerOrCenter(resultDialog, WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));
        resultDialog.SetResult(executionResponse.Data);
        resultDialog.ShowDialog();
    }

    private ReportQueryDto? GetSelectedQueryOrWarn(string actionName)
    {
        if (ViewModel.SelectedQuery is null)
        {
            DialogHelper.ShowWarning($"Selecciona una consulta para {actionName}.");
            return null;
        }

        return ViewModel.SelectedQuery;
    }

    private void UpdateSelectionState()
    {
        var hasSelection = ViewModel.SelectedQuery is not null;

        btnEditar.IsEnabled = hasSelection;
        btnEliminar.IsEnabled = hasSelection;
        btnEjecutar.IsEnabled = hasSelection;
    }
}
