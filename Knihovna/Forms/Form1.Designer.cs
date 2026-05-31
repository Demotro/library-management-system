namespace Knihovna
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dgvCtenari = new DataGridView();
            Jméno = new DataGridViewTextBoxColumn();
            Příjmení = new DataGridViewTextBoxColumn();
            Email = new DataGridViewTextBoxColumn();
            TelefonniCislo = new DataGridViewTextBoxColumn();
            lblCtenari = new Label();
            dgvKnihy = new DataGridView();
            Nazev = new DataGridViewTextBoxColumn();
            Autor = new DataGridViewTextBoxColumn();
            Dostupnost = new DataGridViewCheckBoxColumn();
            ISBN = new DataGridViewTextBoxColumn();
            RokVydani = new DataGridViewTextBoxColumn();
            StavKnihy = new DataGridViewTextBoxColumn();
            lblSeznamKnih = new Label();
            btnVypujcit = new Button();
            btnVratit = new Button();
            btnNovyCtenar = new Button();
            btnEditaceCtenare = new Button();
            dgvVypujcene = new DataGridView();
            Nazev1 = new DataGridViewTextBoxColumn();
            Autor1 = new DataGridViewTextBoxColumn();
            Dostupnost1 = new DataGridViewCheckBoxColumn();
            ISBN1 = new DataGridViewTextBoxColumn();
            RokVydani1 = new DataGridViewTextBoxColumn();
            StavKnihy1 = new DataGridViewTextBoxColumn();
            lblVypujceneKnihy = new Label();
            btnNovaKniha = new Button();
            btnEditaceKnihy = new Button();
            dgvRezervovane = new DataGridView();
            Nazev2 = new DataGridViewTextBoxColumn();
            Autor2 = new DataGridViewTextBoxColumn();
            Dostupnost2 = new DataGridViewCheckBoxColumn();
            ISBN2 = new DataGridViewTextBoxColumn();
            RokVydani2 = new DataGridViewTextBoxColumn();
            StavKnihy2 = new DataGridViewTextBoxColumn();
            btnRezervovat = new Button();
            btnZrusit = new Button();
            btnSmazatKnihu = new Button();
            btnSmazatCtenare = new Button();
            lblRezervovaneKnihy = new Label();
            lblHledatKnihu = new Label();
            txtHledatKnihu = new TextBox();
            btnVymazatHledaniKnih = new Button();
            lblHledatCtenare = new Label();
            txtHledatCtenare = new TextBox();
            btnVymazatHledaniCtenaru = new Button();
            cbFiltrDostupnosti = new ComboBox();
            lblFiltrDostupnosti = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvCtenari).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvKnihy).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvVypujcene).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvRezervovane).BeginInit();
            SuspendLayout();
            // 
            // dgvCtenari
            // 
            dgvCtenari.AllowUserToAddRows = false;
            dgvCtenari.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCtenari.Columns.AddRange(new DataGridViewColumn[] { Jméno, Příjmení, Email, TelefonniCislo });
            dgvCtenari.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvCtenari.Location = new Point(12, 45);
            dgvCtenari.MultiSelect = false;
            dgvCtenari.Name = "dgvCtenari";
            dgvCtenari.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCtenari.Size = new Size(518, 253);
            dgvCtenari.TabIndex = 0;
            dgvCtenari.SelectionChanged += dgvCtenari_SelectionChanged;
            // 
            // Jméno
            // 
            Jméno.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Jméno.DataPropertyName = "Jmeno";
            Jméno.HeaderText = "Jméno";
            Jméno.Name = "Jméno";
            Jméno.Width = 67;
            // 
            // Příjmení
            // 
            Příjmení.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Příjmení.DataPropertyName = "Prijmeni";
            Příjmení.HeaderText = "Příjmení";
            Příjmení.Name = "Příjmení";
            Příjmení.Width = 76;
            // 
            // Email
            // 
            Email.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Email.DataPropertyName = "Email";
            Email.HeaderText = "E-mail";
            Email.Name = "Email";
            Email.Width = 66;
            // 
            // TelefonniCislo
            // 
            TelefonniCislo.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            TelefonniCislo.DataPropertyName = "TelefonniCislo";
            TelefonniCislo.HeaderText = "Telefonní číslo";
            TelefonniCislo.Name = "TelefonniCislo";
            TelefonniCislo.Width = 108;
            // 
            // lblCtenari
            // 
            lblCtenari.AutoSize = true;
            lblCtenari.Location = new Point(12, 19);
            lblCtenari.Name = "lblCtenari";
            lblCtenari.Size = new Size(94, 15);
            lblCtenari.TabIndex = 1;
            lblCtenari.Text = "Seznam čtenářů:";
            // 
            // dgvKnihy
            // 
            dgvKnihy.AllowUserToAddRows = false;
            dgvKnihy.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvKnihy.Columns.AddRange(new DataGridViewColumn[] { Nazev, Autor, Dostupnost, ISBN, RokVydani, StavKnihy });
            dgvKnihy.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvKnihy.Location = new Point(544, 45);
            dgvKnihy.MultiSelect = false;
            dgvKnihy.Name = "dgvKnihy";
            dgvKnihy.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvKnihy.Size = new Size(518, 253);
            dgvKnihy.TabIndex = 2;
            // 
            // Nazev
            // 
            Nazev.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Nazev.DataPropertyName = "Nazev";
            Nazev.HeaderText = "Nazev";
            Nazev.Name = "Nazev";
            Nazev.Width = 64;
            // 
            // Autor
            // 
            Autor.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Autor.DataPropertyName = "Autor";
            Autor.HeaderText = "Autor";
            Autor.Name = "Autor";
            Autor.Width = 62;
            // 
            // Dostupnost
            // 
            Dostupnost.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dostupnost.DataPropertyName = "Dostupnost";
            Dostupnost.HeaderText = "Dostupnost";
            Dostupnost.Name = "Dostupnost";
            Dostupnost.Width = 74;
            // 
            // ISBN
            // 
            ISBN.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            ISBN.DataPropertyName = "ISBN";
            ISBN.HeaderText = "ISBN";
            ISBN.Name = "ISBN";
            ISBN.Width = 57;
            // 
            // RokVydani
            // 
            RokVydani.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            RokVydani.DataPropertyName = "RokVydani";
            RokVydani.HeaderText = "Rok vydání";
            RokVydani.Name = "RokVydani";
            RokVydani.Width = 90;
            // 
            // StavKnihy
            // 
            StavKnihy.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            StavKnihy.DataPropertyName = "StavKnihy";
            StavKnihy.HeaderText = "Stav knihy";
            StavKnihy.Name = "StavKnihy";
            StavKnihy.Width = 86;
            // 
            // lblSeznamKnih
            // 
            lblSeznamKnih.AutoSize = true;
            lblSeznamKnih.Location = new Point(544, 19);
            lblSeznamKnih.Name = "lblSeznamKnih";
            lblSeznamKnih.Size = new Size(77, 15);
            lblSeznamKnih.TabIndex = 3;
            lblSeznamKnih.Text = "Seznam knih:";
            // 
            // btnVypujcit
            // 
            btnVypujcit.Location = new Point(12, 563);
            btnVypujcit.Name = "btnVypujcit";
            btnVypujcit.Size = new Size(107, 23);
            btnVypujcit.TabIndex = 4;
            btnVypujcit.Text = "Vypůjčit";
            btnVypujcit.UseVisualStyleBackColor = true;
            btnVypujcit.Click += btnVypujcit_Click;
            // 
            // btnVratit
            // 
            btnVratit.Location = new Point(423, 563);
            btnVratit.Name = "btnVratit";
            btnVratit.Size = new Size(107, 23);
            btnVratit.TabIndex = 5;
            btnVratit.Text = "Vrátit";
            btnVratit.UseVisualStyleBackColor = true;
            btnVratit.Click += btnVratit_Click;
            // 
            // btnNovyCtenar
            // 
            btnNovyCtenar.Location = new Point(12, 304);
            btnNovyCtenar.Name = "btnNovyCtenar";
            btnNovyCtenar.Size = new Size(107, 23);
            btnNovyCtenar.TabIndex = 6;
            btnNovyCtenar.Text = "Nový čtenář";
            btnNovyCtenar.UseVisualStyleBackColor = true;
            btnNovyCtenar.Click += btnNovyCtenar_Click;
            // 
            // btnEditaceCtenare
            // 
            btnEditaceCtenare.Location = new Point(215, 304);
            btnEditaceCtenare.Name = "btnEditaceCtenare";
            btnEditaceCtenare.Size = new Size(107, 23);
            btnEditaceCtenare.TabIndex = 7;
            btnEditaceCtenare.Text = "Editace čtenáře";
            btnEditaceCtenare.UseVisualStyleBackColor = true;
            btnEditaceCtenare.Click += btnEditaceCtenare_Click;
            // 
            // dgvVypujcene
            // 
            dgvVypujcene.AllowUserToAddRows = false;
            dgvVypujcene.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvVypujcene.Columns.AddRange(new DataGridViewColumn[] { Nazev1, Autor1, Dostupnost1, ISBN1, RokVydani1, StavKnihy1 });
            dgvVypujcene.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvVypujcene.Location = new Point(12, 360);
            dgvVypujcene.MultiSelect = false;
            dgvVypujcene.Name = "dgvVypujcene";
            dgvVypujcene.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVypujcene.Size = new Size(518, 197);
            dgvVypujcene.TabIndex = 8;
            // 
            // Nazev1
            // 
            Nazev1.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Nazev1.DataPropertyName = "Nazev";
            Nazev1.HeaderText = "Název";
            Nazev1.Name = "Nazev1";
            Nazev1.Width = 64;
            // 
            // Autor1
            // 
            Autor1.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Autor1.DataPropertyName = "Autor";
            Autor1.HeaderText = "Autor";
            Autor1.Name = "Autor1";
            Autor1.Width = 62;
            // 
            // Dostupnost1
            // 
            Dostupnost1.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dostupnost1.DataPropertyName = "Dostupnost";
            Dostupnost1.HeaderText = "Dostupnost";
            Dostupnost1.Name = "Dostupnost1";
            Dostupnost1.Width = 74;
            // 
            // ISBN1
            // 
            ISBN1.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            ISBN1.DataPropertyName = "ISBN";
            ISBN1.HeaderText = "ISBN";
            ISBN1.Name = "ISBN1";
            ISBN1.Width = 57;
            // 
            // RokVydani1
            // 
            RokVydani1.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            RokVydani1.DataPropertyName = "RokVydani";
            RokVydani1.HeaderText = "Rok vydání";
            RokVydani1.Name = "RokVydani1";
            RokVydani1.Width = 90;
            // 
            // StavKnihy1
            // 
            StavKnihy1.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            StavKnihy1.DataPropertyName = "StavKnihy";
            StavKnihy1.HeaderText = "Stav knihy";
            StavKnihy1.Name = "StavKnihy1";
            StavKnihy1.Width = 86;
            // 
            // lblVypujceneKnihy
            // 
            lblVypujceneKnihy.AutoSize = true;
            lblVypujceneKnihy.Location = new Point(12, 342);
            lblVypujceneKnihy.Name = "lblVypujceneKnihy";
            lblVypujceneKnihy.Size = new Size(97, 15);
            lblVypujceneKnihy.TabIndex = 9;
            lblVypujceneKnihy.Text = "Vypůjčené knihy:";
            // 
            // btnNovaKniha
            // 
            btnNovaKniha.Location = new Point(544, 304);
            btnNovaKniha.Name = "btnNovaKniha";
            btnNovaKniha.Size = new Size(107, 23);
            btnNovaKniha.TabIndex = 10;
            btnNovaKniha.Text = "Nová kniha";
            btnNovaKniha.UseVisualStyleBackColor = true;
            btnNovaKniha.Click += btnNovaKniha_Click;
            // 
            // btnEditaceKnihy
            // 
            btnEditaceKnihy.Location = new Point(747, 304);
            btnEditaceKnihy.Name = "btnEditaceKnihy";
            btnEditaceKnihy.Size = new Size(107, 23);
            btnEditaceKnihy.TabIndex = 11;
            btnEditaceKnihy.Text = "Editace knihy";
            btnEditaceKnihy.UseVisualStyleBackColor = true;
            btnEditaceKnihy.Click += btnEditaceKnihy_Click;
            // 
            // dgvRezervovane
            // 
            dgvRezervovane.AllowUserToAddRows = false;
            dgvRezervovane.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRezervovane.Columns.AddRange(new DataGridViewColumn[] { Nazev2, Autor2, Dostupnost2, ISBN2, RokVydani2, StavKnihy2 });
            dgvRezervovane.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvRezervovane.Location = new Point(544, 360);
            dgvRezervovane.MultiSelect = false;
            dgvRezervovane.Name = "dgvRezervovane";
            dgvRezervovane.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRezervovane.Size = new Size(518, 197);
            dgvRezervovane.TabIndex = 12;
            // 
            // Nazev2
            // 
            Nazev2.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Nazev2.DataPropertyName = "Nazev";
            Nazev2.HeaderText = "Název";
            Nazev2.Name = "Nazev2";
            Nazev2.Width = 64;
            // 
            // Autor2
            // 
            Autor2.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Autor2.DataPropertyName = "Autor";
            Autor2.HeaderText = "Autor";
            Autor2.Name = "Autor2";
            Autor2.Width = 62;
            // 
            // Dostupnost2
            // 
            Dostupnost2.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dostupnost2.DataPropertyName = "Dostupnost";
            Dostupnost2.HeaderText = "Dostupnost";
            Dostupnost2.Name = "Dostupnost2";
            Dostupnost2.Width = 74;
            // 
            // ISBN2
            // 
            ISBN2.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            ISBN2.DataPropertyName = "ISBN";
            ISBN2.HeaderText = "ISBN";
            ISBN2.Name = "ISBN2";
            ISBN2.Width = 57;
            // 
            // RokVydani2
            // 
            RokVydani2.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            RokVydani2.DataPropertyName = "RokVydani";
            RokVydani2.HeaderText = "Rok vydání";
            RokVydani2.Name = "RokVydani2";
            RokVydani2.Width = 90;
            // 
            // StavKnihy2
            // 
            StavKnihy2.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            StavKnihy2.DataPropertyName = "StavKnihy";
            StavKnihy2.HeaderText = "Stav knihy";
            StavKnihy2.Name = "StavKnihy2";
            StavKnihy2.Width = 86;
            // 
            // btnRezervovat
            // 
            btnRezervovat.Location = new Point(544, 563);
            btnRezervovat.Name = "btnRezervovat";
            btnRezervovat.Size = new Size(107, 23);
            btnRezervovat.TabIndex = 13;
            btnRezervovat.Text = "Rezervovat";
            btnRezervovat.UseVisualStyleBackColor = true;
            btnRezervovat.Click += btnRezervovat_Click;
            // 
            // btnZrusit
            // 
            btnZrusit.Location = new Point(955, 563);
            btnZrusit.Name = "btnZrusit";
            btnZrusit.Size = new Size(107, 23);
            btnZrusit.TabIndex = 14;
            btnZrusit.Text = "Zrušit";
            btnZrusit.UseVisualStyleBackColor = true;
            btnZrusit.Click += btnZrusit_Click;
            // 
            // btnSmazatKnihu
            // 
            btnSmazatKnihu.Location = new Point(955, 304);
            btnSmazatKnihu.Name = "btnSmazatKnihu";
            btnSmazatKnihu.Size = new Size(107, 23);
            btnSmazatKnihu.TabIndex = 15;
            btnSmazatKnihu.Text = "Smazat knihu";
            btnSmazatKnihu.UseVisualStyleBackColor = true;
            btnSmazatKnihu.Click += btnSmazatKnihu_Click;
            // 
            // btnSmazatCtenare
            // 
            btnSmazatCtenare.Location = new Point(423, 304);
            btnSmazatCtenare.Name = "btnSmazatCtenare";
            btnSmazatCtenare.Size = new Size(107, 23);
            btnSmazatCtenare.TabIndex = 16;
            btnSmazatCtenare.Text = "Smazat čtenáře";
            btnSmazatCtenare.UseVisualStyleBackColor = true;
            btnSmazatCtenare.Click += btnSmazatCtenare_Click;
            // 
            // lblRezervovaneKnihy
            // 
            lblRezervovaneKnihy.AutoSize = true;
            lblRezervovaneKnihy.Location = new Point(544, 342);
            lblRezervovaneKnihy.Name = "lblRezervovaneKnihy";
            lblRezervovaneKnihy.Size = new Size(108, 15);
            lblRezervovaneKnihy.TabIndex = 17;
            lblRezervovaneKnihy.Text = "Rezervované knihy:";
            // 
            // lblHledatKnihu
            // 
            lblHledatKnihu.AutoSize = true;
            lblHledatKnihu.Location = new Point(627, 19);
            lblHledatKnihu.Name = "lblHledatKnihu";
            lblHledatKnihu.Size = new Size(78, 15);
            lblHledatKnihu.TabIndex = 18;
            lblHledatKnihu.Text = "Hledat knihu:";
            // 
            // txtHledatKnihu
            // 
            txtHledatKnihu.Location = new Point(711, 16);
            txtHledatKnihu.Name = "txtHledatKnihu";
            txtHledatKnihu.Size = new Size(100, 23);
            txtHledatKnihu.TabIndex = 19;
            txtHledatKnihu.TextChanged += txtHledatKnihu_TextChanged;
            // 
            // btnVymazatHledaniKnih
            // 
            btnVymazatHledaniKnih.Location = new Point(817, 16);
            btnVymazatHledaniKnih.Name = "btnVymazatHledaniKnih";
            btnVymazatHledaniKnih.Size = new Size(75, 23);
            btnVymazatHledaniKnih.TabIndex = 20;
            btnVymazatHledaniKnih.Text = "Vymazat";
            btnVymazatHledaniKnih.UseVisualStyleBackColor = true;
            btnVymazatHledaniKnih.Click += btnVymazatHledaniKnih_Click;
            // 
            // lblHledatCtenare
            // 
            lblHledatCtenare.AutoSize = true;
            lblHledatCtenare.Location = new Point(256, 19);
            lblHledatCtenare.Name = "lblHledatCtenare";
            lblHledatCtenare.Size = new Size(87, 15);
            lblHledatCtenare.TabIndex = 21;
            lblHledatCtenare.Text = "Hledat čtenáře:";
            // 
            // txtHledatCtenare
            // 
            txtHledatCtenare.Location = new Point(349, 16);
            txtHledatCtenare.Name = "txtHledatCtenare";
            txtHledatCtenare.Size = new Size(100, 23);
            txtHledatCtenare.TabIndex = 22;
            txtHledatCtenare.TextChanged += txtHledatCtenare_TextChanged;
            // 
            // btnVymazatHledaniCtenaru
            // 
            btnVymazatHledaniCtenaru.Location = new Point(455, 15);
            btnVymazatHledaniCtenaru.Name = "btnVymazatHledaniCtenaru";
            btnVymazatHledaniCtenaru.Size = new Size(75, 23);
            btnVymazatHledaniCtenaru.TabIndex = 23;
            btnVymazatHledaniCtenaru.Text = "Vymazat";
            btnVymazatHledaniCtenaru.UseVisualStyleBackColor = true;
            btnVymazatHledaniCtenaru.Click += btnVymazatHledaniCtenaru_Click;
            // 
            // cbFiltrDostupnosti
            // 
            cbFiltrDostupnosti.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFiltrDostupnosti.FormattingEnabled = true;
            cbFiltrDostupnosti.Items.AddRange(new object[] { "Všechny", "Dostupné", "Vypůjčené" });
            cbFiltrDostupnosti.Location = new Point(975, 16);
            cbFiltrDostupnosti.Name = "cbFiltrDostupnosti";
            cbFiltrDostupnosti.Size = new Size(87, 23);
            cbFiltrDostupnosti.TabIndex = 24;
            cbFiltrDostupnosti.SelectedIndexChanged += cbFiltrDostupnosti_SelectedIndexChanged;
            // 
            // lblFiltrDostupnosti
            // 
            lblFiltrDostupnosti.AutoSize = true;
            lblFiltrDostupnosti.Location = new Point(898, 19);
            lblFiltrDostupnosti.Name = "lblFiltrDostupnosti";
            lblFiltrDostupnosti.Size = new Size(71, 15);
            lblFiltrDostupnosti.TabIndex = 25;
            lblFiltrDostupnosti.Text = "Dostupnost:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1088, 607);
            Controls.Add(lblFiltrDostupnosti);
            Controls.Add(cbFiltrDostupnosti);
            Controls.Add(btnVymazatHledaniCtenaru);
            Controls.Add(txtHledatCtenare);
            Controls.Add(lblHledatCtenare);
            Controls.Add(btnVymazatHledaniKnih);
            Controls.Add(txtHledatKnihu);
            Controls.Add(lblHledatKnihu);
            Controls.Add(lblRezervovaneKnihy);
            Controls.Add(btnSmazatCtenare);
            Controls.Add(btnSmazatKnihu);
            Controls.Add(btnZrusit);
            Controls.Add(btnRezervovat);
            Controls.Add(dgvRezervovane);
            Controls.Add(btnEditaceKnihy);
            Controls.Add(btnNovaKniha);
            Controls.Add(lblVypujceneKnihy);
            Controls.Add(dgvVypujcene);
            Controls.Add(btnEditaceCtenare);
            Controls.Add(btnNovyCtenar);
            Controls.Add(btnVratit);
            Controls.Add(btnVypujcit);
            Controls.Add(lblSeznamKnih);
            Controls.Add(dgvKnihy);
            Controls.Add(lblCtenari);
            Controls.Add(dgvCtenari);
            Name = "Form1";
            Text = "Správa knihovny";
            ((System.ComponentModel.ISupportInitialize)dgvCtenari).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvKnihy).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvVypujcene).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvRezervovane).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvCtenari;
        private Label lblCtenari;
        private DataGridView dgvKnihy;
        private Label lblSeznamKnih;
        private Button btnVypujcit;
        private Button btnVratit;
        private Button btnNovyCtenar;
        private Button btnEditaceCtenare;
        private DataGridView dgvVypujcene;
        private Label lblVypujceneKnihy;
        private Button btnNovaKniha;
        private Button btnEditaceKnihy;
        private DataGridView dgvRezervovane;
        private Button btnRezervovat;
        private Button btnZrusit;
        private Button btnSmazatKnihu;
        private Button btnSmazatCtenare;
        private Label lblRezervovaneKnihy;
        private DataGridViewTextBoxColumn Nazev;
        private DataGridViewTextBoxColumn Autor;
        private DataGridViewCheckBoxColumn Dostupnost;
        private DataGridViewTextBoxColumn ISBN;
        private DataGridViewTextBoxColumn RokVydani;
        private DataGridViewTextBoxColumn StavKnihy;
        private DataGridViewTextBoxColumn Jméno;
        private DataGridViewTextBoxColumn Příjmení;
        private DataGridViewTextBoxColumn Email;
        private DataGridViewTextBoxColumn TelefonniCislo;
        private DataGridViewTextBoxColumn Nazev1;
        private DataGridViewTextBoxColumn Autor1;
        private DataGridViewCheckBoxColumn Dostupnost1;
        private DataGridViewTextBoxColumn ISBN1;
        private DataGridViewTextBoxColumn RokVydani1;
        private DataGridViewTextBoxColumn StavKnihy1;
        private DataGridViewTextBoxColumn Nazev2;
        private DataGridViewTextBoxColumn Autor2;
        private DataGridViewCheckBoxColumn Dostupnost2;
        private DataGridViewTextBoxColumn ISBN2;
        private DataGridViewTextBoxColumn RokVydani2;
        private DataGridViewTextBoxColumn StavKnihy2;
        private Label lblHledatKnihu;
        private TextBox txtHledatKnihu;
        private Button btnVymazatHledaniKnih;
        private Label lblHledatCtenare;
        private TextBox txtHledatCtenare;
        private Button btnVymazatHledaniCtenaru;
        private ComboBox cbFiltrDostupnosti;
        private Label lblFiltrDostupnosti;
    }
}
