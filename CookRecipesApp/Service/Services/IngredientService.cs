using CookRecipesApp.Service.Interface;
using CookRecipesApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.Service.Services
{
    public class IngredientService : IIngredientService
    {
        public Task AddIngredientAsync(Ingredient ingredient)
        {
            throw new NotImplementedException();
        }

        public Task<List<Ingredient>> GetAllIngredientsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Unit>> GetAllServingUnitsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Unit>> GetAllUnitsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Ingredient?> GetIngredientAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task RemoveIngredientAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateIngredientAsync(Ingredient ingredient)
        {
            throw new NotImplementedException();
        }
    }
}
