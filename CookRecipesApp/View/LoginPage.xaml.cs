using CookRecipesApp.ViewModel;

namespace CookRecipesApp.View;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}