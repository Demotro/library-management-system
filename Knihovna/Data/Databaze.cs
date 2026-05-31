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

        private static readonly LibraryService _libraryService = new LibraryService(
            _knihaRepository,
            _ctenarRepository,
            _vypujckaRepository,
            _rezervaceRepository
        );

        static Databaze()
        {
            NacistData();
        }

        //nacte data z SQLite databaze do BindingListu pro WinForms
        public static void NacistData()
        {
            try
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
            catch (Exception ex)
            {
                ShowDatabaseError("Chyba při načítání dat z databáze.", ex);

                Ctenari = new BindingList<Ctenar>();
                Knihy = new BindingList<Kniha>();
            }
        }

        //obnovi data po zmene v databazi
        public static void Obnovit()
        {
            NacistData();
        }

        public static bool PridatKnihu(Kniha kniha)
        {
            try
            {
                Result result = _libraryService.AddBook(kniha);
                MessageBox.Show(result.Message);

                if (result.Success)
                {
                    Obnovit();
                }

                return result.Success;
            }
            catch (Exception ex)
            {
                ShowDatabaseError("Chyba při přidávání knihy.", ex);
                return false;
            }
        }

        public static bool UpravitKnihu(Kniha kniha)
        {
            try
            {
                Result result = _libraryService.UpdateBook(kniha);
                MessageBox.Show(result.Message);

                if (result.Success)
                {
                    Obnovit();
                }

                return result.Success;
            }
            catch (Exception ex)
            {
                ShowDatabaseError("Chyba při úpravě knihy.", ex);
                return false;
            }
        }

        public static bool PridatCtenare(Ctenar ctenar)
        {
            try
            {
                Result result = _libraryService.AddReader(ctenar);
                MessageBox.Show(result.Message);

                if (result.Success)
                {
                    Obnovit();
                }

                return result.Success;
            }
            catch (Exception ex)
            {
                ShowDatabaseError("Chyba při přidávání čtenáře.", ex);
                return false;
            }
        }

        public static bool UpravitCtenare(Ctenar ctenar)
        {
            try
            {
                Result result = _libraryService.UpdateReader(ctenar);
                MessageBox.Show(result.Message);

                if (result.Success)
                {
                    Obnovit();
                }

                return result.Success;
            }
            catch (Exception ex)
            {
                ShowDatabaseError("Chyba při úpravě čtenáře.", ex);
                return false;
            }
        }

        public static bool Vypujcit(object cO, object kO)
        {
            try
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
            catch (Exception ex)
            {
                ShowDatabaseError("Chyba při vypůjčení knihy.", ex);
                return false;
            }
        }

        public static void Vratit(object cO, object kO)
        {
            try
            {
                var kniha = (Kniha)kO;

                Result result = _libraryService.ReturnBook(kniha.Id);
                MessageBox.Show(result.Message);

                if (result.Success)
                {
                    Obnovit();
                }
            }
            catch (Exception ex)
            {
                ShowDatabaseError("Chyba při vrácení knihy.", ex);
            }
        }

        public static bool Rezervovat(object cO, object kO)
        {
            try
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
            catch (Exception ex)
            {
                ShowDatabaseError("Chyba při rezervaci knihy.", ex);
                return false;
            }
        }

        public static void Zrusit(object cO, object kO)
        {
            try
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
            catch (Exception ex)
            {
                ShowDatabaseError("Chyba při rušení rezervace.", ex);
            }
        }

        public static bool EditovanaKniha(Kniha kniha)
        {
            try
            {
                if (_vypujckaRepository.HasActiveLoanForBook(kniha.Id) ||
                    _rezervaceRepository.HasActiveReservationForBook(kniha.Id))
                {
                    MessageBox.Show("Knihu nemůžeš editovat, protože je vypůjčená nebo rezervovaná.");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                ShowDatabaseError("Chyba při kontrole knihy.", ex);
                return false;
            }
        }

        public static bool SmazatKnihu(Kniha kniha)
        {
            try
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
            catch (Exception ex)
            {
                ShowDatabaseError("Chyba při mazání knihy.", ex);
                return false;
            }
        }

        public static void VratitKnihy(Ctenar ctenar)
        {
            try
            {
                foreach (var kniha in ctenar.Vypujcene.ToList())
                {
                    _libraryService.ReturnBook(kniha.Id);
                }

                Obnovit();
            }
            catch (Exception ex)
            {
                ShowDatabaseError("Chyba při vracení knih čtenáře.", ex);
            }
        }

        public static bool SmazatCtenare(Ctenar ctenar)
        {
            try
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
            catch (Exception ex)
            {
                ShowDatabaseError("Chyba při mazání čtenáře.", ex);
                return false;
            }
        }

        public static bool SmazatelnaKniha(Kniha kniha)
        {
            try
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
            catch (Exception ex)
            {
                ShowDatabaseError("Chyba při kontrole smazání knihy.", ex);
                return false;
            }
        }

        public static bool SmazatelnyCtenar(Ctenar ctenar)
        {
            try
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
            catch (Exception ex)
            {
                ShowDatabaseError("Chyba při kontrole smazání čtenáře.", ex);
                return false;
            }
        }

        private static void ShowDatabaseError(string message, Exception ex)
        {
            MessageBox.Show(
                $"{message}\n\nDetail chyby: {ex.Message}",
                "Chyba databáze",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
    }
}