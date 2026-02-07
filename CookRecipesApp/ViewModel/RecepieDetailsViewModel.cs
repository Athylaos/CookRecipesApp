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
        private ObservableCollection<Comment> visibleComments = new();


        private int ratingValue;
        [ObservableProperty]
        private DateOnly commentTime;
        [ObservableProperty]
        private Comment commentOfUser;
        [ObservableProperty]
        private bool editorEditable = true;
        [ObservableProperty]
        private bool postBtnVisible = true;
        [ObservableProperty]
        private bool delGridVisible = false;
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
                foreach(var c in SelectedRecepie.Comments.Take(10))
                {
                    VisibleComments.Add(c);
                }

                await CommentStatus();
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
        public async  Task PostComment()
        {
            if(!await _userService.IsUserLoggedInAsync())
            {
                Shell.Current.GoToAsync(nameof(LoginPage));
                return;
            }

            if (string.IsNullOrEmpty(CommentText))
            {
                CommentText = string.Empty;
            }
            var user = await _userService.GetCurrentUserAsync();

            Comment comment = new Comment()
            {
                RecepieId = recepieId,
                UserId = user.Id,
                UserName = user.Name,
                Text = CommentText,
                Rating = ratingValue,
                CreatedAt = DateTime.Now
            };

            CommentOfUser = comment;
            CommentTime = DateOnly.FromDateTime(comment.CreatedAt);
            await _recepieService.PostCommentAsync(comment);

            VisibleComments.Add(comment);

            PostBtnVisible = false;
            EditorEditable = false;
            DelGridVisible = true;
        }

        [RelayCommand]
        private void SelectRating(int selectedRating)
        {
            UpdateRatingStars(selectedRating);
        }

        private void UpdateRatingStars(int rating)
        {
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
            ratingValue = rating;
        }

        private async Task CommentStatus()
        {
            var user = await _userService.GetCurrentUserAsync();
            if(user != null)
            {
                if(await _recepieService.UserCommentedAsync(recepieId, user.Id))
                {
                    CommentOfUser = await _recepieService.GetCommentByUserAndRecepieAsync(recepieId, user.Id) ?? new();
                    CommentTime = DateOnly.FromDateTime(CommentOfUser.CreatedAt);
                    CommentText = CommentOfUser.Text;
                    SelectRating(CommentOfUser.Rating);
                    PostBtnVisible = false;
                    EditorEditable = false;
                    DelGridVisible = true;
                    return;
                }
            }
            PostBtnVisible = true;
            EditorEditable = true;
            DelGridVisible = false;
        }

        [RelayCommand]
        private async Task DeleteComment()
        {
            if(CommentOfUser == null)
            {
                return;
            }

            var user = await _userService.GetCurrentUserAsync();
            await _recepieService.DeleteCommentByUserAndRecepieAsync(recepieId, user.Id);
            VisibleComments.Remove(CommentOfUser);

            PostBtnVisible = true;
            EditorEditable = true;
            DelGridVisible = false;

            CommentOfUser = new();
            SelectRating(1);
            CommentText = "";
        }
    }

}

