using CommunityToolkit.Mvvm.ComponentModel;
using CookRecipesApp.Model.Ingredient;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.Model.Recepie
{
    public class RecepiePreview
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string PhotoPath { get; set; } = string.Empty;
        public int CookingTime { get; set; }
        public int Servings { get; set; }
        public UnitDbModel ServingUnit { get; set; } = new();
        public DifficultyLevel DifficultyLevel { get; set; }

        public DateTime RecepieCreated = DateTime.Now;

        private float Rating {  get; set; }
        private int UsersRated { get; set; }

        public Nutritions NutritionsPerUnit { get; set; } = new();

        public List<Category.Category> Categories { get; set; } = new();
        public List<Category.Category> SubCategories { get; set; } = new();

    }
}
