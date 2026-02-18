using CookRecipesApp.Shared.Models;

namespace CookRecipesApp.Service.Interface
{
    public interface IUnitService
    {
        public Task<List<Unit>> GetAllUnitsAsync();
        public Task<List<Unit>> GetAllServingUnitsAsync();
        public Task<List<Unit>> GetIngredientUnitsAsync();


    }
}
