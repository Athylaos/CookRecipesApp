using CookRecipesApp.ViewModel;

namespace CookRecipesApp.View;

public partial class RecipesMainPage : ContentPage
{
	public RecipesMainPage(RecipesMainViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}

    protected override async void OnAppearing()
	{
		base.OnAppearing();
		if(BindingContext is RecipesMainViewModel vm)
		{
			vm.StartAsync();
		}
	}
}