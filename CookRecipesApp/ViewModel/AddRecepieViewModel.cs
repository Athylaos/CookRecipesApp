using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.ViewModel
{
    public partial class AddRecepieViewModel : ObservableObject
    {

        [ObservableProperty] private string test;

        public AddRecepieViewModel()
        {
            Test = "test ok";
        }
    }
}
