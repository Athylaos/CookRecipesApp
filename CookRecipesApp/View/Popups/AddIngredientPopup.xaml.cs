
using CommunityToolkit.Maui.Views;
using CookRecipesApp.ViewModel.Popups;
using System.Diagnostics;

namespace CookRecipesApp.View.Popups;

public partial class AddIngredientPopup : Popup
{
    public AddIngredientPopup()
    {
        InitializeComponent();

        this.BindingContextChanged += AddIngredientPopup_BindingContextChanged;

    }

    private void AddIngredientPopup_BindingContextChanged(object sender, EventArgs e)
    {
        if (BindingContext is AddIngredientPopupViewModel vm)
        {
            // Odhlásit pøedchozí (pro jistotu, aby se nevolalo víckrát)
            vm.OnCloseRequest -= ClosePopupWithResult;

            // Pøihlásit se k odbìru eventu
            vm.OnCloseRequest += ClosePopupWithResult;
        }
    }

    // Tato metoda zavøe popup a vrátí výsledek tomu, kdo volal ShowPopupAsync
    private async void ClosePopupWithResult(object result)
    {
        await CloseAsync();
    }

}
