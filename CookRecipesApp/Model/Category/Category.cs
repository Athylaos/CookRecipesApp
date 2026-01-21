
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using System.Diagnostics;

namespace CookRecipesApp.Model.Category
{
    public partial class Category : ObservableObject
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PictureUrl { get; set; } = "default_picture.png";

        public int SortOrder { get; set; }

        [ObservableProperty] bool isSelected;

        public int? ParentCategoryId { get; set; }
        public ObservableCollection<Category> SubCategories = new();


        partial void OnIsSelectedChanged(bool value)
        {
            Debug.WriteLine($"Category {Name} IsSelected changed to: {value}");
        }

    }
}
