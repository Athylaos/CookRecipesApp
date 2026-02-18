using CookRecipesApp.Service.Interface;
using CookRecipesApp.Shared.DTOs;
using CookRecipesApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Diagnostics;

namespace CookRecipesApp.Service.Services
{
    internal class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://10.0.1.160:7141/api/users";

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task ChangePasswordAsync(Guid userId, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetCurrentUserAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetUserByIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsEmailRegistredAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsUserLoggedInAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<User?> LoginAsync(UserLoginDto loginDto)
        {
            try
            {

                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/login", loginDto);

                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<User>();

                    // local storage..
                    return user;
                }

                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Login Error: {ex.Message}");
                return null;
            }
        }

        public Task LogoutAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RegisterAsync(UserRegistrationDto userDto)
        {
            try
            {
                Debug.WriteLine($"Zkouším volat: {BaseUrl}/register");

                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/register", userDto);
                return response.IsSuccessStatusCode;
            }
            catch (OperationCanceledException)
            {
                return false;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                return false;
            }
            return false;
        }

        public Task RememberCurrentUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
