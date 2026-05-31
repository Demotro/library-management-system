using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Knihovna
{
    //akce pro vytvoreni nebo editaci instance knihy
    public enum ActionType1 { New, Edit }

    //formular pro vytvoreni nebo editaci knihy
    public partial class KnihaDialog : Form
    {
        //urcuje jestli se vytvari nebo edituje kniha
        public ActionType1 Action { get; set; } = ActionType1.New;

        //vola instanci knihy pro vytvoreni nebo editaci
        public Kniha Kniha { get; set; } = null!;

        public KnihaDialog()
        {
            InitializeComponent();
        }

        //metoda formulare, co vidime
        private void KnihaDialog_VisibleChanged(object sender, EventArgs e)
        {
            switch (Action)
            {
                case ActionType1.New:
                    txtNazev.Text = "";
                    txtAutor.Text = "";
                    txtISBN.Text = "";
                    numRok.Value = 1500;
                    cbStavKnihy.Text = "";
                    break;

                case ActionType1.Edit:
                    if (Kniha == null) return;

                    txtNazev.Text = Kniha.Nazev;
                    txtAutor.Text = Kniha.Autor;
                    txtISBN.Text = Kniha.ISBN;
                    numRok.Value = Kniha.RokVydani;

                    if (Kniha is NovaKniha) cbStavKnihy.Text = "Nový";
                    else if (Kniha is DobraKniha) cbStavKnihy.Text = "Dobrý";
                    else if (Kniha is OpotrebovanaKniha) cbStavKnihy.Text = "Opotřebovaný";
                    break;
            }
        }

        //metoda formulare, po kliknuti na OK
        private void OK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNazev.Text) ||
                string.IsNullOrWhiteSpace(txtAutor.Text) ||
                string.IsNullOrWhiteSpace(txtISBN.Text))
            {
                MessageBox.Show("Název knihy, autor knihy a ISBN nesmí být prázdné!");
                return;
            }

            string isbn = txtISBN.Text.Trim();

            string isbnPouzeCisla = isbn.Replace("-", "").Replace(" ", "");

            if (!isbnPouzeCisla.All(char.IsDigit) ||
                (isbnPouzeCisla.Length != 10 && isbnPouzeCisla.Length != 13))
            {
                MessageBox.Show("ISBN musí mít 10 nebo 13 číslic.");
                return;
            }

            string stav = cbStavKnihy.Text;

            if ((stav != "Nový" && stav != "Dobrý" && stav != "Opotřebovaný") ||
                string.IsNullOrWhiteSpace(stav))
            {
                MessageBox.Show("Musíš vybrat jeden ze stavů!");
                return;
            }

            if (Action == ActionType1.Edit && Kniha == null)
            {
                MessageBox.Show("Kniha pro editaci nebyla nalezena.");
                return;
            }

            bool isbnExistuje = false;

            if (Action == ActionType1.New)
            {
                isbnExistuje = Databaze.Knihy.Any(k => k.ISBN == isbn);
            }
            else if (Action == ActionType1.Edit)
            {
                isbnExistuje = Databaze.Knihy.Any(k => k.ISBN == isbn && k.Id != Kniha.Id);
            }

            if (isbnExistuje)
            {
                MessageBox.Show("Toto ISBN již má jiná kniha. Zadej jiné ISBN.");
                return;
            }

            Kniha novaKniha;

            if (stav == "Nový")
            {
                novaKniha = new NovaKniha(txtNazev.Text.Trim(), txtAutor.Text.Trim(), isbn, (int)numRok.Value);
            }
            else if (stav == "Dobrý")
            {
                novaKniha = new DobraKniha(txtNazev.Text.Trim(), txtAutor.Text.Trim(), isbn, (int)numRok.Value);
            }
            else if (stav == "Opotřebovaný")
            {
                novaKniha = new OpotrebovanaKniha(txtNazev.Text.Trim(), txtAutor.Text.Trim(), isbn, (int)numRok.Value);
            }
            else
            {
                MessageBox.Show("Nepodařilo se vytvořit knihu.");
                return;
            }

            if (Action == ActionType1.Edit)
            {
                novaKniha.Id = Kniha.Id;
                novaKniha.Dostupnost = Kniha.Dostupnost;
            }

            Kniha = novaKniha;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        //metoda formulare, po kliknuti na CANCEL
        private void Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}