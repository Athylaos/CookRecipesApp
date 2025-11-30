using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.Model.User
{
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public int RecepiesAdded { get; set; } = 0;

        //public DateOnly UserCreated { get; set; }

        public string Role { get; set; } = "User";

        public string? AvatarUrl { get; set; }

    }
}
