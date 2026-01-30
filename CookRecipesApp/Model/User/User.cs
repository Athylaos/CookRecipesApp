using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.Model.User
{
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Surname { get; set; } = string.Empty;

        public int RecepiesAdded { get; set; } = 0;

        public DateOnly UserCreated { get; set; }

        public string Role { get; set; } = "User";

        public string? AvatarUrl { get; set; }

    }
}
