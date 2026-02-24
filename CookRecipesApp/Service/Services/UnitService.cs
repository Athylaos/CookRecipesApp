using CookRecipesApp.Service.Interface;
using CookRecipesApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace CookRecipesApp.Service.Services
{
    public class UnitService : IUnitService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "units";

        public UnitService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<Unit>> GetAllServingUnitsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<Unit>>($"{BaseUrl}/getServing");
            return response ?? new List<Unit>();
        }

        public async Task<List<Unit>> GetAllUnitsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<Unit>>($"{BaseUrl}/get");
            return response ?? new List<Unit>();
        }

        public Task<List<Unit>> GetIngredientUnitsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
