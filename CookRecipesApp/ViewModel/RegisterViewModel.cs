using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookRecipesApp.Model.User;
using CookRecipesApp.Service;
using CookRecipesApp.View;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.ViewModel
{
    public partial class RegisterViewModel : ObservableObject
    {
        private UserService _userService;
        private SQLiteConnectionFactory _connectionFactory;

        private User newUser;

        private UserRegistrationDto newRegistrationDto;

        [ObservableProperty] private string nameEntry;
        [ObservableProperty] private string surnameEntry;
        [ObservableProperty] private string emailEntry;

        [ObservableProperty] private string password1;
        [ObservableProperty] private string password2;

        [ObservableProperty] bool indicatorVisibility;
        [ObservableProperty] string? indicatorText;


        public RegisterViewModel()
        {
            _connectionFactory = new SQLiteConnectionFactory();
            _userService = new(_connectionFactory);
            newUser = new();

        }

        [RelayCommand]
        public async Task RegisterBtn()
        {
            newUser = new User()
            {
                Name = NameEntry,
                Surname = SurnameEntry,
                Email = EmailEntry,
            };

            System.Diagnostics.Debug.WriteLine(newUser.Email);
            IndicatorVisibility = false;
            if (Password1 != Password2)
            {
                IndicatorVisibility = true;
                IndicatorText = "Password doesn't match";
                return;
            }

            newRegistrationDto = new UserRegistrationDto
            {
                Email = newUser.Email,
                Password = Password1,
            };

            System.Diagnostics.Debug.WriteLine(newRegistrationDto.Email);
            if (!await _userService.RegisterAsync(newRegistrationDto))
            {
                IndicatorVisibility = true;
                IndicatorText = "This email is already registred, please login";
                return;
            }

            newRegistrationDto = new();

            await _userService.UpdateUserAsync(newUser);

            if (await _userService.LoginAsync(newUser.Email, Password1) == null)
            {
                return;
            }

            System.Diagnostics.Debug.WriteLine("All good going to main page");
            Shell.Current.GoToAsync(nameof(TestPage));

        }

        [RelayCommand]
        public void LoginBtn()
        {
            Shell.Current.GoToAsync(nameof(LoginPage));
        }

        [RelayCommand]
        public void BackBtn()
        {
            Shell.Current.Navigation.PopAsync(true);
        }

        [RelayCommand]
        public void SkipBtn()
        {
            Shell.Current.GoToAsync(nameof(TestPage));
        }

    }
}
