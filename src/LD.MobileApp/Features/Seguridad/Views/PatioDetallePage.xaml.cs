using MauiAppLogin.ViewModels;

namespace MauiAppLogin;

public partial class PatioDetallePage : ContentPage
{
    private readonly PatioDetalleViewModel _vm;

    public PatioDetallePage(PatioDetalleViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _vm.Inicializar();
        // Verifica si se seleccionó una cortina al regresar de CortinaSeleccionPage
        await _vm.VerificarCortinaSeleccionadaAsync();
    }
}
