using CookRecipesApp.ViewModel;
namespace CookRecipesApp.View;

public partial class RecipeDetailsPage : ContentPage
{
	public RecipeDetailsPage(RecipeDetailsViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();

	}
}