

namespace CookRecipesApp.Model.User
{
    public class UserRegistrationDto
    {
        public int Id { get; set; }

        public string Email { get; set; } = string.Empty;


        public string Password { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string PasswordSalt { get; set; } = string.Empty;

        public DateOnly RegistredAt { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }

    }
}
