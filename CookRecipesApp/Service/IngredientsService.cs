using CookRecipesApp.Model.Ingredient;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.Service
{
    public interface IIngredientsService
    {
        public Task AddIngredientAsync(Ingredient ingredient);
        public Task RemoveIngredientAsync(int id);
        public Task UpdateIngredientAsync(Ingredient ingredient);
        public Task<List<Ingredient>> GetAllIngredientsAsync();
        public Task<Ingredient?> GetIngredientAsync(int id);

    }
    public class IngredientsService : IIngredientsService
    {

        private readonly ISQLiteAsyncConnection _database;

        public IngredientsService(SQLiteConnectionFactory factory)
        {
            _database = factory.CreateConnection();
        }

        private async Task<Ingredient> IngredientDbModelToIngredientAsync(IngredientDbModel ingredientDbModel)
        {
            var ingredientUnits = await _database.Table<IngredientUnitDbModel>().Where(x => x.IngredientId == ingredientDbModel.Id).ToListAsync();

            var possibleUnitIds = ingredientUnits.Select(pu => pu.UnitId).ToList();
            var allUnits = await _database.Table<UnitDbModel>().ToListAsync();

            var possibleUnits = allUnits.Where(u => possibleUnitIds.Contains(u.Id)).ToList();
            var dfUnit = allUnits.FirstOrDefault(u => u.Id == ingredientDbModel.DefaultUnitId);

            return new Ingredient
            {
                Id = ingredientDbModel.Id,
                Name = ingredientDbModel.Name,
                PossibleUnits = possibleUnits,
                DefaultUnit = dfUnit,

                Calories = ingredientDbModel.Calories,
                Proteins = ingredientDbModel.Proteins,
                Fats = ingredientDbModel.Fats,
                Carbohydrates = ingredientDbModel.Carbohydrates,
                Fiber = ingredientDbModel.Fiber,
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

                Calories = ingredient.Calories,
                Proteins = ingredient.Proteins,
                Fats = ingredient.Fats,
                Carbohydrates = ingredient.Carbohydrates,
                Fiber = ingredient.Fiber
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
            var unitsDict = unitsDbModels.ToDictionary(u => u.Id);

            var allIngredientUnits = await _database.Table<IngredientUnitDbModel>().ToListAsync();

            var ingredientUnitsLookup = allIngredientUnits.ToLookup(iu => iu.IngredientId);

            var ingredients = ingredientsDbModels.Select(dbModel =>
            {
                unitsDict.TryGetValue(dbModel.DefaultUnitId, out var defaultUnit);
                if (defaultUnit == null) defaultUnit = new UnitDbModel { Id = 0, Name = "g" };

                List<UnitDbModel> possibleUnits = new List<UnitDbModel>();
                if (ingredientUnitsLookup.Contains(dbModel.Id))
                {
                    var unitIds = ingredientUnitsLookup[dbModel.Id].Select(iu => iu.UnitId);
                    foreach (var unitId in unitIds)
                    {
                        if (unitsDict.TryGetValue(unitId, out var unit))
                        {
                            possibleUnits.Add(unit);
                        }
                    }
                }

                return new Ingredient
                {
                    Id = dbModel.Id,
                    Name = dbModel.Name,
                    DefaultUnit = defaultUnit,
                    PossibleUnits = possibleUnits,
                    Calories = dbModel.Calories,
                    Proteins = dbModel.Proteins,
                    Fats = dbModel.Fats,
                    Carbohydrates = dbModel.Carbohydrates,
                    Fiber = dbModel.Fiber
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
