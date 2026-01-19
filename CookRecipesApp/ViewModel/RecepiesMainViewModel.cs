using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookRecipesApp.Model.Category;
using CookRecipesApp.Model.Recepie;
using CookRecipesApp.Service;
using CookRecipesApp.Service.Interface;
using CookRecipesApp.View;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace CookRecipesApp.ViewModel
{
    public partial class RecepiesMainViewModel : ObservableObject
    {
        private ICategoryService _categoryService;
        private IIngredientService _ingredientsService;
        private IRecepieService _recepiesService;
        private IUserService _userService;
        [ObservableProperty] private string test;

        public ObservableCollection<Category> Categories { get; set; } = new();
        public ObservableCollection<Recepie> FavouriteRecipes { get; set; } = new();

        public RecepiesMainViewModel(ICategoryService category, IIngredientService ingredient, IRecepieService recepie, IUserService user)
        {
            _categoryService = category;
            _ingredientsService = ingredient;
            _recepiesService = recepie;
            _userService = user;
        }

        public async void StartAsync()
        {
            var cts = await _categoryService.GetAllCategoriesAsync(true);
            Categories.Clear();
            foreach (var ct in cts)
            {
                Categories.Add(ct);
            }

            var rcps = await _recepiesService.GetRecepiesAsync(-1);
            rcps = rcps.Take(10).ToList();
            FavouriteRecipes.Clear();
            foreach (var r in rcps)
            {
                FavouriteRecipes.Add(r);
            }

        }

        [RelayCommand]
        public async Task AddRecepieBtn()
        {
            Debug.WriteLine(_userService.IsUserLoggedInAsync());
            if (await _userService.IsUserLoggedInAsync())
            {
                await Shell.Current.GoToAsync(nameof(AddRecepiePage));
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

            await Shell.Current.GoToAsync($"{nameof(RecepiesCategoryPage)}?CategoryId={category.Id}",true);
        }
    }
}
