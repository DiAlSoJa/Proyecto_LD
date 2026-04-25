using MauiAppLogin.ViewModels;

namespace MauiAppLogin;

public partial class PatioPendientesPage : ContentPage
{
    private readonly PatioPendientesViewModel _vm;

    public PatioPendientesPage(PatioPendientesViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.InicializarAsync();
    }
}
