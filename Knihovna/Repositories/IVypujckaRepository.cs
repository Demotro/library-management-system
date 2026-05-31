using System.Collections.Generic;

namespace Knihovna
{
    public interface IVypujckaRepository
    {
        List<Vypujcka> GetAll();

        List<Vypujcka> GetActiveLoans();

        Vypujcka? GetById(int id);

        Vypujcka? GetActiveLoanByBookId(int knihaId);

        List<Vypujcka> GetLoansByReaderId(int ctenarId);

        void Add(Vypujcka vypujcka);

        void MarkAsReturned(int id);

        bool HasActiveLoanForBook(int knihaId);

        bool HasActiveLoanForReader(int ctenarId);
    }
}