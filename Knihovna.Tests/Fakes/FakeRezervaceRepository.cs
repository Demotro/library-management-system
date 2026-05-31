using System.Collections.Generic;
using System.Linq;

namespace Knihovna.Tests.Fakes
{
    public class FakeRezervaceRepository : IRezervaceRepository
    {
        private readonly List<Rezervace> _rezervace = new List<Rezervace>();
        private int _nextId = 1;

        public List<Rezervace> GetAll()
        {
            return _rezervace.ToList();
        }

        public Rezervace? GetById(int id)
        {
            return _rezervace.FirstOrDefault(r => r.Id == id);
        }

        public List<Rezervace> GetActiveReservationsByBookId(int knihaId)
        {
            return _rezervace
                .Where(r => r.KnihaId == knihaId && r.Stav == "Aktivni")
                .OrderBy(r => r.DatumRezervace)
                .ThenBy(r => r.Id)
                .ToList();
        }

        public List<Rezervace> GetReservationsByReaderId(int ctenarId)
        {
            return _rezervace
                .Where(r => r.CtenarId == ctenarId)
                .ToList();
        }

        public Rezervace? GetFirstActiveReservationByBookId(int knihaId)
        {
            return _rezervace
                .Where(r => r.KnihaId == knihaId && r.Stav == "Aktivni")
                .OrderBy(r => r.DatumRezervace)
                .ThenBy(r => r.Id)
                .FirstOrDefault();
        }

        public void Add(Rezervace rezervace)
        {
            rezervace.Id = _nextId++;
            _rezervace.Add(rezervace);
        }

        public void Cancel(int id)
        {
            Rezervace? rezervace = GetById(id);

            if (rezervace != null)
            {
                rezervace.Stav = "Zrusena";
            }
        }

        public void MarkAsCompleted(int id)
        {
            Rezervace? rezervace = GetById(id);

            if (rezervace != null)
            {
                rezervace.Stav = "Vyrizena";
            }
        }

        public bool ExistsActiveReservation(int knihaId, int ctenarId)
        {
            return _rezervace.Any(r =>
                r.KnihaId == knihaId &&
                r.CtenarId == ctenarId &&
                r.Stav == "Aktivni");
        }

        public bool HasActiveReservationForBook(int knihaId)
        {
            return _rezervace.Any(r =>
                r.KnihaId == knihaId &&
                r.Stav == "Aktivni");
        }

        public bool HasActiveReservationForReader(int ctenarId)
        {
            return _rezervace.Any(r =>
                r.CtenarId == ctenarId &&
                r.Stav == "Aktivni");
        }

        public int CountActiveReservationsForReader(int ctenarId)
        {
            return _rezervace.Count(r =>
                r.CtenarId == ctenarId &&
                r.Stav == "Aktivni");
        }

        public void DeleteByBookId(int knihaId)
        {
            var items = _rezervace
                .Where(r => r.KnihaId == knihaId)
                .ToList();

            foreach (var item in items)
            {
                _rezervace.Remove(item);
            }
        }

        public void DeleteByReaderId(int ctenarId)
        {
            var items = _rezervace
                .Where(r => r.CtenarId == ctenarId)
                .ToList();

            foreach (var item in items)
            {
                _rezervace.Remove(item);
            }
        }
    }
}