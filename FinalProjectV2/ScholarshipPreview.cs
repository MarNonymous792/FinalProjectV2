// File: ScholarshipPreview.cs

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FinalProjectV2
{
    public partial class ScholarshipPreview : Form
    {
        public Scholarship scholarship;

        private string currentUserId;
        private Action<ScholarApplication> submitHandler;
        private ScholarApplication currentApplication;
        private Action<ScholarApplication> acceptHandler;
        private bool isApplicationPreviewMode;

        public ScholarshipPreview()
        {
            InitializeComponent();

            PanelPreviewCard.AutoScroll = false;

            RichTextBoxDescription.ReadOnly = true;
            RichTextBoxDescription.BorderStyle = BorderStyle.None;
            RichTextBoxDescription.ScrollBars = RichTextBoxScrollBars.Vertical;
            RichTextBoxDescription.WordWrap = true;
            RichTextBoxDescription.ShortcutsEnabled = false;
            RichTextBoxDescription.DetectUrls = false;
            RichTextBoxDescription.BackColor = Color.FromArgb(240, 242, 246);

            ListBoxRequirements.IntegralHeight = false;
            ListBoxRequirements.HorizontalScrollbar = true;

            ConfigureActionButtonsAppearance();
        }

        public void SetScholarship(
            Scholarship scholarshipInfo,
            User user,
            Action<ScholarApplication> onSubmit)
        {
            scholarship = scholarshipInfo;
            currentUserId = user != null ? user.UserID : string.Empty;
            submitHandler = onSubmit;
            currentApplication = null;
            acceptHandler = null;
            isApplicationPreviewMode = false;

            PopulateScholarshipDetails();
            ConfigureScholarshipMode();
            ApplyPerfectFixedLayout();
        }

        public void SetApplicationPreview(
            Scholarship scholarshipInfo,
            ScholarApplication applicationInfo,
            User user,
            Action<ScholarApplication> onAccepted)
        {
            scholarship = scholarshipInfo;
            currentApplication = applicationInfo;
            currentUserId = user != null ? user.UserID : string.Empty;
            submitHandler = null;
            acceptHandler = onAccepted;
            isApplicationPreviewMode = true;

            PopulateScholarshipDetails();
            LoadRequirementsPreview(applicationInfo);
            ConfigureApplicationMode();
            ApplyPerfectFixedLayout();
        }

        private void PopulateScholarshipDetails()
        {
            if (scholarship == null)
            {
                LabelScholarshipNameValue.Text = "-";
                LabelProviderValue.Text = "-";
                RichTextBoxDescription.Text = string.Empty;
                LabelAvailableSlotsValue.Text = "-";
                LabelDeadlineValue.Text = "-";
                LabelRequiredCourseValue.Text = "-";
                LabelYearLevelValue.Text = "-";
                return;
            }

            LabelScholarshipNameValue.Text = scholarship.Name;
            LabelProviderValue.Text = scholarship.Provider;
            RichTextBoxDescription.Text = scholarship.Description ?? string.Empty;
            LabelAvailableSlotsValue.Text = scholarship.AvailableSlots.ToString();
            LabelDeadlineValue.Text = scholarship.Deadline.ToString("MMMM dd, yyyy");
            LabelRequiredCourseValue.Text = scholarship.RequiredCourse;
            LabelYearLevelValue.Text = scholarship.MaxYearLevel.ToString();

            ResetDescriptionToTop();
        }

        private void ConfigureScholarshipMode()
        {
            LabelPortal.Text = "SCHOLARSHIP PREVIEW";
            LabelTitle.Text = "Scholarship preview";
            LabelSubtitle.Text = "Review the scholarship details before continuing to the application requirements form.";
            LabelSidebarHint.Text = "Review the scholarship details first before continuing to the requirements form.";

            LabelRequirements.Visible = false;
            PanelRequirements.Visible = false;

            ButtonApply.Visible = true;
            ButtonApply.Enabled = scholarship != null;
            ButtonApply.Text = "Apply";

            ButtonAccept.Visible = false;
            ButtonAccept.Enabled = false;

            ButtonCancelApplication.Visible = false;
            ButtonCancelApplication.Enabled = false;
        }

        private void ConfigureApplicationMode()
        {
            string status = currentApplication != null && !string.IsNullOrWhiteSpace(currentApplication.Status)
                ? currentApplication.Status
                : "Unknown";

            bool isPending = IsPendingStatus(status);
            bool isApproved = IsApprovedStatus(status);

            LabelPortal.Text = "APPLICATION PREVIEW";
            LabelTitle.Text = "Application preview";
            LabelSubtitle.Text = "Review the scholarship details and the requirements you attached. Status: " + status + ".";

            if (isPending)
            {
                LabelSidebarHint.Text = "This view is read-only. You can still cancel the application while it is pending.";
            }
            else if (isApproved)
            {
                LabelSidebarHint.Text = "This view is read-only. The application is approved, so you may now accept it.";
            }
            else
            {
                LabelSidebarHint.Text = "This view is read-only.";
            }

            LabelRequirements.Visible = true;
            PanelRequirements.Visible = true;

            ButtonApply.Visible = false;
            ButtonApply.Enabled = false;

            ButtonAccept.Visible = isApproved;
            ButtonAccept.Enabled = isApproved;
            ButtonAccept.Text = "Accept";

            ButtonCancelApplication.Visible = isPending;
            ButtonCancelApplication.Enabled = isPending;
            ButtonCancelApplication.Text = "Cancel Application";
        }

        private void ConfigureActionButtonsAppearance()
        {
            StyleBackButton(ButtonBack);
            StylePrimaryButton(ButtonApply);
            StylePrimaryButton(ButtonAccept);
            StyleDangerButton(ButtonCancelApplication);
        }

        private void StyleBackButton(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = Color.FromArgb(201, 158, 42);
            button.BackColor = Color.White;
            button.ForeColor = Color.FromArgb(26, 36, 56);
            button.Font = new Font("Segoe UI Semibold", 10.75F, FontStyle.Bold);
            button.Cursor = Cursors.Hand;
            button.TextAlign = ContentAlignment.MiddleCenter;
        }

        private void StylePrimaryButton(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = Color.FromArgb(10, 54, 130);
            button.ForeColor = Color.White;
            button.Font = new Font("Segoe UI Semibold", 10.75F, FontStyle.Bold);
            button.Cursor = Cursors.Hand;
            button.TextAlign = ContentAlignment.MiddleCenter;
        }

        private void StyleDangerButton(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = Color.FromArgb(184, 51, 51);
            button.ForeColor = Color.White;
            button.Font = new Font("Segoe UI Semibold", 10.75F, FontStyle.Bold);
            button.Cursor = Cursors.Hand;
            button.TextAlign = ContentAlignment.MiddleCenter;
        }

        private void ApplyPerfectFixedLayout()
        {
            SuspendLayout();
            PanelPreviewCard.SuspendLayout();

            PanelPreviewCard.AutoScroll = false;
            PanelPreviewCard.Location = new Point(28, 12);
            PanelPreviewCard.Size = new Size(654, 566);

            LabelPortal.Location = new Point(42, 14);

            LabelTitle.Location = new Point(42, 38);
            LabelTitle.AutoSize = true;

            LabelSubtitle.Location = new Point(42, 86);
            LabelSubtitle.Size = new Size(548, 24);

            LabelScholarshipName.Location = new Point(43, 114);
            PanelScholarshipName.Location = new Point(43, 136);
            PanelScholarshipName.Size = new Size(272, 34);

            LabelProvider.Location = new Point(320, 114);
            PanelProvider.Location = new Point(320, 136);
            PanelProvider.Size = new Size(272, 34);

            if (isApplicationPreviewMode)
            {
                LabelDescription.Location = new Point(43, 176);
                PanelDescription.Location = new Point(43, 198);
                PanelDescription.Size = new Size(549, 92);

                RichTextBoxDescription.Location = new Point(14, 8);
                RichTextBoxDescription.Size = new Size(521, 76);

                LabelRequirements.Location = new Point(43, 298);
                PanelRequirements.Location = new Point(43, 320);
                PanelRequirements.Size = new Size(549, 48);

                ListBoxRequirements.Location = new Point(14, 8);
                ListBoxRequirements.Size = new Size(521, 30);

                LabelAvailableSlots.Location = new Point(43, 380);
                PanelAvailableSlots.Location = new Point(43, 402);
                PanelAvailableSlots.Size = new Size(272, 34);

                LabelDeadline.Location = new Point(320, 380);
                PanelDeadline.Location = new Point(320, 402);
                PanelDeadline.Size = new Size(272, 34);

                LabelRequiredCourse.Location = new Point(43, 442);
                PanelRequiredCourse.Location = new Point(43, 464);
                PanelRequiredCourse.Size = new Size(272, 34);

                LabelYearLevel.Location = new Point(320, 442);
                PanelYearLevel.Location = new Point(320, 464);
                PanelYearLevel.Size = new Size(272, 34);

                ButtonBack.Location = new Point(43, 514);
                ButtonBack.Size = new Size(110, 36);

                ButtonCancelApplication.Size = new Size(156, 36);
                ButtonAccept.Size = new Size(156, 36);
                ButtonApply.Size = new Size(136, 36);

                if (ButtonCancelApplication.Visible)
                {
                    ButtonCancelApplication.Location = new Point(436, 514);
                }

                if (ButtonAccept.Visible)
                {
                    ButtonAccept.Location = new Point(436, 514);
                }

                ButtonApply.Location = new Point(456, 514);

                LabelRequirements.Visible = true;
                PanelRequirements.Visible = true;
            }
            else
            {
                LabelDescription.Location = new Point(43, 182);
                PanelDescription.Location = new Point(43, 204);
                PanelDescription.Size = new Size(549, 144);

                RichTextBoxDescription.Location = new Point(14, 8);
                RichTextBoxDescription.Size = new Size(521, 128);

                LabelRequirements.Visible = false;
                PanelRequirements.Visible = false;

                LabelAvailableSlots.Location = new Point(43, 364);
                PanelAvailableSlots.Location = new Point(43, 386);
                PanelAvailableSlots.Size = new Size(272, 34);

                LabelDeadline.Location = new Point(320, 364);
                PanelDeadline.Location = new Point(320, 386);
                PanelDeadline.Size = new Size(272, 34);

                LabelRequiredCourse.Location = new Point(43, 426);
                PanelRequiredCourse.Location = new Point(43, 448);
                PanelRequiredCourse.Size = new Size(272, 34);

                LabelYearLevel.Location = new Point(320, 426);
                PanelYearLevel.Location = new Point(320, 448);
                PanelYearLevel.Size = new Size(272, 34);

                ButtonBack.Location = new Point(43, 510);
                ButtonBack.Size = new Size(110, 36);

                ButtonApply.Location = new Point(456, 510);
                ButtonApply.Size = new Size(136, 36);

                ButtonAccept.Location = new Point(436, 510);
                ButtonAccept.Size = new Size(156, 36);

                ButtonCancelApplication.Visible = false;
            }

            PanelPreviewCard.ResumeLayout();
            ResumeLayout();

            ResetDescriptionToTop();
        }

        private void ResetDescriptionToTop()
        {
            RichTextBoxDescription.SelectionStart = 0;
            RichTextBoxDescription.SelectionLength = 0;
            RichTextBoxDescription.ScrollToCaret();
        }

        private bool IsApprovedStatus(string status)
        {
            return string.Equals(status, "Approved", StringComparison.OrdinalIgnoreCase);
        }

        private bool IsPendingStatus(string status)
        {
            return string.Equals(status, "Pending", StringComparison.OrdinalIgnoreCase);
        }

        private void LoadRequirementsPreview(ScholarApplication application)
        {
            ListBoxRequirements.Items.Clear();

            if (application == null)
            {
                AddEmptyRequirementMessage();
                return;
            }

            AddRequirementItem("Application Letter / CV", application.ApplicationLetterOrCvPath);
            AddRequirementItem("Study Load", application.StudyLoadPath);
            AddRequirementItem("Good Moral", application.GoodMoralPath);
            AddRequirementItem("Previous Grades", application.PreviousGradesPath);
            AddRequirementItem("Parent Tax Returns", application.ParentTaxReturnsPath);

            if (ListBoxRequirements.Items.Count == 0)
            {
                AddEmptyRequirementMessage();
            }
        }

        private void AddRequirementItem(string label, string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return;
            }

            string fileName = Path.GetFileName(filePath);

            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = filePath;
            }

            ListBoxRequirements.Items.Add(new RequirementPreviewItem
            {
                DisplayName = label + ": " + fileName,
                FilePath = filePath
            });
        }

        private void AddEmptyRequirementMessage()
        {
            ListBoxRequirements.Items.Add(new RequirementPreviewItem
            {
                DisplayName = "No attached requirements found.",
                FilePath = string.Empty
            });
        }

        private void ButtonBack_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ButtonApply_Click(object sender, EventArgs e)
        {
            if (isApplicationPreviewMode || scholarship == null)
            {
                return;
            }

            using (Apply applyForm = new Apply())
            {
                applyForm.SetScholarship(scholarship, currentUserId, submitHandler);

                if (applyForm.ShowDialog(this) == DialogResult.OK)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }

        private void ButtonAccept_Click(object sender, EventArgs e)
        {
            if (currentApplication == null || !IsApprovedStatus(currentApplication.Status))
            {
                return;
            }

            DialogResult confirmResult = MessageBox.Show(
                "Do you want to accept this approved application?",
                "SchoolarLink",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult != DialogResult.Yes)
            {
                return;
            }

            bool saved = new ScholarApplicationADO().UpdateApplicationStatus(currentApplication.ApplicationId, "Accepted");

            if (!saved)
            {
                MessageBox.Show(
                    "Unable to update the application status.",
                    "SchoolarLink",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            currentApplication.Status = "Accepted";
            acceptHandler?.Invoke(currentApplication);

            Session.TriggerDatabaseChanged();

            MessageBox.Show(
                "Application accepted successfully.",
                "SchoolarLink",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonCancelApplication_Click(object sender, EventArgs e)
        {
            if (currentApplication == null || !IsPendingStatus(currentApplication.Status))
            {
                return;
            }

            DialogResult confirmResult = MessageBox.Show(
                "Do you want to cancel this pending application?",
                "SchoolarLink",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmResult != DialogResult.Yes)
            {
                return;
            }

            bool saved = new ScholarApplicationADO().UpdateApplicationStatus(currentApplication.ApplicationId, "Cancelled");

            if (!saved)
            {
                MessageBox.Show(
                    "Unable to cancel the application.",
                    "SchoolarLink",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            currentApplication.Status = "Cancelled";
            acceptHandler?.Invoke(currentApplication);

            Session.TriggerDatabaseChanged();

            MessageBox.Show(
                "Application cancelled successfully.",
                "SchoolarLink",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void ListBoxRequirements_DoubleClick(object sender, EventArgs e)
        {
            RequirementPreviewItem selectedItem = ListBoxRequirements.SelectedItem as RequirementPreviewItem;

            if (selectedItem == null || string.IsNullOrWhiteSpace(selectedItem.FilePath))
            {
                return;
            }

            if (!File.Exists(selectedItem.FilePath))
            {
                MessageBox.Show(
                    "The selected file could not be found:\n\n" + selectedItem.FilePath,
                    "Requirement Preview",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = selectedItem.FilePath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to open the selected requirement.\n\n" + ex.Message,
                    "Requirement Preview",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private sealed class RequirementPreviewItem
        {
            public string DisplayName { get; set; }
            public string FilePath { get; set; }

            public override string ToString()
            {
                return DisplayName;
            }
        }
    }
}