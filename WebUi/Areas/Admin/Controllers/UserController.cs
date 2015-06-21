using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;
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

        #region Fields

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IUserService userService;
        private readonly IRoleService roleService;

        #endregion

        #region Constractors

        public UserController(IUserService userService, IRoleService roleService)
        {
            this.userService = userService;
            this.roleService = roleService;
        }

        #endregion

        #region Action Methods

        public ActionResult Index()
        {
            Logger.Trace("Request users list for admin");

           return View(userService.GetAllUsers().Select(x => x.ToUserPreviewViewModel()));
        }

        public ActionResult Edit(int id)
        {
            Logger.Trace("Request user edit page for admin id = {0}", id.ToString());
            BllUser bllUser = userService.GetById(id);
            if (bllUser == null) throw new HttpException(404, string.Format("User id = {0} Not found", id));

            return
                View(bllUser.ToUserEditViewModel(roleService.GetAllRoles(),
                    roleService.GetUserRoles(bllUser.UserName).Select(x => x.Id)));
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(UserEditViewModel model)
        {
            BllUser bllUser = userService.GetById(model.Id);
            if (bllUser == null) throw new HttpException(404, string.Format("User id = {0} Not found. Update user", model.Id));
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
            if (bllUser == null) throw new HttpException(404, string.Format("User id = {0} Not found. Delete user", id));

            userService.Delete(bllUser);
            return RedirectToAction("Index");
        }

        #endregion

        #region Private Methods

        private BllUser UpdateBllUser(BllUser bllUser, UserEditViewModel model)
        {
            bllUser.AboutUser = model.AboutUser;
            bllUser.Name = model.Name;
            bllUser.Surname = model.Surname;
            return bllUser;
        }

        #endregion

    }
}
