using MauiAppLogin.ViewModels;

namespace MauiAppLogin;

public partial class RegisterLicense : ContentPage
{
	public RegisterLicense(RegisterLicenseViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		if (BindingContext is RegisterLicenseViewModel vm)
			await vm.StartCaptureFlowAsync();
	}
}

