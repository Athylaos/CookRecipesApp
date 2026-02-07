using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookRecipesApp.Model.User;
using CookRecipesApp.Model.Category;
using CookRecipesApp.Service.Interface;
using CookRecipesApp.View;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Runtime.CompilerServices;
using CookRecipesApp.Model.Recepie;
using System.Threading.Tasks.Dataflow;
using CookRecipesApp.Service;



namespace CookRecipesApp.ViewModel
{
    public partial class TestViewModel : ObservableObject
    {
        private IUserService _userService;
        private ICategoryService _categoryService;
        private IRecepieService _recepieService;

        private ISQLiteAsyncConnection _database;


        public bool IsLoggedIn;
        [ObservableProperty] private string userName;


        public TestViewModel(IUserService userService, ICategoryService categoryService, IRecepieService recepieService)
        {
            _userService = userService;
            _categoryService = categoryService;
            _recepieService = recepieService;
            _database = new SQLiteConnectionFactory().CreateConnection();

        }

        public async void OnAppStartAsync()
        {
            IsLoggedIn = await _userService.IsUserLoggedInAsync();


            if(IsLoggedIn)
            {
                var user = await _userService.GetCurrentUserAsync();
                UserName = $"Logged in {user.Name} email {user.Email}";
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
            var users = await _database.Table<UserDbModel>().ToListAsync();
            foreach (var u in users)
            {
                System.Diagnostics.Debug.WriteLine($"ID: {u.Id},  Name: '{u.Name}',Email: '{u.Email}', Hash: {u.PasswordHash}");
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
            var recepiesDb = await _recepieService.GetRecepiesAsync(-1);
            foreach (var r in recepies)
            {
                System.Diagnostics.Debug.WriteLine($"ID: {r.Id}, Name: '{r.Title}', Image: {r.PhotoPath}");
            }
            foreach (var r in recepiesDb)
            {
                System.Diagnostics.Debug.WriteLine($"From service ID: {r.Id}, Name: '{r.Title}', Image: {r.PhotoPath}");
            }
        }


        [RelayCommand]
        public void RecepiesMainPageBtn()
        {
            Shell.Current.GoToAsync("//RecepiesMainPage");
        }

        [RelayCommand]
        public async Task RecepieDetailPageBtn()
        {
            await Shell.Current.GoToAsync(nameof(RecepieDetailsPage));
        }

        [RelayCommand]
        public async Task LogoutBtn()
        {
            await _userService.LogoutAsync();
        }
    }
}
