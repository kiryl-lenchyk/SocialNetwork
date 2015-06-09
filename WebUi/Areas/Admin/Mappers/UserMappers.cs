using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetwork.Bll.Interface.Entity;
using WebUi.Areas.Admin.Models;
using WebUi.Models;
using UserPreviewViewModel = WebUi.Areas.Admin.Models.UserPreviewViewModel;

namespace WebUi.Areas.Admin.Mappers
{
    public static class UserMappers
    {
        public static UserPreviewViewModel ToUserPreviewViewModel(this BllUser bllUser)
        {
            return new UserPreviewViewModel()
            {
                Id = bllUser.Id,
                UserName = bllUser.UserName,
                Name = bllUser.Name,
                Surname = bllUser.Surname
            };
        }

        public static UserEditViewModel ToUserEditViewModel(this BllUser bllUser,
            IEnumerable<BllRole> allRoles, IEnumerable<int> userRoles )
        {
            return new UserEditViewModel()
            {
                Id = bllUser.Id,
                UserName = bllUser.UserName,
                BirthDay = bllUser.BirthDay,
                Sex = bllUser.Sex == null ? null : (Sex?) bllUser.Sex.Value,
                AboutUser = bllUser.AboutUser,
                Name = bllUser.Name,
                Surname = bllUser.Surname,
                AllRoles = allRoles.Select(x => x.ToRoleView()).ToList(),
                UserRolesIds = userRoles.ToList()
            };
        }

        

    }
}