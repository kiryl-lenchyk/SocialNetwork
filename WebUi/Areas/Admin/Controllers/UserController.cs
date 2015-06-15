using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using SocialNetwork.Bll.Interface.Entity;
using SocialNetwork.Bll.Interface.Services;
using WebUi.Areas.Admin.Mappers;
using WebUi.Areas.Admin.Models;
using WebUi.Filters;

namespace WebUi.Areas.Admin.Controllers
{

    [AutorizeRolesFromConfig("AdminRoleName")]
    public class UserController : Controller
    {
        private readonly IUserService userService;

        private readonly IRoleService roleService;

        public UserController(IUserService userService, IRoleService roleService)
        {
            this.userService = userService;
            this.roleService = roleService;
        }
       
        public ActionResult Index()
        {
           return View(userService.GetAllUsers().Select(x => x.ToUserPreviewViewModel()));
        }

        public ActionResult Edit(int id)
        {
            BllUser bllUser = userService.GetById(id);
            if (bllUser == null) throw new HttpException(404, "Not found");

            return
                View(bllUser.ToUserEditViewModel(roleService.GetAllRoles(),
                    roleService.GetUserRoles(bllUser.UserName).Select(x => x.Id)));
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(UserEditViewModel model)
        {
            BllUser bllUser = userService.GetById(model.Id);
            if (bllUser == null) throw new HttpException(404, "Not found");
            bllUser = UpdateBllUser(bllUser, model);
            userService.Update(bllUser);
            roleService.UpdateUserRoles(bllUser.UserName,model.SelectedRoles == null ? new List<int>() : model.SelectedRoles.ToList());

            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            BllUser bllUser = userService.GetById(id);
            if (bllUser == null) throw new HttpException(404, "Not found");

            userService.Delete(bllUser);
            return RedirectToAction("Index");
        }

        private BllUser UpdateBllUser(BllUser bllUser, UserEditViewModel model)
        {
            bllUser.AboutUser = model.AboutUser;
            bllUser.Name = model.Name;
            bllUser.Surname = model.Surname;
            return bllUser;
        }

        

    }
}
