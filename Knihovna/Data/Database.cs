using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace Knihovna
{
    public static class Database
    {
        private static readonly string DatabaseFileName = "knihovna.db";

        public static string DatabasePath
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DatabaseFileName);
            }
        }

        public static string ConnectionString
        {
            get
            {
                return $"Data Source={DatabasePath}";
            }
        }

        public static SqliteConnection CreateConnection()
        {
            var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "PRAGMA foreign_keys = ON;";
            command.ExecuteNonQuery();

            return connection;
        }
    }
}