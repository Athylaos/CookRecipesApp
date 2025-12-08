using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookRecipesApp.Model.Category;
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
        private UserService _userService;
        [ObservableProperty] private string test;

        public ObservableCollection<Category> Categories { get; set; } = new();

        public RecepiesMainViewModel()
        {
            _database = _factory.CreateConnection();
            _categoryService = new(_factory);
            _userService = new(_factory);

        }

        public async void StartAsync()
        {
            var cts = await _categoryService.GetAllCategoriesAsync(true);

            foreach (var ct in cts)
            {
                Categories.Add(ct);
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
