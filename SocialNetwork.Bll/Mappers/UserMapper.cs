using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using SocialNetwork.Bll.Interface.Entity;
using SocialNetwork.Dal.Interface.DTO;

namespace SocialNetwork.Bll.Mappers
{
    public static class UserMapper
    {
        public static BllUser ToBllUser(this DalUser  dalUser, int currentUserId = -1)
        {
            BllUser bllUser = new BllUser()
            {
                Id = dalUser.Id,
                UserName = dalUser.UserName,
                Name = dalUser.Name,
                Surname = dalUser.Surname,
                BirthDay = dalUser.BirthDay,
                Sex = dalUser.Sex != null ? (BllSex?)(int)dalUser.Sex.Value : null,
                AboutUser = dalUser.AboutUser,
                PasswordHash = dalUser.PasswordHash,
                CanCurrentUserAddToFriends = dalUser.Id != currentUserId && dalUser.FriendsId.Count(x => x == currentUserId) == 0,
                SendedMessagesId = dalUser.SendedMessagesId,
                GottenMessagesId = dalUser.GottenMessagesId,
                RolesId = dalUser.RolesId,
                FriendsId = dalUser.FriendsId
            };
            bllUser.CanCurrentUserWriteMessage =  dalUser.Id != currentUserId && !bllUser.CanCurrentUserAddToFriends;
            return bllUser;
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
                Sex = bllUser.Sex != null ? (DalSex?)(int)bllUser.Sex.Value : null,
                AboutUser = bllUser.AboutUser,
                PasswordHash = bllUser.PasswordHash,
                SendedMessagesId = bllUser.SendedMessagesId,
                GottenMessagesId = bllUser.GottenMessagesId,
                RolesId = bllUser.RolesId,
                FriendsId = bllUser.FriendsId
            };
        }

    }
}
