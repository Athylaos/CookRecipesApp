using CookRecipesApp.Model.Ingredient;

namespace CookRecipesApp.Service.Interface
{
    public interface IIngredientService
    {
        public Task AddIngredientAsync(Ingredient ingredient);
        public Task<Ingredient?> GetIngredientAsync(int id);
        public Task RemoveIngredientAsync(int id);
        public Task UpdateIngredientAsync(Ingredient ingredient);

        public Task<List<Ingredient>> GetAllIngredientsAsync();
        public Task<List<UnitDbModel>> GetAllServingUnitsAsync();

    }
}
