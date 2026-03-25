using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookRecipesApp.Service;
using CookRecipesApp.Service.Interface;
using CookRecipesApp.Shared.DTOs;
using CookRecipesApp.Shared.Models;
using CookRecipesApp.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CookRecipesApp.ViewModel
{
    [QueryProperty(nameof(CategoryId), "CategoryId")]
    public partial class RecipesCategoryViewModel : ObservableObject
    {
        private readonly ICategoryService _categoryService;
        private readonly IRecipeService _recipeService;

        public RecipesCategoryViewModel(ICategoryService categoryService, IRecipeService recipeService)
        {
            _categoryService = categoryService;
            _recipeService = recipeService;
        }

        [ObservableProperty]
        private string categoryId;

        partial void OnCategoryIdChanged(string value)
        {
            if (Guid.TryParse(value, out var guid))
            {
                _ = LoadCategoryAsync(guid);
            }
        }

        [ObservableProperty]
        Category selectedCategory;

        [ObservableProperty]
        private ObservableCollection<RecipePreviewDto> favoriteRecipes = new ObservableCollection<RecipePreviewDto>();

        public async Task LoadCategoryAsync(Guid id)
        {
            SelectedCategory = await _categoryService.GetCategoryByIdAsync(id) ?? new();

            var favoriteRecipesApi = await _recipeService.GetFilteredRecipePreviewsAsync(new RecipeFilterParametrs() { Amount = 10, CategoryId = id }, null);

            FavoriteRecipes.Clear();
            foreach(var fvr in  favoriteRecipesApi)
            {
                FavoriteRecipes.Add(fvr);
            }
        }

        [RelayCommand]
        public async Task RecipeBtn(RecipePreviewDto recipe)
        {
            if (recipe == null) return;

            await Shell.Current.GoToAsync($"{nameof(RecipeDetailsPage)}?RecipeIdString={recipe.Id}", true);

        }

        [RelayCommand]
        public void RecipesMainPageBtn()
        {
            Shell.Current.GoToAsync("//RecipesMainPage");
        }
    }
}
