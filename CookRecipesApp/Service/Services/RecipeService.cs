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
using System.Text.Json;

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

        public Task DeleteRecipeAsync(Guid id)
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

        public async Task<PostCommentResponse?> PostCommentAsync(Comment comment)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync<Comment>($"{BaseUrl}/postComment", comment);
                if (response.IsSuccessStatusCode)
                {
                    var c = await response.Content.ReadFromJsonAsync<PostCommentResponse>();
                    return c;
                }
                Debug.WriteLine($"Error while posting comment: {response.StatusCode}");
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error while posting comment: {ex.Message}");
                return null;
            }
        }

        public async Task<Guid?> SaveRecipeAsync(RecipeCreateDto createDto, FileResult? photo)
        {
            try
            {
                using var content = new MultipartFormDataContent();
                var recipeJson = JsonSerializer.Serialize(createDto);
                content.Add(new StringContent(recipeJson, Encoding.UTF8, "application/json"), "recipeData");

                if (photo != null)
                {
                    var fileStream = await photo.OpenReadAsync();
                    var fileContent = new StreamContent(fileStream);

                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(photo.ContentType);

                    content.Add(fileContent, "image", photo.FileName);
                }

                var response = await _httpClient.PostAsync($"{BaseUrl}/create", content);
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

        public async Task<List<RecipePreviewDto>> GetRecipePreviewsAsync(int amount)
        {
            var response = await _httpClient.GetFromJsonAsync<List<RecipePreviewDto>>($"{BaseUrl}/getPreviews?amount={amount}");
            return response ?? new List<RecipePreviewDto>();
        }

        public async Task<List<RecipePreviewDto>> GetFilteredRecipePreviewsAsync(RecipeFilterParametrs filter, CancellationToken? ct)
        {
            try
            {
                var url = $"{BaseUrl}/getPreviews/filtered?" +
                          $"amount={filter.Amount}" +
                          $"&skip={filter.Skip}" +
                          $"&onlyFavorites={filter.OnlyFavorites.ToString().ToLower()}" +
                          $"&onlyMine={filter.OnlyMine.ToString().ToLower()}" +
                          $"&sort={(int)filter.Sort}" +
                          $"&sortDescending={filter.SortDescending.ToString().ToLower()}";

                if (!string.IsNullOrEmpty(filter.SearchTerm))
                    url += $"&searchTerm={Uri.EscapeDataString(filter.SearchTerm)}";

                if (filter.CategoryId.HasValue)
                    url += $"&categoryId={filter.CategoryId}";

                if (filter.MinRating.HasValue)
                    url += $"&minRating={filter.MinRating}";

                if (filter.MaxCookingTime.HasValue)
                    url += $"&maxCookingTime={filter.MaxCookingTime}";

                if (filter.MaxDifficulty.HasValue)
                    url += $"&maxDifficulty={filter.MaxDifficulty}";

                if (filter.MaxCalories.HasValue)
                    url += $"&maxCalories={filter.MaxCalories}";

                var response = await _httpClient.GetFromJsonAsync<List<RecipePreviewDto>>(url, ct ?? CancellationToken.None);
                return response ?? new();
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Request was cancelled by user.");
                return new();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Filter Error: {ex.Message}");
                return new();
            }
        }

        public async Task<PostCommentResponse?> GetRecipeCommentAsync(Guid recipeId, Guid? userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/getUserComment/{recipeId}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<PostCommentResponse?>();
                }
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error while getting comment: {ex.Message}");
                return null;
            }
        }

        public async Task<DeleteCommentResponse?> DeleteRecipeCommentAsync(Guid recipeId, Guid? userId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{BaseUrl}/deleteComment/{recipeId}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<DeleteCommentResponse>();
                }
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error while removing comment: {ex.Message}");
                return null;
            }
        }

        public class FavoriteResponse
        {
            public bool IsFavorite { get; set; }
        }
    }
}
