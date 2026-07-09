using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.Constants;
using LD.Contracts.DTOs.ReportQueries;
using LD.FormsX.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LD.FormsX.Features.Reportes.ViewModels;

public partial class ReportesViewModel : ObservableObject
{
    private readonly ReportQueryService _reportQueryService;

    public event Action<List<ReportQueryDto>>? OnDataLoaded;

    [ObservableProperty]
    private ReportQueryDto? selectedQuery;

    [ObservableProperty]
    private string statusText = string.Empty;

    [ObservableProperty]
    private bool isLoading;

    public bool CanManage { get; }

    public bool CanExecute { get; }

    public bool CanView { get; }

    public bool CanCreate => CanManage;

    public bool CanEdit => CanManage;

    public bool CanDelete => CanManage;

    public bool CanViewSql => CanManage;

    public ReportesViewModel(ReportQueryService reportQueryService)
    {
        _reportQueryService = reportQueryService;
        CanManage = UserData.HasPermission(PermissionKeys.Query_Manage);
        CanExecute = UserData.HasPermission(PermissionKeys.Query_Execute);
        CanView = CanManage
            || UserData.HasPermission(PermissionKeys.Query_View)
            || CanExecute;
    }

    public async Task CargarDatosAsync()
    {
        try
        {
            IsLoading = true;

            if (!CanView)
            {
                StatusText = "No tienes permisos para ver consultas.";
                OnDataLoaded?.Invoke([]);
                return;
            }

            if (CanManage)
            {
                var result = await _reportQueryService.GetReportQueriesAsync();
                if (!result.IsSuccess || result.Data is null)
                {
                    DialogHelper.ShowWarning(result.Message ?? "No se pudieron cargar las consultas.");
                    OnDataLoaded?.Invoke([]);
                    StatusText = "No se pudieron cargar las consultas.";
                    return;
                }

                var queries = result.Data
                    .Select(x => new ReportQueryDto
                    {
                        ReportQueryId = x.ReportQueryId,
                        Nombre = x.Nombre,
                        Query = x.Query
                    })
                    .ToList();

                SelectedQuery = null;
                StatusText = $"Registros: {queries.Count}";
                OnDataLoaded?.Invoke(queries);
                return;
            }

            var summaryResult = await _reportQueryService.GetReportQuerySummariesAsync();
            if (!summaryResult.IsSuccess || summaryResult.Data is null)
            {
                DialogHelper.ShowWarning(summaryResult.Message ?? "No se pudieron cargar las consultas.");
                OnDataLoaded?.Invoke([]);
                StatusText = "No se pudieron cargar las consultas.";
                return;
            }

            var summaryQueries = summaryResult.Data
                .Select(x => new ReportQueryDto
                {
                    ReportQueryId = x.ReportQueryId,
                    Nombre = x.Nombre,
                    Query = string.Empty
                })
                .ToList();

            SelectedQuery = null;
            StatusText = $"Registros: {summaryQueries.Count}";
            OnDataLoaded?.Invoke(summaryQueries);
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
            StatusText = "Ocurrió un error al cargar las consultas.";
            OnDataLoaded?.Invoke([]);
        }
        finally
        {
            IsLoading = false;
        }
    }
}
