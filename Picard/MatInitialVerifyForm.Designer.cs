namespace Picard
{
    partial class MatInitialVerifyForm
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.okButton = new System.Windows.Forms.Button();
            this.refreshButton = new System.Windows.Forms.Button();
            this.MatsView = new System.Windows.Forms.ListView();
            this.MatName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MatCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.okButton);
            this.flowLayoutPanel1.Controls.Add(this.refreshButton);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 580);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(521, 29);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // okButton
            // 
            this.okButton.AccessibleDescription = "The data looks good.";
            this.okButton.AccessibleName = "Looks Good";
            this.okButton.Location = new System.Drawing.Point(443, 3);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "Looks Good";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.okButton_MouseClick);
            // 
            // refreshButton
            // 
            this.refreshButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.refreshButton.Location = new System.Drawing.Point(362, 3);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(75, 23);
            this.refreshButton.TabIndex = 1;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.refreshButton_MouseClick);
            // 
            // MatsView
            // 
            this.MatsView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.MatName,
            this.MatCount});
            this.MatsView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MatsView.GridLines = true;
            this.MatsView.Location = new System.Drawing.Point(0, 0);
            this.MatsView.MultiSelect = false;
            this.MatsView.Name = "MatsView";
            this.MatsView.Size = new System.Drawing.Size(521, 580);
            this.MatsView.TabIndex = 1;
            this.MatsView.UseCompatibleStateImageBehavior = false;
            this.MatsView.View = System.Windows.Forms.View.Details;
            // 
            // MatName
            // 
            this.MatName.Text = "Material";
            // 
            // MatCount
            // 
            this.MatCount.Text = "Inventory";
            // 
            // MatInitialVerifyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 609);
            this.Controls.Add(this.MatsView);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "MatInitialVerifyForm";
            this.Text = "Verify Existing Materials";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.ListView MatsView;
        private System.Windows.Forms.ColumnHeader MatName;
        private System.Windows.Forms.ColumnHeader MatCount;
    }
}

