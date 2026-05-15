using System;
using System.Windows.Forms;

namespace FinalProjectV2
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            LinkLabelForgotPassword.LinkClicked += LinkLabelForgotPassword_LinkClicked;
        }

        private void CheckBoxShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            TextBoxPassword.UseSystemPasswordChar = !CheckBoxShowPassword.Checked;
        }

        private void ButtonLogin_Click(object sender, EventArgs e)
        {
            string identifier = TextBoxIdentifier.Text.Trim();
            string password = TextBoxPassword.Text;

            if (string.IsNullOrWhiteSpace(identifier) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show(
                    "Please enter your email, phone number, or school ID, and your password.",
                    "SchoolarLink",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            UserADO userADO = new UserADO();
            User newUser = userADO.Login(identifier, password);

            if (newUser != null)
            {
                Dashboard dashboard = new Dashboard(newUser);
                dashboard.Show();
                Hide();
                return;
            }

            MessageBox.Show(
                "Invalid Credentials.",
                "SchoolarLink",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        private void LinkLabelForgotPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (ForgotPasswordForm forgotPasswordForm = new ForgotPasswordForm())
            {
                forgotPasswordForm.ShowDialog(this);
            }

            TextBoxPassword.Clear();
            TextBoxIdentifier.Focus();
        }

        private void ButtonSidebarCreateAccount_Click(object sender, EventArgs e)
        {
            using (CreateAcc createAcc = new CreateAcc())
            {
                createAcc.ShowDialog(this);
            }
        }

        private void LinkLabelSignUp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (CreateAcc createAcc = new CreateAcc())
            {
                createAcc.ShowDialog(this);
            }
        }
    }
}