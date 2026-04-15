using Pinula.ViewModel;

namespace Pinula.View;

public partial class ProfilePage : ContentPage
{
	public ProfilePage(ProfileViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        if (BindingContext is ProfileViewModel vm)
        {
            vm.StartAsync();
        }
        base.OnAppearing();
    }
    
}