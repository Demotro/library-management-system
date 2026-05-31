using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace Knihovna
{
    public class KnihaRepository : IKnihaRepository
    {
        public List<Kniha> GetAll()
        {
            List<Kniha> knihy = new List<Kniha>();

            using var connection = Database.CreateConnection();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT Id, Nazev, Autor, ISBN, RokVydani, StavKnihy
                FROM Knihy
                ORDER BY Nazev;
            ";

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                Kniha kniha = CreateBookFromReader(reader);
                knihy.Add(kniha);
            }

            return knihy;
        }

        public Kniha? GetById(int id)
        {
            using var connection = Database.CreateConnection();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT Id, Nazev, Autor, ISBN, RokVydani, StavKnihy
                FROM Knihy
                WHERE Id = @Id;
            ";

            command.Parameters.AddWithValue("@Id", id);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return CreateBookFromReader(reader);
            }

            return null;
        }

        public void Add(Kniha kniha)
        {
            using var connection = Database.CreateConnection();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Knihy (Nazev, Autor, ISBN, RokVydani, StavKnihy)
                VALUES (@Nazev, @Autor, @ISBN, @RokVydani, @StavKnihy);

                SELECT last_insert_rowid();
            ";

            command.Parameters.AddWithValue("@Nazev", kniha.Nazev);
            command.Parameters.AddWithValue("@Autor", kniha.Autor);
            command.Parameters.AddWithValue("@ISBN", kniha.ISBN);
            command.Parameters.AddWithValue("@RokVydani", kniha.RokVydani);
            command.Parameters.AddWithValue("@StavKnihy", kniha.StavKnihy);

            long newId = Convert.ToInt64(command.ExecuteScalar());
            kniha.Id = (int)newId;
        }

        public void Update(Kniha kniha)
        {
            using var connection = Database.CreateConnection();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                UPDATE Knihy
                SET Nazev = @Nazev,
                    Autor = @Autor,
                    ISBN = @ISBN,
                    RokVydani = @RokVydani,
                    StavKnihy = @StavKnihy
                WHERE Id = @Id;
            ";

            command.Parameters.AddWithValue("@Id", kniha.Id);
            command.Parameters.AddWithValue("@Nazev", kniha.Nazev);
            command.Parameters.AddWithValue("@Autor", kniha.Autor);
            command.Parameters.AddWithValue("@ISBN", kniha.ISBN);
            command.Parameters.AddWithValue("@RokVydani", kniha.RokVydani);
            command.Parameters.AddWithValue("@StavKnihy", kniha.StavKnihy);

            command.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var connection = Database.CreateConnection();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                DELETE FROM Knihy
                WHERE Id = @Id;
            ";

            command.Parameters.AddWithValue("@Id", id);

            command.ExecuteNonQuery();
        }

        public bool ExistsByIsbn(string isbn)
        {
            using var connection = Database.CreateConnection();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT COUNT(*)
                FROM Knihy
                WHERE ISBN = @ISBN;
            ";

            command.Parameters.AddWithValue("@ISBN", isbn);

            long count = Convert.ToInt64(command.ExecuteScalar());

            return count > 0;
        }

        public bool ExistsByIsbnExceptId(string isbn, int id)
        {
            using var connection = Database.CreateConnection();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT COUNT(*)
                FROM Knihy
                WHERE ISBN = @ISBN
                  AND Id != @Id;
            ";

            command.Parameters.AddWithValue("@ISBN", isbn);
            command.Parameters.AddWithValue("@Id", id);

            long count = Convert.ToInt64(command.ExecuteScalar());

            return count > 0;
        }

        private Kniha CreateBookFromReader(SqliteDataReader reader)
        {
            int id = reader.GetInt32(0);
            string nazev = reader.GetString(1);
            string autor = reader.GetString(2);
            string isbn = reader.GetString(3);
            int rokVydani = reader.GetInt32(4);
            string stavKnihy = reader.GetString(5);

            Kniha kniha = stavKnihy switch
            {
                "Nová" => new NovaKniha(nazev, autor, isbn, rokVydani),
                "Dobrá" => new DobraKniha(nazev, autor, isbn, rokVydani),
                "Opotřebovaná" => new OpotrebovanaKniha(nazev, autor, isbn, rokVydani),
                _ => new DobraKniha(nazev, autor, isbn, rokVydani)
            };

            kniha.Id = id;

            return kniha;
        }
    }
}