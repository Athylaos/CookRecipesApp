using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CookRecipesApp.Model.Ingredient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CookRecipesApp.Service;
using CookRecipesApp.Model.Category;
using CommunityToolkit.Mvvm.Input;
using CookRecipesApp.Model.Recepie;


namespace CookRecipesApp.ViewModel
{
    public partial class AddRecepieViewModel : ObservableObject
    {
        [ObservableProperty]
        private Recepie newRecepie;

        [ObservableProperty] private string test;
        public ObservableCollection<UnitDbModel> ServingUnits { get; } = new ObservableCollection<UnitDbModel>();
        [ObservableProperty] private UnitDbModel selectedServingUnit;

        public List<DifficultyLevel> DifficultyOptions { get; } = Enum.GetValues(typeof(DifficultyLevel)).Cast<DifficultyLevel>().ToList();
        [ObservableProperty] DifficultyLevel difficulty;

        public ObservableCollection<RecepieIngredient> Ingredients { get; } = new();

        public ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>();



        public AddRecepieViewModel()
        {
            Test = "test ok";
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
        public void CategoryBtn(Category category)
        {
            if(category != null)
            {
                category.IsSelected = !category.IsSelected;
            }

        }
    }
}
