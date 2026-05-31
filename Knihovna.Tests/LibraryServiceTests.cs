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
        public void AddReader_WhenDataAreValid_ShouldSucceed()
        {
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");

            Result result = _service.AddReader(ctenar);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Čtenář byl úspěšně přidán.", result.Message);
            Assert.IsNotNull(_ctenarRepository.GetById(ctenar.Id));
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