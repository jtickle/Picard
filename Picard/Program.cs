using System;
using System.Windows.Forms;
using Picard.Authentication;
using Picard.FirstRun;
using Picard.NormalRun;
using Picard.Lib;

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
            // We would like to thank Microsoft for their generous donation of
            // these next two codes.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Inara.cz API
            InaraApi api = new InaraApi();

            // Elite: Dangerous Gameplay Logs
            EliteLogs logs = new EliteLogs();

            // Picard Persistent State
            PersistentState state = new PersistentState();

            // Show a login dialog box and handle user authentication with Inara
            AuthenticationController authCtrl = new AuthenticationController(api, state);
            authCtrl.Run();

            // If unauthenticated, that means they closed the login form
            // Just exit without error, don't write anything
            if (!api.isAuthenticated)
                return;
            
            if(!state.HasHistory())
            {
                // If there is no stored history, perform an initial
                // import and then exit
                FirstRunController firstRunCtrl = new FirstRunController(api, state);
                firstRunCtrl.Run();
            }
            else
            {
                // If there is stored history, run the main program.
                NormalRunController normalRunCtrl = new NormalRunController(api, state, logs);
                normalRunCtrl.Run();
            }
        }
    }
}
