using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookRecipesApp.Shared.Models;
using CookRecipesApp.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CookRecipesApp.Shared.DTOs;

namespace CookRecipesApp.ViewModel.Popups
{
    public partial class AddIngredientPopupViewModel : ObservableObject
    {
        private List<IngredientPreview> _allIngredientsSource;

        public ObservableCollection<IngredientPreview> FilteredIngredients {  get; set; } = new ObservableCollection<IngredientPreview>();
        [ObservableProperty] string searchText;

        [ObservableProperty] IngredientPreview selectedIngredient;
        [ObservableProperty] float quantity;

        [ObservableProperty] IngredientUnit selectedIngredientUnit;

        public event Action<object> OnCloseRequest;

        public AddIngredientPopupViewModel(List<IngredientPreview> il)
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
                Ingredient = new Ingredient { Id = SelectedIngredient.Id},
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
