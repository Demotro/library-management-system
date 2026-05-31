using Microsoft.Data.Sqlite;

namespace Knihovna
{
    public static class DatabaseInitializer
    {
        public static void Initialize()
        {
            using var connection = new SqliteConnection(Database.ConnectionString);
            connection.Open();

            string createKnihyTable = @"
                CREATE TABLE IF NOT EXISTS Knihy (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nazev TEXT NOT NULL,
                    Autor TEXT NOT NULL,
                    ISBN TEXT NOT NULL UNIQUE,
                    RokVydani INTEGER NOT NULL,
                    StavKnihy TEXT NOT NULL
                );
            ";

            string createCtenariTable = @"
                CREATE TABLE IF NOT EXISTS Ctenari (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Jmeno TEXT NOT NULL,
                    Prijmeni TEXT NOT NULL,
                    TelefonniCislo TEXT NOT NULL,
                    Email TEXT NOT NULL UNIQUE
                );
            ";

            string createVypujckyTable = @"
                CREATE TABLE IF NOT EXISTS Vypujcky (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    KnihaId INTEGER NOT NULL,
                    CtenarId INTEGER NOT NULL,
                    DatumVypujceni TEXT NOT NULL,
                    DatumVraceni TEXT NULL,
                    Stav TEXT NOT NULL,
                    FOREIGN KEY (KnihaId) REFERENCES Knihy(Id),
                    FOREIGN KEY (CtenarId) REFERENCES Ctenari(Id)
                );
            ";

            string createRezervaceTable = @"
                CREATE TABLE IF NOT EXISTS Rezervace (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    KnihaId INTEGER NOT NULL,
                    CtenarId INTEGER NOT NULL,
                    DatumRezervace TEXT NOT NULL,
                    Stav TEXT NOT NULL,
                    FOREIGN KEY (KnihaId) REFERENCES Knihy(Id),
                    FOREIGN KEY (CtenarId) REFERENCES Ctenari(Id)
                );
            ";

            ExecuteCommand(connection, createKnihyTable);
            ExecuteCommand(connection, createCtenariTable);
            ExecuteCommand(connection, createVypujckyTable);
            ExecuteCommand(connection, createRezervaceTable);
        }

        private static void ExecuteCommand(SqliteConnection connection, string commandText)
        {
            using var command = connection.CreateCommand();
            command.CommandText = commandText;
            command.ExecuteNonQuery();
        }
    }
}