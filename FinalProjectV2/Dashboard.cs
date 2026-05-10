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
    public partial class Dashboard : Form
    {
        public Dashboard(User user)
        {
            InitializeComponent();

            ScholarshipADO scholarshipADO = new ScholarshipADO();

            List<Scholarship> scholarships = scholarshipADO.GetSuitableScholarships(user);

            foreach (Scholarship scholarship in scholarships)
            {
                string dueDate = scholarship.Deadline.ToString("MMMM dd, yyyy");
                CreateScholarshipCard(scholarshipsPanel, scholarship.Name, $"Due: {dueDate}", scholarshipsPanel.Controls.Count * 105);
            }
        }

        private void CreateScholarshipCard(
            Panel parent,
            string title,
            string dueDate,
            int top
        )
        {
            Panel card = new Panel
            {
                Size = new Size(490, 95),
                Location = new Point(10, top),
                BackColor = Color.LightYellow,
                BorderStyle = BorderStyle.Fixed3D
            };

            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(12, 15)
            };

            Label lblDue = new Label
            {
                Text = dueDate,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.DimGray,
                AutoSize = true,
                Location = new Point(12, 50)
            };

            card.Controls.Add(lblTitle);
            card.Controls.Add(lblDue);

            parent.Controls.Add(card);
        }
    }
}
