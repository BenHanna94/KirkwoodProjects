using DataObjects;
using System.Collections.Generic;

namespace DataAccessLayer
{
    public interface IUserAccessor
    {
        User AuthenticateUser(string username, string passwordHash);

        // interface methods are semicolons
        bool UpdatePasswordHash(int userID, string oldpasswordHash, string newPasswordHash);

        List<User> SelectUsersByActive(bool active = true);

        List<int> SelectUserIDsByRole(string role);

        int UpdateUser(User oldUser, User newUser);

        int InsertUser(User user);

        int ActivateUser(int userID);

        int DeactivateUser(int userID);

        List<string> SelectAllRoles();

        List<string> SelectRolesByUserID(int userID);

        int InsertOrDeleteUserRole(int userID, string role, bool delete = false);

        List<string> SelectAllSkills();

        List<string> SelectSkillsByUserId(int userID);

        int InsertOrDeleteUserSkill(int userID, string skill, bool delete = false);

        User SelectUserByID(int id);

        User GetUserByEmail(string email);
    }
}