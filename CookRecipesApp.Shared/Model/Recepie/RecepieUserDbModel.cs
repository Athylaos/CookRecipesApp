using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.Model.Recepie
{
    public class RecepieUserDbModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull]
        public int UserId { get; set; }
        [NotNull]
        public int RecepieId { get; set; }

        public bool IsFavorite { get; set; }


    }
}
