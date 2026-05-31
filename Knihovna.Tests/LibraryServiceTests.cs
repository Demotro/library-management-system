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
            // Arrange
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

            // Act
            Result result = service.BorrowBook(kniha.Id, ctenar.Id);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(vypujckaRepository.HasActiveLoanForBook(kniha.Id));
            Assert.IsTrue(vypujckaRepository.HasActiveLoanForReader(ctenar.Id));
        }

        [TestMethod]
        public void BorrowBook_WhenBookIsAlreadyBorrowed_ShouldFail()
        {
            // Arrange
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

            // Act
            Result result = service.BorrowBook(kniha.Id, ctenar2.Id);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Kniha je už vypůjčená.", result.Message);
        }

        [TestMethod]
        public void ReserveBook_WhenBookIsAvailable_ShouldFail()
        {
            // Arrange
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

            // Act
            Result result = service.ReserveBook(kniha.Id, ctenar.Id);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Dostupnou knihu nelze rezervovat, protože ji lze rovnou půjčit.", result.Message);
        }

        [TestMethod]
        public void ReserveBook_WhenBookIsBorrowed_ShouldCreateReservation()
        {
            // Arrange
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

            // Act
            Result result = service.ReserveBook(kniha.Id, ctenar2.Id);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(rezervaceRepository.ExistsActiveReservation(kniha.Id, ctenar2.Id));
        }
    }
}