using System;
using System.Collections.Generic;
using System.Text;
using CookRecipesApp.Model.Category;
using CookRecipesApp.Model.Ingredient;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CookRecipesApp.Model.Recepie
{
    public enum DifficultyLevel
    {
        Easy = 1,
        Medium = 2,
        Hard = 3,
        Chef = 4,
    }

    public partial class Recepie : ObservableObject
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string PhotoPath { get; set; } = string.Empty;
        public List<RecepieStep> Steps { get; set; } = new List<RecepieStep>();
        public int CookingTime { get; set; }
        public int Servings { get; set; }
        public UnitDbModel ServingUnit { get; set; } = new();
        public DifficultyLevel DifficultyLevel { get; set; }
        public List<RecepieIngredient> Ingredients { get; set; } = new List<RecepieIngredient>();

        public DateTime RecepieCreated = DateTime.Now;

        [ObservableProperty]
        private float rating;
        [ObservableProperty]
        private int usersRated;
        public List<Comment> Comments { get; set; } = new();

        public Nutritions NutritionsPerUnit { get; set; } = new();

        public List<Category.Category> Categories { get; set; } = new();
        public List<Category.Category> SubCategories { get; set; } = new();
    }

}
