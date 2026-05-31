using System.Collections.Generic;
using System.Linq;
using Knihovna;

namespace Knihovna.Tests.Fakes
{
    public class FakeKnihaRepository : IKnihaRepository
    {
        private readonly List<Kniha> _knihy = new List<Kniha>();
        private int _nextId = 1;

        public List<Kniha> GetAll()
        {
            return _knihy;
        }

        public Kniha? GetById(int id)
        {
            return _knihy.FirstOrDefault(k => k.Id == id);
        }

        public void Add(Kniha kniha)
        {
            kniha.Id = _nextId;
            _nextId++;

            _knihy.Add(kniha);
        }

        public void Update(Kniha kniha)
        {
            Kniha? existingBook = GetById(kniha.Id);

            if (existingBook == null)
            {
                return;
            }

            int index = _knihy.IndexOf(existingBook);
            _knihy[index] = kniha;
        }

        public void Delete(int id)
        {
            Kniha? kniha = GetById(id);

            if (kniha != null)
            {
                _knihy.Remove(kniha);
            }
        }

        public bool ExistsByIsbn(string isbn)
        {
            return _knihy.Any(k => k.ISBN == isbn);
        }

        public bool ExistsByIsbnExceptId(string isbn, int id)
        {
            return _knihy.Any(k => k.ISBN == isbn && k.Id != id);
        }
    }
}