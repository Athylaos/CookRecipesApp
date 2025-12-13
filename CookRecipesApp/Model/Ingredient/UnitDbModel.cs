using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.Model.Ingredient
{
    public class UnitDbModel
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int Id { get; set; }

        [NotNull, Unique]
        public string Name { get; set; } = string.Empty;
        [NotNull]
        public bool IsServingUnit {  get; set; }

        public float ToDefaultUnit { get; set; }
    }
}
