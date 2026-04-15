using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pinula.View;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pinula.ViewModel
{
    public partial class DashboardViewModel : ObservableObject
    {

        [ObservableProperty]
        string test = "NECO";
        [ObservableProperty]
        bool isLoading = false;







        [RelayCommand]
        public void RecipesMainPageBtn()
        {
            Shell.Current.GoToAsync("//RecipesMainPage");
        }

        [RelayCommand]
        public void ProfilePageBtn()
        {
            Shell.Current.GoToAsync(nameof(ProfilePage));
        }


        [RelayCommand]
        public async Task GoBackBtn()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
