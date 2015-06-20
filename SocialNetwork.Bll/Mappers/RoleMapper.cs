using SocialNetwork.Bll.Interface.Entity;
using SocialNetwork.Dal.Interface.DTO;

namespace SocialNetwork.Bll.Mappers
{ 
    /// <summary>
    /// Contains exstension methods for convertion between DalRole and BllRole.
    /// </summary>
    public static class RoleMapper
    {
        /// <summary>
        /// Convert to BllRole.
        /// </summary>
        /// <param name="dalRole">DalRole to convert</param>
        /// <returns>BllRole from this DalRole</returns>
        public static BllRole ToBllRole(this DalRole dalRole)
        {
            return new BllRole()
            {
                Id = dalRole.Id,
                Name = dalRole.Name
            };
        }

        /// <summary>
        /// Convert to DalRole.
        /// </summary>
        /// <param name="bllRole">BllRole to convert</param>
        /// <returns>DalRole from this BllRole</returns>
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
