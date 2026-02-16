namespace CookRecipesApp.View;

using CookRecipesApp.ViewModel;

public partial class RecipesCategoryPage : ContentPage
{
	public RecipesCategoryPage(RecipesCategoryViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}