using CookRecipesApp.ViewModel;
namespace CookRecipesApp.View;

public partial class RecepieDetailsPage : ContentPage
{
	public RecepieDetailsPage(RecepieDetailsViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();

	}
}