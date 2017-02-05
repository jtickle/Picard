using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Picard.FirstRun
{
    public sealed partial class MatInitialVerifyForm : Form
    {
        /// <summary>
        /// Fired when the form first shows.  The expectation is that
        /// the list of materials will be populated with current data
        /// from Inara.
        /// </summary>
        public event EventHandler<EventArgs> FirstLoadMats;

        /// <summary>
        /// Fired when the user requests a reload.  The expectation is
        /// that the list of materials will be cleared and populated
        /// with current data from Inara.
        /// </summary>
        public event EventHandler<EventArgs> ReloadMats;

        /// <summary>
        /// Fired when the user requests to close the window without saving.
        /// Can prevent the window from closing by setting EventArgs.Cancel.
        /// </summary>
        public event EventHandler<CancelEventArgs> CloseWithoutSaving;

        /// <summary>
        /// Fired when the user requests to save and close.  Can prevent the
        /// window from closing by setting EventArgs.Cancel.
        /// </summary>
        public event EventHandler<CancelEventArgs> CloseAndSave;

        private ListViewItem LoadingItem;

        public MatInitialVerifyForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Clears the materials list
        /// </summary>
        public void ClearList()
        {
            MatsView.Items.Clear();
        }

        public void SetLoadingState()
        {
            MatsView.Items.Clear();
            LoadingItem = new ListViewItem("Loading...");
            MatsView.Items.Add(LoadingItem);
            refreshButton.Enabled = false;
            okButton.Enabled = false;
        }

        public void SetReadyState()
        {
            if (LoadingItem != null) {
                MatsView.Items.Remove(LoadingItem);
            }
            refreshButton.Enabled = true;
            okButton.Enabled = true;
        }

        /// <summary>
        /// Adds a material to the list
        /// </summary>
        /// <param name="name">Material Name</param>
        /// <param name="value">Material Count</param>
        public void AddMaterial(string name, int value)
        {
            var i = new ListViewItem(name);
            i.SubItems.Add(value.ToString());
            MatsView.Items.Add(i);
        }

        /// <summary>
        /// Pop up a box asking if the user is OK with the fact that
        /// Inara and Elite will not be synchronized
        /// </summary>
        /// <returns>True if the user is OK</returns>
        public bool IsUserOKWithNoSync()
        {
            var ret = MessageBox.Show(
                "Picard will not be synchronized with your Inara and Elite data.",
                "Really Exit?",
                MessageBoxButtons.OKCancel);
                
            return ret == DialogResult.OK;
        }

        /// <summary>
        /// Pop up a box asking if the user is OK with the fact that
        /// state will be saved with 0 balances for materials
        /// </summary>
        /// <returns>True if the user is OK</returns>
        public bool IsUserOKWithNoMats()
        {
            var ret = MessageBox.Show(
                "According to Inara, you have no materials.  If your Elite Dangerous: Horizons " +
                "character actually has no materials, then this is fine.  However, if " +
                "your Elite Dangerous character does have any engineering materials, " +
                "you need to enter your current balances into Inara manually, and then " +
                "Picard will update those balances in future runs.",
                "No Materials in Inara!",
                MessageBoxButtons.OKCancel);

            return ret == DialogResult.OK;
        }

        /// <summary>
        /// On load, we issue a ReloadMats event
        /// </summary>
        /// <param name="sender">I'm just here so I don't get fined</param>
        /// <param name="e">Ignored</param>
        private void Form1_Load(object sender, EventArgs e)
        {
            FirstLoadMats(this, new EventArgs());
        }

        /// <summary>
        /// On User Clicks Refresh, we issue a ReloadMats event
        /// </summary>
        /// <param name="sender">Ignored</param>
        /// <param name="e">Ignored</param>
        private void refreshButton_MouseClick(object sender, MouseEventArgs e)
        {
            ReloadMats(this, new EventArgs());
        }

        /// <summary>
        /// On User Clicks OK, we issue a CloseAndSave event
        /// </summary>
        /// <param name="sender">Ignored</param>
        /// <param name="e">e.Cancel can be used to prevent closing</param>
        private void okButton_MouseClick(object sender, MouseEventArgs e)
        {
            var cancel = new CancelEventArgs();
            CloseAndSave(this, cancel);

            if (!cancel.Cancel)
            {
                // Disable the CloseWithoutSave event and then close
                FormClosing -= MatInitialVerifyForm_FormClosing;
                Close();
            }
        }

        /// <summary>
        /// On User Clicks the X, we issue a CloseWithoutSaving event.
        /// </summary>
        /// <param name="sender">Ignored</param>
        /// <param name="e">e.Cancel can be used to prevent closing</param>
        private void MatInitialVerifyForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseWithoutSaving(this, e);
        }
    }
}
