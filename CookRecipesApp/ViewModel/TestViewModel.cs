using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookRecipesApp.Model.User;
using CookRecipesApp.Model.Category;
using CookRecipesApp.Service;
using CookRecipesApp.View;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Runtime.CompilerServices;
using CookRecipesApp.Model.Recepie;



namespace CookRecipesApp.ViewModel
{
    public partial class TestViewModel : ObservableObject
    {
        private UserService _userService;
        private CategoryService _categoryService;

        private ISQLiteAsyncConnection _database;


        public bool IsLoggedIn { get; set; }
        [ObservableProperty] private string userName;


        public TestViewModel()
        {
            _database = new SQLiteConnectionFactory().CreateConnection();
            _userService = new(new SQLiteConnectionFactory());
            _categoryService = new(new SQLiteConnectionFactory());

        }

        public async void OnAppStartAsync()
        {
            IsLoggedIn = await _userService.IsUserLoggedInAsync();

            if(IsLoggedIn)
            {
                UserName = "Logged in";
            }
            else
            {
                UserName = "Not logged in";
            }

        }

        [RelayCommand]
        public async void LoginBtn()
        {
            Debug.WriteLine("LoginBtn");
            Shell.Current.GoToAsync(nameof(LoginPage));

        }

        [RelayCommand]
        public async Task DebugDbBtn()
        {
            // Vypíše všechny uživatele do Output okna
            var users = await _database.Table<UserDbModel>().ToListAsync();
            foreach (var u in users)
            {
                System.Diagnostics.Debug.WriteLine($"ID: {u.Id}, Email: '{u.Email}', Hash: {u.PasswordHash}");
            }
        }

        [RelayCommand]
        public async Task CategoryDbBtn()
        {
            var categories = await _database.Table<CategoryDbModel>().ToListAsync();
            foreach(var c in categories)
            {
                System.Diagnostics.Debug.WriteLine($"ID: {c.Id}, Name: '{c.Name}', Image: {c.PictureUrl}");
            }
        }

        [RelayCommand]
        public async Task RecepieDbBtn()
        {
            var recepies = await _database.Table<RecepieDbModel>().ToListAsync();
            foreach (var r in recepies)
            {
                System.Diagnostics.Debug.WriteLine($"ID: {r.Id}, Name: '{r.Title}', Image: {r.PhotoPath}");
            }
        }


        [RelayCommand]
        public void RecepiesMainPageBtn()
        {
            Shell.Current.GoToAsync("//RecepiesMainPage");
        }

    }
}
