using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace CookRecipesApp.Model.User
{
    public class UserDbModel
    {
        [SQLite.NotNull, PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [SQLite.NotNull, Unique]
        public string Username { get; set; }

        [SQLite.NotNull, Unique]
        public string Email { get; set; }

        [SQLite.NotNull]
        public string PasswordHash { get; set; }

        

    }
}
