using System.Collections.Generic;
using System.Linq;
using SocialNetwork.Dal.Interface.DTO;
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
                SecurityStamp = dalUser.SecurityStamp,
                SendedMessages = dalUser.SendedMessages == null ? new List<Message>() : dalUser.SendedMessages.Select(x =>x.ToOrmMessage()).ToList(),
                GottenMessages = dalUser.GottenMessages == null ? new List<Message>() : dalUser.GottenMessages.Select(x => x.ToOrmMessage()).ToList(),
                Roles = dalUser.Roles == null ? new List<Role>() : dalUser.Roles.Select(x => x.ToOrmRole()).ToList(),
                Friends = dalUser.Friends == null ? new List<User>() : dalUser.Friends.Select(x => x.ToOrmUser()).ToList()
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
                SecurityStamp = user.SecurityStamp,
                SendedMessages = user.SendedMessages.Select(x => x.ToDalMessage()),
                GottenMessages = user.GottenMessages.Select(x => x.ToDalMessage()),
                Roles = user.Roles.Select(x => x.ToDalRole()),
                Friends = user.Friends.Select(x => x.ToDalUser()),
            };
        }

    }
}
