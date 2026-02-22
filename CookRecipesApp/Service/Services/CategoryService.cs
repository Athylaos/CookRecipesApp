using CookRecipesApp.Service.Interface;
using CookRecipesApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Diagnostics;
using System.Text;

namespace CookRecipesApp.Service.Services
{
    public class CategoryService : ICategoryService
    {

        private readonly HttpClient _httpClient;
        private const string BaseUrl = "categories";

        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/getAll");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<Category>>() ?? new();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"error while loading categories: {ex.Message}");
            }
            return new List<Category>();
        }

        public Task<Category?> GetCategoryByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Category>> GetChildCategoriesAsync(Guid parentId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Category>> GetMainCategoriesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/getMain");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<Category>>() ?? new();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"error while loading categories: {ex.Message}");
            }
            return new List<Category>();
        }

        public Task<List<Category>> GetRecepieCategoriesAsync(Guid recepieId)
        {
            throw new NotImplementedException();
        }
    }
}
