using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.Model.Recepie
{
    public class RecepieIngredientDbModel
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int Id { get; set; }

        [NotNull, Indexed]
        public int RecepieId { get; set; }

        [NotNull, Indexed]
        public int IngredientId { get; set; }

        public float Quantity { get; set; }
        public int UnitId { get; set; }
    }
}
