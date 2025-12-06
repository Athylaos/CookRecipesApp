using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using CookRecipesApp.Model.Category;

namespace CookRecipesApp.Model.Recepie
{
    public enum DifficultyLevel
    {
        Easy = 1,
        Medium = 2,
        Hard = 3,
        Chef = 4,
    }

    public class Recepie
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string CoockingProcess { get; set; } = string.Empty;
        public int CoockingTime { get; set; }
        public int Servings { get; set; }
        public DifficultyLevel DifficultyLevel { get; set; }     
        public List<Model.Ingredient.Ingredient> Ingredients { get; set; } = new List<Model.Ingredient.Ingredient>();

        public float Calories { get; set; }
        public float Proteins { get; set; }
        public float Fats { get; set; }
        public float Carbohydrates { get; set; }
        public float Fiber { get; set; }

        public DateTime RecepieCreated = DateTime.Now;
        public int Rating { get; set; }
        public int UsersRated { get; set; }
        public List<Comment> Comments { get; set; } = new();

        public List<CategoryDbModel> Categories { get; set; } = new();
        public List<CategoryDbModel> SubCategories { get; set; } = new();







    }
}
