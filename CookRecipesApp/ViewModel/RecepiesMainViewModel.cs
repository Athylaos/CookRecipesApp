using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookRecipesApp.Shared.Models;
using CookRecipesApp.Service;
using CookRecipesApp.Service.Interface;
using CookRecipesApp.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using CookRecipesApp.Shared.DTOs;

namespace CookRecipesApp.ViewModel
{
    public partial class RecipesMainViewModel : ObservableObject
    {
        private ICategoryService _categoryService;
        private IIngredientService _ingredientsService;
        private IRecipeService _recipesService;
        private IUserService _userService;
        [ObservableProperty] private string test;

        public ObservableCollection<Category> Categories { get; set; } = new();
        public ObservableCollection<RecipePreviewDto> FavouriteRecipes { get; set; } = new();

        public RecipesMainViewModel(ICategoryService category, IIngredientService ingredient, IRecipeService recipe, IUserService user)
        {
            _categoryService = category;
            _ingredientsService = ingredient;
            _recipesService = recipe;
            _userService = user;
        }

        public async void StartAsync()
        {
            var cts = await _categoryService.GetAllCategoriesAsync();
            Categories.Clear();
            foreach (var ct in cts)
            {
                Categories.Add(ct);
            }

            var rcps = await _recipesService.GetFilteredRecipePreviewsAsync(new RecipeFilterParametrs() { SearchTerm = "Greek", OnlyFavorites = false, Amount = 10 });
            FavouriteRecipes.Clear();
            foreach (var r in rcps)
            {
                FavouriteRecipes.Add(r);
            }

        }

        [RelayCommand]
        public async Task AddRecipeBtn()
        {
            Debug.WriteLine(_userService.IsUserLoggedInAsync());
            if (await _userService.IsUserLoggedInAsync())
            {
                await Shell.Current.GoToAsync(nameof(AddRecipePage));
            }
            else
            {               
                await Shell.Current.GoToAsync("//LoginPage");
            }
        }

        [RelayCommand]
        public async Task CategoryBtn(Category category)
        {
            if (category == null) return;

            await Shell.Current.GoToAsync($"{nameof(RecipesCategoryPage)}?CategoryId={category.Id}",true);
        }

        [RelayCommand]
        public async Task RecipeBtn(RecipePreviewDto recipe)
        {
            if(recipe == null) return;

            await Shell.Current.GoToAsync($"{nameof(RecipeDetailsPage)}?RecipeId={recipe.Id}", true);
        }
    }
}
