using System;
using System.EnterpriseServices;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using SocialNetwork.Bll.Interface.Entity;
using SocialNetwork.Bll.Interface.Services;

namespace WebUi.Providers
{
    public class SocialNetworkRoleProvider : RoleProvider
    {

        private IRoleService roleService;

        public override string ApplicationName { get; set; }

        public override bool IsUserInRole(string username, string roleName)
        {
            roleService =
                (IRoleService) DependencyResolver.Current.GetService(typeof (IRoleService));

            return roleService.IsUserInRole(username, roleName);

        }

        public override string[] GetRolesForUser(string username)
        {
            roleService =
                (IRoleService) DependencyResolver.Current.GetService(typeof (IRoleService));

            return roleService.GetUserRoles(username).Select(x => x.Name).ToArray();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
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

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
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

        public override string[] GetUsersInRole(string roleName)
        {
            roleService =
                (IRoleService)DependencyResolver.Current.GetService(typeof(IRoleService));

            return roleService.GetUsersInRole(roleName).Select(x => x.Name).ToArray();
        }

        public override string[] GetAllRoles()
        {
            roleService =
                (IRoleService)DependencyResolver.Current.GetService(typeof(IRoleService));

            return roleService.GetAllRoles().Select(x => x.Name).ToArray();

        }

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