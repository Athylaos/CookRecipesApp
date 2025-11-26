using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.Model.Recepie
{
    public class Recepie
    {
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
