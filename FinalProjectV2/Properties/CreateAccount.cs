using System;
using System.Drawing;
using System.Windows.Forms;

namespace ScholarLinkk
{
    public partial class CreateAccount : Form
    {
        public CreateAccount()
        {
            InitializeComponent();
            SetupPlaceholders();

            // Adjust label positions dynamically to look perfectly centered
            lblBrandName.Left = (panelLeft.Width - lblBrandName.Width) / 2;
            lblBrandSub.Left = (panelLeft.Width - lblBrandSub.Width) / 2;
            panelLogo.Left = (panelLeft.Width - panelLogo.Width) / 2;

            // Wire up the Create Account button
            btnCreate.Click += BtnCreate_Click;
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            // Open Form2 and hide Form1
            Step2Account form2 = new Step2Account();
            form2.Show();
            this.Hide();

            // Make sure the application closes fully when Form2 is closed
            form2.FormClosed += (s, args) => this.Close();
        }

        private void SetupPlaceholders()
        {
            AssignPlaceholder(txtEmail, "john.doe@university.edu");
            AssignPlaceholder(txtPassword, "Create a strong password");
            AssignPlaceholder(txtConfirm, "Confirm your password");

            txtPassword.TextChanged += (s, e) =>
            {
                txtPassword.UseSystemPasswordChar = txtPassword.Text != "Create a strong password";
            };

            txtConfirm.TextChanged += (s, e) =>
            {
                txtConfirm.UseSystemPasswordChar = txtConfirm.Text != "Confirm your password";
            };
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
    }
}