using CookRecipesApp.Service.Interface;
using CookRecipesApp.Shared.DTOs;
using CookRecipesApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace CookRecipesApp.Service.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "ingredients";

        public IngredientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task AddIngredientAsync(Ingredient ingredient)
        {
            throw new NotImplementedException();
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
