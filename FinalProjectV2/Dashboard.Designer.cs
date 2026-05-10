// File: Dashboard.Designer.cs

namespace FinalProjectV2
{
    partial class Dashboard
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.sidebarPanel = new System.Windows.Forms.Panel();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnApplications = new System.Windows.Forms.Button();
            this.btnDashboard = new System.Windows.Forms.Button();
            this.lblLogo = new System.Windows.Forms.Label();
            this.topPanel = new System.Windows.Forms.Panel();
            this.lblProfile = new System.Windows.Forms.Label();
            this.lblNotification = new System.Windows.Forms.Label();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.scholarshipsContainer = new System.Windows.Forms.Panel();
            this.lblScholarships = new System.Windows.Forms.Label();
            this.scholarshipsPanel = new System.Windows.Forms.Panel();
            this.sidebarPanel.SuspendLayout();
            this.topPanel.SuspendLayout();
            this.scholarshipsContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // sidebarPanel
            // 
            this.sidebarPanel.BackColor = System.Drawing.Color.White;
            this.sidebarPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sidebarPanel.Controls.Add(this.btnSettings);
            this.sidebarPanel.Controls.Add(this.btnApplications);
            this.sidebarPanel.Controls.Add(this.btnDashboard);
            this.sidebarPanel.Controls.Add(this.lblLogo);
            this.sidebarPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidebarPanel.Location = new System.Drawing.Point(0, 0);
            this.sidebarPanel.Name = "sidebarPanel";
            this.sidebarPanel.Size = new System.Drawing.Size(274, 693);
            this.sidebarPanel.TabIndex = 4;
            // 
            // btnSettings
            // 
            this.btnSettings.BackColor = System.Drawing.Color.White;
            this.btnSettings.FlatAppearance.BorderSize = 0;
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettings.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnSettings.ForeColor = System.Drawing.Color.Black;
            this.btnSettings.Location = new System.Drawing.Point(17, 213);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Padding = new System.Windows.Forms.Padding(11, 0, 0, 0);
            this.btnSettings.Size = new System.Drawing.Size(234, 48);
            this.btnSettings.TabIndex = 0;
            this.btnSettings.Text = "⚙ Settings";
            this.btnSettings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSettings.UseVisualStyleBackColor = false;
            // 
            // btnApplications
            // 
            this.btnApplications.BackColor = System.Drawing.Color.White;
            this.btnApplications.FlatAppearance.BorderSize = 0;
            this.btnApplications.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApplications.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnApplications.ForeColor = System.Drawing.Color.Black;
            this.btnApplications.Location = new System.Drawing.Point(17, 149);
            this.btnApplications.Name = "btnApplications";
            this.btnApplications.Padding = new System.Windows.Forms.Padding(11, 0, 0, 0);
            this.btnApplications.Size = new System.Drawing.Size(234, 48);
            this.btnApplications.TabIndex = 1;
            this.btnApplications.Text = "📄 Applications";
            this.btnApplications.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnApplications.UseVisualStyleBackColor = false;
            // 
            // btnDashboard
            // 
            this.btnDashboard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(82)))), ((int)(((byte)(165)))));
            this.btnDashboard.FlatAppearance.BorderSize = 0;
            this.btnDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDashboard.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnDashboard.ForeColor = System.Drawing.Color.White;
            this.btnDashboard.Location = new System.Drawing.Point(17, 85);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Padding = new System.Windows.Forms.Padding(11, 0, 0, 0);
            this.btnDashboard.Size = new System.Drawing.Size(234, 48);
            this.btnDashboard.TabIndex = 2;
            this.btnDashboard.Text = "🏠 Dashboard";
            this.btnDashboard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDashboard.UseVisualStyleBackColor = false;
            // 
            // lblLogo
            // 
            this.lblLogo.AutoSize = true;
            this.lblLogo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblLogo.ForeColor = System.Drawing.Color.Navy;
            this.lblLogo.Location = new System.Drawing.Point(23, 27);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new System.Drawing.Size(186, 32);
            this.lblLogo.TabIndex = 3;
            this.lblLogo.Text = "🎓 ScholarLink";
            // 
            // topPanel
            // 
            this.topPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(45)))), ((int)(((byte)(120)))));
            this.topPanel.Controls.Add(this.lblProfile);
            this.topPanel.Controls.Add(this.lblNotification);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(274, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(812, 69);
            this.topPanel.TabIndex = 3;
            // 
            // lblProfile
            // 
            this.lblProfile.AutoSize = true;
            this.lblProfile.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblProfile.ForeColor = System.Drawing.Color.White;
            this.lblProfile.Location = new System.Drawing.Point(754, 21);
            this.lblProfile.Name = "lblProfile";
            this.lblProfile.Size = new System.Drawing.Size(39, 28);
            this.lblProfile.TabIndex = 0;
            this.lblProfile.Text = "👤";
            // 
            // lblNotification
            // 
            this.lblNotification.AutoSize = true;
            this.lblNotification.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblNotification.ForeColor = System.Drawing.Color.White;
            this.lblNotification.Location = new System.Drawing.Point(697, 21);
            this.lblNotification.Name = "lblNotification";
            this.lblNotification.Size = new System.Drawing.Size(39, 28);
            this.lblNotification.TabIndex = 1;
            this.lblNotification.Text = "🔔";
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 22F);
            this.lblWelcome.Location = new System.Drawing.Point(301, 99);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(359, 50);
            this.lblWelcome.TabIndex = 2;
            this.lblWelcome.Text = "Welcome Username!";
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.DimGray;
            this.lblSubtitle.Location = new System.Drawing.Point(305, 149);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(375, 25);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "Here\'s the upcoming scholarships available";
            // 
            // scholarshipsContainer
            // 
            this.scholarshipsContainer.BackColor = System.Drawing.Color.WhiteSmoke;
            this.scholarshipsContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.scholarshipsContainer.Controls.Add(this.lblScholarships);
            this.scholarshipsContainer.Controls.Add(this.scholarshipsPanel);
            this.scholarshipsContainer.Location = new System.Drawing.Point(303, 187);
            this.scholarshipsContainer.Name = "scholarshipsContainer";
            this.scholarshipsContainer.Size = new System.Drawing.Size(743, 469);
            this.scholarshipsContainer.TabIndex = 0;
            // 
            // lblScholarships
            // 
            this.lblScholarships.AutoSize = true;
            this.lblScholarships.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.lblScholarships.Location = new System.Drawing.Point(17, 16);
            this.lblScholarships.Name = "lblScholarships";
            this.lblScholarships.Size = new System.Drawing.Size(164, 37);
            this.lblScholarships.TabIndex = 0;
            this.lblScholarships.Text = "Scholarships";
            // 
            // scholarshipsPanel
            // 
            this.scholarshipsPanel.AutoScroll = true;
            this.scholarshipsPanel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.scholarshipsPanel.Location = new System.Drawing.Point(11, 64);
            this.scholarshipsPanel.Name = "scholarshipsPanel";
            this.scholarshipsPanel.Size = new System.Drawing.Size(714, 384);
            this.scholarshipsPanel.TabIndex = 1;
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1086, 693);
            this.Controls.Add(this.scholarshipsContainer);
            this.Controls.Add(this.lblSubtitle);
            this.Controls.Add(this.lblWelcome);
            this.Controls.Add(this.topPanel);
            this.Controls.Add(this.sidebarPanel);
            this.Name = "Dashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dashboard";
            this.sidebarPanel.ResumeLayout(false);
            this.sidebarPanel.PerformLayout();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.scholarshipsContainer.ResumeLayout(false);
            this.scholarshipsContainer.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel sidebarPanel;

        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Button btnApplications;
        private System.Windows.Forms.Button btnSettings;

        private System.Windows.Forms.Label lblLogo;

        private System.Windows.Forms.Panel topPanel;

        private System.Windows.Forms.Label lblNotification;
        private System.Windows.Forms.Label lblProfile;

        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Label lblSubtitle;

        private System.Windows.Forms.Panel scholarshipsContainer;
        private System.Windows.Forms.Label lblScholarships;
        private System.Windows.Forms.Panel scholarshipsPanel;
    }
}