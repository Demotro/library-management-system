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
        public Kniha Kniha { get; set; }

        public KnihaDialog() //konstruktor dialogu
        {
            InitializeComponent();
        }

        //metoda formulare, co vidime
        private void KnihaDialog_VisibleChanged(object sender, EventArgs e)
        {
            switch (Action)
            {
                case ActionType1.New:
                    //kdyz chceme vytvorit novou instanci knihy, tak textova pole jsou prazdna
                    txtNazev.Text = "";
                    txtAutor.Text = "";
                    txtISBN.Text = "";
                    numRok.Value = 1500;
                    cbStavKnihy.Text = "";
                    break;

                case ActionType1.Edit:
                    if (Kniha == null) return;

                    //kdyz chceme editovat instanci knihy, tak se zobrazi vyplnena textova pole
                    txtNazev.Text = Kniha.Nazev;
                    txtAutor.Text = Kniha.Autor;
                    txtISBN.Text = Kniha.ISBN;
                    numRok.Value = Kniha.RokVydani;

                    //nastavuje stav knihy podle typu knihy
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

            //pro kontrolu odebereme pomlcky a mezery
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

            bool isbnExistuje = false;

            //kontrola, aby pri vytvoreni knihy neslo zadat uz existujici ISBN
            if (Action == ActionType1.New)
            {
                isbnExistuje = Databaze.Knihy.Any(k => k.ISBN == isbn);
            }
            //kontrola, aby pri editaci knihy neslo zadat ISBN jine existujici knihy
            else if (Action == ActionType1.Edit)
            {
                isbnExistuje = Databaze.Knihy.Any(k => k.ISBN == isbn && k.Id != Kniha.Id);
            }

            if (isbnExistuje)
            {
                MessageBox.Show("Toto ISBN již má jiná kniha. Zadej jiné ISBN.");
                return;
            }

            Kniha novaKniha = stav switch
            {
                "Nový" => new NovaKniha(txtNazev.Text.Trim(), txtAutor.Text.Trim(), isbn, (int)numRok.Value),
                "Dobrý" => new DobraKniha(txtNazev.Text.Trim(), txtAutor.Text.Trim(), isbn, (int)numRok.Value),
                "Opotřebovaný" => new OpotrebovanaKniha(txtNazev.Text.Trim(), txtAutor.Text.Trim(), isbn, (int)numRok.Value),
                _ => null
            };

            if (novaKniha == null)
            {
                MessageBox.Show("Nepodařilo se vytvořit knihu.");
                return;
            }

            if (Action == ActionType1.Edit && Kniha != null)
            {
                //zachovani hodnot, ktere se v dialogu nemeni
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