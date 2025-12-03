using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookRecipesApp.Service;
using CookRecipesApp.View;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.ViewModel
{
    public partial class LoginViewModel : ObservableObject
    {
        private UserService _userService;
        private SQLiteConnectionFactory _connectionFactory;

        [ObservableProperty] string? email;
        [ObservableProperty] string? password;

        [ObservableProperty] bool indicatorVisibility;
        [ObservableProperty] string? indicatorText;


        public LoginViewModel()
        {
            _connectionFactory = new SQLiteConnectionFactory();
            _userService = new(_connectionFactory);


        }



        [RelayCommand]
        public async Task LoginBtn()
        {
            IndicatorVisibility = false;
            System.Diagnostics.Debug.WriteLine(Email);
            System.Diagnostics.Debug.WriteLine(Password);
            var x = await _userService.IsEmailRegistredAsync(Email);
            if (x)
            {
                System.Diagnostics.Debug.WriteLine("True");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("False");
            }

            if (!await _userService.IsEmailRegistredAsync(Email))
            {
                IndicatorVisibility = true;
                IndicatorText = "Email not registered, please register";
                return;
            }

            if(await _userService.LoginAsync(Email, Password) == null)
            {
                IndicatorVisibility = true;
                IndicatorText = "Wrong password";
                return;
            }

            System.Diagnostics.Debug.WriteLine("Prihlaseno");

        }

        [RelayCommand]
        public void CreateAccountBtn()
        {
            Shell.Current.GoToAsync(nameof(RegisterPage));
        }

        [RelayCommand]
        public void ForgotPasswordBtn()
        {
            IndicatorVisibility = true;
            IndicatorText = "For password reset please contact support";

        }

        [RelayCommand]
        public void SkipBtn()
        {
            System.Diagnostics.Debug.WriteLine("SkipBtn");
            Shell.Current.Navigation.PopAsync(true);
        }



    }
}
