using System;
using System.Windows.Forms;

namespace FinalProjectV2
{
    public partial class ForgotPasswordForm : Form
    {
        private readonly UserADO userADO = new UserADO();

        private string verifiedStudentId = string.Empty;
        private string verifiedUsername = string.Empty;
        private string verifiedEmail = string.Empty;

        public ForgotPasswordForm()
        {
            InitializeComponent();
            ShowVerifyPanel();
        }

        private void ShowVerifyPanel()
        {
            LabelTitle.Text = "Forgot password";
            LabelSubtitle.Text = "Enter your account details to continue with password recovery.";
            LabelFooterNote.Text = "Use your registered account details to continue.";

            PanelVerify.Visible = true;
            PanelReset.Visible = false;

            TextBoxStudentId.Clear();
            TextBoxUsername.Clear();
            TextBoxEmail.Clear();

            CheckBoxShowPasswords.Checked = false;
            TextBoxNewPassword.Clear();
            TextBoxConfirmPassword.Clear();

            TextBoxStudentId.Focus();
            AcceptButton = ButtonSubmitVerification;
        }

        private void ShowResetPanel()
        {
            LabelTitle.Text = "Create new password";
            LabelSubtitle.Text = "Your account has been verified. Enter and confirm your new password.";
            LabelFooterNote.Text = "Choose a strong password you can remember.";

            PanelVerify.Visible = false;
            PanelReset.Visible = true;

            TextBoxNewPassword.Clear();
            TextBoxConfirmPassword.Clear();
            CheckBoxShowPasswords.Checked = false;

            TextBoxNewPassword.Focus();
            AcceptButton = ButtonResetPassword;
        }

        private void ButtonSidebarBackToSignIn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ButtonSubmitVerification_Click(object sender, EventArgs e)
        {
            string studentId = TextBoxStudentId.Text.Trim();
            string username = TextBoxUsername.Text.Trim();
            string email = TextBoxEmail.Text.Trim();

            if (string.IsNullOrWhiteSpace(studentId))
            {
                MessageBox.Show("Please enter your student ID.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBoxStudentId.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Please enter your username.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBoxUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Please enter your email address.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBoxEmail.Focus();
                return;
            }

            if (!email.Contains("@"))
            {
                MessageBox.Show("Please enter a valid email address.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBoxEmail.Focus();
                return;
            }

            bool isValidAccount = userADO.VerifyUserForPasswordReset(studentId, username, email);
            if (!isValidAccount)
            {
                MessageBox.Show(
                    "We could not match those account details. Please check your student ID, username, and email address.",
                    "SchoolarLink",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            verifiedStudentId = studentId;
            verifiedUsername = username;
            verifiedEmail = email;

            ShowResetPanel();
        }

        private void CheckBoxShowPasswords_CheckedChanged(object sender, EventArgs e)
        {
            bool show = CheckBoxShowPasswords.Checked;
            TextBoxNewPassword.UseSystemPasswordChar = !show;
            TextBoxConfirmPassword.UseSystemPasswordChar = !show;
        }

        private void ButtonResetPassword_Click(object sender, EventArgs e)
        {
            string newPassword = TextBoxNewPassword.Text;
            string confirmPassword = TextBoxConfirmPassword.Text;

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("Please enter your new password.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBoxNewPassword.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Please confirm your new password.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBoxConfirmPassword.Focus();
                return;
            }

            if (newPassword.Length < 8)
            {
                MessageBox.Show("Password must be at least 8 characters long.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBoxNewPassword.Focus();
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBoxConfirmPassword.Focus();
                return;
            }

            bool isUpdated = userADO.ResetPassword(verifiedStudentId, verifiedUsername, verifiedEmail, newPassword);
            if (!isUpdated)
            {
                MessageBox.Show("Password reset failed. Please try again.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Your password has been reset successfully.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}