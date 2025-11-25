using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.Service
{
    public class SQLiteConnectionFactory
    {
        public ISQLiteAsyncConnection CreateConnection()
        {
            return new SQLiteAsyncConnection(
                Path.Combine(FileSystem.AppDataDirectory, "userData.db3"),
                SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache
                );
        }
    }
}
