// File: ScholarshipADO.cs
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace FinalProjectV2
{
    internal class ScholarshipADO
    {
        public bool CreateScholarship(Scholarship scholarship)
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                string sql = @"
                    INSERT INTO scholarship
                    (
                        name,
                        provider,
                        description,
                        required_course,
                        max_year_level,
                        deadline,
                        available_slots
                    )
                    VALUES
                    (
                        @name,
                        @provider,
                        @description,
                        @requiredCourse,
                        @maxYearLevel,
                        @deadline,
                        @availableSlots
                    )";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@name", scholarship.Name);
                    cmd.Parameters.AddWithValue("@provider", scholarship.Provider);
                    cmd.Parameters.AddWithValue("@description", scholarship.Description);
                    cmd.Parameters.AddWithValue("@requiredCourse", scholarship.RequiredCourse);
                    cmd.Parameters.AddWithValue("@maxYearLevel", scholarship.MaxYearLevel);
                    cmd.Parameters.AddWithValue("@deadline", scholarship.Deadline);
                    cmd.Parameters.AddWithValue("@availableSlots", scholarship.AvailableSlots);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<Scholarship> GetSuitableScholarships(User user)
        {
            List<Scholarship> scholarships = new List<Scholarship>();

            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                string sql = @"
                    SELECT
                        scholarshipId,
                        name,
                        provider,
                        description,
                        required_course,
                        max_year_level,
                        deadline,
                        available_slots
                    FROM scholarship
                    ORDER BY deadline ASC, name ASC";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    conn.Open();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            scholarships.Add(new Scholarship
                            {
                                ScholarshipId = GetInt32(reader, "scholarshipId"),
                                Name = GetString(reader, "name"),
                                Provider = GetString(reader, "provider"),
                                Description = GetString(reader, "description"),
                                RequiredCourse = GetString(reader, "required_course"),
                                MaxYearLevel = GetInt32(reader, "max_year_level"),
                                Deadline = GetDateTime(reader, "deadline"),
                                AvailableSlots = GetInt32(reader, "available_slots")
                            });
                        }
                    }
                }
            }

            return scholarships;
        }

        public Scholarship GetScholarshipById(int scholarshipId)
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                string sql = @"
                    SELECT
                        scholarshipId,
                        name,
                        provider,
                        description,
                        required_course,
                        max_year_level,
                        deadline,
                        available_slots
                    FROM scholarship
                    WHERE scholarshipId = @scholarshipId
                    LIMIT 1";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@scholarshipId", scholarshipId);
                    conn.Open();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            return null;
                        }

                        return new Scholarship
                        {
                            ScholarshipId = GetInt32(reader, "scholarshipId"),
                            Name = GetString(reader, "name"),
                            Provider = GetString(reader, "provider"),
                            Description = GetString(reader, "description"),
                            RequiredCourse = GetString(reader, "required_course"),
                            MaxYearLevel = GetInt32(reader, "max_year_level"),
                            Deadline = GetDateTime(reader, "deadline"),
                            AvailableSlots = GetInt32(reader, "available_slots")
                        };
                    }
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