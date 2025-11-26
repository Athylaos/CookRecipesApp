using System;
using System.Collections.Generic;
using System.Text;
using CookRecipesApp.Model.Recepie;
using CookRecipesApp.Model.Ingredient;
using SQLite;

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

        public RecepiesService(ISQLiteAsyncConnection database)
        {
            _database = database;
        }


        public Task<List<Recepie>> GetAllRecepiesAsync()
        {        
            var recepiesDbModels = _database.Table<RecepieDbModel>().ToListAsync();
            List<Recepie> recepies = new List<Recepie>();

            foreach (var recepieDbModel in recepiesDbModels.Result)
            {
                recepies.Add(new Recepie(
                    Id: recepieDbModel.Id,
                    UserId: recepieDbModel.UserId,
                    Title: recepieDbModel.Title,
                    CoockingProcess: recepieDbModel.CoockingProcess,
                    CoockingTime: recepieDbModel.CoockingTime,
                    Servings: recepieDbModel.Servings,
                    Ingredients: new List<Model.Ingredient.Ingredient>(), // Ingredients need to be fetched separately
                    Calories: recepieDbModel.Calories,
                    Proteins: recepieDbModel.Proteins,
                    Fats: recepieDbModel.Fats,
                    Carbohydrates: recepieDbModel.Carbohydrates,
                    Fiber: recepieDbModel.Fiber
                ));
            }
        }


    }
}
