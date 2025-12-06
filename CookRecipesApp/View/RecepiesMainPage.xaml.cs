using CookRecipesApp.ViewModel;

namespace CookRecipesApp.View;

public partial class RecepiesMainPage : ContentPage
{
	public RecepiesMainPage(RecepiesMainViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}