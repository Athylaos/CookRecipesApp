using CookRecipesApp.Service.Interface;
using CookRecipesApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.Service.Services
{
    internal class UserService : IUserService
    {
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

        public Task<User?> LoginAsync(string email, string password)
        {
            throw new NotImplementedException();
        }

        public Task LogoutAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> RegisterAsync(User user)
        {
            throw new NotImplementedException();
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
