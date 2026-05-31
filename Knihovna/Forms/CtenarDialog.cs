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
        public Ctenar Ctenar { get; set; } = null!;

        public CtenarDialog()
        {
            InitializeComponent();
        }

        //metoda formulare, co vidime
        private void CtenarDialog_VisibleChanged(object sender, EventArgs e)
        {
            switch (Action)
            {
                case ActionType.New:
                    txtJmeno.Text = "";
                    txtPrijmeni.Text = "";
                    txtEmail.Text = "";
                    txtTelCislo.Text = "";
                    break;

                case ActionType.Edit:
                    if (Ctenar == null) return;

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

            if (Action == ActionType.Edit && Ctenar == null)
            {
                MessageBox.Show("Čtenář pro editaci nebyl nalezen.");
                return;
            }

            string telCislo = txtTelCislo.Text.Trim();

            if (!telCislo.All(char.IsDigit) || telCislo.Length != 9)
            {
                MessageBox.Show("Telefonní číslo musí mít 9 číslic.");
                return;
            }

            Ctenar novyCtenar = new Ctenar(
                txtJmeno.Text.Trim(),
                txtPrijmeni.Text.Trim(),
                telCislo,
                txtEmail.Text.Trim()
            );

            if (Action == ActionType.Edit)
            {
                novyCtenar.Id = Ctenar.Id;
                novyCtenar.Vypujcene = Ctenar.Vypujcene;
                novyCtenar.Rezervovano = Ctenar.Rezervovano;
            }

            Ctenar = novyCtenar;

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