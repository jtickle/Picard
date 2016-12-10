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
        private InaraApi api;

        private WelcomeLoginForm()
        {
        }

        public WelcomeLoginForm(InaraApi api)
        {
            this.api = api;
            InitializeComponent();
        }

        private void WelcomeLoginForm_Load(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            LoginFormPanel.Hide();
            LoggingInPanel.Show();
            StatusLabel.Text = "Logging Into Inara.cz...";

            await api.Authenticate(usernameBox.Text, passwordBox.Text);

            StatusLabel.Text = "Waiting...";
            LoggingInPanel.Hide();
            LoginFormPanel.Show();

            if (api.isAuthenticated)
            {
                Close();
            }
            else
            {
                errorLabel.Text = api.lastError;
            }
        }
    }
}
