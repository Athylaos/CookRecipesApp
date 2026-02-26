using CookRecipesApp.Service.Interface;
using CookRecipesApp.Shared.DTOs;
using CookRecipesApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text;

namespace CookRecipesApp.Service.Services
{
    public class CreateIngredientResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class IngredientService : IIngredientService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "ingredients";

        public IngredientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CreateIngredientResponse> CreateIngredientAsync(IngredientCreateDto ingredientDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/create", ingredientDto);

                if (response.IsSuccessStatusCode)
                {
                    var msg = await response.Content.ReadAsStringAsync();
                    return new CreateIngredientResponse { IsSuccess = true, Message = msg };
                }

                if (response.Content != null)
                {
                    try
                    {
                        var errorObj = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                        if (errorObj != null && errorObj.TryGetValue("message", out var apiMessage))
                        {
                            return new CreateIngredientResponse { IsSuccess = false, Message = apiMessage };
                        }
                    }
                    catch
                    {
                        return new CreateIngredientResponse { IsSuccess = false, Message = $"Error: {response.StatusCode}" };
                    }
                }

                return new CreateIngredientResponse { IsSuccess = false, Message = "Unknown error occurred" };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error while creating ingredient: {ex.Message}");
                return new CreateIngredientResponse { IsSuccess = false, Message = "Connection to server failed." };
            }
        }

        public async Task<List<IngredientPreview>> GetIngredientPreviewsAsync(int amount)
        {
            var response = await _httpClient.GetFromJsonAsync<List<IngredientPreview>>($"{BaseUrl}/getPreviews?amount={amount}");
            return response ?? new List<IngredientPreview>();
        }


        public Task<Ingredient?> GetIngredientAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task RemoveIngredientAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateIngredientAsync(Ingredient ingredient)
        {
            throw new NotImplementedException();
        }
    }
}
