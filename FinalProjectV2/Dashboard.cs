// File: Dashboard.cs

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FinalProjectV2
{
    public partial class Dashboard : Form
    {
        ScholarshipADO sado = new ScholarshipADO();
        ScholarApplicationADO saado = new ScholarApplicationADO();
        Session sesh = new Session();
        private  List<ScholarApplication> applications;
        private List<Scholarship> scholarships;
        private readonly Color activeBlue = Color.FromArgb(10, 54, 130);
        private readonly Color accentGold = Color.FromArgb(201, 158, 42);
        private readonly Color darkText = Color.FromArgb(26, 36, 56);
        private readonly Color lightText = Color.FromArgb(112, 125, 150);

        public Dashboard(User user)
        {
            sesh.CurrentUser = user;
            scholarships = sado.GetSuitableScholarships(sesh.CurrentUser);
            applications = saado.GetUserApplications(user);
            InitializeComponent();
            LoadScholarshipCards();
            LoadApplicationCards();
            LoadProfileData();
            ShowPage(PageSection.Scholarships);
        }

        private void LoadProfileData()
        {
            TextBoxProfileName.Text = string.IsNullOrWhiteSpace(sesh.CurrentUser.FirstName)
                ? "Juan Dela Cruz"
                : sesh.CurrentUser.FirstName;

            TextBoxProfileBirthday.Text = sesh.CurrentUser.birthdate.ToString("MMM dd yyyy");
            TextBoxProfileEmail.Text = string.IsNullOrWhiteSpace(sesh.CurrentUser.Email)
                ? "juan.delacruz@example.com"
                : sesh.CurrentUser.Email;
            TextBoxProfileContact.Text = string.IsNullOrWhiteSpace(sesh.CurrentUser.ContactNo)
                ? "09123456789"
                : sesh.CurrentUser.ContactNo;
        }
        private void LoadScholarshipCards()
        {
            FlowScholarships.SuspendLayout();
            FlowScholarships.Controls.Clear();

            foreach (Scholarship scholarship in scholarships)
            {
                FlowScholarships.Controls.Add(CreateScholarshipCard(scholarship));
            }

            FlowScholarships.ResumeLayout();
        }

        private void LoadApplicationCards()
        {
            FlowApplications.SuspendLayout();
            FlowApplications.Controls.Clear();

            foreach (ScholarApplication application in applications)
            {
                FlowApplications.Controls.Add(CreateApplicationCard(application));
            }

            FlowApplications.ResumeLayout();
        }

        private Control CreateScholarshipCard(Scholarship scholarship)
        {
            Panel card = new Panel
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 0, 0, 14),
                Size = new Size(540, 146)
            };

            Panel accent = new Panel
            {
                BackColor = accentGold,
                Dock = DockStyle.Left,
                Width = 5
            };

            Label labelName = new Label
            {
                AutoSize = false,
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                ForeColor = darkText,
                Location = new Point(18, 14),
                Size = new Size(300, 26),
                Text = scholarship.Name
            };

            Label labelProvider = new Label
            {
                AutoSize = false,
                Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold),
                ForeColor = accentGold,
                Location = new Point(18, 40),
                Size = new Size(300, 20),
                Text = scholarship.Provider
            };

            Label labelDetails = new Label
            {
                AutoSize = false,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = lightText,
                Location = new Point(18, 65),
                Size = new Size(330, 58),
                Text = scholarship.Description
            };

            Label labelSlots = new Label
            {
                AutoSize = false,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = darkText,
                Location = new Point(355, 18),
                Size = new Size(165, 20),
                Text = "Available Slots: " + scholarship.AvailableSlots
            };

            Label labelDeadline = new Label
            {
                AutoSize = false,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = lightText,
                Location = new Point(355, 45),
                Size = new Size(165, 18),
                Text = "Deadline: " + scholarship.Deadline.ToString("MMMM dd, yyyy")
            };

            Button buttonPreview = new Button
            {
                BackColor = activeBlue,
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(385, 92),
                Size = new Size(125, 34),
                Text = "Preview",
                UseVisualStyleBackColor = false,
                Tag = scholarship
            };
            buttonPreview.FlatAppearance.BorderSize = 0;
            buttonPreview.Click += ButtonPreview_Click;

            card.Controls.Add(buttonPreview);
            card.Controls.Add(labelDeadline);
            card.Controls.Add(labelSlots);
            card.Controls.Add(labelDetails);
            card.Controls.Add(labelProvider);
            card.Controls.Add(labelName);
            card.Controls.Add(accent);

            return card;
        }

        private Control CreateApplicationCard(ScholarApplication application)
        {
            ScholarshipADO sado = new ScholarshipADO();
            Scholarship scholarship = sado.GetScholarshipById(application.ScholarshipId);
           
            Panel card = new Panel
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 0, 0, 14),
                Size = new Size(540, 96)
            };

            Panel accent = new Panel
            {
                BackColor = activeBlue,
                Dock = DockStyle.Left,
                Width = 5
            };

            Label labelName = new Label
            {
                AutoSize = false,
                Font = new Font("Segoe UI Semibold", 12.5F, FontStyle.Bold),
                ForeColor = darkText,
                Location = new Point(18, 14),
                Size = new Size(310, 24),
                Text = scholarship.Name
            };

            Label labelProvider = new Label
            {
                AutoSize = false,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = lightText,
                Location = new Point(18, 39),
                Size = new Size(310, 18),
                Text = "Provider: " + scholarship.Provider
            };

            Panel statusPill = new Panel
            {
                BackColor = GetStatusBackColor(application.Status),
                Location = new Point(390, 30),
                Size = new Size(120, 34)
            };

            Label labelStatus = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = GetStatusForeColor(application.Status),
                TextAlign = ContentAlignment.MiddleCenter,
                Text = application.Status
            };

            statusPill.Controls.Add(labelStatus);

            card.Controls.Add(statusPill);
            card.Controls.Add(labelProvider);
            card.Controls.Add(labelName);
            card.Controls.Add(accent);

            return card;
        }

        private void ButtonPreview_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Scholarship scholarship = button != null ? button.Tag as Scholarship : null;

            if (scholarship == null)
            {
                return;
            }

            using (ScholarshipPreview preview = new ScholarshipPreview())
            {
                preview.SetScholarship(
                    scholarship,
                    sesh.CurrentUser,
                    HandleSubmittedApplication);

                if (preview.ShowDialog(this) == DialogResult.OK)
                {
                    ShowPage(PageSection.Applications);
                }
            }
        }

        private void HandleSubmittedApplication(ScholarApplication result)
        {
            if (result == null)
            {
                return;
            }

            ScholarApplication existing = null;

            foreach (ScholarApplication item in applications)
            {

                if (item.ScholarshipId == result.ScholarshipId)
                {
                    existing = item;
                    break;
                }
            }

            if (existing == null)
            {
                applications.Insert(0, new ScholarApplication
                {
                    ScholarshipId = result.ScholarshipId,
                    Status = result.Status
                });
            }
            else
            {
                existing.Status = result.Status;
            }

            LoadApplicationCards();
        }

        private Color GetStatusBackColor(string status)
        {
            switch (status)
            {
                case "Approved":
                    return Color.FromArgb(225, 244, 232);
                case "Rejected":
                    return Color.FromArgb(251, 232, 232);
                default:
                    return Color.FromArgb(255, 245, 220);
            }
        }

        private Color GetStatusForeColor(string status)
        {
            switch (status)
            {
                case "Approved":
                    return Color.FromArgb(38, 128, 70);
                case "Rejected":
                    return Color.FromArgb(184, 51, 51);
                default:
                    return Color.FromArgb(160, 115, 0);
            }
        }

        private void ShowPage(PageSection page)
        {
            PanelPageDashboard.Visible = false;
            PanelPageApplications.Visible = false;
            PanelPageProfile.Visible = false;

            ResetNavButton(ButtonNavDashboard);
            ResetNavButton(ButtonNavApplications);
            ResetNavButton(ButtonNavProfile);

            if (page == PageSection.Scholarships)
            {
                PanelPageDashboard.Visible = true;
                PanelPageDashboard.BringToFront();

                LabelSectionTag.Text = "SCHOLARSHIP DASHBOARD";
                LabelSectionTitle.Text = "Available scholarships";
                LabelSectionSubtitle.Text = "Browse active scholarship opportunities and review the most important details before you apply.";

                SetActiveNav(ButtonNavDashboard);
            }
            else if (page == PageSection.Applications)
            {
                PanelPageApplications.Visible = true;
                PanelPageApplications.BringToFront();

                LabelSectionTag.Text = "MY APPLICATIONS";
                LabelSectionTitle.Text = "Application status";
                LabelSectionSubtitle.Text = "Monitor whether your submitted scholarship applications are pending, approved, or rejected.";

                SetActiveNav(ButtonNavApplications);
            }
            else
            {
                PanelPageProfile.Visible = true;
                PanelPageProfile.BringToFront();

                LabelSectionTag.Text = "PROFILE SETTINGS";
                LabelSectionTitle.Text = "Your scholar profile";
                LabelSectionSubtitle.Text = "Only your email address and contact number can be updated. Your personal details remain visible but locked.";

                SetActiveNav(ButtonNavProfile);
            }
        }

        private void SetActiveNav(Button activeButton)
        {
            activeButton.BackColor = activeBlue;
            activeButton.ForeColor = Color.White;
        }

        private void ResetNavButton(Button button)
        {
            button.BackColor = Color.White;
            button.ForeColor = darkText;
        }

        private void ButtonNavDashboard_Click(object sender, EventArgs e)
        {
            ShowPage(PageSection.Scholarships);
        }

        private void ButtonNavApplications_Click(object sender, EventArgs e)
        {
            ShowPage(PageSection.Applications);
        }

        private void ButtonNavProfile_Click(object sender, EventArgs e)
        {
            ShowPage(PageSection.Profile);
        }

        private void ButtonNavSignOut_Click(object sender, EventArgs e)
        {
            new Login().Show();
            Close();
        }

        private void ButtonSaveProfile_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBoxProfileEmail.Text))
            {
                MessageBox.Show("Please enter your email address.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBoxProfileEmail.Focus();
                return;
            }

            if (!TextBoxProfileEmail.Text.Contains("@"))
            {
                MessageBox.Show("Please enter a valid email address.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBoxProfileEmail.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(TextBoxProfileContact.Text))
            {
                MessageBox.Show("Please enter your contact number.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBoxProfileContact.Focus();
                return;
            }

            sesh.CurrentUser.Email = TextBoxProfileEmail.Text.Trim();
            sesh.CurrentUser.ContactNo = TextBoxProfileContact.Text.Trim();

            MessageBox.Show("Profile settings updated successfully.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private enum PageSection
        {
            Scholarships,
            Applications,
            Profile
        }
    }
}