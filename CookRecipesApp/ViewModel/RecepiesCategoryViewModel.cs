using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookRecipesApp.Shared.Models;
using CookRecipesApp.Service;
using CookRecipesApp.Service.Interface;
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

        Guid categoryId;
        public Guid CategoryId
        {
            get => categoryId;
            set
            {
                SetProperty(ref categoryId, value);
                _ = LoadCategoryAsync(value);
            }
        }

        [ObservableProperty]
        Category selectedCategory;

        [ObservableProperty]
        private ObservableCollection<Recipe> favoriteRecipes = new ObservableCollection<Recipe>();

        public async Task LoadCategoryAsync(Guid id)
        {
            SelectedCategory = await _categoryService.GetCategoryByIdAsync(id);

            var favoriteRecipes = await _recipeService.GetRecipesAsync(3);

            foreach(var fvr in  favoriteRecipes)
            {
                FavoriteRecipes.Add(fvr);
            }
        }

        [RelayCommand]
        public async Task RecipeBtn(Recipe recipe)
        {
            if (recipe == null) return;

            await Shell.Current.GoToAsync($"{nameof(RecipeDetailsPage)}?RecipeId={recipe.Id}", true);
        }

        [RelayCommand]
        public void RecipesMainPageBtn()
        {
            Shell.Current.GoToAsync("//RecipesMainPage");
        }
    }
}
