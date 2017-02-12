namespace Picard.CheapHack
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
            this.CommanderList = new System.Windows.Forms.ComboBox();
            this.okButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(260, 57);
            this.label1.TabIndex = 0;
            this.label1.Text = "Due to a lack of foresight, Picard only supports one commander.  We\'re working on" +
    " it, but for now, please choose which commander you would like to synchroinze wi" +
    "th Inara.";
            // 
            // CommanderList
            // 
            this.CommanderList.FormattingEnabled = true;
            this.CommanderList.Location = new System.Drawing.Point(42, 86);
            this.CommanderList.Name = "CommanderList";
            this.CommanderList.Size = new System.Drawing.Size(202, 21);
            this.CommanderList.TabIndex = 1;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(155, 134);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(117, 23);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "Save Commander";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // ChooseCmdrForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 169);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.CommanderList);
            this.Controls.Add(this.label1);
            this.Name = "ChooseCmdrForm";
            this.Text = "Choose Commander";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox CommanderList;
        private System.Windows.Forms.Button okButton;
    }
}