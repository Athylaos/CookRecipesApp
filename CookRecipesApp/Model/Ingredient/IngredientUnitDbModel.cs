using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.Model.Ingredient
{
    public class IngredientUnitDbModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int UnitId { get; set; }
        public int IngredientId {  get; set; }
        public float ToDefaultUnit { get; set; }



    }
}
