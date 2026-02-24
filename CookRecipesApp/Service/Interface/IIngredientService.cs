using CookRecipesApp.Shared.DTOs;
using CookRecipesApp.Shared.Models;

namespace CookRecipesApp.Service.Interface
{
    public interface IIngredientService
    {
        public Task AddIngredientAsync(Ingredient ingredient);
        public Task<Ingredient?> GetIngredientAsync(Guid id);
        public Task RemoveIngredientAsync(Guid id);
        public Task UpdateIngredientAsync(Ingredient ingredient);

        public Task<List<IngredientPreview>> GetIngredientPreviewsAsync(int amount);
    }
}
