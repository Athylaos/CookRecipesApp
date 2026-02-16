using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookRecipesApp.Shared.Models;
using CookRecipesApp.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CookRecipesApp.ViewModel.Popups
{
    public partial class AddIngredientPopupViewModel : ObservableObject
    {
        private List<Ingredient> _allIngredientsSource;

        public ObservableCollection<Ingredient> FilteredIngredients {  get; set; } = new ObservableCollection<Ingredient>();
        [ObservableProperty] string searchText;

        [ObservableProperty] Ingredient selectedIngredient;
        [ObservableProperty] float quantity;

        [ObservableProperty] IngredientUnit selectedIngredientUnit;

        public event Action<object> OnCloseRequest;

        public AddIngredientPopupViewModel(List<Ingredient> il)
        {
            _allIngredientsSource = il;
            OnSearchTextChanged(string.Empty);
        }

        partial void OnSearchTextChanged(string value)
        {
            FilteredIngredients.Clear();
            if(string.IsNullOrEmpty(value))
            {
                foreach(var i in _allIngredientsSource.Take(10))
                {
                    FilteredIngredients.Add(i);
                }
            }
            else
            {
                var filtered = _allIngredientsSource.Where(i => i.Name.Contains(value, StringComparison.OrdinalIgnoreCase));
                foreach(var i in filtered.Take(10))
                {
                    FilteredIngredients.Add(i);
                }
            }
        }

        [RelayCommand]
        public Task Confirm()
        {
            if (SelectedIngredient == null) return Task.CompletedTask;

            var result = new RecipeIngredient
            {
                Ingredient = SelectedIngredient,
                Quantity = (decimal)Quantity,
                Unit = SelectedIngredientUnit.Unit??null,              
            };

            OnCloseRequest?.Invoke(result);
            return Task.CompletedTask;
        }

        [RelayCommand]
        public Task Cancel()
        {
            OnCloseRequest?.Invoke(null);
            return Task.CompletedTask;
        }

    }
}
