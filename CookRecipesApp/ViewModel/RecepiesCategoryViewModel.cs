using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookRecipesApp.Model.Category;
using CookRecipesApp.Model.Recepie;
using CookRecipesApp.Service;
using CookRecipesApp.Service.Interface;
using CookRecipesApp.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CookRecipesApp.ViewModel
{
    [QueryProperty(nameof(CategoryId), "CategoryId")]
    public partial class RecepiesCategoryViewModel : ObservableObject
    {
        private readonly ICategoryService _categoryService;
        private readonly IRecepieService _recepieService;

        public RecepiesCategoryViewModel(ICategoryService categoryService, IRecepieService recepieService)
        {
            _categoryService = categoryService;
            _recepieService = recepieService;
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

        [ObservableProperty]
        private ObservableCollection<Recepie> favoriteRecepies = new ObservableCollection<Recepie>();

        public async Task LoadCategoryAsync(int id)
        {
            SelectedCategory = await _categoryService.GetCategoryByIdAsync(id);

            var favoriteRecepies = await _recepieService.GetRecepiesAsync(3);

            foreach(var fvr in  favoriteRecepies)
            {
                FavoriteRecepies.Add(fvr);
            }
        }

        [RelayCommand]
        public async Task RecepieBtn(Recepie recepie)
        {
            if (recepie == null) return;

            await Shell.Current.GoToAsync($"{nameof(RecepieDetailsPage)}?RecepieId={recepie.Id}", true);
        }

        [RelayCommand]
        public void RecepiesMainPageBtn()
        {
            Shell.Current.GoToAsync("//RecepiesMainPage");
        }
    }
}
