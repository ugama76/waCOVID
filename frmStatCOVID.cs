using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace waCOVID
{
    public partial class frmStatCOVID : Form
    {
        #region VARIABILI
        private enum enumTipoTracciato
        {
            ATS,
            AUSLPC,
            EMILIA
        }
        #endregion

        #region COSTRUTTORE
        public frmStatCOVID()
        {
            InitializeComponent();
        }
        #endregion

        #region METODI
        private void WriteValues()
        {
            using (var writer = new CsvFileWriter("WriteTest.csv"))
            {
                // Write each row of data
                for (int row = 0; row < 100; row++)
                {
                    // TODO: Populate column values for this row
                    List<string> columns = new List<string>();
                    writer.WriteRow(columns);
                }
            }
        }
        private void ReadValues()
        {
            if (lblFileOrigine.Text.Trim() != "")
            {
                List<string> columns = new List<string>();
                using (var reader = new CsvFileReader(lblFileOrigine.Text))
                {
                    using (var writer = new CsvFileWriter(Path.GetDirectoryName(Application.ExecutablePath) + "\\Test.CSV"))
                    {
                        while (reader.ReadRow(columns))
                        {
                            // TODO: Do something with columns' values
                            // Write each row of data
                            writer.WriteRow(columns);
                        }
                    }
                }
            }
            else
                MessageBox.Show("Selezionare un file", "ATTENZIONE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        private DataTable ReadCsvFile(enumTipoTracciato prmTipo)
        {
            progressBar1.Value = 0;
            progressBar1.Refresh();
            DataTable dtCsv = new DataTable();
            string Fulltext;
            if (lblFileOrigine.Text.Trim() != "")
            {
                this.Cursor = Cursors.WaitCursor;
                using (StreamReader sr = new StreamReader(lblFileOrigine.Text))
                {
                    while (!sr.EndOfStream)
                    {
                        Fulltext = sr.ReadToEnd().ToString(); //read full file text  
                        string[] rows = Fulltext.Split('\n'); //split full file text into rows  
                        progressBar1.Maximum = rows.Count();
                        for (int i = 0; i < rows.Count() - 1; i++)
                        {
                            string[] rowValues = rows[i].Split(';'); //split each row with semicolon to get individual values  
                            switch (prmTipo)
                            {
                                case enumTipoTracciato.ATS:
                                    if (i == 0)
                                    {
                                        for (int j = 0; j < rowValues.Count(); j++)
                                        {
                                            dtCsv.Columns.Add(rowValues[j].Trim()); //add headers  
                                        }
                                        dtCsv.Columns.Add("ASST-Laboratorio"); //add other columns
                                        dtCsv.Columns.Add("Ente_richiedente"); //add other columns
                                        dtCsv.Columns.Add("Data Inizio Sintomi"); //add other columns
                                        dtCsv.Columns.Add("Esito"); //add other columns
                                        dtCsv.Columns.Add("Ospedale di Provenienza"); //add other columns
                                        dtCsv.Columns.Add("Setting"); //add other columns
                                        dtCsv.Columns.Add("Provenienza"); //add other columns
                                        dtCsv.Columns.Add("Materiale"); //add other columns
                                        dtCsv.Columns.Add("BCP"); //add other columns
                                    }
                                    else
                                    {
                                        DataRow dr = dtCsv.NewRow();
                                        for (int k = 0; k < rowValues.Count(); k++)
                                        {
                                            dr[k] = rowValues[k].ToString().Trim();
                                        }
                                        dr["ASST-Laboratorio"] = optTradate.Checked ? "ATS DELL'INSUBRIA" : "ATS DELLA VAL PADANA"; //add other columns
                                        dr["Ente_richiedente"] = ""; //add other columns
                                        if (dr["Risultato"].ToString() == "")
                                        {
                                            if (dr["Data Referto"].ToString() != "")
                                                dr["Esito"] = "NEGATIVO";
                                            else
                                                dr["Esito"] = "IN CORSO";
                                        }
                                        else
                                        {
                                            dr["Esito"] = GetEsito(dr["Test_utilizzato"].ToString(), dr["Risultato"].ToString(), dr["CodRi"].ToString()); //add other columns
                                        }
                                        dr["Data Inizio Sintomi"] = "";
                                        dr["Ospedale di Provenienza"] = "";
                                        if (dr["Punto accesso"].ToString().ToUpper().Contains("MDL"))
                                        {
                                            dr["Setting"] = optTamponi.Checked ? "15_Mcomp" : "Pri_azi"; //add other columns
                                        }
                                        else
                                        {
                                            dr["Setting"] = optTamponi.Checked ? "17_territoriale" : "Pri_citt"; //add other columns
                                        }
                                        dr["Provenienza"] = optTamponi.Checked ? "" : ""; //add other columns
                                        dr["Materiale"] = optTamponi.Checked ? "BAL TNF" : "";
                                        dr["BCP"] = GetLabRif(dr["Punto Accesso"].ToString()); //add other columns
                                        if (dr["ID_accettazione"].ToString().Trim() != "") //16.06.2020 Esclude i preventivi
                                            dtCsv.Rows.Add(dr); //add other rows  
                                    }
                                    break;
                                case enumTipoTracciato.AUSLPC:
                                    if (i == 0)
                                    {
                                        for (int j = 0; j < rowValues.Count(); j++)
                                        {
                                            dtCsv.Columns.Add(rowValues[j].Trim()); //add headers  
                                        }
                                        dtCsv.Columns.Add("Esito"); //add other columns
                                        dtCsv.Columns.Add("tampone nasofaringeo privato"); //add other columns
                                    }
                                    else
                                    {
                                        DataRow dr = dtCsv.NewRow();
                                        for (int k = 0; k < rowValues.Count(); k++)
                                        {
                                            dr[k] = rowValues[k].ToString().Trim();
                                        }
                                        if (dr["Risultato"].ToString() == "" && dr["RisDesc"].ToString() == "")
                                        {
                                            if (dr["Data Referto"].ToString() != "")
                                                dr["Esito"] = "NEGATIVO";
                                            else
                                                dr["Esito"] = "IN CORSO";
                                        }
                                        else
                                        {
                                            if (dr["Test_utilizzato"].ToString() == "COVID")
                                                dr["Esito"] = GetEsito(dr["Test_utilizzato"].ToString(), dr["RisDesc"].ToString(), dr["CodRi"].ToString()); //add other columns
                                            else
                                                dr["Esito"] = GetEsito(dr["Test_utilizzato"].ToString(), dr["Risultato"].ToString(), dr["CodRi"].ToString()); //add other columns
                                        }
                                        if (dr["ID_accettazione"].ToString().Trim() != "") //16.06.2020 Esclude i preventivi
                                            dtCsv.Rows.Add(dr); //add other rows  
                                    }
                                    break;
                                case enumTipoTracciato.EMILIA:
                                    if (i == 0)
                                    {
                                        for (int j = 0; j < rowValues.Count(); j++)
                                        {
                                            dtCsv.Columns.Add(rowValues[j].Trim()); //add headers  
                                        }
                                        dtCsv.Columns.Add("Esito"); //add other columns
                                        dtCsv.Columns.Add("BCP"); //add other columns
                                    }
                                    else
                                    {
                                        DataRow dr = dtCsv.NewRow();
                                        for (int k = 0; k < rowValues.Count(); k++)
                                        {
                                            dr[k] = rowValues[k].ToString().Trim();
                                        }
                                        if (dr["Risultato"].ToString() == "" && dr["RisDesc"].ToString() == "")
                                        {
                                            if (dr["Data Referto"].ToString() != "")
                                                dr["Esito"] = "NEGATIVO";
                                            else
                                                dr["Esito"] = "IN CORSO";
                                        }
                                        else
                                        {
                                            if (dr["Test_utilizzato"].ToString() == "COVID")
                                                dr["Esito"] = GetEsito(dr["Test_utilizzato"].ToString(), dr["RisDesc"].ToString(), dr["CodRi"].ToString()); //add other columns
                                            else
                                                dr["Esito"] = GetEsito(dr["Test_utilizzato"].ToString(), dr["Risultato"].ToString(), dr["CodRi"].ToString()); //add other columns
                                        }
                                        dr["BCP"] = GetLabRif(dr["Punto Accesso"].ToString()); //add other columns
                                        if (dr["ID_accettazione"].ToString().Trim() != "") //16.06.2020 Esclude i preventivi
                                            dtCsv.Rows.Add(dr); //add other rows  
                                    }
                                    break;
                            }
                            progressBar1.Value += 1;
                            progressBar1.Refresh();
                        }
                    }
                }
                this.Cursor = Cursors.Default;
            }
            return dtCsv;
        }
        private string GetLabRif(string prmBCP)
        {
            if (prmBCP.Trim() != "")
            {
                var lstBCP = new List<string>
                {
                "590", "591", "592", "593", "594", "595", "596", "597", "598", "599", "600", "601",
                "602", "603", "604", "605", "606", "607", "608", "609", "610", "611", "612", "613",
                "614", "615", "616", "617", "618", "619", "620", "670", "672", "673", "674", "680",
                "681", "685", "686", "IV3" };

                var lstBCP_ER = new List<string>
                {
                "550", "552", "553", "554", "555", "556", "557", "559", "560", "561", "563", "565", "566",
                "567", "568", "569", "570", "571", "572", "573", "574", "575", "576", "579", "580", "581",
                "AC1", "AC2", "AC3", "AC4", "AC5", "ES1", "ES2", "ES3", "ES4", "ES5", "ME1", "ME2", "ME3",
                "ME4", "ME5", "ME6", "MN1", "MN2", "MN3", "MN4", "MN5", "MR1", "MR2", "MR3", "MR4", "MR5",
                "RV1", "RV2", "RV3", "RV4", "RV5", "SM1", "SM2", "SM3", "SM4", "SM5"
                };
                bool contains;
                string tmpBCP = prmBCP.Substring(0, prmBCP.IndexOf(" ")).Trim();
                if (optEmiliaRomagna.Checked)
                {
                    contains = lstBCP_ER.Contains(tmpBCP, StringComparer.OrdinalIgnoreCase);
                    if (contains)
                        return "EMILIA ROMAGNA";
                    else
                        return "";
                }
                else
                {
                    contains = lstBCP.Contains(tmpBCP, StringComparer.OrdinalIgnoreCase);
                    if (contains)
                        return optViadana.Text;
                    else
                        return optTradate.Text;
                }
            }
            else
                return prmBCP;


            #region OLD CODE
            //string tmpRis = "", tmpBCP = prmBCP.Substring(0, prmBCP.IndexOf(" ")).Trim();
            //switch (tmpBCP)
            //{
            //    // VIADANA
            //    case "590":
            //    case "591":
            //    case "592":
            //    case "593":
            //    case "594":
            //    case "595":
            //    case "596":
            //    case "597":
            //    case "598":
            //    case "599":
            //    case "600":
            //    case "601":
            //    case "602":
            //    case "603":
            //    case "604":
            //    case "605":
            //    case "606":
            //    case "607":
            //    case "608":
            //    case "609":
            //    case "610":
            //    case "611":
            //    case "612":
            //    case "613":
            //    case "614":
            //    case "615":
            //    case "616":
            //    case "617":
            //    case "618":
            //    case "619":
            //    case "620":
            //    case "670":
            //    case "672":
            //    case "673":
            //    case "674":
            //    case "680":
            //    case "681":
            //    case "685":
            //    case "686":
            //    case "IV3":
            //        tmpRis = optViadana.Text;
            //        break;
            //    default:
            //        tmpRis = optTradate.Text;
            //        break;
            //}
            //return tmpRis;
            #endregion
        }
        private string GetEsito(string prmEsame, string prmRisultato, string prmCodRis)
        {
            string tmpRis = "", tmpEsame;
            decimal.TryParse("0" + prmRisultato.Replace(".", ","), out decimal dRiu);
            tmpEsame = prmEsame.Substring(0, prmEsame.IndexOf(" "));
            if (prmCodRis == "AN" || prmCodRis == "ANN")
            {
                tmpRis = "ANNULLATO";
            }
            else
            {
                switch (tmpEsame)
                {
                    case "COVID":
                        if (prmRisultato.Trim() != "")
                        {
                            if (prmRisultato.IndexOf("non rilevato", StringComparison.CurrentCultureIgnoreCase) >= 0)
                                tmpRis = "NEGATIVO";
                            else if (prmRisultato.IndexOf("Singolo target", StringComparison.CurrentCultureIgnoreCase) >= 0)
                                tmpRis = "DEBOLMENTE POSITIVO";
                            else if (prmRisultato.IndexOf("Target RNA Coronavirus rilevat", StringComparison.CurrentCultureIgnoreCase) >= 0
                                || prmRisultato.IndexOf("Target rilevato", StringComparison.CurrentCultureIgnoreCase) >= 0)
                                tmpRis = "POSITIVO";
                            else if (prmRisultato.IndexOf("Campione invalido", StringComparison.CurrentCultureIgnoreCase) >= 0
                                || prmRisultato.IndexOf("Questo referto sostituisce", StringComparison.CurrentCultureIgnoreCase) >= 0)
                                tmpRis = "ANNULLATO";
                        }
                        else
                        {
                            switch (prmCodRis)
                            {
                                case "TNR":
                                    tmpRis = "NEGATIVO";
                                    break;
                                case "TAR":
                                    tmpRis = "POSITIVO";
                                    break;
                                case "TARS":
                                    tmpRis = "DEBOLMENTE POSITIVO";
                                    break;
                                case "ANN":
                                case "CNV":
                                    tmpRis = "ANNULLATO";
                                    break;
                            }
                        }
                        break;
                    case "COVG":
                        if (dRiu < (decimal)0.9)
                            tmpRis = "NEGATIVO";
                        else if (dRiu >= (decimal)0.9 && dRiu < (decimal)1.1)
                            tmpRis = "DUBBIO";
                        else if (dRiu >= (decimal)1.1)
                            tmpRis = "POSITIVO";
                        break;
                    case "COVIGS_2":
                        if (dRiu < (decimal)0.8)
                            tmpRis = "NEGATIVO";
                        else if (dRiu >= (decimal)0.8 && dRiu < (decimal)1.1)
                            tmpRis = "DUBBIO";
                        else if (dRiu >= (decimal)1.1)
                            tmpRis = "POSITIVO";
                        break;
                    case "COVM":
                    case "COVTG":
                    case "COVTGM":
                    case "COVR":
                        if (dRiu < (decimal)1)
                            tmpRis = "NEGATIVO";
                        else if (dRiu >= (decimal)1)
                            tmpRis = "POSITIVO";
                        break;
                    case "COVIGG":
                        if (dRiu < (decimal)12.0)
                            tmpRis = "NEGATIVO";
                        else if (dRiu >= (decimal)12 && dRiu < (decimal)15)
                            tmpRis = "DUBBIO";
                        else if (dRiu >= (decimal)15)
                            tmpRis = "POSITIVO";
                        break;
                }
            }
            return tmpRis;
        }
        private void WriteDtToCSV(enumTipoTracciato prmTipo, DataTable dtDataTable, string strFilePath)
        {
            DataView dv;
            this.Cursor = Cursors.WaitCursor;
            //Data filter 


            switch (prmTipo)
            {
                case enumTipoTracciato.ATS:
                    dv = dtDataTable.DefaultView;

                    if (chkFileEsteso.Checked == false)
                    {
                        dv.RowFilter = "BCP='" + (optTradate.Checked ? optTradate.Text : optViadana.Text) + "' AND Esito<>'ANNULLATO'";
                        dtDataTable = dv.ToTable();

                        if (optSierologici.Checked)
                        {
                            dtDataTable = dtDataTable.DefaultView.ToTable("Selected", false, "ID_accettazione", "Nome", "Cognome", "Data nascita", "CF", "Data_Ricezione", "Data Referto",
                                "Ente_richiedente", "Esito", "Telefono_Paziente", "Test_utilizzato", "Setting", "Provenienza");
                        }
                        else
                        {
                            dtDataTable = dtDataTable.DefaultView.ToTable("Selected", false, "ID_accettazione", "Nome", "Cognome", "Data nascita", "Sesso", "Comune Domicilio", "Data Inizio Sintomi",
                                "Data Ricezione", "Data Referto", "Ospedale di Provenienza", "Esito", "CF", "Telefono_Paziente", "Setting", "Provenienza", "Materiale");
                        }
                    }
                    break;
                case enumTipoTracciato.AUSLPC:
                    //Set column "tampone nasofaringeo privato"
                    dtDataTable = CheckCOVID(dtDataTable);

                    if (chkFileEsteso.Checked == false)
                    {
                        if (chkFileEsteso.Checked == false)
                        {
                            dv = dtDataTable.DefaultView;
                            dv.RowFilter = "Test_utilizzato NOT LIKE 'COVID %' AND ESITO IN ('POSITIVO', 'DEBOLMENTE POSITIVO', 'DUBBIO') AND Esito<>'ANNULLATO'";
                            dtDataTable = RemoveDuplicateRows(dv.ToTable(), "Codice Fiscale");

                            dtDataTable = dtDataTable.DefaultView.ToTable("Selected", false, "Nome", "Cognome", "Codice Fiscale", "Comune residenza", "Cellulare", "Mail", "tampone nasofaringeo privato");
                        }
                    }
                    break;
                case enumTipoTracciato.EMILIA:
                    if (chkFileEsteso.Checked == false)
                    {
                        if (chkFileEsteso.Checked == false)
                        {
                            dv = dtDataTable.DefaultView;
                            dv.RowFilter = "BCP='EMILIA ROMAGNA' AND ESITO IN ('POSITIVO', 'DEBOLMENTE POSITIVO', 'DUBBIO') AND Esito<>'ANNULLATO'";
                            //dtDataTable = RemoveDuplicateRows(dv.ToTable(), "Codice Fiscale");

                            //dtDataTable = dtDataTable.DefaultView.ToTable("Selected", false, "Nome", "Cognome", "Codice Fiscale", "Comune residenza", "Cellulare", "Mail", "tampone nasofaringeo privato");
                        }
                    }
                    break;
                default:
                    break;
            }



            StreamWriter sw = new StreamWriter(strFilePath, false);
            //headers  
            for (int i = 0; i < dtDataTable.Columns.Count; i++)
            {
                sw.Write(dtDataTable.Columns[i]);
                if (i < dtDataTable.Columns.Count - 1)
                    sw.Write(";");
            }
            sw.Write(sw.NewLine);
            foreach (DataRow dr in dtDataTable.Rows)
            {
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(';'))
                        {
                            value = String.Format("\"{0}\"", value);
                            sw.Write(value);
                        }
                        else
                        {
                            sw.Write(dr[i].ToString());
                        }
                    }
                    if (i < dtDataTable.Columns.Count - 1)
                        sw.Write(";");
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
            this.Cursor = Cursors.Default;
        }
        private void GeneraFile()
        {
            enumTipoTracciato tmpTipo;
            if (optPiacenza.Checked)
                tmpTipo = enumTipoTracciato.AUSLPC;
            else if (optEmiliaRomagna.Checked)
                tmpTipo = enumTipoTracciato.EMILIA;
            else
                tmpTipo = enumTipoTracciato.ATS;

            if (lblFileOrigine.Text.Trim() != "")
            {
                DataTable dt = ReadCsvFile(tmpTipo);

                switch (tmpTipo)
                {
                    case enumTipoTracciato.ATS:

                        //Filtrare i dati con le condizioni
                        WriteDtToCSV(tmpTipo, dt, Path.GetDirectoryName(Application.ExecutablePath)
                            + (chkFileEsteso.Checked ? "\\JLab " : "\\ATS ")
                            + (optTamponi.Checked ? "Tamponi " : "Sierologici ")
                            + (optTradate.Checked ? "TRADATE" : "VIADANA")
                            + ".CSV");
                        break;
                    case enumTipoTracciato.AUSLPC:

                        WriteDtToCSV(tmpTipo, dt, Path.GetDirectoryName(Application.ExecutablePath)
                            + (chkFileEsteso.Checked ? "\\JLab PC Sierologici " : "\\AUSL PC Sierologici ")
                            + ".CSV");
                        break;

                    case enumTipoTracciato.EMILIA:

                        WriteDtToCSV(tmpTipo, dt, Path.GetDirectoryName(Application.ExecutablePath)
                            + (chkFileEsteso.Checked ? "\\JLab ER Covid " : "\\AUSL ER Covid ")
                            + ".CSV");
                        break;
                    default:
                        break;
                }



                MessageBox.Show("Elaborazione terminata!", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Selezionare un file per procedere con l'elaborazione", "ATTENZIONE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        private DataTable CheckCOVID(DataTable prmDt)
        {
            DataTable tmpDtCovid = prmDt.Copy();
            if (tmpDtCovid != null && tmpDtCovid.Rows.Count > 0)
            {
                progressBar1.Maximum = prmDt.Rows.Count;
                progressBar1.Value = 0;
                progressBar1.Refresh();

                DataView dvCOVID = tmpDtCovid.DefaultView;
                dvCOVID.RowFilter = "[Codice Fiscale]<>'' AND Test_utilizzato LIKE 'COVID %' AND Esito<>'ANNULLATO'";
                tmpDtCovid = dvCOVID.ToTable();
                if (tmpDtCovid.Rows.Count > 0)
                {
                    for (int i = 0; i < prmDt.Rows.Count; i++)
                    {
                        if (tmpDtCovid.Select("[Codice Fiscale]='" + prmDt.Rows[i]["Codice Fiscale"].ToString() + "'").Length > 0)
                            prmDt.Rows[i]["tampone nasofaringeo privato"] = "Si";
                        else
                            prmDt.Rows[i]["tampone nasofaringeo privato"] = "No";
                        progressBar1.Value += 1;
                        progressBar1.Refresh();
                    }
                }
            }
            return prmDt;

        }
        public DataTable RemoveDuplicateRows(DataTable prmTable, string prmColName)
        {
            Hashtable hTable = new Hashtable();
            ArrayList duplicateList = new ArrayList();

            //Add list of all the unique item value to hashtable, which stores combination of key, value pair.
            //And add duplicate item value in arraylist.
            foreach (DataRow drow in prmTable.Rows)
            {
                if (hTable.Contains(drow[prmColName]))
                    duplicateList.Add(drow);
                else
                    hTable.Add(drow[prmColName], string.Empty);
            }

            //Removing a list of duplicate items from datatable.
            foreach (DataRow dRow in duplicateList)
                prmTable.Rows.Remove(dRow);

            //Datatable which contains unique records will be return as output.
            return prmTable;
        }

        #endregion

        #region EVENTI
        private void cmdSelFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            lblFileOrigine.Text = openFileDialog1.FileName;
            if (lblFileOrigine.Text.IndexOf("COPIA") > 0)
                optPiacenza.Checked = true;
            else if (lblFileOrigine.Text.IndexOf("COVEM") > 0)
                optEmiliaRomagna.Checked = true;
            else
            {
                if (optPiacenza.Checked == true)
                    optTradate.Checked = true;
                if (lblFileOrigine.Text.IndexOf("COATS") > 0)
                    optSierologici.Checked = true;
                else if (lblFileOrigine.Text.IndexOf("COTAM") > 0)
                    optTamponi.Checked = true;
            }
        }
        private void cmdGeneraFile_Click(object sender, EventArgs e)
        {
            GeneraFile();
        }
        private void optSede_CheckedChanged(object sender, EventArgs e)
        {
            pnlTipoFile.Visible = !optPiacenza.Checked || !optEmiliaRomagna.Checked;
        }
        private void frmStatCOVID_Load(object sender, EventArgs e)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            this.Text += " " + version;
        }
        #endregion


    }
}
