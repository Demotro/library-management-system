namespace Knihovna
{
    partial class CtenarDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblJmeno = new Label();
            lblPrijmeni = new Label();
            lblEmail = new Label();
            txtJmeno = new TextBox();
            txtPrijmeni = new TextBox();
            txtEmail = new TextBox();
            OK = new Button();
            Cancel = new Button();
            txtTelCislo = new TextBox();
            lblTelefonniCislo = new Label();
            SuspendLayout();
            // 
            // lblJmeno
            // 
            lblJmeno.AutoSize = true;
            lblJmeno.Location = new Point(52, 50);
            lblJmeno.Name = "lblJmeno";
            lblJmeno.Size = new Size(45, 15);
            lblJmeno.TabIndex = 0;
            lblJmeno.Text = "Jméno:";
            // 
            // lblPrijmeni
            // 
            lblPrijmeni.AutoSize = true;
            lblPrijmeni.Location = new Point(43, 79);
            lblPrijmeni.Name = "lblPrijmeni";
            lblPrijmeni.Size = new Size(54, 15);
            lblPrijmeni.TabIndex = 1;
            lblPrijmeni.Text = "Příjmení:";
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(53, 108);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(44, 15);
            lblEmail.TabIndex = 2;
            lblEmail.Text = "E-mail:";
            // 
            // txtJmeno
            // 
            txtJmeno.Location = new Point(103, 47);
            txtJmeno.Name = "txtJmeno";
            txtJmeno.Size = new Size(148, 23);
            txtJmeno.TabIndex = 3;
            // 
            // txtPrijmeni
            // 
            txtPrijmeni.Location = new Point(103, 76);
            txtPrijmeni.Name = "txtPrijmeni";
            txtPrijmeni.Size = new Size(148, 23);
            txtPrijmeni.TabIndex = 4;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(103, 105);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(148, 23);
            txtEmail.TabIndex = 5;
            // 
            // OK
            // 
            OK.Location = new Point(43, 196);
            OK.Name = "OK";
            OK.Size = new Size(75, 23);
            OK.TabIndex = 6;
            OK.Text = "OK";
            OK.UseVisualStyleBackColor = true;
            OK.Click += OK_Click;
            // 
            // Cancel
            // 
            Cancel.DialogResult = DialogResult.Cancel;
            Cancel.Location = new Point(209, 196);
            Cancel.Name = "Cancel";
            Cancel.Size = new Size(75, 23);
            Cancel.TabIndex = 7;
            Cancel.Text = "Cancel";
            Cancel.UseVisualStyleBackColor = true;
            Cancel.Click += Cancel_Click;
            // 
            // txtTelCislo
            // 
            txtTelCislo.Location = new Point(103, 134);
            txtTelCislo.Name = "txtTelCislo";
            txtTelCislo.Size = new Size(148, 23);
            txtTelCislo.TabIndex = 8;
            // 
            // lblTelefonniCislo
            // 
            lblTelefonniCislo.AutoSize = true;
            lblTelefonniCislo.Location = new Point(12, 137);
            lblTelefonniCislo.Name = "lblTelefonniCislo";
            lblTelefonniCislo.Size = new Size(85, 15);
            lblTelefonniCislo.TabIndex = 9;
            lblTelefonniCislo.Text = "Telefonní číslo:";
            // 
            // CtenarDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(321, 274);
            Controls.Add(lblTelefonniCislo);
            Controls.Add(txtTelCislo);
            Controls.Add(Cancel);
            Controls.Add(OK);
            Controls.Add(txtEmail);
            Controls.Add(txtPrijmeni);
            Controls.Add(txtJmeno);
            Controls.Add(lblEmail);
            Controls.Add(lblPrijmeni);
            Controls.Add(lblJmeno);
            Name = "CtenarDialog";
            Text = "CtenarDialog";
            VisibleChanged += CtenarDialog_VisibleChanged;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblJmeno;
        private Label lblPrijmeni;
        private Label lblEmail;
        private TextBox txtJmeno;
        private TextBox txtPrijmeni;
        private TextBox txtEmail;
        private Button OK;
        private Button Cancel;
        private TextBox txtTelCislo;
        private Label lblTelefonniCislo;
    }
}