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
            if (state.HasInaraCreds())
            {
                loginFrm.loginWithCredentials(
                    state.CurrentState.InaraU,
                    state.CurrentState.InaraP);
            }
            loginFrm.ShowDialog();

            // If unauthenticated, that means they closed the login form
            // Just exit without error
            if (!api.isAuthenticated)
                return;

            // Save Credentials
            state.UpdateInaraCreds(loginFrm.user, loginFrm.pass, api.cmdrName);

            // Show Main Form if there is history; otherwise perform an
            // initial import and then exit
            //IGetData matVerifyForm = state.HasHistory()
            //    ? new MatUpdateForm(api)
            //    : new MatInitialVerifyForm(api);
            var matVerifyForm = new MatInitialVerifyForm(api);
            Application.Run(matVerifyForm);

            // If the user closed without saving, exit without error
            if (!matVerifyForm.ShouldSave)
                return;

            // Save the Deltas from today
            state.AddHistory(matVerifyForm.Deltas);
        }
    }
}
