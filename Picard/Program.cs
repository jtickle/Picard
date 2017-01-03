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

            DoAuthentication(api, state);

            // If unauthenticated, that means they closed the login form
            // Just exit without error, don't write anything
            if (!api.isAuthenticated)
                return;

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
                    MessageBox.Show("Picard exited without updating Inara.cz.", "Picard");
                    return;
                }

                // Save changes to history
                state.AddHistory(resultProvider.GetDeltas());

                // Show post status form and do the post
                var post = new PostForm(api, state, resultProvider.GetTotals());
                Application.Run(post);
            }
            else
            {
                var form = new MatInitialVerifyForm(api);
                resultProvider = form;
                Application.Run(form);

                if (!resultProvider.ShouldSave())
                {
                    MessageBox.Show("Picard exited without saving history.", "Picard");
                    return;
                }

                // Save changes to history
                state.AddHistory(resultProvider.GetDeltas());
            }
        }

        /// <summary>
        /// Show and handle operation of the login form for the
        /// authentication portion of the execution
        /// </summary>
        /// <param name="api">The InaraApi instance to use for authentication</param>
        /// <param name="state">The PersistentState to use for saved authentication data</param>
        static void DoAuthentication(InaraApi api, PersistentState state)
        {
            // Show Login Form
            WelcomeLoginForm loginFrm = new WelcomeLoginForm();

            // Register Login Event
            loginFrm.TryAuthentication += async (object sender, EventArgs e) =>
            {
                loginFrm.SetLoginState();

                if(await api.Authenticate(loginFrm.User, loginFrm.Pass))
                {
                    state.UpdateInaraCreds(
                        loginFrm.User,
                        loginFrm.Pass,
                        api.cmdrName);
                    loginFrm.Close();
                }
                else
                {
                    loginFrm.ErrorLabel = api.lastError;
                    loginFrm.SetUserInputState();
                }
            };

            if (state.HasInaraCreds())
            {
                // Load the form up with saved credentials and tell it to
                // automatically try a login when shown.
                loginFrm.User = state.CurrentState.InaraU;
                loginFrm.Pass = state.CurrentState.InaraP;
                loginFrm.AutoLogin = true;
            }

            loginFrm.ShowDialog();
        }
    }
}
