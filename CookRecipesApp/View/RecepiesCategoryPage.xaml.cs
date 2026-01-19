namespace CookRecipesApp.View;

using CookRecipesApp.ViewModel;

public partial class RecepiesCategoryPage : ContentPage
{
	public RecepiesCategoryPage(RecepiesCategoryViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}