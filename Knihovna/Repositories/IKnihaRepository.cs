using System.Collections.Generic;

namespace Knihovna
{
    public interface IKnihaRepository
    {
        List<Kniha> GetAll();

        Kniha? GetById(int id);

        void Add(Kniha kniha);

        void Update(Kniha kniha);

        void Delete(int id);

        bool ExistsByIsbn(string isbn);
    }
}