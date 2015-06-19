using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SocialNetwork.Dal.Interface.DTO;
using SocialNetwork.Orm;

namespace SocialNetwork.Dal.Mappers
{

    /// <summary>
    /// Contains exstension methods for convertion between DalUser and User from ORM Layaer.
    /// </summary>
    public static class UserMapper
    {
        
        /// <summary>
        /// Convert to ORM User.
        /// </summary>
        /// <param name="dalUser">DalUser to convert</param>
        /// <returns>ORM User from this DalUser</returns>
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
                Friends = dalUser.FriendsId == null ? new List<User>() : dalUser.FriendsId.Select(x => new User()).ToList()
            };
        }

        /// <summary>
        /// Convert to DalUser.
        /// </summary>
        /// <param name="user">ORM User to convert</param>
        /// <returns>DalUser from this ORM User</returns>
        public static DalUser ToDalUser(this User user)
        {
            return ToDalUserConvertion.Compile()(user);
        }

        /// <summary>
        /// Expression that convert ORM User to DalUser. For using in LINQtoSQL query.
        /// </summary>
        public static Expression<Func<User, DalUser>> ToDalUserConvertion
        {
            get
            {
                return (User user) => new DalUser()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Name = user.Name,
                    Surname = user.Surname,
                    BirthDay = user.BirthDay,
                    Sex = user.Sex != null ? (DalSex?) (int) user.Sex.Value : null,
                    AboutUser = user.AboutUser,
                    PasswordHash = user.PasswordHash,
                    FriendsId = user.Friends.Select(x => x.Id),
                };
            }
        }

    }
}
