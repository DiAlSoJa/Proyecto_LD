using MauiAppLogin.ViewModels;

namespace MauiAppLogin;

public partial class SignatureDriver : ContentPage
{
	public SignatureDriver(SignatureDriverViewModel vm)
	{
		InitializeComponent();
		vm.SignaturePad = SignaturePad;
		BindingContext = vm;
	}
}
