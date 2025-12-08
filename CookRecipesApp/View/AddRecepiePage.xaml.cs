namespace CookRecipesApp.View;

using CookRecipesApp.ViewModel;

public partial class AddRecepiePage : ContentPage
{
	public AddRecepiePage(AddRecepieViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}