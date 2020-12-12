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
            LIGURIA,
            LIGURIA_TAR,
            PARMA,
            MODENA,
            SARDEGNA,
            CAMPANIA,
            PUGLIA,
            PUGLIA_V1,
            PUGLIA_V2,
            PUGLIA_DENUNCE_COVID,
            PUGLIA_DENUNCE_ANTIGENICI,
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
        private new string Right(string value, int length)
        {
            return value.Substring(value.Length - length);
        }
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
                    int kRecord = 0;
                    while (!sr.EndOfStream)
                    {
                        Fulltext = sr.ReadToEnd().ToString(); //read full file text  
                        string[] rows = Fulltext.Split('\n'); //split full file text into rows  
                        progressBar1.Maximum = rows.Count();
                        for (int i = 0; i < rows.Count() - 1; i++)
                        {
                            progressBar1.Refresh();
                            progressBar1.Value += 1;
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
                                        dtCsv.Columns.Add("Data Esecuzione"); //add other columns
                                        dtCsv.Columns.Add("Esito"); //add other columns
                                        dtCsv.Columns.Add("Ospedale di Provenienza"); //add other columns
                                        dtCsv.Columns.Add("Setting"); //add other columns
                                        dtCsv.Columns.Add("Provenienza"); //add other columns
                                        dtCsv.Columns.Add("Materiale"); //add other columns
                                        dtCsv.Columns.Add("BCP"); //add other columns
                                        dtCsv.Columns.Add("Link epidemiologico"); //add other columns
                                    }
                                    else
                                    {
                                        DataRow dr = dtCsv.NewRow();
                                        for (int k = 0; k < rowValues.Count(); k++)
                                        {
                                            dr[k] = rowValues[k].ToString().Trim();
                                        }
                                        dr["BCP"] = GetLabRif(dr["Punto Accesso"].ToString()); //add other columns
                                        //il reparto SN14630 viene escluso come mail della Maggiolini del 14-10-2020
                                        if (dr["BCP"].ToString() == "SN14630")
                                        {
                                            kEsclusi += 1;
                                            lblStato.Text = "Risultati esclusi reparto SN14630: " + kEsclusi.ToString();
                                            lblStato.Refresh();
                                            progressBar1.Value += 1;
                                            continue;   //Non prende in considerazione gli esami senza risultato che hanno scaturito un test di approfondimento
                                        }
                                        else
                                        {
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
                                                //Per i COVRAG dubbi trasforma l'esito in positivo
                                                if (tmpEsame.Substring(0, tmpEsame.IndexOf(" ")) == "COVRAG" && (dr["Esito"].ToString() == "DUBBIO" || dr["Esito"].ToString() == "DEBOLMENTE POSITIVO"))
                                                    dr["Esito"] = "POSITIVO";
                                            }
                                            dr["Data Inizio Sintomi"] = "";
                                            if (optAntigenici.Checked)
                                                dr["Data Esecuzione"] = dr["Data Ricezione"];

                                            //16.11 post segnalazione report ATS
                                            if (dr["Telefono_Paziente"].ToString().Trim() == "")
                                            {
                                                if (dr["Tel.Abitazione"].ToString().Trim() != "")
                                                    dr["Telefono_Paziente"] = dr["Tel.Abitazione"];
                                                else if (dr["Tel.Lavoro"].ToString().Trim() != "")
                                                    dr["Telefono_Paziente"] = dr["Tel.Lavoro"];
                                            }

                                            //dr["Ospedale di Provenienza"] = dr["Reparto"].ToString(); //16.11 post segnalazione report ATS
                                            //
                                            if (dr["Punto accesso"].ToString().ToUpper().Contains("MDL"))
                                            {
                                                dr["Ospedale di Provenienza"] = dr["Reparto"].ToString(); //16.11 post segnalazione report ATS

                                                if (optTamponi.Checked || optAntigenici.Checked)
                                                    dr["Setting"] = "15_Mcomp";
                                                else
                                                    dr["Setting"] = "7_Pri_azi";

                                            }
                                            else
                                            {
                                                if (optTradate.Checked) //Mail Nicoletta Fabucci del 18/11 
                                                    dr["Ospedale di Provenienza"] = "LIFEBRAIN LOMBARDIA SRL - TRADATE"; //16.11 post segnalazione report ATS
                                                else
                                                    dr["Ospedale di Provenienza"] = "LIFEBRAIN LOMBARDIA SRL - VIADANA"; //16.11 post segnalazione report ATS

                                                //13.08 Remmato questo if inseguito ad indicazioni di Elena Frontini come mail di lunedì 10/08/2020 14:41
                                                //if (tmpEsame.Substring(0, tmpEsame.IndexOf(" ")) == "COVR") //come da mail Elena Frontini del 19.06
                                                //    dr["Setting"] = "Pri_citt";
                                                //else
                                                //dr["Setting"] = optTamponi.Checked ? "17_territoriale" : "Pri_citt"; //remmato per la delibera 37779 di novembre 2020
                                                if (optTamponi.Checked || optAntigenici.Checked)
                                                    dr["Setting"] = "13_PZ_sin";
                                                else
                                                    dr["Setting"] = "6_Pri_citt ";

                                            }

                                            //Eliminato per la delibera 37779 di novembre 2020
                                            //if (tmpEsame.Substring(0, tmpEsame.IndexOf(" ")) == "COVRAG") //Protocollo G1.2020.0030821 del 09/09/2020
                                            ////    dr["Setting"] = "22_antig";
                                            //if (dr["ATS"].ToString().ToString() != "")
                                            //    dr["Provenienza"] = "ALTRO ATS " + dr["ATS"].ToString(); //16.11.2020 ATS di riferimento territoriale - post segnalazione report ATS
                                            //else
                                            dr["Provenienza"] = GetATSJLab(dr["ATS"].ToString());  //16.11.2020 ATS di riferimento territoriale - post segnalazione report ATS

                                            dr["Materiale"] = (optTamponi.Checked || optAntigenici.Checked) ? "TNF" : "";

                                            dr["Link epidemiologico"] = "NON NOTO";

                                            if (dr["ID_accettazione"].ToString().Trim() != "") //16.06.2020 Esclude i preventivi
                                                dtCsv.Rows.Add(dr); //add other rows  
                                        }
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
                                        if (tmpEsame == "COVID")
                                            dr["Esito"] = GetEsito(tmpEsame, dr["RisDesc"].ToString(), dr["CodRi"].ToString()); //add other columns
                                        else
                                            dr["Esito"] = GetEsito(tmpEsame, dr["Risultato"].ToString(), dr["CodRi"].ToString()); //add other columns
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
                                case enumTipoTracciato.MODENA:
                                    //Nome laboratorio; Altro laboratorio, specificare; Email laboratorio; Cognome assistito; Nome assistito; Data di nascita; Codice fiscale; Comune di residenza; Comune di domicilio; Data esecuzione test sierologico; Data refertazione test sierologico; Tipo prelievo; Test utilizzato; Specificare altro test; IgM pos / neg; IgM titolo; IgG pos / neg; IgG titolo; IgA pos / neg; IgA titolo; Note
                                    if (i == 0)
                                    {
                                        for (int j = 0; j < rowValues.Count(); j++)
                                        {
                                            dtCsv.Columns.Add(rowValues[j].Trim()); //add headers  
                                        }
                                        dtCsv.Columns.Add("Esito"); //add other columns
                                        dtCsv.Columns.Add("Nome laboratorio"); //add other columns
                                        dtCsv.Columns.Add("Altro laboratorio, specificare"); //add other columns
                                        dtCsv.Columns.Add("Email laboratorio"); //add other columns
                                        dtCsv.Columns.Add("Note"); //add other columns
                                        dtCsv.Columns.Add("Comune di domicilio"); //add other columns
                                        dtCsv.Columns.Add("Data esecuzione test sierologico"); //add other columns
                                        dtCsv.Columns.Add("Data refertazione test sierologico"); //add other columns
                                        dtCsv.Columns.Add("Specificare altro test"); //add other columns
                                        dtCsv.Columns.Add("Tipo prelievo"); //add other columns
                                        dtCsv.Columns.Add("Test utilizzato"); //add other columns
                                        dtCsv.Columns.Add("IgM pos / neg"); //add other columns
                                        dtCsv.Columns.Add("IgM titolo"); //add other columns
                                        dtCsv.Columns.Add("IgG pos / neg"); //add other columns
                                        dtCsv.Columns.Add("IgG titolo"); //add other columns
                                        dtCsv.Columns.Add("IgA pos / neg"); //add other columns
                                        dtCsv.Columns.Add("IgA titolo"); //add other columns
                                    }
                                    else
                                    {
                                        DataRow dr = dtCsv.NewRow();
                                        for (int k = 0; k < rowValues.Count(); k++)
                                        {
                                            dr[k] = rowValues[k].ToString().Trim();
                                        }
                                        dr["Nome laboratorio"] = "CDREDI PIACENZA";
                                        dr["Email laboratorio"] = "cdredi@lifebrain.it";
                                        dr["Tipo prelievo"] = "siero";
                                        dr["Comune di domicilio"] = dr["Comune di residenza"];
                                        dr["Data esecuzione test sierologico"] = dr["DataEsecuzione"];
                                        dr["Data refertazione test sierologico"] = dr["DATA_REFERTAZIONE"];
                                        tmpEsame = dr["Test_utilizzato"].ToString();
                                        dr["Test utilizzato"] = GetStrumento(tmpEsame);
                                        Enum.TryParse(GetCUR(prmTipo, tmpEsame, true), out enumTipoEsame tmpTipoEsame);

                                        if (tmpEsame.Substring(0, tmpEsame.IndexOf(" ")) == "COVID")
                                        {
                                            dr["Esito"] = GetEsito(tmpEsame, dr["RisDesc"].ToString(), dr["CodRi"].ToString());
                                        }
                                        else
                                        {
                                            dr["Esito"] = GetEsito(tmpEsame, dr["Risultato"].ToString(), dr["CodRi"].ToString()); //add other columns
                                        }
                                        switch (tmpTipoEsame)
                                        {
                                            case enumTipoEsame.Tampone:
                                                break;
                                            case enumTipoEsame.TestRapido:
                                                if (tmpEsame.Substring(0, tmpEsame.IndexOf(" ")) == "COVRAP_2")
                                                {
                                                    dr["IgG pos / neg"] = dr["Esito"].ToString();
                                                    dr["IgG titolo"] = dr["Risultato"].ToString();
                                                }
                                                else if (tmpEsame.Substring(0, tmpEsame.IndexOf(" ")) == "COVRAP_3")
                                                {
                                                    dr["IgM pos / neg"] = dr["Esito"].ToString();
                                                    dr["IgM titolo"] = dr["Risultato"].ToString();
                                                }
                                                break;
                                            case enumTipoEsame.IgTot:
                                                break;
                                            case enumTipoEsame.IgG:
                                                dr["IgG pos / neg"] = dr["Esito"].ToString();
                                                dr["IgG titolo"] = dr["Risultato"].ToString();
                                                break;
                                            case enumTipoEsame.IgM:
                                                dr["IgM pos / neg"] = dr["Esito"].ToString();
                                                dr["IgM titolo"] = dr["Risultato"].ToString();
                                                break;
                                            case enumTipoEsame.IgA:
                                                dr["IgA pos / neg"] = dr["Esito"].ToString();
                                                dr["IgA titolo"] = dr["Risultato"].ToString();
                                                break;
                                        }

                                        if (dr["Id_richiesta"].ToString().Trim() != "") //16.06.2020 Esclude i preventivi
                                            dtCsv.Rows.Add(dr); //add other rows  
                                    }
                                    break;
                                case enumTipoTracciato.PARMA:
                                    //CF_PAZIENTE; COGNOME; NOME; sesso; DATA_DI_NASCITA; COMUNE RESID; Telefono; data_segnalazione; test_rapido; risultato_test_rapido; test_sierologico; risultato_test_sierologico; tampone_nasofaringeo
                                    if (i == 0)
                                    {
                                        for (int j = 0; j < rowValues.Count(); j++)
                                        {
                                            dtCsv.Columns.Add(rowValues[j].Trim()); //add headers  
                                        }
                                        dtCsv.Columns.Add("Esito"); //add other columns
                                        dtCsv.Columns.Add("Telefono");//add other columns
                                        dtCsv.Columns.Add("data_segnalazione");//add other columns
                                        dtCsv.Columns.Add("test_rapido"); //add other columns
                                        dtCsv.Columns.Add("risultato_test_rapido"); //add other columns
                                        dtCsv.Columns.Add("test_sierologico"); //add other columns
                                        dtCsv.Columns.Add("risultato_test_sierologico"); //add other columns
                                        dtCsv.Columns.Add("tampone_nasofaringeo"); //add other columns
                                    }
                                    else
                                    {
                                        DataRow dr = dtCsv.NewRow();
                                        for (int k = 0; k < rowValues.Count(); k++)
                                        {
                                            dr[k] = rowValues[k].ToString().Trim();
                                        }
                                        DateTime dNas;
                                        tmpEsame = dr["Test_utilizzato"].ToString();
                                        dr["Telefono"] = dr["cellulare"].ToString();
                                        dr["data_segnalazione"] = DateTime.Now.Date.ToString("dd/MM/yyyy");
                                        dr["data_segnalazione"] = dr["DATA_REFERTAZIONE"];
                                        if (!DateTime.TryParse(dr["DATA_DI_NASCITA"].ToString(), out dNas))
                                            dr["DATA_DI_NASCITA"] = GetDataNascitaFromCF(dr["CF_PAZIENTE"].ToString());
                                        if (dr["DATA_DI_NASCITA"].ToString() != "")
                                            dNas = Convert.ToDateTime(dr["DATA_DI_NASCITA"]);
                                        Enum.TryParse(GetCUR(prmTipo, tmpEsame, true), out enumTipoEsame tmpTipoEsame);
                                        if (tmpEsame.Substring(0, tmpEsame.IndexOf(" ")) == "COVID")
                                        {
                                            dr["Esito"] = GetEsito(tmpEsame, dr["RisDesc"].ToString(), dr["CodRi"].ToString());
                                        }
                                        else
                                        {
                                            dr["Esito"] = GetEsito(tmpEsame, dr["Risultato"].ToString(), dr["CodRi"].ToString()); //add other columns
                                        }
                                        switch (tmpTipoEsame)
                                        {
                                            case enumTipoEsame.Tampone:
                                                dr["tampone_nasofaringeo"] = dr["Esito"].ToString();
                                                break;
                                            case enumTipoEsame.TestRapido:
                                                if (tmpEsame.Substring(0, tmpEsame.IndexOf(" ")) == "COVRAP_2")
                                                    dr["test_rapido"] = "IgG";
                                                else if (tmpEsame.Substring(0, tmpEsame.IndexOf(" ")) == "COVRAP_3")
                                                    dr["test_rapido"] = "IgM";
                                                else
                                                    dr["test_rapido"] = "IgM/IgG";

                                                dr["risultato_test_rapido"] = dr["Esito"].ToString();
                                                break;
                                            case enumTipoEsame.IgG:
                                                dr["test_sierologico"] = dr["Esito"].ToString();
                                                dr["risultato_test_sierologico"] = "IgG";
                                                break;
                                            case enumTipoEsame.IgM:
                                                dr["test_sierologico"] = dr["Esito"].ToString();
                                                dr["risultato_test_sierologico"] = "IgM";
                                                break;
                                            case enumTipoEsame.IgA:
                                                break;
                                            case enumTipoEsame.IgTot:
                                                dr["test_sierologico"] = dr["Esito"].ToString();
                                                dr["risultato_test_sierologico"] = "IgM/IgG";
                                                break;
                                            default:
                                                break;
                                        }
                                        if (dr["id_richiesta"].ToString().Trim() != "") //16.06.2020 Esclude i preventivi
                                            dtCsv.Rows.Add(dr); //add other rows  
                                    }
                                    break;
                                case enumTipoTracciato.PIEMONTE:
                                    //ZCCLNE59C65L736B CF di Elenza Zocca
                                    //RSOLSN69M04G197A CF di Alessandro Rosa
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
                                                                //dr["descrStruttura"] = "Lifebrain Piemonte S.r.l."; //add other columns
                                                                //dr["aslAppartenenza"] = "213"; //ASL Alessandria
                                                                //ZCCLNE59C65L736B CF di Elenza Zocca
                                                                //RSOLSN69M04G197A CF di Alessandro Rosa
                                        if (cmbCFRefertante.Text == "ZCCLNE59C65L736B")
                                        {
                                            dr["tipoRichiesta"] = "00"; //SORVEGLIANZA TERRITORIALE
                                            switch (dr["Reparto"].ToString().Trim())
                                            {
                                                //ASL di Cuneo 1 e Azienda Ospedaliera S. Croce e Carle
                                                case "ASLCN1":
                                                case "CROCE":
                                                    dr["descrStruttura"] = "Rete Diagnostica Italiana S.r.l.";
                                                    dr["aslAppartenenza"] = "210"; //Cuneo 1
                                                    dr["idAsr"] = "210"; //Cuneo 1
                                                    break;
                                                case "ASLCN2":
                                                    //ASL di Cuneo 2
                                                    dr["descrStruttura"] = "Rete Diagnostica Italiana S.r.l.";
                                                    dr["aslAppartenenza"] = "211"; //Cuneo 2
                                                    dr["idAsr"] = "211"; //Cuneo 2
                                                    break;
                                                //ASL Città di Torino
                                                case "ASLTO":
                                                    dr["descrStruttura"] = "Rete Diagnostica Italiana S.r.l.";
                                                    dr["aslAppartenenza"] = "301"; //Città di Torino
                                                    dr["idAsr"] = "301"; //Città di Torino
                                                    break;
                                            }

                                        }
                                        else
                                        {
                                            dr["descrStruttura"] = "Lifebrain Piemonte S.r.l."; //add other columns

                                            if (dr["Punto accesso"].ToString().ToUpper().Contains("MDL"))
                                                dr["tipoRichiesta"] = "01"; //SORVEGLIANZA MEDICO COMPETENTE
                                            else
                                                dr["tipoRichiesta"] = "09"; //ESAMI VOLONTARI CITTADINO

                                            if (dr["U.S.L."].ToString().Length == 6)
                                                dr["aslAppartenenza"] = dr["U.S.L."].ToString().Substring(3, 3);
                                            else if (dr["Residenza"].ToString() != "")
                                            {
                                                dr["aslAppartenenza"] = GetASL(dr["Residenza"].ToString(), conn); //ASL paziente
                                                if (dr["aslAppartenenza"].ToString() == "" && dr["Residenza"].ToString().Substring(0, 3) == "999")
                                                    dr["aslAppartenenza"] = "213"; //Se non è stato in grado di trovarla e il paziente è straniero mette quella del laboratorio ASL Alessandria
                                            }
                                        }



                                        dr["code"] = GetCUR(prmTipo, tmpEsame);
                                        switch (dr["code"].ToString())
                                        {
                                            case "91.12.S":
                                                dr["displayName"] = "tampone";
                                                break;
                                            case "91.13.M":
                                                dr["displayName"] = "Test rapido mediante dosaggio dell'antigene di Sars-Cov-2, a lettura manuale";
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
                                            dr["legalauthenticator"] = cmbCFRefertante.Text;
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
                                        //12.08.2020 Monica Cagnali esclusione Lifebrain Toscana e CF vuoti
                                        DataRow dr = dtCsv.NewRow();
                                        for (int k = 0; k < rowValues.Count(); k++)
                                        {
                                            dr[k] = rowValues[k].ToString().Trim();
                                        }
                                        if (dr["CODICE_FISCALE"].ToString() == ""
                                            || dr["DESC_PROVENIENZA_CAMPIONE"].ToString().IndexOf("TOSCANA LIFEBRAIN") > -1)
                                        {
                                            kEsclusi += 1;
                                            lblStato.Text = "Risultati esclusi (senza C.F. o reparto TOSCANA LIFEBRAIN): " + kEsclusi.ToString();
                                            lblStato.Refresh();
                                            continue;
                                        }
                                        dr["ID_RICHIESTA_PK"] = Right(("00000" + dr["ID_RICHIESTA_PK"].ToString()), 5);
                                        DateTime dNas;
                                        if (DateTime.TryParse(dr["DATA_ACCETTAZIONE"].ToString(), out dNas))
                                            dr["ID_RICHIESTA_PK"] = dNas.ToString("yyyyMMdd") + dr["ID_RICHIESTA_PK"].ToString();

                                        dr["TIPOLOGIA_PAZIENTE"] = "1"; //1 = Pazienti esterni/domiciliari
                                        dr["TIPO_IDENTIFICATIVO"] = "CF"; //CF, ENI, STP, TEAM
                                                                          //Recupera Data e Luogo di nascita dal Codice Fiscale
                                        if (!DateTime.TryParse(dr["DATA_NASCITA"].ToString(), out dNas))
                                            dr["DATA_NASCITA"] = GetDataNascitaFromCF(dr["CODICE_FISCALE"].ToString());
                                        if (dr["DATA_NASCITA"].ToString() != "")
                                            dNas = Convert.ToDateTime(dr["DATA_NASCITA"]);
                                        dr["COD_ISTAT_NASCITA"] = GetLuogoNascitaFromCF(dr["CODICE_FISCALE"].ToString(), dNas, conn);
                                        tmpEsame = dr["Test_utilizzato"].ToString();
                                        if (tmpEsame != "")
                                            dr["CODICE_ANALISI"] = tmpEsame.Substring(0, tmpEsame.IndexOf(" "));
                                        dr["ESITO"] = dr["CODICE_ANALISI"];
                                        if (tmpEsame != "")
                                            dr["DESC_ANALISI"] = tmpEsame.Substring(tmpEsame.IndexOf(" "));
                                        dr["DESC_ESITO"] = dr["DESC_ANALISI"];
                                        dr["PROG_ESITO"] = "1";
                                        if (dr["CODICE_ANALISI"].ToString() == "COVID" || dr["CODICE_ANALISI"].ToString() == "COVRAG")
                                        {
                                            /* Commentata come da mail inviata da A.Li.Sa. del 08/09/2020 Tutte le righe identificate con 88820 RETE DIAGNOSTICA ITALIANA  devono essere cambiate in 88812 LIFEBRAIN LIGURIA. 
                                            //RNA TAMPONI
                                            dr["AZIENDA_INVIANTE"] = "88820";
                                            dr["DESC_AZIENDA_INVIANTE"] = "RETE DIAGNOSTICA ITALIANA";
                                            */
                                            bool bTamponeMolecolare = dr["CODICE_ANALISI"].ToString() == "COVID";
                                            dr["AZIENDA_INVIANTE"] = "88812";
                                            dr["DESC_AZIENDA_INVIANTE"] = "LIFEBRAIN LIGURIA";

                                            dr["RISULTATO_NUMERO"] = GetEsito(tmpEsame, dr["RisDesc"].ToString(), dr["CodRi"].ToString()); //add other columns
                                            dr["DESC_RISULTATO"] = dr["RISULTATO_NUMERO"];
                                            switch (dr["RISULTATO_NUMERO"].ToString())
                                            {
                                                case "POSITIVO":
                                                    dr["RISULTATO_NUMERO"] = bTamponeMolecolare ? "1" : "21";
                                                    if (!bTamponeMolecolare)
                                                        dr["DESC_RISULTATO"] = "RILEVATO";
                                                    break;
                                                case "NEGATIVO":
                                                    dr["RISULTATO_NUMERO"] = bTamponeMolecolare ? "2" : "22";
                                                    if (!bTamponeMolecolare)
                                                        dr["DESC_RISULTATO"] = "NON RILEVATO";
                                                    break;
                                                case "DEBOLMENTE POSITIVO":
                                                    dr["RISULTATO_NUMERO"] = bTamponeMolecolare ? "3" : "";
                                                    if (!bTamponeMolecolare)
                                                        dr["DESC_RISULTATO"] = "";
                                                    break;
                                                case "ANNULATO":
                                                    dr["RISULTATO_NUMERO"] = bTamponeMolecolare ? "4" : "23";
                                                    dr["DESC_RISULTATO"] = "INDETERMINATO";
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            //Sierologici
                                            dr["AZIENDA_INVIANTE"] = "88812";
                                            dr["DESC_AZIENDA_INVIANTE"] = "LIFEBRAIN LIGURIA";
                                            dr["RISULTATO_NUMERO"] = GetEsito(tmpEsame, dr["Risultato"].ToString(), dr["CodRi"].ToString()); //add other columns
                                            dr["DESC_RISULTATO"] = dr["RISULTATO_NUMERO"];
                                            switch (dr["RISULTATO_NUMERO"].ToString())
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
                                        dr["DESC_MATERIALE"] = dr["ID_MATERIALE"];
                                        if (dr["ID_MATERIALE"].ToString() != "")
                                            dr["ID_MATERIALE"] = dr["ID_MATERIALE"].ToString().Substring(0, dr["ID_MATERIALE"].ToString().IndexOf(" "));
                                        dr["CODICE_CUR"] = GetCUR(prmTipo, tmpEsame, false);
                                        dr["BCP"] = GetLabRif(dr["Punto Accesso"].ToString()); //add other columns
                                        dr["DATA_ACCETTAZIONE"] += " " + dr["OraAcc"].ToString().Replace(".", ":");
                                        dr["DATA_PRELIEVO"] = dr["DATA_ACCETTAZIONE"];
                                        dr["DATA_ESECUZIONE"] += " " + dr["OraEsame"].ToString().Replace(".", ":");
                                        dr["DATA_ULT_MOD"] = dr["DATA_ESECUZIONE"].ToString() + ":00"; //aggiungo i secondi;
                                        dr["DATA_REFERTAZIONE"] += " " + dr["OraRef"].ToString().Replace(".", ":");

                                        if (dr["ID_RICHIESTA_PK"].ToString().Trim() != "") //16.06.2020 Esclude i preventivi
                                            dtCsv.Rows.Add(dr); //add other rows  
                                    }
                                    break;
                                case enumTipoTracciato.SARDEGNA:
                                    // regione; azienda; laboratorio; id; provenienza; tipo_provenienza; reparto; determinazione; data; cognome; 
                                    // nome; cf; data_nascita; genere; comune; comune_descrizione; categoria; qualifica; esito_biomolecolare; esito_immunologico
                                    if (i == 0)
                                    {
                                        for (int j = 0; j < rowValues.Count(); j++)
                                        {
                                            dtCsv.Columns.Add(rowValues[j].Trim()); //add headers  
                                        }
                                        dtCsv.Columns.Add("regione"); //add other columns
                                        dtCsv.Columns.Add("azienda"); //add other columns
                                        dtCsv.Columns.Add("laboratorio"); //add other columns
                                        dtCsv.Columns.Add("provenienza"); //add other columns
                                        dtCsv.Columns.Add("tipo_provenienza"); //add other columns
                                        dtCsv.Columns.Add("reparto"); //add other columns
                                        dtCsv.Columns.Add("determinazione"); //add other columns
                                        dtCsv.Columns.Add("comune"); //add other columns
                                        dtCsv.Columns.Add("categoria"); //add other columns
                                        dtCsv.Columns.Add("qualifica"); //add other columns
                                        dtCsv.Columns.Add("ESITO"); //add other columns
                                        dtCsv.Columns.Add("esito_biomolecolare"); //add other columns
                                        dtCsv.Columns.Add("esito_immunologico"); //add other columns
                                    }
                                    else
                                    {
                                        kRecord += 1;
                                        DataRow dr = dtCsv.NewRow();
                                        for (int k = 0; k < rowValues.Count(); k++)
                                        {
                                            dr[k] = rowValues[k].ToString().Trim();
                                        }
                                        tmpEsame = dr["Test_utilizzato"].ToString();

                                        dr["regione"] = "200";
                                        dr["azienda"] = "201";
                                        //dr["laboratorio"] = "001737";
                                        dr["laboratorio"] = "120140";

                                        dr["provenienza"] = new string(' ', 8);
                                        dr["tipo_provenienza"] = " ";
                                        dr["reparto"] = new string(' ', 4);

                                        //Inserisce solo i tamponi
                                        //if (GetCUR(prmTipo, tmpEsame, false) == "1")
                                        //    dr["determinazione"] = "1";
                                        //else
                                        //    dr["determinazione"] = new string(' ', 1);
                                        dr["determinazione"] = GetCUR(prmTipo, tmpEsame, false);

                                        DateTime dNas;
                                        if (DateTime.TryParse(dr["DATA"].ToString(), out dNas))
                                            dr["DATA"] = dNas.ToString("ddMMyyyy");
                                        else
                                            dNas = DateTime.Now.Date;

                                        dr["id"] = (dNas.ToString("yyyyMMdd") + ("0000" + dr["Prog"].ToString()).Substring(("0000" + dr["Prog"].ToString()).Length - 4, 4)) + kRecord.ToString("0000");

                                        dr["cognome"] = dr["cognome"].ToString().PadRight(30, ' ').Substring(0, 30);
                                        dr["nome"] = dr["nome"].ToString().PadRight(20, ' ').Substring(0, 20);
                                        dr["cf"] = dr["cf"].ToString().PadRight(16, ' ');
                                        if (DateTime.TryParse(dr["data_nascita"].ToString(), out dNas))
                                            dr["data_nascita"] = dNas.ToString("ddMMyyyy");
                                        else
                                            dr["data_nascita"] = Convert.ToDateTime(GetDataNascitaFromCF(dr["cf"].ToString())).ToString("ddMMyyyy");
                                        dr["genere"] = " ";
                                        dr["comune"] = new string(' ', 6);
                                        dr["comune_descrizione"] = new string(' ', 27);
                                        dr["categoria"] = " ";
                                        dr["qualifica"] = " ";

                                        if (tmpEsame.Substring(0, tmpEsame.IndexOf(" ")) == "COVID" || tmpEsame.Substring(0, tmpEsame.IndexOf(" ")) == "COVRAG") //TAMPONI o Antigenici
                                        {
                                            dr["Esito"] = GetEsito(tmpEsame, dr["RisDesc"].ToString(), dr["CodRi"].ToString());

                                            switch (dr["Esito"].ToString())
                                            {
                                                case "POSITIVO":
                                                    dr["esito_biomolecolare"] = "1";
                                                    break;
                                                case "NEGATIVO":
                                                    dr["esito_biomolecolare"] = "2";
                                                    break;
                                                case "DEBOLMENTE POSITIVO":
                                                    dr["esito_biomolecolare"] = "3";
                                                    break;
                                                case "ANNULATO":
                                                    dr["esito_biomolecolare"] = "4";
                                                    break;
                                                default:
                                                    dr["esito_biomolecolare"] = "9";
                                                    break;
                                            }
                                            dr["esito_immunologico"] = " ";
                                        }
                                        else //SIEROLOGICI
                                        {
                                            dr["Esito"] = GetEsito(tmpEsame, dr["Risultato"].ToString(), dr["CodRi"].ToString());
                                            dr["esito_biomolecolare"] = " ";
                                            Enum.TryParse(GetCUR(prmTipo, tmpEsame, true), out enumTipoEsame tmpTipoEsame);

                                            if (dr["Esito"].ToString() == "ANNULLATO")
                                                dr["esito_immunologico"] = "8";
                                            else if (dr["Esito"].ToString() == "DUBBIO")
                                                dr["esito_immunologico"] = "7";
                                            else
                                            {
                                                switch (tmpTipoEsame)
                                                {
                                                    case enumTipoEsame.Tampone:
                                                    case enumTipoEsame.TestRapido:
                                                        break;
                                                    case enumTipoEsame.IgTot:
                                                        switch (dr["Esito"].ToString())
                                                        {
                                                            case "POSITIVO":
                                                                dr["esito_immunologico"] = "T";
                                                                break;
                                                            case "NEGATIVO":
                                                                dr["esito_immunologico"] = "0";
                                                                break;
                                                        }
                                                        break;
                                                    case enumTipoEsame.IgG:
                                                        switch (dr["Esito"].ToString())
                                                        {
                                                            case "POSITIVO":
                                                                dr["esito_immunologico"] = "D";
                                                                break;
                                                            case "NEGATIVO":
                                                                dr["esito_immunologico"] = "C";
                                                                break;
                                                        }
                                                        break;
                                                    case enumTipoEsame.IgM:
                                                        switch (dr["Esito"].ToString())
                                                        {
                                                            case "POSITIVO":
                                                                dr["esito_immunologico"] = "F";
                                                                break;
                                                            case "NEGATIVO":
                                                                dr["esito_immunologico"] = "E";
                                                                break;
                                                        }
                                                        break;
                                                    case enumTipoEsame.IgA:
                                                        break;
                                                }
                                            }
                                        }
                                        //dr["esito_biomolecolare"] = " ";
                                        //dr["esito_immunologico"] = " ";
                                        if (dr["id"].ToString().Trim() != "") //16.06.2020 Esclude i preventivi
                                            dtCsv.Rows.Add(dr); //add other rows  

                                    }
                                    break;
                                case enumTipoTracciato.CAMPANIA:
                                    if (i == 0)
                                    {
                                        for (int j = 0; j < rowValues.Count(); j++)
                                        {
                                            dtCsv.Columns.Add(rowValues[j].Trim()); //add headers  
                                        }
                                        dtCsv.Columns.Add("Telefono"); //add other columns
                                        dtCsv.Columns.Add("Azienda"); //add other columns
                                        dtCsv.Columns.Add("Presidio"); //add other columns
                                        dtCsv.Columns.Add("Esposizione Operatore"); //add other columns
                                        dtCsv.Columns.Add("Test rapido associato"); //add other columns
                                        dtCsv.Columns.Add("Area di competenza"); //add other columns
                                        dtCsv.Columns.Add("Sintomatologia"); //add other columns
                                        dtCsv.Columns.Add("Data inizio sintomi"); //add other columns
                                        dtCsv.Columns.Add("Esito"); //add other columns

                                        dtCsv.Columns.Add("Posizione paziente"); //add other columns
                                        dtCsv.Columns.Add("Ospedale"); //add other columns
                                        dtCsv.Columns.Add("Soggetto in gravidanza"); //add other columns
                                        dtCsv.Columns.Add("Data presumibile parto"); //add other columns
                                        dtCsv.Columns.Add("Laboratorio di destinazione"); //add other columns
                                        dtCsv.Columns.Add("Tipologia Tampone"); //add other columns
                                        dtCsv.Columns.Add("Medico che verifica il Tampone"); //add other columns
                                        dtCsv.Columns.Add("Risultato"); //add other columns
                                        dtCsv.Columns.Add("Prestazione eseguita"); //add other columns
                                        dtCsv.Columns.Add("Codice NSIS"); //add other columns
                                        dtCsv.Columns.Add("ID Operatore che esegue il Tampone"); //add other columns
                                    }
                                    else
                                    {
                                        DataRow dr = dtCsv.NewRow();
                                        for (int k = 0; k < rowValues.Count(); k++)
                                        {
                                            dr[k] = rowValues[k].ToString().Trim();
                                        }
                                        if (dr["Telefono Cellulare"].ToString().Trim() != "")
                                            dr["Telefono"] = dr["Telefono Cellulare"];
                                        else if (dr["Tel.Abitazione"].ToString().Trim() != "")
                                            dr["Telefono"] = dr["Tel.Abitazione"];
                                        else if (dr["Tel.Lavoro"].ToString().Trim() != "")
                                            dr["Telefono"] = dr["Tel.Lavoro"];

                                        //if (dr["CF"].ToString().Trim() == "" || dr["Telefono"].ToString().Trim() == "")
                                        //{
                                        //    kEsclusi += 1;
                                        //    lblStato.Text = "Pazienti esclusi senza CF e/o Telefono: " + kEsclusi.ToString();
                                        //    lblStato.Refresh();
                                        //    continue;   //Non prende in considerazione i pazienti senza CF
                                        //}

                                        tmpEsame = dr["Test_utilizzato"].ToString();

                                        dr["Area di competenza"] = "Area territoriale";
                                        dr["Sintomatologia"] = "Asintomatico";
                                        dr["Posizione paziente"] = "Domicilio";
                                        dr["Ospedale"] = "Ospedale non trovato";
                                        dr["Soggetto in gravidanza"] = "No";

                                        dr["Laboratorio di destinazione"] = "Informazione non disponibile al momento";
                                        dr["Tipologia Tampone"] = "Primo Tampone Diagnosi";
                                        if (tmpEsame.Substring(0, tmpEsame.IndexOf(" ")) == "COVID") //TAMPONI
                                        {
                                            dr["Esito"] = GetEsito(tmpEsame, dr["RisDesc"].ToString(), dr["CodRi"].ToString());
                                            if (dr["Esito"].ToString() == "DEBOLMENTE POSITIVO")
                                                dr["Esito"] = "POSITIVO";
                                        }
                                        dr["Risultato"] = dr["Esito"];
                                        dr["Prestazione eseguita"] = "TNF";
                                        dr["Ora prestazione"] = dr["Ora prestazione"].ToString().Replace(".", ":");
                                        dr["Codice NSIS"] = "207"; //AGG601
                                        dr["ID Operatore che esegue il Tampone"] = "2070025@regionecampania.it";

                                        if (dr["ID_accettazione"].ToString().Trim() != "") //16.06.2020 Esclude i preventivi
                                            dtCsv.Rows.Add(dr); //add other rows  
                                        else
                                        {
                                            kEsclusi += 1;
                                            lblStato.Text = "Pazienti esclusi senza CF e/o Telefono e/o IDAccettazione: " + kEsclusi.ToString();
                                            lblStato.Refresh();
                                        }
                                    }
                                    break;
                                case enumTipoTracciato.PUGLIA:
                                    if (i == 0)
                                    {
                                        for (int j = 0; j < rowValues.Count(); j++)
                                        {
                                            dtCsv.Columns.Add(rowValues[j].Trim()); //add headers  
                                        }
                                        //if (lblFileOrigine.Text.IndexOf("COVDE") > -1 || lblFileOrigine.Text.IndexOf("ANTDE") > -1)
                                        //    dtCsv.Columns.Add("NUMERO RICHIESTA"); //add other columns
                                        dtCsv.Columns.Add("CODICE TIPO ESAME"); //add other columns
                                        dtCsv.Columns.Add("CODICE LABORATORIO"); //add other columns
                                        dtCsv.Columns.Add("PROGRESSIVO ESAME"); //add other columns
                                        dtCsv.Columns.Add("STRUTTURA RICHIEDENTE"); //add other columns
                                        dtCsv.Columns.Add("ESITO ESAME"); //add other columns
                                        dtCsv.Columns.Add("TIPO TEST"); //add other columns
                                        dtCsv.Columns.Add("Esito"); //add other columns

                                        dtCsv.Columns.Add("TEST MOLECOLARE DOPO ANTIGENICO"); //add other columns
                                        dtCsv.Columns.Add("MOTIVO DELLA RICHIESTA"); //add other columns
                                        dtCsv.Columns.Add("PAZIENTE FUORI REGIONE"); //add other columns
                                        //Colonne per il file delle denunce
                                        dtCsv.Columns.Add("NOME"); //add other columns
                                        dtCsv.Columns.Add("COGNOME"); //add other columns
                                        dtCsv.Columns.Add("DATA DI NASCITA"); //add other columns
                                        dtCsv.Columns.Add("CODICE FISCALE"); //add other columns
                                        dtCsv.Columns.Add("ALL'ATTO DEL PRELIEVO"); //add other columns
                                        dtCsv.Columns.Add("CITTA'"); //add other columns
                                        dtCsv.Columns.Add("TELEFONO"); //add other columns
                                        dtCsv.Columns.Add("TELEFONO FISSO"); //add other columns
                                        dtCsv.Columns.Add("PROVENIENZA"); //add other columns
                                        dtCsv.Columns.Add("E-MAIL"); //add other columns
                                    }
                                    else
                                    {
                                        DataRow dr = dtCsv.NewRow();
                                        for (int k = 0; k < rowValues.Count(); k++)
                                        {
                                            dr[k] = rowValues[k].ToString().Trim();
                                        }
                                        dr["CODICE TIPO ESAME"] = "700"; //Esame ordinario
                                        dr["CODICE LABORATORIO"] = "15";
                                        dr["PROGRESSIVO ESAME"] = "";
                                        dr["PAZIENTE AL PRELIEVO"] = dr["PAZIENTE AL PRELIEVO"].ToString().Replace("<p>", "").Replace("</p>", "").Trim();
                                        dr["STRUTTURA RICHIEDENTE"] = "Lifebrain Lecce Srl";

                                        //Riassegna le colonne per le denunce
                                        dr["NOME"] = dr["NOME PAZIENTE"];
                                        dr["COGNOME"] = dr["COGNOME PAZIENTE"];
                                        dr["DATA DI NASCITA"] = dr["DATA NASCITA PAZIENTE"];
                                        dr["CODICE FISCALE"] = dr["CODICE FISCALE O STP O ENI PAZIENTE"];
                                        dr["ALL'ATTO DEL PRELIEVO"] = dr["PAZIENTE AL PRELIEVO"];
                                        dr["CITTA'"] = dr["RESIDENZA"];
                                        dr["TELEFONO"] = dr["TELEFONO MOBILE PAZIENTE"];
                                        dr["TELEFONO FISSO"] = dr["TELEFONO FISSO PAZIENTE"];
                                        dr["PROVENIENZA"] = "LABORATORIO ANALISI PIGNATELLI SRL";
                                        dr["E-MAIL"] = dr["RECAPITO E-MAIL PAZIENTE"];

                                        if (dr["PAZIENTE AL PRELIEVO"].ToString().IndexOf("SINTOMATICO") == 0 || dr["PAZIENTE AL PRELIEVO"].ToString().IndexOf("PAUCISINTOMATICO") > -1)
                                            dr["PAZIENTE AL PRELIEVO"] = "100";
                                        else
                                            dr["PAZIENTE AL PRELIEVO"] = "101";

                                        tmpEsame = dr["Test_utilizzato"].ToString().Substring(0, dr["Test_utilizzato"].ToString().IndexOf(" "));
                                        if (tmpEsame == "COVID" || tmpEsame == "COVRAG") //TAMPONI
                                        {
                                            dr["Esito"] = GetEsito(dr["Test_utilizzato"].ToString(), dr["RisDesc"].ToString(), dr["RISULTATO"].ToString());
                                            switch (dr["Esito"].ToString())
                                            {
                                                case "NEGATIVO":
                                                    dr["ESITO ESAME"] = "111";
                                                    break;
                                                case "POSITIVO":
                                                    dr["ESITO ESAME"] = "112";
                                                    break;
                                                case "DEBOLMENTE POSITIVO":
                                                case "DUBBIO":
                                                    dr["ESITO ESAME"] = "113";
                                                    break;
                                            }
                                            dr["TIPO TEST"] = tmpEsame == "COVID" ? "711" : "710";
                                            dr["TEST MOLECOLARE DOPO ANTIGENICO"] = "";
                                        }

                                        dr["MOTIVO DELLA RICHIESTA"] = "914"; //Motivo non sanitario
                                        dr["PAZIENTE FUORI REGIONE"] = "";


                                        if (dr["ID_accettazione"].ToString().Trim() != "") //16.06.2020 Esclude i preventivi
                                            dtCsv.Rows.Add(dr); //add other rows  
                                        else
                                        {
                                            kEsclusi += 1;
                                            lblStato.Text = "Pazienti esclusi : " + kEsclusi.ToString();
                                            lblStato.Refresh();
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
                progressBar1.Value = progressBar1.Maximum;
                progressBar1.Refresh();
                conn.Close();
                this.Cursor = Cursors.Default;
            }

            return dtCsv;
        }

        private string GetStrumento(string prmEsame)
        {
            string tmpStru = "";
            string tmpEsame = prmEsame.Substring(0, prmEsame.IndexOf(" "));
            /*
            COVIGM magLUMI
            COVIGG DiaSorin
            COVIG_2 EuroImmun
            COVIG_3 EuroImmun
            COVIG_5 EuroImmun
            COVIG_6 EuroImmun
            COVM magLUMI
            COVG magLUMI
            COVIDG EuroImmun
            COVIDM EuroImmun
            COVIGA EuroImmun
            COVR Elecsys Anti-SARS-CoV-2 ROCHE
            COVT Elecsys Anti-SARS-CoV-2 ROCHE
            COVTG Elecsys Anti-SARS-CoV-2 ROCHE
            COVTGM Elecsys Anti-SARS-CoV-2 ROCHE
            COVIGS Altro (Stratego)
            COVIGS_2 Altro (Stratego)
            COVIGS_3 Altro (Stratego)
             */
            switch (tmpEsame)
            {
                case "COVID": //Tampone
                    tmpStru = "";
                    break;
                case "COVRAP": //Test rapido
                case "COVRAP_2":
                case "COVRAP_3":
                    tmpStru = "Altro";
                    break;
                case "COVG": //IgG
                case "COVM": //IgM
                case "COVIGM":
                    tmpStru = "magLUMI";
                    break;
                case "COVIGG":
                    tmpStru = "DiaSorin";
                    break;
                case "COVIG_2":
                case "COVIG_3":
                case "COVIG_5":
                case "COVIG_6":
                case "COVIDG":
                case "COVIGA": //IgA
                case "COVIDM":
                    tmpStru = "EuroImmun";
                    break;
                case "COVIGS":
                case "COVIGS_2":
                case "COVIGS_3":
                    tmpStru = "Altro";
                    break;
                case "COVT": //Ig Totali
                case "COVTG":
                case "COVTGM":
                case "COVR":
                    tmpStru = "Elecsys Anti-SARS-CoV-2 ROCHE";
                    break;
                default:
                    tmpStru = "Altro";
                    break;
            }
            return tmpStru;
        }
        private string GetATSJLab(string prmDescAslJlLab)
        {
            string tmpATS = prmDescAslJlLab;
            if (tmpATS != "")
            {
                switch (tmpATS.Trim().ToUpper())
                {
                    case "LOMBARDIA 321 - ATS  CITTÀ METROPOLITANA DI MILANO":
                    case "LOMBARDIA 309 - A.S.L. DELLA PROVINCIA DI MILANO 1":
                    case "LOMBARDIA 308 - A.S.L. DI MILANO":
                        tmpATS = "ALTRO ATS DELLA CITTA' METROPOLITANA DI MILANO";
                        break;
                    case "LOMBARDIA 322 - ATS DELL'INSUBRIA":
                    case "LOMBARDIA 314 - ATS DELL'INSUBRIA":
                    case "LOMBARDIA 303 - A.S.L.DELLA PROVINCIA DI COMO":
                    case "LOMBARDIA 303 - A.S.L. DELLA PROVINCIA DI COMO":
                        tmpATS = "ALTRO ATS DELL'INSUBRIA";
                        break;
                    case "LOMBARDIA 323 - ATS DELLA MONTAGNA":
                        tmpATS = "ALTRO ATS DELLA MONTAGNA";
                        break;
                    case "LOMBARDIA 324 - ATS DELLA BRIANZA":
                        tmpATS = "ALTRO ATS DELLA MONTAGNA";
                        break;
                    case "LOMBARDIA 325 - ATS DI BERGAMO":
                        tmpATS = "ALTRO ATS DI BERGAMO";
                        break;
                    case "LOMBARDIA 326 - A.T.S. BRESCIA":
                    case "LOMBARDIA 302 - A.S.L. DELLA PROVINCIA DI BRESCIA":
                        tmpATS = "ALTRO ATS DI BRESCIA";
                        break;
                    case "LOMBARDIA 327 - ATS DELLA VAL PADANA":
                    case "LOMBARDIA 307 - A.S.L. DELLA PROVINCIA DI MANTOVA":
                        tmpATS = "ALTRO ATS DELLA VAL PADANA";
                        break;
                    case "LOMBARDIA 328 - ATS DI PAVIA":
                        tmpATS = "ALTRO ATS DI PAVIA";
                        break;
                }
            }
            return tmpATS;
        }
        private string GetLabRif(string prmBCP)
        {
            if (prmBCP.Trim() != "")
            {
                var lstBCP = new List<string>
                {
                "590", "591", "592", "593", "594", "595", "596", "597", "598", "599", "600", "601",
                "602", "603", "604", "605", "606", "607", "608", "609", "610", "611", "612", "613",
                "614", "615", "616", "617", "618", "619", "620", "621", "622", "623", "624",
                "670", "672", "673", "674", "675", "680", "681", "685", "686", "IV3", "CES", "CRG",
                "MCE", "MPS", "MYM", "PSG", "SCE", "SGR", "MED", "DEL", "690"};

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
                        if (prmCodRis.Trim() != "")
                        {
                            switch (prmCodRis.ToUpper())
                            {
                                case "TNR":
                                case "NEG":
                                case "NEGATIVO":
                                case "V2":
                                    tmpRis = "NEGATIVO";
                                    break;
                                case "TAR":
                                case "POS":
                                case "POSITIVO":
                                case "V1":
                                    tmpRis = "POSITIVO";
                                    break;
                                case "TARS":
                                case "DEBOP":
                                case "DUB":
                                case "DEBOLMENTE POSITIVO":
                                    tmpRis = "DEBOLMENTE POSITIVO";
                                    break;
                                case "ANNULLATO":
                                case "ANN":
                                case "CNV":
                                case "CNP":
                                case "CNI":
                                case "INDET":
                                case "INCO":
                                case "MC":
                                    tmpRis = "ANNULLATO";
                                    break;
                            }
                        }
                        if (tmpRis == "" && prmRisultato.Trim() != "")
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
                        break;
                    case "COVRAG": //Tampone molecolare rapido per ricerca antigene
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
                            case "00067":
                            case "00074":
                            case "DEBOP":
                                if (tmpEsame == "COVRAG")
                                    tmpRis = "DEBOLMENTE POSITIVO";
                                else
                                    tmpRis = "DUBBIO";
                                break;
                            case "":
                            case ".":
                            case "..":
                            case "ANN":
                            case "CNV":
                            case "MC":
                                tmpRis = "ANNULLATO";
                                break;
                        }
                        break;
                    case "COVG":
                    case "COVIG_3":
                    case "COVIGS_3":
                    case "COVIG_5":
                        if (prmCodRis.Trim() != "")
                        {
                            switch (prmCodRis)
                            {
                                case "NEG":
                                case "MAG":
                                    tmpRis = "NEGATIVO";
                                    break;
                                case "POS":
                                    tmpRis = "POSITIVO";
                                    break;
                            }
                        }
                        else
                        {
                            if (dRiu < (decimal)0.9)
                                tmpRis = "NEGATIVO";
                            else if (dRiu >= (decimal)0.9 && dRiu < (decimal)1.1)
                                tmpRis = "DUBBIO";
                            else if (dRiu >= (decimal)1.1)
                                tmpRis = "POSITIVO";
                        }
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
                    case "COVIGA":
                        if (dRiu < (decimal)0.8)
                            tmpRis = "NEGATIVO";
                        else if (dRiu >= (decimal)0.8 && dRiu <= (decimal)1.1)
                            tmpRis = "DUBBIO";
                        else if (dRiu > (decimal)1.1)
                            tmpRis = "POSITIVO";
                        break;
                    case "COVM":
                    case "COVT":
                    case "COVTG":
                    case "COVTGM":
                    case "COVR":
                    case "COVIGM":
                    case "COVIG_6":
                        if (prmCodRis.Trim() != "")
                        {
                            switch (prmCodRis)
                            {
                                case "NEG":
                                case "MAG":
                                    tmpRis = "NEGATIVO";
                                    break;
                                case "POS":
                                    tmpRis = "POSITIVO";
                                    break;
                            }
                        }
                        else
                        {
                            if (dRiu < (decimal)1)
                                tmpRis = "NEGATIVO";
                            else if (dRiu >= (decimal)1)
                                tmpRis = "POSITIVO";
                        }
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prmTipo">Tipologi di tracciato</param>
        /// <param name="prmEsame">Codice dell'esame</param>
        /// <param name="prmTipoEsame">se True restituisce la tipologia di test (es. Tampone, Rapigo, IgM, ecc.</param>
        /// <returns></returns>
        private string GetCUR(enumTipoTracciato prmTipo, string prmEsame, bool prmTipoEsame)
        {
            string tmpCodCUR = "";
            if (prmEsame == "")
                return "";

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
                            case "COVRAG": //Test antigenico
                                tmpCodCUR = "91.13.M";
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
                            case "COVRAG": //Tampone antigenico rapido
                                tmpCodCUR = "C02254000";
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
                    case enumTipoTracciato.SARDEGNA:
                        switch (tmpEsame)
                        {
                            case "COVID": //Tamponi per la Ricerca RNA SARS-CoV-2
                                tmpCodCUR = "1";
                                break;
                            case "COVRAP": //Test Immunocromatografci
                                tmpCodCUR = "3";
                                break;
                            case "COVRAG": //Test Antigenico
                                tmpCodCUR = "4";
                                break;
                            default: //Test CLIA O ELISA
                                tmpCodCUR = "2";
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
                    case "COVRAG": //Tampone
                        tmpCodCUR = enumTipoEsame.Tampone.ToString();
                        break;
                    case "COVRAP": //Test rapido
                    case "COVRAP_3":
                    case "COVRAP_2":
                        tmpCodCUR = enumTipoEsame.TestRapido.ToString();
                        break;
                    case "COVG": //IgG
                    case "COVIGG":
                    case "COVIDG":
                    case "COVIGS_2":
                    case "COVIG_2":
                    case "COVIG_5":
                        tmpCodCUR = enumTipoEsame.IgG.ToString();
                        break;
                    case "COVM": //IgM
                    case "COVIGM":
                    case "COVIDM":
                    case "COVIGS_3":
                    case "COVIG_3":
                    case "COVIG_6":

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
                    else if (chkVerificaDatiCF.Checked)
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
                        string sFiltro = "BCP='" + (optTradate.Checked ? optTradate.Text : optViadana.Text) + "' AND Esito<>'ANNULLATO' AND Cognome<>'PROVA'";
                        if (optTamponi.Checked)
                            sFiltro += " AND Test_utilizzato LIKE 'COVID %'";
                        else if (optAntigenici.Checked)
                            sFiltro += " AND Test_utilizzato LIKE 'COVRAG %'";

                        dv.RowFilter = sFiltro;
                        dtDataTable = dv.ToTable();

                        if (optSierologici.Checked || optTestRapidi.Checked)
                        {
                            dtDataTable = dtDataTable.DefaultView.ToTable("Selected", false, "ID_accettazione", "Nome", "Cognome", "Data nascita", "CF", "Data_Ricezione", "Data Referto",
                                "Ente_richiedente", "Esito", "Telefono_Paziente", "Test_utilizzato", "Setting", "Provenienza");
                        }
                        else if (optTamponi.Checked)
                        {
                            dtDataTable = dtDataTable.DefaultView.ToTable("Selected", false, "ID_accettazione", "Nome", "Cognome", "Data nascita", "Sesso", "Comune Domicilio", "Data Inizio Sintomi",
                                "Data Ricezione", "Data Referto", "Ospedale di Provenienza", "Esito", "CF", "Telefono_Paziente", "Setting", "Provenienza", "Materiale");
                        }
                        else if (optAntigenici.Checked)
                        {
                            dtDataTable = dtDataTable.DefaultView.ToTable("Selected", false, "ID_accettazione", "Nome", "Cognome", "Data nascita", "Sesso", "Comune Domicilio", "Data Inizio Sintomi",
                                "Data Esecuzione", "Ospedale di Provenienza", "Esito", "CF", "Telefono_Paziente", "Setting", "Provenienza", "Materiale", "Link epidemiologico");
                        }
                    }
                    break;
                case enumTipoTracciato.AUSLPC:
                    //Set column "tampone nasofaringeo privato"
                    dtDataTable = CheckCOVID(dtDataTable);

                    if (chkFileEsteso.Checked == false)
                    {
                        //Creo un clone del Datatable per filtrare solo i tamponi COVID Positivi
                        DataTable dtCOVID = dtDataTable.Copy();
                        dv = dtCOVID.DefaultView;
                        dv.RowFilter = "Test_utilizzato LIKE 'COVID %' AND ESITO IN ('POSITIVO', 'DEBOLMENTE POSITIVO', 'DUBBIO') AND Esito<>'ANNULLATO' AND Cognome<>'PROVA'";
                        dtCOVID = dtCOVID.DefaultView.ToTable("Selected", false, "Nome", "Cognome", "Codice Fiscale", "Comune residenza", "Cellulare", "Mail", "tampone nasofaringeo privato");

                        //Filtro il Datatable dei sierologici ewliminando i duplicati
                        dv = dtDataTable.DefaultView;
                        dv.RowFilter = "Test_utilizzato NOT LIKE 'COVID %' AND ESITO IN ('POSITIVO', 'DEBOLMENTE POSITIVO', 'DUBBIO') AND Esito<>'ANNULLATO' AND Cognome<>'PROVA'";
                        dtDataTable = RemoveDuplicateRows(dv.ToTable(), "Codice Fiscale");
                        //Aggiungo il datatble dei covid a quello dei dierologici
                        dtDataTable.Merge(dtCOVID);

                        dtDataTable = dtDataTable.DefaultView.ToTable("Selected", false, "Nome", "Cognome", "Codice Fiscale", "Comune residenza", "Cellulare", "Mail", "tampone nasofaringeo privato");
                    }
                    break;
                case enumTipoTracciato.EMILIA:
                    if (chkFileEsteso.Checked == false)
                    {
                        dv = dtDataTable.DefaultView;
                        dv.RowFilter = "BCP='EMILIA ROMAGNA' AND ESITO IN ('POSITIVO', 'DEBOLMENTE POSITIVO', 'DUBBIO') AND Esito<>'ANNULLATO' AND cognome<>'PROVA'";
                        //dtDataTable = RemoveDuplicateRows(dv.ToTable(), "Codice Fiscale");

                        dtDataTable = dtDataTable.DefaultView.ToTable("Selected", false, "id_richiesta", "dt_richiesta", "cognome", "nome", "dt_nasc", "cod_fisc", "tel", "cod_test", "Igm_ris", "Igg_ris", "es_ris", "cod_lab", "dt_val", "mmg_mc", "Rif_mmg_mc", "dat_lavoro");
                    }
                    break;
                case enumTipoTracciato.PARMA:
                    if (chkFileEsteso.Checked == false)
                    {
                        dv = dtDataTable.DefaultView;
                        dv.RowFilter = " ESITO IN ('POSITIVO', 'DEBOLMENTE POSITIVO', 'DUBBIO') AND Esito<>'ANNULLATO' AND Cognome<>'PROVA'";
                        //dtDataTable = RemoveDuplicateRows(dv.ToTable(), "Codice Fiscale");

                        dtDataTable = dtDataTable.DefaultView.ToTable("Selected", false, "CF_PAZIENTE", "COGNOME", "NOME", "sesso", "DATA_DI_NASCITA", "COMUNE RESID", "Telefono", "data_segnalazione", "test_rapido", "risultato_test_rapido", "test_sierologico", "risultato_test_sierologico", "tampone_nasofaringeo");
                    }
                    break;
                case enumTipoTracciato.MODENA:
                    if (chkFileEsteso.Checked == false)
                    {
                        if (chkFileEsteso.Checked == false)
                        {
                            dv = dtDataTable.DefaultView;
                            dv.RowFilter = " ESITO IN ('POSITIVO', 'DEBOLMENTE POSITIVO', 'DUBBIO') AND Esito<>'ANNULLATO' AND [Cognome assistito]<>'PROVA'";
                            //dtDataTable = RemoveDuplicateRows(dv.ToTable(), "Codice Fiscale");

                            dtDataTable = dtDataTable.DefaultView.ToTable("Selected", false, "Nome laboratorio", "Altro laboratorio, specificare", "Email laboratorio", "Cognome assistito", "Nome assistito", "Data di nascita", "Codice fiscale", "Comune di residenza", "Comune di domicilio", "Data esecuzione test sierologico", "Data refertazione test sierologico", "Tipo prelievo", "Test utilizzato", "Specificare altro test", "IgM pos / neg", "IgM titolo", "IgG pos / neg", "IgG titolo", "IgA pos / neg", "IgA titolo", "Note");
                        }
                    }
                    break;
                case enumTipoTracciato.PIEMONTE:
                    if (chkFileEsteso.Checked == false)
                    {
                        dv = dtDataTable.DefaultView;
                        dv.RowFilter = "esitoDesc<>'ANNULLATO' AND cognome<>'PROVA'";
                        //dtDataTable = RemoveDuplicateRows(dv.ToTable(), "Codice Fiscale");

                        dtDataTable = dtDataTable.DefaultView.ToTable("Selected", false, "descrStruttura", "idStruttura", "matrStruttura", "idAsr", "aslAppartenenza", "tipoRichiesta", "legalauthenticator",
                            "id_aura", "codFisc", "cognome", "nome", "sesso", "dataDiNascita", "ComuneDiNascita", "Residenza", "indirizzoResidenza", "Domicilio", "indirizzoDomicilio", "id_documento",
                            "code", "displayName", "effectiveTime", "esitoCode", "esitoDesc", "Unit", "Value", "ReferenceRange");
                    }
                    break;
                case enumTipoTracciato.LIGURIA:
                    if (chkFileEsteso.Checked == false)
                    {
                        dv = dtDataTable.DefaultView;
                        dv.RowFilter = "Test_utilizzato NOT LIKE 'COVRAG %' AND DESC_RISULTATO<>'ANNULLATO' AND COGNOME<>'PROVA'";
                        dv.Sort = "ID_RICHIESTA_PK ASC";
                        //dtDataTable = RemoveDuplicateRows(dv.ToTable(), "Codice Fiscale");

                        dtDataTable = dtDataTable.DefaultView.ToTable("Selected", false, "AZIENDA_INVIANTE", "DESC_AZIENDA_INVIANTE", "TIPOLOGIA_PAZIENTE", "COGNOME", "NOME", "SESSO", "DATA_NASCITA", "DATA_DECESSO", "COD_ISTAT_NASCITA", "COMUNE_NASCITA", "TIPO_IDENTIFICATIVO",
                            "CODICE_FISCALE", "COD_ISTAT_RESIDENZA", "COMUNE_RESIDENZA", "REGIONE_RESIDENZA", "VIA_RESIDENZA", "CIVICO_RESIDENZA", "CAP_RESIDENZA", "DESC_REGIONE_RESIDENZA", "COD_ISTAT_DOMICILIO",
                            "COMUNE_DOMICILIO", "VIA_DOMICILIO", "CIVICO_DOMICILIO", "CAP_DOMICILIO", "ID_RICHIESTA_PK", "DATA_PRELIEVO", "DATA_ACCETTAZIONE", "DESC_PROVENIENZA_CAMPIONE", "ID_CAMPIONE", "ID_MATERIALE",
                            "DESC_MATERIALE", "CODICE_ANALISI", "DESC_ANALISI", "CODICE_CUR", "PROG_ESITO", "ESITO", "DESC_ESITO", "METODICA", "LOINC", "RISULTATO_NUMERO", "DESC_RISULTATO", "DATA_ESECUZIONE", "DATA_REFERTAZIONE", "DATA_ULT_MOD");

                        //Assegna i PROG_ESITO in base al campo ID_RICHIESTA_PK
                        //dtDataTable = CalcolaProgressiviEsamiLiguria(dtDataTable);
                    }
                    break;
                case enumTipoTracciato.LIGURIA_TAR:
                    if (chkFileEsteso.Checked == false)
                    {
                        dv = dtDataTable.DefaultView;
                        dv.RowFilter = "Test_utilizzato LIKE 'COVRAG %' AND DESC_RISULTATO<>'ANNULLATO' AND COGNOME<>'PROVA'";
                        dv.Sort = "ID_RICHIESTA_PK ASC";
                        //dtDataTable = RemoveDuplicateRows(dv.ToTable(), "Codice Fiscale");

                        dtDataTable = dtDataTable.DefaultView.ToTable("Selected", false, "AZIENDA_INVIANTE", "DESC_AZIENDA_INVIANTE", "TIPOLOGIA_PAZIENTE", "COGNOME", "NOME", "SESSO", "DATA_NASCITA", "DATA_DECESSO", "COD_ISTAT_NASCITA", "COMUNE_NASCITA", "TIPO_IDENTIFICATIVO",
                            "CODICE_FISCALE", "COD_ISTAT_RESIDENZA", "COMUNE_RESIDENZA", "REGIONE_RESIDENZA", "VIA_RESIDENZA", "CIVICO_RESIDENZA", "CAP_RESIDENZA", "DESC_REGIONE_RESIDENZA", "COD_ISTAT_DOMICILIO",
                            "COMUNE_DOMICILIO", "VIA_DOMICILIO", "CIVICO_DOMICILIO", "CAP_DOMICILIO", "ID_RICHIESTA_PK", "DATA_PRELIEVO", "DATA_ACCETTAZIONE", "DESC_PROVENIENZA_CAMPIONE", "ID_CAMPIONE", "ID_MATERIALE",
                            "DESC_MATERIALE", "CODICE_ANALISI", "DESC_ANALISI", "CODICE_CUR", "PROG_ESITO", "ESITO", "DESC_ESITO", "METODICA", "LOINC", "RISULTATO_NUMERO", "DESC_RISULTATO", "DATA_ESECUZIONE", "DATA_REFERTAZIONE", "DATA_ULT_MOD");

                        //Assegna i PROG_ESITO in base al campo ID_RICHIESTA_PK
                        //dtDataTable = CalcolaProgressiviEsamiLiguria(dtDataTable);
                    }
                    break;
                case enumTipoTracciato.SARDEGNA:
                    if (chkFileEsteso.Checked == false)
                    {
                        dv = dtDataTable.DefaultView;
                        dv.RowFilter = " Esito<>'ANNULLATO' AND cognome<>'PROVA'";
                        //dtDataTable = RemoveDuplicateRows(dv.ToTable(), "Codice Fiscale");

                        dtDataTable = dtDataTable.DefaultView.ToTable("Selected", false, "regione", "azienda", "laboratorio", "id", "provenienza", "tipo_provenienza", "reparto", "determinazione", "data", "cognome", "nome",
                            "cf", "data_nascita", "genere", "comune", "comune_descrizione", "categoria", "qualifica", "esito_biomolecolare", "esito_immunologico");
                    }
                    break;
                case enumTipoTracciato.CAMPANIA:
                    if (chkFileEsteso.Checked == false)
                    {
                        dv = dtDataTable.DefaultView;
                        dv.RowFilter = " Esito<>'ANNULLATO' ";

                        dtDataTable = dtDataTable.DefaultView.ToTable("Selected", false, "CF", "Telefono", "Azienda", "Presidio", "Esposizione Operatore", "Test rapido associato", "Area di competenza", "Sintomatologia", "Data inizio sintomi",
                            "Posizione paziente", "Ospedale", "Soggetto in gravidanza", "Data presumibile parto", "Data prelievo", "Laboratorio di destinazione", "Tipologia Tampone", "Medico che verifica il Tampone", "Risultato",
                            "Prestazione eseguita", "Data prestazione", "Ora prestazione", "Codice NSIS", "ID Operatore che esegue il Tampone");
                    }
                    break;
                case enumTipoTracciato.PUGLIA:
                case enumTipoTracciato.PUGLIA_V1:
                case enumTipoTracciato.PUGLIA_V2:
                case enumTipoTracciato.PUGLIA_DENUNCE_COVID:
                case enumTipoTracciato.PUGLIA_DENUNCE_ANTIGENICI:
                    if (chkFileEsteso.Checked == false)
                    {
                        dv = dtDataTable.DefaultView;
                        switch (prmTipo)
                        {
                            case enumTipoTracciato.PUGLIA_V1:
                                dv.RowFilter = " ESITO<>'ANNULLATO' AND TRIM([NUMERO RICHIESTA])='' AND COGNOME<>'PROVA'";
                                dtDataTable = dtDataTable.DefaultView.ToTable("Selected", false, "PUNTO DI ACCESSO", "RESIDENZA", "CODICE TIPO ESAME", "CODICE LABORATORIO", "PROGRESSIVO ESAME", "DATA PRELIEVO", "PAZIENTE AL PRELIEVO", "CODICE FISCALE O STP O ENI PAZIENTE",
                                    "COGNOME PAZIENTE", "NOME PAZIENTE", "DATA NASCITA PAZIENTE", "STRUTTURA RICHIEDENTE", "ESITO ESAME", "DATA ESITO ESAME", "TIPO TEST", "TEST MOLECOLARE DOPO ANTIGENICO",
                                    "MOTIVO DELLA RICHIESTA", "PAZIENTE FUORI REGIONE", "TELEFONO FISSO PAZIENTE", "TELEFONO MOBILE PAZIENTE", "RECAPITO E-MAIL PAZIENTE");
                                break;
                            case enumTipoTracciato.PUGLIA_V2:
                                dv.RowFilter = "Test_utilizzato LIKE 'COVID %' AND TRIM([NUMERO RICHIESTA])<>'' AND ESITO<>'ANNULLATO' AND COGNOME<>'PROVA'";
                                dtDataTable = dtDataTable.DefaultView.ToTable("Selected", false, "NUMERO RICHIESTA", "CODICE TIPO ESAME", "CODICE LABORATORIO", "PROGRESSIVO ESAME", "DATA PRELIEVO", "PAZIENTE AL PRELIEVO",
                                    "CODICE FISCALE O STP O ENI PAZIENTE", "COGNOME PAZIENTE", "NOME PAZIENTE", "DATA NASCITA PAZIENTE", "ESITO ESAME", "DATA ESITO ESAME");
                                break;
                            case enumTipoTracciato.PUGLIA_DENUNCE_COVID:
                                //Tracciato Denunce ai dipartimenti
                                dv.RowFilter = "Test_utilizzato LIKE 'COVID %' AND ESITO<>'ANNULLATO' AND COGNOME<>'PROVA'";
                                dtDataTable = dtDataTable.DefaultView.ToTable("Selected", false, "PUNTO DI ACCESSO", "RESIDENZA", "NOME", "COGNOME", "DATA DI NASCITA", "CODICE FISCALE", "ALL'ATTO DEL PRELIEVO", "CITTA'", "INDIRIZZO", "TELEFONO", "TELEFONO FISSO", "PROVENIENZA", "E-MAIL");
                                break;
                            case enumTipoTracciato.PUGLIA_DENUNCE_ANTIGENICI:
                                //Tracciato Denunce ai dipartimenti
                                dv.RowFilter = "Test_utilizzato LIKE 'COVRAG %' AND ESITO<>'ANNULLATO' AND COGNOME<>'PROVA'";
                                dtDataTable = dtDataTable.DefaultView.ToTable("Selected", false, "PUNTO DI ACCESSO", "RESIDENZA", "NOME", "COGNOME", "DATA DI NASCITA", "CODICE FISCALE", "ALL'ATTO DEL PRELIEVO", "CITTA'", "INDIRIZZO", "TELEFONO", "TELEFONO FISSO", "PROVENIENZA", "E-MAIL");
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                default:
                    break;
            }



            StreamWriter sw = new StreamWriter(strFilePath, false);
            string tmpSep = ";";
            bool bIntesta = true;
            //headers  
            //Per la Liguria il CSV non deve avere intestazione
            //Per la Sardegna il file è ASCII e non CSV
            if (optSardegna.Checked)
            {
                bIntesta = false;
                tmpSep = "";
            }
            if (optLiguria.Checked == true && chkFileEsteso.Checked == false)
                bIntesta = false;
            if (bIntesta)
            {
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    sw.Write(dtDataTable.Columns[i]);
                    if (i < dtDataTable.Columns.Count - 1)
                        sw.Write(";");
                }
                sw.Write(sw.NewLine);
            }
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
                        sw.Write(tmpSep);
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
            else if (optParma.Checked)
                tmpTipo = enumTipoTracciato.PARMA;
            else if (optModena.Checked)
                tmpTipo = enumTipoTracciato.MODENA;
            else if (optPiemonte.Checked)
                tmpTipo = enumTipoTracciato.PIEMONTE;
            else if (optLiguria.Checked)
                tmpTipo = enumTipoTracciato.LIGURIA;
            else if (optSardegna.Checked)
                tmpTipo = enumTipoTracciato.SARDEGNA;
            else if (optCampania.Checked)
                tmpTipo = enumTipoTracciato.CAMPANIA;
            else if (optPuglia.Checked)
                tmpTipo = enumTipoTracciato.PUGLIA;
            else
                tmpTipo = enumTipoTracciato.ATS;

            if (lblFileOrigine.Text.Trim() != "")
            {
                DataTable dt = ReadCsvFile(tmpTipo);
                string sFileName = "";
                switch (tmpTipo)
                {
                    case enumTipoTracciato.ATS:
                        sFileName = Path.GetDirectoryName(Application.ExecutablePath) + (chkFileEsteso.Checked ? "\\JLab " : "\\ATS ");
                        if (optTamponi.Checked)
                            sFileName += "Tamponi ";
                        else if (optSierologici.Checked || optTestRapidi.Checked)
                            sFileName += "Sierologici ";
                        else if (optAntigenici.Checked)
                            sFileName += "Antigenici ";
                        sFileName += (optTradate.Checked ? "TRADATE" : "VIADANA") + ".CSV";
                        //Filtrare i dati con le condizioni
                        WriteDtToCSV(tmpTipo, dt, sFileName);
                        break;
                    case enumTipoTracciato.AUSLPC:

                        WriteDtToCSV(tmpTipo, dt, Path.GetDirectoryName(Application.ExecutablePath)
                            + (chkFileEsteso.Checked ? "\\JLab AUSL Piacenza " : "\\AUSL Piacenza ")
                            + ".CSV");
                        break;

                    case enumTipoTracciato.EMILIA:

                        WriteDtToCSV(tmpTipo, dt, Path.GetDirectoryName(Application.ExecutablePath)
                            + (chkFileEsteso.Checked ? "\\JLab ER Covid " : "\\AUSL ER Covid ")
                            + ".CSV");
                        break;

                    case enumTipoTracciato.PARMA:

                        WriteDtToCSV(tmpTipo, dt, Path.GetDirectoryName(Application.ExecutablePath)
                            + (chkFileEsteso.Checked ? "\\JLab Parma Covid " : "\\AUSL Parma Covid ")
                            + ".CSV");
                        break;
                    case enumTipoTracciato.MODENA:

                        WriteDtToCSV(tmpTipo, dt, Path.GetDirectoryName(Application.ExecutablePath)
                            + (chkFileEsteso.Checked ? "\\JLab Modena Covid " : "\\AUSL Modena Covid ")
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
                        //Scrive 2 File uno per sierologici e tamponi e l'altro per gli antigenici
                        WriteDtToCSV(tmpTipo, dt, Path.GetDirectoryName(Application.ExecutablePath)
                            + (chkFileEsteso.Checked ? "\\JLab Liguria Covid " : "\\COVLP888220")
                            + ".CSV");
                        WriteDtToCSV(enumTipoTracciato.LIGURIA_TAR, dt, Path.GetDirectoryName(Application.ExecutablePath)
                            + (chkFileEsteso.Checked ? "\\JLab Liguria Covid TAR " : "\\TAR888220")
                            + ".CSV");
                        break;
                    case enumTipoTracciato.SARDEGNA:

                        WriteDtToCSV(tmpTipo, dt, Path.GetDirectoryName(Application.ExecutablePath)
                            + (chkFileEsteso.Checked ? "\\JLab Sardegna Covid " : "\\737")
                            + DateTime.Now.Month.ToString("00") + DateTime.Now.Year.ToString().Substring(2, 2) + "T.0" + DateTime.Now.Day.ToString("00"));
                        break;
                    case enumTipoTracciato.CAMPANIA:
                        WriteDtToCSV(tmpTipo, dt, Path.GetDirectoryName(Application.ExecutablePath)
                            + (chkFileEsteso.Checked ? "\\JLab " : "\\Soresa ")
                            + (optTamponi.Checked ? "Tamponi IDR" : "Sierologici-TR IDR")
                            + ".CSV");
                        break;
                    case enumTipoTracciato.PUGLIA:
                        /*
                         File unico V1 Antigenici+COVID
                         File unico V2 solo molecolari
                         2 File Denunce uno per COVID e un per Antigenici
                         */
                        sFileName = Path.GetDirectoryName(Application.ExecutablePath) + (chkFileEsteso.Checked ? "\\JLab " : "\\GIAVA ");
                        sFileName += DateTime.Now.ToString("dd-MM-yyyy");
                        if (lblFileOrigine.Text.IndexOf("COVDE") > -1 || lblFileOrigine.Text.IndexOf("ANTDE") > -1)
                        {
                            WriteDtToCSV(enumTipoTracciato.PUGLIA_DENUNCE_ANTIGENICI, dt, sFileName + " ANTIGENICI DENUNCE.csv");
                            WriteDtToCSV(enumTipoTracciato.PUGLIA_DENUNCE_COVID, dt, sFileName + " COVID DENUNCE.csv");
                        }
                        //sFileName += (optTamponi.Checked ? "Tamponi " : "Antigenici ");
                        if (lblFileOrigine.Text.IndexOf("COGIA") > -1 || lblFileOrigine.Text.IndexOf("ANGIA") > -1)
                        {
                            WriteDtToCSV(enumTipoTracciato.PUGLIA_V1, dt, sFileName + " V1.csv");
                            WriteDtToCSV(enumTipoTracciato.PUGLIA_V2, dt, sFileName + " V2.csv");
                        }
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

        public DataTable CalcolaProgressiviEsamiLiguria(DataTable prmTable)
        {
            string tmpCodAcc = "";
            int k = 1;
            if (prmTable.Rows.Count > 0)
                tmpCodAcc = prmTable.Rows[0]["ID_RICHIESTA_PK"].ToString();
            for (int i = 0; i < prmTable.Rows.Count; i++)
            {
                if (tmpCodAcc == prmTable.Rows[0]["ID_RICHIESTA_PK"].ToString())
                {
                    prmTable.Rows[0]["PROG_ESITO"] = k;
                    k += 1;
                }
                else
                {
                    tmpCodAcc = prmTable.Rows[0]["ID_RICHIESTA_PK"].ToString();
                    prmTable.Rows[0]["PROG_ESITO"] = 1;
                    k += 1;
                }
            }
            return prmTable;
        }
        #endregion

        #region EVENTI
        private void cmdSelFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            lblFileOrigine.Text = openFileDialog1.FileName;
            if (lblFileOrigine.Text.IndexOf("COPIA") > -1)
                optPiacenza.Checked = true;
            else if (lblFileOrigine.Text.IndexOf("COVEM") > -1)
                optEmiliaRomagna.Checked = true;
            else if (lblFileOrigine.Text.IndexOf("COPAR") > -1)
                optParma.Checked = true;
            else if (lblFileOrigine.Text.IndexOf("COMOD") > -1)
                optModena.Checked = true;
            else if (lblFileOrigine.Text.IndexOf("COSAR") > -1)
                optSardegna.Checked = true;
            else if (lblFileOrigine.Text.IndexOf("COASL") > -1 || lblFileOrigine.Text.IndexOf("COALE") > -1)
                optPiemonte.Checked = true;
            else if (lblFileOrigine.Text.IndexOf("ALISA") > -1)
                optLiguria.Checked = true;
            else if (lblFileOrigine.Text.IndexOf("ANGIA") > -1 || lblFileOrigine.Text.IndexOf("ANTDE") > -1)
            {
                optAntigenici.Checked = true;
                optPuglia.Checked = true;
            }
            else if (lblFileOrigine.Text.IndexOf("COGIA") > -1 || lblFileOrigine.Text.IndexOf("COVDE") > -1)
            {
                optTamponi.Checked = true;
                optPuglia.Checked = true;
            }
            else if (lblFileOrigine.Text.IndexOf("CAM") > -1)
            {
                optCampania.Checked = true;
                if (lblFileOrigine.Text.IndexOf("CAMTP") > -1)
                    optTamponi.Checked = true;
                else if (lblFileOrigine.Text.IndexOf("CAMSI") > -1)
                    optSierologici.Checked = true;
                else
                    optTestRapidi.Checked = true;
            }
            else
            {
                if (optPiacenza.Checked == true)
                    optTradate.Checked = true;
                if (lblFileOrigine.Text.IndexOf("COATS") > -1)
                    optSierologici.Checked = true;
                else if (lblFileOrigine.Text.IndexOf("COTAM") > -1)
                    optTamponi.Checked = true;
            }
        }
        private void cmdGeneraFile_Click(object sender, EventArgs e)
        {
            GeneraFile();
        }
        private void optSede_CheckedChanged(object sender, EventArgs e)
        {
            pnlTipoFile.Visible = optTradate.Checked || optViadana.Checked || optCampania.Checked || optPuglia.Checked;
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

        private void button1_Click(object sender, EventArgs e)
        {
            string tempLineValue;
            int Prog = int.Parse("0" + txtStartProg.Text);
            int NRec = 0, NImp = 0, NEsaImp = 0, NEsa = 0, Qt = 0;
            decimal Lordo = 0, Ticket = 0, Netto = 0, ImpLordo = 0, ImpTicket = 0, ImpNetto = 0;
            txtNumRecord.Text = "0";
            txtNumImp.Text = "0";
            txtNumEsami.Text = "0";
            txtLordo.Text = "0";
            txtTicket.Text = "0";
            txtNetto.Text = "0";

            progressBar1.Value = 0;
            using (FileStream inputStream = File.OpenRead(lblFileOrigine.Text))
            {
                using (StreamReader inputReader = new StreamReader(inputStream))
                {
                    Stream baseStream = inputReader.BaseStream;
                    long length = baseStream.Length;
                    using (StreamWriter outputWriter = File.AppendText(lblFileOrigine.Text + ".ugo"))
                    {
                        while (null != (tempLineValue = inputReader.ReadLine()))
                        {
                            var aStringBuilder = new StringBuilder(tempLineValue);
                            if (lblFileOrigine.Text.IndexOf("SPS") > -1)
                            {
                                NRec += 1;
                                aStringBuilder.Remove(28, 20);
                                aStringBuilder.Insert(28, "61115220201001" + Prog.ToString("000000")); //Biotest
                                //aStringBuilder.Insert(28, "60830120200801" + Prog.ToString("000000")); //Citotest
                                //aStringBuilder.Insert(28, "20200050641001" + Prog.ToString("000000")); //Fleming
                                //aStringBuilder.Insert(28, "56308920201001" + Prog.ToString("000000")); //Selab
                                //aStringBuilder.Insert(28, "60580220200901" + Prog.ToString("000000")); //CMV
                                //aStringBuilder.Insert(28, "20206421451001" + Prog.ToString("000000")); //Emolab
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
                            else if (lblFileOrigine.Text.IndexOf("SPA") > -1)
                            {
                                aStringBuilder.Remove(25, 20);
                                aStringBuilder.Insert(25, "61115220201001" + Prog.ToString("000000")); //Biotest
                                //aStringBuilder.Insert(25, "60830120200801" + Prog.ToString("000000")); //Citotest
                                //aStringBuilder.Insert(25, "20200050641001" + Prog.ToString("000000")); //Fleming
                                //aStringBuilder.Insert(25, "56308920200801" + Prog.ToString("000000")); //Selab
                                //aStringBuilder.Insert(25, "60580220200901" + Prog.ToString("000000")); //CMV
                                //aStringBuilder.Insert(25, "20206421451001" + Prog.ToString("000000")); //Emolab

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

            MessageBox.Show("Elaborazione terminata!", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }




    }
}
