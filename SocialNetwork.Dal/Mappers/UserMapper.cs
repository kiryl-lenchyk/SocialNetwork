using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SocialNetwork.Dal.Interface.DTO;
using SocialNetwork.Dal.Repository;
using SocialNetwork.Orm;

namespace SocialNetwork.Dal.Mappers
{
    public static class UserMapper
    {
        public static User ToOrmUser(this DalUser  dalUser)
        {
            return new User()
            {
                Id = dalUser.Id,
                UserName = dalUser.UserName,
                Name = dalUser.Name,
                Surname = dalUser.Surname,
                BirthDay = dalUser.BirthDay,
                Sex = dalUser.Sex != null ? (Sex?)(int)dalUser.Sex.Value : null,
                AboutUser = dalUser.AboutUser,
                PasswordHash = dalUser.PasswordHash,
                SendedMessages = dalUser.SendedMessagesId == null ? new List<Message>() : dalUser.SendedMessagesId.Select(x => new Message()).ToList(),
                GottenMessages = dalUser.GottenMessagesId == null ? new List<Message>() : dalUser.GottenMessagesId.Select(x => new Message()).ToList(),
                Roles = dalUser.RolesId == null ? new List<Role>() : dalUser.RolesId.Select(x => new Role()).ToList(),
                Friends = dalUser.FriendsId == null ? new List<User>() : dalUser.FriendsId.Select(x => new User()).ToList()
            };
        }

        public static DalUser ToDalUser(this User user)
        {
            return new DalUser()
            {
                Id = user.Id,
                UserName = user.UserName,
                Name = user.Name,
                Surname = user.Surname,
                BirthDay = user.BirthDay,
                Sex = user.Sex != null ? (DalSex?) (int) user.Sex.Value : null,
                AboutUser = user.AboutUser,
                PasswordHash = user.PasswordHash,
                SendedMessagesId = user.SendedMessages.Select(x => x.Id),
                GottenMessagesId = user.GottenMessages.Select(x => x.Id),
                RolesId = user.Roles.Select(x => x.Id),
                FriendsId = user.Friends.Select(x => x.Id),
            };
        }

    }
}
