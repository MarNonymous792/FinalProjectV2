using System;
using System.Drawing;
using System.Windows.Forms;

namespace ScholarLinkk
{
    public partial class Step4Account : Form
    {
        public Step4Account()
        {
            InitializeComponent();
            SetupPlaceholders();
            SetupComboBoxes();

            // Wire up navigation
            btnBack.Click += btnBack_Click;

            // Draw Step Circles
            lblStep1Circle.Paint += DrawPastStepCircle;
            lblStep2Circle.Paint += DrawPastStepCircle;
            lblStep3Circle.Paint += DrawActiveStepCircle;

            // Draw Dashed Borders for Upload Zones 
            pnlUploadResume.Paint += DrawDashedBorder;
            pnlUploadTranscripts.Paint += DrawDashedBorder;
            pnlUploadLetters.Paint += DrawDashedBorder;
            pnlUploadPortfolio.Paint += DrawDashedBorder;

            // Hover effect cursors
            lnkHelp.Cursor = Cursors.Hand;
            lblAddSchool.Cursor = Cursors.Hand;
            lblAddCompany.Cursor = Cursors.Hand;
            lnkSkip.Cursor = Cursors.Hand;
        }

        private void SetupPlaceholders()
        {
            AssignPlaceholder(txtSearchSchools, "Search schools...");
            AssignPlaceholder(txtSearchCompanies, "Search companies...");
        }

        private void SetupComboBoxes()
        {
            cmbCountry.Items.Add("Select country");
            cmbCountry.SelectedIndex = 0;
            cmbState.Items.Add("Select state");
            cmbState.SelectedIndex = 0;
        }

        private void AssignPlaceholder(TextBox textBox, string placeholder)
        {
            textBox.Text = placeholder;
            textBox.ForeColor = Color.Gray;

            textBox.Enter += (s, e) =>
            {
                if (textBox.Text == placeholder) { textBox.Text = ""; textBox.ForeColor = Color.Black; }
            };

            textBox.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text)) { textBox.Text = placeholder; textBox.ForeColor = Color.Gray; }
            };
        }

        // Draws the dashed drop-zone border 
        private void DrawDashedBorder(object sender, PaintEventArgs e)
        {
            Panel pnl = sender as Panel;
            using (Pen pen = new Pen(Color.DarkGray, 1.5f))
            {
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                e.Graphics.DrawRectangle(pen, 0, 0, pnl.Width - 1, pnl.Height - 1);
            }
        }

        private void DrawPastStepCircle(object sender, PaintEventArgs e)
        {
            Label lbl = (Label)sender;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(230, 230, 230)))
                e.Graphics.FillEllipse(brush, 0, 0, lbl.Width - 1, lbl.Height - 1);
            TextRenderer.DrawText(e.Graphics, lbl.Text, lbl.Font, lbl.ClientRectangle, Color.Gray, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            lbl.Text = string.Empty;
        }

        private void DrawActiveStepCircle(object sender, PaintEventArgs e)
        {
            Label lbl = (Label)sender;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(26, 26, 26)))
                e.Graphics.FillEllipse(brush, 0, 0, lbl.Width - 1, lbl.Height - 1);
            TextRenderer.DrawText(e.Graphics, lbl.Text, lbl.Font, lbl.ClientRectangle, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            lbl.Text = string.Empty;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Step3Account form3 = new Step3Account();
            form3.Show();
            this.Hide();
            form3.FormClosed += (s, args) => this.Close();
        }

        private void btnCreateAccount_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Account Setup Complete!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // --- THESE ARE THE MISSING FUNCTIONS FIXING YOUR ERRORS ---

        private void btnAddSchool_Click(object sender, EventArgs e)
        {
            // Logic to add more schools
        }

        private void btnAddCompany_Click(object sender, EventArgs e)
        {
            // Logic to add more companies
        }

        private void lnkSkip_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Skipping step 3...", "Skip", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void pnlDragAndDrop_MouseEnter(object sender, EventArgs e)
        {
            Panel pnl = sender as Panel;
            if (pnl != null)
            {
                // Slightly darken the panel to show it's active
                pnl.BackColor = Color.FromArgb(240, 240, 240);
            }
        }

        private void pnlDragAndDrop_MouseLeave(object sender, EventArgs e)
        {
            Panel pnl = sender as Panel;
            if (pnl != null)
            {
                // Return to original color
                pnl.BackColor = Color.FromArgb(250, 250, 250);
            }
        }
    }
}