using CommunityToolkit.Mvvm.ComponentModel;
using CookRecipesApp.Model.Category;
using CookRecipesApp.Service;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CookRecipesApp.ViewModel
{
    public partial class RecepiesMainViewModel : ObservableObject
    {
        private SQLiteConnectionFactory _factory = new();
        private ISQLiteAsyncConnection _database;
        private CategoryService _categoryService;
        [ObservableProperty] private string test;

        public ObservableCollection<Category> Categories { get; set; } = new();

        public RecepiesMainViewModel()
        {
            _database = _factory.CreateConnection();
            _categoryService = new(_factory);

        }

        public async void StartAsync()
        {
            var cts = await _categoryService.GetAllCategoriesAsync(true);

            foreach (var ct in cts)
            {
                Categories.Add(ct);
            }
        }
    }
}
