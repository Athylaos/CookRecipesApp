using CookRecipesApp.Service.Interface;
using CookRecipesApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.Service.Services
{
    public class UnitService : IUnitService
    {
        public Task<List<Unit>> GetAllServingUnitsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Unit>> GetAllUnitsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Unit>> GetIngredientUnitsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
