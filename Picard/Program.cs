using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using Picard.Authentication;
using Picard.FirstRun;
using Picard.NormalRun;
using Picard.Lib;
using Picard.CmdrChooser;
using Picard.CheapHack;
using LibEDJournal;

namespace Picard
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // We would like to thank Microsoft for their generous donation of
            // these next two codes.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Set up Internal Constants
            DataMangler dm = DataMangler.GetInstance();

            // Set up Registry Keys
            Configuration cfg = new Configuration();

            // See if we need to upgrade the state file
            HandleUpgrade();

            // Program will run in a loop until user chooses "Exit"
            // on the chooser, or in the main window
            ChooseCmdrController choose = new ChooseCmdrController(cfg);
            while(choose.Run())
            {
                // Picard Persistent State
                PersistentState state = new PersistentState(
                    Filesystem.GetStatePath(choose.CmdrName));

                // Inara.cz API
                InaraApi api = new InaraApi();

                // Elite Dangerous Gameplay Logs
                EliteJournalParser logs = new EliteJournalParser(
                    choose.JournalPath);
                
                // Show a login dialog box and handle user authentication with Inara
                AuthenticationController authCtrl = new AuthenticationController(api, state);
                authCtrl.Run();

                // If unauthenticated, that means they closed the login form
                // Just exit without error, don't write anything
                if (!api.isAuthenticated)
                    continue;

                if (!state.HasHistory())
                {
                    // If there is no stored history, perform an initial
                    // import and then exit
                    FirstRunController firstRunCtrl = new FirstRunController(
                        api, state);
                    firstRunCtrl.Run();
                }
                else
                {
                    // If there is stored history, run the main program.
                    NormalRunController normalRunCtrl = new NormalRunController(
                        api, state, logs, dm);
                    normalRunCtrl.Run();
                }
            }

            /*if(!state.HasEliteCmdrName())
            {
                StateFileUpgrader.StateFileUpgrader drat = new StateFileUpgrader.StateFileUpgrader(logs, state);
                drat.Run();
            }

            if (state.HasEliteCmdrName())
            {
            }*/
        }

        public static void HandleUpgrade()
        {
            var old = Filesystem.OldStateFile;
            if(File.Exists(old))
            {
                PersistentState state = new PersistentState(old);

                if(!state.HasEliteCmdrName())
                {
                    CheapHackController hack = new CheapHackController(
                        new EliteJournalParser(Filesystem.GuessJournalPath()),
                        state);
                    hack.Run();

                    if(!state.HasEliteCmdrName())
                    {
                        MessageBox.Show("Picard is exiting without saving.");
                        Application.Exit();
                        return;
                    }
                }

                state.StateFile = Filesystem.GetStatePath(
                    state.CurrentState.EliteCmdrName);
                state.Persist();
                File.Delete(old);
                MessageBox.Show("Your Picard state file has been upgraded.", "Multi-Commander Support!!");
            }
        }
    }
}
