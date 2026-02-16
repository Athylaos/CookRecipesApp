using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookRecipesApp.Service;
using CookRecipesApp.Service.Interface;
using CookRecipesApp.View;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.ViewModel
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IUserService _userService;

        [ObservableProperty] string? email;
        [ObservableProperty] string? password;

        [ObservableProperty] bool indicatorVisibility;
        [ObservableProperty] string? indicatorText;


        public LoginViewModel(IUserService userService)
        {
            _userService = userService;
            IndicatorVisibility = false;
            Email = string.Empty;
            Password = string.Empty;


        }



        [RelayCommand]
        public async Task LoginBtn()
        {
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

            Shell.Current.GoToAsync("//TestPage");
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
