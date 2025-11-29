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

        public async Task<List<Ingredient>> GetAllIngredientsForRecepieAsync(int recepieId) // would be better to load all recepie-ingredients and match them in memory, but fine for now
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



        public async Task<Recepie> RecepieDbModelToRecepieAsync(RecepieDbModel recepieDbModel)
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



        public async Task DeleteRecepieAsync(int id)
        {
            await _database.DeleteAsync<RecepieDbModel>(id);
        }

        public async Task<List<Recepie>> GetAllRecepiesAsync()
        {
            var recepieDbList = await _database.Table<RecepieDbModel>().ToListAsync();
            List<Recepie> recepieList = new();

            foreach (var recepie in recepieDbList)
            {
                recepieList.Add(await RecepieDbModelToRecepieAsync(recepie));
            }
            return recepieList;
            
        }

        public async Task<Recepie> GetRecepieAsync(int id)
        {
            var recepieDbModel = await _database.Table<RecepieDbModel>()
                                                .FirstOrDefaultAsync(r => r.Id == id);
            if (recepieDbModel == null) throw new ArgumentNullException("Object not found in database");
            return await RecepieDbModelToRecepieAsync(recepieDbModel);
        }

        public async Task SaveRecepieAsync(Recepie recepie)
        {
            if (recepie == null) throw new ArgumentNullException("Cant save null object to database");

            var recepieDbModel = RecepieToRecepieDbModel(recepie);

            await _database.InsertAsync(recepieDbModel);

            return;
        }

        public async Task UpdateRecepieAsync(Recepie recepie)
        {
            var recepieDbModel = RecepieToRecepieDbModel(recepie);

            await _database.UpdateAsync(recepieDbModel);

            return;
        }
    }
}
