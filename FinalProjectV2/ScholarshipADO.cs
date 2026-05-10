using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectV2
{
    internal class ScholarshipADO
    {
        public List<Scholarship> GetSuitableScholarships(User currentUser)
        {
            List<Scholarship> list = new List<Scholarship>();

            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                string sql = @"SELECT * FROM scholarship 
                       WHERE required_course = @userCourse OR required_course = 'Any'
                       AND max_year_level >= @userYear";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userCourse", currentUser.Course);
                cmd.Parameters.AddWithValue("@userYear", currentUser.YearLevel);

                conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Scholarship
                        {
                            ScholarshipID = Convert.ToInt32(reader["scholarshipId"]),
                            Name = reader["name"].ToString(),
                            Provider = reader["provider"].ToString(),
                            Description = reader["description"].ToString(),
                            RequiredCourse = reader["required_course"].ToString(),
                            MaxYearLevel = Convert.ToInt32(reader["max_year_level"]),
                            AvailableSlots = Convert.ToInt32(reader["available_slots"]),
                            Deadline = Convert.ToDateTime(reader["deadline"]),
                            MinimumGPA = Convert.ToDecimal(reader["minimum_gpa"])
                        });
                    }
                }
            }
            return list;
        }
    }
}
