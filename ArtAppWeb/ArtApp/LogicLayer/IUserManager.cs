using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public interface IUserManager
    {
        User AuthenticateUser(string email, string password);

        bool UpdatePassword(int userID, string newPassword, string oldPassword);

        List<User> RetrieveUserListByActive(bool active = true);

        List<int> RetrieveUserIDsByRole(string role);

        bool EditUser(User oldUser, User newUser);

        bool AddUser(User user);

        bool SetUserActiveState(bool active, int userID);

        List<string> RetrieveUserRoles(int userID);

        List<string> RetrieveUserRoles();

        bool DeleteUserRole(int userID, string role);

        User RetrieveUserByID(int id);

        bool AddUserRole(int userID, string role);

        List<string> RetrieveUserSkills(int userID);

        List<string> RetrieveUserSkills();

        bool DeleteUserSkill(int userID, string skill);

        bool AddUserSkill(int userID, string skill);

        bool FindUser(string email);

        int RetrieveUserIDFromEmail(string email);
    }
}
