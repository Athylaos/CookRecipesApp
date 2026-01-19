using CommunityToolkit.Mvvm.ComponentModel;
using CookRecipesApp.Service;
using CookRecipesApp.Service.Interface;
using CookRecipesApp.Model.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.ViewModel
{
    [QueryProperty(nameof(CategoryId), "CategoryId")]
    public partial class RecepiesCategoryViewModel : ObservableObject
    {
        private readonly ICategoryService _categoryService;

        public RecepiesCategoryViewModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
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
