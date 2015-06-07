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

        private bool isDisposed = false;

        public RoleService(IRoleRepository roleRepository, IUserRepository userRepository)
        {
            this.roleRepository = roleRepository;
            this.userRepository = userRepository;
        }

        public bool IsUserInRole(string username, string roleName)
        {
            if(isDisposed) throw new ObjectDisposedException("RoleService");

            DalRole role = roleRepository.GetByName(roleName);
            if (role == null) return false;
            DalUser user = userRepository.GetByName(username);
            if (user == null) return false;

            return user.RolesId.Contains(role.Id);
        }

        public IEnumerable<BllRole> GetUserRoles(string username)
        {
            if(isDisposed) throw new ObjectDisposedException("RoleService");

            DalUser user = userRepository.GetByName(username);
            if (user == null) return new List<BllRole>();
            
            return roleRepository.GetUserRoles(user).Select(x => x.ToBllRole());
        }

        public void AddUserInRole(string username, string roleName)
        {
            if (isDisposed) throw new ObjectDisposedException("RoleService");

            DalRole role = roleRepository.GetByName(roleName);
            if (role == null) return;
            DalUser user = userRepository.GetByName(username);
            if (user == null) return;

            roleRepository.AddUserToRole(user, role);
        }

        public void RemoveUserFromRole(string username, string roleName)
        {
            if (isDisposed) throw new ObjectDisposedException("RoleService");

            DalRole role = roleRepository.GetByName(roleName);
            if (role == null) return;
            DalUser user = userRepository.GetByName(username);
            if (user == null) return;

            roleRepository.RmoveUserFromRole(user, role);
        }

        public void UpdateUserRoles(string username, IEnumerable<int> rolesIds)
        {
            if (isDisposed) throw new ObjectDisposedException("RoleService");

            DalUser  user = userRepository.GetByName(username);
            if (user == null) return;
            RemoveUserFromRoles(user,rolesIds);
            user = userRepository.GetByName(username);
            AddUserInewRoles(user,rolesIds);
        }

        public IEnumerable<BllUser> GetUsersInRole(string roleName)
        {
            if (isDisposed) throw new ObjectDisposedException("RoleService");

            DalRole role = roleRepository.GetByName(roleName);
            if (role == null) return new List<BllUser>();

            return roleRepository.GetRoleUsers(role).Select(x => x.ToBllUser());
        }

        public IEnumerable<BllRole> GetAllRoles()
        {
            if (isDisposed) throw new ObjectDisposedException("RoleService");

            return roleRepository.GetAll().Select(x => x.ToBllRole());
        }

        private void RemoveUserFromRoles(DalUser user, IEnumerable<int> newRoles)
        {
            var rolesForRemove = user.RolesId.Where(x => !newRoles.Contains(x)).ToList();
            foreach (int oldRoleId in rolesForRemove)
            {
                DalRole role = roleRepository.GetById(oldRoleId);
                if (role != null)
                    RemoveUserFromRole(user.UserName, role.Name);
            }
        }

        private void AddUserInewRoles(DalUser user, IEnumerable<int> newRoles)
        {
            foreach (int newRoleId in newRoles)
            {
                if (!user.RolesId.Contains(newRoleId))
                {
                    DalRole role = roleRepository.GetById(newRoleId);
                    if (role != null)
                        AddUserInRole(user.UserName, role.Name);
                }
            }
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                isDisposed = true;
                userRepository.Dispose();
                roleRepository.Dispose();
            }
        }
    }
}
