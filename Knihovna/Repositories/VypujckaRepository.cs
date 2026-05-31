using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace Knihovna
{
    public class VypujckaRepository : IVypujckaRepository
    {
        public List<Vypujcka> GetAll()
        {
            var vypujcky = new List<Vypujcka>();

            using var connection = Database.CreateConnection();
            using var command = connection.CreateCommand();

            command.CommandText = @"
                SELECT Id, KnihaId, CtenarId, DatumVypujceni, DatumVraceni, Stav
                FROM Vypujcky;
            ";

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                vypujcky.Add(MapVypujcka(reader));
            }

            return vypujcky;
        }

        public List<Vypujcka> GetActiveLoans()
        {
            var vypujcky = new List<Vypujcka>();

            using var connection = Database.CreateConnection();
            using var command = connection.CreateCommand();

            command.CommandText = @"
                SELECT Id, KnihaId, CtenarId, DatumVypujceni, DatumVraceni, Stav
                FROM Vypujcky
                WHERE Stav = 'Aktivni';
            ";

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                vypujcky.Add(MapVypujcka(reader));
            }

            return vypujcky;
        }

        public Vypujcka? GetById(int id)
        {
            using var connection = Database.CreateConnection();
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
                return MapVypujcka(reader);
            }

            return null;
        }

        public Vypujcka? GetActiveLoanByBookId(int knihaId)
        {
            using var connection = Database.CreateConnection();
            using var command = connection.CreateCommand();

            command.CommandText = @"
                SELECT Id, KnihaId, CtenarId, DatumVypujceni, DatumVraceni, Stav
                FROM Vypujcky
                WHERE KnihaId = @KnihaId AND Stav = 'Aktivni'
                LIMIT 1;
            ";

            command.Parameters.AddWithValue("@KnihaId", knihaId);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return MapVypujcka(reader);
            }

            return null;
        }

        public List<Vypujcka> GetLoansByReaderId(int ctenarId)
        {
            var vypujcky = new List<Vypujcka>();

            using var connection = Database.CreateConnection();
            using var command = connection.CreateCommand();

            command.CommandText = @"
                SELECT Id, KnihaId, CtenarId, DatumVypujceni, DatumVraceni, Stav
                FROM Vypujcky
                WHERE CtenarId = @CtenarId;
            ";

            command.Parameters.AddWithValue("@CtenarId", ctenarId);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                vypujcky.Add(MapVypujcka(reader));
            }

            return vypujcky;
        }

        public void Add(Vypujcka vypujcka)
        {
            using var connection = Database.CreateConnection();
            using var command = connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO Vypujcky (KnihaId, CtenarId, DatumVypujceni, DatumVraceni, Stav)
                VALUES (@KnihaId, @CtenarId, @DatumVypujceni, @DatumVraceni, @Stav);

                SELECT last_insert_rowid();
            ";

            command.Parameters.AddWithValue("@KnihaId", vypujcka.KnihaId);
            command.Parameters.AddWithValue("@CtenarId", vypujcka.CtenarId);
            command.Parameters.AddWithValue("@DatumVypujceni", vypujcka.DatumVypujceni.ToString("O"));
            command.Parameters.AddWithValue("@DatumVraceni", DBNull.Value);
            command.Parameters.AddWithValue("@Stav", vypujcka.Stav);

            long id = Convert.ToInt64(command.ExecuteScalar());
            vypujcka.Id = (int)id;
        }

        public void MarkAsReturned(int id)
        {
            using var connection = Database.CreateConnection();
            using var command = connection.CreateCommand();

            command.CommandText = @"
                UPDATE Vypujcky
                SET DatumVraceni = @DatumVraceni,
                    Stav = 'Vracena'
                WHERE Id = @Id;
            ";

            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@DatumVraceni", DateTime.Now.ToString("O"));

            command.ExecuteNonQuery();
        }

        public bool HasActiveLoanForBook(int knihaId)
        {
            using var connection = Database.CreateConnection();
            using var command = connection.CreateCommand();

            command.CommandText = @"
                SELECT COUNT(*)
                FROM Vypujcky
                WHERE KnihaId = @KnihaId AND Stav = 'Aktivni';
            ";

            command.Parameters.AddWithValue("@KnihaId", knihaId);

            long count = Convert.ToInt64(command.ExecuteScalar());

            return count > 0;
        }

        public bool HasActiveLoanForReader(int ctenarId)
        {
            using var connection = Database.CreateConnection();
            using var command = connection.CreateCommand();

            command.CommandText = @"
                SELECT COUNT(*)
                FROM Vypujcky
                WHERE CtenarId = @CtenarId AND Stav = 'Aktivni';
            ";

            command.Parameters.AddWithValue("@CtenarId", ctenarId);

            long count = Convert.ToInt64(command.ExecuteScalar());

            return count > 0;
        }

        public void DeleteByBookId(int knihaId)
        {
            using var connection = Database.CreateConnection();
            using var command = connection.CreateCommand();

            command.CommandText = @"
                DELETE FROM Vypujcky
                WHERE KnihaId = @KnihaId;
            ";

            command.Parameters.AddWithValue("@KnihaId", knihaId);

            command.ExecuteNonQuery();
        }

        public void DeleteByReaderId(int ctenarId)
        {
            using var connection = Database.CreateConnection();
            using var command = connection.CreateCommand();

            command.CommandText = @"
                DELETE FROM Vypujcky
                WHERE CtenarId = @CtenarId;
            ";

            command.Parameters.AddWithValue("@CtenarId", ctenarId);

            command.ExecuteNonQuery();
        }

        private Vypujcka MapVypujcka(SqliteDataReader reader)
        {
            return new Vypujcka
            {
                Id = reader.GetInt32(0),
                KnihaId = reader.GetInt32(1),
                CtenarId = reader.GetInt32(2),
                DatumVypujceni = DateTime.Parse(reader.GetString(3)),
                DatumVraceni = reader.IsDBNull(4) ? null : DateTime.Parse(reader.GetString(4)),
                Stav = reader.GetString(5)
            };
        }
    }
}