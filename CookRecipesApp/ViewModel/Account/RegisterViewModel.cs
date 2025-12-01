using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookRecipesApp.Model.User;
using CookRecipesApp.View.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.ViewModel.Account
{
    public partial class RegisterViewModel : ObservableObject
    {
        [ObservableProperty] private User newUser;

        [ObservableProperty] private UserRegistrationDto newRegistrationDto;

        [ObservableProperty] private string password1;
        [ObservableProperty] private string password2;



        [RelayCommand]
        public void RegisterBtn()
        {

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

        }

    }
}
