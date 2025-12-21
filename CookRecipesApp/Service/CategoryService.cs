using CommunityToolkit.Maui.Core.Extensions;
using CookRecipesApp.Model.Category;
using CookRecipesApp.Model.Recepie;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;


namespace CookRecipesApp.Service
{

    public interface ICategoryService
    {
        public Task<List<Category>> GetRecepieCategoriesAsync(int recepieId);

        public Task<List<Category>> GetAllCategoriesAsync(bool root);
        public Task<Category?> GetCategoryByIdAsync(int id);
    }

    public class CategoryService : ICategoryService
    {
        private readonly ISQLiteAsyncConnection _database;

        public CategoryService(SQLiteConnectionFactory factory)
        {
            _database = factory.CreateConnection();
        }

        private async Task<Category> CategoryDbToCategoryAsync(CategoryDbModel categoryDbModel)
        {
            var subCategoriesDb = await _database.Table<CategoryDbModel>()
                                                 .Where(c => c.ParentCategoryId == categoryDbModel.Id)
                                                 .ToListAsync();

            ObservableCollection<Category> subCategories = new();
            foreach (var subDb in subCategoriesDb)
            {
                subCategories.Add(await CategoryDbToCategoryAsync(subDb));
            }

            Category category = new Category()
            {
                Id = categoryDbModel.Id,
                Name = categoryDbModel.Name,
                ParentCategoryId = categoryDbModel.ParentCategoryId,
                SubCategories = subCategories,
                SortOrder = categoryDbModel.SortOrder
            };

            return category;
        }

        private CategoryDbModel CategoryToCategoryDbModel(Category category)
        {
            return new CategoryDbModel
            {
                Id = category.Id,
                Name = category.Name,
                PictureUrl = category.PictureUrl,
                ParentCategoryId = category.ParentCategoryId,
                SortOrder = category.SortOrder
                
            };
        }

        public async Task<List<Category>> GetAllCategoriesAsync(bool root)
        {
            var allDbModels = await _database.Table<CategoryDbModel>().ToListAsync();

            var allCategories = allDbModels.Select(db => new Category
            {
                Id = db.Id,
                Name = db.Name,
                PictureUrl = db.PictureUrl,
                ParentCategoryId = db.ParentCategoryId,
                SortOrder = db.SortOrder
            }).ToList();

            var lookup = allCategories.ToDictionary(c => c.Id);

            var rootCategories = new List<Category>();

            foreach (var dbModel in allDbModels)
            {
                var currentCategory = lookup[dbModel.Id];

                if (dbModel.ParentCategoryId != null &&
                    lookup.TryGetValue(dbModel.ParentCategoryId.Value, out var parent))
                {
                    parent.SubCategories.Add(currentCategory);
                    currentCategory.ParentCategoryId = parent.Id;
                }
                else
                {
                    rootCategories.Add(currentCategory);
                }
            }

            return (root ? rootCategories : allCategories)
                .OrderBy(c => c.SortOrder)
                .ToList();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            var all = await GetAllCategoriesAsync(false);

            return all.FirstOrDefault(c => c.Id == id);
        }

        public async Task<List<Category>> GetRecepieCategoriesAsync(int recepieId)
        {
            var links = await _database.Table<RecepieCategoryDbModel>()
                                       .Where(rc => rc.RecepieId == recepieId)
                                       .ToListAsync();

            if (links.Count == 0)
                return new List<Category>();

            var categoryIds = links.Select(l => l.CategoryId).ToHashSet();

            var allCategories = await GetAllCategoriesAsync(false);

            var result = allCategories
                .Where(c => categoryIds.Contains(c.Id))
                .OrderBy(c => c.SortOrder)
                .ToList();

            return result;
        }





    }
}
