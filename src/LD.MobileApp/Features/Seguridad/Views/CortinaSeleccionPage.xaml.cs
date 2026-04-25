using MauiAppLogin.ViewModels;

namespace MauiAppLogin;

public partial class CortinaSeleccionPage : ContentPage
{
    private readonly CortinaSeleccionViewModel _vm;

    public CortinaSeleccionPage(CortinaSeleccionViewModel vm)
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
