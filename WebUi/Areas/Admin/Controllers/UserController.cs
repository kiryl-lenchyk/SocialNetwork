using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;
using SocialNetwork.Bll.Interface.Entity;
using SocialNetwork.Bll.Interface.Services;
using WebUi.Areas.Admin.Mappers;
using WebUi.Areas.Admin.Models;

namespace WebUi.Areas.Admin.Controllers
{ 
    [Authorize(Roles = "Admin")]
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
            BllUser bllUser = userService.GetById(id,-1);
            if (bllUser == null) return HttpNotFound();

            return
                View(bllUser.ToUserEditViewModel(roleService.GetAllRoles(),
                    roleService.GetUserRoles(bllUser.UserName).Select(x => x.Id)));
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(UserEditViewModel model)
        {
            BllUser bllUser = userService.GetById(model.Id,-1);
            if (bllUser == null) return HttpNotFound();
            bllUser = UpdateBllUser(bllUser, model);
            userService.Update(bllUser);
            roleService.UpdateUserRoles(bllUser.UserName,model.SelectedRoles == null ? new List<int>() : model.SelectedRoles.ToList());

            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            BllUser bllUser = userService.GetById(id, -1);
            if (bllUser == null) return HttpNotFound();

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
