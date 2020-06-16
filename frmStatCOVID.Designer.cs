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
            this.pnlTipoFile = new System.Windows.Forms.Panel();
            this.optSierologici = new System.Windows.Forms.RadioButton();
            this.optTamponi = new System.Windows.Forms.RadioButton();
            this.chkFileEsteso = new System.Windows.Forms.CheckBox();
            this.optPiacenza = new System.Windows.Forms.RadioButton();
            this.optEmiliaRomagna = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.pnlTipoFile.SuspendLayout();
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
            this.cmdGeneraFile.Location = new System.Drawing.Point(750, 71);
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
            this.progressBar1.Location = new System.Drawing.Point(0, 156);
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
            this.panel1.Controls.Add(this.pnlTipoFile);
            this.panel1.Controls.Add(this.cmdSelFile);
            this.panel1.Controls.Add(this.lblFileOrigine);
            this.panel1.Location = new System.Drawing.Point(12, 14);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(730, 100);
            this.panel1.TabIndex = 7;
            // 
            // pnlTipoFile
            // 
            this.pnlTipoFile.Controls.Add(this.optSierologici);
            this.pnlTipoFile.Controls.Add(this.optTamponi);
            this.pnlTipoFile.Location = new System.Drawing.Point(12, 8);
            this.pnlTipoFile.Name = "pnlTipoFile";
            this.pnlTipoFile.Size = new System.Drawing.Size(261, 37);
            this.pnlTipoFile.TabIndex = 9;
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
            this.chkFileEsteso.Location = new System.Drawing.Point(751, 100);
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
            this.optPiacenza.Location = new System.Drawing.Point(751, 44);
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
            this.optEmiliaRomagna.Location = new System.Drawing.Point(871, 44);
            this.optEmiliaRomagna.Name = "optEmiliaRomagna";
            this.optEmiliaRomagna.Size = new System.Drawing.Size(154, 20);
            this.optEmiliaRomagna.TabIndex = 11;
            this.optEmiliaRomagna.Text = "EMILIA ROMAGNA";
            this.optEmiliaRomagna.UseVisualStyleBackColor = true;
            // 
            // frmStatCOVID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1086, 179);
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
            this.pnlTipoFile.ResumeLayout(false);
            this.pnlTipoFile.PerformLayout();
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
    }
}

