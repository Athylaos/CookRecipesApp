using Pinula.Service.Interface;
using Pinula.Shared.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Diagnostics;
using System.Text;

namespace Pinula.Service.Services
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

        public async Task<Category?> GetCategoryByIdAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/get/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Category>() ?? new();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"error while loading categories: {ex.Message}");
            }
            return new Category();
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
