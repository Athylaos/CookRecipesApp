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

        public RecepiesService(SQLiteConnectionFactory factory)
        {
            _database = factory.CreateConnection();
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
