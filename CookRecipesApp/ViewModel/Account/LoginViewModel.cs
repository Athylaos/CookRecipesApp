using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
