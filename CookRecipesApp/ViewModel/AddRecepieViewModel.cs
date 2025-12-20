using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookRecipesApp.Model.Category;
using CookRecipesApp.Model.Ingredient;
using CookRecipesApp.Model.Recepie;
using CookRecipesApp.Service;
using CookRecipesApp.View.Popups;
using CookRecipesApp.ViewModel.Popups;
using Microsoft.Maui.Controls.Shapes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;


namespace CookRecipesApp.ViewModel
{
    public partial class AddRecepieViewModel : ObservableObject
    {
        private IngredientsService _ingredientsService;
        private RecepiesService _recepiesService;

        [ObservableProperty] string title;
        [ObservableProperty] string time;
        [ObservableProperty] string servings;
        [ObservableProperty] string photoPath;
        public ObservableCollection<UnitDbModel> ServingUnits { get; } = new ObservableCollection<UnitDbModel>();
        [ObservableProperty] private UnitDbModel selectedServingUnit;

        public List<DifficultyLevel> DifficultyOptions { get; } = Enum.GetValues(typeof(DifficultyLevel)).Cast<DifficultyLevel>().ToList();
        [ObservableProperty] DifficultyLevel difficulty = DifficultyLevel.Medium;

        public ObservableCollection<RecepieIngredient> Ingredients { get; } = new();

        public ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>();

        public ObservableCollection<RecepieStep> RecepieSteps { get; } = new ObservableCollection<RecepieStep>();

        [ObservableProperty] string warningText;
        [ObservableProperty] bool warningEnabled;



        public AddRecepieViewModel()
        {
            SQLiteConnectionFactory cf = new();
            _ingredientsService = new(cf);
            _recepiesService = new(cf, _ingredientsService, new CategoryService(cf));
            AddCookingStepBtn();
            PhotoPath = "default_picture.png";
        }

        public async Task StartAsync()
        {
            var su = await new IngredientsService(new SQLiteConnectionFactory()).GetAllServingUnitsAsync();

            foreach(var s in su)
            {
                ServingUnits.Add(s);
            }
            SelectedServingUnit = ServingUnits.FirstOrDefault(su => su.Name == "portion") ?? ServingUnits.First();

            var ca = await new CategoryService(new SQLiteConnectionFactory()).GetAllCategoriesAsync(false);

            foreach(var c in ca)
            {
                Categories.Add(c);
            }


            
        }

        [RelayCommand]
        public async Task AddPhotoBtn()
        {
            try
            {
                var result = await MediaPicker.PickPhotosAsync(new MediaPickerOptions
                {
                    Title = "Choose a photo for the recipe"
                });

                if (result != null)
                {
                    // Lokální cesta k souboru
                    PhotoPath = result.First().FullPath;

                    var newPath = System.IO.Path.Combine(FileSystem.AppDataDirectory, result.First().FileName);
                    using var source = await result.First().OpenReadAsync();
                    using var dest = File.OpenWrite(newPath);
                    await source.CopyToAsync(dest);
                    PhotoPath = newPath;
                }
            }
            catch (Exception ex)
            {
                
            }
        }


        [RelayCommand]
        public void CategoryBtn(Category category)
        {
            if(category != null)
            {
                category.IsSelected = !category.IsSelected;
            }

        }

        [RelayCommand]
        public async Task OpenAddIngredientPopup()
        {
            List<Ingredient> ingredients = await _ingredientsService.GetAllIngredientsAsync();

            var popupVm = new AddIngredientPopupViewModel(ingredients);

            popupVm.OnCloseRequest += (resultData) =>
            {
                if (resultData is RecepieIngredient newIngredient)
                {
                    this.Ingredients.Add(newIngredient);
                }
            };

            var popup = new AddIngredientPopup();
            popup.BindingContext = popupVm;

            await Shell.Current.ShowPopupAsync(popup);
        }

        [RelayCommand]
        public void DelIngredientBtn(RecepieIngredient ingredient)
        {
            Ingredients.Remove(ingredient);
        }

        [RelayCommand]
        public void AddCookingStepBtn()
        {
            RecepieSteps.Add(new RecepieStep { Order = (RecepieSteps.Count+1)});
        }

        [RelayCommand]
        public void RemoveCookingStepBtn()
        {
            if(RecepieSteps.Count > 1)
            {
                RecepieSteps.RemoveAt(RecepieSteps.Count - 1);
            }
        }

        [RelayCommand]
        public async Task FinishRecepieBtn()
        {
            int timeInt = int.TryParse(Time, out var q) ? q : 0;
            int servingsInt = int.TryParse(Servings, out var p) ? p : 0;

            if (string.IsNullOrWhiteSpace(Title))
            {
                WarningEnabled = true;
                WarningText = "Title is mandatory";
                return;
            }
            if (string.IsNullOrWhiteSpace(Time) || q == 0)
            {
                WarningEnabled = true;
                WarningText = "Time is mandatory and can't be 0";
                return;
            }
            if (string.IsNullOrWhiteSpace(Servings) || p == 0)
            {
                WarningEnabled = true;
                WarningText = "Number of servings is mandatory and can't be 0";
                return;
            }
            if(Categories.Where(c => c.IsSelected == true) == null)
            {
                WarningEnabled = true;
                WarningText = "Recepie must have at least one category";
                return;
            }
            if(Ingredients.Count == 0)
            {
                WarningEnabled = true;
                WarningText = "Recepie must have at least one ingredient";
                return;
            }
            foreach(var rs in RecepieSteps)
            {
                if (string.IsNullOrEmpty(rs.ContentText))
                {
                    WarningEnabled = true;
                    WarningText = $"Cooking step can't be empty ({rs.Order})";
                    return;
                }
            }

            Recepie rc = new Recepie
            {
                Title = Title,
                CoockingTime = timeInt,
                Servings = servingsInt,
                ServingUnit = SelectedServingUnit,
                DifficultyLevel = Difficulty,
                Categories = Categories.Where(c => c.IsSelected).ToList(),
                Ingredients = Ingredients.ToList(),
                Steps = RecepieSteps.ToList(),
                PhotoPath = PhotoPath,
            };

            await _recepiesService.SaveRecepieAsync(rc);
            Title = string.Empty;
            Time = string.Empty;
            Servings = string.Empty;
            Difficulty = DifficultyLevel.Medium;
            Categories.Clear();
            Ingredients.Clear();
            RecepieSteps.Clear();


            await Shell.Current.Navigation.PopAsync(true);





        }

    }
}
