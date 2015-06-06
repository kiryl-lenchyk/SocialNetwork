using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetwork.Bll.Interface.Entity;
using WebUi.Areas.Admin.Models;

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

    }
}