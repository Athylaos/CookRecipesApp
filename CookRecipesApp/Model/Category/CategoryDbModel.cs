using SQLite;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace CookRecipesApp.Model.Category
{
    public class CategoryDbModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }
        public string PictureUrl { get; set; } = "default_picture.png";
        public int SortOrder { get; set; }

        public int? ParentCategoryId { get; set; }


    }
}
