using CookRecipesApp.Model.Category;
using CookRecipesApp.Model.Ingredient;
using CookRecipesApp.Model.Recepie;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CookRecipesApp.Service
{
    public class DatabaseSeederService
        {
            private readonly ISQLiteAsyncConnection _database;

            public DatabaseSeederService(ISQLiteAsyncConnection database)
            {
                _database = database;
            }

            public async Task SeedCompleteRecipesAsync()
            {
                // Kontrola jestli už nejsou data
                if (await _database.Table<RecepieDbModel>().CountAsync() > 0) return;

                Debug.WriteLine("🔥 Starting complete recipe seeding...");

                // POŘADÍ SEEDOVÁNÍ
                await SeedUnitsAsync();
                await SeedIngredientsAsync();
                await SeedIngredientUnitsAsync();
                await SeedRecipesAsync();
                await SeedRecipeIngredientsAsync();
                await SeedRecipeStepsAsync();
                await SeedCommentsAsync();

                Debug.WriteLine("✅ Complete seeding done! 5 recipes with ingredients, steps, comments ready.");
            }

            #region 1. JEDNOTKY (11 jednotek)
            private async Task SeedUnitsAsync()
            {
                var units = new[]
                {
                new UnitDbModel { Name = "g", IsServingUnit = false },       // ID 1
                new UnitDbModel { Name = "kg", IsServingUnit = false },      // ID 2
                new UnitDbModel { Name = "ml", IsServingUnit = false },      // ID 3
                new UnitDbModel { Name = "l", IsServingUnit = false },       // ID 4
                new UnitDbModel { Name = "pcs", IsServingUnit = true },      // ID 5
                new UnitDbModel { Name = "portion", IsServingUnit = true },  // ID 6
                new UnitDbModel { Name = "tsp", IsServingUnit = false },     // ID 7
                new UnitDbModel { Name = "tbsp", IsServingUnit = false },    // ID 8
                new UnitDbModel { Name = "cup", IsServingUnit = false },     // ID 9
                new UnitDbModel { Name = "clove", IsServingUnit = false },   // ID 10
                new UnitDbModel { Name = "slice", IsServingUnit = true },    // ID 11
            };
                await _database.InsertAllAsync(units);
                Debug.WriteLine($"✅ {units.Length} units");
            }
            #endregion

            #region 2. SUROVINY (25 surovin pro 5 receptů)
            private async Task SeedIngredientsAsync()
            {
                var ingredients = new[]
                {
                // ID 1-5: Základní zelenina
                new IngredientDbModel { Name = "Onion", DefaultUnitId = 1, Calories = 40, Proteins = 1.1f, Fats = 0.1f, Carbohydrates = 9.3f, Fiber = 1.7f },
                new IngredientDbModel { Name = "Garlic", DefaultUnitId = 10, Calories = 149, Proteins = 6.4f, Fats = 0.5f, Carbohydrates = 33f, Fiber = 2.1f },
                new IngredientDbModel { Name = "Tomato", DefaultUnitId = 1, Calories = 18, Proteins = 0.9f, Fats = 0.2f, Carbohydrates = 3.9f, Fiber = 1.2f },
                new IngredientDbModel { Name = "Bell Pepper", DefaultUnitId = 1, Calories = 31, Proteins = 1f, Fats = 0.3f, Carbohydrates = 6f, Fiber = 2.1f },
                new IngredientDbModel { Name = "Carrot", DefaultUnitId = 1, Calories = 41, Proteins = 0.9f, Fats = 0.2f, Carbohydrates = 10f, Fiber = 2.8f },

                // ID 6-10: Maso a bílkoviny
                new IngredientDbModel { Name = "Chicken Breast", DefaultUnitId = 1, Calories = 165, Proteins = 31f, Fats = 3.6f, Carbohydrates = 0f, Fiber = 0f },
                new IngredientDbModel { Name = "Ground Beef", DefaultUnitId = 1, Calories = 250, Proteins = 26f, Fats = 15f, Carbohydrates = 0f, Fiber = 0f },
                new IngredientDbModel { Name = "Salmon Fillet", DefaultUnitId = 1, Calories = 208, Proteins = 22f, Fats = 13f, Carbohydrates = 0f, Fiber = 0f },
                new IngredientDbModel { Name = "Egg", DefaultUnitId = 5, Calories = 155, Proteins = 13f, Fats = 11f, Carbohydrates = 1.1f, Fiber = 0f },
                new IngredientDbModel { Name = "Tofu", DefaultUnitId = 1, Calories = 76, Proteins = 8f, Fats = 4.8f, Carbohydrates = 1.9f, Fiber = 0.3f },

                // ID 11-15: Obiloviny a těstoviny
                new IngredientDbModel { Name = "Rice", DefaultUnitId = 1, Calories = 130, Proteins = 2.7f, Fats = 0.3f, Carbohydrates = 28f, Fiber = 0.4f },
                new IngredientDbModel { Name = "Pasta", DefaultUnitId = 1, Calories = 371, Proteins = 13f, Fats = 1.5f, Carbohydrates = 75f, Fiber = 3f },
                new IngredientDbModel { Name = "Bread", DefaultUnitId = 11, Calories = 265, Proteins = 9f, Fats = 3.2f, Carbohydrates = 49f, Fiber = 2.7f },
                new IngredientDbModel { Name = "Flour", DefaultUnitId = 1, Calories = 364, Proteins = 10f, Fats = 1f, Carbohydrates = 76f, Fiber = 2.7f },
                new IngredientDbModel { Name = "Tortilla", DefaultUnitId = 5, Calories = 218, Proteins = 6f, Fats = 5f, Carbohydrates = 37f, Fiber = 2.4f },

                // ID 16-20: Mléčné výrobky
                new IngredientDbModel { Name = "Milk", DefaultUnitId = 3, Calories = 42, Proteins = 3.4f, Fats = 1f, Carbohydrates = 5f, Fiber = 0f },
                new IngredientDbModel { Name = "Cheese Cheddar", DefaultUnitId = 1, Calories = 402, Proteins = 25f, Fats = 33f, Carbohydrates = 1.3f, Fiber = 0f },
                new IngredientDbModel { Name = "Butter", DefaultUnitId = 1, Calories = 717, Proteins = 0.9f, Fats = 81f, Carbohydrates = 0.1f, Fiber = 0f },
                new IngredientDbModel { Name = "Sour Cream", DefaultUnitId = 3, Calories = 193, Proteins = 2.4f, Fats = 19f, Carbohydrates = 4.6f, Fiber = 0f },
                new IngredientDbModel { Name = "Yogurt", DefaultUnitId = 3, Calories = 59, Proteins = 3.5f, Fats = 0.4f, Carbohydrates = 10f, Fiber = 0f },

                // ID 21-25: Koření a ostatní
                new IngredientDbModel { Name = "Olive Oil", DefaultUnitId = 3, Calories = 884, Proteins = 0f, Fats = 100f, Carbohydrates = 0f, Fiber = 0f },
                new IngredientDbModel { Name = "Salt", DefaultUnitId = 1, Calories = 0, Proteins = 0f, Fats = 0f, Carbohydrates = 0f, Fiber = 0f },
                new IngredientDbModel { Name = "Black Pepper", DefaultUnitId = 1, Calories = 251, Proteins = 10.4f, Fats = 3.3f, Carbohydrates = 64f, Fiber = 25f },
                new IngredientDbModel { Name = "Paprika", DefaultUnitId = 7, Calories = 282, Proteins = 14f, Fats = 13f, Carbohydrates = 54f, Fiber = 34f },
                new IngredientDbModel { Name = "Soy Sauce", DefaultUnitId = 3, Calories = 53, Proteins = 5.6f, Fats = 0.1f, Carbohydrates = 4.9f, Fiber = 0.8f },
            };

                await _database.InsertAllAsync(ingredients);
                Debug.WriteLine($"✅ {ingredients.Length} ingredients");
            }
            #endregion

            #region 3. VAZBY INGREDIENT-UNIT (VŠECHNY suroviny mají všechny možné jednotky)
            private async Task SeedIngredientUnitsAsync()
            {
                var links = new List<IngredientUnitDbModel>();

                // Helper pro přidání vazeb
                void AddLink(int ingId, int unitId, float factor) =>
                    links.Add(new IngredientUnitDbModel { IngredientId = ingId, UnitId = unitId, ToDefaultUnit = factor });

                // ONION (ID 1) - DefaultUnit: g
                AddLink(1, 1, 1f);      // g -> g
                AddLink(1, 2, 1000f);   // kg -> 1000g
                AddLink(1, 5, 150f);    // pcs (1 cibule) -> 150g

                // GARLIC (ID 2) - DefaultUnit: clove
                AddLink(2, 10, 1f);     // clove -> 1 clove
                AddLink(2, 1, 5f);      // g -> 5g (1 stroužek = 5g)
                AddLink(2, 5, 30f);     // pcs (celá hlava) -> 30g

                // TOMATO (ID 3)
                AddLink(3, 1, 1f);
                AddLink(3, 2, 1000f);
                AddLink(3, 5, 120f);    // 1 rajče = 120g
                AddLink(3, 11, 20f);    // 1 plát = 20g

                // BELL PEPPER (ID 4)
                AddLink(4, 1, 1f);
                AddLink(4, 5, 180f);    // 1 paprika = 180g

                // CARROT (ID 5)
                AddLink(5, 1, 1f);
                AddLink(5, 5, 60f);     // 1 mrkev = 60g

                // CHICKEN BREAST (ID 6)
                AddLink(6, 1, 1f);
                AddLink(6, 2, 1000f);
                AddLink(6, 5, 200f);    // 1 ks prso = 200g

                // GROUND BEEF (ID 7)
                AddLink(7, 1, 1f);
                AddLink(7, 2, 1000f);

                // SALMON (ID 8)
                AddLink(8, 1, 1f);
                AddLink(8, 5, 150f);    // 1 filet = 150g

                // EGG (ID 9)
                AddLink(9, 5, 1f);      // pcs -> 1 vejce
                AddLink(9, 1, 50f);     // g -> 50g (1 vejce)

                // TOFU (ID 10)
                AddLink(10, 1, 1f);
                AddLink(10, 9, 250f);   // cup -> 250g

                // RICE (ID 11)
                AddLink(11, 1, 1f);
                AddLink(11, 9, 180f);   // cup (syrové) -> 180g
                AddLink(11, 8, 12f);    // tbsp -> 12g

                // PASTA (ID 12)
                AddLink(12, 1, 1f);
                AddLink(12, 2, 1000f);

                // BREAD (ID 13)
                AddLink(13, 11, 1f);    // slice -> 1 plát
                AddLink(13, 1, 30f);    // g -> 30g (1 plát)

                // FLOUR (ID 14)
                AddLink(14, 1, 1f);
                AddLink(14, 9, 120f);   // cup -> 120g
                AddLink(14, 8, 8f);     // tbsp -> 8g
                AddLink(14, 7, 3f);     // tsp -> 3g

                // TORTILLA (ID 15)
                AddLink(15, 5, 1f);     // pcs
                AddLink(15, 1, 50f);    // 1 tortilla = 50g

                // MILK (ID 16)
                AddLink(16, 3, 1f);     // ml
                AddLink(16, 4, 1000f);  // l -> 1000ml
                AddLink(16, 9, 240f);   // cup -> 240ml
                AddLink(16, 8, 15f);    // tbsp -> 15ml

                // CHEESE (ID 17)
                AddLink(17, 1, 1f);
                AddLink(17, 9, 113f);   // cup (strouh.) -> 113g

                // BUTTER (ID 18)
                AddLink(18, 1, 1f);
                AddLink(18, 8, 14f);    // tbsp -> 14g
                AddLink(18, 7, 5f);     // tsp -> 5g

                // SOUR CREAM (ID 19)
                AddLink(19, 3, 1f);
                AddLink(19, 8, 15f);
                AddLink(19, 9, 240f);

                // YOGURT (ID 20)
                AddLink(20, 3, 1f);
                AddLink(20, 9, 245f);

                // OLIVE OIL (ID 21)
                AddLink(21, 3, 1f);
                AddLink(21, 8, 14f);
                AddLink(21, 7, 5f);

                // SALT (ID 22)
                AddLink(22, 1, 1f);
                AddLink(22, 7, 6f);     // tsp -> 6g
                AddLink(22, 8, 18f);    // tbsp -> 18g

                // PEPPER (ID 23)
                AddLink(23, 1, 1f);
                AddLink(23, 7, 2f);

                // PAPRIKA (ID 24)
                AddLink(24, 7, 2f);
                AddLink(24, 8, 6f);
                AddLink(24, 1, 1f);

                // SOY SAUCE (ID 25)
                AddLink(25, 3, 1f);
                AddLink(25, 8, 15f);
                AddLink(25, 7, 5f);

                await _database.InsertAllAsync(links);
                Debug.WriteLine($"✅ {links.Count} ingredient-unit links");
            }
            #endregion

            #region 4. RECEPTY (5 receptů s plnými daty)
            private async Task SeedRecipesAsync()
            {
                var recipes = new[]
                {
                // RECEPT 1: Spaghetti Bolognese
                new RecepieDbModel
                {
                    UserId = 1,
                    Title = "Spaghetti Bolognese",
                    CookingTime = 45,
                    Servings = 4,
                    ServingUnitId = 6, // portion
                    Difficulty = 2,
                    Calories = 520,
                    Proteins = 28f,
                    Fats = 18f,
                    Carbohydrates = 65f,
                    Fiber = 4.5f,
                    RecepieCreated = DateTime.Now.AddDays(-15).ToString("o"),
                    Rating = 4.7f,
                    UsersRated = 34
                },

                // RECEPT 2: Grilled Chicken Salad
                new RecepieDbModel
                {
                    UserId = 1,
                    Title = "Grilled Chicken Salad",
                    CookingTime = 20,
                    Servings = 2,
                    ServingUnitId = 6,
                    Difficulty = 1,
                    Calories = 350,
                    Proteins = 38f,
                    Fats = 12f,
                    Carbohydrates = 22f,
                    Fiber = 6f,
                    RecepieCreated = DateTime.Now.AddDays(-8).ToString("o"),
                    Rating = 4.5f,
                    UsersRated = 28
                },

                // RECEPT 3: Veggie Stir Fry
                new RecepieDbModel
                {
                    UserId = 1,
                    Title = "Vegetable Stir Fry",
                    CookingTime = 15,
                    Servings = 3,
                    ServingUnitId = 6,
                    Difficulty = 1,
                    Calories = 280,
                    Proteins = 10f,
                    Fats = 14f,
                    Carbohydrates = 32f,
                    Fiber = 8f,
                    RecepieCreated = DateTime.Now.AddDays(-3).ToString("o"),
                    Rating = 4.3f,
                    UsersRated = 19
                },

                // RECEPT 4: Salmon with Rice
                new RecepieDbModel
                {
                    UserId = 1,
                    Title = "Baked Salmon with Rice",
                    CookingTime = 35,
                    Servings = 2,
                    ServingUnitId = 6,
                    Difficulty = 2,
                    Calories = 480,
                    Proteins = 32f,
                    Fats = 18f,
                    Carbohydrates = 45f,
                    Fiber = 2f,
                    RecepieCreated = DateTime.Now.AddDays(-20).ToString("o"),
                    Rating = 4.8f,
                    UsersRated = 45
                },

                // RECEPT 5: Breakfast Omelette
                new RecepieDbModel
                {
                    UserId = 1,
                    Title = "Cheese & Veggie Omelette",
                    CookingTime = 10,
                    Servings = 1,
                    ServingUnitId = 6,
                    Difficulty = 1,
                    Calories = 320,
                    Proteins = 22f,
                    Fats = 24f,
                    Carbohydrates = 8f,
                    Fiber = 2f,
                    RecepieCreated = DateTime.Now.AddDays(-5).ToString("o"),
                    Rating = 4.6f,
                    UsersRated = 22
                }
            };

                await _database.InsertAllAsync(recipes);
                Debug.WriteLine($"✅ {recipes.Length} recipes");
            }
            #endregion

            #region 5. INGREDIENCE RECEPTŮ
            private async Task SeedRecipeIngredientsAsync()
            {
                var links = new[]
                {
                // RECEPT 1: Spaghetti Bolognese
                new RecepieIngredientDbModel { RecepieId = 1, IngredientId = 12, Quantity = 400, UnitId = 1 },  // Pasta 400g
                new RecepieIngredientDbModel { RecepieId = 1, IngredientId = 7, Quantity = 500, UnitId = 1 },   // Ground beef 500g
                new RecepieIngredientDbModel { RecepieId = 1, IngredientId = 3, Quantity = 3, UnitId = 5 },     // 3 rajčata
                new RecepieIngredientDbModel { RecepieId = 1, IngredientId = 1, Quantity = 1, UnitId = 5 },     // 1 cibule
                new RecepieIngredientDbModel { RecepieId = 1, IngredientId = 2, Quantity = 3, UnitId = 10 },    // 3 stroužky česneku
                new RecepieIngredientDbModel { RecepieId = 1, IngredientId = 21, Quantity = 30, UnitId = 3 },   // 30ml olivový olej
                new RecepieIngredientDbModel { RecepieId = 1, IngredientId = 22, Quantity = 1, UnitId = 7 },    // Sůl (lžička)
                new RecepieIngredientDbModel { RecepieId = 1, IngredientId = 23, Quantity = 1, UnitId = 7 },    // Pepř

                // RECEPT 2: Grilled Chicken Salad
                new RecepieIngredientDbModel { RecepieId = 2, IngredientId = 6, Quantity = 300, UnitId = 1 },   // Chicken 300g
                new RecepieIngredientDbModel { RecepieId = 2, IngredientId = 3, Quantity = 2, UnitId = 5 },     // 2 rajčata
                new RecepieIngredientDbModel { RecepieId = 2, IngredientId = 4, Quantity = 1, UnitId = 5 },     // 1 paprika
                new RecepieIngredientDbModel { RecepieId = 2, IngredientId = 21, Quantity = 20, UnitId = 3 },   // Olej
                new RecepieIngredientDbModel { RecepieId = 2, IngredientId = 22, Quantity = 1, UnitId = 7 },
                new RecepieIngredientDbModel { RecepieId = 2, IngredientId = 2, Quantity = 1, UnitId = 10 },    // Česnek

                // RECEPT 3: Veggie Stir Fry
                new RecepieIngredientDbModel { RecepieId = 3, IngredientId = 4, Quantity = 2, UnitId = 5 },     // 2 papriky
                new RecepieIngredientDbModel { RecepieId = 3, IngredientId = 5, Quantity = 2, UnitId = 5 },     // 2 mrkve
                new RecepieIngredientDbModel { RecepieId = 3, IngredientId = 10, Quantity = 200, UnitId = 1 },  // Tofu 200g
                new RecepieIngredientDbModel { RecepieId = 3, IngredientId = 25, Quantity = 30, UnitId = 3 },   // Sojovka 30ml
                new RecepieIngredientDbModel { RecepieId = 3, IngredientId = 2, Quantity = 2, UnitId = 10 },
                new RecepieIngredientDbModel { RecepieId = 3, IngredientId = 21, Quantity = 20, UnitId = 3 },

                // RECEPT 4: Salmon with Rice
                new RecepieIngredientDbModel { RecepieId = 4, IngredientId = 8, Quantity = 2, UnitId = 5 },     // 2 filety
                new RecepieIngredientDbModel { RecepieId = 4, IngredientId = 11, Quantity = 1, UnitId = 9 },    // 1 cup rýže
                new RecepieIngredientDbModel { RecepieId = 4, IngredientId = 18, Quantity = 2, UnitId = 8 },    // Máslo 2 tbsp
                new RecepieIngredientDbModel { RecepieId = 4, IngredientId = 2, Quantity = 2, UnitId = 10 },
                new RecepieIngredientDbModel { RecepieId = 4, IngredientId = 22, Quantity = 1, UnitId = 7 },

                // RECEPT 5: Omelette
                new RecepieIngredientDbModel { RecepieId = 5, IngredientId = 9, Quantity = 3, UnitId = 5 },     // 3 vejce
                new RecepieIngredientDbModel { RecepieId = 5, IngredientId = 17, Quantity = 50, UnitId = 1 },   // Sýr 50g
                new RecepieIngredientDbModel { RecepieId = 5, IngredientId = 4, Quantity = 80, UnitId = 1 },    // Paprika 80g
                new RecepieIngredientDbModel { RecepieId = 5, IngredientId = 18, Quantity = 1, UnitId = 8 },    // Máslo
                new RecepieIngredientDbModel { RecepieId = 5, IngredientId = 22, Quantity = 1, UnitId = 7 },
            };

                await _database.InsertAllAsync(links);
                Debug.WriteLine($"✅ {links.Length} recipe ingredients");
            }
            #endregion

            #region 6. KROKY RECEPTŮ
            private async Task SeedRecipeStepsAsync()
            {
                var steps = new[]
                {
                // RECEPT 1 kroky
                new RecepieStepDbModel { RecepieId = 1, StepNumber = 1, Description = "Boil water for pasta and cook according to package instructions" },
                new RecepieStepDbModel { RecepieId = 1, StepNumber = 2, Description = "Dice onion and mince garlic. Heat olive oil in a large pan" },
                new RecepieStepDbModel { RecepieId = 1, StepNumber = 3, Description = "Brown ground beef with onions and garlic for 7-8 minutes" },
                new RecepieStepDbModel { RecepieId = 1, StepNumber = 4, Description = "Add chopped tomatoes, season with salt and pepper. Simmer for 20 min" },
                new RecepieStepDbModel { RecepieId = 1, StepNumber = 5, Description = "Drain pasta, mix with sauce and serve hot" },

                // RECEPT 2 kroky
                new RecepieStepDbModel { RecepieId = 2, StepNumber = 1, Description = "Season chicken breast with salt, pepper and minced garlic" },
                new RecepieStepDbModel { RecepieId = 2, StepNumber = 2, Description = "Grill chicken for 6-7 minutes each side until fully cooked" },
                new RecepieStepDbModel { RecepieId = 2, StepNumber = 3, Description = "Chop tomatoes and bell peppers into bite-sized pieces" },
                new RecepieStepDbModel { RecepieId = 2, StepNumber = 4, Description = "Slice grilled chicken and toss with vegetables and olive oil" },

                // RECEPT 3 kroky
                new RecepieStepDbModel { RecepieId = 3, StepNumber = 1, Description = "Cut all vegetables and tofu into thin strips" },
                new RecepieStepDbModel { RecepieId = 3, StepNumber = 2, Description = "Heat oil in wok on high heat. Add garlic for 30 seconds" },
                new RecepieStepDbModel { RecepieId = 3, StepNumber = 3, Description = "Stir-fry vegetables and tofu for 5-6 minutes" },
                new RecepieStepDbModel { RecepieId = 3, StepNumber = 4, Description = "Add soy sauce, toss and serve immediately" },

                // RECEPT 4 kroky
                new RecepieStepDbModel { RecepieId = 4, StepNumber = 1, Description = "Preheat oven to 200°C (400°F)" },
                new RecepieStepDbModel { RecepieId = 4, StepNumber = 2, Description = "Cook rice according to package instructions" },
                new RecepieStepDbModel { RecepieId = 4, StepNumber = 3, Description = "Season salmon with garlic, salt and brush with melted butter" },
                new RecepieStepDbModel { RecepieId = 4, StepNumber = 4, Description = "Bake salmon for 15-18 minutes until flaky" },
                new RecepieStepDbModel { RecepieId = 4, StepNumber = 5, Description = "Serve salmon over rice" },

                // RECEPT 5 kroky
                new RecepieStepDbModel { RecepieId = 5, StepNumber = 1, Description = "Beat eggs with salt in a bowl" },
                new RecepieStepDbModel { RecepieId = 5, StepNumber = 2, Description = "Dice bell pepper into small pieces" },
                new RecepieStepDbModel { RecepieId = 5, StepNumber = 3, Description = "Melt butter in pan, add peppers and sauté for 2 minutes" },
                new RecepieStepDbModel { RecepieId = 5, StepNumber = 4, Description = "Pour eggs, sprinkle cheese on top and fold when set" },
            };

                await _database.InsertAllAsync(steps);
                Debug.WriteLine($"✅ {steps.Length} recipe steps");
            }
            #endregion

            #region 7. KOMENTÁŘE (2-3 na recept)
            private async Task SeedCommentsAsync()
            {
                var comments = new[]
                {
                // RECEPT 1 komentáře
                new CommentDbModel { RecepieId = 1, UserId = 2, Text = "Best bolognese I've ever made! Family loved it.", CreatedAt = DateTime.Now.AddDays(-14).ToString("o") },
                new CommentDbModel { RecepieId = 1, UserId = 3, Text = "Great recipe but I added more garlic. Still perfect!", CreatedAt = DateTime.Now.AddDays(-12).ToString("o") },
                new CommentDbModel { RecepieId = 1, UserId = 4, Text = "Good but needed more cooking time for the sauce", CreatedAt = DateTime.Now.AddDays(-7).ToString("o") },

                // RECEPT 2 komentáře
                new CommentDbModel { RecepieId = 2, UserId = 2, Text = "Healthy and delicious! Perfect for lunch prep", CreatedAt = DateTime.Now.AddDays(-7).ToString("o") },
                new CommentDbModel { RecepieId = 2, UserId = 5, Text = "Very fresh, chicken was tender and juicy", CreatedAt = DateTime.Now.AddDays(-5).ToString("o") },

                // RECEPT 3 komentáře
                new CommentDbModel { RecepieId = 3, UserId = 3, Text = "Quick weeknight dinner winner!", CreatedAt = DateTime.Now.AddDays(-2).ToString("o") },
                new CommentDbModel { RecepieId = 3, UserId = 6, Text = "Added mushrooms too, was amazing", CreatedAt = DateTime.Now.AddDays(-1).ToString("o") },

                // RECEPT 4 komentáře
                new CommentDbModel { RecepieId = 4, UserId = 2, Text = "Salmon cooked perfectly, very moist!", CreatedAt = DateTime.Now.AddDays(-19).ToString("o") },
                new CommentDbModel { RecepieId = 4, UserId = 4, Text = "Great omega-3 meal, will make again", CreatedAt = DateTime.Now.AddDays(-16).ToString("o") },
                new CommentDbModel { RecepieId = 4, UserId = 7, Text = "Rice was fluffy, salmon tender. 10/10", CreatedAt = DateTime.Now.AddDays(-10).ToString("o") },

                // RECEPT 5 komentáře
                new CommentDbModel { RecepieId = 5, UserId = 5, Text = "Perfect breakfast! Made it 3 times this week", CreatedAt = DateTime.Now.AddDays(-4).ToString("o") },
                new CommentDbModel { RecepieId = 5, UserId = 3, Text = "Simple and tasty, added spinach too", CreatedAt = DateTime.Now.AddDays(-3).ToString("o") },
            };

                await _database.InsertAllAsync(comments);
                Debug.WriteLine($"✅ {comments.Length} comments");
            }
            #endregion
        }
    }
