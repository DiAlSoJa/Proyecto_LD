using MauiAppLogin.ViewModels;

namespace MauiAppLogin;

public partial class RegisterLicense : ContentPage
{
	public RegisterLicense(RegisterLicenseViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}

