using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace waCOVID
{
    static class Program
    {
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Thread.CurrentThread.CurrentCulture = new CultureInfo("it-IT", false);
            if (args.Length == 1 && args[0] == "FLUSSIVENETO")
                Application.Run(new frmFlussi(frmFlussi.enumRegioni.Veneto));
            if (args.Length == 1 && args[0] == "FLUSSILAZIO")
                Application.Run(new frmFlussi(frmFlussi.enumRegioni.Lazio));
            else if (args.Length == 1 && args[0] == "FLUSSI")
                Application.Run(new frmFlussi(frmFlussi.enumRegioni.Tutte));
            else
                Application.Run(new frmStatCOVID());


        }
    }
}

