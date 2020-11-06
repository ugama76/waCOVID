using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace waCOVID
{
    public partial class frmFlussi : Form
    {
        public frmFlussi()
        {
            InitializeComponent();
            cmbTracciato.SelectedIndex = 0;
        }

        private void cmdSelFile1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            if (((Button)sender).Name == "cmdSelFile1")
                lblFile1.Text = openFileDialog1.FileName;
            else
                lblFile2.Text = openFileDialog1.FileName;

            if (lblFile1.Text.Trim() != "" && lblFile2.Text.Trim() != "")
            {
                chkUnisci.Checked = true;
                chkUnisci.Enabled = true;
            }
            else
            {
                chkUnisci.Checked = false;
                chkUnisci.Enabled = false;
            }

        }

        private void cmdReset_Click(object sender, EventArgs e)
        {
            lblFile1.Text = "";
            lblFile2.Text = "";
            chkUnisci.Checked = false;
            chkUnisci.Enabled = false;
        }

        private void AzzeraContatori()
        {
            txtNumRecord.Text = "0";
            txtNumImp.Text = "0";
            txtNumEsami.Text = "0";
            txtLordo.Text = "0";
            txtTicket.Text = "0";
            txtNetto.Text = "0";

            progressBar1.Value = 0;
        }


        private void ElaboraVeneto()
        {
            string tempLineValue;
            int Prog = int.Parse("0" + txtStartProg.Text);
            int NRec = 0, NImp = 0, NEsaImp = 0, NEsa = 0, Qt = 0;
            decimal Lordo = 0, Ticket = 0, Netto = 0, ImpLordo = 0, ImpTicket = 0, ImpNetto = 0;

            AzzeraContatori();

            using (FileStream inputStream = File.OpenRead(lblFile1.Text))
            {
                using (StreamReader inputReader = new StreamReader(inputStream))
                {
                    Stream baseStream = inputReader.BaseStream;
                    long length = baseStream.Length;
                    using (StreamWriter outputWriter = File.AppendText(lblFile1.Text + ".ugo"))
                    {
                        while (null != (tempLineValue = inputReader.ReadLine()))
                        {
                            var aStringBuilder = new StringBuilder(tempLineValue);
                            if (lblFile1.Text.IndexOf("SPS") > 0)
                            {
                                NRec += 1;
                                //aStringBuilder.Remove(28, 20);
                                //aStringBuilder.Insert(28, "60830120200801" + Prog.ToString("000000")); //Citotest
                                //aStringBuilder.Insert(28, "20200050640801" + Prog.ToString("000000")); //Fleming
                                //aStringBuilder.Insert(28, "56308920200801" + Prog.ToString("000000")); //Selab
                                //aStringBuilder.Insert(28, "60580220200901" + Prog.ToString("000000")); //CMV
                                //aStringBuilder.Insert(28, "64214520200801" + Prog.ToString("000000")); //Emolab
                                if (tempLineValue.Substring(48, 2) == "99")
                                {
                                    NImp += 1;
                                    Prog += 1;
                                    ImpTicket = decimal.Parse(tempLineValue.Substring(131, 9).Replace(".", ","));
                                    Ticket += ImpTicket;
                                    ImpNetto = decimal.Parse(tempLineValue.Substring(140, 9).Replace(".", ","));
                                    Netto += ImpNetto;

                                    if (ImpLordo - ImpTicket != ImpNetto)
                                        MessageBox.Show("Riscontrata anomalia Lordo-Ticket<>Netto alla riga: " + NRec.ToString());
                                    ImpLordo = 0;
                                    NEsaImp = 0;
                                }
                                else
                                {
                                    NEsaImp += 1;
                                    if (NEsaImp > 9)
                                        MessageBox.Show("Superato limite prestazioni alla riga: " + NRec.ToString());
                                    Qt = int.Parse(tempLineValue.Substring(128, 3));
                                    if (Qt == 0)
                                        MessageBox.Show("Riscontrata anomalia Quantità esami=0 alla riga: " + NRec.ToString());
                                    NEsa += Qt;
                                    ImpLordo += decimal.Parse(tempLineValue.Substring(140, 9).Replace(".", ","));
                                    Lordo += decimal.Parse(tempLineValue.Substring(140, 9).Replace(".", ","));

                                }
                            }
                            else if (lblFile1.Text.IndexOf("SPA") > 0)
                            {
                                //aStringBuilder.Remove(25, 20);
                                //aStringBuilder.Insert(25, "60830120200801" + Prog.ToString("000000")); //Citotest
                                //aStringBuilder.Insert(25, "20200050640801" + Prog.ToString("000000")); //Fleming
                                //aStringBuilder.Insert(25, "56308920200801" + Prog.ToString("000000")); //Selab
                                //aStringBuilder.Insert(25, "60580220200901" + Prog.ToString("000000")); //CMV
                                //aStringBuilder.Insert(25, "64214520200801" + Prog.ToString("000000")); //Emolab

                                Prog += 1;
                            }
                            tempLineValue = aStringBuilder.ToString();
                            outputWriter.WriteLine(tempLineValue);

                            progressBar1.Value = (int)(baseStream.Position / length * 100);
                            progressBar1.Refresh();
                        }
                    }
                }
            }
            txtNumRecord.Text = NRec.ToString();
            txtNumImp.Text = NImp.ToString();
            txtNumEsami.Text = NEsa.ToString();
            txtLordo.Text = Lordo.ToString("#.##");
            txtTicket.Text = Ticket.ToString("#.##");
            txtNetto.Text = Netto.ToString("#.##");

        }
        private void ElaboraLazio()
        {
            if (chkRinumera.Checked)
            {
                string tempLineValue;
                int Prog = int.Parse("0" + txtStartProg.Text);
                int NRec = 0, NImp = 0, NEsaImp = 0, NEsa = 0, Qt = 0;
                decimal Lordo = 0, Ticket = 0, Netto = 0, ImpLordo = 0, ImpTicket = 0, ImpNetto = 0;

                AzzeraContatori();
                string sMergeFileName = chkUnisci.Checked ? Path.GetDirectoryName(lblFile1.Text) + "\\merge\\" + Path.GetFileName(lblFile1.Text) : lblFile1.Text;

                using (FileStream inputStream = File.OpenRead(sMergeFileName))
                {
                    using (StreamReader inputReader = new StreamReader(inputStream))
                    {
                        Stream baseStream = inputReader.BaseStream;
                        long length = baseStream.Length;


                        using (StreamWriter outputWriter = File.AppendText(sMergeFileName + ".num"))
                        {
                            while (null != (tempLineValue = inputReader.ReadLine()))
                            {
                                var aStringBuilder = new StringBuilder(tempLineValue);
                                NRec += 1;
                                aStringBuilder.Remove(0, 7);
                                aStringBuilder.Insert(0, Prog.ToString("0000000"));
                                Prog += 1;
                                //Verifica se si tratta del file Sanitario
                                if (lblFile1.Text.IndexOf("S") > 0)
                                {
                                    if (tempLineValue.Substring(82, 2) == "99")
                                    {
                                        NImp += 1;
                                        ImpTicket = decimal.Parse(tempLineValue.Substring(109, 7).Replace(".", ","));
                                        Ticket += ImpTicket;
                                        ImpNetto = decimal.Parse(tempLineValue.Substring(116, 7).Replace(".", ","));
                                        Netto += ImpNetto;

                                        if (ImpLordo - ImpTicket != ImpNetto)
                                            MessageBox.Show("Riscontrata anomalia Lordo-Ticket<>Netto alla riga: " + NRec.ToString());
                                        ImpLordo = 0;
                                        NEsaImp = 0;
                                    }
                                    else
                                    {
                                        NEsaImp += 1;
                                        if (NEsaImp > 9)
                                            MessageBox.Show("Superato limite prestazioni alla riga: " + NRec.ToString());
                                        Qt = int.Parse(tempLineValue.Substring(99, 2));
                                        if (Qt == 0)
                                            MessageBox.Show("Riscontrata anomalia Quantità esami=0 alla riga: " + NRec.ToString());
                                        NEsa += Qt;
                                        ImpLordo += decimal.Parse(tempLineValue.Substring(116, 7).Replace(".", ","));
                                        Lordo += decimal.Parse(tempLineValue.Substring(116, 7).Replace(".", ","));
                                    }
                                }
                                tempLineValue = aStringBuilder.ToString();
                                outputWriter.WriteLine(tempLineValue);

                                progressBar1.Value = (int)(baseStream.Position / length * 100);
                                progressBar1.Refresh();
                            }
                        }
                        
                    }
                }
                //Elimina il file di partenza e rinomina il file .num 
                if (File.Exists(sMergeFileName + ".num"))
                {
                    File.Delete(sMergeFileName);
                    File.Move(sMergeFileName + ".num", sMergeFileName);
                }

                txtNumRecord.Text = NRec.ToString();
                txtNumImp.Text = NImp.ToString();
                txtNumEsami.Text = NEsa.ToString();
                txtLordo.Text = Lordo.ToString("#.##");
                txtTicket.Text = Ticket.ToString("#.##");
                txtNetto.Text = Netto.ToString("#.##");


            }
        }

        private static void CombineMultipleFilesIntoSingleFile(string inputDirectoryPath, string[] inputFilePaths, string outputFilePath)
        {
            Console.WriteLine("Number of files: {0}.", inputFilePaths.Length);
            using (var outputStream = File.Create(outputFilePath))
            {
                foreach (var inputFilePath in inputFilePaths)
                {
                    using (var inputStream = File.OpenRead(inputFilePath))
                    {
                        // Buffer size can be passed as the second argument.
                        inputStream.CopyTo(outputStream);
                    }
                    Console.WriteLine("The file {0} has been processed.", inputFilePath);
                }
            }
        }

        private void cmdElabora_Click(object sender, EventArgs e)
        {
            if (chkUnisci.Checked)
            {
                string[] tmpFiles = new string[] { lblFile1.Text, lblFile2.Text };
                //Crea una sottocartella merge
                string sMergeFileName = Path.GetDirectoryName(lblFile1.Text) + "\\merge";
                if (!Directory.Exists(sMergeFileName))
                {
                    DirectoryInfo di = Directory.CreateDirectory(sMergeFileName);
                }
                sMergeFileName += "\\" + Path.GetFileName(lblFile1.Text);
                CombineMultipleFilesIntoSingleFile(Path.GetDirectoryName(lblFile1.Text), tmpFiles, sMergeFileName);
            }
            switch (cmbTracciato.Text)
            {
                case "Veneto":
                    ElaboraVeneto();
                    break;
                case "Lazio":
                    ElaboraLazio();
                    break;
            }
            MessageBox.Show("Elaborazione terminata!", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void frmFlussi_Load(object sender, EventArgs e)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            this.Text += " " + version;
        }
    }
}
