using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookRecipesApp.Model.Category;
using CookRecipesApp.Model.Recepie;
using CookRecipesApp.Service;
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
        private SQLiteConnectionFactory _factory = new();
        private ISQLiteAsyncConnection _database;
        private CategoryService _categoryService;
        private IngredientsService _ingredientsService;
        private RecepiesService _recepiesService;
        private UserService _userService;
        [ObservableProperty] private string test;

        public ObservableCollection<Category> Categories { get; set; } = new();
        public ObservableCollection<Recepie> FavouriteRecipes { get; set; } = new();

        public RecepiesMainViewModel()
        {
            _database = _factory.CreateConnection();
            _categoryService = new(_factory);
            _ingredientsService = new(_factory);
            _recepiesService = new(_factory, _ingredientsService, _categoryService);
            _userService = new(_factory);
        }

        public async void StartAsync()
        {
            var cts = await _categoryService.GetAllCategoriesAsync(true);

            foreach (var ct in cts)
            {
                Categories.Add(ct);
            }

            var rcps = await _recepiesService.GetAllRecepiesAsync();
            rcps = rcps.Take(10).ToList();

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
                await Shell.Current.GoToAsync("//LoginPage");
            }
            else
            {
                await Shell.Current.GoToAsync((nameof(AddRecepiePage)));
            }
        }
    }
}
