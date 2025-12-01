using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookRecipesApp.View.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.ViewModel.Account
{
    public partial class LoginViewModel : ObservableObject
    {

        [ObservableProperty] string? email;
        [ObservableProperty] string? password;

        [ObservableProperty] bool indicatorVisibility;
        [ObservableProperty] string? indicatorText;


        public LoginViewModel()
        {


        }



        [RelayCommand]
        public void LoginBtn()
        {

        }

        [RelayCommand]
        public void CreateAccountBtn()
        {
            Shell.Current.GoToAsync(nameof(RegisterPage));
        }

        [RelayCommand]
        public void ForgotPasswordBtn()
        {

        }

        [RelayCommand]
        public void SkipBtn()
        {

        }



    }
}
