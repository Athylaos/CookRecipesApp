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
            UnitDbModel unitDbModel = await _database.Table<UnitDbModel>().FirstOrDefaultAsync(x => x.Id == ingredientDbModel.UnitId);
            return new Ingredient(
                ingredientDbModel.Id,
                ingredientDbModel.Name,
                ingredientDbModel.Quantity,
                unitDbModel,
                ingredientDbModel.Calories,
                ingredientDbModel.Proteins,
                ingredientDbModel.Fats,
                ingredientDbModel.Carbohydrates,
                ingredientDbModel.Fiber);
        }

        private async Task<IngredientDbModel> IngredientToIngredientDbModelAsync(Ingredient ingredient)
        {
            return new IngredientDbModel()
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
                Quantity = ingredient.Quantity,
                UnitId = ingredient.Unit.Id,
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
            var ingredientDbModel = await IngredientToIngredientDbModelAsync(ingredient);
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

            var ingredients = ingredientsDbModels.Select(ingredientDbModel =>
                new Ingredient(
                    ingredientDbModel.Id,
                    ingredientDbModel.Name,
                    ingredientDbModel.Quantity,
                    unitsDbModels.Find(u => u.Id == ingredientDbModel.UnitId) ?? new UnitDbModel { Id = 0, Name = "g"},
                    ingredientDbModel.Calories,
                    ingredientDbModel.Proteins,
                    ingredientDbModel.Fats,
                    ingredientDbModel.Carbohydrates,
                    ingredientDbModel.Fiber
                )
            ).ToList();

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
            var ingredientDbModel = await IngredientToIngredientDbModelAsync(ingredient);

            await _database.UpdateAsync(ingredientDbModel);

            return;
        }
    }
}
