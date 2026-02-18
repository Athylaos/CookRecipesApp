using CookRecipesApp.Shared.DTOs;
using CookRecipesApp.Shared.Models;

namespace CookRecipesApp.Service.Interface
{
    public interface IUserService
    {
        Task<bool> RegisterAsync(UserRegistrationDto user);
        Task<User?> LoginAsync(UserLoginDto loginDto);
        Task LogoutAsync();

        Task<User?> GetCurrentUserAsync();
        Task RememberCurrentUserAsync(User user);
        Task<bool> IsUserLoggedInAsync();

        Task<User?> GetUserByIdAsync(Guid userId);
        Task UpdateUserAsync(User user);
        Task ChangePasswordAsync(Guid userId, string oldPassword, string newPassword);

        Task<bool> IsEmailRegistredAsync(string email);
    }
}
