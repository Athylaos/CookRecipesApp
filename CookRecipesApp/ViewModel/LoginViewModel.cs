using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.ViewModel
{
    public partial class LoginViewModel : ObservableObject
    {

        [ObservableProperty] string userName;

        public LoginViewModel()
        {
            UserName = "ahoj test";
        }

    }
}
