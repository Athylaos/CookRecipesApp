using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.Model
{
    public class UserLoginDto
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull, Unique]
        public string Username { get; set; }
        [NotNull, Unique]
        public string Email { get; set; }
        [NotNull]
        public string Password { get; set; }
    }
}
