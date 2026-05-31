using System.Collections.Generic;
using System.Linq;

namespace Knihovna.Tests.Fakes
{
    public class FakeVypujckaRepository : IVypujckaRepository
    {
        private readonly List<Vypujcka> _vypujcky = new List<Vypujcka>();
        private int _nextId = 1;

        public List<Vypujcka> GetAll()
        {
            return _vypujcky.ToList();
        }

        public List<Vypujcka> GetActiveLoans()
        {
            return _vypujcky
                .Where(v => v.Stav == "Aktivni")
                .ToList();
        }

        public Vypujcka? GetById(int id)
        {
            return _vypujcky.FirstOrDefault(v => v.Id == id);
        }

        public Vypujcka? GetActiveLoanByBookId(int knihaId)
        {
            return _vypujcky.FirstOrDefault(v =>
                v.KnihaId == knihaId &&
                v.Stav == "Aktivni");
        }

        public List<Vypujcka> GetLoansByReaderId(int ctenarId)
        {
            return _vypujcky
                .Where(v => v.CtenarId == ctenarId)
                .ToList();
        }

        public void Add(Vypujcka vypujcka)
        {
            vypujcka.Id = _nextId++;
            _vypujcky.Add(vypujcka);
        }

        public void MarkAsReturned(int id)
        {
            Vypujcka? vypujcka = GetById(id);

            if (vypujcka != null)
            {
                vypujcka.Stav = "Vracena";
                vypujcka.DatumVraceni = System.DateTime.Now;
            }
        }

        public bool HasActiveLoanForBook(int knihaId)
        {
            return _vypujcky.Any(v =>
                v.KnihaId == knihaId &&
                v.Stav == "Aktivni");
        }

        public bool HasActiveLoanForReader(int ctenarId)
        {
            return _vypujcky.Any(v =>
                v.CtenarId == ctenarId &&
                v.Stav == "Aktivni");
        }

        public int CountActiveLoansForReader(int ctenarId)
        {
            return _vypujcky.Count(v =>
                v.CtenarId == ctenarId &&
                v.Stav == "Aktivni");
        }

        public void DeleteByBookId(int knihaId)
        {
            var items = _vypujcky
                .Where(v => v.KnihaId == knihaId)
                .ToList();

            foreach (var item in items)
            {
                _vypujcky.Remove(item);
            }
        }

        public void DeleteByReaderId(int ctenarId)
        {
            var items = _vypujcky
                .Where(v => v.CtenarId == ctenarId)
                .ToList();

            foreach (var item in items)
            {
                _vypujcky.Remove(item);
            }
        }
    }
}