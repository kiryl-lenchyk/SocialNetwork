﻿using System;
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
DalUser user = userRepository.GetByName(username);
            if (user == null) return false;

            return roleRepository.GetUserRoles(user).Count(x => x.Name == roleName) != 0;
        }

        public IEnumerable<BllRole> GetUserRoles(string username)
        {
            DalUser user = userRepository.GetByName(username);
            if (user == null) return new List<BllRole>();
            
            return roleRepository.GetUserRoles(user).Select(x => x.ToBllRole());
        }

        public void AddUserInRole(string username, string roleName)
        {
DalRole role = roleRepository.GetByName(roleName);
            if (role == null) return;
            DalUser user = userRepository.GetByName(username);
            if (user == null) return;

            roleRepository.AddUserToRole(user, role);
        }

        public void RemoveUserFromRole(string username, string roleName)
        {
DalRole role = roleRepository.GetByName(roleName);
            if (role == null) return;
            DalUser user = userRepository.GetByName(username);
            if (user == null) return;

            roleRepository.RemoveUserFromRole(user, role);
        }

        public void UpdateUserRoles(string username, IEnumerable<int> rolesIds)
        {
DalUser  user = userRepository.GetByName(username);
            if (user == null) return;
            RemoveUserFromRoles(user,rolesIds);
            user = userRepository.GetByName(username);
            AddUserInewRoles(user,rolesIds);
        }

        public IEnumerable<BllUser> GetUsersInRole(string roleName)
        {
DalRole role = roleRepository.GetByName(roleName);
            if (role == null) return new List<BllUser>();

            return roleRepository.GetRoleUsers(role).Select(x => x.ToBllUser());
        }

        public IEnumerable<BllRole> GetAllRoles()
        {
return roleRepository.GetAll().Select(x => x.ToBllRole());
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
