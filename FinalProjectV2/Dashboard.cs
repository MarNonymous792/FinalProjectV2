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
        public Dashboard()
        {
            InitializeComponent();

            CreateScholarshipCard(
                scholarshipsPanel,
                "Scholar1",
                "Due: Jan 15, 2025",
                20
            );

            CreateScholarshipCard(
                scholarshipsPanel,
                "Scholar2",
                "Due: Jan 18, 2025",
                140
            );

            CreateScholarshipCard(
                scholarshipsPanel,
                "Scholar3",
                "Due: Jan 20, 2025",
                260
            );
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
                Size = new Size(610, 95),
                Location = new Point(15, top),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
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
