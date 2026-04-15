namespace Pinula.View;

using Pinula.ViewModel;

public partial class RecipesCategoryPage : ContentPage
{
	public RecipesCategoryPage(RecipesCategoryViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }
}