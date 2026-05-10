using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectV2
{
    internal class ScholarApplicationADO
    {
        public bool SubmitApplication(string userId, int scholarshipId)
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                string sql = @"INSERT INTO application (userid, scholarshipId, date_submitted, status, remarks) 
                       VALUES (@uid, @sid, CURDATE(), 'Pending', 'Your application is being reviewed.')";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@uid", userId);
                cmd.Parameters.AddWithValue("@sid", scholarshipId);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public List<ScholarApplication> GetUserApplications(string userId)
        {
            List<ScholarApplication> list = new List<ScholarApplication>();
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                string sql = @"SELECT a.*, s.name, s.provider 
                       FROM application a 
                       JOIN scholarship s ON a.scholarshipId = s.scholarshipId 
                       WHERE a.userid = @uid";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@uid", userId);

                conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new ScholarApplication
                        {
                            ApplicationID = Convert.ToInt32(reader["applicationid"]),
                            Status = reader["status"].ToString(),
                            DateSubmitted = Convert.ToDateTime(reader["date_submitted"]),
                            Remarks = reader["remarks"].ToString(),
                            ScholarshipName = reader["name"].ToString(),
                            Provider = reader["provider"].ToString()
                        });
                    }
                }
            }
            return list;
        }
    }
}
