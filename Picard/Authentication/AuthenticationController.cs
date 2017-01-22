using System;
using Picard.Lib;

namespace Picard.Authentication
{
    class AuthenticationController : IDisposable
    {
        protected InaraApi api;
        protected PersistentState state;

        protected WelcomeLoginForm form;

        public AuthenticationController(InaraApi api, PersistentState state)
        {
            this.api = api;
            this.state = state;

            form = new WelcomeLoginForm();
            form.TryAuthentication += OnTryAuthentication;
        }

        protected async void OnTryAuthentication (object sender, EventArgs e)
        {
            // Disable Inputs and show "logging in"
            form.SetLoginState();

            // Attempt Authentication with InaraAPI
            if (await api.Authenticate(form.User, form.Pass))
            {
                // Successful; update credentials and close the form
                state.UpdateInaraCreds(
                    form.User,
                    form.Pass,
                    api.cmdrName);
                state.Persist();
                form.Close();
            }
            else
            {
                // Failure; enable inputs and show error
                form.ErrorLabel = api.lastError;
                form.SetUserInputState();
            }

        }

        /// <summary>
        /// Show and handle operation of the login form for the
        /// authentication portion of the execution
        /// </summary>
        public void Run()
        {
            // If we already have saved Inara credentials, load the form up with
            // them and automatically try to login once the UI is visible
            if (state.HasInaraCreds())
            {
                form.User = state.CurrentState.InaraU;
                form.Pass = state.CurrentState.InaraP;
                form.AutoLogin = true;
            }

            // Show and Wait
            form.ShowDialog();
        }

        public void Dispose()
        {
            ((IDisposable)form).Dispose();
        }
    }
}
