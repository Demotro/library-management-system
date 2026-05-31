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

            //pripojuje datove zdroje pro zobrazeni ctenaru a knih
            dgvCtenari.DataSource = Databaze.Ctenari;
            dgvKnihy.DataSource = Databaze.Knihy;
        }

        //metoda pro nastaveni tlacitek podle dostupnosti v seznamech
        private void SetButtons()
        {
            btnVratit.Enabled = dgvVypujcene.Rows.Count > 0;
            btnZrusit.Enabled = dgvRezervovane.Rows.Count > 0;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Databaze.Serializuj(); //ulozi data, kdyz zavreme formular
        }

        private void dgvCtenari_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCtenari.CurrentRow == null) return;
            //nastavuje datove zdroje pro rezervovane a vypujcene knihy
            var vybranyCtenar = (Ctenar)dgvCtenari.CurrentRow.DataBoundItem;

            dgvRezervovane.DataSource = vybranyCtenar.Rezervovano;
            dgvVypujcene.DataSource = vybranyCtenar.Vypujcene;

            SetButtons();
        }

        private void btnVypujcit_Click(object sender, EventArgs e)
        {
            if (dgvCtenari.CurrentRow == null || dgvKnihy.CurrentRow == null) return;
            //volani metody pro vypujceni knihy
            Databaze.Vypujcit(dgvCtenari.CurrentRow.DataBoundItem,
                dgvKnihy.CurrentRow.DataBoundItem);
            //obnoveni datovych zdroju knih
            dgvKnihy.DataSource = null;
            dgvKnihy.DataSource = Databaze.Knihy;
            SetButtons();
        }

        private void btnVratit_Click(object sender, EventArgs e)
        {
            if (dgvCtenari.CurrentRow == null || dgvVypujcene.CurrentRow == null) return;
            //volani metody pro vraceni knihy
            Databaze.Vratit(
                dgvCtenari.CurrentRow.DataBoundItem,
                dgvVypujcene.CurrentRow.DataBoundItem
            );
            //obnoveni datovych zdroju knih
            dgvKnihy.DataSource = null;
            dgvKnihy.DataSource = Databaze.Knihy;
            SetButtons();
        }

        private void btnRezervovat_Click(object sender, EventArgs e)
        {   //volani metody pro rezervaci knihy
            if (dgvCtenari.CurrentRow == null || dgvKnihy.CurrentRow == null) return;

            Databaze.Rezervovat(
                dgvCtenari.CurrentRow.DataBoundItem,
                dgvKnihy.CurrentRow.DataBoundItem
            );

            SetButtons();
        }

        private void btnZrusit_Click(object sender, EventArgs e)
        {   //volani metody pro zruceni rezervace knihy
            if (dgvCtenari.CurrentRow == null || dgvRezervovane.CurrentRow == null) return;

            Databaze.Zrusit(
                dgvCtenari.CurrentRow.DataBoundItem,
                dgvRezervovane.CurrentRow.DataBoundItem
            );

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
                    Databaze.Ctenari.Add(dlg.Ctenar);
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
                dlg.ShowDialog();
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
                    Databaze.Knihy.Add(dlg.Kniha);
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
                    dlg.ShowDialog();
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
                Databaze.SmazatKnihu(vybranaKniha);
            }

            SetButtons();
        }

        private void btnSmazatCtenare_Click(object sender, EventArgs e)
        {
            if (dgvCtenari.CurrentRow == null) return;

            var vybranyCtenar = (Ctenar)dgvCtenari.CurrentRow.DataBoundItem;
            //volani metody pro smazani ctenare
            Databaze.SmazatCtenare(vybranyCtenar);
            //musime obnovit datove zdroje, jelikoz se muze stat, ze smazeme
            //ctenare, ktery ma vypujcenou knihu
            dgvKnihy.DataSource = null;
            dgvKnihy.DataSource = Databaze.Knihy;
            SetButtons();
        }
    }
}
