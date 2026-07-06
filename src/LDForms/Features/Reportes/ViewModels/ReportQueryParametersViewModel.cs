using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Services;
using LD.Contracts.DTOs.ReportQueries;
using LD.FormsX.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace LD.FormsX.Features.Reportes.ViewModels;

public partial class ReportQueryParametersViewModel : ObservableObject
{
    private readonly ReportQueryService _reportQueryService;

    public Action? RequestClose { get; set; }

    public int ReportQueryId { get; private set; }

    public string QueryName { get; private set; } = string.Empty;

    public Dictionary<string, string?> ResultParameters { get; private set; } = new(StringComparer.OrdinalIgnoreCase);

    public ObservableCollection<ReportQueryParameterInputViewModel> Parameters { get; } = new();

    private bool ParametersLoaded { get; set; }

    [ObservableProperty]
    private string headerTitle = "Parámetros de consulta";

    [ObservableProperty]
    private string statusText = string.Empty;

    [ObservableProperty]
    private bool isBusy;

    public ReportQueryParametersViewModel(ReportQueryService reportQueryService)
    {
        _reportQueryService = reportQueryService;
    }

    public void SetReportQuery(ReportQueryDto reportQuery)
    {
        ReportQueryId = reportQuery.ReportQueryId;
        QueryName = reportQuery.Nombre;
        HeaderTitle = $"Parámetros - {reportQuery.Nombre}";
        ParametersLoaded = false;
    }

    public void SetParameters(IEnumerable<ReportQueryParameterDto> parameters)
    {
        Parameters.Clear();

        foreach (var parameter in parameters ?? Enumerable.Empty<ReportQueryParameterDto>())
        {
            if (string.IsNullOrWhiteSpace(parameter.Nombre))
                continue;

            Parameters.Add(new ReportQueryParameterInputViewModel(parameter.Nombre.Trim()));
        }

        ParametersLoaded = true;
        StatusText = Parameters.Count == 0
            ? "La consulta no requiere parámetros."
            : $"Ingresa {Parameters.Count} parámetro(s).";
    }

    public async Task LoadAsync()
    {
        try
        {
            if (ParametersLoaded)
                return;

            IsBusy = true;
            StatusText = "Cargando parámetros...";

            Parameters.Clear();

            var response = await _reportQueryService.GetReportQueryParametersAsync(ReportQueryId);
            if (!response.IsSuccess || response.Data is null)
            {
                StatusText = response.Message ?? "No se pudieron cargar los parámetros.";
                DialogHelper.ShowWarning(StatusText);
                RequestClose?.Invoke();
                return;
            }

            SetParameters(response.Data);
        }
        catch (Exception ex)
        {
            StatusText = "Ocurrió un error al cargar los parámetros.";
            DialogHelper.ShowError(ex.Message);
            RequestClose?.Invoke();
        }
        finally
        {
            IsBusy = false;
        }
    }

    public void Confirm()
    {
        ResultParameters = Parameters.ToDictionary(
            x => x.Nombre,
            x => string.IsNullOrWhiteSpace(x.Valor) ? null : x.Valor.Trim(),
            StringComparer.OrdinalIgnoreCase);

        RequestClose?.Invoke();
    }
}

public partial class ReportQueryParameterInputViewModel : ObservableObject
{
    public ReportQueryParameterInputViewModel(string nombre)
    {
        Nombre = nombre;
    }

    [ObservableProperty]
    private string nombre = string.Empty;

    [ObservableProperty]
    private string valor = string.Empty;

    public string DisplayName => string.IsNullOrWhiteSpace(Nombre) ? string.Empty : $"@{Nombre}";

    partial void OnNombreChanged(string value)
    {
        OnPropertyChanged(nameof(DisplayName));
    }
}
