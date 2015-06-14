using System;
using System.Collections.Generic;
using System.Linq;
using SocialNetwork.Bll.Interface.Entity;
using SocialNetwork.Bll.Interface.Services;
using SocialNetwork.Bll.Mappers;
using SocialNetwork.Dal.Interface.DTO;
using SocialNetwork.Dal.Interface.Repository;

namespace SocialNetwork.Bll.Service
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository roleRepository;

        private readonly IUserRepository userRepository;

        public RoleService(IRoleRepository roleRepository, IUserRepository userRepository)
        {
            this.roleRepository = roleRepository;
            this.userRepository = userRepository;
        }

        public bool IsUserInRole(string username, string roleName)
        {
            if (username == null) throw new ArgumentNullException("username");
            if (roleName == null) throw new ArgumentNullException("roleName");
            
            DalUser user = userRepository.GetByName(username);
            if (user == null) throw new ArgumentException(String.Format("User name={0} not found",username));

            return roleRepository.GetUserRoles(user).Count(x => x.Name == roleName) != 0;
        }

        public IEnumerable<BllRole> GetUserRoles(string username)
        {
            if (username == null) throw new ArgumentNullException("username");

            DalUser user = userRepository.GetByName(username);
            if (user == null) throw new ArgumentException(String.Format("User name={0} not found", username));

            return roleRepository.GetUserRoles(user).ToList().Select(x => x.ToBllRole());
        }

        public void AddUserInRole(string username, string roleName)
        {
            if (username == null) throw new ArgumentNullException("username");
            if (roleName == null) throw new ArgumentNullException("roleName");

            DalRole role = roleRepository.GetByName(roleName);
            if (role == null) throw new ArgumentException(String.Format("Role name={0} not found", roleName));
            DalUser user = userRepository.GetByName(username);
            if (user == null) throw new ArgumentException(String.Format("User name={0} not found", username));

            roleRepository.AddUserToRole(user, role);
        }

        public void RemoveUserFromRole(string username, string roleName)
        {
            DalRole role = roleRepository.GetByName(roleName);
            if (role == null) throw new ArgumentException(String.Format("Role name={0} not found", roleName));
            DalUser user = userRepository.GetByName(username);
            if (user == null) throw new ArgumentException(String.Format("User name={0} not found", username));

            roleRepository.RemoveUserFromRole(user, role);
        }

        public void UpdateUserRoles(string username, IEnumerable<int> rolesIds)
        {
            if (username == null) throw new ArgumentNullException("username");
            if (rolesIds == null) throw new ArgumentNullException("rolesIds");

            DalUser user = userRepository.GetByName(username);
            if (user == null) throw new ArgumentException(String.Format("User name={0} not found", username));

            RemoveUserFromRoles(user, rolesIds);
            user = userRepository.GetByName(username);
            AddUserInewRoles(user, rolesIds);
        }

        public IEnumerable<BllUser> GetUsersInRole(string roleName)
        {
            if (roleName == null) throw new ArgumentNullException("roleName");

            DalRole role = roleRepository.GetByName(roleName);
            if (role == null) throw new ArgumentException(String.Format("Role name={0} not found", roleName));

            return roleRepository.GetRoleUsers(role).ToList().Select(x => x.ToBllUser());
        }

        public IEnumerable<BllRole> GetAllRoles()
        {
            return roleRepository.GetAll().ToList().Select(x => x.ToBllRole());
        }

        private void RemoveUserFromRoles(DalUser user, IEnumerable<int> newRoles)
        {
            var rolesForRemove =  roleRepository.GetUserRoles(user).Where(x => !newRoles.Contains(x.Id)).ToList();
            foreach (DalRole oldRole in rolesForRemove)
            {
                RemoveUserFromRole(user.UserName, oldRole.Name);
            }
        }

        private void AddUserInewRoles(DalUser user, IEnumerable<int> newRoles)
        {
            var oldRoles = roleRepository.GetUserRoles(user).ToList();
            foreach (int newRoleId in newRoles)
            {
                if (oldRoles.Count(x=> x.Id == newRoleId) == 0)
                {
                    DalRole role = roleRepository.GetById(newRoleId);
                    if (role != null)
                        AddUserInRole(user.UserName, role.Name);
                }
            }
        }

    }
}
