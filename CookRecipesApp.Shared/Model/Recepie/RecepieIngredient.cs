using CommunityToolkit.Mvvm.ComponentModel;
using CookRecipesApp.Model.Ingredient;
using System;
using System.Collections.Generic;
using System.Text;
using static CookRecipesApp.Model.Ingredient.Ingredient;

namespace CookRecipesApp.Model.Recepie
{
    public partial class RecepieIngredient : ObservableObject
    {
        public int Id { get; set; }
        public int RecepieId { get; set; }

        [ObservableProperty]
        private CookRecipesApp.Model.Ingredient.Ingredient ingredient;

        [ObservableProperty]
        private float quantity;

        [ObservableProperty]
        private IngredientUnitInfo selectedUnitInfo;

        //UI

        [ObservableProperty]
        private bool isReady;


    }
}
