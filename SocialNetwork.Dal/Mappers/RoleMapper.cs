using System;
using System.Linq.Expressions;
using SocialNetwork.Dal.Interface.DTO;
using SocialNetwork.Orm;

namespace SocialNetwork.Dal.Mappers
{

    /// <summary>
    /// Contains exstension methods for convertion between DalRole and Role from ORM Layaer.
    /// </summary>
    public static class RoleMapper
    {

        /// <summary>
        /// Convert to ORM Role.
        /// </summary>
        /// <param name="dalRole">DalRole to convert</param>
        /// <returns>ORM Role from this DalRole</returns>
        public static Role ToOrmRole(this DalRole dalRole)
        {
            return new Role()
            {
                Id = dalRole.Id,
                Name = dalRole.Name
            };
        }

        /// <summary>
        /// Convert to DalRole.
        /// </summary>
        /// <param name="role">ORM Role to convert</param>
        /// <returns>DalRole from this ORM Role</returns>
        public static DalRole ToDalRole(this Role role)
        {
            return ToDalRolExpression.Compile()(role);
        }

        /// <summary>
        /// Expression that convert ORM Role to DalRole. For using in LINQtoSQL query.
        /// </summary>
        public static Expression<Func<Role, DalRole>> ToDalRolExpression
        {
            get
            {
                return (Role role) => new DalRole()
                {
                    Id = role.Id,
                    Name = role.Name
                };
            }
        }

    }
}
