using CookRecipesApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.Shared.DTOs
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;

        public User User { get; set; } = null!;

    }
}
