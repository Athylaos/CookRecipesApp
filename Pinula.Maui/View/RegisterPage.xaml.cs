using Pinula.ViewModel;

namespace Pinula.View;

public partial class RegisterPage : ContentPage
{
	public RegisterPage(RegisterViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}