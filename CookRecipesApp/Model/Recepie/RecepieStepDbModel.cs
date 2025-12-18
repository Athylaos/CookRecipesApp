using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.Model.Recepie
{
    public class RecepieStepDbModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull]
        public int RecepieId { get; set; }
        public string ContentText { get; set; } = string.Empty;
        public int Orded {  get; set; }


    }
}
