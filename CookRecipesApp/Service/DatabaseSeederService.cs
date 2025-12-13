using CookRecipesApp.Model.Category;
using CookRecipesApp.Model.Ingredient;
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

        public async Task SeedUnitsAsync()
        {
            var count = await _database.Table<UnitDbModel>().CountAsync();
            if (count > 0)
            {
                return;
            }

            var units = new List<UnitDbModel>
    {
        // --- Weight---
        new UnitDbModel { Name = "g", IsServingUnit = false },
        new UnitDbModel { Name = "kg", IsServingUnit = false },
        new UnitDbModel { Name = "mg", IsServingUnit = false },
        new UnitDbModel { Name = "oz", IsServingUnit = false },  // Ounce
        new UnitDbModel { Name = "lb", IsServingUnit = false },  // Pound

        // --- Volume---
        new UnitDbModel { Name = "ml", IsServingUnit = false },
        new UnitDbModel { Name = "l", IsServingUnit = true },
        new UnitDbModel { Name = "tsp", IsServingUnit = false }, // Teaspoon
        new UnitDbModel { Name = "tbsp", IsServingUnit = false },// Tablespoon
        new UnitDbModel { Name = "cup", IsServingUnit = true },
        new UnitDbModel { Name = "fl oz", IsServingUnit = false },
        new UnitDbModel { Name = "pint", IsServingUnit = true },
        new UnitDbModel { Name = "gallon", IsServingUnit = true },

        // --- Pieces / Counts---
        new UnitDbModel { Name = "pcs", IsServingUnit = false }, // Pieces
        new UnitDbModel { Name = "slice", IsServingUnit = true },
        new UnitDbModel { Name = "pinch", IsServingUnit = false },
        new UnitDbModel { Name = "handful", IsServingUnit = false },
        new UnitDbModel { Name = "package", IsServingUnit = false },

        // --- Serving Units---
        new UnitDbModel { Name = "portion", IsServingUnit = true },
        new UnitDbModel { Name = "piece", IsServingUnit = true},
        new UnitDbModel { Name = "serving", IsServingUnit = true },
        new UnitDbModel { Name = "plate", IsServingUnit = true },
        new UnitDbModel { Name = "bowl", IsServingUnit = true },
        new UnitDbModel { Name = "batch", IsServingUnit = true },
        new UnitDbModel { Name = "drink", IsServingUnit = true }
    };

            await _database.InsertAllAsync(units);
        }

        private async Task<int> GetUnitId(string unitName)
        {
            var unit = await _database.Table<UnitDbModel>()
                                      .FirstOrDefaultAsync(u => u.Name == unitName);
            return unit?.Id ?? -1; // Vrať -1, pokud se nenajde
        }

        public async Task SeedIngredientsAsync()
        {
            // Zkontroluj, jestli už data neexistují
            var count = await _database.Table<IngredientDbModel>().CountAsync();
            if (count > 0)
            {
                return;
            }

            // Získej ID základních jednotek
            var g = await GetUnitId("g");
            var ml = await GetUnitId("ml");
            var pcs = await GetUnitId("pcs");

            var ingredients = new List<IngredientDbModel>
    {
        // Vegetables
        new IngredientDbModel { Name = "Onion", Quantity = 0, UnitId = g, Calories = 40, Proteins = 1.1f, Fats = 0.1f, Carbohydrates = 9.3f, Fiber = 1.7f },
        new IngredientDbModel { Name = "Garlic", Quantity = 0, UnitId = g, Calories = 149, Proteins = 6.4f, Fats = 0.5f, Carbohydrates = 33.1f, Fiber = 2.1f },
        new IngredientDbModel { Name = "Carrot", Quantity = 0, UnitId = g, Calories = 41, Proteins = 0.9f, Fats = 0.2f, Carbohydrates = 9.6f, Fiber = 2.8f },
        new IngredientDbModel { Name = "Tomato", Quantity = 0, UnitId = g, Calories = 18, Proteins = 0.9f, Fats = 0.2f, Carbohydrates = 3.9f, Fiber = 1.2f },
        new IngredientDbModel { Name = "Potato", Quantity = 0, UnitId = g, Calories = 77, Proteins = 2f, Fats = 0.1f, Carbohydrates = 17f, Fiber = 2.2f },

        // Fruits
        new IngredientDbModel { Name = "Apple", Quantity = 0, UnitId = g, Calories = 52, Proteins = 0.3f, Fats = 0.2f, Carbohydrates = 14f, Fiber = 2.4f },
        new IngredientDbModel { Name = "Banana", Quantity = 0, UnitId = g, Calories = 89, Proteins = 1.1f, Fats = 0.3f, Carbohydrates = 23f, Fiber = 2.6f },
        new IngredientDbModel { Name = "Lemon", Quantity = 0, UnitId = g, Calories = 29, Proteins = 1.1f, Fats = 0.3f, Carbohydrates = 9f, Fiber = 2.8f },

        // Meats & Fish
        new IngredientDbModel { Name = "Chicken Breast", Quantity = 0, UnitId = g, Calories = 165, Proteins = 31f, Fats = 3.6f, Carbohydrates = 0, Fiber = 0 },
        new IngredientDbModel { Name = "Beef Steak", Quantity = 0, UnitId = g, Calories = 271, Proteins = 26f, Fats = 17f, Carbohydrates = 0, Fiber = 0 },
        new IngredientDbModel { Name = "Salmon", Quantity = 0, UnitId = g, Calories = 208, Proteins = 20f, Fats = 13f, Carbohydrates = 0, Fiber = 0 },

        // Dairy & Eggs
        new IngredientDbModel { Name = "Egg", Quantity = 1, UnitId = pcs, Calories = 78, Proteins = 6f, Fats = 5f, Carbohydrates = 0.6f, Fiber = 0 },
        new IngredientDbModel { Name = "Milk", Quantity = 0, UnitId = ml, Calories = 42, Proteins = 3.4f, Fats = 1f, Carbohydrates = 5f, Fiber = 0 },
        new IngredientDbModel { Name = "Butter", Quantity = 0, UnitId = g, Calories = 717, Proteins = 0.9f, Fats = 81f, Carbohydrates = 0.1f, Fiber = 0 },
        new IngredientDbModel { Name = "Cheddar Cheese", Quantity = 0, UnitId = g, Calories = 404, Proteins = 25f, Fats = 33f, Carbohydrates = 1.3f, Fiber = 0 },

        // Grains & Pasta
        new IngredientDbModel { Name = "All-Purpose Flour", Quantity = 0, UnitId = g, Calories = 364, Proteins = 10f, Fats = 1f, Carbohydrates = 76f, Fiber = 2.7f },
        new IngredientDbModel { Name = "White Rice", Quantity = 0, UnitId = g, Calories = 130, Proteins = 2.7f, Fats = 0.3f, Carbohydrates = 28f, Fiber = 0.4f },
        new IngredientDbModel { Name = "Spaghetti", Quantity = 0, UnitId = g, Calories = 158, Proteins = 5.8f, Fats = 0.9f, Carbohydrates = 31f, Fiber = 1.8f },
        new IngredientDbModel { Name = "Bread", Quantity = 0, UnitId = g, Calories = 265, Proteins = 9f, Fats = 3.2f, Carbohydrates = 49f, Fiber = 2.7f },

        // Liquids & Oils
        new IngredientDbModel { Name = "Water", Quantity = 0, UnitId = ml, Calories = 0, Proteins = 0, Fats = 0, Carbohydrates = 0, Fiber = 0 },
        new IngredientDbModel { Name = "Olive Oil", Quantity = 0, UnitId = ml, Calories = 884, Proteins = 0, Fats = 0f, Carbohydrates = 0, Fiber = 0 },
        new IngredientDbModel { Name = "Vegetable Broth", Quantity = 0, UnitId = ml, Calories = 5, Proteins = 0.2f, Fats = 0.1f, Carbohydrates = 1f, Fiber = 0.3f },
        
        // Spices & Sugars
        new IngredientDbModel { Name = "Salt", Quantity = 0, UnitId = g, Calories = 0, Proteins = 0, Fats = 0, Carbohydrates = 0, Fiber = 0 },
        new IngredientDbModel { Name = "Black Pepper", Quantity = 0, UnitId = g, Calories = 251, Proteins = 10f, Fats = 3.3f, Carbohydrates = 64f, Fiber = 25f },
        new IngredientDbModel { Name = "Sugar", Quantity = 0, UnitId = g, Calories = 387, Proteins = 0, Fats = 0, Carbohydrates = 0f, Fiber = 0 },
        
        // Nuts & Seeds
        new IngredientDbModel { Name = "Almonds", Quantity = 0, UnitId = g, Calories = 579, Proteins = 21f, Fats = 49f, Carbohydrates = 22f, Fiber = 12.5f },
        new IngredientDbModel { Name = "Walnuts", Quantity = 0, UnitId = g, Calories = 654, Proteins = 15f, Fats = 65f, Carbohydrates = 14f, Fiber = 7f },
        
        // Sweets
        new IngredientDbModel { Name = "Dark Chocolate", Quantity = 0, UnitId = g, Calories = 546, Proteins = 4.9f, Fats = 31f, Carbohydrates = 61f, Fiber = 7f },
        new IngredientDbModel { Name = "Honey", Quantity = 0, UnitId = g, Calories = 304, Proteins = 0.3f, Fats = 0, Carbohydrates = 82f, Fiber = 0.2f }
    };

            await _database.InsertAllAsync(ingredients);
            System.Diagnostics.Debug.WriteLine($"[Seeder] Inserted {ingredients.Count} ingredients.");
        }



    }
}

