using MauiAppLogin.ViewModels;

namespace MauiAppLogin;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel loginViewModel)
	{
		InitializeComponent();
        BindingContext = loginViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((LoginViewModel)BindingContext).TryAutoLoginAsync();
    }
}