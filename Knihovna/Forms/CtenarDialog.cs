using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Knihovna
{
    //akce pro vytvoreni nebo editaci instance ctenare
    public enum ActionType { New, Edit }

    //formular pro vytvoreni nebo editaci ctenare
    public partial class CtenarDialog : Form
    {
        //urcuje jestli se vytvari ctenar nebo edituje ctenar
        public ActionType Action { get; set; } = ActionType.New;

        //vola instanci ctenare pro vytvoreni nebo editaci
        public Ctenar Ctenar { get; set; }

        public CtenarDialog() //konstruktor dialogu
        {
            InitializeComponent();
        }

        //metoda formulare, co vidime
        private void CtenarDialog_VisibleChanged(object sender, EventArgs e)
        {
            switch (Action)
            {
                case ActionType.New:
                    //kdyz chceme vytvorit novou instanci ctenare, tak text. pole jsou prazdna
                    txtJmeno.Text = "";
                    txtPrijmeni.Text = "";
                    txtEmail.Text = "";
                    txtTelCislo.Text = "";
                    break;

                case ActionType.Edit:
                    //kdyz chceme editovat instanci ctenare, tak se zobrazi vyplnene text. pole
                    txtJmeno.Text = Ctenar.Jmeno;
                    txtPrijmeni.Text = Ctenar.Prijmeni;
                    txtEmail.Text = Ctenar.Email;
                    txtTelCislo.Text = Ctenar.TelefonniCislo;
                    break;
            }
        }

        //metoda formulare, po kliknuti na OK
        private void OK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtJmeno.Text) ||
                string.IsNullOrWhiteSpace(txtPrijmeni.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtTelCislo.Text))
            {
                MessageBox.Show("Jméno, příjmení, e-mail a telefonní číslo nesmí být prázdné!");
                return;
            }

            string telCislo = txtTelCislo.Text.Trim();

            if (!telCislo.All(char.IsDigit) || telCislo.Length != 9)
            {
                MessageBox.Show("Telefonní číslo musí mít 9 číslic.");
                return;
            }

            switch (Action)
            {
                case ActionType.New:
                    //vytvori novou instanci s vyplnenymi text. poli
                    Ctenar = new Ctenar(txtJmeno.Text, txtPrijmeni.Text, telCislo, txtEmail.Text);
                    break;

                case ActionType.Edit:
                    //aktualizuje instanci ctenare, kterou editujeme
                    Ctenar.Jmeno = txtJmeno.Text;
                    Ctenar.Prijmeni = txtPrijmeni.Text;
                    Ctenar.Email = txtEmail.Text;
                    Ctenar.TelefonniCislo = telCislo;
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