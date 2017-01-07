using System;
using System.Windows.Forms;

namespace Picard.Authentication
{
    public partial class WelcomeLoginForm : Form
    {
        /// <summary>
        /// Fired when the user wishes to try an authentication.  Also
        /// fires automatically when the window loads if AutoLogin is
        /// set.
        /// </summary>
        public event EventHandler<EventArgs> TryAuthentication;

        /// <summary>
        /// If true, the window will fire off an immediate authentication
        /// event when it successfully loads and displays.
        /// </summary>
        public bool AutoLogin = false;

        /// <summary>
        /// Username field for Inara authentication
        /// </summary>
        public string User
        {
            get
            {
                return usernameBox.Text;
            }
            set
            {
                usernameBox.Text = value;
            }
        }

        /// <summary>
        /// Password field for Inara authentication
        /// </summary>
        public string Pass
        {
            get
            {
                return passwordBox.Text;
            }
            set
            {
                passwordBox.Text = value;
            }
        }

        /// <summary>
        /// Bright red text indicating what went wrong
        /// </summary>
        public string ErrorLabel
        {
            get
            {
                return errorLabel.Text;
            }
            set
            {
                errorLabel.Text = value;
            }
        }

        public WelcomeLoginForm()
        {
            InitializeComponent();
            User = "";
            Pass = "";
        }

        /// <summary>
        /// Hide inputs and indicate that the program is doing something
        /// </summary>
        public void SetLoginState()
        {
            LoginFormPanel.Hide();
            LoggingInPanel.Show();
            StatusLabel.Text = "Logging Into Inara.cz...";
        }

        /// <summary>
        /// Show inputs
        /// </summary>
        public void SetUserInputState()
        {
            LoginFormPanel.Show();
            LoggingInPanel.Hide();
        }

        /// <summary>
        /// The user indicates that they wish to attempt authentication
        /// </summary>
        /// <param name="sender">Ignored</param>
        /// <param name="e">Ignored</param>
        private void button1_Click(object sender, EventArgs e)
        {
            TryAuthentication(this, new EventArgs());
        }

        /// <summary>
        /// If AutoLogin has been set to true, we presume that so has User and
        /// Pass, and attempt a login as soon as the form is visible.
        /// </summary>
        /// <param name="sender">Ignored</param>
        /// <param name="e">Ignored</param>
        private void WelcomeLoginForm_Load(object sender, EventArgs e)
        {
            if (AutoLogin)
            {
                TryAuthentication(this, new EventArgs());
            }
        }
    }
}