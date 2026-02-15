using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.Model.Recepie
{
    public class RecepieCategoryDbModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull]
        public int RecepieId { get; set; }
        [NotNull]
        public int CategoryId { get; set; }

    }
}
