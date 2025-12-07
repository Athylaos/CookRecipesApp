using CookRecipesApp.ViewModel;

namespace CookRecipesApp.View;

public partial class RecepiesMainPage : ContentPage
{
	public RecepiesMainPage(RecepiesMainViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}

    protected override async void OnAppearing()
	{
		base.OnAppearing();
		if(BindingContext is RecepiesMainViewModel vm)
		{
			vm.StartAsync();
		}
	}
}