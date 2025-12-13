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

        public async Task SeedDataAsync()
        {
            // 1. Kontrola, jestli už data neexistují (pokud ano, konec)
            if (await _database.Table<UnitDbModel>().CountAsync() > 0) return;

            // --- A. SEED UNITS (JEDNOTKY) ---
            var unitG = new UnitDbModel { Name = "g", IsServingUnit = false };
            var unitKg = new UnitDbModel { Name = "kg", IsServingUnit = false };
            var unitMl = new UnitDbModel { Name = "ml", IsServingUnit = false };
            var unitL = new UnitDbModel { Name = "l", IsServingUnit = false };
            var unitPcs = new UnitDbModel { Name = "pcs", IsServingUnit = false };
            var unitTsp = new UnitDbModel { Name = "tsp", IsServingUnit = false }; // Lžička
            var unitTbsp = new UnitDbModel { Name = "tbsp", IsServingUnit = false }; // Lžíce
            var unitCup = new UnitDbModel { Name = "cup", IsServingUnit = false }; // Hrnek
            var unitSlice = new UnitDbModel { Name = "slice", IsServingUnit = true };
            var unitClove = new UnitDbModel { Name = "clove", IsServingUnit = false }; // Stroužek

            // Vložení a získání IDček (SQLite IDčka generuje až po insertu)
            await _database.InsertAsync(unitG);
            await _database.InsertAsync(unitKg);
            await _database.InsertAsync(unitMl);
            await _database.InsertAsync(unitL);
            await _database.InsertAsync(unitPcs);
            await _database.InsertAsync(unitTsp);
            await _database.InsertAsync(unitTbsp);
            await _database.InsertAsync(unitCup);
            await _database.InsertAsync(unitSlice);
            await _database.InsertAsync(unitClove);

            // --- B. SEED INGREDIENTS (SUROVINY) ---
            var ingredientsToInsert = new List<IngredientDbModel>();
            var ingredientUnitsToInsert = new List<IngredientUnitDbModel>();

            // Pomocná lokální funkce pro přidání suroviny a jejích vazeb
            void AddIng(string name, int defaultUnitId, float cal, float prot, float fat, float carb, float fib, params int[] otherUnitIds)
            {
                var ing = new IngredientDbModel
                {
                    Name = name,
                    DefaultUnitId = defaultUnitId,
                    Calories = cal,
                    Proteins = prot,
                    Fats = fat,
                    Carbohydrates = carb,
                    Fiber = fib
                };
                ingredientsToInsert.Add(ing);

                // Vazby přidáme až po vložení suroviny (kvůli získání ID), takže si je zatím uložíme bokem
                // Ale pozor: Tady ještě nemáme ID ingredience. Musíme to udělat ve dvou krocích.
                // Proto si uložíme "akci", která se provede až budeme mít ID.
            }

            // Definice surovin (Nutriční hodnoty jsou orientační na 100g/ml nebo 1ks)

            // 1. Zelenina
            var onion = CreateIng("Onion", unitG.Id, 40, 1.1f, 0.1f, 9.3f, 1.7f);
            var garlic = CreateIng("Garlic", unitClove.Id, 149, 6.4f, 0.5f, 33.1f, 2.1f); // Kalorie na 100g, ale default je stroužek
            var carrot = CreateIng("Carrot", unitG.Id, 41, 0.9f, 0.2f, 9.6f, 2.8f);
            var tomato = CreateIng("Tomato", unitG.Id, 18, 0.9f, 0.2f, 3.9f, 1.2f);

            // 2. Maso
            var chicken = CreateIng("Chicken Breast", unitG.Id, 165, 31f, 3.6f, 0f, 0f);
            var beef = CreateIng("Beef Steak", unitG.Id, 271, 26f, 17f, 0f, 0f);

            // 3. Ostatní
            var egg = CreateIng("Egg", unitPcs.Id, 155, 13f, 11f, 1.1f, 0f); // Hodnoty na 100g (cca 2 vejce)
            var flour = CreateIng("Flour", unitG.Id, 364, 10f, 1f, 76f, 2.7f);
            var milk = CreateIng("Milk", unitMl.Id, 42, 3.4f, 1f, 5f, 0f);
            var oil = CreateIng("Olive Oil", unitMl.Id, 884, 0f, 100f, 0f, 0f);
            var salt = CreateIng("Salt", unitG.Id, 0, 0, 0, 0, 0);

            // Vložení surovin do DB (aby dostaly ID)
            var allIngredients = new List<IngredientDbModel> { onion, garlic, carrot, tomato, chicken, beef, egg, flour, milk, oil, salt };

            foreach (var ing in allIngredients)
            {
                await _database.InsertAsync(ing); // Teď má ing.Id hodnotu
            }

            // --- C. SEED INGREDIENT UNITS (VAZBY - MOŽNÉ JEDNOTKY) ---
            var links = new List<IngredientUnitDbModel>();

            // Pomocná metoda pro přidání vazby s konverzí
            // factor = kolikrát se vejde DefaultUnit do této jednotky
            // Příklad: Default je gram (g). 
            // Jednotka je kg. Factor = 1000 (1 kg = 1000 g).
            // Jednotka je lžíce (tbsp). Factor = 15 (1 lžíce vody = 15 g).
            void AddLink(int ingId, int unitId, float factor)
            {
                links.Add(new IngredientUnitDbModel
                {
                    IngredientId = ingId,
                    UnitId = unitId,
                    ToDefaultUnit = factor
                });
            }

            // --- CIBULE (Default: g) ---
            AddLink(onion.Id, unitG.Id, 1f);       // 1 g = 1 g
            AddLink(onion.Id, unitPcs.Id, 150f);   // 1 ks cibule = cca 150 g

            // --- ČESNEK (Default: stroužek/clove) ---
            // Pozor: Default je clove. Takže konverze je vztažená k 1 stroužku.
            // To je trochu matoucí, lepší by bylo mít default g.
            // Pokud změníme default česneku na g:
            // garlic.DefaultUnitId = unitG.Id; (pokud jsi to změnil nahoře)
            // Pak:
            AddLink(garlic.Id, unitG.Id, 1f);
            AddLink(garlic.Id, unitClove.Id, 5f);  // 1 stroužek = 5 g

            // --- KUŘE (Default: g) ---
            AddLink(chicken.Id, unitG.Id, 1f);
            AddLink(chicken.Id, unitKg.Id, 1000f); // 1 kg = 1000 g
            AddLink(chicken.Id, unitPcs.Id, 150f); // 1 ks prsa = cca 150 g

            // --- VEJCE (Default: pcs) ---
            AddLink(egg.Id, unitPcs.Id, 1f);
            // Pokud bychom chtěli váhu: 1 vejce = 50 g (ale default je pcs, takže by to bylo obráceně)
            // Nechme jen pcs.

            // --- MOUKA (Default: g) ---
            AddLink(flour.Id, unitG.Id, 1f);
            AddLink(flour.Id, unitKg.Id, 1000f);
            AddLink(flour.Id, unitCup.Id, 120f);   // 1 hrnek mouky = 120 g (lehčí než voda)
            AddLink(flour.Id, unitTbsp.Id, 8f);    // 1 lžíce mouky = cca 8 g

            // --- MLÉKO (Default: ml) ---
            AddLink(milk.Id, unitMl.Id, 1f);
            AddLink(milk.Id, unitL.Id, 1000f);     // 1 l = 1000 ml
            AddLink(milk.Id, unitCup.Id, 240f);    // 1 hrnek = 240 ml
            AddLink(milk.Id, unitTbsp.Id, 15f);    // 1 lžíce = 15 ml

            // --- OLEJ (Default: ml) ---
            AddLink(oil.Id, unitMl.Id, 1f);
            AddLink(oil.Id, unitTbsp.Id, 15f);
            AddLink(oil.Id, unitTsp.Id, 5f);       // 1 lžička = 5 ml

            // --- SŮL (Default: g) ---
            AddLink(salt.Id, unitG.Id, 1f);
            AddLink(salt.Id, unitTsp.Id, 6f);      // 1 lžička soli = cca 6 g (těžší než voda)


            // Vložení všech vazeb najednou
            await _database.InsertAllAsync(links);

            System.Diagnostics.Debug.WriteLine("Seeding complete: Units, Ingredients and Links created.");
        }

        // Pomocná metoda pro vytvoření objektu (jen pro přehlednost kódu výše)
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



    }
}

