using System.Collections.Generic;
using System.Linq;
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
                SecurityStamp = dalUser.SecurityStamp,
                SendedMessages = dalUser.SendedMessages == null ? new List<BllMessage>() : dalUser.SendedMessages.Select(x =>x.ToBllMessage()).ToList(),
                GottenMessages = dalUser.GottenMessages == null ? new List<BllMessage>() : dalUser.GottenMessages.Select(x => x.ToBllMessage()).ToList(),
                Roles = dalUser.Roles == null ? new List<BllRole>() : dalUser.Roles.Select(x => x.ToBllRole()).ToList(),
                Friends = dalUser.Friends == null ? new List<BllUser>() : dalUser.Friends.Select(x => x.ToBllUser()).ToList()
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
                Sex = bllUser.Sex != null ? (DalSex?)(int)bllUser.Sex.Value : null,
                AboutUser = bllUser.AboutUser,
                PasswordHash = bllUser.PasswordHash,
                SecurityStamp = bllUser.SecurityStamp,
                SendedMessages = bllUser.SendedMessages == null ? new List<DalMessage>() : bllUser.SendedMessages.Select(x =>x.ToDalMessage()).ToList(),
                GottenMessages = bllUser.GottenMessages == null ? new List<DalMessage>() : bllUser.GottenMessages.Select(x => x.ToDalMessage()).ToList(),
                Roles = bllUser.Roles == null ? new List<DalRole>() : bllUser.Roles.Select(x => x.ToDalRole()).ToList(),
                Friends = bllUser.Friends == null ? new List<DalUser>() : bllUser.Friends.Select(x => x.ToDalUser()).ToList()
            };
        }

    }
}
