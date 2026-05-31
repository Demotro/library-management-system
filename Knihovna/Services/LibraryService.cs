using System;
using System.Linq;
using System.Net.Mail;

namespace Knihovna
{
    public class LibraryService
    {
        private readonly IKnihaRepository _knihaRepository;
        private readonly ICtenarRepository _ctenarRepository;
        private readonly IVypujckaRepository _vypujckaRepository;
        private readonly IRezervaceRepository _rezervaceRepository;

        public LibraryService()
            : this(
                  new KnihaRepository(),
                  new CtenarRepository(),
                  new VypujckaRepository(),
                  new RezervaceRepository())
        {
        }

        public LibraryService(
            IKnihaRepository knihaRepository,
            ICtenarRepository ctenarRepository,
            IVypujckaRepository vypujckaRepository,
            IRezervaceRepository rezervaceRepository)
        {
            _knihaRepository = knihaRepository;
            _ctenarRepository = ctenarRepository;
            _vypujckaRepository = vypujckaRepository;
            _rezervaceRepository = rezervaceRepository;
        }

        public Result AddBook(Kniha kniha)
        {
            if (string.IsNullOrWhiteSpace(kniha.Nazev))
            {
                return Result.Fail("Název knihy je povinný.");
            }

            if (string.IsNullOrWhiteSpace(kniha.Autor))
            {
                return Result.Fail("Autor knihy je povinný.");
            }

            kniha.Nazev = kniha.Nazev.Trim();
            kniha.Autor = kniha.Autor.Trim();

            if (!IsValidPublicationYear(kniha.RokVydani))
            {
                return Result.Fail("Rok vydání musí být mezi 1450 a aktuálním rokem.");
            }

            if (string.IsNullOrWhiteSpace(kniha.ISBN))
            {
                return Result.Fail("ISBN je povinné.");
            }

            if (!IsValidIsbn(kniha.ISBN))
            {
                return Result.Fail("ISBN musí mít 10 nebo 13 číslic.");
            }

            kniha.ISBN = NormalizeIsbn(kniha.ISBN);

            if (_knihaRepository.ExistsByIsbn(kniha.ISBN))
            {
                return Result.Fail("Kniha s tímto ISBN už existuje.");
            }

            _knihaRepository.Add(kniha);

            return Result.Ok("Kniha byla úspěšně přidána.");
        }

        public Result UpdateBook(Kniha kniha)
        {
            Kniha? existingBook = _knihaRepository.GetById(kniha.Id);

            if (existingBook == null)
            {
                return Result.Fail("Kniha nebyla nalezena.");
            }

            if (string.IsNullOrWhiteSpace(kniha.Nazev))
            {
                return Result.Fail("Název knihy je povinný.");
            }

            if (string.IsNullOrWhiteSpace(kniha.Autor))
            {
                return Result.Fail("Autor knihy je povinný.");
            }

            kniha.Nazev = kniha.Nazev.Trim();
            kniha.Autor = kniha.Autor.Trim();

            if (!IsValidPublicationYear(kniha.RokVydani))
            {
                return Result.Fail("Rok vydání musí být mezi 1450 a aktuálním rokem.");
            }

            if (string.IsNullOrWhiteSpace(kniha.ISBN))
            {
                return Result.Fail("ISBN je povinné.");
            }

            if (!IsValidIsbn(kniha.ISBN))
            {
                return Result.Fail("ISBN musí mít 10 nebo 13 číslic.");
            }

            kniha.ISBN = NormalizeIsbn(kniha.ISBN);

            if (_knihaRepository.ExistsByIsbnExceptId(kniha.ISBN, kniha.Id))
            {
                return Result.Fail("Jiná kniha s tímto ISBN už existuje.");
            }

            if (_vypujckaRepository.HasActiveLoanForBook(kniha.Id))
            {
                return Result.Fail("Knihu nelze upravit, protože je aktuálně vypůjčená.");
            }

            if (_rezervaceRepository.HasActiveReservationForBook(kniha.Id))
            {
                return Result.Fail("Knihu nelze upravit, protože má aktivní rezervace.");
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

            _rezervaceRepository.DeleteByBookId(knihaId);
            _vypujckaRepository.DeleteByBookId(knihaId);
            _knihaRepository.Delete(knihaId);

            return Result.Ok("Kniha byla úspěšně smazána.");
        }

        public Result AddReader(Ctenar ctenar)
        {
            if (string.IsNullOrWhiteSpace(ctenar.Jmeno))
            {
                return Result.Fail("Jméno čtenáře je povinné.");
            }

            if (string.IsNullOrWhiteSpace(ctenar.Prijmeni))
            {
                return Result.Fail("Příjmení čtenáře je povinné.");
            }

            ctenar.Jmeno = ctenar.Jmeno.Trim();
            ctenar.Prijmeni = ctenar.Prijmeni.Trim();

            if (string.IsNullOrWhiteSpace(ctenar.Email))
            {
                return Result.Fail("E-mail čtenáře je povinný.");
            }

            if (!IsValidEmail(ctenar.Email))
            {
                return Result.Fail("E-mail musí být ve správném formátu.");
            }

            ctenar.Email = NormalizeEmail(ctenar.Email);

            if (string.IsNullOrWhiteSpace(ctenar.TelefonniCislo))
            {
                return Result.Fail("Telefonní číslo je povinné.");
            }

            ctenar.TelefonniCislo = NormalizePhoneNumber(ctenar.TelefonniCislo);

            if (!IsValidPhoneNumber(ctenar.TelefonniCislo))
            {
                return Result.Fail("Telefonní číslo musí mít 9 číslic.");
            }

            if (_ctenarRepository.ExistsByEmail(ctenar.Email))
            {
                return Result.Fail("Čtenář s tímto e-mailem už existuje.");
            }

            _ctenarRepository.Add(ctenar);

            return Result.Ok("Čtenář byl úspěšně přidán.");
        }

        public Result UpdateReader(Ctenar ctenar)
        {
            Ctenar? existingReader = _ctenarRepository.GetById(ctenar.Id);

            if (existingReader == null)
            {
                return Result.Fail("Čtenář nebyl nalezen.");
            }

            if (string.IsNullOrWhiteSpace(ctenar.Jmeno))
            {
                return Result.Fail("Jméno čtenáře je povinné.");
            }

            if (string.IsNullOrWhiteSpace(ctenar.Prijmeni))
            {
                return Result.Fail("Příjmení čtenáře je povinné.");
            }

            ctenar.Jmeno = ctenar.Jmeno.Trim();
            ctenar.Prijmeni = ctenar.Prijmeni.Trim();

            if (string.IsNullOrWhiteSpace(ctenar.Email))
            {
                return Result.Fail("E-mail čtenáře je povinný.");
            }

            if (!IsValidEmail(ctenar.Email))
            {
                return Result.Fail("E-mail musí být ve správném formátu.");
            }

            ctenar.Email = NormalizeEmail(ctenar.Email);

            if (string.IsNullOrWhiteSpace(ctenar.TelefonniCislo))
            {
                return Result.Fail("Telefonní číslo je povinné.");
            }

            ctenar.TelefonniCislo = NormalizePhoneNumber(ctenar.TelefonniCislo);

            if (!IsValidPhoneNumber(ctenar.TelefonniCislo))
            {
                return Result.Fail("Telefonní číslo musí mít 9 číslic.");
            }

            if (_ctenarRepository.ExistsByEmailExceptId(ctenar.Email, ctenar.Id))
            {
                return Result.Fail("Jiný čtenář s tímto e-mailem už existuje.");
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

            _rezervaceRepository.DeleteByReaderId(ctenarId);
            _vypujckaRepository.DeleteByReaderId(ctenarId);
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

            var vypujcka = new Vypujcka
            {
                KnihaId = knihaId,
                CtenarId = ctenarId,
                DatumVypujceni = DateTime.Now,
                DatumVraceni = null,
                Stav = "Aktivni"
            };

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

            Vypujcka? activeLoan = _vypujckaRepository.GetActiveLoanByBookId(knihaId);

            if (activeLoan == null)
            {
                return Result.Fail("Kniha není aktuálně vypůjčená.");
            }

            _vypujckaRepository.MarkAsReturned(activeLoan.Id);

            Rezervace? firstReservation = _rezervaceRepository.GetFirstActiveReservationByBookId(knihaId);

            if (firstReservation != null)
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

            if (!_vypujckaRepository.HasActiveLoanForBook(knihaId))
            {
                return Result.Fail("Dostupnou knihu nelze rezervovat, protože ji lze rovnou půjčit.");
            }

            Vypujcka? activeLoan = _vypujckaRepository.GetActiveLoanByBookId(knihaId);

            if (activeLoan != null && activeLoan.CtenarId == ctenarId)
            {
                return Result.Fail("Čtenář nemůže rezervovat knihu, kterou má právě vypůjčenou.");
            }

            if (_rezervaceRepository.ExistsActiveReservation(knihaId, ctenarId))
            {
                return Result.Fail("Čtenář už má tuto knihu rezervovanou.");
            }

            var rezervace = new Rezervace
            {
                KnihaId = knihaId,
                CtenarId = ctenarId,
                DatumRezervace = DateTime.Now,
                Stav = "Aktivni"
            };

            _rezervaceRepository.Add(rezervace);

            return Result.Ok("Kniha byla úspěšně rezervována.");
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
                return Result.Fail("Rezervace už není aktivní.");
            }

            _rezervaceRepository.Cancel(rezervaceId);

            return Result.Ok("Rezervace byla zrušena.");
        }

        private bool IsValidIsbn(string isbn)
        {
            string normalizedIsbn = NormalizeIsbn(isbn);

            return normalizedIsbn.All(char.IsDigit) &&
                   (normalizedIsbn.Length == 10 || normalizedIsbn.Length == 13);
        }

        private string NormalizeIsbn(string isbn)
        {
            return isbn.Replace("-", "").Replace(" ", "");
        }

        private bool IsValidPublicationYear(int year)
        {
            return year >= 1450 && year <= DateTime.Now.Year;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                string normalizedEmail = email.Trim();

                var mailAddress = new MailAddress(normalizedEmail);

                return mailAddress.Address == normalizedEmail &&
                       normalizedEmail.Count(c => c == '@') == 1 &&
                       mailAddress.Host.Contains('.') &&
                       !mailAddress.Host.StartsWith(".") &&
                       !mailAddress.Host.EndsWith(".");
            }
            catch
            {
                return false;
            }
        }

        private string NormalizeEmail(string email)
        {
            return email.Trim().ToLower();
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return phoneNumber.All(char.IsDigit) && phoneNumber.Length == 9;
        }

        private string NormalizePhoneNumber(string phoneNumber)
        {
            return phoneNumber.Replace(" ", "").Replace("-", "");
        }
    }
}