
using CommunityToolkit.Maui.Views;
using CookRecipesApp.ViewModel;
using CookRecipesApp.ViewModel.Popups;
using System.Diagnostics;

namespace CookRecipesApp.View.Popups;

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
