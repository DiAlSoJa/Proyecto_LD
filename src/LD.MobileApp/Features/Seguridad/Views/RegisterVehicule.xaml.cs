using MauiAppLogin.ViewModels;

namespace MauiAppLogin;

public partial class RegisterVehicule : ContentPage
{
    public RegisterVehicule(RegisterVehiculeViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}

