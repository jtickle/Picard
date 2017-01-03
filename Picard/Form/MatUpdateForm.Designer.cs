namespace Picard
{
    partial class MatUpdateForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.MatsView = new System.Windows.Forms.ListView();
            this.MatName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MatLast = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MatDelta = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MatResult = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MatCorrection = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
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
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 603);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(555, 29);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // okButton
            // 
            this.okButton.AccessibleDescription = "The data looks good.";
            this.okButton.AccessibleName = "Looks Good";
            this.okButton.AutoSize = true;
            this.okButton.Location = new System.Drawing.Point(477, 3);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "Looks Good";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.AutoSize = true;
            this.refreshButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.refreshButton.Location = new System.Drawing.Point(396, 3);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(75, 23);
            this.refreshButton.TabIndex = 1;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(555, 32);
            this.panel1.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(426, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please give this the ol\' once-over and compare it with what you have in Elite: Da" +
    "ngerous.";
            // 
            // MatsView
            // 
            this.MatsView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.MatName,
            this.MatLast,
            this.MatCorrection,
            this.MatDelta,
            this.MatResult});
            this.MatsView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MatsView.GridLines = true;
            this.MatsView.Location = new System.Drawing.Point(0, 32);
            this.MatsView.MultiSelect = false;
            this.MatsView.Name = "MatsView";
            this.MatsView.Size = new System.Drawing.Size(555, 571);
            this.MatsView.TabIndex = 4;
            this.MatsView.UseCompatibleStateImageBehavior = false;
            this.MatsView.View = System.Windows.Forms.View.Details;
            // 
            // MatName
            // 
            this.MatName.Text = "Material";
            this.MatName.Width = 270;
            // 
            // MatLast
            // 
            this.MatLast.Text = "Current";
            // 
            // MatDelta
            // 
            this.MatDelta.Text = "Delta";
            // 
            // MatResult
            // 
            this.MatResult.Text = "Elite";
            // 
            // MatCorrection
            // 
            this.MatCorrection.Text = "Correction";
            // 
            // MatUpdateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 632);
            this.Controls.Add(this.MatsView);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.panel1);
            this.Name = "MatUpdateForm";
            this.Text = "Update Inara.cz From Elite Logs";
            this.Load += new System.EventHandler(this.MatUpdateForm_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView MatsView;
        private System.Windows.Forms.ColumnHeader MatName;
        private System.Windows.Forms.ColumnHeader MatLast;
        private System.Windows.Forms.ColumnHeader MatDelta;
        private System.Windows.Forms.ColumnHeader MatResult;
        private System.Windows.Forms.ColumnHeader MatCorrection;
    }
}