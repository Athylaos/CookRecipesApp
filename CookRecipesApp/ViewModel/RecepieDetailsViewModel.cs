using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookRecipesApp.Shared.Models;
using CookRecipesApp.Shared.DTOs;
using CookRecipesApp.Service.Interface;
using CookRecipesApp.View;
using CookRecipesApp.ModelsUI;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;


namespace CookRecipesApp.ViewModel
{
    [QueryProperty(nameof(RecipeId), "RecipeId")]
    public partial class RecipeDetailsViewModel : ObservableObject
    {

        private IRecipeService _recipeService;
        private IUserService _userService;

        Guid recipeId;
        public Guid RecipeId
        {
            get => recipeId;
            set
            {
                SetProperty(ref recipeId, value);
            }
        }

        [ObservableProperty]
        private RecipeDetailsDto selectedRecipe = new();
        [ObservableProperty]
        private string favoriteIconPath = "favorite.png";
        [ObservableProperty]
        private bool isLoading = true;
        [ObservableProperty]
        private ObservableCollection<Comment> visibleComments = new();

        private int ratingValue;
        [ObservableProperty]
        private DateOnly commentTime;
        [ObservableProperty]
        private Comment commentOfUser = new Comment();
        [ObservableProperty]
        private bool editorEditable = true;
        [ObservableProperty]
        private bool postBtnVisible = true;
        [ObservableProperty]
        private bool delGridVisible = false;
        [ObservableProperty]
        private string commentText = string.Empty;
        [ObservableProperty]
        private ObservableCollection<RatingStar> ratingStars = new ObservableCollection<RatingStar>
        {
            new RatingStar { RatingValue = 1, Icon = "favorite.png" },
            new RatingStar { RatingValue = 2, Icon = "favorite.png" },
            new RatingStar { RatingValue = 3, Icon = "favorite.png" },
            new RatingStar { RatingValue = 4, Icon = "favorite.png" },
            new RatingStar { RatingValue = 5, Icon = "favorite.png" }
        };


        public async Task LoadRecipeAsync(Guid id)
        {
            IsLoading = true;

            try
            {
                var result = await _recipeService.GetRecipeDetailsAsync(id);

                if (result == null)
                {
                    // Ošetření, když recept neexistuje
                    await Shell.Current.DisplayAlertAsync("Chyba", "Recept se nepodařilo načíst.", "OK");
                    return;
                }

                SelectedRecipe = result;
                /*
                                if (!await _userService.IsUserLoggedInAsync())
                                {
                                    FavoriteIconPath = "favorite.png";
                                }
                                else
                                {
                                    var user = await _userService.GetCurrentUserAsync();
                                    var fv = await _recipeService.IsFavoriteAsync(SelectedRecipe.Id, user.Id);

                                    FavoriteIconPath = fv ? "favorite_full.png" : "favorite.png";
                                }
                */
                FavoriteIconPath = SelectedRecipe.IsFavorite ? "favorite_full.png" : "favorite.png";
                VisibleComments.Clear();
                foreach(var c in SelectedRecipe.Comments.Take(10))
                {
                    VisibleComments.Add(c);
                }

                //await CommentStatus();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading recipe: {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        public RecipeDetailsViewModel(IRecipeService recipeService, IUserService userService)
        {
            _recipeService = recipeService;
            _userService = userService;
        }

        [RelayCommand]
        public void IngredientTappedCommand(RecipeIngredient ig)
        {
            if(ig is null) return;

            //ig.IsReady = !ig.IsReady;

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
                await _recipeService.ChangeFavoriteAsync(selectedRecipe.Id, user.Id);

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
                RecipeId = recipeId,
                UserId = user.Id,
                User = user,
                Text = CommentText,
                Rating = (short)ratingValue,
                CreatedAt = DateTime.Now
            };

            CommentOfUser = comment;
            CommentTime = DateOnly.FromDateTime(comment.CreatedAt??DateTime.Now);
            var res = await _recipeService.PostCommentAsync(comment);

            SelectedRecipe.Rating = (decimal)res.Item1;
            SelectedRecipe.UsersRated = res.Item2;

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
                if(await _recipeService.UserCommentedAsync(recipeId, user.Id))
                {
                    CommentOfUser = await _recipeService.GetCommentByUserAndRecipeAsync(recipeId, user.Id) ?? new();
                    CommentTime = DateOnly.FromDateTime(CommentOfUser.CreatedAt??DateTime.Now);
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
            CommentText = "";
            SelectRating(1);
        }

        [RelayCommand]
        private async Task DeleteComment()
        {
            if(CommentOfUser == null)
            {
                return;
            }

            var user = await _userService.GetCurrentUserAsync();
            var res = await _recipeService.DeleteCommentByUserAndRecipeAsync(recipeId, user.Id);
            SelectedRecipe.Rating = (decimal)res.Item1;
            SelectedRecipe.UsersRated = res.Item2;
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

