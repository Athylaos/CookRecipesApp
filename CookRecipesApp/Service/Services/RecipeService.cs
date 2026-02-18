using CookRecipesApp.Service.Interface;
using CookRecipesApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.Service.Services
{
    internal class RecipeService : IRecipeService
    {
        public Task ChangeFavoriteAsync(Guid recipeId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<(float, int)> DeleteCommentByUserAndRecipeAsync(Guid recipeId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRecipeAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Comment?> GetCommentByUserAndRecipeAsync(Guid recipeId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Recipe>> GetFavoriteRecipesAsync(Guid userId, int amount)
        {
            throw new NotImplementedException();
        }

        public Task<Recipe> GetRecipeAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Recipe>> GetRecipesAsync(int amount)
        {
            throw new NotImplementedException();
        }

        public Task<List<Recipe>> GetRecipesByCategoryAsync(Guid categoryId, int amount)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsFavoriteAsync(Guid recipeId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<(float, int)> PostCommentAsync(Comment comment)
        {
            throw new NotImplementedException();
        }

        public Task SaveRecipeAsync(Recipe recipe)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRecipeAsync(Recipe recipe)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UserCommentedAsync(Guid recipeId, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
