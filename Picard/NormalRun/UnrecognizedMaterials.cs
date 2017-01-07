using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Picard.NormalRun
{
    public partial class UnrecognizedMaterials : Form
    {
        protected IDictionary<string, int> unknown;

        public UnrecognizedMaterials(IDictionary<string, int> unknown)
        {
            this.unknown = unknown;
            InitializeComponent();
        }

        private void UnrecognizedMaterials_Load(object sender, EventArgs e)
        {
            string clipboard = "";
            ListViewItem i;

            foreach(var u in unknown)
            {
                i = new ListViewItem(u.Key);
                i.SubItems.Add(u.Value.ToString());
                listView1.Items.Add(i);

                clipboard = clipboard + "IgnoreCommodities.Add(\"" + u.Key + "\");\n";
            }

            Clipboard.SetDataObject(clipboard, true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
