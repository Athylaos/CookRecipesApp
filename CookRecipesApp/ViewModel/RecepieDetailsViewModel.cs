using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.ViewModel
{
    public partial class RecepieDetailsViewModel : ObservableObject
    {
        [ObservableProperty]
        string test;

        RecepieDetailsViewModel()
        {
            Test = "neco";
        }

    }
}
