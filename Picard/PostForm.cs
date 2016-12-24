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
    public partial class PostForm : Form
    {
        private IDictionary<string, int> result;
        private InaraApi api;
        private PersistentState state;

        public PostForm(InaraApi capi, PersistentState cstate,
            IDictionary<string, int> cresult)
        {
            state = cstate;
            result = cresult;
            api = capi;
            InitializeComponent();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void PostForm_Load(object sender, EventArgs e)
        {
            statusLabel.Text = "Posting Data to Inara...";

            await api.PostMaterialsSheet(result);

            statusLabel.Text = "Your data was successfully posted!";
            okButton.Text = "Cool";
            okButton.Enabled = true;
        }
    }
}
