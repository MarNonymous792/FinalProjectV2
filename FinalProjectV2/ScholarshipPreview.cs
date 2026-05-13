using System;
using System.Windows.Forms;

namespace FinalProjectV2
{
    public partial class ScholarshipPreview : Form
    {
        public Scholarship scholarship;
        private String currentUserId;
        private Action<ScholarApplication> submitHandler;

        public ScholarshipPreview()
        {
            InitializeComponent();
        }

        public void SetScholarship(
            Scholarship scholarshipInfo,
            User user,
            Action<ScholarApplication> onSubmit)
        {
            scholarship = scholarshipInfo;
            currentUserId = user.UserID;
            submitHandler = onSubmit;

            if (scholarship == null)
            {
                return;
            }

            LabelScholarshipNameValue.Text = scholarship.Name;
            LabelProviderValue.Text = scholarship.Provider;
            TextBoxDescription.Text = scholarship.Description;
            LabelAvailableSlotsValue.Text = scholarship.AvailableSlots.ToString();
            LabelDeadlineValue.Text = scholarship.Deadline.ToString("MMMM dd, yyyy");
            LabelRequiredCourseValue.Text = scholarship.RequiredCourse;
            LabelYearLevelValue.Text = scholarship.MaxYearLevel.ToString();
        }

        private void ButtonBack_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ButtonApply_Click(object sender, EventArgs e)
        {
            if (scholarship == null)
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
    }
}