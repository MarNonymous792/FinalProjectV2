// Dashboard.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace FinalProjectV2
{
    public partial class Dashboard : Form
    {
        private readonly ScholarshipADO sado = new ScholarshipADO();
        private readonly ScholarApplicationADO saado = new ScholarApplicationADO();
        private readonly Session sesh = new Session();

        private List<ScholarApplication> applications;
        private List<Scholarship> scholarships;

        private readonly Color activeBlue = Color.FromArgb(10, 54, 130);
        private readonly Color accentGold = Color.FromArgb(201, 158, 42);
        private readonly Color darkText = Color.FromArgb(26, 36, 56);
        private readonly Color lightText = Color.FromArgb(112, 125, 150);

        private readonly Dictionary<string, Label> lockedProfileValueLabels = new Dictionary<string, Label>();

        public Dashboard(User user)
        {
            sesh.CurrentUser = user;
            scholarships = sado.GetSuitableScholarships(sesh.CurrentUser);
            applications = saado.GetUserApplications(user);

            InitializeComponent();
            InitializeProfileLayout();
            LoadScholarshipCards();
            LoadApplicationCards();
            LoadProfileData();
            ShowPage(PageSection.Scholarships);
        }

        private void InitializeProfileLayout()
        {
            PanelPageProfile.SuspendLayout();

            LabelProfileName.Visible = false;
            PanelProfileName.Visible = false;
            LabelProfileBirthday.Visible = false;
            PanelProfileBirthday.Visible = false;

            CreateLockedProfileField(
                "StudentId",
                "Student ID",
                new Point(15, 0),
                new Point(15, 24),
                new Size(541, 42));

            CreateLockedProfileField(
                "LastName",
                "Lastname",
                new Point(15, 81),
                new Point(15, 105),
                new Size(170, 42));

            CreateLockedProfileField(
                "FirstName",
                "Firstname",
                new Point(200, 81),
                new Point(200, 105),
                new Size(170, 42));

            CreateLockedProfileField(
                "MiddleName",
                "Middlename",
                new Point(385, 81),
                new Point(385, 105),
                new Size(171, 42));

            CreateLockedProfileField(
                "Birthday",
                "Birthday",
                new Point(15, 162),
                new Point(15, 186),
                new Size(255, 42));

            TextBoxProfileEmail.TabIndex = 1;
            TextBoxProfileContact.TabIndex = 2;
            ButtonSaveProfile.TabIndex = 3;

            PanelPageProfile.ResumeLayout(false);
            PanelPageProfile.PerformLayout();
        }

        private void CreateLockedProfileField(string key, string title, Point labelLocation, Point panelLocation, Size panelSize)
        {
            Label titleLabel = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = darkText,
                Location = labelLocation,
                Text = title
            };

            Panel fieldPanel = new Panel
            {
                BackColor = Color.FromArgb(240, 242, 246),
                Location = panelLocation,
                Size = panelSize,
                Cursor = Cursors.Default
            };

            Panel accentPanel = new Panel
            {
                BackColor = accentGold,
                Dock = DockStyle.Left,
                Width = 5
            };

            Label valueLabel = new Label
            {
                AutoEllipsis = true,
                BackColor = Color.FromArgb(240, 242, 246),
                Cursor = Cursors.Default,
                Font = new Font("Segoe UI", 10.75F),
                ForeColor = darkText,
                Location = new Point(16, 10),
                Size = new Size(panelSize.Width - 32, 24),
                Text = string.Empty,
                TextAlign = ContentAlignment.MiddleLeft
            };

            fieldPanel.Controls.Add(valueLabel);
            fieldPanel.Controls.Add(accentPanel);

            PanelPageProfile.Controls.Add(titleLabel);
            PanelPageProfile.Controls.Add(fieldPanel);

            titleLabel.BringToFront();
            fieldPanel.BringToFront();

            lockedProfileValueLabels[key] = valueLabel;
        }

        private void LoadProfileData()
        {
            SetLockedProfileFieldValue(
                "StudentId",
                GetUserStringValue(
                    "N/A",
                    "StudentId",
                    "StudentID",
                    "StudentNo",
                    "StudentNumber",
                    "IdNumber",
                    "IdNo",
                    "ScholarId"));

            SetLockedProfileFieldValue(
                "LastName",
                GetUserStringValue(
                    "N/A",
                    "LastName",
                    "Lastname",
                    "Surname",
                    "FamilyName"));

            SetLockedProfileFieldValue(
                "FirstName",
                GetUserStringValue(
                    "Juan",
                    "FirstName",
                    "Firstname",
                    "GivenName"));

            SetLockedProfileFieldValue(
                "MiddleName",
                GetUserStringValue(
                    "N/A",
                    "MiddleName",
                    "Middlename",
                    "MiddleInitial"));

            SetLockedProfileFieldValue(
                "Birthday",
                GetUserDateValue(
                    "MMM dd yyyy",
                    "N/A",
                    "birthdate",
                    "Birthdate",
                    "Birthday",
                    "DateOfBirth"));

            TextBoxProfileEmail.Text = string.IsNullOrWhiteSpace(sesh.CurrentUser.Email)
                ? "juan.delacruz@example.com"
                : sesh.CurrentUser.Email;

            TextBoxProfileContact.Text = string.IsNullOrWhiteSpace(sesh.CurrentUser.ContactNo)
                ? "09123456789"
                : sesh.CurrentUser.ContactNo;
        }

        private void SetLockedProfileFieldValue(string key, string value)
        {
            if (lockedProfileValueLabels.TryGetValue(key, out Label label))
            {
                label.Text = value;
            }
        }

        private object GetUserPropertyValue(params string[] propertyNames)
        {
            if (sesh.CurrentUser == null)
            {
                return null;
            }

            Type userType = sesh.CurrentUser.GetType();

            foreach (string propertyName in propertyNames)
            {
                PropertyInfo property = userType.GetProperty(
                    propertyName,
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                if (property != null)
                {
                    return property.GetValue(sesh.CurrentUser);
                }
            }

            return null;
        }

        private string GetUserStringValue(string fallback, params string[] propertyNames)
        {
            object value = GetUserPropertyValue(propertyNames);

            if (value == null)
            {
                return fallback;
            }

            string text = value.ToString()?.Trim();

            return string.IsNullOrWhiteSpace(text) ? fallback : text;
        }

        private string GetUserDateValue(string format, string fallback, params string[] propertyNames)
        {
            object value = GetUserPropertyValue(propertyNames);

            if (value == null)
            {
                return fallback;
            }

            if (value is DateTime dateTimeValue)
            {
                return dateTimeValue.ToString(format);
            }

            if (DateTime.TryParse(value.ToString(), out DateTime parsedDate))
            {
                return parsedDate.ToString(format);
            }

            return fallback;
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
            Scholarship scholarship = sado.GetScholarshipById(application.ScholarshipId);

            Panel card = new Panel
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 0, 0, 14),
                Size = new Size(540, 110),
                Cursor = Cursors.Hand,
                Tag = application
            };

            Panel accent = new Panel
            {
                BackColor = activeBlue,
                Dock = DockStyle.Left,
                Width = 5,
                Tag = application
            };

            Label labelName = new Label
            {
                AutoSize = false,
                Font = new Font("Segoe UI Semibold", 12.5F, FontStyle.Bold),
                ForeColor = darkText,
                Location = new Point(18, 14),
                Size = new Size(310, 24),
                Text = scholarship != null ? scholarship.Name : "Scholarship",
                Cursor = Cursors.Hand,
                Tag = application
            };

            Label labelProvider = new Label
            {
                AutoSize = false,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = lightText,
                Location = new Point(18, 39),
                Size = new Size(310, 18),
                Text = scholarship != null ? "Provider: " + scholarship.Provider : "Provider: N/A",
                Cursor = Cursors.Hand,
                Tag = application
            };

            Label labelHint = new Label
            {
                AutoSize = false,
                Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                ForeColor = lightText,
                Location = new Point(18, 65),
                Size = new Size(280, 18),
                Text = "Click card to preview application",
                Cursor = Cursors.Hand,
                Tag = application
            };

            Panel statusPill = new Panel
            {
                BackColor = GetStatusBackColor(application.Status),
                Location = new Point(390, 22),
                Size = new Size(120, 34),
                Cursor = Cursors.Hand,
                Tag = application
            };

            Label labelStatus = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = GetStatusForeColor(application.Status),
                TextAlign = ContentAlignment.MiddleCenter,
                Text = application.Status,
                Cursor = Cursors.Hand,
                Tag = application
            };

            statusPill.Controls.Add(labelStatus);

            card.Controls.Add(statusPill);
            card.Controls.Add(labelHint);
            card.Controls.Add(labelProvider);
            card.Controls.Add(labelName);
            card.Controls.Add(accent);

            BindApplicationCardClick(card, application);

            return card;
        }

        private void BindApplicationCardClick(Control control, ScholarApplication application)
        {
            control.Tag = application;
            control.Click += ApplicationCard_Click;

            foreach (Control child in control.Controls)
            {
                BindApplicationCardClick(child, application);
            }
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
                preview.SetScholarship(scholarship, sesh.CurrentUser, HandleSubmittedApplication);

                if (preview.ShowDialog(this) == DialogResult.OK)
                {
                    ShowPage(PageSection.Applications);
                }
            }
        }

        private void ApplicationCard_Click(object sender, EventArgs e)
        {
            Control clickedControl = sender as Control;
            ScholarApplication application = clickedControl != null ? clickedControl.Tag as ScholarApplication : null;

            if (application == null)
            {
                return;
            }

            Scholarship scholarship = sado.GetScholarshipById(application.ScholarshipId);

            if (scholarship == null)
            {
                MessageBox.Show(
                    "Scholarship details could not be loaded.",
                    "SchoolarLink",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            using (ScholarshipPreview preview = new ScholarshipPreview())
            {
                preview.SetApplicationPreview(
                    scholarship,
                    application,
                    sesh.CurrentUser,
                    HandleAcceptedApplication);

                preview.ShowDialog(this);
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

        private void HandleAcceptedApplication(ScholarApplication result)
        {
            if (result == null)
            {
                return;
            }

            foreach (ScholarApplication item in applications)
            {
                if (item.ScholarshipId == result.ScholarshipId)
                {
                    item.Status = result.Status;
                    break;
                }
            }

            LoadApplicationCards();
            ShowPage(PageSection.Applications);
        }

        private Color GetStatusBackColor(string status)
        {
            switch (status)
            {
                case "Approved":
                    return Color.FromArgb(225, 244, 232);
                case "Rejected":
                    return Color.FromArgb(251, 232, 232);
                case "Accepted":
                    return Color.FromArgb(227, 239, 255);
                case "Cancelled":
                    return Color.FromArgb(242, 242, 242);
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
                case "Accepted":
                    return Color.FromArgb(10, 54, 130);
                case "Cancelled":
                    return Color.FromArgb(110, 110, 110);
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
                LabelSectionSubtitle.Text = "Monitor your submitted applications and open any card to preview its scholarship details and attached requirements.";

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