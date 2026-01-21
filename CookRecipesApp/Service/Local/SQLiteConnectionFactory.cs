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

        public async Task ResetDatabaseAsync()
        {
            var connection = CreateConnection();
            try
            {
                // Zkusit zavřít připojení
                await connection.CloseAsync();
            }
            catch { /* ignorovat */ }

            // Smazat soubor
            if (File.Exists(Path.Combine(FileSystem.AppDataDirectory, "userData.db3")))
            {
                File.Delete(Path.Combine(FileSystem.AppDataDirectory, "userData.db3"));
            }

            // Vytvořit nové připojení
            var newConnection = CreateConnection();
        }
    }
}
