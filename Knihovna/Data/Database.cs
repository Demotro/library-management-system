using System;
using System.IO;

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
    }
}