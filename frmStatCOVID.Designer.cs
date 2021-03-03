namespace waCOVID
{
    partial class frmStatCOVID
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStatCOVID));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.cmdSelFile = new System.Windows.Forms.Button();
            this.lblFileOrigine = new System.Windows.Forms.Label();
            this.cmdGeneraFile = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.optTradate = new System.Windows.Forms.RadioButton();
            this.optViadana = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkVerificaDatiCF = new System.Windows.Forms.CheckBox();
            this.pnlTipoFile = new System.Windows.Forms.Panel();
            this.optAntigenici = new System.Windows.Forms.RadioButton();
            this.optTestRapidi = new System.Windows.Forms.RadioButton();
            this.optSierologici = new System.Windows.Forms.RadioButton();
            this.optTamponi = new System.Windows.Forms.RadioButton();
            this.chkFileEsteso = new System.Windows.Forms.CheckBox();
            this.optPiacenza = new System.Windows.Forms.RadioButton();
            this.optEmiliaRomagna = new System.Windows.Forms.RadioButton();
            this.optPiemonte = new System.Windows.Forms.RadioButton();
            this.chkCFRefertante = new System.Windows.Forms.CheckBox();
            this.pnlCFRefertante = new System.Windows.Forms.Panel();
            this.cmbCFRefertante = new System.Windows.Forms.ComboBox();
            this.lblStato = new System.Windows.Forms.Label();
            this.optLiguria = new System.Windows.Forms.RadioButton();
            this.optModena = new System.Windows.Forms.RadioButton();
            this.optParma = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNetto = new System.Windows.Forms.TextBox();
            this.txtTicket = new System.Windows.Forms.TextBox();
            this.txtLordo = new System.Windows.Forms.TextBox();
            this.txtNumEsami = new System.Windows.Forms.TextBox();
            this.txtNumImp = new System.Windows.Forms.TextBox();
            this.txtNumRecord = new System.Windows.Forms.TextBox();
            this.txtStartProg = new System.Windows.Forms.TextBox();
            this.optSardegna = new System.Windows.Forms.RadioButton();
            this.optCampania = new System.Windows.Forms.RadioButton();
            this.optPuglia = new System.Windows.Forms.RadioButton();
            this.optVeneto = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.pnlTipoFile.SuspendLayout();
            this.pnlCFRefertante.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // cmdSelFile
            // 
            this.cmdSelFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdSelFile.Location = new System.Drawing.Point(12, 63);
            this.cmdSelFile.Name = "cmdSelFile";
            this.cmdSelFile.Size = new System.Drawing.Size(205, 27);
            this.cmdSelFile.TabIndex = 0;
            this.cmdSelFile.Text = "Seleziona file";
            this.cmdSelFile.UseVisualStyleBackColor = true;
            this.cmdSelFile.Click += new System.EventHandler(this.cmdSelFile_Click);
            // 
            // lblFileOrigine
            // 
            this.lblFileOrigine.BackColor = System.Drawing.Color.LightYellow;
            this.lblFileOrigine.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblFileOrigine.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFileOrigine.Location = new System.Drawing.Point(223, 65);
            this.lblFileOrigine.Name = "lblFileOrigine";
            this.lblFileOrigine.Size = new System.Drawing.Size(497, 22);
            this.lblFileOrigine.TabIndex = 1;
            this.lblFileOrigine.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmdGeneraFile
            // 
            this.cmdGeneraFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdGeneraFile.Location = new System.Drawing.Point(750, 187);
            this.cmdGeneraFile.Name = "cmdGeneraFile";
            this.cmdGeneraFile.Size = new System.Drawing.Size(213, 27);
            this.cmdGeneraFile.TabIndex = 3;
            this.cmdGeneraFile.Text = "Genera file";
            this.cmdGeneraFile.UseVisualStyleBackColor = true;
            this.cmdGeneraFile.Click += new System.EventHandler(this.cmdGeneraFile_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.progressBar1.Location = new System.Drawing.Point(0, 346);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(1086, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 4;
            // 
            // optTradate
            // 
            this.optTradate.AutoSize = true;
            this.optTradate.Checked = true;
            this.optTradate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optTradate.Location = new System.Drawing.Point(751, 18);
            this.optTradate.Name = "optTradate";
            this.optTradate.Size = new System.Drawing.Size(98, 20);
            this.optTradate.TabIndex = 5;
            this.optTradate.TabStop = true;
            this.optTradate.Text = "TRADATE";
            this.optTradate.UseVisualStyleBackColor = true;
            this.optTradate.CheckedChanged += new System.EventHandler(this.optSede_CheckedChanged);
            // 
            // optViadana
            // 
            this.optViadana.AutoSize = true;
            this.optViadana.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optViadana.Location = new System.Drawing.Point(871, 18);
            this.optViadana.Name = "optViadana";
            this.optViadana.Size = new System.Drawing.Size(92, 20);
            this.optViadana.TabIndex = 6;
            this.optViadana.Text = "VIADANA";
            this.optViadana.UseVisualStyleBackColor = true;
            this.optViadana.CheckedChanged += new System.EventHandler(this.optSede_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkVerificaDatiCF);
            this.panel1.Controls.Add(this.pnlTipoFile);
            this.panel1.Controls.Add(this.cmdSelFile);
            this.panel1.Controls.Add(this.lblFileOrigine);
            this.panel1.Location = new System.Drawing.Point(12, 14);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(730, 136);
            this.panel1.TabIndex = 7;
            // 
            // chkVerificaDatiCF
            // 
            this.chkVerificaDatiCF.AutoSize = true;
            this.chkVerificaDatiCF.Checked = true;
            this.chkVerificaDatiCF.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkVerificaDatiCF.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkVerificaDatiCF.Location = new System.Drawing.Point(12, 96);
            this.chkVerificaDatiCF.Name = "chkVerificaDatiCF";
            this.chkVerificaDatiCF.Size = new System.Drawing.Size(221, 20);
            this.chkVerificaDatiCF.TabIndex = 22;
            this.chkVerificaDatiCF.Text = "Verifica correttezza dati C.F.";
            this.chkVerificaDatiCF.UseVisualStyleBackColor = true;
            // 
            // pnlTipoFile
            // 
            this.pnlTipoFile.Controls.Add(this.optAntigenici);
            this.pnlTipoFile.Controls.Add(this.optTestRapidi);
            this.pnlTipoFile.Controls.Add(this.optSierologici);
            this.pnlTipoFile.Controls.Add(this.optTamponi);
            this.pnlTipoFile.Location = new System.Drawing.Point(12, 8);
            this.pnlTipoFile.Name = "pnlTipoFile";
            this.pnlTipoFile.Size = new System.Drawing.Size(495, 37);
            this.pnlTipoFile.TabIndex = 9;
            // 
            // optAntigenici
            // 
            this.optAntigenici.AutoSize = true;
            this.optAntigenici.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optAntigenici.Location = new System.Drawing.Point(350, 3);
            this.optAntigenici.Name = "optAntigenici";
            this.optAntigenici.Size = new System.Drawing.Size(94, 20);
            this.optAntigenici.TabIndex = 10;
            this.optAntigenici.Text = "Antigenici";
            this.optAntigenici.UseVisualStyleBackColor = true;
            // 
            // optTestRapidi
            // 
            this.optTestRapidi.AutoSize = true;
            this.optTestRapidi.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optTestRapidi.Location = new System.Drawing.Point(228, 3);
            this.optTestRapidi.Name = "optTestRapidi";
            this.optTestRapidi.Size = new System.Drawing.Size(101, 20);
            this.optTestRapidi.TabIndex = 9;
            this.optTestRapidi.Text = "Test rapidi";
            this.optTestRapidi.UseVisualStyleBackColor = true;
            // 
            // optSierologici
            // 
            this.optSierologici.AutoSize = true;
            this.optSierologici.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optSierologici.Location = new System.Drawing.Point(109, 3);
            this.optSierologici.Name = "optSierologici";
            this.optSierologici.Size = new System.Drawing.Size(101, 20);
            this.optSierologici.TabIndex = 8;
            this.optSierologici.Text = "Sierologici";
            this.optSierologici.UseVisualStyleBackColor = true;
            // 
            // optTamponi
            // 
            this.optTamponi.AutoSize = true;
            this.optTamponi.Checked = true;
            this.optTamponi.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optTamponi.Location = new System.Drawing.Point(12, 3);
            this.optTamponi.Name = "optTamponi";
            this.optTamponi.Size = new System.Drawing.Size(87, 20);
            this.optTamponi.TabIndex = 7;
            this.optTamponi.TabStop = true;
            this.optTamponi.Text = "Tamponi";
            this.optTamponi.UseVisualStyleBackColor = true;
            // 
            // chkFileEsteso
            // 
            this.chkFileEsteso.AutoSize = true;
            this.chkFileEsteso.Location = new System.Drawing.Point(751, 216);
            this.chkFileEsteso.Name = "chkFileEsteso";
            this.chkFileEsteso.Size = new System.Drawing.Size(141, 20);
            this.chkFileEsteso.TabIndex = 9;
            this.chkFileEsteso.Text = "Formato file esteso";
            this.chkFileEsteso.UseVisualStyleBackColor = true;
            // 
            // optPiacenza
            // 
            this.optPiacenza.AutoSize = true;
            this.optPiacenza.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optPiacenza.Location = new System.Drawing.Point(751, 46);
            this.optPiacenza.Name = "optPiacenza";
            this.optPiacenza.Size = new System.Drawing.Size(100, 20);
            this.optPiacenza.TabIndex = 10;
            this.optPiacenza.Text = "PIACENZA";
            this.optPiacenza.UseVisualStyleBackColor = true;
            this.optPiacenza.CheckedChanged += new System.EventHandler(this.optSede_CheckedChanged);
            // 
            // optEmiliaRomagna
            // 
            this.optEmiliaRomagna.AutoSize = true;
            this.optEmiliaRomagna.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optEmiliaRomagna.Location = new System.Drawing.Point(871, 46);
            this.optEmiliaRomagna.Name = "optEmiliaRomagna";
            this.optEmiliaRomagna.Size = new System.Drawing.Size(154, 20);
            this.optEmiliaRomagna.TabIndex = 11;
            this.optEmiliaRomagna.Text = "EMILIA ROMAGNA";
            this.optEmiliaRomagna.UseVisualStyleBackColor = true;
            this.optEmiliaRomagna.CheckedChanged += new System.EventHandler(this.optSede_CheckedChanged);
            // 
            // optPiemonte
            // 
            this.optPiemonte.AutoSize = true;
            this.optPiemonte.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optPiemonte.Location = new System.Drawing.Point(751, 102);
            this.optPiemonte.Name = "optPiemonte";
            this.optPiemonte.Size = new System.Drawing.Size(104, 20);
            this.optPiemonte.TabIndex = 12;
            this.optPiemonte.Text = "PIEMONTE";
            this.optPiemonte.UseVisualStyleBackColor = true;
            this.optPiemonte.CheckedChanged += new System.EventHandler(this.optSede_CheckedChanged);
            // 
            // chkCFRefertante
            // 
            this.chkCFRefertante.AutoSize = true;
            this.chkCFRefertante.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCFRefertante.Location = new System.Drawing.Point(8, 18);
            this.chkCFRefertante.Name = "chkCFRefertante";
            this.chkCFRefertante.Size = new System.Drawing.Size(130, 20);
            this.chkCFRefertante.TabIndex = 13;
            this.chkCFRefertante.Text = "C.F. Refertante";
            this.chkCFRefertante.UseVisualStyleBackColor = true;
            // 
            // pnlCFRefertante
            // 
            this.pnlCFRefertante.Controls.Add(this.cmbCFRefertante);
            this.pnlCFRefertante.Controls.Add(this.chkCFRefertante);
            this.pnlCFRefertante.Location = new System.Drawing.Point(12, 156);
            this.pnlCFRefertante.Name = "pnlCFRefertante";
            this.pnlCFRefertante.Size = new System.Drawing.Size(364, 63);
            this.pnlCFRefertante.TabIndex = 15;
            this.pnlCFRefertante.Visible = false;
            // 
            // cmbCFRefertante
            // 
            this.cmbCFRefertante.FormattingEnabled = true;
            this.cmbCFRefertante.Items.AddRange(new object[] {
            "RSOLSN69M04G197A",
            "ZCCLNE59C65L736B"});
            this.cmbCFRefertante.Location = new System.Drawing.Point(144, 14);
            this.cmbCFRefertante.Name = "cmbCFRefertante";
            this.cmbCFRefertante.Size = new System.Drawing.Size(197, 24);
            this.cmbCFRefertante.TabIndex = 14;
            // 
            // lblStato
            // 
            this.lblStato.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStato.AutoSize = true;
            this.lblStato.Location = new System.Drawing.Point(9, 322);
            this.lblStato.Name = "lblStato";
            this.lblStato.Size = new System.Drawing.Size(0, 16);
            this.lblStato.TabIndex = 16;
            // 
            // optLiguria
            // 
            this.optLiguria.AutoSize = true;
            this.optLiguria.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optLiguria.Location = new System.Drawing.Point(871, 102);
            this.optLiguria.Name = "optLiguria";
            this.optLiguria.Size = new System.Drawing.Size(85, 20);
            this.optLiguria.TabIndex = 17;
            this.optLiguria.Text = "LIGURIA";
            this.optLiguria.UseVisualStyleBackColor = true;
            // 
            // optModena
            // 
            this.optModena.AutoSize = true;
            this.optModena.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optModena.Location = new System.Drawing.Point(871, 74);
            this.optModena.Name = "optModena";
            this.optModena.Size = new System.Drawing.Size(91, 20);
            this.optModena.TabIndex = 19;
            this.optModena.Text = "MODENA";
            this.optModena.UseVisualStyleBackColor = true;
            // 
            // optParma
            // 
            this.optParma.AutoSize = true;
            this.optParma.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optParma.Location = new System.Drawing.Point(751, 74);
            this.optParma.Name = "optParma";
            this.optParma.Size = new System.Drawing.Size(79, 20);
            this.optParma.TabIndex = 18;
            this.optParma.Text = "PARMA";
            this.optParma.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(29, 65);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(129, 23);
            this.button1.TabIndex = 20;
            this.button1.Text = "Rinumera Citotest";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.txtNetto);
            this.panel2.Controls.Add(this.txtTicket);
            this.panel2.Controls.Add(this.txtLordo);
            this.panel2.Controls.Add(this.txtNumEsami);
            this.panel2.Controls.Add(this.txtNumImp);
            this.panel2.Controls.Add(this.txtNumRecord);
            this.panel2.Controls.Add(this.txtStartProg);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Location = new System.Drawing.Point(12, 242);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1062, 96);
            this.panel2.TabIndex = 21;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(879, 46);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 16);
            this.label7.TabIndex = 34;
            this.label7.Text = "Netto";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(734, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 16);
            this.label6.TabIndex = 33;
            this.label6.Text = "Ticket";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(593, 46);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 16);
            this.label5.TabIndex = 32;
            this.label5.Text = "Lordo";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(460, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 16);
            this.label4.TabIndex = 31;
            this.label4.Text = "N°Esami";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(332, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 16);
            this.label3.TabIndex = 30;
            this.label3.Text = "N°Ricette";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(200, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 16);
            this.label2.TabIndex = 29;
            this.label2.Text = "N°Record";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 16);
            this.label1.TabIndex = 28;
            this.label1.Text = "Inizia dal Progressivo";
            // 
            // txtNetto
            // 
            this.txtNetto.Location = new System.Drawing.Point(882, 65);
            this.txtNetto.Name = "txtNetto";
            this.txtNetto.Size = new System.Drawing.Size(100, 22);
            this.txtNetto.TabIndex = 27;
            this.txtNetto.Text = "0";
            // 
            // txtTicket
            // 
            this.txtTicket.Location = new System.Drawing.Point(737, 65);
            this.txtTicket.Name = "txtTicket";
            this.txtTicket.Size = new System.Drawing.Size(100, 22);
            this.txtTicket.TabIndex = 26;
            this.txtTicket.Text = "0";
            // 
            // txtLordo
            // 
            this.txtLordo.Location = new System.Drawing.Point(596, 65);
            this.txtLordo.Name = "txtLordo";
            this.txtLordo.Size = new System.Drawing.Size(100, 22);
            this.txtLordo.TabIndex = 25;
            this.txtLordo.Text = "0";
            // 
            // txtNumEsami
            // 
            this.txtNumEsami.Location = new System.Drawing.Point(463, 65);
            this.txtNumEsami.Name = "txtNumEsami";
            this.txtNumEsami.Size = new System.Drawing.Size(100, 22);
            this.txtNumEsami.TabIndex = 24;
            this.txtNumEsami.Text = "0";
            // 
            // txtNumImp
            // 
            this.txtNumImp.Location = new System.Drawing.Point(335, 65);
            this.txtNumImp.Name = "txtNumImp";
            this.txtNumImp.Size = new System.Drawing.Size(100, 22);
            this.txtNumImp.TabIndex = 23;
            this.txtNumImp.Text = "0";
            // 
            // txtNumRecord
            // 
            this.txtNumRecord.Location = new System.Drawing.Point(203, 65);
            this.txtNumRecord.Name = "txtNumRecord";
            this.txtNumRecord.Size = new System.Drawing.Size(100, 22);
            this.txtNumRecord.TabIndex = 22;
            this.txtNumRecord.Text = "0";
            // 
            // txtStartProg
            // 
            this.txtStartProg.Location = new System.Drawing.Point(29, 28);
            this.txtStartProg.Name = "txtStartProg";
            this.txtStartProg.Size = new System.Drawing.Size(100, 22);
            this.txtStartProg.TabIndex = 21;
            this.txtStartProg.Text = "1";
            // 
            // optSardegna
            // 
            this.optSardegna.AutoSize = true;
            this.optSardegna.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optSardegna.Location = new System.Drawing.Point(751, 130);
            this.optSardegna.Name = "optSardegna";
            this.optSardegna.Size = new System.Drawing.Size(110, 20);
            this.optSardegna.TabIndex = 22;
            this.optSardegna.Text = "SARDEGNA";
            this.optSardegna.UseVisualStyleBackColor = true;
            // 
            // optCampania
            // 
            this.optCampania.AutoSize = true;
            this.optCampania.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optCampania.Location = new System.Drawing.Point(871, 130);
            this.optCampania.Name = "optCampania";
            this.optCampania.Size = new System.Drawing.Size(103, 20);
            this.optCampania.TabIndex = 23;
            this.optCampania.Text = "CAMPANIA";
            this.optCampania.UseVisualStyleBackColor = true;
            // 
            // optPuglia
            // 
            this.optPuglia.AutoSize = true;
            this.optPuglia.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optPuglia.Location = new System.Drawing.Point(751, 156);
            this.optPuglia.Name = "optPuglia";
            this.optPuglia.Size = new System.Drawing.Size(80, 20);
            this.optPuglia.TabIndex = 24;
            this.optPuglia.Text = "PUGLIA";
            this.optPuglia.UseVisualStyleBackColor = true;
            // 
            // optVeneto
            // 
            this.optVeneto.AutoSize = true;
            this.optVeneto.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optVeneto.Location = new System.Drawing.Point(871, 156);
            this.optVeneto.Name = "optVeneto";
            this.optVeneto.Size = new System.Drawing.Size(88, 20);
            this.optVeneto.TabIndex = 25;
            this.optVeneto.Text = "VENETO";
            this.optVeneto.UseVisualStyleBackColor = true;
            // 
            // frmStatCOVID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1086, 369);
            this.Controls.Add(this.optVeneto);
            this.Controls.Add(this.optPuglia);
            this.Controls.Add(this.optCampania);
            this.Controls.Add(this.optSardegna);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.optModena);
            this.Controls.Add(this.optParma);
            this.Controls.Add(this.optLiguria);
            this.Controls.Add(this.lblStato);
            this.Controls.Add(this.pnlCFRefertante);
            this.Controls.Add(this.optPiemonte);
            this.Controls.Add(this.optEmiliaRomagna);
            this.Controls.Add(this.optPiacenza);
            this.Controls.Add(this.chkFileEsteso);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.optViadana);
            this.Controls.Add(this.optTradate);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.cmdGeneraFile);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmStatCOVID";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Statistiche COVID";
            this.Load += new System.EventHandler(this.frmStatCOVID_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlTipoFile.ResumeLayout(false);
            this.pnlTipoFile.PerformLayout();
            this.pnlCFRefertante.ResumeLayout(false);
            this.pnlCFRefertante.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button cmdSelFile;
        private System.Windows.Forms.Label lblFileOrigine;
        private System.Windows.Forms.Button cmdGeneraFile;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.RadioButton optTradate;
        private System.Windows.Forms.RadioButton optViadana;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton optSierologici;
        private System.Windows.Forms.RadioButton optTamponi;
        private System.Windows.Forms.CheckBox chkFileEsteso;
        private System.Windows.Forms.RadioButton optPiacenza;
        private System.Windows.Forms.Panel pnlTipoFile;
        private System.Windows.Forms.RadioButton optEmiliaRomagna;
        private System.Windows.Forms.RadioButton optPiemonte;
        private System.Windows.Forms.CheckBox chkCFRefertante;
        private System.Windows.Forms.Panel pnlCFRefertante;
        private System.Windows.Forms.Label lblStato;
        private System.Windows.Forms.RadioButton optLiguria;
        private System.Windows.Forms.RadioButton optModena;
        private System.Windows.Forms.RadioButton optParma;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtStartProg;
        private System.Windows.Forms.TextBox txtNetto;
        private System.Windows.Forms.TextBox txtTicket;
        private System.Windows.Forms.TextBox txtLordo;
        private System.Windows.Forms.TextBox txtNumEsami;
        private System.Windows.Forms.TextBox txtNumImp;
        private System.Windows.Forms.TextBox txtNumRecord;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkVerificaDatiCF;
        private System.Windows.Forms.RadioButton optSardegna;
        private System.Windows.Forms.RadioButton optTestRapidi;
        private System.Windows.Forms.RadioButton optCampania;
        private System.Windows.Forms.ComboBox cmbCFRefertante;
        private System.Windows.Forms.RadioButton optAntigenici;
        private System.Windows.Forms.RadioButton optPuglia;
        private System.Windows.Forms.RadioButton optVeneto;
    }
}

