using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectV2
{
    internal class UserADO
    {
        public bool RegisterUser(string id, string user, string pass, string fname, string lname, decimal gpa)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(pass);

            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                string sql = "INSERT INTO users (userid, username, password, fname, lname, gpa, status) " +
                             "VALUES (@id, @user, @pass, @fn, @ln, @gpa, 'Active')"; //!!!!incomplete, not correct data to be inserted!!!1

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@user", user);
                cmd.Parameters.AddWithValue("@pass", hashedPassword); // Pass the hashed string here
                cmd.Parameters.AddWithValue("@fn", fname);
                cmd.Parameters.AddWithValue("@ln", lname);
                cmd.Parameters.AddWithValue("@gpa", gpa);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
    }
        }
}
