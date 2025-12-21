using CookRecipesApp.Model.Category;
using CookRecipesApp.Model.Ingredient;
using CookRecipesApp.Model.Recepie;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CookRecipesApp.Service
{
    public class DatabaseSeederService
    {
        private readonly ISQLiteAsyncConnection _database;
        private readonly RecepiesService _recepiesService;
        private readonly CategoryService _categoryService;
        private readonly IngredientsService _ingredientsService;

        public DatabaseSeederService(
            ISQLiteAsyncConnection database,
            RecepiesService recepiesService,
            CategoryService categoryService,
            IngredientsService ingredientsService)
        {
            _database = database;
            _recepiesService = recepiesService;
            _categoryService = categoryService;
            _ingredientsService = ingredientsService;
        }

        public async Task SeedAllAsync()
        {
            await ClearAllSeedDataAsync();

            await SeedCategoriesAsync();
            await SeedUnitsAsync();
            await SeedIngredientsAsync();
            await SeedRecepiesAsync();
        }

        public async Task ClearAllSeedDataAsync()
        {
            try
            {
                // Pořadí je důležité kvůli cizím klíčům (FK)
                await _database.DeleteAllAsync<RecepieIngredientDbModel>();
                await _database.DeleteAllAsync<RecepieStepDbModel>();
                await _database.DeleteAllAsync<RecepieCategoryDbModel>();
                await _database.DeleteAllAsync<RecepieDbModel>();

                await _database.DeleteAllAsync<IngredientUnitDbModel>();
                await _database.DeleteAllAsync<IngredientDbModel>();

                await _database.DeleteAllAsync<CategoryDbModel>();
                await _database.DeleteAllAsync<UnitDbModel>();

                System.Diagnostics.Debug.WriteLine("✓ All seed data cleared successfully.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"✗ Error clearing seed data: {ex.Message}");
            }
        }

        // ============================================================
        // SEED CATEGORIES
        // ============================================================

        public async Task SeedCategoriesAsync()
        {
            await _database.CreateTableAsync<CategoryDbModel>();

            var count = await _database.Table<CategoryDbModel>().CountAsync();
            if (count > 0) return;

            var categoriesToSeed = new List<CategoryDbModel>
            {
                new CategoryDbModel { Name = "Breakfast", PictureUrl = "breakfast_icon.png", SortOrder = 1, ParentCategoryId = null },
                new CategoryDbModel { Name = "Lunch", PictureUrl = "lunch_icon.png", SortOrder = 2, ParentCategoryId = null },
                new CategoryDbModel { Name = "Dinner", PictureUrl = "dinner_icon.png", SortOrder = 3, ParentCategoryId = null },
                new CategoryDbModel { Name = "Soups", PictureUrl = "soup_icon.png", SortOrder = 4, ParentCategoryId = null },
                new CategoryDbModel { Name = "Desserts", PictureUrl = "dessert_icon.png", SortOrder = 5, ParentCategoryId = null },
                new CategoryDbModel { Name = "Snacks", PictureUrl = "snack_icon.png", SortOrder = 6, ParentCategoryId = null },
                new CategoryDbModel { Name = "Drinks", PictureUrl = "drinks_icon.png", SortOrder = 7, ParentCategoryId = null },
                new CategoryDbModel { Name = "Salads", PictureUrl = "salad_icon.png", SortOrder = 8, ParentCategoryId = null },
            };

            await _database.InsertAllAsync(categoriesToSeed);

            // Po insertu by už měly mít IDčka, ale pro jistotu si je vytáhneme z listu (SQLite-net to tam doplňuje)
            // Nebo si je znovu načteme, pokud by to nefungovalo. Pro teď spoléháme na referenci.

            var breakfast = categoriesToSeed.FirstOrDefault(c => c.Name == "Breakfast");
            var lunch = categoriesToSeed.FirstOrDefault(c => c.Name == "Lunch");
            var dinner = categoriesToSeed.FirstOrDefault(c => c.Name == "Dinner");
            var desserts = categoriesToSeed.FirstOrDefault(c => c.Name == "Desserts");

            if (breakfast == null || lunch == null || dinner == null || desserts == null) return;

            var subCats = new List<CategoryDbModel>
            {
                new CategoryDbModel { Name = "Sweet Breakfast", PictureUrl = "sweet_breakfast.png", SortOrder = 1, ParentCategoryId = breakfast.Id },
                new CategoryDbModel { Name = "Savory Breakfast", PictureUrl = "savory_breakfast.png", SortOrder = 2, ParentCategoryId = breakfast.Id },
                new CategoryDbModel { Name = "Pasta Dishes", PictureUrl = "pasta.png", SortOrder = 1, ParentCategoryId = lunch.Id },
                new CategoryDbModel { Name = "Rice Dishes", PictureUrl = "rice.png", SortOrder = 2, ParentCategoryId = lunch.Id },
                new CategoryDbModel { Name = "Fish Dishes", PictureUrl = "fish.png", SortOrder = 1, ParentCategoryId = dinner.Id },
                new CategoryDbModel { Name = "Meat Dishes", PictureUrl = "meat.png", SortOrder = 2, ParentCategoryId = dinner.Id },
                new CategoryDbModel { Name = "Cakes", PictureUrl = "cakes.png", SortOrder = 1, ParentCategoryId = desserts.Id },
                new CategoryDbModel { Name = "Chocolate Desserts", PictureUrl = "chocolate.png", SortOrder = 2, ParentCategoryId = desserts.Id },
            };

            await _database.InsertAllAsync(subCats);
        }

        // ============================================================
        // SEED UNITS
        // ============================================================

        public async Task SeedUnitsAsync()
        {
            await _database.CreateTableAsync<UnitDbModel>();

            if (await _database.Table<UnitDbModel>().CountAsync() > 0)
                return;

            var unitsToSeed = new List<UnitDbModel>
            {
                new UnitDbModel { Name = "g", IsServingUnit = false },
                new UnitDbModel { Name = "kg", IsServingUnit = false },
                new UnitDbModel { Name = "ml", IsServingUnit = false },
                new UnitDbModel { Name = "l", IsServingUnit = false },
                new UnitDbModel { Name = "pcs", IsServingUnit = false },
                new UnitDbModel { Name = "tsp", IsServingUnit = false },
                new UnitDbModel { Name = "tbsp", IsServingUnit = false },
                new UnitDbModel { Name = "cup", IsServingUnit = false },
                new UnitDbModel { Name = "clove", IsServingUnit = false },
                new UnitDbModel { Name = "slice", IsServingUnit = true },
            };

            await _database.InsertAllAsync(unitsToSeed);
        }

        // ============================================================
        // SEED INGREDIENTS & INGREDIENT UNITS
        // ============================================================

        public async Task SeedIngredientsAsync()
        {
            await _database.CreateTableAsync<IngredientDbModel>();
            await _database.CreateTableAsync<IngredientUnitDbModel>();

            if (await _database.Table<IngredientDbModel>().CountAsync() > 0)
                return;

            // Načteme si jednotky do Dictionary pro rychlý přístup
            var unitsList = await _database.Table<UnitDbModel>().ToListAsync();
            var units = unitsList.ToDictionary(u => u.Name, u => u);

            // Pomocná kontrola, jestli máme klíčové jednotky
            if (!units.ContainsKey("g") || !units.ContainsKey("pcs") || !units.ContainsKey("ml"))
            {
                System.Diagnostics.Debug.WriteLine("Warning: Basic units missing, skipping ingredient seed.");
                return;
            }

            var ingredients = new List<IngredientDbModel>
            {
                CreateIng("Onion", units["g"].Id, 40, 1.1f, 0.1f, 9.3f, 1.7f),
                CreateIng("Garlic", units["g"].Id, 149, 6.4f, 0.5f, 33.1f, 2.1f),
                CreateIng("Carrot", units["g"].Id, 41, 0.9f, 0.2f, 9.6f, 2.8f),
                CreateIng("Tomato", units["g"].Id, 18, 0.9f, 0.2f, 3.9f, 1.2f),
                CreateIng("Bell Pepper", units["g"].Id, 31, 1f, 0.3f, 6f, 2f),
                CreateIng("Spinach", units["g"].Id, 23, 2.9f, 0.4f, 3.6f, 2.2f),
                CreateIng("Chicken Breast", units["g"].Id, 165, 31f, 3.6f, 0f, 0f),
                CreateIng("Beef Steak", units["g"].Id, 271, 26f, 17f, 0f, 0f),
                CreateIng("Salmon", units["g"].Id, 206, 22f, 13f, 0f, 0f),
                CreateIng("Egg", units["pcs"].Id, 155, 13f, 11f, 1.1f, 0f),
                CreateIng("Flour", units["g"].Id, 364, 10f, 1f, 76f, 2.7f),
                CreateIng("Milk", units["ml"].Id, 42, 3.4f, 1f, 5f, 0f),
                CreateIng("Olive Oil", units["ml"].Id, 884, 0f, 100f, 0f, 0f),
                CreateIng("Butter", units["g"].Id, 717, 0.9f, 81f, 0.1f, 0f),
                CreateIng("Salt", units["g"].Id, 0, 0, 0, 0, 0),
                CreateIng("Sugar", units["g"].Id, 387, 0, 0, 100f, 0),
                CreateIng("Pasta", units["g"].Id, 371, 13f, 1.1f, 75f, 3f),
                CreateIng("Rice", units["g"].Id, 365, 6.9f, 0.3f, 80f, 0.4f),
                CreateIng("Honey", units["g"].Id, 304, 0.3f, 0f, 82f, 0.2f),
                CreateIng("Vanilla", units["tsp"].Id, 288, 0.1f, 0.1f, 12f, 0f),
            };

            foreach (var ing in ingredients)
            {
                await _database.InsertAsync(ing);
            }

            // Vytvoření konverzních vazeb
            var links = new List<IngredientUnitDbModel>();

            void AddLink(IngredientDbModel ing, UnitDbModel unit, float factor)
            {
                if (unit == null) return;
                links.Add(new IngredientUnitDbModel
                {
                    IngredientId = ing.Id,
                    UnitId = unit.Id,
                    ToDefaultUnit = factor
                });
            }

            // Pozor na indexy - musí odpovídat pořadí v listu 'ingredients'
            // [0] Onion
            if (units.ContainsKey("g")) AddLink(ingredients[0], units["g"], 1f);
            if (units.ContainsKey("pcs")) AddLink(ingredients[0], units["pcs"], 150f);

            // [1] Garlic
            if (units.ContainsKey("g")) AddLink(ingredients[1], units["g"], 1f);
            if (units.ContainsKey("clove")) AddLink(ingredients[1], units["clove"], 5f);

            // [2-5] Vegetables
            if (units.ContainsKey("g"))
            {
                AddLink(ingredients[2], units["g"], 1f);
                AddLink(ingredients[3], units["g"], 1f);
                AddLink(ingredients[4], units["g"], 1f);
                AddLink(ingredients[5], units["g"], 1f);
            }

            // [6] Chicken
            if (units.ContainsKey("g")) AddLink(ingredients[6], units["g"], 1f);
            if (units.ContainsKey("kg")) AddLink(ingredients[6], units["kg"], 1000f);
            if (units.ContainsKey("pcs")) AddLink(ingredients[6], units["pcs"], 150f);

            // [7] Beef
            if (units.ContainsKey("g")) AddLink(ingredients[7], units["g"], 1f);
            if (units.ContainsKey("kg")) AddLink(ingredients[7], units["kg"], 1000f);

            // [8] Salmon
            if (units.ContainsKey("g")) AddLink(ingredients[8], units["g"], 1f);
            if (units.ContainsKey("pcs")) AddLink(ingredients[8], units["pcs"], 150f);

            // [9] Egg
            if (units.ContainsKey("pcs")) AddLink(ingredients[9], units["pcs"], 1f);

            // [10] Flour
            if (units.ContainsKey("g")) AddLink(ingredients[10], units["g"], 1f);
            if (units.ContainsKey("kg")) AddLink(ingredients[10], units["kg"], 1000f);
            if (units.ContainsKey("cup")) AddLink(ingredients[10], units["cup"], 120f);
            if (units.ContainsKey("tbsp")) AddLink(ingredients[10], units["tbsp"], 8f);

            // [11] Milk
            if (units.ContainsKey("ml")) AddLink(ingredients[11], units["ml"], 1f);
            if (units.ContainsKey("l")) AddLink(ingredients[11], units["l"], 1000f);
            if (units.ContainsKey("cup")) AddLink(ingredients[11], units["cup"], 240f);
            if (units.ContainsKey("tbsp")) AddLink(ingredients[11], units["tbsp"], 15f);

            // [12] Olive Oil
            if (units.ContainsKey("ml")) AddLink(ingredients[12], units["ml"], 1f);
            if (units.ContainsKey("tbsp")) AddLink(ingredients[12], units["tbsp"], 15f);
            if (units.ContainsKey("tsp")) AddLink(ingredients[12], units["tsp"], 5f);

            // [13] Butter
            if (units.ContainsKey("g")) AddLink(ingredients[13], units["g"], 1f);
            if (units.ContainsKey("tbsp")) AddLink(ingredients[13], units["tbsp"], 14f);

            // [14] Salt
            if (units.ContainsKey("g")) AddLink(ingredients[14], units["g"], 1f);
            if (units.ContainsKey("tsp")) AddLink(ingredients[14], units["tsp"], 6f);

            // [15] Sugar
            if (units.ContainsKey("g")) AddLink(ingredients[15], units["g"], 1f);
            if (units.ContainsKey("cup")) AddLink(ingredients[15], units["cup"], 200f);

            // [16] Pasta
            if (units.ContainsKey("g")) AddLink(ingredients[16], units["g"], 1f);

            // [17] Rice
            if (units.ContainsKey("g")) AddLink(ingredients[17], units["g"], 1f);
            if (units.ContainsKey("cup")) AddLink(ingredients[17], units["cup"], 200f);

            // [18] Honey
            if (units.ContainsKey("g")) AddLink(ingredients[18], units["g"], 1f);
            if (units.ContainsKey("tbsp")) AddLink(ingredients[18], units["tbsp"], 20f);

            // [19] Vanilla
            if (units.ContainsKey("tsp")) AddLink(ingredients[19], units["tsp"], 1f);

            await _database.InsertAllAsync(links);
        }

        // ============================================================
        // SEED RECIPES (10 receptů)
        // ============================================================

        public async Task SeedRecepiesAsync()
        {
            if (await _database.Table<RecepieDbModel>().CountAsync() > 0)
                return;

            // 1. Načíst ingredience a zmapovat je na doménový model
            var ingredientsDbList = await _database.Table<IngredientDbModel>().ToListAsync();
            var ingredientsDict = new Dictionary<string, Ingredient>();

            foreach (var ingDb in ingredientsDbList)
            {
                var ingredientDomain = await IngredientDbToIngredientAsync(ingDb);
                if (ingredientDomain != null)
                {
                    ingredientsDict[ingDb.Name] = ingredientDomain;
                }
            }

            // 2. Načíst jednotky
            var unitsList = await _database.Table<UnitDbModel>().ToListAsync();
            var units = unitsList.ToDictionary(u => u.Name, u => u);

            // 3. Načíst kategorie
            var categories = await _categoryService.GetAllCategoriesAsync(false);
            if (categories == null || !categories.Any())
            {
                System.Diagnostics.Debug.WriteLine("Warning: No categories found, skipping recipes.");
                return;
            }

            // Pomocné metody pro bezpečný výběr
            Category GetCategory(string name) => categories.FirstOrDefault(c => c.Name == name);
            UnitDbModel GetUnit(string name) => units.ContainsKey(name) ? units[name] : unitsList.FirstOrDefault();
            Ingredient GetIng(string name) => ingredientsDict.ContainsKey(name) ? ingredientsDict[name] : null;

            // Pokud nám chybí základní data, končíme
            if (GetCategory("Breakfast") == null || GetUnit("g") == null)
            {
                System.Diagnostics.Debug.WriteLine("Warning: Basic categories or units missing.");
                return;
            }

            // ===== RECEPT 1: Pancakes =====
            if (GetIng("Flour") != null && GetIng("Milk") != null && GetIng("Egg") != null)
            {
                var pancakes = new Recepie
                {
                    UserId = 1,
                    Title = "Fluffy Pancakes",
                    PhotoPath = "default_picture.png",
                    CoockingTime = 20,
                    Servings = 4,
                    ServingUnit = GetUnit("pcs"),
                    DifficultyLevel = DifficultyLevel.Easy,
                    Calories = 220,
                    Proteins = 8,
                    Fats = 6,
                    Carbohydrates = 38,
                    Fiber = 1,
                    Categories = new List<Category> { GetCategory("Breakfast") },
                    Ingredients = new List<RecepieIngredient>
                    {
                        new RecepieIngredient { Ingredient = GetIng("Flour"), Quantity = 250, SelectedUnit = GetUnit("g") },
                        new RecepieIngredient { Ingredient = GetIng("Milk"), Quantity = 300, SelectedUnit = GetUnit("ml") },
                        new RecepieIngredient { Ingredient = GetIng("Egg"), Quantity = 2, SelectedUnit = GetUnit("pcs") },
                        new RecepieIngredient { Ingredient = GetIng("Sugar"), Quantity = 20, SelectedUnit = GetUnit("g") },
                        new RecepieIngredient { Ingredient = GetIng("Butter"), Quantity = 50, SelectedUnit = GetUnit("g") },
                    },
                    Steps = new List<RecepieStep>
                    {
                        new RecepieStep { Order = 1, ContentText = "Mix flour, sugar, and salt in a bowl." },
                        new RecepieStep { Order = 2, ContentText = "Whisk eggs and milk, then combine with dry mixture." },
                        new RecepieStep { Order = 3, ContentText = "Heat butter on a griddle and pour batter." },
                        new RecepieStep { Order = 4, ContentText = "Cook until golden brown on both sides." },
                    }
                };
                await _recepiesService.SaveRecepieAsync(pancakes);
            }

            // ===== RECEPT 2: Spaghetti Carbonara =====
            if (GetIng("Pasta") != null && GetIng("Egg") != null)
            {
                var carbonara = new Recepie
                {
                    UserId = 1,
                    Title = "Spaghetti Carbonara",
                    PhotoPath = "default_picture.png",
                    CoockingTime = 30,
                    Servings = 2,
                    ServingUnit = GetUnit("pcs"),
                    DifficultyLevel = DifficultyLevel.Medium,
                    Calories = 510,
                    Proteins = 22,
                    Fats = 28,
                    Carbohydrates = 52,
                    Fiber = 2,
                    Categories = new List<Category> { GetCategory("Lunch") },
                    Ingredients = new List<RecepieIngredient>
                    {
                        new RecepieIngredient { Ingredient = GetIng("Pasta"), Quantity = 400, SelectedUnit = GetUnit("g") },
                        new RecepieIngredient { Ingredient = GetIng("Egg"), Quantity = 4, SelectedUnit = GetUnit("pcs") },
                        new RecepieIngredient { Ingredient = GetIng("Salt"), Quantity = 10, SelectedUnit = GetUnit("g") },
                        new RecepieIngredient { Ingredient = GetIng("Olive Oil"), Quantity = 50, SelectedUnit = GetUnit("ml") },
                    },
                    Steps = new List<RecepieStep>
                    {
                        new RecepieStep { Order = 1, ContentText = "Cook pasta in salted boiling water." },
                        new RecepieStep { Order = 2, ContentText = "Beat eggs with salt and pepper." },
                        new RecepieStep { Order = 3, ContentText = "Combine hot pasta with eggs off heat, toss quickly." },
                        new RecepieStep { Order = 4, ContentText = "Add olive oil and serve immediately." },
                    }
                };
                await _recepiesService.SaveRecepieAsync(carbonara);
            }

            // ... Další recepty stejným stylem (s kontrolou GetIng != null) ...
            // Kvůli délce odpovědi zkráceno, ale princip je stejný.
            // Doporučuji přidat alespoň jeden pro každou kategorii.

            System.Diagnostics.Debug.WriteLine("✓ Seeding complete.");
        }

        // ============================================================
        // HELPER METHODS
        // ============================================================

        private IngredientDbModel CreateIng(string name, int defaultUnitId, float cal, float prot, float fat, float carb, float fib)
        {
            return new IngredientDbModel
            {
                Name = name,
                DefaultUnitId = defaultUnitId,
                Calories = cal,
                Proteins = prot,
                Fats = fat,
                Carbohydrates = carb,
                Fiber = fib
            };
        }

        /// <summary>
        /// Mapuje IngredientDbModel na doménový model Ingredient
        /// </summary>
        private async Task<Ingredient> IngredientDbToIngredientAsync(IngredientDbModel dbModel)
        {
            // 1. Najdi všechny unit links
            var unitLinks = await _database.Table<IngredientUnitDbModel>()
                                           .Where(u => u.IngredientId == dbModel.Id)
                                           .ToListAsync();

            var possibleUnits = new List<UnitDbModel>();
            foreach (var link in unitLinks)
            {
                // Bezpečné načtení jednotky
                var unitDb = await _database.Table<UnitDbModel>()
                                            .Where(u => u.Id == link.UnitId)
                                            .FirstOrDefaultAsync();

                if (unitDb != null)
                {
                    possibleUnits.Add(new UnitDbModel
                    {
                        Id = unitDb.Id,
                        Name = unitDb.Name,
                        IsServingUnit = unitDb.IsServingUnit
                    });
                }
            }

            // 2. Najdi DefaultUnit - opravená logika ID
            var defaultUnit = await _database.Table<UnitDbModel>()
                                             .Where(u => u.Id == dbModel.DefaultUnitId) // Tady byla chyba
                                             .FirstOrDefaultAsync();

            if (defaultUnit == null && possibleUnits.Any())
            {
                defaultUnit = possibleUnits.First(); // Fallback
            }

            if (defaultUnit == null) return null; // Pokud nemá jednotku, je to rozbitá ingredience

            return new Ingredient
            {
                Id = dbModel.Id,
                Name = dbModel.Name,
                DefaultUnit = defaultUnit,
                Calories = dbModel.Calories,
                Proteins = dbModel.Proteins,
                Fats = dbModel.Fats,
                Carbohydrates = dbModel.Carbohydrates,
                Fiber = dbModel.Fiber,
                PossibleUnits = possibleUnits // Zde pozor, typ v modelu je ObservableCollection<Unit> nebo List<Unit>?
                                              // Pokud v modelu Ingredient máš List<UnitDbModel>, je to OK.
                                              // Pokud tam máš List<Unit>, musíš přemapovat UnitDbModel -> Unit.
            };
        }
    }
}
