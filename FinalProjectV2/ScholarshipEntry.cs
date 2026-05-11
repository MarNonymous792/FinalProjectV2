using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProjectV2
{
    public partial class ScholarshipEntry : Form
    {
        public ScholarshipEntry()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ScholarshipADO sado = new ScholarshipADO();

           DateTime na = checkBox1.Checked ? DateTime.MaxValue : timePicker.Value;

            Scholarship s = new Scholarship() {
                Name = nameTxtBox.Text,
                Provider = providerTxtBox.Text,
                Description = descTxtBox.Text,
                RequiredCourse = courseTxtBox.Text,
                MaxYearLevel = int.Parse(yearTxtBox.Text),
                Deadline = na,
                AvailableSlots = int.Parse(slotsTxtBox.Text)
            };


            if (sado.CreateScholarship(s))
            {
                MessageBox.Show("Scholarship created successfully!");
                //clear form
                nameTxtBox.Text = "";
                descTxtBox.Text = "";
                courseTxtBox.Text = "";
                yearTxtBox.Text = "";
                slotsTxtBox.Text = "";
                providerTxtBox.Text = "";
            }
            else { 
                MessageBox.Show("Failed to create scholarship. Please check your input and try again.");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            timePicker.Enabled = !checkBox1.Checked;
        }
    }
}
