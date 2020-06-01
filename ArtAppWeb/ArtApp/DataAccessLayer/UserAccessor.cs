using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DataObjects;

namespace DataAccessLayer
{
    public class UserAccessor : IUserAccessor
    {
        public User AuthenticateUser(string username, string passwordHash)
        {
            User result = null; 

            // first get a connectrion. Opens a connection so we can communicate with database
            var conn = DBConnection.GetConnection();

            // next we need a command object. This holds the stored procedure and passes it to the connection
            var cmd = new SqlCommand("sp_authenticate_user", conn);

            // set the command type
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameters for the procedure
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 250);
            cmd.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 100);

            // Set values for the parameters
            cmd.Parameters["@Email"].Value = username;
            cmd.Parameters["@PasswordHash"].Value = passwordHash;

            // now that the command is set up, we can execute it.
            try
            {
                // open connection
                conn.Open();

                // Execute command
                if (1 == Convert.ToInt32(cmd.ExecuteScalar()))
                {
                    // if command worked correctly, get the user object
                    result = GetUserByEmail(username);
                }
                else
                {
                    throw new ApplicationException("User not found.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }


        public List<User> SelectUsersByActive(bool active = true)
        {

            List<User> users = new List<User>();

            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_select_users_by_active");
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@Active", SqlDbType.Bit);
            cmd.Parameters["@Active"].Value = active;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var user = new User();
                        user.UserID = reader.GetInt32(0);
                        user.Name = reader.GetString(1);
                        user.PhoneNumber = reader.GetString(2);
                        user.Email = reader.GetString(3);
                        user.Active = reader.GetBoolean(4);

                        users.Add(user);
                    }
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }


            return users;
        }

        public bool UpdatePasswordHash(int userID, string oldPasswordHash, string newPasswordHash)
        {
            bool updateSuccess = false;

            // connecttion
            var conn = DBConnection.GetConnection();

            // cmd
            var cmd = new SqlCommand("sp_update_password");
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            //parameters
            cmd.Parameters.Add("@UserID", SqlDbType.Int);
            cmd.Parameters.Add("@OldPasswordHash", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@NewPasswordHash", SqlDbType.NVarChar, 100);

            // values
            cmd.Parameters["@UserID"].Value = userID;
            cmd.Parameters["@OldPasswordHash"].Value = oldPasswordHash;
            cmd.Parameters["@NewPasswordHash"].Value = newPasswordHash;

            // execute the command
            try
            {
                conn.Open();
                int rows = cmd.ExecuteNonQuery();

                updateSuccess = (rows == 1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return updateSuccess;
        }


        public User GetUserByEmail(string email)
        {
            User user = new User();

            // connection
            var conn = DBConnection.GetConnection();

            // Command objects (2)
            var cmd1 = new SqlCommand("sp_select_user_by_email");
            var cmd2 = new SqlCommand("sp_select_roles_by_userID");

            cmd1.Connection = conn;
            cmd2.Connection = conn;

            cmd1.CommandType = CommandType.StoredProcedure;
            cmd2.CommandType = CommandType.StoredProcedure;

            // parameters
            cmd1.Parameters.Add("@Email", SqlDbType.NVarChar, 250);
            cmd1.Parameters["@Email"].Value = email;

            cmd2.Parameters.Add("@UserID", SqlDbType.Int);
            // We can't set the values of this parameter yet

            try
            {
                // open connection
                conn.Open();

                // execute the first command
                var reader1 = cmd1.ExecuteReader();

                if (reader1.Read())
                {
                    user.UserID = reader1.GetInt32(0);
                    user.Name = reader1.GetString(1);
                    user.PhoneNumber = reader1.GetString(2);
                    user.Email = email;

                }
                else
                {
                    throw new ApplicationException("User not found.");
                }
                reader1.Close(); // no longer needed
                cmd2.Parameters["@UserID"].Value = user.UserID;

                var reader2 = cmd2.ExecuteReader();


                List<string> roles = new List<string>();
                while (reader2.Read())
                {
                    string role = reader2.GetString(0);
                    roles.Add(role);
                }
                user.Roles = roles;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return user;
        }

        public int InsertUser(User user)
        {
            int emlpoyeeID = 0;

            // connecttion
            var conn = DBConnection.GetConnection();

            // cmd
            var cmd = new SqlCommand("sp_insert_user");
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            //Automatically assumes the value is an int32. Decimal and money are more ambiguous

            cmd.Parameters.AddWithValue("@Name", user.Name);
            cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
            cmd.Parameters.AddWithValue("@Email", user.Email);

            try
            {
                conn.Open();
                emlpoyeeID = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return emlpoyeeID;
        }

        public int UpdateUser(User oldUser, User newUser)
        {
            int rows = 0;

            // connecttion
            var conn = DBConnection.GetConnection();

            // cmd
            var cmd = new SqlCommand("sp_update_user");
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            //Automatically assumes the value is an int32. Decimal and money are more ambiguous
            cmd.Parameters.AddWithValue("@UserID", oldUser.UserID);

            cmd.Parameters.AddWithValue("@NewName", newUser.Name);
            cmd.Parameters.AddWithValue("@NewPhoneNumber", newUser.PhoneNumber);
            cmd.Parameters.AddWithValue("@NewEmail", newUser.Email);

            cmd.Parameters.AddWithValue("@OldName", oldUser.Name);
            cmd.Parameters.AddWithValue("@OldPhoneNumber", oldUser.PhoneNumber);
            cmd.Parameters.AddWithValue("@OldEmail", oldUser.Email);

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return rows;
        }

        public int ActivateUser(int userID)
        {
            int rows = 0;

            // connecttion
            var conn = DBConnection.GetConnection();

            // cmd
            var cmd = new SqlCommand("sp_reactivate_user");
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserID", userID);

            try
            {
                conn.Open();

                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return rows;
        }

        public int DeactivateUser(int userID)
        {
            int rows = 0;

            // connecttion
            var conn = DBConnection.GetConnection();

            // cmd
            var cmd = new SqlCommand("sp_deactivate_user");
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserID", userID);

            try
            {
                conn.Open();

                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return rows;
        }

        public List<string> SelectAllRoles()
        {

            List<string> roles = new List<string>();

            // connection
            var conn = DBConnection.GetConnection();

            // Command objects (2)
            var cmd = new SqlCommand("sp_select_all_roles");

            cmd.Connection = conn;

            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                // open connection
                conn.Open();

                // execute the first command


                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string role = reader.GetString(0);
                    roles.Add(role);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return roles;

        }

        public List<string> SelectRolesByUserID(int userID)
        {

            List<string> roles = new List<string>();

            // connection
            var conn = DBConnection.GetConnection();

            // Command objects (2)
            var cmd = new SqlCommand("[sp_select_roles_by_userID]");

            cmd.Connection = conn;

            cmd.CommandType = CommandType.StoredProcedure;

            // parameters

            cmd.Parameters.Add("@UserID", SqlDbType.Int);
            cmd.Parameters["@UserID"].Value = userID;

            try
            {
                // open connection
                conn.Open();
                


                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string role = reader.GetString(0);
                    roles.Add(role);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return roles;

        }

        public int InsertOrDeleteUserRole(int userID, string role, bool delete = false)
        {

            int rows = 0;

            var conn = DBConnection.GetConnection();

            string cmdTxt = delete ? "sp_delete_user_role" : "sp_insert_user_role";

            var cmd = new SqlCommand(cmdTxt, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@RoleID", role);

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return rows;
        }
    

        public List<string> SelectAllSkills()
        {
            {

                List<string> skills = new List<string>();

                // connection
                var conn = DBConnection.GetConnection();

                // Command objects (2)
                var cmd = new SqlCommand("sp_select_all_skills");

                cmd.Connection = conn;

                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    // open connection
                    conn.Open();

                    // execute the first command


                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string skill = reader.GetString(0);
                        skills.Add(skill);
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return skills;

            }
        }

        public List<string> SelectSkillsByUserId(int userID)
        {
            List<string> skills = new List<string>();

            // connection
            var conn = DBConnection.GetConnection();

            // Command objects (2)
            var cmd = new SqlCommand("[sp_select_skills_by_userID]");

            cmd.Connection = conn;

            cmd.CommandType = CommandType.StoredProcedure;

            // parameters

            cmd.Parameters.Add("@UserID", SqlDbType.Int);
            cmd.Parameters["@UserID"].Value = userID;

            try
            {
                // open connection
                conn.Open();



                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string skill = reader.GetString(0);
                    skills.Add(skill);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return skills;
        }

        public int InsertOrDeleteUserSkill(int userID, string skill, bool delete = false)
        {
            int rows = 0;

            var conn = DBConnection.GetConnection();

            string cmdTxt = delete ? "sp_delete_user_skill" : "sp_insert_user_skill";

            var cmd = new SqlCommand(cmdTxt, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@SkillID", skill);

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return rows;
        }

        public List<int> SelectUserIDsByRole(string role)
        {
            {

                List<int> users = new List<int>();

                var conn = DBConnection.GetConnection();
                var cmd = new SqlCommand("sp_select_users_by_roleID");
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                
                cmd.Parameters.AddWithValue("@RoleID", role);

                try
                {
                    conn.Open();
                    var reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            
                            int userID = reader.GetInt32(0);

                            users.Add(userID);
                        }
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }


                return users;
            }
        }

        public User SelectUserByID(int id)
        {
            User user = null;

            // first get a connectrion. Opens a connection so we can communicate with database
            var conn = DBConnection.GetConnection();

            // next we need a command object. This holds the stored procedure and passes it to the connection
            var cmd = new SqlCommand("sp_select_user_by_id", conn);

            // set the command type
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameters for the procedure
            cmd.Parameters.AddWithValue("@UserID", id);

            // now that the command is set up, we can execute it.
            try
            {
                // open connection
                conn.Open();

                var reader = cmd.ExecuteReader();

                // Execute command
                if (reader.HasRows)
                {
                    reader.Read();

                    user = new User()
                    {
                        UserID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        PhoneNumber = reader.GetString(2),
                        Email = reader.GetString(3),
                        Active = reader.GetBoolean(4)
                    };

                }
                else
                {
                    throw new ApplicationException("User not found.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return user;
        }
    }
}
