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
                    SELECT
                        a.applicationid AS ApplicationId,
                        a.scholarshipId AS ScholarshipId,
                        a.userid AS UserId,
                        a.status AS Status,
                        a.date_submitted AS DateApplied,
                        a.cvpath AS ApplicationLetterOrCvPath,
                        a.studyloadpath AS StudyLoadPath,
                        a.goodmoralpath AS GoodMoralPath,
                        a.previousgradespath AS PreviousGradesPath,
                        a.taxreturnpath AS ParentTaxReturnsPath
                    FROM application a
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
                                ApplicationId = GetInt32(reader, "ApplicationId"),
                                ScholarshipId = GetInt32(reader, "ScholarshipId"),
                                UserId = GetString(reader, "UserId"),
                                Status = GetString(reader, "Status"),
                                DateApplied = GetDateTime(reader, "DateApplied"),
                                ApplicationLetterOrCvPath = GetString(reader, "ApplicationLetterOrCvPath"),
                                StudyLoadPath = GetString(reader, "StudyLoadPath"),
                                GoodMoralPath = GetString(reader, "GoodMoralPath"),
                                PreviousGradesPath = GetString(reader, "PreviousGradesPath"),
                                ParentTaxReturnsPath = GetString(reader, "ParentTaxReturnsPath")
                            });
                        }
                    }
                }
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