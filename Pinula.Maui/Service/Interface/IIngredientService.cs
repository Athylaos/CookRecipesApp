using Pinula.Service.Services;
using Pinula.Shared.DTOs;
using Pinula.Shared.Models;

namespace Pinula.Service.Interface
{
    public interface IIngredientService
    {
        public Task<CreateIngredientResponse> CreateIngredientAsync(IngredientCreateDto ingredientDto);
        public Task<Ingredient?> GetIngredientAsync(Guid id);
        public Task RemoveIngredientAsync(Guid id);
        public Task UpdateIngredientAsync(Ingredient ingredient);

        public Task<List<IngredientPreview>> GetIngredientPreviewsAsync(int amount);
    }
}
