using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace Knihovna
{
    public class RezervaceRepository : IRezervaceRepository
    {
        public List<Rezervace> GetAll()
        {
            List<Rezervace> rezervace = new List<Rezervace>();

            using var connection = Database.CreateConnection();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT Id, KnihaId, CtenarId, DatumRezervace, Stav
                FROM Rezervace
                ORDER BY DatumRezervace DESC;
            ";

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                Rezervace item = CreateReservationFromReader(reader);
                rezervace.Add(item);
            }

            return rezervace;
        }

        public Rezervace? GetById(int id)
        {
            using var connection = Database.CreateConnection();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT Id, KnihaId, CtenarId, DatumRezervace, Stav
                FROM Rezervace
                WHERE Id = @Id;
            ";

            command.Parameters.AddWithValue("@Id", id);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return CreateReservationFromReader(reader);
            }

            return null;
        }

        public List<Rezervace> GetActiveReservationsByBookId(int knihaId)
        {
            List<Rezervace> rezervace = new List<Rezervace>();

            using var connection = Database.CreateConnection();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT Id, KnihaId, CtenarId, DatumRezervace, Stav
                FROM Rezervace
                WHERE KnihaId = @KnihaId
                  AND Stav = 'Aktivni'
                ORDER BY DatumRezervace ASC;
            ";

            command.Parameters.AddWithValue("@KnihaId", knihaId);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                Rezervace item = CreateReservationFromReader(reader);
                rezervace.Add(item);
            }

            return rezervace;
        }

        public List<Rezervace> GetReservationsByReaderId(int ctenarId)
        {
            List<Rezervace> rezervace = new List<Rezervace>();

            using var connection = Database.CreateConnection();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT Id, KnihaId, CtenarId, DatumRezervace, Stav
                FROM Rezervace
                WHERE CtenarId = @CtenarId
                ORDER BY DatumRezervace DESC;
            ";

            command.Parameters.AddWithValue("@CtenarId", ctenarId);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                Rezervace item = CreateReservationFromReader(reader);
                rezervace.Add(item);
            }

            return rezervace;
        }

        public Rezervace? GetFirstActiveReservationByBookId(int knihaId)
        {
            using var connection = Database.CreateConnection();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT Id, KnihaId, CtenarId, DatumRezervace, Stav
                FROM Rezervace
                WHERE KnihaId = @KnihaId
                  AND Stav = 'Aktivni'
                ORDER BY DatumRezervace ASC
                LIMIT 1;
            ";

            command.Parameters.AddWithValue("@KnihaId", knihaId);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return CreateReservationFromReader(reader);
            }

            return null;
        }

        public void Add(Rezervace rezervace)
        {
            using var connection = Database.CreateConnection();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Rezervace (KnihaId, CtenarId, DatumRezervace, Stav)
                VALUES (@KnihaId, @CtenarId, @DatumRezervace, @Stav);

                SELECT last_insert_rowid();
            ";

            command.Parameters.AddWithValue("@KnihaId", rezervace.KnihaId);
            command.Parameters.AddWithValue("@CtenarId", rezervace.CtenarId);
            command.Parameters.AddWithValue("@DatumRezervace", rezervace.DatumRezervace.ToString("yyyy-MM-dd HH:mm:ss"));
            command.Parameters.AddWithValue("@Stav", rezervace.Stav);

            long newId = Convert.ToInt64(command.ExecuteScalar());
            rezervace.Id = (int)newId;
        }

        public void Cancel(int id)
        {
            using var connection = Database.CreateConnection();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                UPDATE Rezervace
                SET Stav = 'Zrusena'
                WHERE Id = @Id;
            ";

            command.Parameters.AddWithValue("@Id", id);

            command.ExecuteNonQuery();
        }

        public void MarkAsCompleted(int id)
        {
            using var connection = Database.CreateConnection();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                UPDATE Rezervace
                SET Stav = 'Vyrizena'
                WHERE Id = @Id;
            ";

            command.Parameters.AddWithValue("@Id", id);

            command.ExecuteNonQuery();
        }

        public bool ExistsActiveReservation(int knihaId, int ctenarId)
        {
            using var connection = Database.CreateConnection();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT COUNT(*)
                FROM Rezervace
                WHERE KnihaId = @KnihaId
                  AND CtenarId = @CtenarId
                  AND Stav = 'Aktivni';
            ";

            command.Parameters.AddWithValue("@KnihaId", knihaId);
            command.Parameters.AddWithValue("@CtenarId", ctenarId);

            long count = Convert.ToInt64(command.ExecuteScalar());

            return count > 0;
        }

        public bool HasActiveReservationForBook(int knihaId)
        {
            using var connection = Database.CreateConnection();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT COUNT(*)
                FROM Rezervace
                WHERE KnihaId = @KnihaId
                  AND Stav = 'Aktivni';
            ";

            command.Parameters.AddWithValue("@KnihaId", knihaId);

            long count = Convert.ToInt64(command.ExecuteScalar());

            return count > 0;
        }

        public bool HasActiveReservationForReader(int ctenarId)
        {
            using var connection = Database.CreateConnection();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT COUNT(*)
                FROM Rezervace
                WHERE CtenarId = @CtenarId
                  AND Stav = 'Aktivni';
            ";

            command.Parameters.AddWithValue("@CtenarId", ctenarId);

            long count = Convert.ToInt64(command.ExecuteScalar());

            return count > 0;
        }

        private Rezervace CreateReservationFromReader(SqliteDataReader reader)
        {
            Rezervace rezervace = new Rezervace();

            rezervace.Id = reader.GetInt32(0);
            rezervace.KnihaId = reader.GetInt32(1);
            rezervace.CtenarId = reader.GetInt32(2);
            rezervace.DatumRezervace = DateTime.Parse(reader.GetString(3));
            rezervace.Stav = reader.GetString(4);

            return rezervace;
        }
    }
}