namespace waCOVID
{
    partial class frmFlussi
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkMedico = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbTracciato = new System.Windows.Forms.ComboBox();
            this.cmdReset = new System.Windows.Forms.Button();
            this.cmdElabora = new System.Windows.Forms.Button();
            this.chkRinumera = new System.Windows.Forms.CheckBox();
            this.chkUnisci = new System.Windows.Forms.CheckBox();
            this.cmdSelFile2 = new System.Windows.Forms.Button();
            this.lblFile2 = new System.Windows.Forms.Label();
            this.cmdSelFile1 = new System.Windows.Forms.Button();
            this.lblFile1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNetto = new System.Windows.Forms.TextBox();
            this.txtTicket = new System.Windows.Forms.TextBox();
            this.txtLordo = new System.Windows.Forms.TextBox();
            this.txtNumEsami = new System.Windows.Forms.TextBox();
            this.txtNumImp = new System.Windows.Forms.TextBox();
            this.txtNumRecord = new System.Windows.Forms.TextBox();
            this.txtStartProg = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.cmdSelFile4 = new System.Windows.Forms.Button();
            this.lblFile4 = new System.Windows.Forms.Label();
            this.cmdSelFile3 = new System.Windows.Forms.Button();
            this.lblFile3 = new System.Windows.Forms.Label();
            this.lblEtiFile1 = new System.Windows.Forms.Label();
            this.lblEtiFile3 = new System.Windows.Forms.Label();
            this.lblEtiFile2 = new System.Windows.Forms.Label();
            this.lblEtiFile4 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblEtiFile4);
            this.panel2.Controls.Add(this.lblEtiFile2);
            this.panel2.Controls.Add(this.lblEtiFile3);
            this.panel2.Controls.Add(this.lblEtiFile1);
            this.panel2.Controls.Add(this.cmdSelFile4);
            this.panel2.Controls.Add(this.lblFile4);
            this.panel2.Controls.Add(this.cmdSelFile3);
            this.panel2.Controls.Add(this.lblFile3);
            this.panel2.Controls.Add(this.chkMedico);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.cmbTracciato);
            this.panel2.Controls.Add(this.cmdReset);
            this.panel2.Controls.Add(this.cmdElabora);
            this.panel2.Controls.Add(this.chkRinumera);
            this.panel2.Controls.Add(this.chkUnisci);
            this.panel2.Controls.Add(this.cmdSelFile2);
            this.panel2.Controls.Add(this.lblFile2);
            this.panel2.Controls.Add(this.cmdSelFile1);
            this.panel2.Controls.Add(this.lblFile1);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.txtNetto);
            this.panel2.Controls.Add(this.txtTicket);
            this.panel2.Controls.Add(this.txtLordo);
            this.panel2.Controls.Add(this.txtNumEsami);
            this.panel2.Controls.Add(this.txtNumImp);
            this.panel2.Controls.Add(this.txtNumRecord);
            this.panel2.Controls.Add(this.txtStartProg);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(858, 473);
            this.panel2.TabIndex = 22;
            // 
            // chkMedico
            // 
            this.chkMedico.AutoSize = true;
            this.chkMedico.Checked = true;
            this.chkMedico.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMedico.Location = new System.Drawing.Point(344, 37);
            this.chkMedico.Name = "chkMedico";
            this.chkMedico.Size = new System.Drawing.Size(146, 20);
            this.chkMedico.TabIndex = 1;
            this.chkMedico.Text = "Aggiorna ID Medico";
            this.chkMedico.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(7, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 16);
            this.label1.TabIndex = 44;
            this.label1.Text = "Tracciato";
            // 
            // cmbTracciato
            // 
            this.cmbTracciato.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTracciato.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbTracciato.FormattingEnabled = true;
            this.cmbTracciato.Items.AddRange(new object[] {
            "Veneto",
            "Lazio",
            "Toscana"});
            this.cmbTracciato.Location = new System.Drawing.Point(10, 35);
            this.cmbTracciato.Name = "cmbTracciato";
            this.cmbTracciato.Size = new System.Drawing.Size(328, 24);
            this.cmbTracciato.TabIndex = 0;
            this.cmbTracciato.SelectedIndexChanged += new System.EventHandler(this.cmbTracciato_SelectedIndexChanged);
            // 
            // cmdReset
            // 
            this.cmdReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdReset.ForeColor = System.Drawing.Color.DarkRed;
            this.cmdReset.Location = new System.Drawing.Point(636, 3);
            this.cmdReset.Name = "cmdReset";
            this.cmdReset.Size = new System.Drawing.Size(94, 29);
            this.cmdReset.TabIndex = 10;
            this.cmdReset.Text = "RESET";
            this.cmdReset.UseVisualStyleBackColor = true;
            this.cmdReset.Click += new System.EventHandler(this.cmdReset_Click);
            // 
            // cmdElabora
            // 
            this.cmdElabora.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdElabora.Location = new System.Drawing.Point(344, 326);
            this.cmdElabora.Name = "cmdElabora";
            this.cmdElabora.Size = new System.Drawing.Size(156, 33);
            this.cmdElabora.TabIndex = 9;
            this.cmdElabora.Text = "ELABORA";
            this.cmdElabora.UseVisualStyleBackColor = true;
            this.cmdElabora.Click += new System.EventHandler(this.cmdElabora_Click);
            // 
            // chkRinumera
            // 
            this.chkRinumera.AutoSize = true;
            this.chkRinumera.Checked = true;
            this.chkRinumera.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRinumera.Location = new System.Drawing.Point(15, 226);
            this.chkRinumera.Name = "chkRinumera";
            this.chkRinumera.Size = new System.Drawing.Size(183, 20);
            this.chkRinumera.TabIndex = 7;
            this.chkRinumera.Text = "Rinumera dal Progressivo";
            this.chkRinumera.UseVisualStyleBackColor = true;
            // 
            // chkUnisci
            // 
            this.chkUnisci.AutoSize = true;
            this.chkUnisci.Checked = true;
            this.chkUnisci.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUnisci.Enabled = false;
            this.chkUnisci.Location = new System.Drawing.Point(15, 198);
            this.chkUnisci.Name = "chkUnisci";
            this.chkUnisci.Size = new System.Drawing.Size(89, 20);
            this.chkUnisci.TabIndex = 6;
            this.chkUnisci.Text = "Unisci File";
            this.chkUnisci.UseVisualStyleBackColor = true;
            // 
            // cmdSelFile2
            // 
            this.cmdSelFile2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdSelFile2.Location = new System.Drawing.Point(389, 143);
            this.cmdSelFile2.Name = "cmdSelFile2";
            this.cmdSelFile2.Size = new System.Drawing.Size(30, 27);
            this.cmdSelFile2.TabIndex = 4;
            this.cmdSelFile2.Text = "...";
            this.cmdSelFile2.UseVisualStyleBackColor = true;
            this.cmdSelFile2.Click += new System.EventHandler(this.cmdSelFile1_Click);
            // 
            // lblFile2
            // 
            this.lblFile2.BackColor = System.Drawing.Color.LightYellow;
            this.lblFile2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblFile2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFile2.Location = new System.Drawing.Point(10, 148);
            this.lblFile2.Name = "lblFile2";
            this.lblFile2.Size = new System.Drawing.Size(373, 22);
            this.lblFile2.TabIndex = 38;
            this.lblFile2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmdSelFile1
            // 
            this.cmdSelFile1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdSelFile1.Location = new System.Drawing.Point(388, 97);
            this.cmdSelFile1.Name = "cmdSelFile1";
            this.cmdSelFile1.Size = new System.Drawing.Size(30, 27);
            this.cmdSelFile1.TabIndex = 2;
            this.cmdSelFile1.Text = "...";
            this.cmdSelFile1.UseVisualStyleBackColor = true;
            this.cmdSelFile1.Click += new System.EventHandler(this.cmdSelFile1_Click);
            // 
            // lblFile1
            // 
            this.lblFile1.BackColor = System.Drawing.Color.LightYellow;
            this.lblFile1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblFile1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFile1.Location = new System.Drawing.Point(10, 102);
            this.lblFile1.Name = "lblFile1";
            this.lblFile1.Size = new System.Drawing.Size(372, 22);
            this.lblFile1.TabIndex = 2;
            this.lblFile1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(685, 397);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 16);
            this.label7.TabIndex = 34;
            this.label7.Text = "Netto";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(509, 397);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 16);
            this.label6.TabIndex = 33;
            this.label6.Text = "Ticket";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(336, 397);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 16);
            this.label5.TabIndex = 32;
            this.label5.Text = "Lordo";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(214, 397);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 16);
            this.label4.TabIndex = 31;
            this.label4.Text = "N°Esami";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(112, 397);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 16);
            this.label3.TabIndex = 30;
            this.label3.Text = "N°Ricette";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 397);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 16);
            this.label2.TabIndex = 29;
            this.label2.Text = "N°Record";
            // 
            // txtNetto
            // 
            this.txtNetto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtNetto.BackColor = System.Drawing.Color.White;
            this.txtNetto.Location = new System.Drawing.Point(689, 419);
            this.txtNetto.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtNetto.Name = "txtNetto";
            this.txtNetto.ReadOnly = true;
            this.txtNetto.Size = new System.Drawing.Size(123, 22);
            this.txtNetto.TabIndex = 27;
            this.txtNetto.Text = "0";
            // 
            // txtTicket
            // 
            this.txtTicket.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtTicket.BackColor = System.Drawing.Color.White;
            this.txtTicket.Location = new System.Drawing.Point(513, 419);
            this.txtTicket.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtTicket.Name = "txtTicket";
            this.txtTicket.ReadOnly = true;
            this.txtTicket.Size = new System.Drawing.Size(123, 22);
            this.txtTicket.TabIndex = 26;
            this.txtTicket.Text = "0";
            // 
            // txtLordo
            // 
            this.txtLordo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtLordo.BackColor = System.Drawing.Color.White;
            this.txtLordo.Location = new System.Drawing.Point(340, 419);
            this.txtLordo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtLordo.Name = "txtLordo";
            this.txtLordo.ReadOnly = true;
            this.txtLordo.Size = new System.Drawing.Size(123, 22);
            this.txtLordo.TabIndex = 25;
            this.txtLordo.Text = "0";
            // 
            // txtNumEsami
            // 
            this.txtNumEsami.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtNumEsami.BackColor = System.Drawing.Color.White;
            this.txtNumEsami.Location = new System.Drawing.Point(218, 419);
            this.txtNumEsami.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtNumEsami.Name = "txtNumEsami";
            this.txtNumEsami.ReadOnly = true;
            this.txtNumEsami.Size = new System.Drawing.Size(82, 22);
            this.txtNumEsami.TabIndex = 24;
            this.txtNumEsami.Text = "0";
            // 
            // txtNumImp
            // 
            this.txtNumImp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtNumImp.BackColor = System.Drawing.Color.White;
            this.txtNumImp.Location = new System.Drawing.Point(116, 419);
            this.txtNumImp.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtNumImp.Name = "txtNumImp";
            this.txtNumImp.ReadOnly = true;
            this.txtNumImp.Size = new System.Drawing.Size(82, 22);
            this.txtNumImp.TabIndex = 23;
            this.txtNumImp.Text = "0";
            // 
            // txtNumRecord
            // 
            this.txtNumRecord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtNumRecord.BackColor = System.Drawing.Color.White;
            this.txtNumRecord.Location = new System.Drawing.Point(16, 419);
            this.txtNumRecord.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtNumRecord.Name = "txtNumRecord";
            this.txtNumRecord.ReadOnly = true;
            this.txtNumRecord.Size = new System.Drawing.Size(82, 22);
            this.txtNumRecord.TabIndex = 22;
            this.txtNumRecord.Text = "0";
            // 
            // txtStartProg
            // 
            this.txtStartProg.Location = new System.Drawing.Point(15, 247);
            this.txtStartProg.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtStartProg.Name = "txtStartProg";
            this.txtStartProg.Size = new System.Drawing.Size(77, 22);
            this.txtStartProg.TabIndex = 8;
            this.txtStartProg.Text = "1";
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.progressBar1.Location = new System.Drawing.Point(0, 450);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(858, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 23;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // cmdSelFile4
            // 
            this.cmdSelFile4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdSelFile4.Location = new System.Drawing.Point(818, 143);
            this.cmdSelFile4.Name = "cmdSelFile4";
            this.cmdSelFile4.Size = new System.Drawing.Size(30, 27);
            this.cmdSelFile4.TabIndex = 5;
            this.cmdSelFile4.Text = "...";
            this.cmdSelFile4.UseVisualStyleBackColor = true;
            this.cmdSelFile4.Click += new System.EventHandler(this.cmdSelFile1_Click);
            // 
            // lblFile4
            // 
            this.lblFile4.BackColor = System.Drawing.Color.LightYellow;
            this.lblFile4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblFile4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFile4.Location = new System.Drawing.Point(439, 148);
            this.lblFile4.Name = "lblFile4";
            this.lblFile4.Size = new System.Drawing.Size(373, 22);
            this.lblFile4.TabIndex = 49;
            this.lblFile4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmdSelFile3
            // 
            this.cmdSelFile3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdSelFile3.Location = new System.Drawing.Point(817, 97);
            this.cmdSelFile3.Name = "cmdSelFile3";
            this.cmdSelFile3.Size = new System.Drawing.Size(30, 27);
            this.cmdSelFile3.TabIndex = 3;
            this.cmdSelFile3.Text = "...";
            this.cmdSelFile3.UseVisualStyleBackColor = true;
            this.cmdSelFile3.Click += new System.EventHandler(this.cmdSelFile1_Click);
            // 
            // lblFile3
            // 
            this.lblFile3.BackColor = System.Drawing.Color.LightYellow;
            this.lblFile3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblFile3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFile3.Location = new System.Drawing.Point(439, 102);
            this.lblFile3.Name = "lblFile3";
            this.lblFile3.Size = new System.Drawing.Size(372, 22);
            this.lblFile3.TabIndex = 47;
            this.lblFile3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblEtiFile1
            // 
            this.lblEtiFile1.AutoSize = true;
            this.lblEtiFile1.Location = new System.Drawing.Point(7, 86);
            this.lblEtiFile1.Name = "lblEtiFile1";
            this.lblEtiFile1.Size = new System.Drawing.Size(89, 16);
            this.lblEtiFile1.TabIndex = 50;
            this.lblEtiFile1.Text = "File A Themix";
            // 
            // lblEtiFile3
            // 
            this.lblEtiFile3.AutoSize = true;
            this.lblEtiFile3.Location = new System.Drawing.Point(436, 86);
            this.lblEtiFile3.Name = "lblEtiFile3";
            this.lblEtiFile3.Size = new System.Drawing.Size(89, 16);
            this.lblEtiFile3.TabIndex = 51;
            this.lblEtiFile3.Text = "File S Themix";
            // 
            // lblEtiFile2
            // 
            this.lblEtiFile2.AutoSize = true;
            this.lblEtiFile2.Location = new System.Drawing.Point(7, 132);
            this.lblEtiFile2.Name = "lblEtiFile2";
            this.lblEtiFile2.Size = new System.Drawing.Size(75, 16);
            this.lblEtiFile2.TabIndex = 52;
            this.lblEtiFile2.Text = "File A JLab";
            // 
            // lblEtiFile4
            // 
            this.lblEtiFile4.AutoSize = true;
            this.lblEtiFile4.Location = new System.Drawing.Point(436, 132);
            this.lblEtiFile4.Name = "lblEtiFile4";
            this.lblEtiFile4.Size = new System.Drawing.Size(75, 16);
            this.lblEtiFile4.TabIndex = 53;
            this.lblEtiFile4.Text = "File S JLab";
            // 
            // frmFlussi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(858, 473);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "frmFlussi";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Elaborazione Flussi";
            this.Load += new System.EventHandler(this.frmFlussi_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNetto;
        private System.Windows.Forms.TextBox txtTicket;
        private System.Windows.Forms.TextBox txtLordo;
        private System.Windows.Forms.TextBox txtNumEsami;
        private System.Windows.Forms.TextBox txtNumImp;
        private System.Windows.Forms.TextBox txtNumRecord;
        private System.Windows.Forms.TextBox txtStartProg;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.CheckBox chkRinumera;
        private System.Windows.Forms.CheckBox chkUnisci;
        private System.Windows.Forms.Button cmdSelFile2;
        private System.Windows.Forms.Label lblFile2;
        private System.Windows.Forms.Button cmdSelFile1;
        private System.Windows.Forms.Label lblFile1;
        private System.Windows.Forms.Button cmdElabora;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button cmdReset;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbTracciato;
        private System.Windows.Forms.CheckBox chkMedico;
        private System.Windows.Forms.Button cmdSelFile4;
        private System.Windows.Forms.Label lblFile4;
        private System.Windows.Forms.Button cmdSelFile3;
        private System.Windows.Forms.Label lblFile3;
        private System.Windows.Forms.Label lblEtiFile4;
        private System.Windows.Forms.Label lblEtiFile2;
        private System.Windows.Forms.Label lblEtiFile3;
        private System.Windows.Forms.Label lblEtiFile1;
    }
}