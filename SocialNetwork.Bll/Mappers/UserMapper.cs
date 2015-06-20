using SocialNetwork.Bll.Interface.Entity;
using SocialNetwork.Dal.Interface.DTO;

namespace SocialNetwork.Bll.Mappers
{

    /// <summary>
    /// Contains exstension methods for convertion between DalUser and BllUser.
    /// </summary>
    public static class UserMapper
    {
        /// <summary>
        /// Convert to BllUser.
        /// </summary>
        /// <param name="dalUser">DalUser to convert</param>
        /// <returns>BllUser from this DalUser</returns>
        public static BllUser ToBllUser(this DalUser  dalUser)
        {
            return new BllUser()
            {
                Id = dalUser.Id,
                UserName = dalUser.UserName,
                Name = dalUser.Name,
                Surname = dalUser.Surname,
                BirthDay = dalUser.BirthDay,
                Sex = dalUser.Sex != null ? (BllSex?)(int)dalUser.Sex.Value : null,
                AboutUser = dalUser.AboutUser,
                PasswordHash = dalUser.PasswordHash,
                FriendsId = dalUser.FriendsId
            };
        }

        /// <summary>
        /// Convert to DalUser.
        /// </summary>
        /// <param name="bllUser">BllUser to convert</param>
        /// <returns>DalUser from this BllUser</returns>
        public static DalUser ToDalUser(this BllUser bllUser)
        {
            return new DalUser()
            {
                Id = bllUser.Id,
                UserName = bllUser.UserName,
                Name = bllUser.Name,
                Surname = bllUser.Surname,
                BirthDay = bllUser.BirthDay,
                Sex = bllUser.Sex.ToDalSex(),
                AboutUser = bllUser.AboutUser,
                PasswordHash = bllUser.PasswordHash,
                FriendsId = bllUser.FriendsId
            };
        }

        /// <summary>
        /// Convert to DalSex.
        /// </summary>
        /// <param name="bllSex">BllSex to convert</param>
        /// <returns>DalSex from this BllSex</returns>
        public static DalSex? ToDalSex(this BllSex? bllSex)
        {
            return bllSex != null ? (DalSex?) (int) bllSex.Value : null;
        }

    }
}
