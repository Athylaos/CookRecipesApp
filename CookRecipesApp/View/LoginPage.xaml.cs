
namespace CookRecipesApp.View;

public partial class LoginPage : ContentPage
{
	public LoginPage(CookRecipesApp.ViewModel.LoginViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}