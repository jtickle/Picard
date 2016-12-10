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
    public partial class MatInitialVerifyForm : Form
    {
        private InaraApi api;

        public MatInitialVerifyForm(InaraApi api)
        {
            this.api = api;
            InitializeComponent();
        }

        private async void ReloadMats()
        {
            MatsView.Items.Clear();

            var mats = await api.GetMaterialsSheet();

            foreach (var mat in mats)
            {
                var i = new ListViewItem(mat.Key);
                i.SubItems.Add(mat.Value.ToString());
                MatsView.Items.Add(i);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ReloadMats();
        }

        private void refreshButton_MouseClick(object sender, MouseEventArgs e)
        {
            ReloadMats();
        }

        private void okButton_MouseClick(object sender, MouseEventArgs e)
        {

        }
    }
}
