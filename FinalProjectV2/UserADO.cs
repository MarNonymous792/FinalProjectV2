using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace FinalProjectV2
{
    internal class UserADO
    {
        public bool RegisterUser(string id, string user, string pass, string fname, string lname, string mname, string email, string course, int year)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(pass);

            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                string sql = "INSERT INTO users (userid, username, password, fname, lname, mname, email, course, yearlevel) " +
                             "VALUES (@id, @user, @pass, @fn, @ln, @mname, @email, @course, @yearlevel)";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@user", user);
                cmd.Parameters.AddWithValue("@pass", hashedPassword); // Pass the hashed string here
                cmd.Parameters.AddWithValue("@fn", fname);
                cmd.Parameters.AddWithValue("@ln", lname);
                cmd.Parameters.AddWithValue("@mname", mname);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@course", course);

                cmd.Parameters.AddWithValue("@yearlevel", year);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }      
        }

        public User Login(string inputUsername, string inputPassword)
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            { 
                string sql = "SELECT * FROM users WHERE username = @username";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@username", inputUsername);

                conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string storedHash = reader["password"].ToString();

                        if (BCrypt.Net.BCrypt.Verify(inputPassword, storedHash))
                        {
                            
                            return new User
                            {
                                UserID = reader["userid"].ToString(), 
                                Username = reader["username"].ToString(),
                                FirstName = reader["fname"].ToString(),
                                LastName = reader["lname"].ToString(),
                                MiddleName = reader["mname"].ToString(),
                                Email = reader["email"].ToString(),
                                Course = reader["course"].ToString(),
                                YearLevel = Convert.ToInt32(reader["yearlevel"]),
                                
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}
