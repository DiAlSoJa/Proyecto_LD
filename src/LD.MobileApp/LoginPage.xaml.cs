using MauiAppLogin.ViewModels;

namespace MauiAppLogin;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
        BindingContext = new LoginViewModel();
    }
}