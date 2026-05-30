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
{   //akce pro vytvoreni nebo editaci instance knihy
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
                    //kdyz chceme vytvorit novou instanci knihy, tak text. pole jsou prazdna
                    txtNazev.Text = "";
                    txtAutor.Text = "";
                    txtISBN.Text = "";
                    numRok.Value = 1500;//nastavi zakladni rok
                    break;
                case ActionType1.Edit:
                    //Kdyz chceme editovat instanci knihy, tak se zobrazi vyplnene text. pole
                    txtNazev.Text = Kniha.Nazev;
                    txtAutor.Text = Kniha.Autor;
                    txtISBN.Text = Kniha.ISBN.ToString();
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
            if (string.IsNullOrWhiteSpace(txtNazev.Text) || string.IsNullOrWhiteSpace(txtAutor.Text)
                || string.IsNullOrWhiteSpace(txtISBN.Text))
            { MessageBox.Show("Název knihy//Autor knihy/ISBN nesmí být prázdný!"); return; }

            if (!int.TryParse(txtISBN.Text, out int isbn) || isbn <= 0)
            { MessageBox.Show("ISBN musí být kladné číslo!"); return; }

            string stav = cbStavKnihy.Text;
            if (stav != "Nový" && stav != "Dobrý" && stav != "Opotřebovaný" || string.IsNullOrWhiteSpace(stav))
            { MessageBox.Show("Musíš vybrat jeden ze stavů!"); return; }

            bool isbnExistuje = false;
            //kontrola, aby nenastalo, ze pri vytvoreni knihy nezadame uz existujici ISBN
            if (Action == ActionType1.New)
            { isbnExistuje = Databaze.Knihy.Any(k => k.ISBN == isbn); }
            //kontrola, aby nenastalo, ze pri editace knihy nezadame uz existujici ISBN, nebo to same ISBN
            else if (Action == ActionType1.Edit)
            { isbnExistuje = Databaze.Knihy.Any(k => k.ISBN == isbn && k != Kniha); }
            if (isbnExistuje) //jestli existuje tak vyhodi tuto zpravu
            { MessageBox.Show("Toto ISBN již má jiná kniha. Zadej jiný ISBN."); return; }

            switch (Action)
            {
                case ActionType1.New:
                    //vytvori novou instanci knihy podle stavu a vyplnenymi text. poli
                    Kniha = stav switch
                    {
                        "Nový" => new NovaKniha(txtNazev.Text, txtAutor.Text, isbn, (int)numRok.Value),
                        "Dobrý" => new DobraKniha(txtNazev.Text, txtAutor.Text, isbn, (int)numRok.Value),
                        "Opotřebovaný" => new OpotrebovanaKniha(txtNazev.Text, txtAutor.Text, isbn, (int)numRok.Value),
                    };
                    break;
                case ActionType1.Edit:
                    //vytvori nove instanci knihy podle stavu a vyplnenymi text. poli
                    Kniha newKniha = stav switch
                    {
                        "Nový" => new NovaKniha(txtNazev.Text, txtAutor.Text, isbn, (int)numRok.Value),
                        "Dobrý" => new DobraKniha(txtNazev.Text, txtAutor.Text, isbn, (int)numRok.Value),
                        "Opotřebovaný" => new OpotrebovanaKniha(txtNazev.Text, txtAutor.Text, isbn, (int)numRok.Value),
                    };

                    var index = Databaze.Knihy.IndexOf(Kniha); //najde index knihy
                    if (index >= 0)
                    {
                        Databaze.Knihy[index] = newKniha; //nahradi starou instanci knihy novou
                        Kniha = newKniha; //kniha ukazuje na novou instanci
                    }
                    break;
            }
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

