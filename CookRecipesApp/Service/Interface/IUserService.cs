using CookRecipesApp.Shared.Models;

namespace CookRecipesApp.Service.Interface
{
    public interface IUserService
    {
        Task<bool> RegisterAsync(User user);
        Task<User?> LoginAsync(string email, string password);
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
