using Pinula.ViewModel;

namespace Pinula.View;

public partial class DashboardPage : ContentPage
{
	public DashboardPage(DashboardViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}