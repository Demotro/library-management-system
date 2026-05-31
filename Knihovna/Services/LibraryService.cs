using System;
using System.Linq;

namespace Knihovna
{
    public class LibraryService
    {
        private readonly IKnihaRepository _knihaRepository;
        private readonly ICtenarRepository _ctenarRepository;
        private readonly IVypujckaRepository _vypujckaRepository;
        private readonly IRezervaceRepository _rezervaceRepository;

        public LibraryService()
        {
            _knihaRepository = new KnihaRepository();
            _ctenarRepository = new CtenarRepository();
            _vypujckaRepository = new VypujckaRepository();
            _rezervaceRepository = new RezervaceRepository();
        }

        public Result AddBook(Kniha kniha)
        {
            if (kniha == null)
            {
                return Result.Fail("Kniha neexistuje.");
            }

            if (string.IsNullOrWhiteSpace(kniha.Nazev))
            {
                return Result.Fail("Název knihy nesmí být prázdný.");
            }

            if (string.IsNullOrWhiteSpace(kniha.Autor))
            {
                return Result.Fail("Autor knihy nesmí být prázdný.");
            }

            if (string.IsNullOrWhiteSpace(kniha.ISBN))
            {
                return Result.Fail("ISBN nesmí být prázdné.");
            }

            if (_knihaRepository.ExistsByIsbn(kniha.ISBN))
            {
                return Result.Fail("Kniha se stejným ISBN už existuje.");
            }

            if (kniha.RokVydani < 1500 || kniha.RokVydani > DateTime.Now.Year)
            {
                return Result.Fail("Rok vydání není platný.");
            }

            _knihaRepository.Add(kniha);

            return Result.Ok("Kniha byla úspěšně přidána.");
        }

        public Result UpdateBook(Kniha kniha)
        {
            if (kniha == null)
            {
                return Result.Fail("Kniha neexistuje.");
            }

            if (kniha.Id <= 0)
            {
                return Result.Fail("Kniha nemá platné ID.");
            }

            if (string.IsNullOrWhiteSpace(kniha.Nazev))
            {
                return Result.Fail("Název knihy nesmí být prázdný.");
            }

            if (string.IsNullOrWhiteSpace(kniha.Autor))
            {
                return Result.Fail("Autor knihy nesmí být prázdný.");
            }

            if (string.IsNullOrWhiteSpace(kniha.ISBN))
            {
                return Result.Fail("ISBN nesmí být prázdné.");
            }

            if (kniha.RokVydani < 1500 || kniha.RokVydani > DateTime.Now.Year)
            {
                return Result.Fail("Rok vydání není platný.");
            }

            bool isbnPouzivaJinaKniha = _knihaRepository
                .GetAll()
                .Any(k => k.ISBN == kniha.ISBN && k.Id != kniha.Id);

            if (isbnPouzivaJinaKniha)
            {
                return Result.Fail("Toto ISBN už používá jiná kniha.");
            }

            _knihaRepository.Update(kniha);

            return Result.Ok("Kniha byla úspěšně upravena.");
        }

        public Result DeleteBook(int knihaId)
        {
            Kniha? kniha = _knihaRepository.GetById(knihaId);

            if (kniha == null)
            {
                return Result.Fail("Kniha nebyla nalezena.");
            }

            if (_vypujckaRepository.HasActiveLoanForBook(knihaId))
            {
                return Result.Fail("Knihu nelze smazat, protože je aktuálně vypůjčená.");
            }

            if (_rezervaceRepository.HasActiveReservationForBook(knihaId))
            {
                return Result.Fail("Knihu nelze smazat, protože má aktivní rezervace.");
            }

            _knihaRepository.Delete(knihaId);

            return Result.Ok("Kniha byla úspěšně smazána.");
        }

        public Result AddReader(Ctenar ctenar)
        {
            if (ctenar == null)
            {
                return Result.Fail("Čtenář neexistuje.");
            }

            if (string.IsNullOrWhiteSpace(ctenar.Jmeno))
            {
                return Result.Fail("Jméno nesmí být prázdné.");
            }

            if (string.IsNullOrWhiteSpace(ctenar.Prijmeni))
            {
                return Result.Fail("Příjmení nesmí být prázdné.");
            }

            if (string.IsNullOrWhiteSpace(ctenar.Email))
            {
                return Result.Fail("E-mail nesmí být prázdný.");
            }

            if (!ctenar.Email.Contains("@") || !ctenar.Email.Contains("."))
            {
                return Result.Fail("E-mail nemá platný formát.");
            }

            if (string.IsNullOrWhiteSpace(ctenar.TelefonniCislo))
            {
                return Result.Fail("Telefonní číslo nesmí být prázdné.");
            }

            if (!ctenar.TelefonniCislo.All(char.IsDigit) || ctenar.TelefonniCislo.Length != 9)
            {
                return Result.Fail("Telefonní číslo musí mít 9 číslic.");
            }

            if (_ctenarRepository.ExistsByEmail(ctenar.Email))
            {
                return Result.Fail("Čtenář se stejným e-mailem už existuje.");
            }

            _ctenarRepository.Add(ctenar);

            return Result.Ok("Čtenář byl úspěšně přidán.");
        }

        public Result UpdateReader(Ctenar ctenar)
        {
            if (ctenar == null)
            {
                return Result.Fail("Čtenář neexistuje.");
            }

            if (ctenar.Id <= 0)
            {
                return Result.Fail("Čtenář nemá platné ID.");
            }

            if (string.IsNullOrWhiteSpace(ctenar.Jmeno))
            {
                return Result.Fail("Jméno nesmí být prázdné.");
            }

            if (string.IsNullOrWhiteSpace(ctenar.Prijmeni))
            {
                return Result.Fail("Příjmení nesmí být prázdné.");
            }

            if (string.IsNullOrWhiteSpace(ctenar.Email))
            {
                return Result.Fail("E-mail nesmí být prázdný.");
            }

            if (!ctenar.Email.Contains("@") || !ctenar.Email.Contains("."))
            {
                return Result.Fail("E-mail nemá platný formát.");
            }

            if (string.IsNullOrWhiteSpace(ctenar.TelefonniCislo))
            {
                return Result.Fail("Telefonní číslo nesmí být prázdné.");
            }

            if (!ctenar.TelefonniCislo.All(char.IsDigit) || ctenar.TelefonniCislo.Length != 9)
            {
                return Result.Fail("Telefonní číslo musí mít 9 číslic.");
            }

            bool emailPouzivaJinyCtenar = _ctenarRepository
                .GetAll()
                .Any(c => c.Email == ctenar.Email && c.Id != ctenar.Id);

            if (emailPouzivaJinyCtenar)
            {
                return Result.Fail("Tento e-mail už používá jiný čtenář.");
            }

            _ctenarRepository.Update(ctenar);

            return Result.Ok("Čtenář byl úspěšně upraven.");
        }

        public Result DeleteReader(int ctenarId)
        {
            Ctenar? ctenar = _ctenarRepository.GetById(ctenarId);

            if (ctenar == null)
            {
                return Result.Fail("Čtenář nebyl nalezen.");
            }

            if (_vypujckaRepository.HasActiveLoanForReader(ctenarId))
            {
                return Result.Fail("Čtenáře nelze smazat, protože má aktivní výpůjčku.");
            }

            if (_rezervaceRepository.HasActiveReservationForReader(ctenarId))
            {
                return Result.Fail("Čtenáře nelze smazat, protože má aktivní rezervaci.");
            }

            _ctenarRepository.Delete(ctenarId);

            return Result.Ok("Čtenář byl úspěšně smazán.");
        }

        public Result BorrowBook(int knihaId, int ctenarId)
        {
            Kniha? kniha = _knihaRepository.GetById(knihaId);

            if (kniha == null)
            {
                return Result.Fail("Kniha nebyla nalezena.");
            }

            Ctenar? ctenar = _ctenarRepository.GetById(ctenarId);

            if (ctenar == null)
            {
                return Result.Fail("Čtenář nebyl nalezen.");
            }

            if (_vypujckaRepository.HasActiveLoanForBook(knihaId))
            {
                return Result.Fail("Kniha je už vypůjčená.");
            }

            Rezervace? prvniRezervace = _rezervaceRepository.GetFirstActiveReservationByBookId(knihaId);

            if (prvniRezervace != null && prvniRezervace.CtenarId != ctenarId)
            {
                return Result.Fail("Kniha je rezervovaná pro jiného čtenáře.");
            }

            Vypujcka vypujcka = new Vypujcka(knihaId, ctenarId);
            _vypujckaRepository.Add(vypujcka);

            if (prvniRezervace != null && prvniRezervace.CtenarId == ctenarId)
            {
                _rezervaceRepository.MarkAsCompleted(prvniRezervace.Id);
            }

            return Result.Ok("Kniha byla úspěšně vypůjčena.");
        }

        public Result ReturnBook(int knihaId)
        {
            Kniha? kniha = _knihaRepository.GetById(knihaId);

            if (kniha == null)
            {
                return Result.Fail("Kniha nebyla nalezena.");
            }

            Vypujcka? aktivniVypujcka = _vypujckaRepository.GetActiveLoanByBookId(knihaId);

            if (aktivniVypujcka == null)
            {
                return Result.Fail("Kniha není aktuálně vypůjčená.");
            }

            _vypujckaRepository.MarkAsReturned(aktivniVypujcka.Id);

            Rezervace? prvniRezervace = _rezervaceRepository.GetFirstActiveReservationByBookId(knihaId);

            if (prvniRezervace != null)
            {
                return Result.Ok("Kniha byla vrácena. Čeká na prvního čtenáře v pořadníku rezervací.");
            }

            return Result.Ok("Kniha byla úspěšně vrácena.");
        }

        public Result ReserveBook(int knihaId, int ctenarId)
        {
            Kniha? kniha = _knihaRepository.GetById(knihaId);

            if (kniha == null)
            {
                return Result.Fail("Kniha nebyla nalezena.");
            }

            Ctenar? ctenar = _ctenarRepository.GetById(ctenarId);

            if (ctenar == null)
            {
                return Result.Fail("Čtenář nebyl nalezen.");
            }

            Vypujcka? aktivniVypujcka = _vypujckaRepository.GetActiveLoanByBookId(knihaId);

            if (aktivniVypujcka == null)
            {
                return Result.Fail("Dostupnou knihu nelze rezervovat, protože ji lze rovnou půjčit.");
            }

            if (aktivniVypujcka.CtenarId == ctenarId)
            {
                return Result.Fail("Čtenář nemůže rezervovat knihu, kterou má právě vypůjčenou.");
            }

            if (_rezervaceRepository.ExistsActiveReservation(knihaId, ctenarId))
            {
                return Result.Fail("Čtenář už má tuto knihu rezervovanou.");
            }

            Rezervace rezervace = new Rezervace(knihaId, ctenarId);
            _rezervaceRepository.Add(rezervace);

            return Result.Ok("Rezervace byla úspěšně vytvořena.");
        }

        public Result CancelReservation(int rezervaceId)
        {
            Rezervace? rezervace = _rezervaceRepository.GetById(rezervaceId);

            if (rezervace == null)
            {
                return Result.Fail("Rezervace nebyla nalezena.");
            }

            if (rezervace.Stav != "Aktivni")
            {
                return Result.Fail("Zrušit lze pouze aktivní rezervaci.");
            }

            _rezervaceRepository.Cancel(rezervaceId);

            return Result.Ok("Rezervace byla úspěšně zrušena.");
        }
    }
}