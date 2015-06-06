using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using SocialNetwork.Dal.Interface.DTO;

namespace SocialNetwork.Dal.Interface.Repository
{
    public interface IRoleRepository : IRepository<DalRole>
    {
        DalRole GetByName(String roleName);

        IEnumerable<DalRole> GetUserRoles(DalUser user);
        
        IEnumerable<DalUser> GetRoleUsers(DalRole role);

        void AddUserToRole(DalUser user, DalRole role);

        void RmoveUserFromRole(DalUser user, DalRole role);
    }
}
