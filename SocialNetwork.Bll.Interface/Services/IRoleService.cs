using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Bll.Interface.Entity;

namespace SocialNetwork.Bll.Interface.Services
{
    public interface IRoleService : IDisposable
    {
        bool IsUserInRole(string username, string roleName);

        IEnumerable<BllRole> GetUserRoles(string username);

        void AddUserInRole(string username, string roleName);

        void RemoveUserFromRole(string username, string roleName);

        IEnumerable<BllUser> GetUsersInRole(string roleName);

        IEnumerable<BllRole> GetAllRoles();
    }
}
