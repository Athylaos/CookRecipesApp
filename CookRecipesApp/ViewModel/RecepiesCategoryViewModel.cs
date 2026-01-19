using CommunityToolkit.Mvvm.ComponentModel;
using CookRecipesApp.Service;
using CookRecipesApp.Model.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.ViewModel
{
    [QueryProperty(nameof(CategoryId), "CategoryId")]
    public partial class RecepiesCategoryViewModel : ObservableObject
    {
        private readonly CategoryService _categoryService;

        public RecepiesCategoryViewModel()
        {
            _categoryService = new(new SQLiteConnectionFactory());
        }

        int categoryId;
        public int CategoryId
        {
            get => categoryId;
            set
            {
                SetProperty(ref categoryId, value);
                _ = LoadCategoryAsync(value);
            }
        }

        [ObservableProperty]
        Category selectedCategory;

        private async Task LoadCategoryAsync(int id)
        {
            SelectedCategory = await _categoryService.GetCategoryByIdAsync(id);

        }
    }
}
