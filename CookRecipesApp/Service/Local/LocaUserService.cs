using CookRecipesApp.Model.User;
using CookRecipesApp.Service.Interface;
using SQLite;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CookRecipesApp.Service
{
    public class LocalUserService : IUserService
    {
        private ISQLiteAsyncConnection _database;

        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 100_000;

        public LocalUserService(SQLiteConnectionFactory factory)
        {
            _database = factory.CreateConnection();
        }

        private (string Hash, string Salt) HashPassword(string password)
        {
            byte[] saltBytes = RandomNumberGenerator.GetBytes(SaltSize);

            byte[] hashBytes = Rfc2898DeriveBytes.Pbkdf2(
                password: Encoding.UTF8.GetBytes(password),
                salt: saltBytes,
                iterations: Iterations,
                hashAlgorithm: HashAlgorithmName.SHA256,
                outputLength: KeySize);

            string hash = Convert.ToBase64String(hashBytes);
            string salt = Convert.ToBase64String(saltBytes);

            return (hash, salt);
        }

        private bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            byte[] saltBytes = Convert.FromBase64String(storedSalt);

            byte[] hashBytes = Rfc2898DeriveBytes.Pbkdf2(
                password: Encoding.UTF8.GetBytes(password),
                salt: saltBytes,
                iterations: Iterations,
                hashAlgorithm: HashAlgorithmName.SHA256,
                outputLength: KeySize);

            string computedHash = Convert.ToBase64String(hashBytes);

            return CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(computedHash),
                Encoding.UTF8.GetBytes(storedHash));
        }

        private User UserDbModelToUser(UserDbModel userDbModel)
        {
            User user = new User()
            {
                Id = userDbModel.Id,
                Email = userDbModel.Email,
                Name = userDbModel.Name,
                Surname = userDbModel.Surname,
                RecepiesAdded = userDbModel.RecepiesAdded,
                //UserCreated = userDbModel.UserCreated,
                Role = userDbModel.Role,
                AvatarUrl = userDbModel.AvatarUrl
            };
            return user;
        }

        private UserDbModel UserAndUserRegistrationDtoToUserDbModel(User? user, UserRegistrationDto? userDto)
        {
            UserDbModel userDbModel = new UserDbModel();

            if(user != null)
            {
                userDbModel.Id = user.Id;
                userDbModel.Email = user.Email;
                userDbModel.Name = user.Name;
                userDbModel.Surname = user.Surname;
                userDbModel.RecepiesAdded = user.RecepiesAdded;
                //userDbModel.UserCreated = user.UserCreated;
                userDbModel.Role = user.Role;
                userDbModel.AvatarUrl = user.AvatarUrl;
            }

            if(userDto != null)
            {
                if (string.IsNullOrEmpty(userDbModel.Email))
                {
                    userDbModel.Email = userDto.Email;
                }
                userDbModel.PasswordHash = userDto.PasswordHash;
                userDbModel.PasswordSalt = userDto.PasswordSalt;
            }

            return userDbModel;
        }





        public async Task ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            var user = await _database.Table<UserDbModel>()
                                      .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                throw new ArgumentException("User not found");

            if (!VerifyPassword(oldPassword, user.PasswordHash, user.PasswordSalt))
                throw new ArgumentException("Old password is not valid");

            var (newHash, newSalt) = HashPassword(newPassword);
            user.PasswordHash = newHash;
            user.PasswordSalt = newSalt;

            await _database.UpdateAsync(user);
        }

        public async Task<User?> GetCurrentUserAsync()
        {
            var idString = await SecureStorage.GetAsync("current_user_id");

            if (string.IsNullOrEmpty(idString) || !int.TryParse(idString, out var id))
                return null;

            var userDb = await _database.Table<UserDbModel>()
                                        .FirstOrDefaultAsync(x => x.Id == id);

            return userDb is null ? null : UserDbModelToUser(userDb);
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            var userDbModel = await _database.Table<UserDbModel>().Where(x => x.Id == userId).FirstOrDefaultAsync();

            if(userDbModel == null)
            {
                return null;
            }

            return UserDbModelToUser(userDbModel);
        }

        public async Task<bool> IsUserLoggedInAsync()
        {
            var id = await SecureStorage.GetAsync("current_user_id");
            return !string.IsNullOrEmpty(id);
        }

        public async Task<User?> LoginAsync(string email, string password)
        {
            var userDb = await _database.Table<UserDbModel>()
                                        .FirstOrDefaultAsync(x => x.Email == email);

            if (userDb == null) return null;

            if (!VerifyPassword(password, userDb.PasswordHash, userDb.PasswordSalt)) return null;

            var user = UserDbModelToUser(userDb);
            await RememberCurrentUserAsync(user);

            return user;
        }

        public Task LogoutAsync()
        {
            SecureStorage.Remove("current_user_id");
            return Task.CompletedTask;
        }

        public async Task<bool> RegisterAsync(UserRegistrationDto registration)
        {
            if(registration == null) throw new ArgumentNullException("Argument is null");

            if(await _database.Table<UserDbModel>().Where(x => x.Email == registration.Email).FirstOrDefaultAsync() != null)
            {
                return false;
            }


            var (hash, salt) = HashPassword(registration.Password);
            registration.PasswordHash = hash;
            registration.PasswordSalt = salt;

            var userDbModel = UserAndUserRegistrationDtoToUserDbModel(null, registration);

            await _database.InsertAsync(userDbModel);
            return true;
        }

        public async Task RememberCurrentUserAsync(User user)
        {
            if (user == null) throw new ArgumentNullException("user is null");

            await SecureStorage.SetAsync("current_user_id", user.Id.ToString());
        }

        public async Task UpdateUserAsync(User user) // only used for non-sensitive information, use ChangePasswordAsync for changing paswd
        {
            if (user == null) throw new ArgumentNullException("Argument is null");

            var userDbModelNew = UserAndUserRegistrationDtoToUserDbModel(user, null);

            await _database.UpdateAsync(userDbModelNew);
        }

        public async Task<bool> IsEmailRegistredAsync(string email)
        {
            int count = await _database.Table<UserDbModel>()
                                       .Where(x => x.Email == email)
                                       .CountAsync();
            return count > 0;
        }
    }
}
