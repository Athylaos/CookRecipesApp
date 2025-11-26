using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.Model
{
    public class RecepieIngredientDbModel
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int Id { get; set; }

        [NotNull]
        public int RecepieId { get; set; }

        [NotNull]
        public int IngredientId { get; set; }
    }
}
