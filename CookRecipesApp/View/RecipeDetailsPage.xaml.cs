using CookRecipesApp.ViewModel;
namespace CookRecipesApp.View;

public partial class RecipeDetailsPage : ContentPage
{
	public RecipeDetailsPage(RecipeDetailsViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();

	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var vm = BindingContext as RecipeDetailsViewModel;
        if (vm != null && vm.RecipeId != Guid.Empty)
        {
            vm.LoadRecipeAsync(vm.RecipeId);
        }
    }
}