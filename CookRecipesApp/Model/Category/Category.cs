
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.Model.Category
{
    public partial class Category : ObservableObject
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PictureUrl { get; set; } = "default_picture.png";

        public int SortOrder { get; set; }

        [ObservableProperty] bool isSelected;

        public int? ParentCategory { get; set; }
        public List<Category> SubCategories = new List<Category>();
    }
}
