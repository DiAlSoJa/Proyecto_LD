using MauiAppLogin.ViewModels;

namespace MauiAppLogin;

public partial class TaskSecurityPage : ContentPage
{
    private readonly TaskSecurityViewModel _vm;

    public TaskSecurityPage(TaskSecurityViewModel vm)
    {
        InitializeComponent();
        _vm            = vm;
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.InicializarAsync();
    }
}
