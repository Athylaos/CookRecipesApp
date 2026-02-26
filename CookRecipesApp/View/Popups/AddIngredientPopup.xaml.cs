
using CommunityToolkit.Maui.Views;
using CookRecipesApp.ViewModel;
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
            vm.OnCloseRequest -= ClosePopupWithResult;

            vm.OnCloseRequest += ClosePopupWithResult;
        }
    }

    private async void ClosePopupWithResult(object result)
    {
        await CloseAsync();
    }

}
