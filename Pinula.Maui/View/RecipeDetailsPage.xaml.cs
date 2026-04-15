using Pinula.ViewModel;
namespace Pinula.View;

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
    }
}