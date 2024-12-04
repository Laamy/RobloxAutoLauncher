namespace RobloxAutoLauncher
{
    partial class InstallerWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InstallerWindow));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.smoothButton1 = new RobloxAutoLauncher.GameUtils.SmoothButton();
            this.processBarControl1 = new RobloxAutoLauncher.GameUtils.ProcessBarControl();
            this.smoothButton2 = new RobloxAutoLauncher.GameUtils.SmoothButton();
            this.smoothButton3 = new RobloxAutoLauncher.GameUtils.SmoothButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(65, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(120, 120);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // smoothButton1
            // 
            this.smoothButton1.BackColor = System.Drawing.Color.Transparent;
            this.smoothButton1.BorderColor = System.Drawing.Color.Black;
            this.smoothButton1.ButtonText = "Install";
            this.smoothButton1.ForeColor = System.Drawing.SystemColors.Control;
            this.smoothButton1.HoverColor = System.Drawing.Color.LightBlue;
            this.smoothButton1.Location = new System.Drawing.Point(17, 162);
            this.smoothButton1.Name = "smoothButton1";
            this.smoothButton1.PressColor = System.Drawing.Color.DarkBlue;
            this.smoothButton1.Size = new System.Drawing.Size(70, 23);
            this.smoothButton1.TabIndex = 6;
            this.smoothButton1.Click += new System.EventHandler(this.InstallApp);
            // 
            // processBarControl1
            // 
            this.processBarControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.processBarControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.processBarControl1.Location = new System.Drawing.Point(17, 141);
            this.processBarControl1.Name = "processBarControl1";
            this.processBarControl1.Size = new System.Drawing.Size(222, 15);
            this.processBarControl1.TabIndex = 5;
            // 
            // smoothButton2
            // 
            this.smoothButton2.BackColor = System.Drawing.Color.Transparent;
            this.smoothButton2.BorderColor = System.Drawing.Color.Black;
            this.smoothButton2.ButtonText = "Repair";
            this.smoothButton2.ForeColor = System.Drawing.SystemColors.Control;
            this.smoothButton2.HoverColor = System.Drawing.Color.LightBlue;
            this.smoothButton2.Location = new System.Drawing.Point(93, 162);
            this.smoothButton2.Name = "smoothButton2";
            this.smoothButton2.PressColor = System.Drawing.Color.DarkBlue;
            this.smoothButton2.Size = new System.Drawing.Size(70, 23);
            this.smoothButton2.TabIndex = 7;
            this.smoothButton2.Click += new System.EventHandler(this.RepairApp);
            // 
            // smoothButton3
            // 
            this.smoothButton3.BackColor = System.Drawing.Color.Transparent;
            this.smoothButton3.BorderColor = System.Drawing.Color.Black;
            this.smoothButton3.ButtonText = "Uninstall";
            this.smoothButton3.ForeColor = System.Drawing.SystemColors.Control;
            this.smoothButton3.HoverColor = System.Drawing.Color.LightBlue;
            this.smoothButton3.Location = new System.Drawing.Point(169, 162);
            this.smoothButton3.Name = "smoothButton3";
            this.smoothButton3.PressColor = System.Drawing.Color.DarkBlue;
            this.smoothButton3.Size = new System.Drawing.Size(70, 23);
            this.smoothButton3.TabIndex = 8;
            this.smoothButton3.Click += new System.EventHandler(this.UninstallApp);
            // 
            // InstallerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(258, 195);
            this.Controls.Add(this.smoothButton3);
            this.Controls.Add(this.smoothButton2);
            this.Controls.Add(this.smoothButton1);
            this.Controls.Add(this.processBarControl1);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InstallerWindow";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "RobloxAL Installer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.InstallerWindow_FormClosing);
            this.Load += new System.EventHandler(this.InstallerWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private GameUtils.ProcessBarControl processBarControl1;
        private GameUtils.SmoothButton smoothButton1;
        private GameUtils.SmoothButton smoothButton2;
        private GameUtils.SmoothButton smoothButton3;
    }
}
