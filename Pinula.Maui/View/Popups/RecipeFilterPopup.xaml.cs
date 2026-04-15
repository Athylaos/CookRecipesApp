
using CommunityToolkit.Maui.Views;
using Pinula.ViewModel;
using Pinula.ViewModel.Popups;
using System.Diagnostics;

namespace Pinula.View.Popups;

public partial class RecipeFilterPopup : Popup
{
    public RecipeFilterPopup(RecipeFilterViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;

        vm.Result.ContinueWith(_ => MainThread.BeginInvokeOnMainThread(ClosePopup));
    }

    private void ClosePopup()
    {
        this.CloseAsync();
    }
}
