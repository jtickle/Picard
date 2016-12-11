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

            InaraApi api = new InaraApi();
            PersistentState state = new PersistentState();

            // Show Login Form and login automatically if we remember creds
            WelcomeLoginForm loginFrm = new WelcomeLoginForm(api);
            if (state.HasState())
            {
                loginFrm.loginWithCredentials(
                    state.CurrentState.InaraU,
                    state.CurrentState.InaraP);
            }
            loginFrm.ShowDialog();

            // If unauthenticated, that means they closed the login form
            // Just exit without error
            if (!api.isAuthenticated)
            {
                return;
            }

            // Save Credentials
            state.UpdateInaraCreds(loginFrm.user, loginFrm.pass);
            Application.Run(new MatInitialVerifyForm(api));
        }
    }
}
