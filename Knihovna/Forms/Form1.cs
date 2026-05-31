using System;

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

            RefreshDataSources();
        }

        //obnovi datove zdroje po zmene v databazi
        private void RefreshDataSources()
        {
            dgvCtenari.DataSource = null;
            dgvKnihy.DataSource = null;
            dgvRezervovane.DataSource = null;
            dgvVypujcene.DataSource = null;

            dgvCtenari.DataSource = Databaze.Ctenari;
            dgvKnihy.DataSource = Databaze.Knihy;

            RefreshSelectedReaderBooks();
            SetButtons();
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

        //metoda pro nastaveni tlacitek podle dostupnosti v seznamech
        private void SetButtons()
        {
            btnVratit.Enabled = dgvVypujcene.Rows.Count > 0;
            btnZrusit.Enabled = dgvRezervovane.Rows.Count > 0;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Databaze.Serializuj(); //u SQLite verze uz nic neuklada, data se ukladaji prubezne
        }

        private void dgvCtenari_SelectionChanged(object sender, EventArgs e)
        {
            RefreshSelectedReaderBooks();
            SetButtons();
        }

        private void btnVypujcit_Click(object sender, EventArgs e)
        {
            if (dgvCtenari.CurrentRow == null || dgvKnihy.CurrentRow == null) return;

            bool success = Databaze.Vypujcit(
                dgvCtenari.CurrentRow.DataBoundItem,
                dgvKnihy.CurrentRow.DataBoundItem
            );

            if (success)
            {
                RefreshDataSources();
            }

            SetButtons();
        }

        private void btnVratit_Click(object sender, EventArgs e)
        {
            if (dgvCtenari.CurrentRow == null || dgvVypujcene.CurrentRow == null) return;

            Databaze.Vratit(
                dgvCtenari.CurrentRow.DataBoundItem,
                dgvVypujcene.CurrentRow.DataBoundItem
            );

            RefreshDataSources();
            SetButtons();
        }

        private void btnRezervovat_Click(object sender, EventArgs e)
        {
            if (dgvCtenari.CurrentRow == null || dgvKnihy.CurrentRow == null) return;

            bool success = Databaze.Rezervovat(
                dgvCtenari.CurrentRow.DataBoundItem,
                dgvKnihy.CurrentRow.DataBoundItem
            );

            if (success)
            {
                RefreshDataSources();
            }

            SetButtons();
        }

        private void btnZrusit_Click(object sender, EventArgs e)
        {
            if (dgvCtenari.CurrentRow == null || dgvRezervovane.CurrentRow == null) return;

            Databaze.Zrusit(
                dgvCtenari.CurrentRow.DataBoundItem,
                dgvRezervovane.CurrentRow.DataBoundItem
            );

            RefreshDataSources();
            SetButtons();
        }

        private void btnNovyCtenar_Click(object sender, EventArgs e)
        {
            //zobrazeni dialogu pro vytvoreni noveho ctenare 
            using (var dlg = new CtenarDialog())
            {
                dlg.Action = ActionType.New;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Databaze.PridatCtenare(dlg.Ctenar);
                    RefreshDataSources();
                }
            }

            SetButtons();
        }

        private void btnEditaceCtenare_Click(object sender, EventArgs e)
        {
            if (dgvCtenari.CurrentRow == null) return;

            var vybranyCtenar = (Ctenar)dgvCtenari.CurrentRow.DataBoundItem;

            //zobrazeni dialogu pro editaci ctenare
            using (var dlg = new CtenarDialog())
            {
                dlg.Action = ActionType.Edit;
                dlg.Ctenar = vybranyCtenar;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Databaze.UpravitCtenare(dlg.Ctenar);
                    RefreshDataSources();
                }
            }

            SetButtons();
        }

        private void btnNovaKniha_Click(object sender, EventArgs e)
        {
            //zobrazeni dialogu pro vytvoreni nove knihy
            using (var dlg = new KnihaDialog())
            {
                dlg.Action = ActionType1.New;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Databaze.PridatKnihu(dlg.Kniha);
                    RefreshDataSources();
                }
            }

            SetButtons();
        }

        private void btnEditaceKnihy_Click(object sender, EventArgs e)
        {
            if (dgvKnihy.CurrentRow == null) return;

            var vybranaKniha = (Kniha)dgvKnihy.CurrentRow.DataBoundItem;

            //kontrola, jestli muzeme knihu editovat
            if (Databaze.EditovanaKniha(vybranaKniha))
            {
                //zobrazeni dialogu pro editaci knihy
                using (var dlg = new KnihaDialog())
                {
                    dlg.Action = ActionType1.Edit;
                    dlg.Kniha = vybranaKniha;

                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        Databaze.UpravitKnihu(dlg.Kniha);
                        RefreshDataSources();
                    }
                }
            }

            SetButtons();
        }

        private void btnSmazatKnihu_Click(object sender, EventArgs e)
        {
            if (dgvKnihy.CurrentRow == null) return;

            var vybranaKniha = (Kniha)dgvKnihy.CurrentRow.DataBoundItem;

            if (Databaze.SmazatelnaKniha(vybranaKniha))
            {
                bool success = Databaze.SmazatKnihu(vybranaKniha);

                if (success)
                {
                    RefreshDataSources();
                }
            }

            SetButtons();
        }

        private void btnSmazatCtenare_Click(object sender, EventArgs e)
        {
            if (dgvCtenari.CurrentRow == null) return;

            var vybranyCtenar = (Ctenar)dgvCtenari.CurrentRow.DataBoundItem;

            bool success = Databaze.SmazatCtenare(vybranyCtenar);

            if (success)
            {
                RefreshDataSources();
            }

            SetButtons();
        }
    }
}