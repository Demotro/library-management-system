using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace Knihovna
{
    public class RezervaceRepository : IRezervaceRepository
    {
        public List<Rezervace> GetAll()
        {
            var rezervace = new List<Rezervace>();

            using var connection = Database.CreateConnection();
            using var command = connection.CreateCommand();

            command.CommandText = @"
                SELECT Id, KnihaId, CtenarId, DatumRezervace, Stav
                FROM Rezervace;
            ";

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                rezervace.Add(MapRezervace(reader));
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
                return MapRezervace(reader);
            }

            return null;
        }

        public List<Rezervace> GetActiveReservationsByBookId(int knihaId)
        {
            var rezervace = new List<Rezervace>();

            using var connection = Database.CreateConnection();
            using var command = connection.CreateCommand();

            command.CommandText = @"
                SELECT Id, KnihaId, CtenarId, DatumRezervace, Stav
                FROM Rezervace
                WHERE KnihaId = @KnihaId AND Stav = 'Aktivni'
                ORDER BY DatumRezervace ASC, Id ASC;
            ";

            command.Parameters.AddWithValue("@KnihaId", knihaId);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                rezervace.Add(MapRezervace(reader));
            }

            return rezervace;
        }

        public List<Rezervace> GetReservationsByReaderId(int ctenarId)
        {
            var rezervace = new List<Rezervace>();

            using var connection = Database.CreateConnection();
            using var command = connection.CreateCommand();

            command.CommandText = @"
                SELECT Id, KnihaId, CtenarId, DatumRezervace, Stav
                FROM Rezervace
                WHERE CtenarId = @CtenarId;
            ";

            command.Parameters.AddWithValue("@CtenarId", ctenarId);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                rezervace.Add(MapRezervace(reader));
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
                WHERE KnihaId = @KnihaId AND Stav = 'Aktivni'
                ORDER BY DatumRezervace ASC, Id ASC
                LIMIT 1;
            ";

            command.Parameters.AddWithValue("@KnihaId", knihaId);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return MapRezervace(reader);
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
            command.Parameters.AddWithValue("@DatumRezervace", rezervace.DatumRezervace.ToString("O"));
            command.Parameters.AddWithValue("@Stav", rezervace.Stav);

            long id = Convert.ToInt64(command.ExecuteScalar());
            rezervace.Id = (int)id;
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

        public int CountActiveReservationsForReader(int ctenarId)
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

            return (int)count;
        }

        public void DeleteByBookId(int knihaId)
        {
            using var connection = Database.CreateConnection();
            using var command = connection.CreateCommand();

            command.CommandText = @"
                DELETE FROM Rezervace
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
                DELETE FROM Rezervace
                WHERE CtenarId = @CtenarId;
            ";

            command.Parameters.AddWithValue("@CtenarId", ctenarId);

            command.ExecuteNonQuery();
        }

        private Rezervace MapRezervace(SqliteDataReader reader)
        {
            return new Rezervace
            {
                Id = reader.GetInt32(0),
                KnihaId = reader.GetInt32(1),
                CtenarId = reader.GetInt32(2),
                DatumRezervace = DateTime.Parse(reader.GetString(3)),
                Stav = reader.GetString(4)
            };
        }
    }
}