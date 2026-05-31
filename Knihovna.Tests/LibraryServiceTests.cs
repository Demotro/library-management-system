using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Knihovna.Tests.Fakes;

namespace Knihovna.Tests
{
    [TestClass]
    public class LibraryServiceTests
    {
        private FakeKnihaRepository _knihaRepository = null!;
        private FakeCtenarRepository _ctenarRepository = null!;
        private FakeVypujckaRepository _vypujckaRepository = null!;
        private FakeRezervaceRepository _rezervaceRepository = null!;
        private LibraryService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            _knihaRepository = new FakeKnihaRepository();
            _ctenarRepository = new FakeCtenarRepository();
            _vypujckaRepository = new FakeVypujckaRepository();
            _rezervaceRepository = new FakeRezervaceRepository();

            _service = new LibraryService(
                _knihaRepository,
                _ctenarRepository,
                _vypujckaRepository,
                _rezervaceRepository
            );
        }

        [TestMethod]
        public void AddBook_WhenDataAreValid_ShouldSucceed()
        {
            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);

            Result result = _service.AddBook(kniha);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Kniha byla úspěšně přidána.", result.Message);
            Assert.IsNotNull(_knihaRepository.GetById(kniha.Id));
        }

        [TestMethod]
        public void AddBook_WhenTitleAndAuthorContainSpaces_ShouldTrimValues()
        {
            var kniha = new DobraKniha("  Test Book  ", "  Test Author  ", "1234567890", 2020);

            Result result = _service.AddBook(kniha);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Kniha byla úspěšně přidána.", result.Message);
            Assert.AreEqual("Test Book", kniha.Nazev);
            Assert.AreEqual("Test Author", kniha.Autor);
        }

        [TestMethod]
        public void AddBook_WhenTitleIsTooLong_ShouldFail()
        {
            string longTitle = new string('A', 101);
            var kniha = new DobraKniha(longTitle, "Test Author", "1234567890", 2020);

            Result result = _service.AddBook(kniha);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Název knihy může mít maximálně 100 znaků.", result.Message);
        }

        [TestMethod]
        public void AddBook_WhenTitleHasMaximumLength_ShouldSucceed()
        {
            string title = new string('A', 100);
            var kniha = new DobraKniha(title, "Test Author", "1234567890", 2020);

            Result result = _service.AddBook(kniha);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Kniha byla úspěšně přidána.", result.Message);
            Assert.AreEqual(100, kniha.Nazev.Length);
        }

        [TestMethod]
        public void AddBook_WhenAuthorIsTooLong_ShouldFail()
        {
            string longAuthor = new string('A', 101);
            var kniha = new DobraKniha("Test Book", longAuthor, "1234567890", 2020);

            Result result = _service.AddBook(kniha);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Autor knihy může mít maximálně 100 znaků.", result.Message);
        }

        [TestMethod]
        public void AddBook_WhenAuthorHasMaximumLength_ShouldSucceed()
        {
            string author = new string('A', 100);
            var kniha = new DobraKniha("Test Book", author, "1234567890", 2020);

            Result result = _service.AddBook(kniha);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Kniha byla úspěšně přidána.", result.Message);
            Assert.AreEqual(100, kniha.Autor.Length);
        }

        [TestMethod]
        public void AddBook_WhenPublicationYearIsTooLow_ShouldFail()
        {
            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 1449);

            Result result = _service.AddBook(kniha);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Rok vydání musí být mezi 1450 a aktuálním rokem.", result.Message);
        }

        [TestMethod]
        public void AddBook_WhenPublicationYearIsInFuture_ShouldFail()
        {
            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", DateTime.Now.Year + 1);

            Result result = _service.AddBook(kniha);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Rok vydání musí být mezi 1450 a aktuálním rokem.", result.Message);
        }

        [TestMethod]
        public void AddBook_WhenIsbnAlreadyExists_ShouldFail()
        {
            var kniha1 = new DobraKniha("Book One", "Author One", "1234567890", 2020);
            var kniha2 = new DobraKniha("Book Two", "Author Two", "1234567890", 2021);

            _knihaRepository.Add(kniha1);

            Result result = _service.AddBook(kniha2);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Kniha s tímto ISBN už existuje.", result.Message);
        }

        [TestMethod]
        public void AddBook_WhenIsbnContainsHyphens_ShouldSaveNormalizedIsbn()
        {
            var kniha = new DobraKniha("Test Book", "Test Author", "123-456-7890", 2020);

            Result result = _service.AddBook(kniha);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("1234567890", kniha.ISBN);
        }

        [TestMethod]
        public void AddBook_WhenSameIsbnHasDifferentFormat_ShouldFail()
        {
            var kniha1 = new DobraKniha("Book One", "Author One", "1234567890", 2020);
            var kniha2 = new DobraKniha("Book Two", "Author Two", "123-456-7890", 2021);

            _service.AddBook(kniha1);

            Result result = _service.AddBook(kniha2);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Kniha s tímto ISBN už existuje.", result.Message);
        }

        [TestMethod]
        public void AddBook_WhenIsbnHasInvalidFormat_ShouldFail()
        {
            var kniha = new DobraKniha("Test Book", "Test Author", "12345", 2020);

            Result result = _service.AddBook(kniha);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("ISBN musí mít 10 nebo 13 číslic.", result.Message);
        }

        [TestMethod]
        public void UpdateBook_WhenBookDoesNotExist_ShouldFail()
        {
            var kniha = new DobraKniha("Missing Book", "Missing Author", "1234567890", 2020);
            kniha.Id = 999;

            Result result = _service.UpdateBook(kniha);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Kniha nebyla nalezena.", result.Message);
        }

        [TestMethod]
        public void UpdateBook_WhenIsbnHasInvalidFormat_ShouldFail()
        {
            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            _knihaRepository.Add(kniha);

            var upravenaKniha = new DobraKniha("Test Book Updated", "Test Author", "abc123", 2021);
            upravenaKniha.Id = kniha.Id;

            Result result = _service.UpdateBook(upravenaKniha);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("ISBN musí mít 10 nebo 13 číslic.", result.Message);
        }

        [TestMethod]
        public void UpdateBook_WhenIsbnContainsHyphens_ShouldSaveNormalizedIsbn()
        {
            var kniha = new DobraKniha("Old Book", "Old Author", "1234567890", 2020);
            _knihaRepository.Add(kniha);

            var upravenaKniha = new DobraKniha("Updated Book", "Updated Author", "123-456-7890", 2021);
            upravenaKniha.Id = kniha.Id;

            Result result = _service.UpdateBook(upravenaKniha);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Kniha byla úspěšně upravena.", result.Message);
            Assert.AreEqual("1234567890", upravenaKniha.ISBN);
        }

        [TestMethod]
        public void UpdateBook_WhenSameIsbnHasDifferentFormat_ShouldFail()
        {
            var kniha1 = new DobraKniha("Book One", "Author One", "1234567890", 2020);
            var kniha2 = new DobraKniha("Book Two", "Author Two", "0987654321", 2021);

            _knihaRepository.Add(kniha1);
            _knihaRepository.Add(kniha2);

            var upravenaKniha = new DobraKniha("Book Two Updated", "Author Two", "123-456-7890", 2021);
            upravenaKniha.Id = kniha2.Id;

            Result result = _service.UpdateBook(upravenaKniha);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Jiná kniha s tímto ISBN už existuje.", result.Message);
        }

        [TestMethod]
        public void UpdateBook_WhenTitleAndAuthorContainSpaces_ShouldTrimValues()
        {
            var kniha = new DobraKniha("Old Book", "Old Author", "1234567890", 2020);
            _knihaRepository.Add(kniha);

            var upravenaKniha = new DobraKniha("  Updated Book  ", "  Updated Author  ", "1234567890", 2021);
            upravenaKniha.Id = kniha.Id;

            Result result = _service.UpdateBook(upravenaKniha);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Kniha byla úspěšně upravena.", result.Message);
            Assert.AreEqual("Updated Book", upravenaKniha.Nazev);
            Assert.AreEqual("Updated Author", upravenaKniha.Autor);
        }

        [TestMethod]
        public void UpdateBook_WhenTitleIsTooLong_ShouldFail()
        {
            var kniha = new DobraKniha("Old Book", "Old Author", "1234567890", 2020);
            _knihaRepository.Add(kniha);

            string longTitle = new string('A', 101);
            var upravenaKniha = new DobraKniha(longTitle, "Updated Author", "1234567890", 2021);
            upravenaKniha.Id = kniha.Id;

            Result result = _service.UpdateBook(upravenaKniha);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Název knihy může mít maximálně 100 znaků.", result.Message);
        }

        [TestMethod]
        public void UpdateBook_WhenTitleHasMaximumLength_ShouldSucceed()
        {
            var kniha = new DobraKniha("Old Book", "Old Author", "1234567890", 2020);
            _knihaRepository.Add(kniha);

            string title = new string('A', 100);
            var upravenaKniha = new DobraKniha(title, "Updated Author", "1234567890", 2021);
            upravenaKniha.Id = kniha.Id;

            Result result = _service.UpdateBook(upravenaKniha);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Kniha byla úspěšně upravena.", result.Message);
            Assert.AreEqual(100, upravenaKniha.Nazev.Length);
        }

        [TestMethod]
        public void UpdateBook_WhenAuthorIsTooLong_ShouldFail()
        {
            var kniha = new DobraKniha("Old Book", "Old Author", "1234567890", 2020);
            _knihaRepository.Add(kniha);

            string longAuthor = new string('A', 101);
            var upravenaKniha = new DobraKniha("Updated Book", longAuthor, "1234567890", 2021);
            upravenaKniha.Id = kniha.Id;

            Result result = _service.UpdateBook(upravenaKniha);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Autor knihy může mít maximálně 100 znaků.", result.Message);
        }

        [TestMethod]
        public void UpdateBook_WhenAuthorHasMaximumLength_ShouldSucceed()
        {
            var kniha = new DobraKniha("Old Book", "Old Author", "1234567890", 2020);
            _knihaRepository.Add(kniha);

            string author = new string('A', 100);
            var upravenaKniha = new DobraKniha("Updated Book", author, "1234567890", 2021);
            upravenaKniha.Id = kniha.Id;

            Result result = _service.UpdateBook(upravenaKniha);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Kniha byla úspěšně upravena.", result.Message);
            Assert.AreEqual(100, upravenaKniha.Autor.Length);
        }

        [TestMethod]
        public void UpdateBook_WhenPublicationYearIsTooLow_ShouldFail()
        {
            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            _knihaRepository.Add(kniha);

            var upravenaKniha = new DobraKniha("Updated Book", "Updated Author", "1234567890", 1449);
            upravenaKniha.Id = kniha.Id;

            Result result = _service.UpdateBook(upravenaKniha);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Rok vydání musí být mezi 1450 a aktuálním rokem.", result.Message);
        }

        [TestMethod]
        public void UpdateBook_WhenPublicationYearIsInFuture_ShouldFail()
        {
            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            _knihaRepository.Add(kniha);

            var upravenaKniha = new DobraKniha("Updated Book", "Updated Author", "1234567890", DateTime.Now.Year + 1);
            upravenaKniha.Id = kniha.Id;

            Result result = _service.UpdateBook(upravenaKniha);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Rok vydání musí být mezi 1450 a aktuálním rokem.", result.Message);
        }

        [TestMethod]
        public void AddReader_WhenDataAreValid_ShouldSucceed()
        {
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");

            Result result = _service.AddReader(ctenar);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Čtenář byl úspěšně přidán.", result.Message);
            Assert.IsNotNull(_ctenarRepository.GetById(ctenar.Id));
        }

        [TestMethod]
        public void AddReader_WhenNameAndSurnameContainSpaces_ShouldTrimValues()
        {
            var ctenar = new Ctenar("  Jan  ", "  Novak  ", "123456789", "jan@test.cz");

            Result result = _service.AddReader(ctenar);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Čtenář byl úspěšně přidán.", result.Message);
            Assert.AreEqual("Jan", ctenar.Jmeno);
            Assert.AreEqual("Novak", ctenar.Prijmeni);
        }

        [TestMethod]
        public void AddReader_WhenNameIsTooLong_ShouldFail()
        {
            string longName = new string('A', 51);
            var ctenar = new Ctenar(longName, "Novak", "123456789", "jan@test.cz");

            Result result = _service.AddReader(ctenar);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Jméno čtenáře může mít maximálně 50 znaků.", result.Message);
        }

        [TestMethod]
        public void AddReader_WhenNameHasMaximumLength_ShouldSucceed()
        {
            string name = new string('A', 50);
            var ctenar = new Ctenar(name, "Novak", "123456789", "jan@test.cz");

            Result result = _service.AddReader(ctenar);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Čtenář byl úspěšně přidán.", result.Message);
            Assert.AreEqual(50, ctenar.Jmeno.Length);
        }

        [TestMethod]
        public void AddReader_WhenSurnameIsTooLong_ShouldFail()
        {
            string longSurname = new string('A', 51);
            var ctenar = new Ctenar("Jan", longSurname, "123456789", "jan@test.cz");

            Result result = _service.AddReader(ctenar);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Příjmení čtenáře může mít maximálně 50 znaků.", result.Message);
        }

        [TestMethod]
        public void AddReader_WhenSurnameHasMaximumLength_ShouldSucceed()
        {
            string surname = new string('A', 50);
            var ctenar = new Ctenar("Jan", surname, "123456789", "jan@test.cz");

            Result result = _service.AddReader(ctenar);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Čtenář byl úspěšně přidán.", result.Message);
            Assert.AreEqual(50, ctenar.Prijmeni.Length);
        }

        [TestMethod]
        public void AddReader_WhenEmailIsTooLong_ShouldFail()
        {
            string longEmail = new string('a', 95) + "@test.cz";
            var ctenar = new Ctenar("Jan", "Novak", "123456789", longEmail);

            Result result = _service.AddReader(ctenar);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("E-mail čtenáře může mít maximálně 100 znaků.", result.Message);
        }

        [TestMethod]
        public void AddReader_WhenEmailHasMaximumLength_ShouldSucceed()
        {
            string email = new string('a', 50) + "@" + new string('b', 46) + ".cz";
            var ctenar = new Ctenar("Jan", "Novak", "123456789", email);

            Result result = _service.AddReader(ctenar);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Čtenář byl úspěšně přidán.", result.Message);
            Assert.AreEqual(100, ctenar.Email.Length);
        }

        [TestMethod]
        public void AddReader_WhenEmailAlreadyExists_ShouldFail()
        {
            var ctenar1 = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            var ctenar2 = new Ctenar("Petr", "Svoboda", "987654321", "jan@test.cz");

            _ctenarRepository.Add(ctenar1);

            Result result = _service.AddReader(ctenar2);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Čtenář s tímto e-mailem už existuje.", result.Message);
        }

        [TestMethod]
        public void AddReader_WhenEmailContainsSpaces_ShouldSaveNormalizedEmail()
        {
            var ctenar = new Ctenar("Jan", "Novak", "123456789", " JAN@Test.cz ");

            Result result = _service.AddReader(ctenar);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("jan@test.cz", ctenar.Email);
        }

        [TestMethod]
        public void AddReader_WhenSameEmailHasDifferentCase_ShouldFail()
        {
            var ctenar1 = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            var ctenar2 = new Ctenar("Petr", "Svoboda", "987654321", "JAN@Test.cz");

            _service.AddReader(ctenar1);

            Result result = _service.AddReader(ctenar2);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Čtenář s tímto e-mailem už existuje.", result.Message);
        }

        [TestMethod]
        public void AddReader_WhenEmailHasInvalidFormat_ShouldFail()
        {
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "spatnyemail");

            Result result = _service.AddReader(ctenar);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("E-mail musí být ve správném formátu.", result.Message);
        }

        [TestMethod]
        public void AddReader_WhenEmailHasMultipleAtSymbols_ShouldFail()
        {
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@@test.cz");

            Result result = _service.AddReader(ctenar);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("E-mail musí být ve správném formátu.", result.Message);
        }

        [TestMethod]
        public void AddReader_WhenEmailDomainStartsWithDot_ShouldFail()
        {
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@.cz");

            Result result = _service.AddReader(ctenar);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("E-mail musí být ve správném formátu.", result.Message);
        }

        [TestMethod]
        public void AddReader_WhenEmailDomainEndsWithDot_ShouldFail()
        {
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@test.");

            Result result = _service.AddReader(ctenar);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("E-mail musí být ve správném formátu.", result.Message);
        }

        [TestMethod]
        public void AddReader_WhenPhoneNumberHasInvalidFormat_ShouldFail()
        {
            var ctenar = new Ctenar("Jan", "Novak", "12345", "jan@test.cz");

            Result result = _service.AddReader(ctenar);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Telefonní číslo musí mít 9 číslic.", result.Message);
        }

        [TestMethod]
        public void AddReader_WhenPhoneNumberContainsSpacesAndHyphens_ShouldSaveNormalizedPhoneNumber()
        {
            var ctenar = new Ctenar("Jan", "Novak", "123-456 789", "jan@test.cz");

            Result result = _service.AddReader(ctenar);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Čtenář byl úspěšně přidán.", result.Message);
            Assert.AreEqual("123456789", ctenar.TelefonniCislo);
        }

        [TestMethod]
        public void UpdateReader_WhenReaderDoesNotExist_ShouldFail()
        {
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            ctenar.Id = 999;

            Result result = _service.UpdateReader(ctenar);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Čtenář nebyl nalezen.", result.Message);
        }

        [TestMethod]
        public void UpdateReader_WhenNameAndSurnameContainSpaces_ShouldTrimValues()
        {
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            _ctenarRepository.Add(ctenar);

            var upravenyCtenar = new Ctenar("  Petr  ", "  Svoboda  ", "123456789", "jan@test.cz");
            upravenyCtenar.Id = ctenar.Id;

            Result result = _service.UpdateReader(upravenyCtenar);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Čtenář byl úspěšně upraven.", result.Message);
            Assert.AreEqual("Petr", upravenyCtenar.Jmeno);
            Assert.AreEqual("Svoboda", upravenyCtenar.Prijmeni);
        }

        [TestMethod]
        public void UpdateReader_WhenNameIsTooLong_ShouldFail()
        {
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            _ctenarRepository.Add(ctenar);

            string longName = new string('A', 51);
            var upravenyCtenar = new Ctenar(longName, "Novak", "123456789", "jan@test.cz");
            upravenyCtenar.Id = ctenar.Id;

            Result result = _service.UpdateReader(upravenyCtenar);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Jméno čtenáře může mít maximálně 50 znaků.", result.Message);
        }

        [TestMethod]
        public void UpdateReader_WhenNameHasMaximumLength_ShouldSucceed()
        {
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            _ctenarRepository.Add(ctenar);

            string name = new string('A', 50);
            var upravenyCtenar = new Ctenar(name, "Novak", "123456789", "jan@test.cz");
            upravenyCtenar.Id = ctenar.Id;

            Result result = _service.UpdateReader(upravenyCtenar);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Čtenář byl úspěšně upraven.", result.Message);
            Assert.AreEqual(50, upravenyCtenar.Jmeno.Length);
        }

        [TestMethod]
        public void UpdateReader_WhenSurnameIsTooLong_ShouldFail()
        {
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            _ctenarRepository.Add(ctenar);

            string longSurname = new string('A', 51);
            var upravenyCtenar = new Ctenar("Jan", longSurname, "123456789", "jan@test.cz");
            upravenyCtenar.Id = ctenar.Id;

            Result result = _service.UpdateReader(upravenyCtenar);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Příjmení čtenáře může mít maximálně 50 znaků.", result.Message);
        }

        [TestMethod]
        public void UpdateReader_WhenSurnameHasMaximumLength_ShouldSucceed()
        {
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            _ctenarRepository.Add(ctenar);

            string surname = new string('A', 50);
            var upravenyCtenar = new Ctenar("Jan", surname, "123456789", "jan@test.cz");
            upravenyCtenar.Id = ctenar.Id;

            Result result = _service.UpdateReader(upravenyCtenar);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Čtenář byl úspěšně upraven.", result.Message);
            Assert.AreEqual(50, upravenyCtenar.Prijmeni.Length);
        }

        [TestMethod]
        public void UpdateReader_WhenEmailIsTooLong_ShouldFail()
        {
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            _ctenarRepository.Add(ctenar);

            string longEmail = new string('a', 95) + "@test.cz";
            var upravenyCtenar = new Ctenar("Jan", "Novak", "123456789", longEmail);
            upravenyCtenar.Id = ctenar.Id;

            Result result = _service.UpdateReader(upravenyCtenar);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("E-mail čtenáře může mít maximálně 100 znaků.", result.Message);
        }

        [TestMethod]
        public void UpdateReader_WhenEmailHasMaximumLength_ShouldSucceed()
        {
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            _ctenarRepository.Add(ctenar);

            string email = new string('a', 50) + "@" + new string('b', 46) + ".cz";
            var upravenyCtenar = new Ctenar("Jan", "Novak", "123456789", email);
            upravenyCtenar.Id = ctenar.Id;

            Result result = _service.UpdateReader(upravenyCtenar);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Čtenář byl úspěšně upraven.", result.Message);
            Assert.AreEqual(100, upravenyCtenar.Email.Length);
        }

        [TestMethod]
        public void UpdateReader_WhenEmailContainsSpaces_ShouldSaveNormalizedEmail()
        {
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            _ctenarRepository.Add(ctenar);

            var upravenyCtenar = new Ctenar("Jan", "Novak", "123456789", " JAN@Test.cz ");
            upravenyCtenar.Id = ctenar.Id;

            Result result = _service.UpdateReader(upravenyCtenar);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Čtenář byl úspěšně upraven.", result.Message);
            Assert.AreEqual("jan@test.cz", upravenyCtenar.Email);
        }

        [TestMethod]
        public void UpdateReader_WhenEmailHasInvalidFormat_ShouldFail()
        {
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            _ctenarRepository.Add(ctenar);

            var upravenyCtenar = new Ctenar("Jan", "Novotny", "123456789", "spatnyemail");
            upravenyCtenar.Id = ctenar.Id;

            Result result = _service.UpdateReader(upravenyCtenar);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("E-mail musí být ve správném formátu.", result.Message);
        }

        [TestMethod]
        public void UpdateReader_WhenEmailHasMultipleAtSymbols_ShouldFail()
        {
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            _ctenarRepository.Add(ctenar);

            var upravenyCtenar = new Ctenar("Jan", "Novak", "123456789", "jan@@test.cz");
            upravenyCtenar.Id = ctenar.Id;

            Result result = _service.UpdateReader(upravenyCtenar);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("E-mail musí být ve správném formátu.", result.Message);
        }

        [TestMethod]
        public void UpdateReader_WhenEmailDomainStartsWithDot_ShouldFail()
        {
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            _ctenarRepository.Add(ctenar);

            var upravenyCtenar = new Ctenar("Jan", "Novak", "123456789", "jan@.cz");
            upravenyCtenar.Id = ctenar.Id;

            Result result = _service.UpdateReader(upravenyCtenar);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("E-mail musí být ve správném formátu.", result.Message);
        }

        [TestMethod]
        public void UpdateReader_WhenEmailDomainEndsWithDot_ShouldFail()
        {
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            _ctenarRepository.Add(ctenar);

            var upravenyCtenar = new Ctenar("Jan", "Novak", "123456789", "jan@test.");
            upravenyCtenar.Id = ctenar.Id;

            Result result = _service.UpdateReader(upravenyCtenar);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("E-mail musí být ve správném formátu.", result.Message);
        }

        [TestMethod]
        public void UpdateReader_WhenPhoneNumberHasInvalidFormat_ShouldFail()
        {
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            _ctenarRepository.Add(ctenar);

            var upravenyCtenar = new Ctenar("Jan", "Novotny", "12345", "jan@test.cz");
            upravenyCtenar.Id = ctenar.Id;

            Result result = _service.UpdateReader(upravenyCtenar);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Telefonní číslo musí mít 9 číslic.", result.Message);
        }

        [TestMethod]
        public void UpdateReader_WhenPhoneNumberContainsSpacesAndHyphens_ShouldSaveNormalizedPhoneNumber()
        {
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            _ctenarRepository.Add(ctenar);

            var upravenyCtenar = new Ctenar("Jan", "Novotny", "123-456 789", "jan@test.cz");
            upravenyCtenar.Id = ctenar.Id;

            Result result = _service.UpdateReader(upravenyCtenar);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Čtenář byl úspěšně upraven.", result.Message);
            Assert.AreEqual("123456789", upravenyCtenar.TelefonniCislo);
        }

        [TestMethod]
        public void BorrowBook_WhenBookIsAvailable_ShouldCreateLoan()
        {
            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");

            _knihaRepository.Add(kniha);
            _ctenarRepository.Add(ctenar);

            Result result = _service.BorrowBook(kniha.Id, ctenar.Id);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(_vypujckaRepository.HasActiveLoanForBook(kniha.Id));
            Assert.IsTrue(_vypujckaRepository.HasActiveLoanForReader(ctenar.Id));
        }

        [TestMethod]
        public void BorrowBook_WhenBookDoesNotExist_ShouldFail()
        {
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            _ctenarRepository.Add(ctenar);

            Result result = _service.BorrowBook(999, ctenar.Id);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Kniha nebyla nalezena.", result.Message);
        }

        [TestMethod]
        public void BorrowBook_WhenReaderDoesNotExist_ShouldFail()
        {
            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            _knihaRepository.Add(kniha);

            Result result = _service.BorrowBook(kniha.Id, 999);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Čtenář nebyl nalezen.", result.Message);
        }

        [TestMethod]
        public void BorrowBook_WhenBookIsAlreadyBorrowed_ShouldFail()
        {
            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            var ctenar1 = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            var ctenar2 = new Ctenar("Petr", "Svoboda", "987654321", "petr@test.cz");

            _knihaRepository.Add(kniha);
            _ctenarRepository.Add(ctenar1);
            _ctenarRepository.Add(ctenar2);

            _service.BorrowBook(kniha.Id, ctenar1.Id);

            Result result = _service.BorrowBook(kniha.Id, ctenar2.Id);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Kniha je už vypůjčená.", result.Message);
        }

        [TestMethod]
        public void UpdateBook_WhenBookHasActiveLoan_ShouldFail()
        {
            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");

            _knihaRepository.Add(kniha);
            _ctenarRepository.Add(ctenar);

            _service.BorrowBook(kniha.Id, ctenar.Id);

            var upravenaKniha = new DobraKniha("Updated Book", "Updated Author", "1234567890", 2021);
            upravenaKniha.Id = kniha.Id;

            Result result = _service.UpdateBook(upravenaKniha);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Knihu nelze upravit, protože je aktuálně vypůjčená.", result.Message);
        }

        [TestMethod]
        public void UpdateBook_WhenBookHasActiveReservation_ShouldFail()
        {
            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            var ctenar1 = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            var ctenar2 = new Ctenar("Petr", "Svoboda", "987654321", "petr@test.cz");

            _knihaRepository.Add(kniha);
            _ctenarRepository.Add(ctenar1);
            _ctenarRepository.Add(ctenar2);

            _service.BorrowBook(kniha.Id, ctenar1.Id);
            _service.ReserveBook(kniha.Id, ctenar2.Id);
            _service.ReturnBook(kniha.Id);

            var upravenaKniha = new DobraKniha("Updated Book", "Updated Author", "1234567890", 2021);
            upravenaKniha.Id = kniha.Id;

            Result result = _service.UpdateBook(upravenaKniha);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Knihu nelze upravit, protože má aktivní rezervace.", result.Message);
        }

        [TestMethod]
        public void BorrowBook_WhenBookReservedForAnotherReader_ShouldFail()
        {
            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            var ctenar1 = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            var ctenar2 = new Ctenar("Petr", "Svoboda", "987654321", "petr@test.cz");
            var ctenar3 = new Ctenar("Karel", "Dvorak", "111222333", "karel@test.cz");

            _knihaRepository.Add(kniha);
            _ctenarRepository.Add(ctenar1);
            _ctenarRepository.Add(ctenar2);
            _ctenarRepository.Add(ctenar3);

            _service.BorrowBook(kniha.Id, ctenar1.Id);
            _service.ReserveBook(kniha.Id, ctenar2.Id);
            _service.ReturnBook(kniha.Id);

            Result result = _service.BorrowBook(kniha.Id, ctenar3.Id);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Kniha je rezervovaná pro jiného čtenáře.", result.Message);
        }

        [TestMethod]
        public void BorrowBook_WhenBookReservedForSameReader_ShouldCompleteReservation()
        {
            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            var ctenar1 = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            var ctenar2 = new Ctenar("Petr", "Svoboda", "987654321", "petr@test.cz");

            _knihaRepository.Add(kniha);
            _ctenarRepository.Add(ctenar1);
            _ctenarRepository.Add(ctenar2);

            _service.BorrowBook(kniha.Id, ctenar1.Id);
            _service.ReserveBook(kniha.Id, ctenar2.Id);

            Rezervace rezervace = _rezervaceRepository.GetAll().First();

            _service.ReturnBook(kniha.Id);

            Result result = _service.BorrowBook(kniha.Id, ctenar2.Id);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Kniha byla úspěšně vypůjčena.", result.Message);
            Assert.AreEqual("Vyrizena", rezervace.Stav);
            Assert.IsTrue(_vypujckaRepository.HasActiveLoanForReader(ctenar2.Id));
        }

        [TestMethod]
        public void BorrowBook_WhenMultipleReservationsExist_ShouldAllowOnlyFirstReaderInQueue()
        {
            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            var ctenar1 = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            var ctenar2 = new Ctenar("Petr", "Svoboda", "987654321", "petr@test.cz");
            var ctenar3 = new Ctenar("Karel", "Dvorak", "111222333", "karel@test.cz");

            _knihaRepository.Add(kniha);
            _ctenarRepository.Add(ctenar1);
            _ctenarRepository.Add(ctenar2);
            _ctenarRepository.Add(ctenar3);

            _service.BorrowBook(kniha.Id, ctenar1.Id);
            _service.ReserveBook(kniha.Id, ctenar2.Id);
            _service.ReserveBook(kniha.Id, ctenar3.Id);
            _service.ReturnBook(kniha.Id);

            Result result = _service.BorrowBook(kniha.Id, ctenar3.Id);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Kniha je rezervovaná pro jiného čtenáře.", result.Message);
        }

        [TestMethod]
        public void BorrowBook_WhenMultipleReservationsExist_ShouldCompleteFirstReservation()
        {
            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            var ctenar1 = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            var ctenar2 = new Ctenar("Petr", "Svoboda", "987654321", "petr@test.cz");
            var ctenar3 = new Ctenar("Karel", "Dvorak", "111222333", "karel@test.cz");

            _knihaRepository.Add(kniha);
            _ctenarRepository.Add(ctenar1);
            _ctenarRepository.Add(ctenar2);
            _ctenarRepository.Add(ctenar3);

            _service.BorrowBook(kniha.Id, ctenar1.Id);
            _service.ReserveBook(kniha.Id, ctenar2.Id);
            _service.ReserveBook(kniha.Id, ctenar3.Id);

            Rezervace firstReservation = _rezervaceRepository.GetAll()
                .OrderBy(r => r.DatumRezervace)
                .ThenBy(r => r.Id)
                .First();

            Rezervace secondReservation = _rezervaceRepository.GetAll()
                .OrderBy(r => r.DatumRezervace)
                .ThenBy(r => r.Id)
                .Skip(1)
                .First();

            _service.ReturnBook(kniha.Id);

            Result result = _service.BorrowBook(kniha.Id, ctenar2.Id);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Kniha byla úspěšně vypůjčena.", result.Message);
            Assert.AreEqual("Vyrizena", firstReservation.Stav);
            Assert.AreEqual("Aktivni", secondReservation.Stav);
            Assert.IsTrue(_vypujckaRepository.HasActiveLoanForReader(ctenar2.Id));
        }

        [TestMethod]
        public void BorrowBook_WhenFirstReservationIsCancelled_ShouldAllowNextReaderInQueue()
        {
            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            var ctenar1 = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            var ctenar2 = new Ctenar("Petr", "Svoboda", "987654321", "petr@test.cz");
            var ctenar3 = new Ctenar("Karel", "Dvorak", "111222333", "karel@test.cz");

            _knihaRepository.Add(kniha);
            _ctenarRepository.Add(ctenar1);
            _ctenarRepository.Add(ctenar2);
            _ctenarRepository.Add(ctenar3);

            _service.BorrowBook(kniha.Id, ctenar1.Id);
            _service.ReserveBook(kniha.Id, ctenar2.Id);
            _service.ReserveBook(kniha.Id, ctenar3.Id);

            Rezervace firstReservation = _rezervaceRepository.GetAll()
                .OrderBy(r => r.DatumRezervace)
                .ThenBy(r => r.Id)
                .First();

            _service.CancelReservation(firstReservation.Id);
            _service.ReturnBook(kniha.Id);

            Result result = _service.BorrowBook(kniha.Id, ctenar3.Id);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Kniha byla úspěšně vypůjčena.", result.Message);
            Assert.AreEqual("Zrusena", firstReservation.Stav);
            Assert.IsTrue(_vypujckaRepository.HasActiveLoanForReader(ctenar3.Id));
        }

        [TestMethod]
        public void BorrowBook_WhenReservationsHaveSameDate_ShouldUseLowerReservationIdFirst()
        {
            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            var ctenar1 = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            var ctenar2 = new Ctenar("Petr", "Svoboda", "987654321", "petr@test.cz");
            var ctenar3 = new Ctenar("Karel", "Dvorak", "111222333", "karel@test.cz");

            _knihaRepository.Add(kniha);
            _ctenarRepository.Add(ctenar1);
            _ctenarRepository.Add(ctenar2);
            _ctenarRepository.Add(ctenar3);

            _service.BorrowBook(kniha.Id, ctenar1.Id);

            DateTime sameReservationDate = new DateTime(2025, 1, 1, 10, 0, 0);

            var firstReservation = new Rezervace
            {
                KnihaId = kniha.Id,
                CtenarId = ctenar2.Id,
                DatumRezervace = sameReservationDate,
                Stav = "Aktivni"
            };

            var secondReservation = new Rezervace
            {
                KnihaId = kniha.Id,
                CtenarId = ctenar3.Id,
                DatumRezervace = sameReservationDate,
                Stav = "Aktivni"
            };

            _rezervaceRepository.Add(firstReservation);
            _rezervaceRepository.Add(secondReservation);

            _service.ReturnBook(kniha.Id);

            Result result = _service.BorrowBook(kniha.Id, ctenar3.Id);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Kniha je rezervovaná pro jiného čtenáře.", result.Message);
            Assert.AreEqual(1, firstReservation.Id);
            Assert.AreEqual(2, secondReservation.Id);
        }

        [TestMethod]
        public void ReturnBook_WhenBookIsBorrowed_ShouldMarkLoanAsReturned()
        {
            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");

            _knihaRepository.Add(kniha);
            _ctenarRepository.Add(ctenar);

            _service.BorrowBook(kniha.Id, ctenar.Id);

            Result result = _service.ReturnBook(kniha.Id);

            Assert.IsTrue(result.Success);
            Assert.IsFalse(_vypujckaRepository.HasActiveLoanForBook(kniha.Id));
            Assert.IsFalse(_vypujckaRepository.HasActiveLoanForReader(ctenar.Id));
        }

        [TestMethod]
        public void ReturnBook_WhenBookDoesNotExist_ShouldFail()
        {
            Result result = _service.ReturnBook(999);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Kniha nebyla nalezena.", result.Message);
        }

        [TestMethod]
        public void ReturnBook_WhenBookIsNotBorrowed_ShouldFail()
        {
            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            _knihaRepository.Add(kniha);

            Result result = _service.ReturnBook(kniha.Id);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Kniha není aktuálně vypůjčená.", result.Message);
        }

        [TestMethod]
        public void ReserveBook_WhenBookIsAvailable_ShouldFail()
        {
            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");

            _knihaRepository.Add(kniha);
            _ctenarRepository.Add(ctenar);

            Result result = _service.ReserveBook(kniha.Id, ctenar.Id);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Dostupnou knihu nelze rezervovat, protože ji lze rovnou půjčit.", result.Message);
        }

        [TestMethod]
        public void ReserveBook_WhenBookDoesNotExist_ShouldFail()
        {
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            _ctenarRepository.Add(ctenar);

            Result result = _service.ReserveBook(999, ctenar.Id);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Kniha nebyla nalezena.", result.Message);
        }

        [TestMethod]
        public void ReserveBook_WhenReaderDoesNotExist_ShouldFail()
        {
            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            _knihaRepository.Add(kniha);

            Result result = _service.ReserveBook(kniha.Id, 999);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Čtenář nebyl nalezen.", result.Message);
        }

        [TestMethod]
        public void ReserveBook_WhenBookIsBorrowed_ShouldCreateReservation()
        {
            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            var ctenar1 = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            var ctenar2 = new Ctenar("Petr", "Svoboda", "987654321", "petr@test.cz");

            _knihaRepository.Add(kniha);
            _ctenarRepository.Add(ctenar1);
            _ctenarRepository.Add(ctenar2);

            _service.BorrowBook(kniha.Id, ctenar1.Id);

            Result result = _service.ReserveBook(kniha.Id, ctenar2.Id);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(_rezervaceRepository.ExistsActiveReservation(kniha.Id, ctenar2.Id));
        }

        [TestMethod]
        public void ReserveBook_WhenReaderAlreadyReservedSameBook_ShouldFail()
        {
            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            var ctenar1 = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            var ctenar2 = new Ctenar("Petr", "Svoboda", "987654321", "petr@test.cz");

            _knihaRepository.Add(kniha);
            _ctenarRepository.Add(ctenar1);
            _ctenarRepository.Add(ctenar2);

            _service.BorrowBook(kniha.Id, ctenar1.Id);
            _service.ReserveBook(kniha.Id, ctenar2.Id);

            Result result = _service.ReserveBook(kniha.Id, ctenar2.Id);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Čtenář už má tuto knihu rezervovanou.", result.Message);
        }

        [TestMethod]
        public void ReserveBook_WhenReaderBorrowedSameBook_ShouldFail()
        {
            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");

            _knihaRepository.Add(kniha);
            _ctenarRepository.Add(ctenar);

            _service.BorrowBook(kniha.Id, ctenar.Id);

            Result result = _service.ReserveBook(kniha.Id, ctenar.Id);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Čtenář nemůže rezervovat knihu, kterou má právě vypůjčenou.", result.Message);
        }

        [TestMethod]
        public void CancelReservation_WhenReservationExists_ShouldCancelReservation()
        {
            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            var ctenar1 = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            var ctenar2 = new Ctenar("Petr", "Svoboda", "987654321", "petr@test.cz");

            _knihaRepository.Add(kniha);
            _ctenarRepository.Add(ctenar1);
            _ctenarRepository.Add(ctenar2);

            _service.BorrowBook(kniha.Id, ctenar1.Id);
            _service.ReserveBook(kniha.Id, ctenar2.Id);

            Rezervace rezervace = _rezervaceRepository.GetAll().First();

            Result result = _service.CancelReservation(rezervace.Id);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Rezervace byla zrušena.", result.Message);
            Assert.AreEqual("Zrusena", rezervace.Stav);
        }

        [TestMethod]
        public void CancelReservation_WhenReservationDoesNotExist_ShouldFail()
        {
            Result result = _service.CancelReservation(999);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Rezervace nebyla nalezena.", result.Message);
        }

        [TestMethod]
        public void CancelReservation_WhenReservationIsAlreadyCancelled_ShouldFail()
        {
            var rezervace = new Rezervace
            {
                KnihaId = 1,
                CtenarId = 1,
                DatumRezervace = DateTime.Now,
                Stav = "Zrusena"
            };

            _rezervaceRepository.Add(rezervace);

            Result result = _service.CancelReservation(rezervace.Id);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Rezervace už není aktivní.", result.Message);
        }

        [TestMethod]
        public void DeleteBook_WhenBookDoesNotExist_ShouldFail()
        {
            Result result = _service.DeleteBook(999);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Kniha nebyla nalezena.", result.Message);
        }

        [TestMethod]
        public void DeleteBook_WhenBookHasActiveLoan_ShouldFail()
        {
            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");

            _knihaRepository.Add(kniha);
            _ctenarRepository.Add(ctenar);

            _service.BorrowBook(kniha.Id, ctenar.Id);

            Result result = _service.DeleteBook(kniha.Id);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Knihu nelze smazat, protože je aktuálně vypůjčená.", result.Message);
        }

        [TestMethod]
        public void DeleteBook_WhenBookHasActiveReservation_ShouldFail()
        {
            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            var ctenar1 = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            var ctenar2 = new Ctenar("Petr", "Svoboda", "987654321", "petr@test.cz");

            _knihaRepository.Add(kniha);
            _ctenarRepository.Add(ctenar1);
            _ctenarRepository.Add(ctenar2);

            _service.BorrowBook(kniha.Id, ctenar1.Id);
            _service.ReserveBook(kniha.Id, ctenar2.Id);
            _service.ReturnBook(kniha.Id);

            Result result = _service.DeleteBook(kniha.Id);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Knihu nelze smazat, protože má aktivní rezervace.", result.Message);
        }

        [TestMethod]
        public void DeleteBook_WhenBookHasOnlyReturnedLoans_ShouldSucceed()
        {
            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");

            _knihaRepository.Add(kniha);
            _ctenarRepository.Add(ctenar);

            _service.BorrowBook(kniha.Id, ctenar.Id);
            _service.ReturnBook(kniha.Id);

            Result result = _service.DeleteBook(kniha.Id);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Kniha byla úspěšně smazána.", result.Message);
            Assert.IsNull(_knihaRepository.GetById(kniha.Id));
            Assert.IsFalse(_vypujckaRepository.GetAll().Any(v => v.KnihaId == kniha.Id));
        }

        [TestMethod]
        public void DeleteBook_WhenBookHasOnlyCancelledReservations_ShouldSucceed()
        {
            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            var ctenar1 = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            var ctenar2 = new Ctenar("Petr", "Svoboda", "987654321", "petr@test.cz");

            _knihaRepository.Add(kniha);
            _ctenarRepository.Add(ctenar1);
            _ctenarRepository.Add(ctenar2);

            _service.BorrowBook(kniha.Id, ctenar1.Id);
            _service.ReserveBook(kniha.Id, ctenar2.Id);

            var rezervace = _rezervaceRepository.GetAll().First();
            _rezervaceRepository.Cancel(rezervace.Id);

            _service.ReturnBook(kniha.Id);

            Result result = _service.DeleteBook(kniha.Id);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Kniha byla úspěšně smazána.", result.Message);
            Assert.IsNull(_knihaRepository.GetById(kniha.Id));
            Assert.IsFalse(_rezervaceRepository.GetAll().Any(r => r.KnihaId == kniha.Id));
        }

        [TestMethod]
        public void DeleteReader_WhenReaderDoesNotExist_ShouldFail()
        {
            Result result = _service.DeleteReader(999);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Čtenář nebyl nalezen.", result.Message);
        }

        [TestMethod]
        public void DeleteReader_WhenReaderHasActiveLoan_ShouldFail()
        {
            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");

            _knihaRepository.Add(kniha);
            _ctenarRepository.Add(ctenar);

            _service.BorrowBook(kniha.Id, ctenar.Id);

            Result result = _service.DeleteReader(ctenar.Id);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Čtenáře nelze smazat, protože má aktivní výpůjčku.", result.Message);
        }

        [TestMethod]
        public void DeleteReader_WhenReaderHasActiveReservation_ShouldFail()
        {
            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            var ctenar1 = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            var ctenar2 = new Ctenar("Petr", "Svoboda", "987654321", "petr@test.cz");

            _knihaRepository.Add(kniha);
            _ctenarRepository.Add(ctenar1);
            _ctenarRepository.Add(ctenar2);

            _service.BorrowBook(kniha.Id, ctenar1.Id);
            _service.ReserveBook(kniha.Id, ctenar2.Id);

            Result result = _service.DeleteReader(ctenar2.Id);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Čtenáře nelze smazat, protože má aktivní rezervaci.", result.Message);
        }

        [TestMethod]
        public void DeleteReader_WhenReaderHasOnlyReturnedLoans_ShouldSucceed()
        {
            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");

            _knihaRepository.Add(kniha);
            _ctenarRepository.Add(ctenar);

            _service.BorrowBook(kniha.Id, ctenar.Id);
            _service.ReturnBook(kniha.Id);

            Result result = _service.DeleteReader(ctenar.Id);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Čtenář byl úspěšně smazán.", result.Message);
            Assert.IsNull(_ctenarRepository.GetById(ctenar.Id));
            Assert.IsFalse(_vypujckaRepository.GetLoansByReaderId(ctenar.Id).Any());
        }

        [TestMethod]
        public void DeleteReader_WhenReaderHasOnlyCancelledReservations_ShouldSucceed()
        {
            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            var ctenar1 = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            var ctenar2 = new Ctenar("Petr", "Svoboda", "987654321", "petr@test.cz");

            _knihaRepository.Add(kniha);
            _ctenarRepository.Add(ctenar1);
            _ctenarRepository.Add(ctenar2);

            _service.BorrowBook(kniha.Id, ctenar1.Id);
            _service.ReserveBook(kniha.Id, ctenar2.Id);

            var rezervace = _rezervaceRepository.GetAll().First();
            _rezervaceRepository.Cancel(rezervace.Id);

            Result result = _service.DeleteReader(ctenar2.Id);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Čtenář byl úspěšně smazán.", result.Message);
            Assert.IsNull(_ctenarRepository.GetById(ctenar2.Id));
            Assert.IsFalse(_rezervaceRepository.GetAll().Any(r => r.CtenarId == ctenar2.Id));
        }

        [TestMethod]
        public void UpdateBook_WhenIsbnBelongsToAnotherBook_ShouldFail()
        {
            var kniha1 = new DobraKniha("Book One", "Author One", "1234567890", 2020);
            var kniha2 = new DobraKniha("Book Two", "Author Two", "0987654321", 2021);

            _knihaRepository.Add(kniha1);
            _knihaRepository.Add(kniha2);

            var upravenaKniha = new DobraKniha("Book Two Updated", "Author Two", "1234567890", 2021);
            upravenaKniha.Id = kniha2.Id;

            Result result = _service.UpdateBook(upravenaKniha);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Jiná kniha s tímto ISBN už existuje.", result.Message);
        }

        [TestMethod]
        public void UpdateBook_WhenIsbnBelongsToSameBook_ShouldSucceed()
        {
            var kniha = new DobraKniha("Book One", "Author One", "1234567890", 2020);
            _knihaRepository.Add(kniha);

            var upravenaKniha = new DobraKniha("Book One Updated", "Author One Updated", "1234567890", 2022);
            upravenaKniha.Id = kniha.Id;

            Result result = _service.UpdateBook(upravenaKniha);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Kniha byla úspěšně upravena.", result.Message);
        }

        [TestMethod]
        public void UpdateReader_WhenEmailBelongsToAnotherReader_ShouldFail()
        {
            var ctenar1 = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            var ctenar2 = new Ctenar("Petr", "Svoboda", "987654321", "petr@test.cz");

            _ctenarRepository.Add(ctenar1);
            _ctenarRepository.Add(ctenar2);

            var upravenyCtenar = new Ctenar("Petr", "Svoboda", "987654321", "jan@test.cz");
            upravenyCtenar.Id = ctenar2.Id;

            Result result = _service.UpdateReader(upravenyCtenar);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Jiný čtenář s tímto e-mailem už existuje.", result.Message);
        }

        [TestMethod]
        public void UpdateReader_WhenSameEmailHasDifferentCase_ShouldFail()
        {
            var ctenar1 = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            var ctenar2 = new Ctenar("Petr", "Svoboda", "987654321", "petr@test.cz");

            _ctenarRepository.Add(ctenar1);
            _ctenarRepository.Add(ctenar2);

            var upravenyCtenar = new Ctenar("Petr", "Svoboda", "987654321", "JAN@Test.cz");
            upravenyCtenar.Id = ctenar2.Id;

            Result result = _service.UpdateReader(upravenyCtenar);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Jiný čtenář s tímto e-mailem už existuje.", result.Message);
        }

        [TestMethod]
        public void UpdateReader_WhenEmailBelongsToSameReader_ShouldSucceed()
        {
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            _ctenarRepository.Add(ctenar);

            var upravenyCtenar = new Ctenar("Jan", "Novotny", "123456789", "jan@test.cz");
            upravenyCtenar.Id = ctenar.Id;

            Result result = _service.UpdateReader(upravenyCtenar);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Čtenář byl úspěšně upraven.", result.Message);
        }
    }
}