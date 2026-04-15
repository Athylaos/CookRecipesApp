using Pinula.Service.Interface;
using Pinula.Shared.DTOs;
using Pinula.Shared.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Pinula.Service.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "users";

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task ChangePasswordAsync(Guid userId, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDisplayDto?> GetCurrentUserAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/getMe");

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Debug.WriteLine("User not logged in");
                    return null;
                }

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<UserDisplayDto>();
                }

                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Getting current user error: {ex.Message}");
                return null;
            }
        }

        public async Task<UserDisplayDto?> GetUserByIdAsync(Guid userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/getUserDisplay/{userId}");

                if (response.StatusCode == HttpStatusCode.NotFound)
                    return null;

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<UserDisplayDto>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Getting user error: {ex.Message}");
                return null;
            }
        }

        public Task<bool> IsEmailRegistredAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsUserLoggedInAsync()
        {
            if(string.IsNullOrEmpty(await SecureStorage.Default.GetAsync("auth_token")) || string.IsNullOrEmpty(await SecureStorage.Default.GetAsync("user_id")))
            {
                return false;
            }
            return true;
        }

        public async Task<User?> LoginAsync(UserLoginDto loginDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/login", loginDto);

                if (response.IsSuccessStatusCode)
                {
                    var authResult = await response.Content.ReadFromJsonAsync<LoginResponse>();

                    if (authResult != null)
                    {
                        await SecureStorage.Default.SetAsync("auth_token", authResult.Token);

                        await SecureStorage.Default.SetAsync("user_id", authResult.User.Id.ToString());

                        return authResult.User;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Login Error: {ex.Message}");
                return null;
            }
        }

        public void Logout()
        {
            SecureStorage.Default.Remove("auth_token");
            SecureStorage.Default.Remove("user_id");
        }

        public async Task<bool> RegisterAsync(UserRegistrationDto userDto)
        {
            try
            {
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

        public async Task<bool> UpdateUserAsync(UserUpdateDto? userUpdateDto, FileResult? photo)
        {
            try
            {
                using var content = new MultipartFormDataContent();
                var userJson = JsonSerializer.Serialize(userUpdateDto);
                content.Add(new StringContent(userJson, Encoding.UTF8, "application/json"), "userData");

                if (photo != null)
                {
                    var fileStream = await photo.OpenReadAsync();
                    var fileContent = new StreamContent(fileStream);
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(photo.ContentType);
                    content.Add(fileContent, "image", photo.FileName);
                }
                var response = await _httpClient.PutAsync($"{BaseUrl}/update", content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error while updating user: {ex.Message}");
                return false;
            }
        }

    }
}
