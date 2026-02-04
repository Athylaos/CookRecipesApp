using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookRecipesApp.Model.Category;
using CookRecipesApp.Model.Recepie;
using CookRecipesApp.Service.Interface;
using CookRecipesApp.View;
using CookRecipesApp.Model.Help;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

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
        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string commentText;
        [ObservableProperty]
        private ObservableCollection<RatingStar> ratingStars = new ObservableCollection<RatingStar>
        {
            new RatingStar { RatingValue = 1, Icon = "favorite.png" },
            new RatingStar { RatingValue = 2, Icon = "favorite.png" },
            new RatingStar { RatingValue = 3, Icon = "favorite.png" },
            new RatingStar { RatingValue = 4, Icon = "favorite.png" },
            new RatingStar { RatingValue = 5, Icon = "favorite.png" }
        };


        private async Task LoadRecepieAsync(int id)
        {
            IsLoading = true;

            try
            {
                SelectedRecepie = await _recepieService.GetRecepieAsync(id);

                if (!await _userService.IsUserLoggedInAsync())
                {
                    FavoriteIconPath = "favorite.png";
                }
                else
                {
                    var user = await _userService.GetCurrentUserAsync();
                    var fv = await _recepieService.IsFavoriteAsync(SelectedRecepie.Id, user.Id);

                    FavoriteIconPath = fv ? "favorite_full.png" : "favorite.png";
                }
            }
            finally
            {
                IsLoading = false;
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
                Shell.Current.GoToAsync(nameof(LoginPage));
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

        [RelayCommand]
        public async  Task ShowAddCommandBtn()
        {

        }

        [RelayCommand]
        private void SelectRating(int selectedRating)
        {
            UpdateRatingStars(selectedRating);
        }

        private void UpdateRatingStars(int rating)
        {
            Debug.WriteLine($"Hvezda cislo:{rating}");
            RatingStars.Clear();
            for (int i = 1; i <= 5; i++)
            {
                var star = new RatingStar
                {
                    RatingValue = i,
                    Icon = i <= rating ? "favorite_full.png" : "favorite.png"
                };
                RatingStars.Add(star);
            }
        }
    }

}

