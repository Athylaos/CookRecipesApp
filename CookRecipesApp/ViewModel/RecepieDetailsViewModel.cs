using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookRecipesApp.Model.Category;
using CookRecipesApp.Model.Recepie;
using CookRecipesApp.Service.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CookRecipesApp.ViewModel
{
    [QueryProperty(nameof(RecepieId), "RecepieId")]
    public partial class RecepieDetailsViewModel : ObservableObject
    {

        private IRecepieService _recepieService;
        private IUserService _userService;

        int recepieId;
        public int RecepieId
        {
            get => recepieId;
            set
            {
                SetProperty(ref recepieId, value);
                _ = LoadRecepieAsync(value);
            }
        }

        [ObservableProperty]
        private Recepie selectedRecepie;
        [ObservableProperty]
        private string favoriteIconPath;


        private async Task LoadRecepieAsync(int id)
        {
            SelectedRecepie = await _recepieService.GetRecepieAsync(id);

            if (!await _userService.IsUserLoggedInAsync())
            {
                FavoriteIconPath = "favorite.png";
            }
            else
            {

                var user = await _userService.GetCurrentUserAsync();
                var fv = await _recepieService.IsFavoriteAsync(selectedRecepie.Id, user.Id);

                if (fv)
                {
                    FavoriteIconPath = "favorite_full.png";
                }
                else
                {
                    FavoriteIconPath = "favorite.png";
                }
            }


        }


        public RecepieDetailsViewModel(IRecepieService recepieService, IUserService userService)
        {
            _recepieService = recepieService;
            _userService = userService;
        }


        [RelayCommand]
        public void IngredientTappedCommand(RecepieIngredient ig)
        {
            if(ig is null) return;

            ig.IsReady = !ig.IsReady;

        }

        [RelayCommand]
        public async Task FavoriteBtn()
        {

            if(!await _userService.IsUserLoggedInAsync())
            {
                return; 
            }
            else
            {
                var user = await _userService.GetCurrentUserAsync();
                await _recepieService.ChangeFavoriteAsync(selectedRecepie.Id, user.Id);

                if(FavoriteIconPath == "favorite.png")
                {
                    FavoriteIconPath = "favorite_full.png";
                }
                else
                {
                    FavoriteIconPath = "favorite.png";
                }
            }
        }

        [RelayCommand]
        public async Task GoBackBtn()
        {
            await Shell.Current.GoToAsync("..");
        }

    }
}
