using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookRecipesApp.Model.Category;
using CookRecipesApp.Model.Recepie;
using CookRecipesApp.Service.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CookRecipesApp.ViewModel
{
    [QueryProperty(nameof(RecepieId), "RecepieId")]
    public partial class RecepieDetailsViewModel : ObservableObject
    {

        private IRecepieService _recepieService;

        int recepieId;
        public int RecepieId
        {
            get => recepieId;
            set
            {
                SetProperty(ref recepieId, value);
                _ = LoadRecepieAsync(value);
            }
        }

        [ObservableProperty]
        Recepie selectedRecepie;

        private async Task LoadRecepieAsync(int id)
        {
            SelectedRecepie = await _recepieService.GetRecepieAsync(id);

        }


        public RecepieDetailsViewModel(IRecepieService recepieService)
        {
            _recepieService = recepieService;
        }


        [RelayCommand]
        public void IngredientTappedCommand(RecepieIngredient ig)
        {
            if(ig is null) return;

            ig.IsReady = !ig.IsReady;

            Debug.WriteLine(ig.Ingredient.Name);

        }

    }
}
