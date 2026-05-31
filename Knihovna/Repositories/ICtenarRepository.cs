using System.Collections.Generic;

namespace Knihovna
{
    public interface ICtenarRepository
    {
        List<Ctenar> GetAll();

        Ctenar? GetById(int id);

        void Add(Ctenar ctenar);

        void Update(Ctenar ctenar);

        void Delete(int id);

        bool ExistsByEmail(string email);

        bool ExistsByEmailExceptId(string email, int id);
    }
}