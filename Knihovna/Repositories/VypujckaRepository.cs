using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace Knihovna
{
    public class VypujckaRepository : IVypujckaRepository
    {
        public List<Vypujcka> GetAll()
        {
            List<Vypujcka> vypujcky = new List<Vypujcka>();

            using var connection = new SqliteConnection(Database.ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT Id, KnihaId, CtenarId, DatumVypujceni, DatumVraceni, Stav
                FROM Vypujcky
                ORDER BY DatumVypujceni DESC;
            ";

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                Vypujcka vypujcka = CreateLoanFromReader(reader);
                vypujcky.Add(vypujcka);
            }

            return vypujcky;
        }

        public List<Vypujcka> GetActiveLoans()
        {
            List<Vypujcka> vypujcky = new List<Vypujcka>();

            using var connection = new SqliteConnection(Database.ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT Id, KnihaId, CtenarId, DatumVypujceni, DatumVraceni, Stav
                FROM Vypujcky
                WHERE Stav = 'Aktivni'
                ORDER BY DatumVypujceni DESC;
            ";

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                Vypujcka vypujcka = CreateLoanFromReader(reader);
                vypujcky.Add(vypujcka);
            }

            return vypujcky;
        }

        public Vypujcka? GetById(int id)
        {
            using var connection = new SqliteConnection(Database.ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT Id, KnihaId, CtenarId, DatumVypujceni, DatumVraceni, Stav
                FROM Vypujcky
                WHERE Id = @Id;
            ";

            command.Parameters.AddWithValue("@Id", id);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return CreateLoanFromReader(reader);
            }

            return null;
        }

        public Vypujcka? GetActiveLoanByBookId(int knihaId)
        {
            using var connection = new SqliteConnection(Database.ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT Id, KnihaId, CtenarId, DatumVypujceni, DatumVraceni, Stav
                FROM Vypujcky
                WHERE KnihaId = @KnihaId
                  AND Stav = 'Aktivni'
                LIMIT 1;
            ";

            command.Parameters.AddWithValue("@KnihaId", knihaId);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return CreateLoanFromReader(reader);
            }

            return null;
        }

        public List<Vypujcka> GetLoansByReaderId(int ctenarId)
        {
            List<Vypujcka> vypujcky = new List<Vypujcka>();

            using var connection = new SqliteConnection(Database.ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT Id, KnihaId, CtenarId, DatumVypujceni, DatumVraceni, Stav
                FROM Vypujcky
                WHERE CtenarId = @CtenarId
                ORDER BY DatumVypujceni DESC;
            ";

            command.Parameters.AddWithValue("@CtenarId", ctenarId);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                Vypujcka vypujcka = CreateLoanFromReader(reader);
                vypujcky.Add(vypujcka);
            }

            return vypujcky;
        }

        public void Add(Vypujcka vypujcka)
        {
            using var connection = new SqliteConnection(Database.ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Vypujcky (KnihaId, CtenarId, DatumVypujceni, DatumVraceni, Stav)
                VALUES (@KnihaId, @CtenarId, @DatumVypujceni, @DatumVraceni, @Stav);
            ";

            command.Parameters.AddWithValue("@KnihaId", vypujcka.KnihaId);
            command.Parameters.AddWithValue("@CtenarId", vypujcka.CtenarId);
            command.Parameters.AddWithValue("@DatumVypujceni", vypujcka.DatumVypujceni.ToString("yyyy-MM-dd HH:mm:ss"));
            command.Parameters.AddWithValue("@DatumVraceni", DBNull.Value);
            command.Parameters.AddWithValue("@Stav", vypujcka.Stav);

            command.ExecuteNonQuery();
        }

        public void MarkAsReturned(int id)
        {
            using var connection = new SqliteConnection(Database.ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                UPDATE Vypujcky
                SET DatumVraceni = @DatumVraceni,
                    Stav = 'Vracena'
                WHERE Id = @Id;
            ";

            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@DatumVraceni", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            command.ExecuteNonQuery();
        }

        public bool HasActiveLoanForBook(int knihaId)
        {
            using var connection = new SqliteConnection(Database.ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT COUNT(*)
                FROM Vypujcky
                WHERE KnihaId = @KnihaId
                  AND Stav = 'Aktivni';
            ";

            command.Parameters.AddWithValue("@KnihaId", knihaId);

            long count = (long)command.ExecuteScalar();

            return count > 0;
        }

        public bool HasActiveLoanForReader(int ctenarId)
        {
            using var connection = new SqliteConnection(Database.ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT COUNT(*)
                FROM Vypujcky
                WHERE CtenarId = @CtenarId
                  AND Stav = 'Aktivni';
            ";

            command.Parameters.AddWithValue("@CtenarId", ctenarId);

            long count = (long)command.ExecuteScalar();

            return count > 0;
        }

        private Vypujcka CreateLoanFromReader(SqliteDataReader reader)
        {
            Vypujcka vypujcka = new Vypujcka();

            vypujcka.Id = reader.GetInt32(0);
            vypujcka.KnihaId = reader.GetInt32(1);
            vypujcka.CtenarId = reader.GetInt32(2);
            vypujcka.DatumVypujceni = DateTime.Parse(reader.GetString(3));

            if (reader.IsDBNull(4))
            {
                vypujcka.DatumVraceni = null;
            }
            else
            {
                vypujcka.DatumVraceni = DateTime.Parse(reader.GetString(4));
            }

            vypujcka.Stav = reader.GetString(5);

            return vypujcka;
        }
    }
}