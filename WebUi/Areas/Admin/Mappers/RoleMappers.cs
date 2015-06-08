using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetwork.Bll.Interface.Entity;
using WebUi.Areas.Admin.Models;

namespace WebUi.Areas.Admin.Mappers
{
    public static  class RoleMappers
    {
        public static RoleViewModel ToRoleView(this BllRole bllRole )
        {
            return new RoleViewModel()
            {
                Id = bllRole.Id,
                Name = bllRole.Name
            };
        }
    }
}