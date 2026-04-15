using Pinula.Shared.DTOs;
using Pinula.Shared.Models;

namespace Pinula.Service.Interface
{
    public interface IUserService
    {
        Task<bool> RegisterAsync(UserRegistrationDto user);
        Task<User?> LoginAsync(UserLoginDto loginDto);
        void Logout();

        Task<UserDisplayDto?> GetCurrentUserAsync();
        Task RememberCurrentUserAsync(User user);
        Task<bool> IsUserLoggedInAsync();

        Task<UserDisplayDto?> GetUserByIdAsync(Guid userId);
        Task<bool> UpdateUserAsync(UserUpdateDto userUpdateDto, FileResult? photo);
        Task ChangePasswordAsync(Guid userId, string oldPassword, string newPassword);

        Task<bool> IsEmailRegistredAsync(string email);
    }
}
