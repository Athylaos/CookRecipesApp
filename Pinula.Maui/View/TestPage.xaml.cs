using Pinula.ViewModel;

namespace Pinula.View;

public partial class TestPage : ContentPage
{
	public TestPage(TestViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
		base.OnAppearing();

		if(BindingContext is TestViewModel vm)
		{
			vm.OnAppStartAsync();
		}

	}
}