using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.Constants;
using LD.Contracts.DTOs.Security;
using LD.Contracts.Enums;
using LD.FormsX.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace LD.FormsX.Features.ControlPatio.ViewModels;

public partial class ControlPatioViewModel : ObservableObject
{
    private const string SequenceAllValue = "All";
    private readonly PatioClientService _patioClientService;
    private bool _suspendFilterRefresh;

    [ObservableProperty]
    private bool canView;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string loadingMessage = "Cargando monitor de patio...";

    [ObservableProperty]
    private string statusText = "Sin datos cargados";

    [ObservableProperty]
    private string lastRefreshText = "Sin actualización";

    [ObservableProperty]
    private string searchText = string.Empty;

    [ObservableProperty]
    private string selectedSequenceFilterValue = SequenceAllValue;

    [ObservableProperty]
    private bool onlyCritical;

    [ObservableProperty]
    private PatioMonitorDto? selectedRegistration;

    [ObservableProperty]
    private int activeCount;

    [ObservableProperty]
    private int visibleCount;

    [ObservableProperty]
    private int criticalCount;

    [ObservableProperty]
    private int pendingMovementsCount;

    [ObservableProperty]
    private int cajaCount;

    [ObservableProperty]
    private int tractoCount;

    [ObservableProperty]
    private int averageMinutesInPatio;

    public ObservableCollection<PatioMonitorDto> PatioRows { get; } = new();

    public ICollectionView PatioRowsView { get; }

    public ObservableCollection<PatioMonitorFilterOption> SequenceFilters { get; } = new()
    {
        new PatioMonitorFilterOption("Todas", SequenceAllValue),
        new PatioMonitorFilterOption("Caja", PatioMonitorSequence_e.Caja.ToString()),
        new PatioMonitorFilterOption("Tracto", PatioMonitorSequence_e.Tracto.ToString()),
        new PatioMonitorFilterOption("Sin definir", PatioMonitorSequence_e.Desconocido.ToString())
    };

    public IReadOnlyList<PatioMonitorStepDto> SelectedSteps => SelectedRegistration?.Steps ?? new List<PatioMonitorStepDto>();

    public ControlPatioViewModel(PatioClientService patioClientService)
    {
        _patioClientService = patioClientService;

        CanView = UserData.HasPermission(PermissionKeys.YardControl_View)
            || UserData.HasPermission(PermissionKeys.Security_View);

        PatioRowsView = CollectionViewSource.GetDefaultView(PatioRows);
        PatioRowsView.Filter = FilterRow;

        if (!CanView)
        {
            StatusText = "No tienes permiso para ver control de patio.";
            LastRefreshText = "Acceso restringido";
        }
    }

    [RelayCommand]
    public async Task CargarDatosAsync()
    {
        if (!CanView)
        {
            return;
        }

        try
        {
            IsLoading = true;
            LoadingMessage = "Cargando monitor de patio...";

            var result = await _patioClientService.GetPatioMonitorAsync();
            if (!result.IsSuccess)
            {
                DialogHelper.ShowWarning(result.ErrorMessage ?? result.Message ?? "No se pudo cargar el monitor de patio.");
                return;
            }

            PatioRows.Clear();
            foreach (var row in result.Data ?? new List<PatioMonitorDto>())
            {
                PatioRows.Add(row);
            }

            LastRefreshText = $"Actualizado {DateTime.Now:dd/MM/yyyy HH:mm}";
            ApplyFilters(ensureSelection: true);
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void LimpiarFiltros()
    {
        _suspendFilterRefresh = true;
        SearchText = string.Empty;
        SelectedSequenceFilterValue = SequenceAllValue;
        OnlyCritical = false;
        _suspendFilterRefresh = false;

        ApplyFilters(ensureSelection: true);
    }

    partial void OnSearchTextChanged(string value) => ApplyFilters(ensureSelection: true);

    partial void OnSelectedSequenceFilterValueChanged(string value) => ApplyFilters(ensureSelection: true);

    partial void OnOnlyCriticalChanged(bool value) => ApplyFilters(ensureSelection: true);

    partial void OnSelectedRegistrationChanged(PatioMonitorDto? value)
    {
        OnPropertyChanged(nameof(SelectedSteps));
    }

    private void ApplyFilters(bool ensureSelection = false)
    {
        if (_suspendFilterRefresh)
        {
            return;
        }

        PatioRowsView.Refresh();

        var visibleRows = PatioRowsView.Cast<PatioMonitorDto>().ToList();
        ActiveCount = PatioRows.Count;
        VisibleCount = visibleRows.Count;
        CriticalCount = visibleRows.Count(x => x.IsCritical);
        PendingMovementsCount = visibleRows.Sum(x => x.PendingSteps);
        CajaCount = visibleRows.Count(x => x.SequenceType == PatioMonitorSequence_e.Caja);
        TractoCount = visibleRows.Count(x => x.SequenceType == PatioMonitorSequence_e.Tracto);
        AverageMinutesInPatio = visibleRows.Count == 0
            ? 0
            : (int)Math.Round(visibleRows.Average(x => x.MinutesInPatio));

        StatusText = ActiveCount == 0
            ? "Sin unidades activas en patio"
            : VisibleCount == ActiveCount
                ? $"{ActiveCount:N0} unidades activas"
                : $"Mostrando {VisibleCount:N0} de {ActiveCount:N0} unidades activas";

        if (!ensureSelection)
        {
            return;
        }

        if (SelectedRegistration is not null && visibleRows.Contains(SelectedRegistration))
        {
            return;
        }

        SelectedRegistration = visibleRows.FirstOrDefault();
    }

    private bool FilterRow(object obj)
    {
        if (obj is not PatioMonitorDto row)
        {
            return false;
        }

        if (OnlyCritical && !row.IsCritical)
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(SelectedSequenceFilterValue)
            && !string.Equals(SelectedSequenceFilterValue, SequenceAllValue, StringComparison.OrdinalIgnoreCase)
            && !string.Equals(row.SequenceType.ToString(), SelectedSequenceFilterValue, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(SearchText))
        {
            return true;
        }

        var query = SearchText.Trim();

        return Contains(row.Tipo, query)
            || Contains(row.TipoVehiculo, query)
            || Contains(row.SequenceText, query)
            || Contains(row.Nombre, query)
            || Contains(row.Licencia, query)
            || Contains(row.Celular, query)
            || Contains(row.Linea, query)
            || Contains(row.Origen, query)
            || Contains(row.Numero, query)
            || Contains(row.Placa, query)
            || Contains(row.CurrentStep, query)
            || Contains(row.NextStep, query)
            || Contains(row.CurrentArea, query)
            || Contains(row.StatusBadgeText, query)
            || Contains(row.StatusText, query)
            || Contains(row.AlertText, query)
            || Contains(row.CortinaNumero ?? string.Empty, query)
            || row.Steps.Any(step =>
                Contains(step.Label, query)
                || Contains(step.Area, query)
                || Contains(step.StateText, query)
                || Contains(step.Note ?? string.Empty, query));
    }

    private static bool Contains(string? source, string value)
        => !string.IsNullOrWhiteSpace(source)
           && source.Contains(value, StringComparison.OrdinalIgnoreCase);
}

public sealed record PatioMonitorFilterOption(string Label, string Value);
