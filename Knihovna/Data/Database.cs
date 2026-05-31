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

            EnableForeignKeys(connection);
            EnsureDatabaseCreated(connection);

            return connection;
        }

        private static void EnableForeignKeys(SqliteConnection connection)
        {
            using var command = connection.CreateCommand();

            command.CommandText = "PRAGMA foreign_keys = ON;";
            command.ExecuteNonQuery();
        }

        private static void EnsureDatabaseCreated(SqliteConnection connection)
        {
            using var command = connection.CreateCommand();

            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Knihy (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nazev TEXT NOT NULL,
                    Autor TEXT NOT NULL,
                    ISBN TEXT NOT NULL,
                    RokVydani INTEGER NOT NULL,
                    StavKnihy TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Ctenari (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Jmeno TEXT NOT NULL,
                    Prijmeni TEXT NOT NULL,
                    TelefonniCislo TEXT NOT NULL,
                    Email TEXT NOT NULL
                );

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

                CREATE TABLE IF NOT EXISTS Rezervace (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    KnihaId INTEGER NOT NULL,
                    CtenarId INTEGER NOT NULL,
                    DatumRezervace TEXT NOT NULL,
                    Stav TEXT NOT NULL,
                    FOREIGN KEY (KnihaId) REFERENCES Knihy(Id),
                    FOREIGN KEY (CtenarId) REFERENCES Ctenari(Id)
                );

                CREATE UNIQUE INDEX IF NOT EXISTS IX_Knihy_ISBN
                ON Knihy (ISBN);

                CREATE UNIQUE INDEX IF NOT EXISTS IX_Ctenari_Email
                ON Ctenari (Email);

                CREATE INDEX IF NOT EXISTS IX_Vypujcky_KnihaId
                ON Vypujcky (KnihaId);

                CREATE INDEX IF NOT EXISTS IX_Vypujcky_CtenarId
                ON Vypujcky (CtenarId);

                CREATE INDEX IF NOT EXISTS IX_Vypujcky_Stav
                ON Vypujcky (Stav);

                CREATE INDEX IF NOT EXISTS IX_Vypujcky_KnihaId_Stav
                ON Vypujcky (KnihaId, Stav);

                CREATE INDEX IF NOT EXISTS IX_Vypujcky_CtenarId_Stav
                ON Vypujcky (CtenarId, Stav);

                CREATE INDEX IF NOT EXISTS IX_Rezervace_KnihaId
                ON Rezervace (KnihaId);

                CREATE INDEX IF NOT EXISTS IX_Rezervace_CtenarId
                ON Rezervace (CtenarId);

                CREATE INDEX IF NOT EXISTS IX_Rezervace_Stav
                ON Rezervace (Stav);

                CREATE INDEX IF NOT EXISTS IX_Rezervace_KnihaId_Stav_DatumRezervace_Id
                ON Rezervace (KnihaId, Stav, DatumRezervace, Id);

                CREATE INDEX IF NOT EXISTS IX_Rezervace_CtenarId_Stav
                ON Rezervace (CtenarId, Stav);
            ";

            command.ExecuteNonQuery();
        }
    }
}