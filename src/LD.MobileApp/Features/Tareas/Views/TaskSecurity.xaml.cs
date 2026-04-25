using MauiAppLogin.ViewModels;

namespace MauiAppLogin;

public partial class TaskSecurity : ContentPage, IQueryAttributable
{
    private readonly TaskSecurityViewModel _vm;

    public TaskSecurity(TaskSecurityViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        // Parámetro de navegación heredado — no se usa en la nueva implementación
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.InicializarAsync();
    }
}
