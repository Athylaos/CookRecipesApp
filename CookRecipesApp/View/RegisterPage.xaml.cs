using CookRecipesApp.ViewModel;

namespace CookRecipesApp.View;

public partial class RegisterPage : ContentPage
{
	public RegisterPage(RegisterViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}