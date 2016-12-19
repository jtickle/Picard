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
    public sealed partial class MatUpdateForm : Form, IGetData
    {
        private InaraApi api;
        private PersistentState state;
        private EliteLogs logs;

        private bool shouldSave;
        private IDictionary<string, int> last;
        private IDictionary<string, int> deltas;
        private IDictionary<string, int> result;

        public MatUpdateForm(InaraApi api, PersistentState state)
        {
            this.api = api;
            this.state = state;
            logs = new EliteLogs();
            InitializeComponent();
        }

        private void ReloadMats()
        {
            MatsView.Items.Clear();
            shouldSave = false;

            last = state.CalculateCurrentInventory();
            deltas = logs.GetDeltasSince(state.GetLastUpdateTimestamp());
            result = DeltaTools.Add(last, deltas);

            foreach(var r in result)
            {
                var i = new ListViewItem(r.Key);

                if(last.ContainsKey(r.Key))
                {
                    i.SubItems.Add(last[r.Key].ToString());
                }
                else
                {
                    i.SubItems.Add("0");
                }

                if (deltas.ContainsKey(r.Key))
                {
                    i.SubItems.Add(deltas[r.Key].ToString());
                }
                else
                {
                    i.SubItems.Add("0");
                }

                i.SubItems.Add(r.Value.ToString());

                MatsView.Items.Add(i);
            }
        }

        IDictionary<string, int> IGetData.GetDeltas()
        {
            return this.deltas;
        }

        bool IGetData.ShouldSave()
        {
            return this.shouldSave;
        }

        private void MatUpdateForm_Load(object sender, EventArgs e)
        {
            ReloadMats();
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            ReloadMats();
        }
    }
}
