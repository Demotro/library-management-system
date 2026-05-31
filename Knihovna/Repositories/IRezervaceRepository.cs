using System.Collections.Generic;

namespace Knihovna
{
    public interface IRezervaceRepository
    {
        List<Rezervace> GetAll();
        Rezervace? GetById(int id);
        List<Rezervace> GetActiveReservationsByBookId(int knihaId);
        List<Rezervace> GetReservationsByReaderId(int ctenarId);
        Rezervace? GetFirstActiveReservationByBookId(int knihaId);

        void Add(Rezervace rezervace);
        void Cancel(int id);
        void MarkAsCompleted(int id);

        bool ExistsActiveReservation(int knihaId, int ctenarId);
        bool HasActiveReservationForBook(int knihaId);
        bool HasActiveReservationForReader(int ctenarId);
        int CountActiveReservationsForReader(int ctenarId);

        void DeleteByBookId(int knihaId);
        void DeleteByReaderId(int ctenarId);
    }
}