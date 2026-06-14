using MauiAppLogin.ViewModels;

namespace MauiAppLogin;

public partial class DashboardPage : ContentPage
{
    private readonly DashboardViewModel _vm;

    public DashboardPage(DashboardViewModel dashboardViewModel)
    {
        InitializeComponent();
        _vm = dashboardViewModel;
        BindingContext = dashboardViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _vm.OnNavigatedTo();
        await _vm.CargarTareasPendientesAsync();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _vm.OnNavigatedFrom();
    }
}
