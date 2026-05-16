using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace FinalProjectV2
{
    internal class ScholarApplicationADO
    {
        public bool SubmitApplication(string userId, int scholarshipId)
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                string sql = @"
                    INSERT INTO application
                    (
                        userid,
                        scholarshipId,
                        date_submitted,
                        status,
                        remarks
                    )
                    VALUES
                    (
                        @uid,
                        @sid,
                        CURDATE(),
                        'Pending',
                        'Your application is being reviewed.'
                    )";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@uid", userId);
                    cmd.Parameters.AddWithValue("@sid", scholarshipId);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<ScholarApplication> GetUserApplications(User user)
        {
            List<ScholarApplication> list = new List<ScholarApplication>();

            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                string sql = @"
                    SELECT * FROM application a
                    WHERE a.userid = @uid
                    ORDER BY a.date_submitted DESC, a.applicationid DESC";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@uid", user.UserID);
                    conn.Open();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new ScholarApplication
                            {
                                ApplicationId = GetInt32(reader, "applicationId"),
                                ScholarshipId = GetInt32(reader, "scholarshipId"),
                                UserId = GetString(reader, "userid"),
                                Status = GetString(reader, "status"),
                                DateApplied = GetDateTime(reader, "date_submitted"),
                                ApplicationLetterOrCvPath = GetString(reader, "cvpath"),
                                StudyLoadPath = GetString(reader, "studyloadpath"),
                                GoodMoralPath = GetString(reader, "goodmoralpath"),
                                PreviousGradesPath = GetString(reader, "previousgradespath"),
                                ParentTaxReturnsPath = GetString(reader, "taxreturnpath")
                            });
                        }
                    }
                }
            }

            ScholarApplication acceptedApp = null;
            foreach (var app in list)
            {
                if (string.Equals(app.Status, "Accepted", StringComparison.OrdinalIgnoreCase))
                {
                    acceptedApp = app;
                    break;
                }
            }

            if (acceptedApp != null)
            {
                list.Clear();
                list.Add(acceptedApp);
            }

            return list;
        }

        public bool UpdateApplicationStatus(int applicationId, string status)
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                string remarks;

                switch (status)
                {
                    case "Accepted":
                        remarks = "You accepted the approved scholarship application.";
                        break;
                    case "Approved":
                        remarks = "Your application has been approved.";
                        break;
                    case "Rejected":
                        remarks = "Your application has been rejected.";
                        break;
                    case "Cancelled":
                        remarks = "You cancelled your pending application.";
                        break;
                    default:
                        remarks = "Application status updated.";
                        break;
                }

                string sql = @"
                    UPDATE application
                    SET
                        status = @status,
                        remarks = @remarks
                    WHERE applicationid = @applicationId";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@remarks", remarks);
                    cmd.Parameters.AddWithValue("@applicationId", applicationId);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        private int GetInt32(MySqlDataReader reader, string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);

            if (reader.IsDBNull(ordinal))
            {
                return 0;
            }

            return Convert.ToInt32(reader.GetValue(ordinal));
        }

        private DateTime GetDateTime(MySqlDataReader reader, string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);

            if (reader.IsDBNull(ordinal))
            {
                return DateTime.MinValue;
            }

            return Convert.ToDateTime(reader.GetValue(ordinal));
        }

        private string GetString(MySqlDataReader reader, string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);

            if (reader.IsDBNull(ordinal))
            {
                return string.Empty;
            }

            return reader.GetValue(ordinal).ToString();
        }
    }
}