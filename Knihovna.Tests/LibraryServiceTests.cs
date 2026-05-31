using Microsoft.VisualStudio.TestTools.UnitTesting;
using Knihovna.Tests.Fakes;

namespace Knihovna.Tests
{
    [TestClass]
    public class LibraryServiceTests
    {
        private LibraryService CreateService(
            FakeKnihaRepository knihaRepository,
            FakeCtenarRepository ctenarRepository,
            FakeVypujckaRepository vypujckaRepository,
            FakeRezervaceRepository rezervaceRepository)
        {
            return new LibraryService(
                knihaRepository,
                ctenarRepository,
                vypujckaRepository,
                rezervaceRepository
            );
        }

        [TestMethod]
        public void BorrowBook_WhenBookIsAvailable_ShouldCreateLoan()
        {
            var knihaRepository = new FakeKnihaRepository();
            var ctenarRepository = new FakeCtenarRepository();
            var vypujckaRepository = new FakeVypujckaRepository();
            var rezervaceRepository = new FakeRezervaceRepository();

            var service = CreateService(
                knihaRepository,
                ctenarRepository,
                vypujckaRepository,
                rezervaceRepository
            );

            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");

            knihaRepository.Add(kniha);
            ctenarRepository.Add(ctenar);

            Result result = service.BorrowBook(kniha.Id, ctenar.Id);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(vypujckaRepository.HasActiveLoanForBook(kniha.Id));
            Assert.IsTrue(vypujckaRepository.HasActiveLoanForReader(ctenar.Id));
        }

        [TestMethod]
        public void BorrowBook_WhenBookIsAlreadyBorrowed_ShouldFail()
        {
            var knihaRepository = new FakeKnihaRepository();
            var ctenarRepository = new FakeCtenarRepository();
            var vypujckaRepository = new FakeVypujckaRepository();
            var rezervaceRepository = new FakeRezervaceRepository();

            var service = CreateService(
                knihaRepository,
                ctenarRepository,
                vypujckaRepository,
                rezervaceRepository
            );

            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            var ctenar1 = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            var ctenar2 = new Ctenar("Petr", "Svoboda", "987654321", "petr@test.cz");

            knihaRepository.Add(kniha);
            ctenarRepository.Add(ctenar1);
            ctenarRepository.Add(ctenar2);

            service.BorrowBook(kniha.Id, ctenar1.Id);

            Result result = service.BorrowBook(kniha.Id, ctenar2.Id);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Kniha je už vypůjčená.", result.Message);
        }

        [TestMethod]
        public void ReturnBook_WhenBookIsBorrowed_ShouldMarkLoanAsReturned()
        {
            var knihaRepository = new FakeKnihaRepository();
            var ctenarRepository = new FakeCtenarRepository();
            var vypujckaRepository = new FakeVypujckaRepository();
            var rezervaceRepository = new FakeRezervaceRepository();

            var service = CreateService(
                knihaRepository,
                ctenarRepository,
                vypujckaRepository,
                rezervaceRepository
            );

            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");

            knihaRepository.Add(kniha);
            ctenarRepository.Add(ctenar);

            service.BorrowBook(kniha.Id, ctenar.Id);

            Result result = service.ReturnBook(kniha.Id);

            Assert.IsTrue(result.Success);
            Assert.IsFalse(vypujckaRepository.HasActiveLoanForBook(kniha.Id));
            Assert.IsFalse(vypujckaRepository.HasActiveLoanForReader(ctenar.Id));
        }

        [TestMethod]
        public void ReserveBook_WhenBookIsAvailable_ShouldFail()
        {
            var knihaRepository = new FakeKnihaRepository();
            var ctenarRepository = new FakeCtenarRepository();
            var vypujckaRepository = new FakeVypujckaRepository();
            var rezervaceRepository = new FakeRezervaceRepository();

            var service = CreateService(
                knihaRepository,
                ctenarRepository,
                vypujckaRepository,
                rezervaceRepository
            );

            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");

            knihaRepository.Add(kniha);
            ctenarRepository.Add(ctenar);

            Result result = service.ReserveBook(kniha.Id, ctenar.Id);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Dostupnou knihu nelze rezervovat, protože ji lze rovnou půjčit.", result.Message);
        }

        [TestMethod]
        public void ReserveBook_WhenBookIsBorrowed_ShouldCreateReservation()
        {
            var knihaRepository = new FakeKnihaRepository();
            var ctenarRepository = new FakeCtenarRepository();
            var vypujckaRepository = new FakeVypujckaRepository();
            var rezervaceRepository = new FakeRezervaceRepository();

            var service = CreateService(
                knihaRepository,
                ctenarRepository,
                vypujckaRepository,
                rezervaceRepository
            );

            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            var ctenar1 = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            var ctenar2 = new Ctenar("Petr", "Svoboda", "987654321", "petr@test.cz");

            knihaRepository.Add(kniha);
            ctenarRepository.Add(ctenar1);
            ctenarRepository.Add(ctenar2);

            service.BorrowBook(kniha.Id, ctenar1.Id);

            Result result = service.ReserveBook(kniha.Id, ctenar2.Id);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(rezervaceRepository.ExistsActiveReservation(kniha.Id, ctenar2.Id));
        }

        [TestMethod]
        public void ReserveBook_WhenReaderAlreadyReservedSameBook_ShouldFail()
        {
            var knihaRepository = new FakeKnihaRepository();
            var ctenarRepository = new FakeCtenarRepository();
            var vypujckaRepository = new FakeVypujckaRepository();
            var rezervaceRepository = new FakeRezervaceRepository();

            var service = CreateService(
                knihaRepository,
                ctenarRepository,
                vypujckaRepository,
                rezervaceRepository
            );

            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            var ctenar1 = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            var ctenar2 = new Ctenar("Petr", "Svoboda", "987654321", "petr@test.cz");

            knihaRepository.Add(kniha);
            ctenarRepository.Add(ctenar1);
            ctenarRepository.Add(ctenar2);

            service.BorrowBook(kniha.Id, ctenar1.Id);
            service.ReserveBook(kniha.Id, ctenar2.Id);

            Result result = service.ReserveBook(kniha.Id, ctenar2.Id);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Čtenář už má tuto knihu rezervovanou.", result.Message);
        }

        [TestMethod]
        public void ReserveBook_WhenReaderBorrowedSameBook_ShouldFail()
        {
            var knihaRepository = new FakeKnihaRepository();
            var ctenarRepository = new FakeCtenarRepository();
            var vypujckaRepository = new FakeVypujckaRepository();
            var rezervaceRepository = new FakeRezervaceRepository();

            var service = CreateService(
                knihaRepository,
                ctenarRepository,
                vypujckaRepository,
                rezervaceRepository
            );

            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");

            knihaRepository.Add(kniha);
            ctenarRepository.Add(ctenar);

            service.BorrowBook(kniha.Id, ctenar.Id);

            Result result = service.ReserveBook(kniha.Id, ctenar.Id);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Čtenář nemůže rezervovat knihu, kterou má právě vypůjčenou.", result.Message);
        }

        [TestMethod]
        public void DeleteBook_WhenBookHasActiveLoan_ShouldFail()
        {
            var knihaRepository = new FakeKnihaRepository();
            var ctenarRepository = new FakeCtenarRepository();
            var vypujckaRepository = new FakeVypujckaRepository();
            var rezervaceRepository = new FakeRezervaceRepository();

            var service = CreateService(
                knihaRepository,
                ctenarRepository,
                vypujckaRepository,
                rezervaceRepository
            );

            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");

            knihaRepository.Add(kniha);
            ctenarRepository.Add(ctenar);

            service.BorrowBook(kniha.Id, ctenar.Id);

            Result result = service.DeleteBook(kniha.Id);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Knihu nelze smazat, protože je aktuálně vypůjčená.", result.Message);
        }

        [TestMethod]
        public void DeleteBook_WhenBookHasActiveReservation_ShouldFail()
        {
            var knihaRepository = new FakeKnihaRepository();
            var ctenarRepository = new FakeCtenarRepository();
            var vypujckaRepository = new FakeVypujckaRepository();
            var rezervaceRepository = new FakeRezervaceRepository();

            var service = CreateService(
                knihaRepository,
                ctenarRepository,
                vypujckaRepository,
                rezervaceRepository
            );

            var kniha = new DobraKniha("Test Book", "Test Author", "1234567890", 2020);
            var ctenar1 = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            var ctenar2 = new Ctenar("Petr", "Svoboda", "987654321", "petr@test.cz");

            knihaRepository.Add(kniha);
            ctenarRepository.Add(ctenar1);
            ctenarRepository.Add(ctenar2);

            service.BorrowBook(kniha.Id, ctenar1.Id);
            service.ReserveBook(kniha.Id, ctenar2.Id);
            service.ReturnBook(kniha.Id);

            Result result = service.DeleteBook(kniha.Id);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Knihu nelze smazat, protože má aktivní rezervace.", result.Message);
        }

        [TestMethod]
        public void UpdateBook_WhenIsbnBelongsToAnotherBook_ShouldFail()
        {
            var knihaRepository = new FakeKnihaRepository();
            var ctenarRepository = new FakeCtenarRepository();
            var vypujckaRepository = new FakeVypujckaRepository();
            var rezervaceRepository = new FakeRezervaceRepository();

            var service = CreateService(
                knihaRepository,
                ctenarRepository,
                vypujckaRepository,
                rezervaceRepository
            );

            var kniha1 = new DobraKniha("Book One", "Author One", "1234567890", 2020);
            var kniha2 = new DobraKniha("Book Two", "Author Two", "0987654321", 2021);

            knihaRepository.Add(kniha1);
            knihaRepository.Add(kniha2);

            var upravenaKniha = new DobraKniha("Book Two Updated", "Author Two", "1234567890", 2021);
            upravenaKniha.Id = kniha2.Id;

            Result result = service.UpdateBook(upravenaKniha);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Jiná kniha s tímto ISBN už existuje.", result.Message);
        }

        [TestMethod]
        public void UpdateBook_WhenIsbnBelongsToSameBook_ShouldSucceed()
        {
            var knihaRepository = new FakeKnihaRepository();
            var ctenarRepository = new FakeCtenarRepository();
            var vypujckaRepository = new FakeVypujckaRepository();
            var rezervaceRepository = new FakeRezervaceRepository();

            var service = CreateService(
                knihaRepository,
                ctenarRepository,
                vypujckaRepository,
                rezervaceRepository
            );

            var kniha = new DobraKniha("Book One", "Author One", "1234567890", 2020);
            knihaRepository.Add(kniha);

            var upravenaKniha = new DobraKniha("Book One Updated", "Author One Updated", "1234567890", 2022);
            upravenaKniha.Id = kniha.Id;

            Result result = service.UpdateBook(upravenaKniha);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Kniha byla úspěšně upravena.", result.Message);
        }

        [TestMethod]
        public void UpdateReader_WhenEmailBelongsToAnotherReader_ShouldFail()
        {
            var knihaRepository = new FakeKnihaRepository();
            var ctenarRepository = new FakeCtenarRepository();
            var vypujckaRepository = new FakeVypujckaRepository();
            var rezervaceRepository = new FakeRezervaceRepository();

            var service = CreateService(
                knihaRepository,
                ctenarRepository,
                vypujckaRepository,
                rezervaceRepository
            );

            var ctenar1 = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            var ctenar2 = new Ctenar("Petr", "Svoboda", "987654321", "petr@test.cz");

            ctenarRepository.Add(ctenar1);
            ctenarRepository.Add(ctenar2);

            var upravenyCtenar = new Ctenar("Petr", "Svoboda", "987654321", "jan@test.cz");
            upravenyCtenar.Id = ctenar2.Id;

            Result result = service.UpdateReader(upravenyCtenar);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Jiný čtenář s tímto e-mailem už existuje.", result.Message);
        }

        [TestMethod]
        public void UpdateReader_WhenEmailBelongsToSameReader_ShouldSucceed()
        {
            var knihaRepository = new FakeKnihaRepository();
            var ctenarRepository = new FakeCtenarRepository();
            var vypujckaRepository = new FakeVypujckaRepository();
            var rezervaceRepository = new FakeRezervaceRepository();

            var service = CreateService(
                knihaRepository,
                ctenarRepository,
                vypujckaRepository,
                rezervaceRepository
            );

            var ctenar = new Ctenar("Jan", "Novak", "123456789", "jan@test.cz");
            ctenarRepository.Add(ctenar);

            var upravenyCtenar = new Ctenar("Jan", "Novotny", "123456789", "jan@test.cz");
            upravenyCtenar.Id = ctenar.Id;

            Result result = service.UpdateReader(upravenyCtenar);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Čtenář byl úspěšně upraven.", result.Message);
        }
    }
}