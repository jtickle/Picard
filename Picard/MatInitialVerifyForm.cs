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
    public partial class MatInitialVerifyForm : Form, IGetData
    {
        private InaraApi api;

        private bool shouldSave;
        private IDictionary<string, int> Deltas;

        public MatInitialVerifyForm(InaraApi api)
        {
            this.api = api;
            InitializeComponent();
        }

        private async void ReloadMats()
        {
            MatsView.Items.Clear();
            shouldSave = false;

            Deltas = await api.GetMaterialsSheet();

            foreach (var mat in Deltas)
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
            // Alert the user if the data is all zeroes
            bool allZero = true;
            foreach (var mat in Deltas)
            {
                if (mat.Value != 0)
                {
                    allZero = false;
                    break;
                }
            }
            if (allZero)
            {
                var Deltas = MessageBox.Show(this,
                    "According to Inara, you have no materials.  If you have just started playing Elite: Dangerous or if you have created a new character, this is fine.  But usually you are coming into this with more mats.  Are you sure you want to start from here with 0 mats?",
                    "No Materials in Inara!",
                    MessageBoxButtons.OKCancel);
                if(Deltas != DialogResult.OK)
                {
                    return;
                }
            }

            shouldSave = true;
            Close();
        }

        private void MatInitialVerifyForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (shouldSave) return;

            var Deltas = MessageBox.Show(this,
                "Picard will not be synchronized with your Inara and Elite data.",
                "Really Exit?",
                MessageBoxButtons.OKCancel);

            if(Deltas != DialogResult.OK)
            {
                e.Cancel = true;
            }
        }

        IDictionary<string, int> IGetData.GetDeltas()
        {
            return Deltas;
        }

        bool IGetData.ShouldSave()
        {
            return shouldSave;
        }
    }
}
