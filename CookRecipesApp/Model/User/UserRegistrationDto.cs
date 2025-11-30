using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.Model.User
{
    public class UserRegistrationDto
    {
        [NotNull, PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull, Unique]
        public string Email { get; set; }

        public string Password {  get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }
    }
}
