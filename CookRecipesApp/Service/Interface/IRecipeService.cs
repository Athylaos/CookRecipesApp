using CookRecipesApp.Shared.Models;

namespace CookRecipesApp.Service.Interface
{
    public interface IRecipeService
    {
        public Task SaveRecipeAsync(Recipe recipe);
        public Task<Recipe> GetRecipeAsync(Guid id);
        public Task UpdateRecipeAsync(Recipe recipe);
        public Task DeleteRecipeAsync(Guid id);

        public Task<List<Recipe>> GetRecipesAsync(int amount);
        public Task<List<Recipe>> GetRecipesByCategory(Guid categoryId, int amount);
        public Task<List<Recipe>> GetFavoriteRecipes(Guid userId, int amount);

        public Task ChangeFavoriteAsync(Guid recipeId, Guid userId);
        public Task<bool> IsFavoriteAsync(Guid recipeId, Guid userId);

        public Task<bool> UserCommentedAsync(Guid recipeId, Guid  userId);
        public Task<(float, int)> PostCommentAsync(Comment comment);
        public Task<Comment?> GetCommentByUserAndRecipeAsync(Guid recipeId, Guid userId);
        public Task<(float, int)> DeleteCommentByUserAndRecipeAsync(Guid recipeId, Guid userId);

    }
}
