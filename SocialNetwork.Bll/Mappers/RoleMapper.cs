using SocialNetwork.Bll.Interface.Entity;
using SocialNetwork.Dal.Interface.DTO;

namespace SocialNetwork.Bll.Mappers
{
    public static class RoleMapper
    {
        public static BllRole ToBllRole(this DalRole dalRole)
        {
            return new BllRole()
            {
                Id = dalRole.Id,
                Name = dalRole.Name
            };
        }

        public static DalRole ToDalRole(this BllRole bllRole)
        {
            return new DalRole()
            {
                Id = bllRole.Id,
                Name = bllRole.Name
            };
        }

    }
}
