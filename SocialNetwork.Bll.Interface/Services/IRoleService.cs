using System.Collections.Generic;
using SocialNetwork.Bll.Interface.Entity;

namespace SocialNetwork.Bll.Interface.Services
{
    /// <summary>
    /// Represent business logic function for Role. 
    /// </summary>
    public interface IRoleService
    {

        /// <summary>
        /// Return true if user with username is in role with roleName.
        /// </summary>
        /// <param name="username">username of user.</param>
        /// <param name="roleName">name of role.</param>
        /// <returns>true if user with username is in role with roleName.</returns>
        bool IsUserInRole(string username, string roleName);

        /// <summary>
        /// Get all roles for user.
        /// </summary>
        /// <param name="username">username for user.</param>
        /// <returns>IEnumerable of all user's roles.</returns>
        IEnumerable<BllRole> GetUserRoles(string username);

        /// <summary>
        /// Add role to user's roles.
        /// </summary>
        /// <param name="username">username of user.</param>
        /// <param name="roleName">role name.</param>
        void AddUserInRole(string username, string roleName);

        /// <summary>
        /// Remove role from user's roles list.
        /// </summary>
        /// <param name="username">username of user.</param>
        /// <param name="roleName">role name.</param>
        void RemoveUserFromRole(string username, string roleName);

        /// <summary>
        /// Set for user new list of roles.
        /// </summary>
        /// <param name="username">username of user.</param>
        /// <param name="rolesIds">collection of new user's roles.</param>
        void UpdateUserRoles(string username, IEnumerable<int> rolesIds);

        /// <summary>
        /// Get all users in role.
        /// </summary>
        /// <param name="roleName">role name</param>
        /// <returns>IEnumerable of all users in role.</returns>
        IEnumerable<BllUser> GetUsersInRole(string roleName);

        /// <summary>
        /// Get all roles from storage.
        /// </summary>
        /// <returns>IEnumerable of all roles.</returns>
        IEnumerable<BllRole> GetAllRoles();
    }
}
