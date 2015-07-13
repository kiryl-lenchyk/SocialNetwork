using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Dal.Interface.DTO;
using SocialNetwork.Orm;

namespace SocialNetwork.Dal.Mappers
{

    /// <summary>
    /// Contains exstension methods for convertion between DalAvatar and User from ORM Layaer.
    /// </summary>
    public static class AvatarMapper
    {

        /// <summary>
        /// Expression that convert ORM User to DalAvatar. For using in LINQtoSQL query.
        /// </summary>
        public static Expression<Func<User, DalAvatar>> ToDalAvatarConvertion
        {
            get
            {
                return (User user) => new DalAvatar()
                {
                    Id = user.Id,
                    UserId = user.Id,
                    ImageBytes = user.Avatar
                };
            }
        }

        /// <summary>
        /// Convert to DalAvatar.
        /// </summary>
        /// <param name="user">ORM User to convert</param>
        /// <returns>DalAvatar from this ORM User</returns>
        public static DalAvatar ToDalAvatar(this User user)
        {
            return ToDalAvatarConvertion.Compile()(user);
        }
    }
}
