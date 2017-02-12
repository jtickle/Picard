using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Picard.CheapHack
{
    public partial class ChooseCmdrForm : Form
    {
        public event EventHandler<EventArgs> CommanderChosen;
        public string SelectedCommander
        {
            get
            {
                return CommanderList.SelectedItem.ToString();
            }
            protected set
            {
            }
        }

        public ChooseCmdrForm()
        {
            InitializeComponent();
        }

        public void AddCmdr(string cmdr)
        {
            CommanderList.Items.Add(cmdr);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if(CommanderChosen != null)
            {
                CommanderChosen(this, e);
            }
            Close();
        }
    }
}
