using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Services;
using LD.Contracts.Equipment;
using LD.FormsX.Helpers;

namespace LD.FormsX.Features.CheckList.ViewModels;

public partial class EquiposTabViewModel : ObservableObject
{
    private readonly EquipmentService _equipmentService;

    [ObservableProperty]
    private string statusText = "Sin equipos para mostrar";

    [ObservableProperty]
    private EquipmentDto? selectedEquipment;

    public event Action<List<EquipmentDto>>? OnEquiposLoaded;

    public EquiposTabViewModel(EquipmentService equipmentService)
    {
        _equipmentService = equipmentService;
    }

    public async Task CargarEquiposAsync()
    {
        try
        {
            var response = await _equipmentService.GetEquipments();
            if (!response.IsSuccess || response.Data is null)
            {
                StatusText = response.Message ?? response.ErrorMessage ?? "No se pudieron cargar los equipos.";
                OnEquiposLoaded?.Invoke(new List<EquipmentDto>());
                return;
            }
            StatusText = response.Data.Count > 0
                ? $"{response.Data.Count} equipo(s) cargado(s)"
                : "Sin equipos para mostrar";
            OnEquiposLoaded?.Invoke(response.Data);
        }
        catch (Exception ex)
        {
            StatusText = "No se pudieron cargar los equipos.";
            DialogHelper.ShowError(ex.Message);
        }
    }
}
