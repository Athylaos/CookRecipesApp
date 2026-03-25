using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.ModelsUI
{
    public partial class RatingStar : ObservableObject
    {
        public int RatingValue { get; set; }
        [ObservableProperty]
        public string icon;
    }
}
