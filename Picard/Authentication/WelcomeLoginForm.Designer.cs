namespace Picard.Authentication
{
    partial class WelcomeLoginForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.LoginFormPanel = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.usernameBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.LoggingInPanel = new System.Windows.Forms.Panel();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.LoginFormPanel.SuspendLayout();
            this.LoggingInPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(461, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please log in to your Inara.cz account.  This information will be saved in plaint" +
    "ext on your system.";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.LoginFormPanel);
            this.groupBox1.Controls.Add(this.LoggingInPanel);
            this.groupBox1.Location = new System.Drawing.Point(142, 91);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 144);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Inara.cz Login";
            // 
            // LoginFormPanel
            // 
            this.LoginFormPanel.Controls.Add(this.button1);
            this.LoginFormPanel.Controls.Add(this.passwordBox);
            this.LoginFormPanel.Controls.Add(this.usernameBox);
            this.LoginFormPanel.Controls.Add(this.label3);
            this.LoginFormPanel.Controls.Add(this.label2);
            this.LoginFormPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LoginFormPanel.Location = new System.Drawing.Point(3, 16);
            this.LoginFormPanel.Name = "LoginFormPanel";
            this.LoginFormPanel.Size = new System.Drawing.Size(194, 125);
            this.LoginFormPanel.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(46, 88);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Log In and Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // passwordBox
            // 
            this.passwordBox.Location = new System.Drawing.Point(78, 49);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.Size = new System.Drawing.Size(100, 20);
            this.passwordBox.TabIndex = 8;
            this.passwordBox.UseSystemPasswordChar = true;
            // 
            // usernameBox
            // 
            this.usernameBox.Location = new System.Drawing.Point(78, 13);
            this.usernameBox.Name = "usernameBox";
            this.usernameBox.Size = new System.Drawing.Size(100, 20);
            this.usernameBox.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Password";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Username";
            // 
            // LoggingInPanel
            // 
            this.LoggingInPanel.Controls.Add(this.StatusLabel);
            this.LoggingInPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LoggingInPanel.Location = new System.Drawing.Point(3, 16);
            this.LoggingInPanel.Name = "LoggingInPanel";
            this.LoggingInPanel.Size = new System.Drawing.Size(194, 125);
            this.LoggingInPanel.TabIndex = 1;
            this.LoggingInPanel.Visible = false;
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Location = new System.Drawing.Point(66, 52);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(52, 13);
            this.StatusLabel.TabIndex = 0;
            this.StatusLabel.Text = "Waiting...";
            // 
            // errorLabel
            // 
            this.errorLabel.ForeColor = System.Drawing.Color.Red;
            this.errorLabel.Location = new System.Drawing.Point(25, 270);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(458, 20);
            this.errorLabel.TabIndex = 2;
            this.errorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WelcomeLoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 325);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Name = "WelcomeLoginForm";
            this.Text = "Picard - Elite: Dangerous Log Loader";
            this.Load += new System.EventHandler(this.WelcomeLoginForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.LoginFormPanel.ResumeLayout(false);
            this.LoginFormPanel.PerformLayout();
            this.LoggingInPanel.ResumeLayout(false);
            this.LoggingInPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel LoggingInPanel;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.Panel LoginFormPanel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox passwordBox;
        private System.Windows.Forms.TextBox usernameBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label errorLabel;
    }
}