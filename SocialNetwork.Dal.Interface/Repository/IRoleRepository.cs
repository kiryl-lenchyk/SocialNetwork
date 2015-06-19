using System;
using System.Collections.Generic;
using SocialNetwork.Dal.Interface.DTO;

namespace SocialNetwork.Dal.Interface.Repository
{
    /// <summary>
    /// Represent storage of DalRole.
    /// </summary>
    public interface IRoleRepository : IRepository<DalRole>
    {
        /// <summary>
        /// Get role by name, or null if role not found.
        /// </summary>
        /// <param name="roleName">role name for search</param>
        /// <returns>founded role or null if it's not found</returns>
        DalRole GetByName(String roleName);

        /// <summary>
        /// Get IEnumerable of roles for one user.
        /// </summary>
        /// <param name="user">user to find roles.</param>
        /// <returns>IEnumerable of roles</returns>
        IEnumerable<DalRole> GetUserRoles(DalUser user);

        /// <summary>
        /// Get IEnumarable of user for one role.
        /// </summary>
        /// <param name="role">role to find users.</param>
        /// <returns>IEnumerable of users</returns>
        IEnumerable<DalUser> GetRoleUsers(DalRole role);

        /// <summary>
        /// Add role to user's roles list.
        /// </summary>
        /// <param name="user">user to add role.</param>
        /// <param name="role">new user role.</param>
        void AddUserToRole(DalUser user, DalRole role);

        /// <summary>
        /// Remove role from user's roles list.
        /// </summary>
        /// <param name="user">user to remove role.</param>
        /// <param name="role">user role to remove.</param>
        void RemoveUserFromRole(DalUser user, DalRole role);
    }
}