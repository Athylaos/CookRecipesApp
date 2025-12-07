using CookRecipesApp.Model.Category;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CookRecipesApp.Service
{
    public class DatabaseSeederService
    {
        private readonly ISQLiteAsyncConnection _database;

        public DatabaseSeederService(ISQLiteAsyncConnection database)
        {
            _database = database;
        }

        public async Task SeedCategoriesAsync()
        {
            await _database.CreateTableAsync<CategoryDbModel>();

            var count = await _database.Table<CategoryDbModel>().CountAsync();

            if (count == 0)
            {
                var categoriesToSeed = new List<CategoryDbModel>
            {
                new CategoryDbModel
                {
                    Name = "Breakfast", // Breakfast
                    PictureUrl = "breakfast_icon.png",
                    SortOrder = 1,
                    ParentCategoryId = null
                },
                new CategoryDbModel
                {
                    Name = "Lunch", // Lunch
                    PictureUrl = "lunch_icon.png",
                    SortOrder = 2,
                    ParentCategoryId = null
                },
                new CategoryDbModel
                {
                    Name = "Dinner", // Dinner
                    PictureUrl = "dinner_icon.png",
                    SortOrder = 3,
                    ParentCategoryId = null
                },
                new CategoryDbModel
                {
                    Name = "Soups", // Soups
                    PictureUrl = "soup_icon.png",
                    SortOrder = 4,
                    ParentCategoryId = null
                },
                new CategoryDbModel
                {
                    Name = "Desserts", // Desserts
                    PictureUrl = "dessert_icon.png",
                    SortOrder = 5,
                    ParentCategoryId = null
                },
                new CategoryDbModel
                {
                    Name = "Snacks", // Snacks
                    PictureUrl = "snack_icon.png",
                    SortOrder = 6,
                    ParentCategoryId = null
                },
                new CategoryDbModel
                {
                    Name = "Drinks", // Drinks
                    PictureUrl = "drinks_icon.png",
                    SortOrder = 7,
                    ParentCategoryId = null
                },
                new CategoryDbModel
                {
                    Name = "Salads", // Salads
                    PictureUrl = "salad_icon.png",
                    SortOrder = 8,
                    ParentCategoryId = null
                }
            };
                await _database.InsertAllAsync(categoriesToSeed);
            }
        }
    }
}

