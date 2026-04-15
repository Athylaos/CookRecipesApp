
using CommunityToolkit.Maui.Views;
using Pinula.ViewModel;
using Pinula.ViewModel.Popups;
using System.Diagnostics;

namespace Pinula.View.Popups;

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
