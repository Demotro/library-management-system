using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Knihovna
{
    public static class Databaze
    {
        //datove zdroje pro zobrazeni ve formulari
        public static BindingList<Ctenar> Ctenari { get; private set; } = new BindingList<Ctenar>();
        public static BindingList<Kniha> Knihy { get; private set; } = new BindingList<Kniha>();

        private static readonly IKnihaRepository _knihaRepository = new KnihaRepository();
        private static readonly ICtenarRepository _ctenarRepository = new CtenarRepository();
        private static readonly IVypujckaRepository _vypujckaRepository = new VypujckaRepository();
        private static readonly IRezervaceRepository _rezervaceRepository = new RezervaceRepository();
        private static readonly LibraryService _libraryService = new LibraryService();

        static Databaze()
        {
            NacistData();
        }

        //nacte data z SQLite databaze do BindingListu pro WinForms
        public static void NacistData()
        {
            var knihy = _knihaRepository.GetAll();
            var ctenari = _ctenarRepository.GetAll();

            foreach (var kniha in knihy)
            {
                kniha.Dostupnost = true;
            }

            foreach (var vypujcka in _vypujckaRepository.GetActiveLoans())
            {
                var kniha = knihy.FirstOrDefault(k => k.Id == vypujcka.KnihaId);
                var ctenar = ctenari.FirstOrDefault(c => c.Id == vypujcka.CtenarId);

                if (kniha != null && ctenar != null)
                {
                    kniha.Dostupnost = false;
                    ctenar.Vypujcene.Add(kniha);
                }
            }

            foreach (var rezervace in _rezervaceRepository.GetAll().Where(r => r.Stav == "Aktivni"))
            {
                var kniha = knihy.FirstOrDefault(k => k.Id == rezervace.KnihaId);
                var ctenar = ctenari.FirstOrDefault(c => c.Id == rezervace.CtenarId);

                if (kniha != null && ctenar != null)
                {
                    ctenar.Rezervovano.Add(kniha);
                }
            }

            Ctenari = new BindingList<Ctenar>(ctenari);
            Knihy = new BindingList<Kniha>(knihy);
        }

        //obnovi data po zmene v databazi
        public static void Obnovit()
        {
            NacistData();
        }

        public static bool PridatKnihu(Kniha kniha)
        {
            Result result = _libraryService.AddBook(kniha);
            MessageBox.Show(result.Message);

            if (result.Success)
            {
                Obnovit();
            }

            return result.Success;
        }

        public static bool UpravitKnihu(Kniha kniha)
        {
            Result result = _libraryService.UpdateBook(kniha);
            MessageBox.Show(result.Message);

            if (result.Success)
            {
                Obnovit();
            }

            return result.Success;
        }

        public static bool PridatCtenare(Ctenar ctenar)
        {
            Result result = _libraryService.AddReader(ctenar);
            MessageBox.Show(result.Message);

            if (result.Success)
            {
                Obnovit();
            }

            return result.Success;
        }

        public static bool UpravitCtenare(Ctenar ctenar)
        {
            Result result = _libraryService.UpdateReader(ctenar);
            MessageBox.Show(result.Message);

            if (result.Success)
            {
                Obnovit();
            }

            return result.Success;
        }

        public static bool Vypujcit(object cO, object kO)
        {
            var ctenar = (Ctenar)cO;
            var kniha = (Kniha)kO;

            Result result = _libraryService.BorrowBook(kniha.Id, ctenar.Id);
            MessageBox.Show(result.Message);

            if (result.Success)
            {
                Obnovit();
            }

            return result.Success;
        }

        public static void Vratit(object cO, object kO)
        {
            var kniha = (Kniha)kO;

            Result result = _libraryService.ReturnBook(kniha.Id);
            MessageBox.Show(result.Message);

            if (result.Success)
            {
                Obnovit();
            }
        }

        public static bool Rezervovat(object cO, object kO)
        {
            var ctenar = (Ctenar)cO;
            var kniha = (Kniha)kO;

            Result result = _libraryService.ReserveBook(kniha.Id, ctenar.Id);
            MessageBox.Show(result.Message);

            if (result.Success)
            {
                Obnovit();
            }

            return result.Success;
        }

        public static void Zrusit(object cO, object kO)
        {
            var ctenar = (Ctenar)cO;
            var kniha = (Kniha)kO;

            var rezervace = _rezervaceRepository
                .GetReservationsByReaderId(ctenar.Id)
                .FirstOrDefault(r => r.KnihaId == kniha.Id && r.Stav == "Aktivni");

            if (rezervace == null)
            {
                MessageBox.Show("Rezervace nebyla nalezena.");
                return;
            }

            Result result = _libraryService.CancelReservation(rezervace.Id);
            MessageBox.Show(result.Message);

            if (result.Success)
            {
                Obnovit();
            }
        }

        public static bool EditovanaKniha(Kniha kniha)
        {
            if (_vypujckaRepository.HasActiveLoanForBook(kniha.Id) ||
                _rezervaceRepository.HasActiveReservationForBook(kniha.Id))
            {
                MessageBox.Show("Knihu nemůžeš editovat, protože je vypůjčená nebo rezervovaná.");
                return false;
            }

            return true;
        }

        public static bool SmazatKnihu(Kniha kniha)
        {
            if (Knihy.Count == 1)
            {
                MessageBox.Show("Nemůžeš smazat poslední knihu. V databázi musí zůstat alespoň jedna kniha!");
                return false;
            }

            Result result = _libraryService.DeleteBook(kniha.Id);
            MessageBox.Show(result.Message);

            if (result.Success)
            {
                Obnovit();
            }

            return result.Success;
        }

        public static void VratitKnihy(Ctenar ctenar)
        {
            foreach (var kniha in ctenar.Vypujcene.ToList())
            {
                _libraryService.ReturnBook(kniha.Id);
            }

            Obnovit();
        }

        public static bool SmazatCtenare(Ctenar ctenar)
        {
            if (Ctenari.Count == 1)
            {
                MessageBox.Show("Nemůžeš smazat posledního čtenáře. V databázi musí zůstat alespoň jeden čtenář!");
                return false;
            }

            Result result = _libraryService.DeleteReader(ctenar.Id);
            MessageBox.Show(result.Message);

            if (result.Success)
            {
                Obnovit();
            }

            return result.Success;
        }

        public static bool SmazatelnaKniha(Kniha kniha)
        {
            if (Knihy.Count == 1)
            {
                MessageBox.Show("Nemůžeš smazat poslední knihu. V databázi musí zůstat alespoň jedna kniha!");
                return false;
            }

            if (_vypujckaRepository.HasActiveLoanForBook(kniha.Id) ||
                _rezervaceRepository.HasActiveReservationForBook(kniha.Id))
            {
                MessageBox.Show("Knihu nelze smazat, protože je vypůjčená nebo rezervovaná.");
                return false;
            }

            return true;
        }

        public static bool SmazatelnyCtenar(Ctenar ctenar)
        {
            if (Ctenari.Count == 1)
            {
                MessageBox.Show("Nemůžeš smazat posledního čtenáře. V databázi musí zůstat alespoň jeden čtenář!");
                return false;
            }

            if (_vypujckaRepository.HasActiveLoanForReader(ctenar.Id))
            {
                MessageBox.Show("Čtenáře nelze smazat, protože má aktivní výpůjčku.");
                return false;
            }

            if (_rezervaceRepository.HasActiveReservationForReader(ctenar.Id))
            {
                MessageBox.Show("Čtenáře nelze smazat, protože má aktivní rezervaci.");
                return false;
            }

            return true;
        }
    }
}