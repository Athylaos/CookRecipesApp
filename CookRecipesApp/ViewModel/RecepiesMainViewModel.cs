using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.ViewModel
{
    public partial class RecepiesMainViewModel : ObservableObject
    {

        [ObservableProperty] private string test;

        public RecepiesMainViewModel()
        {
            Test = "test okak";
        }
    }
}
