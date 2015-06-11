using SocialNetwork.Bll.Interface.Entity;
using SocialNetwork.Dal.Interface.DTO;

namespace SocialNetwork.Bll.Mappers
{
    public static class UserMapper
    {
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

        public static DalSex? ToDalSex(this BllSex? bllSex)
        {
            return bllSex != null ? (DalSex?) (int) bllSex.Value : null;
        }

    }
}
