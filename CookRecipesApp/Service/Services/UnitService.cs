using CookRecipesApp.Service.Interface;
using CookRecipesApp.Shared.DTOs;
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
        public async Task<List<UnitPreviewDto>> GetAllServingUnitsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<UnitPreviewDto>>($"{BaseUrl}/getServing");
            return response ?? new List<UnitPreviewDto>();
        }

        public async Task<List<UnitPreviewDto>> GetAllUnitsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<UnitPreviewDto>>($"{BaseUrl}/get");
            return response ?? new List<UnitPreviewDto>();
        }

        public Task<List<UnitPreviewDto>> GetIngredientUnitsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
