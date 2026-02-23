using CookRecipesApp.Service.Interface;
using CookRecipesApp.Shared.DTOs;
using CookRecipesApp.Shared.Models;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace CookRecipesApp.Service.Services
{
    public class RecipeService : IRecipeService
    {

        private readonly HttpClient _httpClient;
        private const string BaseUrl = "recipes";

        public RecipeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<bool?> ChangeFavoriteAsync(Guid recipeId, Guid userId)
        {
            try
            {
                var response = await _httpClient.PostAsync($"{BaseUrl}/toggleFavorite/{recipeId}", null);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<FavoriteResponse>();
                    return result?.IsFavorite;
                }

                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Toggle favorite error: {ex.Message}");
                return null;
            }
        }

        public Task<bool> IsFavoriteAsync(Guid recipeId, Guid userId)
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


        public async Task<RecipeDetailsDto?> GetRecipeDetailsAsync(Guid recipeId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<RecipeDetailsDto>($"{BaseUrl}/getRecipeDetails/{recipeId}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error while loading recipe details: {ex.Message}");
                return null;
            }
        }

        public async Task<List<Recipe>> GetRecipesAsync(int amount)
        {
            var response = await _httpClient.GetFromJsonAsync<List<Recipe>>($"{BaseUrl}/get?amount={amount}");
            return response ?? new List<Recipe>();
        }

        public Task<(float, int)> PostCommentAsync(Comment comment)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid?> SaveRecipeAsync(RecipeCreateDto createDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync<RecipeCreateDto>($"{BaseUrl}/create", createDto);
                if (response.IsSuccessStatusCode)
                {
                    var guid = await response.Content.ReadFromJsonAsync<Guid>();
                    return guid;
                }

                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error while creating recipe: {ex.Message}");
                return null;
            }
        }

        public Task UpdateRecipeAsync(Recipe recipe)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UserCommentedAsync(Guid recipeId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<RecipePreviewDto>> GetRecipePreviewsAsync(int amount)
        {
            var response = await _httpClient.GetFromJsonAsync<List<RecipePreviewDto>>($"{BaseUrl}/getPreviews?amount={amount}");
            return response ?? new List<RecipePreviewDto>();
        }

        public async Task<List<RecipePreviewDto>> GetFilteredRecipePreviewsAsync(RecipeFilterParametrs filter)
        {
            try
            {
                var url = $"{BaseUrl}/getPreviews/filtered?amount={filter.Amount}&onlyFavorites={filter.OnlyFavorites}";

                if (!string.IsNullOrEmpty(filter.SearchTerm)) url += $"&searchTerm={Uri.EscapeDataString(filter.SearchTerm)}";
                if (filter.CategoryId.HasValue) url += $"&categoryId={filter.CategoryId}";
                if (filter.MaxCookingTime.HasValue) url += $"&maxCookingTime={filter.MaxCookingTime}";
                if (filter.MaxDifficulty.HasValue) url += $"&maxDifficulty={filter.MaxDifficulty}";

                var response = await _httpClient.GetFromJsonAsync<List<RecipePreviewDto>>(url);
                return response ?? new();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Filter Error: {ex.Message}");
                return new();
            }
        }






        public class FavoriteResponse
        {
            public bool IsFavorite { get; set; }
        }
    }
}
