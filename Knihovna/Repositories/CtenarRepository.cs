using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace Knihovna
{
    public class CtenarRepository : ICtenarRepository
    {
        public List<Ctenar> GetAll()
        {
            List<Ctenar> ctenari = new List<Ctenar>();

            using var connection = new SqliteConnection(Database.ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT Id, Jmeno, Prijmeni, TelefonniCislo, Email
                FROM Ctenari
                ORDER BY Prijmeni, Jmeno;
            ";

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                Ctenar ctenar = CreateReaderFromReader(reader);
                ctenari.Add(ctenar);
            }

            return ctenari;
        }

        public Ctenar? GetById(int id)
        {
            using var connection = new SqliteConnection(Database.ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT Id, Jmeno, Prijmeni, TelefonniCislo, Email
                FROM Ctenari
                WHERE Id = @Id;
            ";

            command.Parameters.AddWithValue("@Id", id);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return CreateReaderFromReader(reader);
            }

            return null;
        }

        public void Add(Ctenar ctenar)
        {
            using var connection = new SqliteConnection(Database.ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Ctenari (Jmeno, Prijmeni, TelefonniCislo, Email)
                VALUES (@Jmeno, @Prijmeni, @TelefonniCislo, @Email);
            ";

            command.Parameters.AddWithValue("@Jmeno", ctenar.Jmeno);
            command.Parameters.AddWithValue("@Prijmeni", ctenar.Prijmeni);
            command.Parameters.AddWithValue("@TelefonniCislo", ctenar.TelefonniCislo);
            command.Parameters.AddWithValue("@Email", ctenar.Email);

            command.ExecuteNonQuery();
        }

        public void Update(Ctenar ctenar)
        {
            using var connection = new SqliteConnection(Database.ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                UPDATE Ctenari
                SET Jmeno = @Jmeno,
                    Prijmeni = @Prijmeni,
                    TelefonniCislo = @TelefonniCislo,
                    Email = @Email
                WHERE Id = @Id;
            ";

            command.Parameters.AddWithValue("@Id", ctenar.Id);
            command.Parameters.AddWithValue("@Jmeno", ctenar.Jmeno);
            command.Parameters.AddWithValue("@Prijmeni", ctenar.Prijmeni);
            command.Parameters.AddWithValue("@TelefonniCislo", ctenar.TelefonniCislo);
            command.Parameters.AddWithValue("@Email", ctenar.Email);

            command.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var connection = new SqliteConnection(Database.ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                DELETE FROM Ctenari
                WHERE Id = @Id;
            ";

            command.Parameters.AddWithValue("@Id", id);

            command.ExecuteNonQuery();
        }

        public bool ExistsByEmail(string email)
        {
            using var connection = new SqliteConnection(Database.ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT COUNT(*)
                FROM Ctenari
                WHERE Email = @Email;
            ";

            command.Parameters.AddWithValue("@Email", email);

            long count = (long)command.ExecuteScalar();

            return count > 0;
        }

        private Ctenar CreateReaderFromReader(SqliteDataReader reader)
        {
            int id = reader.GetInt32(0);
            string jmeno = reader.GetString(1);
            string prijmeni = reader.GetString(2);
            string telefonniCislo = reader.GetString(3);
            string email = reader.GetString(4);

            Ctenar ctenar = new Ctenar(jmeno, prijmeni, telefonniCislo, email);
            ctenar.Id = id;

            return ctenar;
        }
    }
}