using SQLite;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace CookRecipesApp.Model.Recepie
{
    public class CategoryDbModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        public int? ParentCategoryId { get; set; }


    }
}
