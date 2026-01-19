using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.Model.Ingredient
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public UnitDbModel DefaultUnit { get; set; } = new();
        public List<UnitDbModel> PossibleUnits { get; set; } = new();

        public float Calories { get; set; }
        public float Proteins { get; set; }
        public float Fats { get; set; }
        public float Carbohydrates { get; set; }
        public float Fiber { get; set; }

    }
}
