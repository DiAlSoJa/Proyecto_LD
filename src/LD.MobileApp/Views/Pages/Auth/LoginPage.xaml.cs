using MauiAppLogin.ViewModels;

namespace MauiAppLogin;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel loginViewModel)
	{
		InitializeComponent();
        BindingContext = loginViewModel;
    }
}