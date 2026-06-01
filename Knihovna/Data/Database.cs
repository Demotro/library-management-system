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
            SeedSampleData(connection);

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

        private static void SeedSampleData(SqliteConnection connection)
        {
            if (CountRows(connection, "Knihy") == 0)
            {
                InsertSampleBooks(connection);
            }

            if (CountRows(connection, "Ctenari") == 0)
            {
                InsertSampleReaders(connection);
            }
        }

        private static int CountRows(SqliteConnection connection, string tableName)
        {
            using var command = connection.CreateCommand();

            command.CommandText = $"SELECT COUNT(*) FROM {tableName};";

            return Convert.ToInt32(command.ExecuteScalar());
        }

        private static void InsertSampleBooks(SqliteConnection connection)
        {
            using var command = connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO Knihy (Nazev, Autor, ISBN, RokVydani, StavKnihy)
                VALUES
                ('1984', 'George Orwell', '9780451524935', 1949, 'Dobra'),
                ('Animal Farm', 'George Orwell', '9780451526342', 1945, 'Dobra'),
                ('The Hobbit', 'J. R. R. Tolkien', '9780261102217', 1937, 'Dobra'),
                ('The Lord of the Rings', 'J. R. R. Tolkien', '9780618640157', 1954, 'Opotrebovana'),
                ('Dune', 'Frank Herbert', '9780441172719', 1965, 'Dobra'),
                ('Brave New World', 'Aldous Huxley', '9780060850524', 1932, 'Opotrebovana'),
                ('Harry Potter and the Philosopher''s Stone', 'J. K. Rowling', '9780747532699', 1997, 'Nova'),
                ('Crime and Punishment', 'Fyodor Dostoevsky', '9780140449136', 1866, 'Dobra'),
                ('Clean Code', 'Robert C. Martin', '9780132350884', 2008, 'Dobra'),
                ('The Pragmatic Programmer', 'Andrew Hunt', '9780201616224', 1999, 'Opotrebovana'),
                ('Design Patterns', 'Erich Gamma', '9780201633610', 1994, 'Dobra'),
                ('C# in Depth', 'Jon Skeet', '9781617294532', 2019, 'Nova');
            ";

            command.ExecuteNonQuery();
        }

        private static void InsertSampleReaders(SqliteConnection connection)
        {
            using var command = connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO Ctenari (Jmeno, Prijmeni, TelefonniCislo, Email)
                VALUES
                ('Jan', 'Novak', '777123456', 'jan.novak@example.com'),
                ('Anna', 'Svobodova', '777654321', 'anna.svobodova@example.com'),
                ('Petr', 'Dvorak', '777111222', 'petr.dvorak@example.com'),
                ('Lucie', 'Cerna', '777222333', 'lucie.cerna@example.com'),
                ('Martin', 'Prochazka', '777333444', 'martin.prochazka@example.com'),
                ('Tereza', 'Kralova', '777444555', 'tereza.kralova@example.com'),
                ('David', 'Vesely', '777555666', 'david.vesely@example.com'),
                ('Eva', 'Mala', '777666777', 'eva.mala@example.com');
            ";

            command.ExecuteNonQuery();
        }
    }
}