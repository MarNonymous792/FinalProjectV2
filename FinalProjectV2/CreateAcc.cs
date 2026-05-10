using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FinalProjectV2
{
    public partial class CreateAcc : Form
    {
        private readonly Color darkBlue = Color.FromArgb(13, 33, 62);
        private readonly Color deepBlue = Color.FromArgb(24, 54, 99);
        private readonly Color accentGold = Color.FromArgb(201, 158, 42);
        private readonly Color textDark = Color.FromArgb(24, 34, 56);
        private readonly Color textMuted = Color.FromArgb(111, 123, 146);
        private readonly Color inputBackground = Color.FromArgb(246, 248, 252);
        private readonly Color inactiveCard = Color.FromArgb(244, 246, 250);

        private const string UcPortalUrl = "https://enrollment.uc.edu.ph/college";

        private const string NoStudyLoadText = "No file selected";
        private const string NoGradesText = "No file selected";
        private const string NoGoodMoralText = "No file selected";
        private const string NoIncomeDocsText = "No file selected";

        private const int WmNclButtonDown = 0xA1;
        private const int HtCaption = 0x2;

        private int currentStepIndex;

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        public CreateAcc()
        {
            InitializeComponent();
            InitializeTheme();
            RegisterDragTargets();
            InitializeAcademicDefaults();
            RegisterOutlineButtonPainting();
            ShowStep(0);
        }

        private void InitializeTheme()
        {
            DoubleBuffered = true;
            BackColor = Color.White;

            PanelRight.BackColor = Color.White;
            PanelContentHost.BackColor = Color.White;

            StyleInputPanel(PanelCredentialsEmail);
            StyleInputPanel(PanelCredentialsPassword);
            StyleInputPanel(PanelCredentialsConfirmPassword);

            StyleInputPanel(PanelPersonalName);
            StyleInputPanel(PanelPersonalBirthplace);
            StyleInputPanel(PanelPersonalCurrentResidence);
            StyleInputPanel(PanelPersonalProvincialResidence);
            StyleInputPanel(PanelPersonalContactDetails);

            StyleInputPanel(PanelAcademicIdNumber);
            StyleInputPanel(PanelAcademicProgram);

            StyleStepCard(PanelStepOneCard, LabelStepOneNumber, LabelStepOneText, true);
            StyleStepCard(PanelStepTwoCard, LabelStepTwoNumber, LabelStepTwoText, false);
            StyleStepCard(PanelStepThreeCard, LabelStepThreeNumber, LabelStepThreeText, false);

            StylePrimaryButton(ButtonNext);
            StylePrimaryButton(ButtonDone);
            StyleOutlinedButton(ButtonBack);

            StyleOutlinedButton(ButtonUploadStudyLoad);
            StyleOutlinedButton(ButtonUploadGrades);
            StyleOutlinedButton(ButtonUploadGoodMoral);
            StyleOutlinedButton(ButtonUploadIncomeDocs);

            LabelAppName.BackColor = Color.Transparent;
            LabelTagline.BackColor = Color.Transparent;
            LabelFeatureTitle.BackColor = Color.Transparent;
            LabelFeature1.BackColor = Color.Transparent;
            LabelFeature2.BackColor = Color.Transparent;
            LabelFeature3.BackColor = Color.Transparent;
            LabelFooterLeft.BackColor = Color.Transparent;

            LabelPortal.ForeColor = accentGold;
            LabelWelcome.ForeColor = textDark;
            LabelSubtitle.ForeColor = textMuted;
            LabelUcHint.ForeColor = textMuted;

            LabelStudyLoadFile.ForeColor = textMuted;
            LabelGradesFile.ForeColor = textMuted;
            LabelGoodMoralFile.ForeColor = textMuted;
            LabelIncomeDocsFile.ForeColor = textMuted;

            TextBoxEmail.BackColor = inputBackground;
            TextBoxPassword.BackColor = inputBackground;
            TextBoxConfirmPassword.BackColor = inputBackground;
            TextBoxFullName.BackColor = inputBackground;
            TextBoxBirthplace.BackColor = inputBackground;
            TextBoxCurrentResidence.BackColor = inputBackground;
            TextBoxProvincialResidence.BackColor = inputBackground;
            TextBoxContactDetails.BackColor = inputBackground;
            TextBoxIdNumber.BackColor = inputBackground;
            TextBoxProgram.BackColor = inputBackground;

            TextBoxEmail.ForeColor = textDark;
            TextBoxPassword.ForeColor = textDark;
            TextBoxConfirmPassword.ForeColor = textDark;
            TextBoxFullName.ForeColor = textDark;
            TextBoxBirthplace.ForeColor = textDark;
            TextBoxCurrentResidence.ForeColor = textDark;
            TextBoxProvincialResidence.ForeColor = textDark;
            TextBoxContactDetails.ForeColor = textDark;
            TextBoxIdNumber.ForeColor = textDark;
            TextBoxProgram.ForeColor = textDark;

            CheckBoxShowPasswords.ForeColor = textMuted;
            RadioButtonUcYes.ForeColor = textDark;
            RadioButtonUcNo.ForeColor = textDark;

            ButtonMinimize.ForeColor = textDark;
            ButtonClose.ForeColor = textDark;

            DateTimePickerBirthday.MaxDate = DateTime.Today;
            DateTimePickerBirthday.Value = new DateTime(2006, 1, 1);
            DateTimePickerBirthday.Format = DateTimePickerFormat.Custom;
            DateTimePickerBirthday.CustomFormat = "MMMM dd, yyyy";

            ComboBoxYearLevel.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboBoxYearLevel.SelectedIndex = 0;

            ToggleUcEnrollmentFields();
            ApplyRoundedLayout();
        }

        private void InitializeAcademicDefaults()
        {
            LabelStudyLoadFile.Text = NoStudyLoadText;
            LabelGradesFile.Text = NoGradesText;
            LabelGoodMoralFile.Text = NoGoodMoralText;
            LabelIncomeDocsFile.Text = NoIncomeDocsText;
        }

        private void RegisterOutlineButtonPainting()
        {
            RegisterOutlinePaint(ButtonBack);
            RegisterOutlinePaint(ButtonUploadStudyLoad);
            RegisterOutlinePaint(ButtonUploadGrades);
            RegisterOutlinePaint(ButtonUploadGoodMoral);
            RegisterOutlinePaint(ButtonUploadIncomeDocs);
        }

        private void RegisterOutlinePaint(Button button)
        {
            button.Paint += OutlinedButton_Paint;
            button.Resize += OutlinedButton_Resize;
        }

        private void OutlinedButton_Resize(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                int radius = button == ButtonBack ? 18 : 16;
                ApplyRoundedRegion(button, radius);
                button.Invalidate();
            }
        }

        private void OutlinedButton_Paint(object sender, PaintEventArgs e)
        {
            if (!(sender is Button button))
            {
                return;
            }

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle borderRect = new Rectangle(1, 1, button.Width - 3, button.Height - 3);
            int radius = button == ButtonBack ? 18 : 16;

            Color borderColor = accentGold;
            Color fillColor = Color.White;
            Color textColor = darkBlue;

            if (!button.Enabled)
            {
                borderColor = Color.FromArgb(210, 210, 210);
                textColor = Color.FromArgb(160, 160, 160);
            }
            else if (button.ClientRectangle.Contains(button.PointToClient(Cursor.Position)) &&
                     Control.MouseButtons == MouseButtons.None)
            {
                fillColor = Color.FromArgb(252, 250, 244);
            }
            else if (button.ClientRectangle.Contains(button.PointToClient(Cursor.Position)) &&
                     Control.MouseButtons == MouseButtons.Left)
            {
                fillColor = Color.FromArgb(247, 243, 229);
            }

            using (GraphicsPath fillPath = CreateRoundedPath(borderRect, radius))
            using (SolidBrush fillBrush = new SolidBrush(fillColor))
            using (Pen borderPen = new Pen(borderColor, 1.4f))
            using (StringFormat sf = new StringFormat())
            using (SolidBrush textBrush = new SolidBrush(textColor))
            {
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;

                e.Graphics.FillPath(fillBrush, fillPath);
                e.Graphics.DrawPath(borderPen, fillPath);

                Rectangle textRect = new Rectangle(
                    borderRect.X + 8,
                    borderRect.Y,
                    borderRect.Width - 16,
                    borderRect.Height);

                e.Graphics.DrawString(button.Text, button.Font, textBrush, textRect, sf);
            }
        }

        private void StyleInputPanel(Panel panel)
        {
            panel.BackColor = inputBackground;
        }

        private void StylePrimaryButton(Button button)
        {
            button.BackColor = darkBlue;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(10, 25, 48);
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(22, 48, 90);
        }

        private void StyleOutlinedButton(Button button)
        {
            button.BackColor = Color.White;
            button.ForeColor = darkBlue;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.BorderColor = Color.White;
            button.FlatAppearance.MouseDownBackColor = Color.White;
            button.FlatAppearance.MouseOverBackColor = Color.White;
            button.UseVisualStyleBackColor = false;
            button.Padding = new Padding(0);
        }

        private void StyleStepCard(Panel panel, Label numberLabel, Label textLabel, bool active)
        {
            panel.BackColor = active ? darkBlue : inactiveCard;
            numberLabel.ForeColor = active ? accentGold : darkBlue;
            textLabel.ForeColor = active ? Color.White : textDark;
        }

        private void RegisterDragTargets()
        {
            RegisterDrag(this);
            RegisterDrag(PanelLeft);
            RegisterDrag(PanelRight);
            RegisterDrag(LabelAppName);
            RegisterDrag(LabelTagline);
            RegisterDrag(LabelPortal);
            RegisterDrag(LabelWelcome);
            RegisterDrag(LabelSubtitle);
        }

        private void RegisterDrag(Control control)
        {
            if (control != null)
            {
                control.MouseDown += DragWindow;
            }
        }

        private void DragWindow(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            ReleaseCapture();
            SendMessage(Handle, WmNclButtonDown, HtCaption, 0);
        }

        private void CreateAcc_Shown(object sender, EventArgs e)
        {
            ApplyRoundedLayout();
        }

        private void CreateAcc_Resize(object sender, EventArgs e)
        {
            ApplyRoundedLayout();
        }

        private void ApplyRoundedLayout()
        {
            ApplyRoundedForm(26);

            ApplyRoundedRegion(PanelStepOneCard, 18);
            ApplyRoundedRegion(PanelStepTwoCard, 18);
            ApplyRoundedRegion(PanelStepThreeCard, 18);

            ApplyRoundedRegion(PanelCredentialsEmail, 18);
            ApplyRoundedRegion(PanelCredentialsPassword, 18);
            ApplyRoundedRegion(PanelCredentialsConfirmPassword, 18);

            ApplyRoundedRegion(PanelPersonalName, 18);
            ApplyRoundedRegion(PanelPersonalBirthplace, 18);
            ApplyRoundedRegion(PanelPersonalCurrentResidence, 18);
            ApplyRoundedRegion(PanelPersonalProvincialResidence, 18);
            ApplyRoundedRegion(PanelPersonalContactDetails, 18);

            ApplyRoundedRegion(PanelAcademicIdNumber, 18);
            ApplyRoundedRegion(PanelAcademicProgram, 18);

            ApplyRoundedRegion(ButtonBack, 18);
            ApplyRoundedRegion(ButtonNext, 18);
            ApplyRoundedRegion(ButtonDone, 18);

            ApplyRoundedRegion(ButtonUploadStudyLoad, 16);
            ApplyRoundedRegion(ButtonUploadGrades, 16);
            ApplyRoundedRegion(ButtonUploadGoodMoral, 16);
            ApplyRoundedRegion(ButtonUploadIncomeDocs, 16);
        }

        private void ApplyRoundedForm(int radius)
        {
            if (Width <= 0 || Height <= 0)
            {
                return;
            }

            using (GraphicsPath path = CreateRoundedPath(new Rectangle(0, 0, Width, Height), radius))
            {
                Region = new Region(path);
            }
        }

        private void ApplyRoundedRegion(Control control, int radius)
        {
            if (control == null || control.Width <= 0 || control.Height <= 0)
            {
                return;
            }

            using (GraphicsPath path = CreateRoundedPath(new Rectangle(0, 0, control.Width, control.Height), radius))
            {
                control.Region = new Region(path);
            }
        }

        private GraphicsPath CreateRoundedPath(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;
            Rectangle arc = new Rectangle(bounds.X, bounds.Y, diameter, diameter);

            path.StartFigure();
            path.AddArc(arc, 180, 90);

            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        private void ShowStep(int stepIndex)
        {
            currentStepIndex = stepIndex;

            PanelStepCredentials.Visible = stepIndex == 0;
            PanelStepPersonal.Visible = stepIndex == 1;
            PanelStepAcademic.Visible = stepIndex == 2;

            ButtonBack.Visible = stepIndex > 0;
            ButtonNext.Visible = stepIndex < 2;
            ButtonDone.Visible = stepIndex == 2;

            AcceptButton = stepIndex == 2 ? ButtonDone : ButtonNext;

            PanelContentHost.AutoScrollPosition = new Point(0, 0);

            UpdateStepCards();
            ToggleUcEnrollmentFields();
        }

        private void UpdateStepCards()
        {
            StyleStepCard(PanelStepOneCard, LabelStepOneNumber, LabelStepOneText, currentStepIndex == 0);
            StyleStepCard(PanelStepTwoCard, LabelStepTwoNumber, LabelStepTwoText, currentStepIndex == 1);
            StyleStepCard(PanelStepThreeCard, LabelStepThreeNumber, LabelStepThreeText, currentStepIndex == 2);
        }

        private void ToggleUcEnrollmentFields()
        {
            bool isUcStudent = RadioButtonUcYes.Checked;
            bool isNotUcStudent = RadioButtonUcNo.Checked;

            PanelUcFields.Visible = isUcStudent;
            PanelUcFields.Enabled = isUcStudent;

            if (isUcStudent)
            {
                LabelUcHint.Text = "Provide your current UC academic documents to complete the setup.";
                ButtonDone.Text = "Finish Setup";
            }
            else if (isNotUcStudent)
            {
                LabelUcHint.Text = "This will open the University of Cebu enrollment portal when you finish.";
                ButtonDone.Text = "Open UC Portal";
            }
            else
            {
                LabelUcHint.Text = "Select Yes or No to continue.";
                ButtonDone.Text = "Finish Setup";
            }
        }

        private bool ValidateCurrentStep()
        {
            switch (currentStepIndex)
            {
                case 0:
                    return ValidateStepCredentials();
                case 1:
                    return ValidateStepPersonal();
                case 2:
                    return ValidateStepAcademic();
                default:
                    return true;
            }
        }

        private bool ValidateStepCredentials()
        {
            if (string.IsNullOrWhiteSpace(TextBoxEmail.Text))
            {
                MessageBox.Show("Please enter your email address.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBoxEmail.Focus();
                return false;
            }

            if (!TextBoxEmail.Text.Contains("@"))
            {
                MessageBox.Show("Please enter a valid email address.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBoxEmail.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(TextBoxPassword.Text))
            {
                MessageBox.Show("Please enter your password.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBoxPassword.Focus();
                return false;
            }

            if (TextBoxPassword.Text.Length < 8)
            {
                MessageBox.Show("Password must be at least 8 characters long.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBoxPassword.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(TextBoxConfirmPassword.Text))
            {
                MessageBox.Show("Please confirm your password.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBoxConfirmPassword.Focus();
                return false;
            }

            if (!string.Equals(TextBoxPassword.Text, TextBoxConfirmPassword.Text, StringComparison.Ordinal))
            {
                MessageBox.Show("Password and confirm password do not match.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBoxConfirmPassword.Focus();
                return false;
            }

            return true;
        }

        private bool ValidateStepPersonal()
        {
            if (string.IsNullOrWhiteSpace(TextBoxFullName.Text))
            {
                MessageBox.Show("Please enter your full name.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBoxFullName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(TextBoxBirthplace.Text))
            {
                MessageBox.Show("Please enter your birthplace.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBoxBirthplace.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(TextBoxCurrentResidence.Text))
            {
                MessageBox.Show("Please enter your current place of residence.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBoxCurrentResidence.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(TextBoxProvincialResidence.Text))
            {
                MessageBox.Show("Please enter your provincial residence.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBoxProvincialResidence.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(TextBoxContactDetails.Text))
            {
                MessageBox.Show("Please enter your contact details.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBoxContactDetails.Focus();
                return false;
            }

            return true;
        }

        private bool ValidateStepAcademic()
        {
            if (!RadioButtonUcYes.Checked && !RadioButtonUcNo.Checked)
            {
                MessageBox.Show("Please answer whether you are currently enrolled in UC.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (RadioButtonUcNo.Checked)
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(TextBoxIdNumber.Text))
            {
                MessageBox.Show("Please enter your UC ID number.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBoxIdNumber.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(TextBoxProgram.Text))
            {
                MessageBox.Show("Please enter your enrolled program.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBoxProgram.Focus();
                return false;
            }

            if (ComboBoxYearLevel.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select your year level.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ComboBoxYearLevel.Focus();
                return false;
            }

            if (!IsFileSelected(LabelStudyLoadFile, NoStudyLoadText))
            {
                MessageBox.Show("Please upload your study load.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!IsFileSelected(LabelGradesFile, NoGradesText))
            {
                MessageBox.Show("Please upload your grades from last year.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!IsFileSelected(LabelGoodMoralFile, NoGoodMoralText))
            {
                MessageBox.Show("Please upload your good moral.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!IsFileSelected(LabelIncomeDocsFile, NoIncomeDocsText))
            {
                MessageBox.Show("Please upload your parent's income tax returns or certificate of indigency.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private bool IsFileSelected(Label label, string defaultText)
        {
            return !string.Equals(label.Text, defaultText, StringComparison.Ordinal);
        }

        private void ButtonNext_Click(object sender, EventArgs e)
        {
            if (!ValidateCurrentStep())
            {
                return;
            }

            ShowStep(currentStepIndex + 1);
        }

        private void ButtonBack_Click(object sender, EventArgs e)
        {
            if (currentStepIndex <= 0)
            {
                return;
            }

            ShowStep(currentStepIndex - 1);
        }

        private void ButtonDone_Click(object sender, EventArgs e)
        {
            if (!ValidateCurrentStep())
            {
                return;
            }

            if (RadioButtonUcNo.Checked)
            {
                OpenUcPortal();
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            MessageBox.Show(
                "Account setup completed.\n\nYour SchoolarLink profile is ready.",
                "SchoolarLink",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void CheckBoxShowPasswords_CheckedChanged(object sender, EventArgs e)
        {
            bool showPasswords = CheckBoxShowPasswords.Checked;
            TextBoxPassword.UseSystemPasswordChar = !showPasswords;
            TextBoxConfirmPassword.UseSystemPasswordChar = !showPasswords;
        }

        private void RadioButtonUc_CheckedChanged(object sender, EventArgs e)
        {
            ToggleUcEnrollmentFields();
        }

        private void ButtonUploadStudyLoad_Click(object sender, EventArgs e)
        {
            SelectFile(LabelStudyLoadFile, "Select Study Load");
        }

        private void ButtonUploadGrades_Click(object sender, EventArgs e)
        {
            SelectFile(LabelGradesFile, "Select Last Year Grades");
        }

        private void ButtonUploadGoodMoral_Click(object sender, EventArgs e)
        {
            SelectFile(LabelGoodMoralFile, "Select Good Moral");
        }

        private void ButtonUploadIncomeDocs_Click(object sender, EventArgs e)
        {
            SelectFile(LabelIncomeDocsFile, "Select Income Tax Returns or Certificate of Indigency");
        }

        private void SelectFile(Label targetLabel, string title)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = title;
                dialog.Filter =
                    "Supported Files|*.pdf;*.jpg;*.jpeg;*.png;*.doc;*.docx|" +
                    "PDF Files|*.pdf|" +
                    "Image Files|*.jpg;*.jpeg;*.png|" +
                    "Word Documents|*.doc;*.docx|" +
                    "All Files|*.*";

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    targetLabel.Text = Path.GetFileName(dialog.FileName);
                    targetLabel.Tag = dialog.FileName;
                }
            }
        }

        private void OpenUcPortal()
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = UcPortalUrl,
                    UseShellExecute = true
                });
            }
            catch
            {
                MessageBox.Show(
                    "Unable to open the UC portal automatically.\n\nOpen this link manually:\n" + UcPortalUrl,
                    "SchoolarLink",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void ButtonMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void PanelLeft_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect = PanelLeft.ClientRectangle;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (LinearGradientBrush brush = new LinearGradientBrush(
                rect,
                darkBlue,
                deepBlue,
                LinearGradientMode.ForwardDiagonal))
            {
                e.Graphics.FillRectangle(brush, rect);
            }

            using (SolidBrush glowBrush = new SolidBrush(Color.FromArgb(28, 255, 255, 255)))
            {
                e.Graphics.FillEllipse(glowBrush, 18, 8, 100, 100);
                e.Graphics.FillEllipse(glowBrush, 180, 20, 128, 140);
                e.Graphics.FillEllipse(glowBrush, 172, 340, 140, 140);
                e.Graphics.FillEllipse(glowBrush, -84, 470, 210, 210);
            }
        }

        private void LabelSubtitle_Click(object sender, EventArgs e)
        {

        }
    }
}