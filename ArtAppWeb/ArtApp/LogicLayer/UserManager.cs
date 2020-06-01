using DataAccessLayer;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class UserManager : IUserManager
    {
        private IUserAccessor _userAccessor;

        public UserManager()
        {
            _userAccessor = new UserAccessor();
        }

        public UserManager(UserAccessor userAccessor)
        {
            _userAccessor = userAccessor;
        }

        public User AuthenticateUser(string email, string password)
        {
            User result = null;

            // we need to hash the password
            var passwordHash = HashPassword(password);
            password = null;

            try
            {
                result = _userAccessor.AuthenticateUser(email, passwordHash);
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Login failed!", ex);
            }


            return result;
        }

        public List<User> RetrieveUserListByActive(bool active = true)
        {
            try
            {
                return _userAccessor.SelectUsersByActive(active);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Data not found. ", ex);
            }
        }

        public bool UpdatePassword(int userID, string newPassword, string oldPassword)
        {
            bool isUpdated = false;

            string newPasswordHash = HashPassword(newPassword);
            string oldPasswordHash = HashPassword(oldPassword);

            try
            {
                isUpdated = _userAccessor.UpdatePasswordHash(userID, oldPasswordHash, newPasswordHash);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Update Failed", ex);
            }

            return isUpdated;
        }

        private string HashPassword(string source)
        {
            // Use SHA256
            string result;

            // We need a byte array because cryptography is bits and bytes
            byte[] data;

            // create a hash provider object
            using (SHA256 sha256hash = SHA256.Create())
            {
                //hash the input
                data = sha256hash.ComputeHash(Encoding.UTF8.GetBytes(source));
            }
            // Build a string from the result
            var s = new StringBuilder();

            // Loop through bytes to build the damn string
            for (int i = 0; i < data.Length; i++)
            {
                s.Append(data[i].ToString("x2"));
            }

            result = s.ToString().ToUpper();

            return result;

        }

        public bool AddUser(User user)
        {
            bool result = false;
            try
            {
                result = _userAccessor.InsertUser(user) > 0;
            }
            catch (Exception ex)
            {

                throw new ApplicationException("User Not Added", ex);
            }


            return result;
        }


        public bool EditUser(User oldUser, User newUser)
        {

            bool result = false;

            try
            {
                result = _userAccessor.UpdateUser(oldUser, newUser) == 1;
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Update failed", ex);
            }

            return result;
        }

        public bool SetUserActiveState(bool active, int userID)
        {
            bool result = false;

            try
            {
                if (active)
                {
                    // 1 is the rows effected

                    result = 1 == _userAccessor.ActivateUser(userID);
                }
                else
                {
                    result = 1 == _userAccessor.DeactivateUser(userID);
                }
                if (result == false)
                {
                    throw new ApplicationException("User record not updated");
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Update failed.", ex);
            }

            return result;
        }

        public List<string> RetrieveUserRoles(int userID)
        {
            List<string> roles = null;

            try
            {
                roles = _userAccessor.SelectRolesByUserID(userID);
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Roles not found.", ex);
            }

            return roles;
        }

        public List<string> RetrieveUserRoles()
        {
            List<string> roles = null;

            try
            {
                roles = _userAccessor.SelectAllRoles();
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Roles not found.", ex);
            }

            return roles;
        }

        public bool DeleteUserRole(int userID, string role)
        {
        
            bool result = false;

            try
            {
                // One on the left side to better clarify that a comparison is going on here.
                // Third variable is a named parameter. Not nessecary, but good for clarity
                result = (1 == _userAccessor.InsertOrDeleteUserRole(userID, role, delete: true));
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Role not removed.", ex);
            }

            return result;
        
        }

        public bool AddUserRole(int userID, string role)
        {

            bool result = false;

            try
            {
                // One on the left side to better clarify that a comparison is going on here.
                result = (1 == _userAccessor.InsertOrDeleteUserRole(userID, role));
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Role not removed.", ex);
            }

            return result;

        }

        public List<string> RetrieveUserSkills(int userID)
        {
        
           List<string> roles = null;

           try
           {
               roles = _userAccessor.SelectSkillsByUserId(userID);
           }
           catch (Exception ex)
           {

               throw new ApplicationException("Roles not found.", ex);
           }

           return roles;
        
        }

        public List<string> RetrieveUserSkills()
        {

            List<string> roles = null;

            try
            {
                roles = _userAccessor.SelectAllSkills();
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Roles not found.", ex);
            }

            return roles;

        }

        public bool DeleteUserSkill(int userID, string skill)
        {
            {
                bool result = false;

                try
                {
                    // One on the left side to better clarify that a comparison is going on here.
                    result = (1 == _userAccessor.InsertOrDeleteUserSkill(userID, skill, delete: true));
                }
                catch (Exception ex)
                {

                    throw new ApplicationException("Role not removed.", ex);
                }

                return result;

            }
        }

        public bool AddUserSkill(int userID, string skill)
        {
           bool result = false;

           try
           {
               // One on the left side to better clarify that a comparison is going on here.
               result = (1 == _userAccessor.InsertOrDeleteUserSkill(userID, skill));
           }
           catch (Exception ex)
           {

               throw new ApplicationException("Role not removed.", ex);
           }

           return result;

        }

        public List<int> RetrieveUserIDsByRole(string role)
        {
            List<int> users;

            try
            {
                users = _userAccessor.SelectUserIDsByRole(role);
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Roles not found.", ex);
            }

            return users;
        }

        public User RetrieveUserByID(int id)
        {
            User user;

            try
            {
                user = _userAccessor.SelectUserByID(id);
            }
            catch(Exception ex)
            {
                throw new ApplicationException("User not found.", ex);
            }

            return user;
        }

        public bool FindUser(string email)
        {
            try
            {
                return _userAccessor.GetUserByEmail(email) != null;
            }
            catch (ApplicationException ax)
            {
                if (ax.Message == "User not found.")
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Database error.", ex);
            }
        }

        public int RetrieveUserIDFromEmail(string email)
        {
            try
            {
                return _userAccessor.GetUserByEmail(email).UserID;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Database Error", ex);
            }
        }
    }
}
