using FinalProjectV2;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace FinalProjectV2
{
    public partial class CreateAcc : Form
    {
        public CreateAcc()
        {
            InitializeComponent();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            LoadCourseOptions();
            LoadYearLevelOptions();
        }

        private void LoadCourseOptions()
        {
            ComboBoxCourse.Items.Clear();
            ComboBoxCourse.Items.AddRange(new object[]
            {
                "Select course",
                "BSIT",
                "BSCS",
                "BSCrim",
                "BSNursing",
                "BSBA",
                "BSA",
                "BSCE",
                "BSMarE",
                "BSEE",
                "BSArch",
                "BSEd / BEEd",
                "BSHM",
                "BSTM",
                "BSPsych",
                "BSBio",
                "BSMT",
                "BSPharm",
                "BSSW",
                "BSOA",
                "BSEntrep",
                "BSCA",
                "BSMT",
                "BSME"
            });
            ComboBoxCourse.SelectedIndex = 0;
        }

        private void LoadYearLevelOptions()
        {
            ComboBoxYearLevel.Items.Clear();
            ComboBoxYearLevel.Items.AddRange(new object[]
            {
                "Select year level",
                "1st Year",
                "2nd Year",
                "3rd Year",
                "4th Year",
                "5th Year"
            });
            ComboBoxYearLevel.SelectedIndex = 0;
        }

        private void CheckBoxShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            bool hidden = !CheckBoxShowPassword.Checked;
            TextBoxPassword.UseSystemPasswordChar = hidden;
            TextBoxConfirmPassword.UseSystemPasswordChar = hidden;
        }

        private void ButtonCreateAccount_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBoxUsername.Text))
            {
                MessageBox.Show("Please enter a username.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBoxUsername.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(TextBoxStudentID.Text))
            {
                MessageBox.Show("Please enter your student ID.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBoxStudentID.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(TextBoxFirstName.Text))
            {
                MessageBox.Show("Please enter your first name.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBoxFirstName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(TextBoxLastName.Text))
            {
                MessageBox.Show("Please enter your last name.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBoxLastName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(TextBoxEmail.Text) || !TextBoxEmail.Text.Contains("@"))
            {
                MessageBox.Show("Please enter a valid email address.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBoxEmail.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(TextBoxContactNumber.Text))
            {
                MessageBox.Show("Please enter your contact number.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBoxContactNumber.Focus();
                return;
            }

            if (ComboBoxCourse.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select your course.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ComboBoxCourse.Focus();
                return;
            }

            if (ComboBoxYearLevel.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select your year level.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ComboBoxYearLevel.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(TextBoxPassword.Text))
            {
                MessageBox.Show("Please enter your password.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBoxPassword.Focus();
                return;
            }

            if (TextBoxPassword.Text.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBoxPassword.Focus();
                return;
            }

            if (TextBoxPassword.Text != TextBoxConfirmPassword.Text)
            {
                MessageBox.Show("Password and confirm password do not match.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBoxConfirmPassword.Focus();
                return;
            }

                
            UserADO userADO = new UserADO();

            string username = TextBoxUsername.Text;
            string password = TextBoxPassword.Text;
            string fname = TextBoxFirstName.Text;
            string mname = TextBoxMiddleName.Text;
            string lname = TextBoxLastName.Text;
            DateTime birthdate = DateTimePickerBirthday.Value;
            string email = TextBoxEmail.Text;
            string contact = TextBoxContactNumber.Text;
            string course = ComboBoxCourse.SelectedItem.ToString();
            int year = ComboBoxYearLevel.SelectedIndex;
            string studentId = TextBoxStudentID.Text;
            if (userADO.RegisterUser(studentId, username, password, fname, lname, mname,birthdate, email,contact, course, year))
            {
                MessageBox.Show("Account created successfully! You can now log in.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else { 
                MessageBox.Show("Username or student ID already exists. Please choose a different one.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


            Close();
        }

        private void ButtonBackToLogin_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}