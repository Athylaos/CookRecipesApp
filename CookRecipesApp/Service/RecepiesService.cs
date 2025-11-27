using System;
using System.Collections.Generic;
using System.Text;
using CookRecipesApp.Model.Recepie;
using CookRecipesApp.Model.Ingredient;
using SQLite;
using CookRecipesApp.Model;
using System.Threading.Tasks;

namespace CookRecipesApp.Service
{
    public interface IRecepiesService
    {
        public Task<List<Recepie>> GetAllRecepiesAsync();
        public Task<Recepie> GetRecepieAsync(int id);
        public Task SaveRecepieAsync(Recepie recepie);
        public Task DeleteRecepieAsync(int id);
        public Task UpdateRecepieAsync(Recepie recepie);


    }

    public class RecepiesService : IRecepiesService
    {
        private readonly ISQLiteAsyncConnection _database;
        private readonly IngredientsService _ingredientService;

        public RecepiesService(SQLiteConnectionFactory factory, IngredientsService ingredientsService)
        {
            _database = factory.CreateConnection();
            _ingredientService = ingredientsService;
        }

        public async Task<List<Ingredient>> GetAllIngredientsForRecepieAsync(int recepieId)
        {
            var recepieIngredients = await _database
                .Table<RecepieIngredientDbModel>()
                .Where(ri => ri.RecepieId == recepieId)
                .ToListAsync();

            var ingredients = new List<Ingredient>();

            foreach (var ri in recepieIngredients)
            {
                var ing = await _ingredientService.GetIngredientAsync(ri.IngredientId);
                if (ing != null)
                {
                    ingredients.Add(ing);
                }
            }

            return ingredients;
        }



        public async Task<Recepie> RecepieDbModelToRecepie(RecepieDbModel recepieDbModel)
        {
            Recepie recepie = new Recepie()
            {
                Id = recepieDbModel.Id,
                UserId = recepieDbModel.UserId,
                Title = recepieDbModel.Title,
                CoockingProcess = recepieDbModel.CoockingProcess,
                CoockingTime = recepieDbModel.CoockingTime,
                Servings = recepieDbModel.Servings,
                Ingredients = await GetAllIngredientsForRecepieAsync(recepieDbModel.Id),
                Calories = recepieDbModel.Calories,
                Proteins = recepieDbModel.Proteins,
                Fats = recepieDbModel.Fats,
                Carbohydrates = recepieDbModel.Carbohydrates,
                Fiber = recepieDbModel.Fiber,
            };
            return recepie;
        }

        public RecepieDbModel RecepieToRecepieDbModel(Recepie recepie)
        {
            RecepieDbModel recepieDbModel = new()
            {
                Id = recepie.Id,
                UserId = recepie.UserId,
                Title = recepie.Title,
                CoockingProcess = recepie.CoockingProcess,
                CoockingTime = recepie.CoockingTime,
                Servings = recepie.Servings,
                Calories = recepie.Calories,
                Proteins = recepie.Proteins,
                Fats = recepie.Fats,
                Carbohydrates = recepie.Carbohydrates,
                Fiber = recepie.Fiber
            };            

            return recepieDbModel;
        }



        public Task DeleteRecepieAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Recepie>> GetAllRecepiesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Recepie> GetRecepieAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task SaveRecepieAsync(Recepie recepie)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRecepieAsync(Recepie recepie)
        {
            throw new NotImplementedException();
        }
    }
}
