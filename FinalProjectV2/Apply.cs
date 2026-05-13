using System;
using System.IO;
using System.Windows.Forms;

namespace FinalProjectV2
{
    public partial class Apply : Form
    {
        private Scholarship scholarship;
        private string currentUserId;
        private Action<ScholarApplication> submitHandler;
        private string applicationLetterPath = string.Empty;
        private string studyLoadPath = string.Empty;
        private string goodMoralPath = string.Empty;
        private string previousGradesPath = string.Empty;
        private string parentTaxReturnsPath = string.Empty;

        public Apply()
        {
            InitializeComponent();
        }

        public void SetScholarship(
            Scholarship scholarshipInfo,
            string userId,
            Action<ScholarApplication> onSubmit)
        {
            scholarship = scholarshipInfo;
            currentUserId = userId;
            submitHandler = onSubmit;

            if (scholarship == null)
            {
                return;
            }

            LabelScholarshipNameValue.Text = scholarship.Name;
            LabelProviderValue.Text = scholarship.Provider;
            LabelDeadlineValue.Text = scholarship.Deadline.ToString("MMMM dd, yyyy");
        }

        private string PickFile(string title)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = title;
                dialog.Filter =
                    "Supported Files|*.pdf;*.doc;*.docx;*.jpg;*.jpeg;*.png|" +
                    "PDF Files|*.pdf|" +
                    "Word Documents|*.doc;*.docx|" +
                    "Images|*.jpg;*.jpeg;*.png|" +
                    "All Files|*.*";

                return dialog.ShowDialog(this) == DialogResult.OK ? dialog.FileName : string.Empty;
            }
        }

        private void ButtonUploadApplicationLetter_Click(object sender, EventArgs e)
        {
            string file = PickFile("Select Application Letter / CV");
            if (string.IsNullOrWhiteSpace(file))
            {
                return;
            }

            applicationLetterPath = file;
            LabelApplicationLetterFile.Text = Path.GetFileName(file);
        }

        private void ButtonUploadStudyLoad_Click(object sender, EventArgs e)
        {
            string file = PickFile("Select Study Load");
            if (string.IsNullOrWhiteSpace(file))
            {
                return;
            }

            studyLoadPath = file;
            LabelStudyLoadFile.Text = Path.GetFileName(file);
        }

        private void ButtonUploadGoodMoral_Click(object sender, EventArgs e)
        {
            string file = PickFile("Select Good Moral");
            if (string.IsNullOrWhiteSpace(file))
            {
                return;
            }

            goodMoralPath = file;
            LabelGoodMoralFile.Text = Path.GetFileName(file);
        }

        private void ButtonUploadPreviousGrades_Click(object sender, EventArgs e)
        {
            string file = PickFile("Select Previous Grades");
            if (string.IsNullOrWhiteSpace(file))
            {
                return;
            }

            previousGradesPath = file;
            LabelPreviousGradesFile.Text = Path.GetFileName(file);
        }

        private void ButtonUploadParentTaxReturns_Click(object sender, EventArgs e)
        {
            string file = PickFile("Select Parent's Tax Returns");
            if (string.IsNullOrWhiteSpace(file))
            {
                return;
            }

            parentTaxReturnsPath = file;
            LabelParentTaxReturnsFile.Text = Path.GetFileName(file);
        }

        private bool ValidateRequirements()
        {
            if (string.IsNullOrWhiteSpace(applicationLetterPath))
            {
                MessageBox.Show("Please upload your Application Letter / CV.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(studyLoadPath))
            {
                MessageBox.Show("Please upload your Study Load.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(goodMoralPath))
            {
                MessageBox.Show("Please upload your Good Moral.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(previousGradesPath))
            {
                MessageBox.Show("Please upload your Previous Grades.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(parentTaxReturnsPath))
            {
                MessageBox.Show("Please upload your Parent's Tax Returns.", "SchoolarLink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void ButtonBack_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ButtonSubmit_Click(object sender, EventArgs e)
        {
            if (scholarship == null)
            {
                return;
            }

            if (!ValidateRequirements())
            {
               
                return;
            }

            ScholarApplication result = new ScholarApplication
            {
                ScholarshipId = scholarship.ScholarshipID,
                UserId = currentUserId,
                Status = "Pending",
                ApplicationLetterOrCvPath = applicationLetterPath,
                StudyLoadPath = studyLoadPath,
                GoodMoralPath = goodMoralPath,
                PreviousGradesPath = previousGradesPath,
                ParentTaxReturnsPath = parentTaxReturnsPath,
                DateApplied = DateTime.Now
            };

            if (submitHandler != null)
            {
                submitHandler(result);
            }

            ScholarApplicationADO saado = new ScholarApplicationADO();

            if (saado.SubmitApplication(result.UserId, result.ScholarshipId)) {
                MessageBox.Show(
                    "Application submitted successfully.",
                    "SchoolarLink",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }else
            {
                MessageBox.Show(
                    "Failed to submit application. Please try again.",
                    "SchoolarLink",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }



            DialogResult = DialogResult.OK;
            Close();
        }
    }
}