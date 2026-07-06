using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Services;
using LD.Contracts.DTOs.ReportQueries;
using LD.Contracts.Requests;
using LD.FormsX.Helpers;

namespace LD.FormsX.Features.Reportes.ViewModels;

public partial class ReportQueryEditorViewModel : ObservableObject
{
    private readonly ReportQueryService _reportQueryService;

    public Action? RequestClose { get; set; }

    public ReportQueryDto? SelectedQuery { get; private set; }

    [ObservableProperty]
    private string headerTitle = "Nueva consulta";

    [ObservableProperty]
    private string nombre = string.Empty;

    [ObservableProperty]
    private string query = string.Empty;

    [ObservableProperty]
    private bool isBusy;

    public ReportQueryEditorViewModel(ReportQueryService reportQueryService)
    {
        _reportQueryService = reportQueryService;
    }

    public void SetQuery(ReportQueryDto? reportQuery)
    {
        SelectedQuery = reportQuery;

        if (reportQuery is null)
        {
            HeaderTitle = "Nueva consulta";
            Nombre = string.Empty;
            Query = string.Empty;
            return;
        }

        HeaderTitle = "Editar consulta";
        Nombre = reportQuery.Nombre;
        Query = reportQuery.Query;
    }

    public async Task SaveAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                DialogHelper.ShowWarning("El nombre de la consulta es obligatorio.");
                return;
            }

            if (string.IsNullOrWhiteSpace(Query))
            {
                DialogHelper.ShowWarning("La consulta SQL es obligatoria.");
                return;
            }

            IsBusy = true;

            var request = new ReportQueryRequest
            {
                Nombre = Nombre.Trim(),
                Query = Query.Trim()
            };

            var response = SelectedQuery is null
                ? await _reportQueryService.CreateReportQueryAsync(request)
                : await _reportQueryService.UpdateReportQueryAsync(SelectedQuery.ReportQueryId, request);

            if (!response.IsSuccess)
            {
                DialogHelper.ShowError(response.ErrorMessage ?? response.Message ?? "No se pudo guardar la consulta.");
                return;
            }

            DialogHelper.ShowSuccess(response.Data ?? response.Message ?? "Consulta guardada correctamente.");
            RequestClose?.Invoke();
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
        finally
        {
            IsBusy = false;
        }
    }
}
