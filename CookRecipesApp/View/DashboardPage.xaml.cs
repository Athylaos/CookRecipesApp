using CookRecipesApp.ViewModel;

namespace CookRecipesApp.View;

public partial class DashboardPage : ContentPage
{
	public DashboardPage(DashboardViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}