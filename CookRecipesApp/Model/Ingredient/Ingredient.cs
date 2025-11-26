using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.Model.Ingredient
{
    public class Ingredient
    {
        public Ingredient(int id, string name, float quantity, UnitDbModel unit, float calories, float proteins, float fats, float carbohydrates, float fiber)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            Unit = unit;
            Calories = calories;
            Proteins = proteins;
            Fats = fats;
            Carbohydrates = carbohydrates;
            Fiber = fiber;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public float Quantity { get; set; }
        public UnitDbModel Unit { get; set; }

        public float Calories { get; set; }
        public float Proteins { get; set; }
        public float Fats { get; set; }
        public float Carbohydrates { get; set; }
        public float Fiber { get; set; }

    }
}
