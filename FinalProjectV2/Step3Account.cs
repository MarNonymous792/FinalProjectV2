using System;
using System.Drawing;
using System.Windows.Forms;

namespace ScholarLinkk
{
    public partial class Step3Account : Form
    {
        public Step3Account()
        {
            InitializeComponent();
            SetupPlaceholders();
            SetupComboBoxes();

            // Wire up the navigation buttons
            btnContinue.Click += btnContinue_Click;
            btnBack.Click += btnBack_Click;

            // Make the Exit label look clickable and wire it up
            lblExitSetup.Cursor = Cursors.Hand;
            lblExitSetup.Click += lblExitSetup_Click;

            // Attach custom paint event to draw the step indicators.
            // In Form3, Step 2 is the active one.
            lblStep1.Paint += DrawStepCircle;
            lblStep2.Paint += DrawStepCircle;
            lblStep3.Paint += DrawStepCircle;
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            // Transition to Form 4
            Step4Account form4 = new Step4Account();
            form4.Show();
            this.Hide();

            // Ensure the application closes completely when Form4 is closed.
            form4.FormClosed += (s, args) => this.Close();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            // Transition back to Form 2
            Step2Account form2 = new Step2Account();
            form2.Show();
            this.Hide();

            form2.FormClosed += (s, args) => this.Close();
        }

        private void lblExitSetup_Click(object sender, EventArgs e)
        {
            // Ask the user to confirm before exiting the setup process
            DialogResult result = MessageBox.Show(
                "Are you sure you want to exit the setup? Your progress will not be saved.",
                "Exit Setup",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Go back to the very first screen (Form1)
                CreateAccount form1 = new CreateAccount();
                form1.Show();
                this.Hide();

                form1.FormClosed += (s, args) => this.Close();
            }
        }

        private void SetupPlaceholders()
        {
            // Set up all placeholders with gray text
            AssignPlaceholder(txtSchoolInst, "Enter your school name");
            AssignPlaceholder(txtGPA, "e.g., 3.75");
            AssignPlaceholder(txtMajorField, "e.g., Computer Science");
            AssignPlaceholder(txtGradDate, "--------- ----📅");
            AssignPlaceholder(txtCity, "Enter your city");
            AssignPlaceholder(txtExtrAct, "Describe your extracurricular activities, clubs, sports, or organizations you're involved in...");
            AssignPlaceholder(txtVolExp, "Share your volunteer work, community service, or social impact initiatives...");
            AssignPlaceholder(txtSkillsComp, "e.g., Python, Public Speaking, Project Management (separate with commas)");
        }

        private void SetupComboBoxes()
        {
            // Add placeholder items and default selections
            cmbGPAScale.Items.Add("4.0 Scale");
            cmbGPAScale.SelectedIndex = 0;

            cmbAcadYear.Items.Add("Select year");
            cmbAcadYear.SelectedIndex = 0;

            cmbCountry.Items.Add("Select country");
            cmbCountry.SelectedIndex = 0;

            cmbStateProv.Items.Add("Select state");
            cmbStateProv.SelectedIndex = 0;

            cmbResStatus.Items.Add("Select status");
            cmbResStatus.SelectedIndex = 0;
        }

        private void AssignPlaceholder(TextBox textBox, string placeholder)
        {
            textBox.Text = placeholder;
            textBox.ForeColor = Color.Gray;

            textBox.Enter += (s, e) =>
            {
                if (textBox.Text == placeholder)
                {
                    textBox.Text = "";
                    textBox.ForeColor = Color.Black;
                }
            };

            textBox.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = placeholder;
                    textBox.ForeColor = Color.Gray;
                }
            };
        }

        // Custom drawing method for circular progress steps (Step 2 is active in Form3)
        private void DrawStepCircle(object sender, PaintEventArgs e)
        {
            Label lbl = (Label)sender;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // In this form, Step 2 is active. Steps 1 and 3 are in the past/future.
            bool isActive = lbl.Name == "lblStep2";
            Color bgColor = isActive ? Color.FromArgb(26, 26, 26) : Color.FromArgb(235, 235, 235);
            Color txtColor = isActive ? Color.White : Color.Gray;

            using (SolidBrush brush = new SolidBrush(bgColor))
            {
                e.Graphics.FillEllipse(brush, 0, 0, lbl.Width - 1, lbl.Height - 1);
            }

            TextRenderer.DrawText(e.Graphics, lbl.Text, lbl.Font, lbl.ClientRectangle, txtColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

            lbl.Text = string.Empty; // Prevent text from double-rendering
        }
    }
}