using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using SocialNetwork.Bll.Interface.Services;
using SocialNetwork.Logger.Interface;

namespace WebUi.Providers
{
    /// <summary>
    /// RoleProvider for SocialNetwork
    /// </summary>
    public class SocialNetworkRoleProvider : RoleProvider
    {
        #region Fields
        private ILogger logger;
        private IRoleService roleService;

        #endregion

        #region Realised Members

        /// <summary>
        /// Gets or sets the name of the application to store and retrieve role information for.
        /// </summary>
        /// <returns>
        /// The name of the application to store and retrieve role information for.
        /// </returns>
        public override string ApplicationName { get; set; }

        /// <summary>
        /// Gets a value indicating whether the specified user is in the specified role for the configured applicationName.
        /// </summary>
        /// <returns>
        /// true if the specified user is in the specified role for the configured applicationName; otherwise, false.
        /// </returns>
        /// <param name="username">The user name to search for.</param><param name="roleName">The role to search in.</param>
        public override bool IsUserInRole(string username, string roleName)
        {
            logger = (ILogger)DependencyResolver.Current.GetService(typeof(ILogger));
            roleService =
                (IRoleService) DependencyResolver.Current.GetService(typeof (IRoleService));

            if (roleService.IsUserInRole(username, roleName)) return true;
            logger.Log(LogLevel.Trace,"User in role return false. Username = {0} Role = {1}",username, roleName);
            return false;

        }

        /// <summary>
        /// Gets a list of the roles that a specified user is in for the configured applicationName.
        /// </summary>
        /// <returns>
        /// A string array containing the names of all the roles that the specified user is in for the configured applicationName.
        /// </returns>
        /// <param name="username">The user to return a list of roles for.</param>
        public override string[] GetRolesForUser(string username)
        {
            logger = (ILogger)DependencyResolver.Current.GetService(typeof(ILogger));
            roleService =
                (IRoleService) DependencyResolver.Current.GetService(typeof (IRoleService));

            return roleService.GetUserRoles(username).Select(x => x.Name).ToArray();
        }

        /// <summary>
        /// Adds the specified user names to the specified roles for the configured applicationName.
        /// </summary>
        /// <param name="usernames">A string array of user names to be added to the specified roles. </param><param name="roleNames">A string array of the role names to add the specified user names to.</param>
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            logger = (ILogger)DependencyResolver.Current.GetService(typeof(ILogger));
            roleService =
                (IRoleService)DependencyResolver.Current.GetService(typeof(IRoleService));

            foreach (string username in usernames)
            {
                foreach (string roleName in roleNames)
                {
                    roleService.AddUserInRole(username, roleName);
                }
            }
        }

        /// <summary>
        /// Removes the specified user names from the specified roles for the configured applicationName.
        /// </summary>
        /// <param name="usernames">A string array of user names to be removed from the specified roles. </param><param name="roleNames">A string array of role names to remove the specified user names from.</param>
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            logger = (ILogger)DependencyResolver.Current.GetService(typeof(ILogger));
            roleService =
                (IRoleService)DependencyResolver.Current.GetService(typeof(IRoleService));

            foreach (string username in usernames)
            {
                foreach (string roleName in roleNames)
                {
                    roleService.RemoveUserFromRole(username, roleName);
                }
            }
        }

        /// <summary>
        /// Gets a list of users in the specified role for the configured applicationName.
        /// </summary>
        /// <returns>
        /// A string array containing the names of all the users who are members of the specified role for the configured applicationName.
        /// </returns>
        /// <param name="roleName">The name of the role to get the list of users for.</param>
        public override string[] GetUsersInRole(string roleName)
        {
            logger = (ILogger)DependencyResolver.Current.GetService(typeof(ILogger));
            roleService =
                (IRoleService)DependencyResolver.Current.GetService(typeof(IRoleService));

            return roleService.GetUsersInRole(roleName).Select(x => x.Name).ToArray();
        }

        /// <summary>
        /// Gets a list of all the roles for the configured applicationName.
        /// </summary>
        /// <returns>
        /// A string array containing the names of all the roles stored in the data source for the configured applicationName.
        /// </returns>
        public override string[] GetAllRoles()
        {
            logger = (ILogger)DependencyResolver.Current.GetService(typeof(ILogger));
            roleService =
                (IRoleService)DependencyResolver.Current.GetService(typeof(IRoleService));

            return roleService.GetAllRoles().Select(x => x.Name).ToArray();

        }

        #endregion

        #region NotImplemented

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}