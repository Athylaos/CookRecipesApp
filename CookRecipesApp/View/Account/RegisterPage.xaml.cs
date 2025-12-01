using CookRecipesApp.ViewModel.Account;

namespace CookRecipesApp.View.Account;

public partial class RegisterPage : ContentPage
{
	public RegisterPage(RegisterViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}