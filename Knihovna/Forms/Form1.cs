using System;
using System.ComponentModel;
using System.Linq;

namespace Knihovna
{
    public partial class Form1 : Form
    {
        //konstruktor formulare
        public Form1()
        {
            InitializeComponent();

            dgvKnihy.AutoGenerateColumns = false;
            dgvCtenari.AutoGenerateColumns = false;

            dgvKnihy.SelectionChanged += dgvKnihy_SelectionChanged;
            dgvVypujcene.SelectionChanged += dgvVypujcene_SelectionChanged;
            dgvRezervovane.SelectionChanged += dgvRezervovane_SelectionChanged;

            if (cbFiltrDostupnosti.Items.Count > 0 && cbFiltrDostupnosti.SelectedIndex == -1)
            {
                cbFiltrDostupnosti.SelectedIndex = 0;
            }

            RefreshDataSources();
        }

        //obnovi datove zdroje po zmene v databazi
        private void RefreshDataSources(int? selectedCtenarId = null, int? selectedKnihaId = null)
        {
            dgvCtenari.DataSource = null;
            dgvKnihy.DataSource = null;
            dgvRezervovane.DataSource = null;
            dgvVypujcene.DataSource = null;

            ApplyReaderSearchFilter();
            ApplyBookSearchFilter();

            if (selectedCtenarId.HasValue)
            {
                SelectCtenarById(selectedCtenarId.Value);
            }

            if (selectedKnihaId.HasValue)
            {
                SelectKnihaById(selectedKnihaId.Value);
            }

            RefreshSelectedReaderBooks();
            SetButtons();
        }

        //aplikuje vyhledavani knih podle nazvu, autora nebo ISBN a filtr podle dostupnosti
        private void ApplyBookSearchFilter()
        {
            string searchText = txtHledatKnihu.Text.Trim().ToLower();
            string selectedFilter = cbFiltrDostupnosti.SelectedItem?.ToString() ?? "Všechny";

            var filteredBooks = Databaze.Knihy.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                filteredBooks = filteredBooks.Where(k =>
                    k.Nazev.ToLower().Contains(searchText) ||
                    k.Autor.ToLower().Contains(searchText) ||
                    k.ISBN.ToLower().Contains(searchText));
            }

            if (selectedFilter == "Dostupné")
            {
                filteredBooks = filteredBooks.Where(k => k.Dostupnost == true);
            }
            else if (selectedFilter == "Vypùjèené")
            {
                filteredBooks = filteredBooks.Where(k => k.Dostupnost == false);
            }

            dgvKnihy.DataSource = new BindingList<Kniha>(filteredBooks.ToList());
        }

        //aplikuje vyhledavani ctenaru podle jmena, prijmeni, e-mailu nebo telefonu
        private void ApplyReaderSearchFilter()
        {
            string searchText = txtHledatCtenare.Text.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                dgvCtenari.DataSource = Databaze.Ctenari;
                return;
            }

            var filteredReaders = Databaze.Ctenari
                .Where(c =>
                    c.Jmeno.ToLower().Contains(searchText) ||
                    c.Prijmeni.ToLower().Contains(searchText) ||
                    c.Email.ToLower().Contains(searchText) ||
                    c.TelefonniCislo.ToLower().Contains(searchText))
                .ToList();

            dgvCtenari.DataSource = new BindingList<Ctenar>(filteredReaders);
        }

        //vrati ID aktualne vybraneho ctenare
        private int? GetSelectedCtenarId()
        {
            if (dgvCtenari.CurrentRow == null) return null;

            var ctenar = dgvCtenari.CurrentRow.DataBoundItem as Ctenar;

            if (ctenar == null) return null;

            return ctenar.Id;
        }

        //vrati ID aktualne vybrane knihy
        private int? GetSelectedKnihaId()
        {
            if (dgvKnihy.CurrentRow == null) return null;

            var kniha = dgvKnihy.CurrentRow.DataBoundItem as Kniha;

            if (kniha == null) return null;

            return kniha.Id;
        }

        //znovu vybere ctenare podle ID
        private void SelectCtenarById(int ctenarId)
        {
            dgvCtenari.ClearSelection();

            foreach (DataGridViewRow row in dgvCtenari.Rows)
            {
                var ctenar = row.DataBoundItem as Ctenar;

                if (ctenar != null && ctenar.Id == ctenarId)
                {
                    row.Selected = true;
                    dgvCtenari.CurrentCell = row.Cells[0];
                    return;
                }
            }
        }

        //znovu vybere knihu podle ID
        private void SelectKnihaById(int knihaId)
        {
            dgvKnihy.ClearSelection();

            foreach (DataGridViewRow row in dgvKnihy.Rows)
            {
                var kniha = row.DataBoundItem as Kniha;

                if (kniha != null && kniha.Id == knihaId)
                {
                    row.Selected = true;
                    dgvKnihy.CurrentCell = row.Cells[0];
                    return;
                }
            }
        }

        //obnovi vypujcene a rezervovane knihy vybraneho ctenare
        private void RefreshSelectedReaderBooks()
        {
            if (dgvCtenari.CurrentRow == null)
            {
                dgvRezervovane.DataSource = null;
                dgvVypujcene.DataSource = null;
                return;
            }

            var vybranyCtenar = dgvCtenari.CurrentRow.DataBoundItem as Ctenar;

            if (vybranyCtenar == null)
            {
                dgvRezervovane.DataSource = null;
                dgvVypujcene.DataSource = null;
                return;
            }

            dgvRezervovane.DataSource = vybranyCtenar.Rezervovano;
            dgvVypujcene.DataSource = vybranyCtenar.Vypujcene;
        }

        //metoda pro nastaveni tlacitek podle aktualniho vyberu
        private void SetButtons()
        {
            bool jeVybranyCtenar = dgvCtenari.CurrentRow != null &&
                                   dgvCtenari.CurrentRow.DataBoundItem is Ctenar;

            bool jeVybranaKniha = dgvKnihy.CurrentRow != null &&
                                  dgvKnihy.CurrentRow.DataBoundItem is Kniha;

            bool jeVybranaVypujcenaKniha = dgvVypujcene.CurrentRow != null &&
                                           dgvVypujcene.CurrentRow.DataBoundItem is Kniha;

            bool jeVybranaRezervovanaKniha = dgvRezervovane.CurrentRow != null &&
                                             dgvRezervovane.CurrentRow.DataBoundItem is Kniha;

            btnEditaceCtenare.Enabled = jeVybranyCtenar;
            btnSmazatCtenare.Enabled = jeVybranyCtenar;

            btnEditaceKnihy.Enabled = jeVybranaKniha;
            btnSmazatKnihu.Enabled = jeVybranaKniha;

            btnVypujcit.Enabled = jeVybranyCtenar && jeVybranaKniha;
            btnRezervovat.Enabled = jeVybranyCtenar && jeVybranaKniha;

            btnVratit.Enabled = jeVybranyCtenar && jeVybranaVypujcenaKniha;
            btnZrusit.Enabled = jeVybranyCtenar && jeVybranaRezervovanaKniha;
        }

        private void dgvCtenari_SelectionChanged(object sender, EventArgs e)
        {
            RefreshSelectedReaderBooks();
            SetButtons();
        }

        private void dgvKnihy_SelectionChanged(object? sender, EventArgs e)
        {
            SetButtons();
        }

        private void dgvVypujcene_SelectionChanged(object? sender, EventArgs e)
        {
            SetButtons();
        }

        private void dgvRezervovane_SelectionChanged(object? sender, EventArgs e)
        {
            SetButtons();
        }

        private void btnVypujcit_Click(object sender, EventArgs e)
        {
            if (dgvCtenari.CurrentRow == null || dgvKnihy.CurrentRow == null) return;

            int? selectedCtenarId = GetSelectedCtenarId();
            int? selectedKnihaId = GetSelectedKnihaId();

            bool success = Databaze.Vypujcit(
                dgvCtenari.CurrentRow.DataBoundItem,
                dgvKnihy.CurrentRow.DataBoundItem
            );

            if (success)
            {
                RefreshDataSources(selectedCtenarId, selectedKnihaId);
            }

            SetButtons();
        }

        private void btnVratit_Click(object sender, EventArgs e)
        {
            if (dgvCtenari.CurrentRow == null || dgvVypujcene.CurrentRow == null) return;

            int? selectedCtenarId = GetSelectedCtenarId();
            int? selectedKnihaId = GetSelectedKnihaId();

            var vypujcenaKniha = dgvVypujcene.CurrentRow.DataBoundItem as Kniha;

            if (vypujcenaKniha == null) return;

            DialogResult potvrzeni = MessageBox.Show(
                $"Opravdu chcete vrátit knihu \"{vypujcenaKniha.Nazev}\"?",
                "Potvrzení vrácení knihy",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (potvrzeni != DialogResult.Yes)
            {
                SetButtons();
                return;
            }

            Databaze.Vratit(
                dgvCtenari.CurrentRow.DataBoundItem,
                dgvVypujcene.CurrentRow.DataBoundItem
            );

            RefreshDataSources(selectedCtenarId, selectedKnihaId);
            SetButtons();
        }

        private void btnRezervovat_Click(object sender, EventArgs e)
        {
            if (dgvCtenari.CurrentRow == null || dgvKnihy.CurrentRow == null) return;

            int? selectedCtenarId = GetSelectedCtenarId();
            int? selectedKnihaId = GetSelectedKnihaId();

            bool success = Databaze.Rezervovat(
                dgvCtenari.CurrentRow.DataBoundItem,
                dgvKnihy.CurrentRow.DataBoundItem
            );

            if (success)
            {
                RefreshDataSources(selectedCtenarId, selectedKnihaId);
            }

            SetButtons();
        }

        private void btnZrusit_Click(object sender, EventArgs e)
        {
            if (dgvCtenari.CurrentRow == null || dgvRezervovane.CurrentRow == null) return;

            int? selectedCtenarId = GetSelectedCtenarId();
            int? selectedKnihaId = GetSelectedKnihaId();

            var rezervovanaKniha = dgvRezervovane.CurrentRow.DataBoundItem as Kniha;

            if (rezervovanaKniha == null) return;

            DialogResult potvrzeni = MessageBox.Show(
                $"Opravdu chcete zrušit rezervaci knihy \"{rezervovanaKniha.Nazev}\"?",
                "Potvrzení zrušení rezervace",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (potvrzeni != DialogResult.Yes)
            {
                SetButtons();
                return;
            }

            Databaze.Zrusit(
                dgvCtenari.CurrentRow.DataBoundItem,
                dgvRezervovane.CurrentRow.DataBoundItem
            );

            RefreshDataSources(selectedCtenarId, selectedKnihaId);
            SetButtons();
        }

        private void btnNovyCtenar_Click(object sender, EventArgs e)
        {
            using (var dlg = new CtenarDialog())
            {
                dlg.Action = ActionType.New;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Databaze.PridatCtenare(dlg.Ctenar);
                    RefreshDataSources(dlg.Ctenar.Id, GetSelectedKnihaId());
                }
            }

            SetButtons();
        }

        private void btnEditaceCtenare_Click(object sender, EventArgs e)
        {
            if (dgvCtenari.CurrentRow == null) return;

            var vybranyCtenar = (Ctenar)dgvCtenari.CurrentRow.DataBoundItem;

            using (var dlg = new CtenarDialog())
            {
                dlg.Action = ActionType.Edit;
                dlg.Ctenar = vybranyCtenar;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Databaze.UpravitCtenare(dlg.Ctenar);
                    RefreshDataSources(dlg.Ctenar.Id, GetSelectedKnihaId());
                }
            }

            SetButtons();
        }

        private void btnNovaKniha_Click(object sender, EventArgs e)
        {
            using (var dlg = new KnihaDialog())
            {
                dlg.Action = ActionType1.New;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Databaze.PridatKnihu(dlg.Kniha);
                    RefreshDataSources(GetSelectedCtenarId(), dlg.Kniha.Id);
                }
            }

            SetButtons();
        }

        private void btnEditaceKnihy_Click(object sender, EventArgs e)
        {
            if (dgvKnihy.CurrentRow == null) return;

            var vybranaKniha = (Kniha)dgvKnihy.CurrentRow.DataBoundItem;

            if (Databaze.EditovanaKniha(vybranaKniha))
            {
                using (var dlg = new KnihaDialog())
                {
                    dlg.Action = ActionType1.Edit;
                    dlg.Kniha = vybranaKniha;

                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        Databaze.UpravitKnihu(dlg.Kniha);
                        RefreshDataSources(GetSelectedCtenarId(), dlg.Kniha.Id);
                    }
                }
            }

            SetButtons();
        }

        private void btnSmazatKnihu_Click(object sender, EventArgs e)
        {
            if (dgvKnihy.CurrentRow == null) return;

            int? selectedCtenarId = GetSelectedCtenarId();

            var vybranaKniha = (Kniha)dgvKnihy.CurrentRow.DataBoundItem;

            if (!Databaze.SmazatelnaKniha(vybranaKniha))
            {
                SetButtons();
                return;
            }

            DialogResult potvrzeni = MessageBox.Show(
                $"Opravdu chcete smazat knihu \"{vybranaKniha.Nazev}\"?",
                "Potvrzení smazání",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (potvrzeni != DialogResult.Yes)
            {
                SetButtons();
                return;
            }

            bool success = Databaze.SmazatKnihu(vybranaKniha);

            if (success)
            {
                RefreshDataSources(selectedCtenarId, null);
            }

            SetButtons();
        }

        private void btnSmazatCtenare_Click(object sender, EventArgs e)
        {
            if (dgvCtenari.CurrentRow == null) return;

            int? selectedKnihaId = GetSelectedKnihaId();

            var vybranyCtenar = (Ctenar)dgvCtenari.CurrentRow.DataBoundItem;

            if (!Databaze.SmazatelnyCtenar(vybranyCtenar))
            {
                SetButtons();
                return;
            }

            DialogResult potvrzeni = MessageBox.Show(
                $"Opravdu chcete smazat ètenáøe {vybranyCtenar.Jmeno} {vybranyCtenar.Prijmeni}?",
                "Potvrzení smazání",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (potvrzeni != DialogResult.Yes)
            {
                SetButtons();
                return;
            }

            bool success = Databaze.SmazatCtenare(vybranyCtenar);

            if (success)
            {
                RefreshDataSources(null, selectedKnihaId);
            }

            SetButtons();
        }

        private void txtHledatKnihu_TextChanged(object sender, EventArgs e)
        {
            int? selectedCtenarId = GetSelectedCtenarId();
            int? selectedKnihaId = GetSelectedKnihaId();

            ApplyBookSearchFilter();

            if (selectedCtenarId.HasValue)
            {
                SelectCtenarById(selectedCtenarId.Value);
            }

            if (selectedKnihaId.HasValue)
            {
                SelectKnihaById(selectedKnihaId.Value);
            }

            RefreshSelectedReaderBooks();
            SetButtons();
        }

        private void btnVymazatHledaniKnih_Click(object sender, EventArgs e)
        {
            int? selectedCtenarId = GetSelectedCtenarId();
            int? selectedKnihaId = GetSelectedKnihaId();

            txtHledatKnihu.Text = "";
            ApplyBookSearchFilter();

            if (selectedCtenarId.HasValue)
            {
                SelectCtenarById(selectedCtenarId.Value);
            }

            if (selectedKnihaId.HasValue)
            {
                SelectKnihaById(selectedKnihaId.Value);
            }

            RefreshSelectedReaderBooks();
            SetButtons();
        }

        private void txtHledatCtenare_TextChanged(object sender, EventArgs e)
        {
            int? selectedKnihaId = GetSelectedKnihaId();

            ApplyReaderSearchFilter();

            RefreshSelectedReaderBooks();
            SetButtons();

            if (selectedKnihaId.HasValue)
            {
                SelectKnihaById(selectedKnihaId.Value);
            }
        }

        private void btnVymazatHledaniCtenaru_Click(object sender, EventArgs e)
        {
            txtHledatCtenare.Text = "";
            ApplyReaderSearchFilter();
            RefreshSelectedReaderBooks();
            SetButtons();
        }

        private void cbFiltrDostupnosti_SelectedIndexChanged(object sender, EventArgs e)
        {
            int? selectedCtenarId = GetSelectedCtenarId();
            int? selectedKnihaId = GetSelectedKnihaId();

            ApplyBookSearchFilter();

            if (selectedCtenarId.HasValue)
            {
                SelectCtenarById(selectedCtenarId.Value);
            }

            if (selectedKnihaId.HasValue)
            {
                SelectKnihaById(selectedKnihaId.Value);
            }

            RefreshSelectedReaderBooks();
            SetButtons();
        }
    }
}