using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Picard.NormalRun
{
    public sealed partial class MatUpdateForm : Form
    {
        /// <summary>
        /// Fired when the user wishes to load materials, or when
        /// the form first loads
        /// </summary>
        public EventHandler<EventArgs> ReloadMats;

        /// <summary>
        /// Fired when the user wishes to post to Inara and save changes
        /// </summary>
        public EventHandler<CancelEventArgs> PostAndSave;

        /// <summary>
        /// Fired when the user wishes to close without saving
        /// </summary>
        public EventHandler<CancelEventArgs> CloseWithoutSave;

        /// <summary>
        /// Set to true to show the Inara correction column, false to hide
        /// </summary>
        public bool ShowCorrectionColumn
        {
            get
            {
                return MatsView.Columns[2].Width != 0;
            }
            set
            {
                MatsView.Columns[2].Width = value
                    ? 60
                    : 0;
            }
        }

        public MatUpdateForm()
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

        /// <summary>
        /// Indicate to the user that we are doing something
        /// </summary>
        public void SetLoadingState()
        {
            okButton.Enabled = false;
            refreshButton.Enabled = false;
        }

        /// <summary>
        /// Indicate to the user that the form is ready to be used
        /// </summary>
        /// <param name="canSave">True if there is data to be saved</param>
        public void SetReadyState(bool canSave)
        {
            refreshButton.Enabled = true;

            // Only enable okButton if there are changes to save.
            okButton.Enabled = canSave;

            if (okButton.Enabled)
            {
                okButton.Text = "Looks Good";
            }
            else
            {
                okButton.Text = "Nothing To Do";
            }
        }

        public void AddMaterial(string name, int last, int correction,
            int delta, int result)
        {
            var i = new ListViewItem(name);
            i.SubItems.Add(last.ToString());
            i.SubItems.Add(correction.ToString());
            i.SubItems.Add(delta.ToString());
            i.SubItems.Add(result.ToString());
            
            if(delta != 0)
            {
                // Bold if there was a change
                i.Font = new Font(i.Font, FontStyle.Bold);
            }

            if(delta > 0)
            {
                // Blue background if there was an increase
                i.BackColor = Color.LightBlue;
            }
            else if(delta < 0)
            {
                // Red background if there was a decrease
                i.BackColor = Color.Pink;
            }

            MatsView.Items.Add(i);
        }

        /// <summary>
        /// Ask the user how they want to handle an inconsistency between
        /// Picard and Inara
        /// </summary>
        public bool DoesUserWantInaraCorrection()
        {
            var result = MessageBox.Show(
                "There is a disagreement between Picard and Inara " +
                "regarding your current material counts.\n\n" +
                "If you recently MANUALLY updated Inara and want " +
                "to synchronize, click OK.\n\n" +
                "If you want to quit without saving, click Cancel.",
                "Why are these two characters even speaking to each other?",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button2);

            return result == DialogResult.OK;
        }

        public void NewInaraMaterialsNotification()
        {
            MessageBox.Show(
                "Good news: seems there was a game update!\n" +
                "Bad news: you will need to upgrade Picard.\n\n" +
                "A new version will be available soon from\n" +
                "https://jtickle.github.com/picard\n\n" +
                "In the mean time, Picard will quit without saving " +
                "in order to avoid risk to your data.",
                "Time to Upgrade!");
        }

        public void ShowNegativeValueBadNews(string StateFile)
        {
            MessageBox.Show(
                "Oh no.  Your Material count has gone into the negative, " +
                "which means something went wrong.  You will need to manually " +
                "re-synchronize with Inara and reset your Picard State file " +
                "by deleting " + StateFile,
                "Very Unfortunate Error");
        }

        public void ShowCloseWithoutSave()
        {
            MessageBox.Show("Picard is exiting without changing Inara or saving state.", "Picard");
        }

        private void MatUpdateForm_Load(object sender, EventArgs e)
        {
            ReloadMats(sender, new EventArgs());
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            ReloadMats(sender, new EventArgs());
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            var cancel = new CancelEventArgs();
            PostAndSave(sender, cancel);

            // Close the window if not cancelled
            if(!cancel.Cancel)
            {
                FormClosing -= MatUpdateForm_FormClosing;
                Close();
            }
        }

        private void MatUpdateForm_FormClosing(object sender, CancelEventArgs e)
        {
            CloseWithoutSave(sender, e);
        }
    }
}
