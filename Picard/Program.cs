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

            // Show Login Form
            WelcomeLoginForm loginFrm = new WelcomeLoginForm(api);
            if (state.HasInaraCreds())
            {
                // Show but Login automatically if we remember creds from last time
                // This should gracefully handle errors such as a changed password
                loginFrm.loginWithCredentials(
                    state.CurrentState.InaraU,
                    state.CurrentState.InaraP);
            }
            loginFrm.ShowDialog();

            // If unauthenticated, that means they closed the login form
            // Just exit without error, don't write anything
            if (!api.isAuthenticated)
                return;

            // Save Credentials
            state.UpdateInaraCreds(loginFrm.user, loginFrm.pass, api.cmdrName);

            // Show Main Form if there is history; otherwise perform an
            // initial import and then exit
            IGetData resultProvider;
            if(state.HasHistory())
            {
                var form = new MatUpdateForm(api, state);
                resultProvider = form;
                Application.Run(form);

                if (!resultProvider.ShouldSave())
                {
                    MessageBox.Show("Picard exited without saving state or updating Inara.cz.", "Picard");
                    return;
                }

                // Show post status form and do the post
                var post = new PostForm(api, state, resultProvider.GetTotals());
                Application.Run(post);
            }
            else
            {
                var form = new MatInitialVerifyForm(api);
                resultProvider = form;
                Application.Run(form);
            }

            // If the user closed without saving, exit without error
            if (!resultProvider.ShouldSave())
            {
                MessageBox.Show("Picard exited without saving state or updating Inara.cz.", "Picard");
                return;
            }

            // Save changes to history
            state.AddHistory(resultProvider.GetDeltas());
        }
    }
}
