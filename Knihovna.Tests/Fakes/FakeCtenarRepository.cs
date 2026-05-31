using System.Collections.Generic;
using System.Linq;
using Knihovna;

namespace Knihovna.Tests.Fakes
{
    public class FakeCtenarRepository : ICtenarRepository
    {
        private readonly List<Ctenar> _ctenari = new List<Ctenar>();
        private int _nextId = 1;

        public List<Ctenar> GetAll()
        {
            return _ctenari;
        }

        public Ctenar? GetById(int id)
        {
            return _ctenari.FirstOrDefault(c => c.Id == id);
        }

        public void Add(Ctenar ctenar)
        {
            ctenar.Id = _nextId;
            _nextId++;

            _ctenari.Add(ctenar);
        }

        public void Update(Ctenar ctenar)
        {
            Ctenar? existingReader = GetById(ctenar.Id);

            if (existingReader == null)
            {
                return;
            }

            int index = _ctenari.IndexOf(existingReader);
            _ctenari[index] = ctenar;
        }

        public void Delete(int id)
        {
            Ctenar? ctenar = GetById(id);

            if (ctenar != null)
            {
                _ctenari.Remove(ctenar);
            }
        }

        public bool ExistsByEmail(string email)
        {
            return _ctenari.Any(c => c.Email == email);
        }

        public bool ExistsByEmailExceptId(string email, int id)
        {
            return _ctenari.Any(c => c.Email == email && c.Id != id);
        }
    }
}