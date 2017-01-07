using System;
using System.Windows.Forms;

namespace Picard.NormalRun
{
    public partial class PostForm : Form
    {
        public bool Loading
        {
            get
            {
                return !okButton.Enabled;
            }
            set
            {
                okButton.Enabled = !value;
                if(value)
                {
                    statusLabel.Text = "Posting Data to Inara...";
                    okButton.Text = "Posting...";
                }
                else
                {
                    statusLabel.Text = "Your data was successfully posted!";
                    okButton.Text = "Cool.";
                }
            }
        }

        public PostForm()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
