
namespace Pinula.View;

public partial class LoginPage : ContentPage
{
	public LoginPage(Pinula.ViewModel.LoginViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}