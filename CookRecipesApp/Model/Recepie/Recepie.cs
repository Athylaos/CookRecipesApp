using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.Model.Recepie
{
    public class Recepie
    {

        public Recepie(int Id, int UserId, string Title, string CoockingProcess, int CoockingTime, int Servings, List<Ingredient.Ingredient> Ingredients, float Calories, float Proteins, float Fats, float Carbohydrates, float Fiber)
        {
            this.Id = Id;
            this.UserId = UserId;
            this.Title = Title;
            this.CoockingProcess = CoockingProcess;
            this.CoockingTime = CoockingTime;
            this.Servings = Servings;
            this.Ingredients = Ingredients;
            this.Calories = Calories;
            this.Proteins = Proteins;
            this.Fats = Fats;
            this.Carbohydrates = Carbohydrates;
            this.Fiber = Fiber;
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string CoockingProcess { get; set; } = string.Empty;
        public int CoockingTime { get; set; }
        public int Servings { get; set; }
        public List<Model.Ingredient.Ingredient> Ingredients { get; set; } = new List<Model.Ingredient.Ingredient>();

        public float Calories { get; set; }
        public float Proteins { get; set; }
        public float Fats { get; set; }
        public float Carbohydrates { get; set; }
        public float Fiber { get; set; }



    }
}
