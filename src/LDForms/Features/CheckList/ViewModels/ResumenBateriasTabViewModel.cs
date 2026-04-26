using CommunityToolkit.Mvvm.ComponentModel;

namespace LD.FormsX.Features.CheckList.ViewModels;

// Pendiente: requiere endpoint de resumen de checklists filtrado por tipo Batería.
public partial class ResumenBateriasTabViewModel : ObservableObject
{
    [ObservableProperty]
    private string statusText = "Sin registros para mostrar";

    [ObservableProperty]
    private DateTime? fechaDesde;

    [ObservableProperty]
    private DateTime? fechaHasta;
}
