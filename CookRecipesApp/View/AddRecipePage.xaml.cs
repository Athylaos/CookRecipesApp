namespace CookRecipesApp.View;

using CookRecipesApp.ViewModel;

public partial class AddRecipePage : ContentPage
{
	public AddRecipePage(AddRecipeViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is AddRecipeViewModel vm)
        {
            vm.StartAsync();
        }
    }
}