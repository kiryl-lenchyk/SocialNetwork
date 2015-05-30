using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetwork.Bll.Interface.Entity;
using SocialNetwork.Bll.Interface.Services;
using WebUi.Models;

namespace WebUi.Infractracture.Mappers
{
    public static class UserMapper
    {
        public static UserPageViewModel ToUserPageViewModel(this BllUser bllUser, IUserService userService)
        {
            return new UserPageViewModel()
            {
                Id = bllUser.Id,
                BirthDay = bllUser.BirthDay,
                Name = bllUser.Name,
                Surname = bllUser.Surname,
                Sex = bllUser.Sex != null ? (Sex?)(int)bllUser.Sex : null,
                AboutUser = bllUser.AboutUser,
                CanAddToFriends = bllUser.CanCurrentUserAddToFriends,
                CanWriteMessage = bllUser.CanCurrentUserWriteMessage,
                UserName = bllUser.UserName,
                Friends = bllUser.FriendsId.Select(x => userService.GetById(x,-1).ToUserPreviewViewModel())
            };
        }

        public static UserPreviewViewModel ToUserPreviewViewModel(this BllUser bllUser)
        {
            return new UserPreviewViewModel()
            {
                Id = bllUser.Id,
                BirthDay = bllUser.BirthDay,
                Name = bllUser.Name,
                Surname = bllUser.Surname,
                Sex = bllUser.Sex != null ? (Sex?) (int) bllUser.Sex : null
            };
        }

        public static BllSex? ToNullableBllSex(this Sex? viewSex)
        {
            return viewSex != null ? (BllSex?) (int) viewSex : null;
        }

    }
}