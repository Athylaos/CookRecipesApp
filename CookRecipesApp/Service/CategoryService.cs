using CookRecipesApp.Model.Category;
using CookRecipesApp.Model.Recepie;
using SQLite;
using System;
using System.Collections.Generic;
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

            List<Category> subCategories = new List<Category>();
            foreach (var subDb in subCategoriesDb)
            {
                subCategories.Add(await CategoryDbToCategoryAsync(subDb));
            }

            Category category = new Category()
            {
                Id = categoryDbModel.Id,
                Name = categoryDbModel.Name,
                ParentCategory = categoryDbModel.ParentCategoryId,
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
                ParentCategoryId = category.ParentCategory,
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
                ParentCategory = db.ParentCategoryId,
                SortOrder= db.SortOrder
            }).ToList();

            var lookup = allCategories.ToDictionary(c => c.Id);

            List<Category> rootCategories = new List<Category>();

            foreach (var dbModel in allDbModels)
            {
                var currentCategory = lookup[dbModel.Id];

                if (dbModel.ParentCategoryId != null && lookup.ContainsKey(dbModel.ParentCategoryId.Value))
                {
                    var parent = lookup[dbModel.ParentCategoryId.Value];
                    parent.SubCategories.Add(currentCategory);

                    currentCategory.ParentCategory = parent.Id;
                }
                else
                {
                    rootCategories.Add(currentCategory);
                }
            }

            if (root)
            {
                return rootCategories.OrderBy(c => c.SortOrder).ToList();
            }
            return allCategories.OrderBy(c => c.SortOrder).ToList();
        }


        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            var categoryDb = await _database.Table<CategoryDbModel>().FirstOrDefaultAsync(c => c.Id == id);
            if (categoryDb == null) return null;

            return await CategoryDbToCategoryAsync(categoryDb);
        }


        public async Task<List<Category>> GetRecepieCategoriesAsync(int recepieId)
        {
            var links = await _database.Table<RecepieCategoryDbModel>()
                                       .Where(rc => rc.RecepieId == recepieId)
                                       .ToListAsync();

            if (links.Count == 0) return new List<Category>();

            var categoryIds = links.Select(l => l.CategoryId).ToList();

            var allDbModels = await _database.Table<CategoryDbModel>().ToListAsync();

            var allCategories = allDbModels.Select(db => new Category
            {
                Id = db.Id,
                Name = db.Name,
                PictureUrl = db.PictureUrl,
                SortOrder = db.SortOrder,
            }).ToList();

            var lookup = allCategories.ToDictionary(c => c.Id);

            foreach (var dbModel in allDbModels)
            {
                if (dbModel.ParentCategoryId != null && lookup.ContainsKey(dbModel.ParentCategoryId.Value))
                {
                    var child = lookup[dbModel.Id];
                    var parent = lookup[dbModel.ParentCategoryId.Value];
                    parent.SubCategories.Add(child);
                    child.ParentCategory = parent.Id;
                }
            }

            var result = allCategories.Where(c => categoryIds.Contains(c.Id)).ToList();

            return result.OrderBy(c => c.SortOrder).ToList();
        }

        public async Task<List<Category>> GetAllSelectedCategories()
        {
            var allCategories = await GetAllCategoriesAsync(false);
            var result = allCategories.Where(c => c.IsSelected).ToList();
            return result;
        }




    }
}
