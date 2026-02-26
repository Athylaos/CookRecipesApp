using CookRecipesApp.Shared.DTOs;
using CookRecipesApp.Shared.Models;

namespace CookRecipesApp.Service.Interface
{
    public interface IUnitService
    {
        public Task<List<UnitPreviewDto>> GetAllUnitsAsync();
        public Task<List<UnitPreviewDto>> GetAllServingUnitsAsync();
        public Task<List<UnitPreviewDto>> GetIngredientUnitsAsync();


    }
}
