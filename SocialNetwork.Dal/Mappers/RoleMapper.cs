using SocialNetwork.Dal.Interface.DTO;
using SocialNetwork.Orm;

namespace SocialNetwork.Dal.Mappers
{
    public static class RoleMapper
    {
        public static Role ToOrmRole(this DalRole dalRole)
        {
            return new Role()
            {
                Id = dalRole.Id,
                Name = dalRole.Name
            };
        }

        public static DalRole ToDalRole(this Role role)
        {
            return new DalRole()
            {
                Id = role.Id,
                Name = role.Name
            };
        }

    }
}
