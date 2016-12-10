using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Picard
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // TODO: Check for Data File

            // Prompt for Authentication
            Application.Run(new PicardContext());

            // TODO: check for data file, prompt for Authentication
            // TODO: check for LastInaraSnapshotTimestamp, do InaraInit, exit
            
            //Application.Run(new MatInitialVerifyForm());
        }
    }
}
