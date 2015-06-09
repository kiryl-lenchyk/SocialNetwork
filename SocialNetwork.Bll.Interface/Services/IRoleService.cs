using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Bll.Interface.Entity;

namespace SocialNetwork.Bll.Interface.Services
{
    public interface IRoleService
    {
        bool IsUserInRole(string username, string roleName);

        IEnumerable<BllRole> GetUserRoles(string username);

        void AddUserInRole(string username, string roleName);

        void RemoveUserFromRole(string username, string roleName);

        void UpdateUserRoles(string username, IEnumerable<int> rolesIds);

        IEnumerable<BllUser> GetUsersInRole(string roleName);

        IEnumerable<BllRole> GetAllRoles();
    }
}
