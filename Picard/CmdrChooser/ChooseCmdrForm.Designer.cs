namespace Picard.CmdrChooser
{
    partial class ChooseCmdrForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.JournalPathEntry = new System.Windows.Forms.TextBox();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.NextStepLabel = new System.Windows.Forms.Label();
            this.CmdrSelector = new System.Windows.Forms.ComboBox();
            this.LoadButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(328, 31);
            this.label1.TabIndex = 0;
            this.label1.Text = "Find your Journal file path.  It should be under your user profile directory, in " +
    "a folder called \"Saved Games\" in your language.";
            // 
            // JournalPathEntry
            // 
            this.JournalPathEntry.Location = new System.Drawing.Point(24, 43);
            this.JournalPathEntry.Name = "JournalPathEntry";
            this.JournalPathEntry.Size = new System.Drawing.Size(235, 20);
            this.JournalPathEntry.TabIndex = 1;
            this.JournalPathEntry.TextChanged += new System.EventHandler(this.JournalPathEntry_TextChanged);
            this.JournalPathEntry.Leave += new System.EventHandler(this.JournalPathEntry_Leave);
            // 
            // BrowseButton
            // 
            this.BrowseButton.Location = new System.Drawing.Point(265, 41);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(75, 23);
            this.BrowseButton.TabIndex = 2;
            this.BrowseButton.Text = "&Browse...";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // NextStepLabel
            // 
            this.NextStepLabel.AutoSize = true;
            this.NextStepLabel.Location = new System.Drawing.Point(12, 76);
            this.NextStepLabel.Name = "NextStepLabel";
            this.NextStepLabel.Size = new System.Drawing.Size(155, 13);
            this.NextStepLabel.TabIndex = 5;
            this.NextStepLabel.Text = "Select a commander to update.";
            // 
            // CmdrSelector
            // 
            this.CmdrSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmdrSelector.FormattingEnabled = true;
            this.CmdrSelector.Location = new System.Drawing.Point(24, 92);
            this.CmdrSelector.Name = "CmdrSelector";
            this.CmdrSelector.Size = new System.Drawing.Size(263, 21);
            this.CmdrSelector.TabIndex = 6;
            this.CmdrSelector.SelectedIndexChanged += new System.EventHandler(this.CmdrSelector_SelectedIndexChanged);
            // 
            // LoadButton
            // 
            this.LoadButton.Location = new System.Drawing.Point(231, 137);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(109, 23);
            this.LoadButton.TabIndex = 7;
            this.LoadButton.Text = "Load Commander";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(150, 137);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 8;
            this.CloseButton.Text = "E&xit";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // ChooseCmdrForm
            // 
            this.AcceptButton = this.LoadButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(352, 176);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.LoadButton);
            this.Controls.Add(this.CmdrSelector);
            this.Controls.Add(this.NextStepLabel);
            this.Controls.Add(this.BrowseButton);
            this.Controls.Add(this.JournalPathEntry);
            this.Controls.Add(this.label1);
            this.Name = "ChooseCmdrForm";
            this.Text = "Choose Commander";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox JournalPathEntry;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.Label NextStepLabel;
        private System.Windows.Forms.ComboBox CmdrSelector;
        private System.Windows.Forms.Button LoadButton;
        private System.Windows.Forms.Button CloseButton;
    }
}