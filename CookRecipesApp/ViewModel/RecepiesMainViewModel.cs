using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookRecipesApp.Service;
using CookRecipesApp.Service.Interface;
using CookRecipesApp.Shared.DTOs;
using CookRecipesApp.Shared.Models;
using CookRecipesApp.View;
using CookRecipesApp.View.Popups;
using CookRecipesApp.ViewModel.Popups;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace CookRecipesApp.ViewModel
{
    public partial class RecipesMainViewModel : ObservableObject
    {
        private ICategoryService _categoryService;
        private IIngredientService _ingredientsService;
        private IRecipeService _recipesService;
        private IUserService _userService;
        [ObservableProperty] private string test;
        private CancellationTokenSource? _searchCts;

        [ObservableProperty]
        bool isSearching;
        [ObservableProperty]
        string searchTerm;
        [ObservableProperty]
        RecipeFilterParametrs filterParametrs;
        [ObservableProperty]
        bool isEmpty;
        public ObservableCollection<RecipePreviewDto> SearchedRecipes { get; set; } = new();     

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

            var rcps = await _recipesService.GetFilteredRecipePreviewsAsync(new RecipeFilterParametrs() { OnlyFavorites = false, Amount = 10 }, null);
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
            if (recipe is null) return;

            await Shell.Current.GoToAsync($"{nameof(RecipeDetailsPage)}?RecipeIdString={recipe.Id}", true);
 
        }


        partial void OnSearchTermChanged(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                IsSearching = false;
                return;
            }
            RestartSearch(true);
        }

        private void RestartSearch(bool isDebounced)
        {
            IsSearching = true;
            _searchCts?.Cancel();
            _searchCts?.Dispose();
            _searchCts = new CancellationTokenSource();

            _ = SearchAsync(_searchCts.Token, isDebounced);
        }

        [RelayCommand]
        public async Task OpenFilterPopup()
        {
            var vm = new RecipeFilterViewModel { FilterParametrs = FilterParametrs??new() };
            var popup = new RecipeFilterPopup(vm);
            Shell.Current.CurrentPage.ShowPopup(popup);

            var result = await vm.Result;
            if (result != null)
            {
                FilterParametrs = result;
                RestartSearch(false);
            }
        }

        [RelayCommand]
        public void ClearFilters()
        {
            SearchTerm = string.Empty;
            FilterParametrs = new RecipeFilterParametrs();
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                IsSearching = false;
                return;
            }
            RestartSearch(false);
        }

        private async Task SearchAsync(CancellationToken token, bool withDelay)
        {
            try
            {
                if (withDelay)
                    await Task.Delay(500, token);

                if (!IsSearching)
                {
                    MainThread.BeginInvokeOnMainThread(() => {
                        IsEmpty = false;
                        SearchedRecipes.Clear();
                    });
                    return;
                }

                if (FilterParametrs is null) FilterParametrs = new();
                FilterParametrs.SearchTerm = SearchTerm;

                var results = await _recipesService.GetFilteredRecipePreviewsAsync(FilterParametrs, token);

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (token.IsCancellationRequested) return;

                    IsSearching = true;
                    SearchedRecipes.Clear();

                    foreach (var r in results)
                        SearchedRecipes.Add(r);

                    IsEmpty = SearchedRecipes.Count == 0;
                });
            }
            catch (OperationCanceledException) { }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }
    }
}
