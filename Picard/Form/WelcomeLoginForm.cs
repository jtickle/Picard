using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Picard
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

        public void SetLoginState()
        {
            LoginFormPanel.Hide();
            LoggingInPanel.Show();
            StatusLabel.Text = "Logging Into Inara.cz...";
        }

        public void SetUserInputState()
        {
            LoginFormPanel.Show();
            LoggingInPanel.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TryAuthentication(this, new EventArgs());
        }

        private void WelcomeLoginForm_Load(object sender, EventArgs e)
        {
            if (AutoLogin)
            {
                TryAuthentication(this, new EventArgs());
            }
        }
    }
}