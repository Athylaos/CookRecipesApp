using CookRecipesApp.Model.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.Service.Interface
{
    public interface IUserService
    {
        Task<bool> RegisterAsync(UserRegistrationDto registration);

        Task<User?> LoginAsync(string email, string password);

        Task LogoutAsync();

        Task<User?> GetCurrentUserAsync();

        Task RememberCurrentUserAsync(User user);

        Task<bool> IsUserLoggedInAsync();

        Task<User?> GetUserByIdAsync(int userId);

        Task UpdateUserAsync(User user);

        Task ChangePasswordAsync(int userId, string oldPassword, string newPassword);

        Task<bool> IsEmailRegistredAsync(string email);
    }
}
