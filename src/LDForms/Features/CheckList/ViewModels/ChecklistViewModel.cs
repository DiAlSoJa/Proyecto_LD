using CommunityToolkit.Mvvm.ComponentModel;

namespace LD.FormsX.Features.CheckList.ViewModels;

public partial class ChecklistViewModel : ObservableObject
{
    public EquiposTabViewModel EquiposTab { get; }
    public ResumenTabViewModel ResumenTab { get; }
    public ResumenBateriasTabViewModel ResumenBateriasTab { get; }
    public ConfiguracionPreguntasTabViewModel ConfiguracionPreguntasTab { get; }

    public ChecklistViewModel(
        EquiposTabViewModel equiposTab,
        ResumenTabViewModel resumenTab,
        ResumenBateriasTabViewModel resumenBateriasTab,
        ConfiguracionPreguntasTabViewModel configuracionPreguntasTab)
    {
        EquiposTab             = equiposTab;
        ResumenTab             = resumenTab;
        ResumenBateriasTab     = resumenBateriasTab;
        ConfiguracionPreguntasTab = configuracionPreguntasTab;
    }
}
