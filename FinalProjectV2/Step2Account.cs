using System;
using System.Drawing;
using System.Windows.Forms;

namespace ScholarLinkk
{
    public partial class Step2Account : Form
    {
        public Step2Account()
        {
            InitializeComponent();
            SetupPlaceholders();
            SetupComboBoxes();

            // THIS IS THE MISSING LINK - Wiring the button to the action
            btnContinue.Click += btnContinue_Click;

            // Attach custom paint events to draw the circular step indicators
            lblStep1.Paint += DrawStepCircle;
            lblStep2.Paint += DrawStepCircle;
            lblStep3.Paint += DrawStepCircle;
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            Step3Account form3 = new Step3Account();
            form3.Show();
            this.Hide();

            // Ensure the app closes fully if Form3 is closed
            form3.FormClosed += (s, args) => this.Close();
        }

        private void SetupPlaceholders()
        {
            AssignPlaceholder(txtFirstName, "Enter your first name");
            AssignPlaceholder(txtLastName, "Enter your last name");
            AssignPlaceholder(txtDOB, "mm/dd/yyyy 📅");
        }

        private void SetupComboBoxes()
        {
            cmbCountry.Items.Add("Select your country");
            cmbCountry.SelectedIndex = 0;

            cmbNationality.Items.Add("Select your nationality");
            cmbNationality.SelectedIndex = 0;
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

        private void DrawStepCircle(object sender, PaintEventArgs e)
        {
            Label lbl = (Label)sender;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            bool isActive = lbl.Name == "lblStep1";
            Color bgColor = isActive ? Color.FromArgb(26, 26, 26) : Color.FromArgb(235, 235, 235);
            Color txtColor = isActive ? Color.White : Color.Gray;

            using (SolidBrush brush = new SolidBrush(bgColor))
            {
                e.Graphics.FillEllipse(brush, 0, 0, lbl.Width - 1, lbl.Height - 1);
            }

            TextRenderer.DrawText(e.Graphics, lbl.Text, lbl.Font, lbl.ClientRectangle, txtColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

            lbl.Text = string.Empty;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CreateAccount form1 = new CreateAccount();
            form1.Show();
            this.Hide();
        }
    }
}