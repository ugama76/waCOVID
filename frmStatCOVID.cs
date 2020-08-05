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
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using Microsoft.Win32.SafeHandles;

namespace waCOVID
{
    public partial class frmStatCOVID : Form
    {
        #region VARIABILI
        private enum enumTipoTracciato
        {
            ATS,
            AUSLPC,
            EMILIA,
            PIEMONTE,
            LIGURIA
        }
        private enum enumTipoEsame
        {
            Tampone,
            TestRapido,
            IgG,
            IgM,
            IgA,
            IgTot
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
            string Fulltext, tmpEsame;
            if (lblFileOrigine.Text.Trim() != "")
            {
                this.Cursor = Cursors.WaitCursor;
                SQLiteConnection conn = new SQLiteConnection("data source=" + Path.GetDirectoryName(Application.ExecutablePath) + @"\db\Istat.s3db; Version=3;");
                conn.Open();
                using (StreamReader sr = new StreamReader(lblFileOrigine.Text))
                {
                    int kEsclusi = 0;
                    while (!sr.EndOfStream)
                    {
                        Fulltext = sr.ReadToEnd().ToString(); //read full file text  
                        string[] rows = Fulltext.Split('\n'); //split full file text into rows  
                        progressBar1.Maximum = rows.Count();
                        for (int i = 0; i < rows.Count() - 1; i++)
                        {
                            progressBar1.Refresh();
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
                                        tmpEsame = dr["Test_utilizzato"].ToString();
                                        dr["ASST-Laboratorio"] = optTradate.Checked ? "ATS DELL'INSUBRIA" : "ATS DELLA VAL PADANA"; //add other columns
                                        dr["Ente_richiedente"] = ""; //add other columns
                                        if (dr["Risultato"].ToString() == "" && dr["Data Referto"].ToString() == "")
                                        {
                                            //if (dr["Data Referto"].ToString() != "")
                                            //    dr["Esito"] = "NEGATIVO";
                                            //else
                                            dr["Esito"] = "IN CORSO";
                                        }
                                        else
                                        {
                                            dr["Esito"] = GetEsito(tmpEsame, dr["Risultato"].ToString(), dr["CodRi"].ToString()); //add other columns
                                        }
                                        dr["Data Inizio Sintomi"] = "";
                                        dr["Ospedale di Provenienza"] = "";
                                        if (dr["Punto accesso"].ToString().ToUpper().Contains("MDL"))
                                        {
                                            dr["Setting"] = optTamponi.Checked ? "15_Mcomp" : "Pri_azi"; //add other columns
                                        }
                                        else
                                        {
                                            if (tmpEsame.Substring(0, tmpEsame.IndexOf(" ")) == "COVR") //come da mail Elena Frontini del 19.06
                                                dr["Setting"] = "Pri_citt";
                                            else
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
                                        tmpEsame = dr["Test_utilizzato"].ToString();
                                        if (dr["Risultato"].ToString() == "" && dr["RisDesc"].ToString() == "")
                                        {
                                            if (dr["Data Referto"].ToString() != "")
                                                dr["Esito"] = "NEGATIVO";
                                            else
                                                dr["Esito"] = "IN CORSO";
                                        }
                                        else
                                        {
                                            if (tmpEsame == "COVID")
                                                dr["Esito"] = GetEsito(tmpEsame, dr["RisDesc"].ToString(), dr["CodRi"].ToString()); //add other columns
                                            else
                                                dr["Esito"] = GetEsito(tmpEsame, dr["Risultato"].ToString(), dr["CodRi"].ToString()); //add other columns
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
                                        dtCsv.Columns.Add("dt_richiesta"); //add other columns
                                        dtCsv.Columns.Add("cod_test"); //add other columns
                                        dtCsv.Columns.Add("tipo_test"); //add other columns
                                        dtCsv.Columns.Add("Igm_ris"); //add other columns
                                        dtCsv.Columns.Add("Igg_ris"); //add other columns
                                        dtCsv.Columns.Add("es_ris"); //add other columns
                                        dtCsv.Columns.Add("cod_lab"); //add other columns
                                        dtCsv.Columns.Add("dt_val"); //add other columns
                                        dtCsv.Columns.Add("Rif_MMG_MC"); //add other columns
                                        dtCsv.Columns.Add("dat_lavoro"); //add other columns

                                    }
                                    else
                                    {
                                        DataRow dr = dtCsv.NewRow();
                                        for (int k = 0; k < rowValues.Count(); k++)
                                        {
                                            dr[k] = rowValues[k].ToString().Trim();
                                        }
                                        tmpEsame = dr["Test_utilizzato"].ToString();
                                        dr["cod_test"] = GetCUR(prmTipo, tmpEsame);
                                        if (tmpEsame == "COVID")
                                        {
                                            //RNA
                                            dr["Esito"] = GetEsito(tmpEsame, dr["RisDesc"].ToString(), dr["CodRi"].ToString()); //add other columns
                                            if (dr["Esito"].ToString().IndexOf("POSITIVO") != -1)
                                                dr["es_ris"] = "pos";
                                            else if (dr["Esito"].ToString().IndexOf("NEGATIVO") != -1)
                                                dr["es_ris"] = "neg";
                                        }
                                        else
                                        {
                                            //Sierologici
                                            dr["Esito"] = GetEsito(tmpEsame, dr["Risultato"].ToString(), dr["CodRi"].ToString()); //add other columns
                                        }
                                        dr["tipo_test"] = GetCUR(prmTipo, tmpEsame, true);
                                        dr["BCP"] = GetLabRif(dr["Punto Accesso"].ToString()); //add other columns
                                        dr["dt_richiesta"] = dr["Data Accesso"];
                                        dr["dt_val"] = dr["Data Referto"];
                                        dr["cod_lab"] = "Lifebrain Emilia-Romagna S.r.l.";
                                        if (dr["id_richiesta"].ToString().Trim() != "") //16.06.2020 Esclude i preventivi
                                            dtCsv.Rows.Add(dr); //add other rows  
                                    }
                                    break;
                                case enumTipoTracciato.PIEMONTE:
                                    if (i == 0)
                                    {
                                        for (int j = 0; j < rowValues.Count(); j++)
                                        {
                                            dtCsv.Columns.Add(rowValues[j].Trim()); //add headers  
                                        }
                                        dtCsv.Columns.Add("Esito"); //add other columns
                                        dtCsv.Columns.Add("BCP"); //add other columns
                                        dtCsv.Columns.Add("descrStruttura"); //add other columns
                                        dtCsv.Columns.Add("idStruttura"); //add other columns
                                        dtCsv.Columns.Add("matrStruttura"); //add other columns
                                        dtCsv.Columns.Add("idAsr"); //add other columns
                                        dtCsv.Columns.Add("aslAppartenenza"); //add other columns
                                        dtCsv.Columns.Add("tipoRichiesta"); //add other columns
                                        dtCsv.Columns.Add("legalauthenticator"); //add other columns
                                        dtCsv.Columns.Add("id_aura"); //add other columns
                                        dtCsv.Columns.Add("Domicilio"); //add other columns
                                        dtCsv.Columns.Add("indirizzoDomicilio"); //add other columns
                                        dtCsv.Columns.Add("code"); //add other columns
                                        dtCsv.Columns.Add("displayName"); //add other columns
                                        dtCsv.Columns.Add("effectiveTime"); //add other columns
                                        dtCsv.Columns.Add("esitoCode"); //add other columns
                                        dtCsv.Columns.Add("esitoDesc"); //add other columns
                                        dtCsv.Columns.Add("Unit"); //add other columns
                                        dtCsv.Columns.Add("Value"); //add other columns
                                        dtCsv.Columns.Add("ReferenceRange"); //add other columns
                                    }
                                    else
                                    {
                                        DataRow dr = dtCsv.NewRow();
                                        for (int k = 0; k < rowValues.Count(); k++)
                                        {
                                            dr[k] = rowValues[k].ToString().Trim();
                                        }
                                        tmpEsame = dr["Test_utilizzato"].ToString();
                                        if (dr["Risultato"].ToString().Trim() == "" && dr["CodRi"].ToString().Trim() == ""
                                            && (dr["RisDesc"].ToString().Trim() == "" || dr["RisDesc"].ToString().Trim() == "." || dr["RisDesc"].ToString().Trim() == ".."))
                                        {
                                            kEsclusi += 1;
                                            lblStato.Text = "Risultati esclusi: " + kEsclusi.ToString();
                                            lblStato.Refresh();
                                            progressBar1.Value += 1;
                                            continue;   //Non prende in considerazione gli esami senza risultato che hanno scaturito un test di approfondimento
                                        }
                                        else
                                        {
                                            if (tmpEsame.Substring(0, tmpEsame.IndexOf(" ")) == "COVID")
                                            {
                                                dr["esitoDesc"] = GetEsito(tmpEsame, dr["RisDesc"].ToString(), dr["CodRi"].ToString()); //add other columns
                                                if (dr["esitoDesc"].ToString() == "DEBOLMENTE POSITIVO")
                                                    dr["esitoDesc"] = "DUBBIO";
                                            }
                                            else
                                            {
                                                dr["esitoDesc"] = GetEsito(tmpEsame, dr["Risultato"].ToString(), dr["CodRi"].ToString()); //add other columns
                                            }
                                            switch (dr["esitoDesc"].ToString())
                                            {
                                                case "NEGATIVO":
                                                    dr["esitoCode"] = "N";
                                                    break;
                                                case "POSITIVO":
                                                    dr["esitoCode"] = "P";
                                                    break;
                                                case "NON PERVENUTO":
                                                    dr["esitoCode"] = "NP";
                                                    break;
                                                case "DUBBIO":
                                                    dr["esitoCode"] = "D";
                                                    break;
                                                case "INDTERMINATO":
                                                    dr["esitoCode"] = "I";
                                                    break;
                                            }
                                        }
                                        if (dr["Punto accesso"].ToString().ToUpper().Contains("MDL"))
                                            dr["tipoRichiesta"] = "01"; //SORVEGLIANZA MEDICO COMPETENTE
                                        else
                                            dr["tipoRichiesta"] = "09"; //ESAMI VOLONTARI CITTADINO
                                        //dr["id_documento"] = "2.16.840.1.113883.2.9.2.10.4.4.129990010" + dr["id_documento"].ToString(); //per il campo Id Documento, hanno assegnato dalla regione il prefisso "2.16.840.1.113883.2.9.2.10.4.4.129990010000000000000"
                                        dr["id_documento"] = "2.16.840.1.113883.2.9.2.10.4.4.129990010000000000000" + dr["id_documento"].ToString(); //per il campo Id Documento, hanno assegnato dalla regione il prefisso "2.16.840.1.113883.2.9.2.10.4.4.129990010000000000000"

                                        //Recupera Data e Luogo di nascita dal Codice Fiscale
                                        DateTime dNas;
                                        if (!DateTime.TryParse(dr["dataDiNascita"].ToString(), out dNas))
                                            dr["dataDiNascita"] = GetDataNascitaFromCF(dr["CodFisc"].ToString());
                                        dNas = Convert.ToDateTime(dr["dataDiNascita"]);
                                        dr["comuneDiNascita"] = GetLuogoNascitaFromCF(dr["CodFisc"].ToString(), dNas, conn);
                                        if (dr["Residenza"].ToString().Trim() != "")
                                            dr["Residenza"] = dr["Residenza"];
                                        dr["Domicilio"] = dr["Residenza"];
                                        dr["indirizzoDomicilio"] = dr["indirizzoResidenza"];
                                        dr["BCP"] = "PIEMONTE"; //GetLabRif(dr["Punto Accesso"].ToString()); //add other columns
                                        dr["descrStruttura"] = "Lifebrain Piemonte S.r.l."; //add other columns
                                        //dr["aslAppartenenza"] = "213"; //ASL Alessandria
                                        if (dr["U.S.L."].ToString().Length == 6)
                                            dr["aslAppartenenza"] = dr["U.S.L."].ToString().Substring(3, 3);
                                        else if (dr["Residenza"].ToString() != "")
                                        {
                                            dr["aslAppartenenza"] = GetASL(dr["Residenza"].ToString(), conn); //ASL paziente
                                            if (dr["aslAppartenenza"].ToString() == "" && dr["Residenza"].ToString().Substring(0, 3) == "999")
                                                dr["aslAppartenenza"] = "213"; //Se non è stato in grado di trovarla e il paziente è straniero mette quella del laboratorio ASL Alessandria
                                        }
                                        dr["code"] = GetCUR(prmTipo, tmpEsame);
                                        switch (dr["code"].ToString())
                                        {
                                            case "91.12.S":
                                                dr["displayName"] = "tampone";
                                                break;
                                            case "91.31.c":
                                                dr["displayName"] = "test sierologico igg";
                                                break;
                                            case "91.31.d":
                                                dr["displayName"] = "test sierologico igm";
                                                break;
                                        }
                                        //dr["displayName"] = (tmpEsame.Substring(tmpEsame.IndexOf(" "), tmpEsame.Length - tmpEsame.IndexOf(" "))).Trim();
                                        if (DateTime.TryParse(dr["Data Refertazione"].ToString(), out dNas))
                                        {
                                            string sOraEsame = dr["OraEsame"].ToString().Replace(".", ":");
                                            if (sOraEsame != "")
                                            {
                                                TimeSpan time = TimeSpan.Parse(sOraEsame);
                                                dNas = dNas.Add(time);
                                            }
                                            dr["effectiveTime"] = dNas.ToString("yyyyMMddHHmmss+0000");
                                        }
                                        if (chkCFRefertante.Checked)
                                            dr["legalauthenticator"] = txtCFRefertante.Text;
                                        if (dr["id_documento"].ToString().Trim() != "") //16.06.2020 Esclude i preventivi
                                            dtCsv.Rows.Add(dr); //add other rows  
                                    }
                                    break;
                                case enumTipoTracciato.LIGURIA:
                                    if (i == 0)
                                    {
                                        for (int j = 0; j < rowValues.Count(); j++)
                                        {
                                            dtCsv.Columns.Add(rowValues[j].Trim()); //add headers  
                                        }
                                        /*
                                         * AZIENDA_INVIANTE; DESC_AZIENDA_INVIANTE; TIPOLOGIA_PAZIENTE; COGNOME; NOME; SESSO; DATA_NASCITA; DATA_DECESSO; COD_ISTAT_NASCITA; COMUNE_NASCITA; TIPO_IDENTIFICATIVO; 
                                         * CODICE_FISCALE; COD_ISTAT_RESIDENZA; COMUNE_RESIDENZA; REGIONE_RESIDENZA; VIA_RESIDENZA; CIVICO_RESIDENZA; CAP_RESIDENZA; DESC_REGIONE_RESIDENZA; COD_ISTAT_DOMICILIO; 
                                         * COMUNE_DOMICILIO; VIA_DOMICILIO; CIVICO_DOMICILIO; CAP_DOMICILIO; ID_RICHIESTA_PK; DATA_PRELIEVO; DATA_ACCETTAZIONE; DESC_PROVENIENZA_CAMPIONE; ID_CAMPIONE; ID_MATERIALE; 
                                         * DESC_MATERIALE; CODICE_ANALISI; DESC_ANALISI; CODICE_CUR; PROG_ESITO; ESITO; DESC_ESITO; METODICA; LOINC; RISULTATO_NUMERO; DESC_RISULTATO; DATA_ESECUZIONE; DATA_REFERTAZIONE; DATA_ULT_MOD; 
                                         */
                                        dtCsv.Columns.Add("BCP"); //add other columns
                                        dtCsv.Columns.Add("AZIENDA_INVIANTE"); //add other columns
                                        dtCsv.Columns.Add("DESC_AZIENDA_INVIANTE"); //add other columns
                                        dtCsv.Columns.Add("TIPOLOGIA_PAZIENTE"); //add other columns
                                        dtCsv.Columns.Add("DATA_DECESSO"); //add other columns
                                        dtCsv.Columns.Add("COD_ISTAT_NASCITA"); //add other columns
                                        dtCsv.Columns.Add("TIPO_IDENTIFICATIVO"); //add other columns
                                        dtCsv.Columns.Add("REGIONE_RESIDENZA"); //add other columns
                                        dtCsv.Columns.Add("CIVICO_RESIDENZA"); //add other columns
                                        dtCsv.Columns.Add("CAP_RESIDENZA"); //add other columns
                                        dtCsv.Columns.Add("DESC_REGIONE_RESIDENZA"); //add other columns
                                        dtCsv.Columns.Add("COD_ISTAT_DOMICILIO"); //add other columns
                                        dtCsv.Columns.Add("COMUNE_DOMICILIO"); //add other columns
                                        dtCsv.Columns.Add("VIA_DOMICILIO"); //add other columns
                                        dtCsv.Columns.Add("CIVICO_DOMICILIO"); //add other columns
                                        dtCsv.Columns.Add("CAP_DOMICILIO"); //add other columns
                                        dtCsv.Columns.Add("DATA_PRELIEVO"); //add other columns
                                        dtCsv.Columns.Add("DESC_MATERIALE"); //add other columns
                                        dtCsv.Columns.Add("CODICE_ANALISI"); //add other columns
                                        dtCsv.Columns.Add("DESC_ANALISI"); //add other columns
                                        dtCsv.Columns.Add("CODICE_CUR"); //add other columns
                                        dtCsv.Columns.Add("PROG_ESITO"); //add other columns
                                        dtCsv.Columns.Add("ESITO"); //add other columns
                                        dtCsv.Columns.Add("DESC_ESITO"); //add other columns
                                        dtCsv.Columns.Add("LOINC"); //add other columns
                                        dtCsv.Columns.Add("RISULTATO_NUMERO"); //add other columns
                                        dtCsv.Columns.Add("DESC_RISULTATO"); //add other columns
                                        dtCsv.Columns.Add("DATA_ULT_MOD"); //add other columns
                                    }
                                    else
                                    {
                                        DataRow dr = dtCsv.NewRow();
                                        for (int k = 0; k < rowValues.Count(); k++)
                                        {
                                            dr[k] = rowValues[k].ToString().Trim();
                                        }
                                        dr["TIPOLOGIA_PAZIENTE"] = "1"; //1 = Pazienti esterni/domiciliari
                                        dr["TIPO_IDENTIFICATIVO"] = "CF"; //CF, ENI, STP, TEAM
                                        //Recupera Data e Luogo di nascita dal Codice Fiscale
                                        DateTime dNas;
                                        if (!DateTime.TryParse(dr["DATA_NASCITA"].ToString(), out dNas))
                                            dr["DATA_NASCITA"] = GetDataNascitaFromCF(dr["CODICE_FISCALE"].ToString());
                                        dNas = Convert.ToDateTime(dr["DATA_NASCITA"]);
                                        dr["COMUNE_NASCITA"] = GetLuogoNascitaFromCF(dr["CODICE_FISCALE"].ToString(), dNas, conn);
                                        tmpEsame = dr["Test_utilizzato"].ToString();
                                        dr["CODICE_ANALISI"] = tmpEsame.Substring(0, tmpEsame.IndexOf(" "));
                                        dr["ESITO"] = dr["CODICE_ANALISI"];
                                        dr["DESC_ANALISI"] = tmpEsame.Substring(tmpEsame.IndexOf(" "));
                                        dr["DESC_ESITO"] = dr["DESC_ANALISI"];
                                        dr["PROG_ESITO"] = "1";
                                        if (tmpEsame == "COVID")
                                        {
                                            //RNA
                                            dr["RISULTATO_NUMERO"] = GetEsito(tmpEsame, dr["RisDesc"].ToString(), dr["CodRi"].ToString()); //add other columns
                                            dr["DESC_RISULTATO"] = dr["RISULTATO_NUMERO"];
                                            switch (dr["ESITO"].ToString())
                                            {
                                                case "POSITIVO":
                                                    dr["RISULTATO_NUMERO"] = "1";
                                                    break;
                                                case "NEGATIVO":
                                                    dr["RISULTATO_NUMERO"] = "2";
                                                    break;
                                                case "DEBOLMENTE POSITIVO":
                                                    dr["RISULTATO_NUMERO"] = "3";
                                                    break;
                                                case "ANNULATO":
                                                    dr["RISULTATO_NUMERO"] = "4";
                                                    dr["DESC_RISULTATO"] = "INDETERMINATO";
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            //Sierologici
                                            dr["RISULTATO_NUMERO"] = GetEsito(tmpEsame, dr["Risultato"].ToString(), dr["CodRi"].ToString()); //add other columns
                                            dr["DESC_RISULTATO"] = dr["RISULTATO_NUMERO"];
                                            switch (dr["ESITO"].ToString())
                                            {
                                                case "POSITIVO":
                                                    dr["RISULTATO_NUMERO"] = "11";
                                                    break;
                                                case "NEGATIVO":
                                                    dr["RISULTATO_NUMERO"] = "12";
                                                    break;
                                                case "DUBBIO":
                                                    dr["RISULTATO_NUMERO"] = "13";
                                                    dr["DESC_RISULTATO"] = "INDETERMINATO";
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                        dr["CODICE_CUR"] = GetCUR(prmTipo, tmpEsame, true);
                                        dr["BCP"] = GetLabRif(dr["Punto Accesso"].ToString()); //add other columns
                                        dr["DATA_ACCETTAZIONE"] += " " + dr["OraAcc"];
                                        dr["DATA_PRELIEVO"] = dr["DATA_ACCETTAZIONE"];
                                        dr["DATA_ESECUZIONE"] += " " + dr["OraEsame"];
                                        dr["DATA_ULT_MOD"] = dr["DATA_ESECUZIONE"];
                                        dr["DATA_REFERTAZIONE"] += " " + dr["OraRef"];
                                        dr["AZIENDA_INVIANTE"] = "002602";
                                        dr["DESC_AZIENDA_INVIANTE"] = "Lifebrain Liguria S.r.l.";
                                        if (dr["ID_RICHIESTA_PK"].ToString().Trim() != "") //16.06.2020 Esclude i preventivi
                                            dtCsv.Rows.Add(dr); //add other rows  
                                    }
                                    break;

                            }
                            progressBar1.Value += 1;
                        }
                    }
                }
                progressBar1.Refresh();
                conn.Close();
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

            //Se non ci sono elementi valutabili annulla il risultato onde evitare decodifiche errate
            if (prmCodRis == "" && prmRisultato == "")
                return "ANNULLATO";

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
                                case "NEG":
                                    tmpRis = "NEGATIVO";
                                    break;
                                case "TAR":
                                case "POS":
                                    tmpRis = "POSITIVO";
                                    break;
                                case "TARS":
                                case "DEBOP":
                                    tmpRis = "DEBOLMENTE POSITIVO";
                                    break;
                                case "ANN":
                                case "CNV":
                                case "CNP":
                                case "CNI":
                                case "INDET":
                                    tmpRis = "ANNULLATO";
                                    break;
                            }
                        }
                        break;
                    case "COVRAP_2":
                    case "COVRAP_3":
                        switch (prmCodRis)
                        {
                            case "NEG":
                                tmpRis = "NEGATIVO";
                                break;
                            case "POS":
                                tmpRis = "POSITIVO";
                                break;
                            case "00074":
                                tmpRis = "DUBBIO";
                                break;
                            case "":
                            case ".":
                            case "..":
                            case "ANN":
                            case "CNV":
                                tmpRis = "ANNULLATO";
                                break;
                        }
                        break;
                    case "COVG":
                    case "COVIG_3":
                    case "COVIG_5":
                        if (dRiu < (decimal)0.9)
                            tmpRis = "NEGATIVO";
                        else if (dRiu >= (decimal)0.9 && dRiu < (decimal)1.1)
                            tmpRis = "DUBBIO";
                        else if (dRiu >= (decimal)1.1)
                            tmpRis = "POSITIVO";
                        break;
                    case "COVIDM":
                        if (dRiu < (decimal)0.9)
                            tmpRis = "NEGATIVO";
                        else if (dRiu >= (decimal)0.9 && dRiu <= (decimal)1.1)
                            tmpRis = "DUBBIO";
                        else if (dRiu > (decimal)1.1)
                            tmpRis = "POSITIVO";
                        break;
                    case "COVIG_2":
                    case "COVIGS_2":
                        if (dRiu < (decimal)0.8)
                            tmpRis = "NEGATIVO";
                        else if (dRiu >= (decimal)0.8 && dRiu < (decimal)1.1)
                            tmpRis = "DUBBIO";
                        else if (dRiu >= (decimal)1.1)
                            tmpRis = "POSITIVO";
                        break;
                    case "COVIDG":
                        if (dRiu < (decimal)0.8)
                            tmpRis = "NEGATIVO";
                        else if (dRiu >= (decimal)0.8 && dRiu <= (decimal)1.1)
                            tmpRis = "DUBBIO";
                        else if (dRiu > (decimal)1.1)
                            tmpRis = "POSITIVO";
                        break;
                    case "COVM":
                    case "COVTG":
                    case "COVTGM":
                    case "COVR":
                    case "COVIGM":
                    case "COVIG_6":
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
            if (tmpRis == "")
                MessageBox.Show("Vuoto");
            return tmpRis;
        }
        private string GetCUR(enumTipoTracciato prmTipo, string prmEsame)
        {
            return GetCUR(prmTipo, prmEsame, false);
        }
        private string GetCUR(enumTipoTracciato prmTipo, string prmEsame, bool prmTipoEsame)
        {
            string tmpCodCUR = "";
            string tmpEsame = prmEsame.Substring(0, prmEsame.IndexOf(" "));
            if (prmTipoEsame == false)
            {
                switch (prmTipo)
                {
                    case enumTipoTracciato.ATS:
                        break;
                    case enumTipoTracciato.AUSLPC:
                        break;
                    case enumTipoTracciato.EMILIA:
                        switch (tmpEsame)
                        {
                            case "COVID": //Tamponi per la Ricerca RNA SARS-CoV-2
                                tmpCodCUR = "44";
                                break;
                            case "COVRAP": //Test Immunocromatografci
                                tmpCodCUR = "01";
                                break;
                            default: //Test CLIA O ELISA
                                tmpCodCUR = "02";
                                break;
                        }
                        break;
                    case enumTipoTracciato.PIEMONTE:
                        switch (tmpEsame)
                        {
                            case "COVID": //Tampone
                                tmpCodCUR = "91.12.S";
                                break;
                            case "COVRAP": //Test rapido
                                break;
                            case "COVG": //IgG
                            case "COVIGG":
                            case "COVIDG":
                            case "COVIGS_2":
                            case "COVIG_2":
                            case "COVIG_5":
                            case "COVRAP_2":
                                tmpCodCUR = "91.31.c";
                                break;
                            case "COVM": //IgM
                            case "COVIGM":
                            case "COVIDM":
                            case "COVIGS_3":
                            case "COVIG_3":
                            case "COVIG_6":
                            case "COVRAP_3":
                                tmpCodCUR = "91.31.d";
                                break;
                            case "COVT": //Ig Totali
                            case "COVTG":
                            case "COVTGM":
                            case "COVR":
                                break;
                            case "COVIGA": //IgA
                                break;
                        }
                        break;
                    case enumTipoTracciato.LIGURIA:
                        switch (tmpEsame)
                        {
                            case "COVID": //Tampone
                                tmpCodCUR = "C02238600";
                                break;
                            case "COVRAP": //Test rapido
                                break;
                            case "COVG": //IgG
                            case "COVIGG":
                            case "COVIDG":
                            case "COVIGS_2":
                            case "COVIG_2":
                            case "COVIG_5":
                            case "COVRAP_2":
                                tmpCodCUR = "C02242600";
                                break;
                            case "COVM": //IgM
                            case "COVIGM":
                            case "COVIDM":
                            case "COVIGS_3":
                            case "COVIG_3":
                            case "COVIG_6":
                            case "COVRAP_3":
                                tmpCodCUR = "C02242700";
                                break;
                            case "COVT": //Ig Totali
                            case "COVTG":
                            case "COVTGM":
                            case "COVR":
                                tmpCodCUR = "C02238600";
                                break;
                            case "COVIGA": //IgA
                                break;
                        }
                        break;

                    default:
                        break;
                }
            }
            else
            {
                switch (tmpEsame)
                {
                    case "COVID": //Tampone
                        tmpCodCUR = enumTipoEsame.Tampone.ToString();
                        break;
                    case "COVRAP": //Test rapido
                        tmpCodCUR = enumTipoEsame.TestRapido.ToString();
                        break;
                    case "COVG": //IgG
                    case "COVIGG":
                    case "COVIDG":
                    case "COVIGS_2":
                    case "COVIG_2":
                    case "COVIG_5":
                    case "COVRAP_2":
                        tmpCodCUR = enumTipoEsame.IgG.ToString();
                        break;
                    case "COVM": //IgM
                    case "COVIGM":
                    case "COVIDM":
                    case "COVIGS_3":
                    case "COVIG_3":
                    case "COVIG_6":
                    case "COVRAP_3":
                        tmpCodCUR = enumTipoEsame.IgM.ToString();
                        break;
                    case "COVT": //Ig Totali
                    case "COVTG":
                    case "COVTGM":
                    case "COVR":
                        tmpCodCUR = enumTipoEsame.IgTot.ToString();
                        break;
                    case "COVIGA": //IgA
                        tmpCodCUR = enumTipoEsame.IgA.ToString();
                        break;
                }
            }
            return tmpCodCUR;
        }
        private string GetASL(string prmResIstat, SQLiteConnection prmConn)
        {
            string tmpAsl = "";
            if (prmResIstat != "")
            {
                string sQry = "SELECT ASL FROM Comuni WHERE CodIstat='" + prmResIstat + "' ORDER BY DataInizio DESC";
                SQLiteCommand cmd = new SQLiteCommand(sQry, prmConn);
                var firstColumn = cmd.ExecuteScalar();
                if (firstColumn != null)
                    tmpAsl = firstColumn.ToString();
            }
            return tmpAsl;
        }
        private string GetDataNascitaFromCF(string prmCF)
        {
            try
            {
                Dictionary<string, string> month = new Dictionary<string, string>();
                // To Upper
                prmCF = prmCF.ToUpper();
                month.Add("A", "01");
                month.Add("B", "02");
                month.Add("C", "03");
                month.Add("D", "04");
                month.Add("E", "05");
                month.Add("H", "06");
                month.Add("L", "07");
                month.Add("M", "08");
                month.Add("P", "09");
                month.Add("R", "10");
                month.Add("S", "11");
                month.Add("T", "12");
                // Get Date
                string date = prmCF.Substring(6, 5);
                int y = int.Parse(date.Substring(0, 2));
                string yy = ((y < 9) ? "20" : "19") + y.ToString("00");
                string m = month[date.Substring(2, 1)];
                int d = int.Parse(date.Substring(3, 2));
                if (d > 31)
                    d -= 40;
                // Return Date
                return string.Format("{0}/{1}/{2}", d.ToString("00"), m, yy);
            }
            catch
            {
                return string.Empty;
            }
        }
        private string GetLuogoNascitaFromCF(string prmCF, DateTime prmDtNas, SQLiteConnection prmConn)
        {
            if (prmCF != "" && prmCF.Length == 16)
            {
                //Recupera il belfiore dal CF
                string tmpBelfiore = prmCF.Substring(11, 4);

                string sQry = "SELECT CodIstat FROM Comuni WHERE Belfiore='" + tmpBelfiore + "' AND '" + prmDtNas.ToString("yyyy-MM-dd") + "' BETWEEN DataInizio AND DataFine ORDER BY DataInizio DESC";
                SQLiteCommand cmd = new SQLiteCommand(sQry, prmConn);
                var firstColumn = cmd.ExecuteScalar();
                if (firstColumn != null)
                    return firstColumn.ToString(); //Restituisce la prima occorrenza
                else
                {
                    sQry = "SELECT CodIstat FROM Comuni WHERE Belfiore='" + tmpBelfiore + "' ORDER BY DataInizio DESC";
                    cmd = new SQLiteCommand(sQry, prmConn);
                    SQLiteDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        return dr["CodIstat"].ToString(); //Restituisce la prima occorrenza
                    }
                    else
                        MessageBox.Show("Nessun comune trovato con Belfiore: " + tmpBelfiore + "\r\n(C.F. paziente: " + prmCF + ")", "ERRORE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return "";
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

                            dtDataTable = dtDataTable.DefaultView.ToTable("Selected", false, "id_richiesta", "dt_richiesta", "cognome", "nome", "dt_nasc", "cod_fisc", "tel", "cod_test", "Igm_ris", "Igg_ris", "es_ris", "cod_lab", "dt_val", "mmg_mc", "Rif_mmg_mc", "dat_lavoro");
                        }
                    }
                    break;
                case enumTipoTracciato.PIEMONTE:
                    if (chkFileEsteso.Checked == false)
                    {
                        if (chkFileEsteso.Checked == false)
                        {
                            dv = dtDataTable.DefaultView;
                            dv.RowFilter = "esitoDesc<>'ANNULLATO'";
                            //dtDataTable = RemoveDuplicateRows(dv.ToTable(), "Codice Fiscale");

                            dtDataTable = dtDataTable.DefaultView.ToTable("Selected", false, "descrStruttura", "idStruttura", "matrStruttura", "idAsr", "aslAppartenenza", "tipoRichiesta", "legalauthenticator",
                                "id_aura", "codFisc", "cognome", "nome", "sesso", "dataDiNascita", "ComuneDiNascita", "Residenza", "indirizzoResidenza", "Domicilio", "indirizzoDomicilio", "id_documento",
                                "code", "displayName", "effectiveTime", "esitoCode", "esitoDesc", "Unit", "Value", "ReferenceRange");
                        }
                    }
                    break;
                case enumTipoTracciato.LIGURIA:
                    if (chkFileEsteso.Checked == false)
                    {
                        if (chkFileEsteso.Checked == false)
                        {
                            dv = dtDataTable.DefaultView;
                            dv.RowFilter = "DESC_RISULTATO<>'ANNULLATO'";
                            //dtDataTable = RemoveDuplicateRows(dv.ToTable(), "Codice Fiscale");

                            dtDataTable = dtDataTable.DefaultView.ToTable("Selected", false, "AZIENDA_INVIANTE", "DESC_AZIENDA_INVIANTE", "TIPOLOGIA_PAZIENTE", "COGNOME", "NOME", "SESSO", "DATA_NASCITA", "DATA_DECESSO", "COD_ISTAT_NASCITA", "COMUNE_NASCITA", "TIPO_IDENTIFICATIVO",
                                "CODICE_FISCALE", "COD_ISTAT_RESIDENZA", "COMUNE_RESIDENZA", "REGIONE_RESIDENZA", "VIA_RESIDENZA", "CIVICO_RESIDENZA", "CAP_RESIDENZA", "DESC_REGIONE_RESIDENZA", "COD_ISTAT_DOMICILIO", 
                                "COMUNE_DOMICILIO", "VIA_DOMICILIO", "CIVICO_DOMICILIO", "CAP_DOMICILIO", "ID_RICHIESTA_PK", "DATA_PRELIEVO", "DATA_ACCETTAZIONE", "DESC_PROVENIENZA_CAMPIONE", "ID_CAMPIONE", "ID_MATERIALE", 
                                "DESC_MATERIALE", "CODICE_ANALISI", "DESC_ANALISI", "CODICE_CUR", "PROG_ESITO", "ESITO", "DESC_ESITO", "METODICA", "LOINC", "RISULTATO_NUMERO", "DESC_RISULTATO", "DATA_ESECUZIONE", "DATA_REFERTAZIONE", "DATA_ULT_MOD");
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
            else if (optPiemonte.Checked)
                tmpTipo = enumTipoTracciato.PIEMONTE;
            else if (optLiguria.Checked)
                tmpTipo = enumTipoTracciato.LIGURIA;
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
                    case enumTipoTracciato.PIEMONTE:
                        //18.06 Abbiamo parlato con Alessandro Rosa che ci ha detto di non considerare i pazienti che effettuano le Ig Totali che quindi in caso
                        //di risultato negativo non verrano comunicati alla regione
                        WriteDtToCSV(tmpTipo, dt, Path.GetDirectoryName(Application.ExecutablePath)
                            + (chkFileEsteso.Checked ? "\\JLab Piemonte Covid " : "\\Piemonte Covid ")
                            + ".CSV");
                        break;
                    case enumTipoTracciato.LIGURIA:
                        WriteDtToCSV(tmpTipo, dt, Path.GetDirectoryName(Application.ExecutablePath)
                            + (chkFileEsteso.Checked ? "\\JLab Liguria Covid " : "\\Liguria Covid ")
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
            else if (lblFileOrigine.Text.IndexOf("COASL") > 0)
                optPiemonte.Checked = true;
            else if (lblFileOrigine.Text.IndexOf("ALISA") > 0)
                optLiguria.Checked = true;
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
            pnlTipoFile.Visible = optTradate.Checked || optViadana.Checked;
            pnlCFRefertante.Visible = optPiemonte.Checked;
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
