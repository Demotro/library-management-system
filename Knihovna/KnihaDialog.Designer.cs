namespace Knihovna
{
    partial class KnihaDialog
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
            txtNazev = new TextBox();
            txtAutor = new TextBox();
            Cancel = new Button();
            OK = new Button();
            lblNazev = new Label();
            lblAutor = new Label();
            lblRokVydani = new Label();
            numRok = new NumericUpDown();
            txtISBN = new TextBox();
            lblISBN = new Label();
            cbStavKnihy = new ComboBox();
            lblStavKnihy = new Label();
            ((System.ComponentModel.ISupportInitialize)numRok).BeginInit();
            SuspendLayout();
            // 
            // txtNazev
            // 
            txtNazev.Location = new Point(92, 38);
            txtNazev.Name = "txtNazev";
            txtNazev.Size = new Size(145, 23);
            txtNazev.TabIndex = 4;
            // 
            // txtAutor
            // 
            txtAutor.Location = new Point(92, 67);
            txtAutor.Name = "txtAutor";
            txtAutor.Size = new Size(145, 23);
            txtAutor.TabIndex = 5;
            // 
            // Cancel
            // 
            Cancel.DialogResult = DialogResult.Cancel;
            Cancel.Location = new Point(201, 217);
            Cancel.Name = "Cancel";
            Cancel.Size = new Size(75, 23);
            Cancel.TabIndex = 8;
            Cancel.Text = "Cancel";
            Cancel.UseVisualStyleBackColor = true;
            Cancel.Click += Cancel_Click;
            // 
            // OK
            // 
            OK.Location = new Point(22, 217);
            OK.Name = "OK";
            OK.Size = new Size(75, 23);
            OK.TabIndex = 9;
            OK.Text = "OK";
            OK.UseVisualStyleBackColor = true;
            OK.Click += OK_Click;
            // 
            // lblNazev
            // 
            lblNazev.AutoSize = true;
            lblNazev.Location = new Point(44, 41);
            lblNazev.Name = "lblNazev";
            lblNazev.Size = new Size(42, 15);
            lblNazev.TabIndex = 10;
            lblNazev.Text = "Název:";
            // 
            // lblAutor
            // 
            lblAutor.AutoSize = true;
            lblAutor.Location = new Point(46, 70);
            lblAutor.Name = "lblAutor";
            lblAutor.Size = new Size(40, 15);
            lblAutor.TabIndex = 11;
            lblAutor.Text = "Autor:";
            // 
            // lblRokVydani
            // 
            lblRokVydani.AutoSize = true;
            lblRokVydani.Location = new Point(18, 127);
            lblRokVydani.Name = "lblRokVydani";
            lblRokVydani.Size = new Size(68, 15);
            lblRokVydani.TabIndex = 12;
            lblRokVydani.Text = "Rok vydání:";
            // 
            // numRok
            // 
            numRok.Location = new Point(92, 125);
            numRok.Maximum = new decimal(new int[] { 2025, 0, 0, 0 });
            numRok.Minimum = new decimal(new int[] { 1500, 0, 0, 0 });
            numRok.Name = "numRok";
            numRok.Size = new Size(145, 23);
            numRok.TabIndex = 13;
            numRok.Value = new decimal(new int[] { 1500, 0, 0, 0 });
            // 
            // txtISBN
            // 
            txtISBN.Location = new Point(92, 96);
            txtISBN.Name = "txtISBN";
            txtISBN.Size = new Size(145, 23);
            txtISBN.TabIndex = 14;
            // 
            // lblISBN
            // 
            lblISBN.AutoSize = true;
            lblISBN.Location = new Point(51, 99);
            lblISBN.Name = "lblISBN";
            lblISBN.Size = new Size(35, 15);
            lblISBN.TabIndex = 15;
            lblISBN.Text = "ISBN:";
            // 
            // cbStavKnihy
            // 
            cbStavKnihy.FormattingEnabled = true;
            cbStavKnihy.Items.AddRange(new object[] { "Nový", "Dobrý", "Opotřebovaný" });
            cbStavKnihy.Location = new Point(92, 154);
            cbStavKnihy.Name = "cbStavKnihy";
            cbStavKnihy.Size = new Size(145, 23);
            cbStavKnihy.TabIndex = 16;
            // 
            // lblStavKnihy
            // 
            lblStavKnihy.AutoSize = true;
            lblStavKnihy.Location = new Point(22, 157);
            lblStavKnihy.Name = "lblStavKnihy";
            lblStavKnihy.Size = new Size(64, 15);
            lblStavKnihy.TabIndex = 17;
            lblStavKnihy.Text = "Stav knihy:";
            // 
            // KnihaDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(308, 295);
            Controls.Add(lblStavKnihy);
            Controls.Add(cbStavKnihy);
            Controls.Add(lblISBN);
            Controls.Add(txtISBN);
            Controls.Add(numRok);
            Controls.Add(lblRokVydani);
            Controls.Add(lblAutor);
            Controls.Add(lblNazev);
            Controls.Add(OK);
            Controls.Add(Cancel);
            Controls.Add(txtAutor);
            Controls.Add(txtNazev);
            Name = "KnihaDialog";
            Text = "KnihaDialog";
            VisibleChanged += KnihaDialog_VisibleChanged;
            ((System.ComponentModel.ISupportInitialize)numRok).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtNazev;
        private TextBox txtAutor;
        private Button Cancel;
        private Button OK;
        private Label lblNazev;
        private Label lblAutor;
        private Label lblRokVydani;
        private NumericUpDown numRok;
        private TextBox txtISBN;
        private Label lblISBN;
        private ComboBox cbStavKnihy;
        private Label lblStavKnihy;
    }
}