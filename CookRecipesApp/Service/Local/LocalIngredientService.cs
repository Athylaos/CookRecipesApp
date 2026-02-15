
using CookRecipesApp.Model.Ingredient;
using CookRecipesApp.Service.Interface;
using CookRecipesApp.Model.Recepie;
using SQLite;

namespace CookRecipesApp.Service
{
    public class LocalIngredientService : IIngredientService
    {

        private readonly ISQLiteAsyncConnection _database;

        public LocalIngredientService(SQLiteConnectionFactory factory)
        {
            _database = factory.CreateConnection();
        }

        private async Task<Ingredient> IngredientDbModelToIngredientAsync(IngredientDbModel ingredientDbModel)
        {
            var ingredientUnitsLinks = await _database.Table<IngredientUnitDbModel>().Where(x => x.IngredientId == ingredientDbModel.Id).ToListAsync();

            var unitIds = ingredientUnitsLinks.Select(u => u.UnitId).ToList();
            if (!unitIds.Contains(ingredientDbModel.DefaultUnitId))
            {
                unitIds.Add(ingredientDbModel.DefaultUnitId);
            }

            var allUnits = await _database.Table<UnitDbModel>().ToListAsync();

            var possibleIngredientUnits = ingredientUnitsLinks.Select(link => new Ingredient.IngredientUnitInfo
            {
                Unit = allUnits.FirstOrDefault(u => u.Id == link.UnitId),
                ConversionFactor = link.ToDefaultUnit
            }).ToList();

            return new Ingredient
            {
                Id = ingredientDbModel.Id,
                Name = ingredientDbModel.Name,
                PossibleUnits = possibleIngredientUnits,
                DefaultUnit = allUnits.FirstOrDefault(u => u.Id == ingredientDbModel.DefaultUnitId),
                
                Nutritions = new Nutritions()
                {
                    Calories = ingredientDbModel.Calories,
                    Proteins = ingredientDbModel.Proteins,
                    Fats = ingredientDbModel.Fats,
                    Carbohydrates = ingredientDbModel.Carbohydrates,
                    Fiber = ingredientDbModel.Fiber,
                }
            };
        }



        private IngredientDbModel IngredientToIngredientDbModel(Ingredient ingredient)
        {
            int defaultUnitId = ingredient.DefaultUnit?.Id ?? 0;

            return new IngredientDbModel
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
                DefaultUnitId = defaultUnitId,

                Calories = ingredient.Nutritions.Calories,
                Proteins = ingredient.Nutritions.Proteins,
                Fats = ingredient.Nutritions.Fats,
                Carbohydrates = ingredient.Nutritions.Carbohydrates,
                Fiber = ingredient.Nutritions.Fiber
            };
        }


        public async Task AddIngredientAsync(Ingredient ingredient)
        {
            if(ingredient == null)
            {
                return; 
            }
            var ingredientDbModel = IngredientToIngredientDbModel(ingredient);

            await _database.InsertAsync(ingredientDbModel);

            if (ingredient.PossibleUnits != null && ingredient.PossibleUnits.Any())
            {
                var ingredientUnitLinks = ingredient.PossibleUnits.Select(unit =>
                    new IngredientUnitDbModel
                    {
                        IngredientId = ingredientDbModel.Id,
                        UnitId = unit.Unit.Id,
                        ToDefaultUnit = unit.ConversionFactor
                    }).ToList();

                await _database.InsertAllAsync(ingredientUnitLinks);
            }

            return;
        }

        public async Task<List<Ingredient>> GetAllIngredientsAsync()
        {
            var ingredientsDbModels = await _database.Table<IngredientDbModel>().ToListAsync();
            if (ingredientsDbModels.Count == 0)
            {
                return new List<Ingredient>();
            }

            var unitsDbModels = await _database.Table<UnitDbModel>().ToListAsync();
            var allIngredientUnits = await _database.Table<IngredientUnitDbModel>().ToListAsync();

            var unitsDict = unitsDbModels.ToDictionary(u => u.Id);
            var ingredientUnitsLookup = allIngredientUnits.ToLookup(iu => iu.IngredientId);

            var ingredients = ingredientsDbModels.Select(dbModel =>
            {
                unitsDict.TryGetValue(dbModel.DefaultUnitId, out var defaultUnit);
                if (defaultUnit == null) defaultUnit = new UnitDbModel { Id = 0, Name = "unknown" };

                var possibleUnitsInfo = new List<Ingredient.IngredientUnitInfo>();

                if (ingredientUnitsLookup.Contains(dbModel.Id))
                {
                    foreach (var link in ingredientUnitsLookup[dbModel.Id])
                    {
                        if (unitsDict.TryGetValue(link.UnitId, out var unit))
                        {
                            possibleUnitsInfo.Add(new Ingredient.IngredientUnitInfo
                            {
                                Unit = unit,
                                ConversionFactor = link.ToDefaultUnit
                            });
                        }
                    }
                }

                return new Ingredient
                {
                    Id = dbModel.Id,
                    Name = dbModel.Name,
                    DefaultUnit = defaultUnit,
                    PossibleUnits = possibleUnitsInfo,

                    Nutritions = new Nutritions()
                    {
                        Calories = dbModel.Calories,
                        Proteins = dbModel.Proteins,
                        Fats = dbModel.Fats,
                        Carbohydrates = dbModel.Carbohydrates,
                        Fiber = dbModel.Fiber,
                    }
                };
            }).ToList();

            return ingredients;
        }



        public async Task<Ingredient?> GetIngredientAsync(int id)
        {
            var ingredientDbModel = await _database.Table<IngredientDbModel>().FirstOrDefaultAsync(i => i.Id == id);

            if (ingredientDbModel == null)
            {
                return null;
            }

            var ingredient = await IngredientDbModelToIngredientAsync(ingredientDbModel);

            return ingredient;
        }

        public async Task RemoveIngredientAsync(int id)
        {               
            var ingredientDbModel = await _database.Table<IngredientDbModel>().FirstOrDefaultAsync(i => i.Id == id);

            if (ingredientDbModel != null)
            {
                await _database.DeleteAsync(ingredientDbModel);
            }
            return;
        }

        public async Task UpdateIngredientAsync(Ingredient ingredient)
        {
            var ingredientDbModel = IngredientToIngredientDbModel(ingredient);

            await _database.UpdateAsync(ingredientDbModel);

            return;
        }

        public async Task<List<UnitDbModel>> GetAllServingUnitsAsync()
        {
            var units = await _database.Table<UnitDbModel>().Where(u => u.IsServingUnit).ToListAsync();
            return units;
        }
    }
}
