﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using SocialNetwork.Bll.Interface.Entity;
using SocialNetwork.Bll.Interface.Services;
using WebUi.Models;

namespace WebUi.Infractracture.Mappers
{
    public static class UserMapper
    {
        public static UserPageViewModel ToUserPageViewModel(this BllUser bllUser, IUserService userService, int currentUserId)
        {
            return new UserPageViewModel()
            {
                Id = bllUser.Id,
                BirthDay = bllUser.BirthDay,
                Name = bllUser.Name,
                Surname = bllUser.Surname,
                Sex = bllUser.Sex != null ? (Sex?)(int)bllUser.Sex : null,
                AboutUser = bllUser.AboutUser,
                CanAddToFriends = userService.CanUserAddToFriends(currentUserId, bllUser.Id),
                CanWriteMessage = userService.CanUserWrieMesage(bllUser.Id, currentUserId),
                UserName = bllUser.UserName,
                Friends = bllUser.FriendsId.Select(x => userService.GetById(x).ToUserPreviewViewModel()),
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

        public static EdirAccountViewModel ToEdirAccountViewModel(this BllUser bllUser)
        {
            return new EdirAccountViewModel()
            {
                Id = bllUser.Id,
                BirthDay = bllUser.BirthDay,
                Name = bllUser.Name,
                Surname = bllUser.Surname,
                Sex = bllUser.Sex != null ? (Sex?)(int)bllUser.Sex : null,
                AboutUser = bllUser.AboutUser
            };
        }

        public static BllUser ToBllUser(this EdirAccountViewModel edirAccountViewModel)
        {
            return new BllUser()
            {
                Id = edirAccountViewModel.Id,
                BirthDay = edirAccountViewModel.BirthDay,
                Name = edirAccountViewModel.Name,
                Surname = edirAccountViewModel.Surname,
                Sex = edirAccountViewModel.Sex != null ? (BllSex?)(int)edirAccountViewModel.Sex : null,
                AboutUser = edirAccountViewModel.AboutUser
            };
        }


        public static BllSex? ToNullableBllSex(this Sex? viewSex)
        {
            return viewSex != null ? (BllSex?) (int) viewSex : null;
        }

    }
}