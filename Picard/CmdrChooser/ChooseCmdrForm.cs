using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Picard.CmdrChooser
{
    public partial class ChooseCmdrForm : Form
    {
        public event EventHandler<EventArgs> LocationSelected;
        public event EventHandler<EventArgs> CmdrSelected;

        protected bool PathStale = false;

        public string JournalPath
        {
            get
            {
                return JournalPathEntry.Text;
            }

            set
            {
                JournalPathEntry.Text = value;
            }
        }

        public string SelectedCmdr
        {
            get
            {
                return CmdrSelector.Text;
            }
            set
            {
                if(value != null && CmdrSelector.Items.Contains(value))
                {
                    CmdrSelector.SelectedIndex =
                        CmdrSelector.Items.IndexOf(value);
                    SetCmdrSelectedState();
                }
            }
        }

        public bool FirstLoad = true;

        public ChooseCmdrForm(string InitialDirectory)
        {
            InitializeComponent();

            JournalPathEntry.Text = InitialDirectory;
        }

        public void ShowDirectoryNotExists()
        {
            if (FirstLoad) return;

            MessageBox.Show("The selected directory does not exist.", "Journal Problem",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ShowDirectoryNotJournal()
        {
            if (FirstLoad) return;

            MessageBox.Show("The selected directory contains no journals.", "Journal Problem",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            var folderDialog = new FolderBrowserDialog();
            folderDialog.SelectedPath = JournalPathEntry.Text;
            var result = folderDialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                JournalPathEntry.Text = folderDialog.SelectedPath;
                if(LocationSelected != null)
                {
                    LocationSelected(this, e);
                }
            }
        }

        public void ClearCmdrs()
        {
            CmdrSelector.Items.Clear();
        }

        public void AddCmdrs(ICollection<string> Cmdrs)
        {
            CmdrSelector.Items.AddRange(Cmdrs.ToArray());
        }

        public void SetInvalidJournalState()
        {
            LoadButton.Enabled = false;
            CmdrSelector.Enabled = false;
        }

        public void SetNoCmdrSelectedState()
        {
            LoadButton.Enabled = false;
            CmdrSelector.Enabled = true;
        }

        public void SetCmdrSelectedState()
        {
            LoadButton.Enabled = true;
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CmdrSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(CmdrSelected != null)
            {
                CmdrSelected(this, e);
            }
        }

        private void JournalPathEntry_Leave(object sender, EventArgs e)
        {
            if (PathStale)
            {
                PathStale = false;
                if (LocationSelected != null)
                {
                    LocationSelected(this, e);
                }
            }
        }

        private void JournalPathEntry_TextChanged(object sender, EventArgs e)
        {
            PathStale = true;
        }
    }
}
