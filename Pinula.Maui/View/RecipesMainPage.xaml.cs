using Pinula.ViewModel;

namespace Pinula.View;

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
		};
	}
}