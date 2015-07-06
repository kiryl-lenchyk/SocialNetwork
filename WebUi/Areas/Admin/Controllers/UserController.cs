using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using PagedList;
using SocialNetwork.Bll.Interface.Entity;
using SocialNetwork.Bll.Interface.Services;
using SocialNetwork.Logger.Interface;
using WebUi.Areas.Admin.Mappers;
using WebUi.Areas.Admin.Models;
using WebUi.Filters;

namespace WebUi.Areas.Admin.Controllers
{

    [AutorizeRolesFromConfig("AdminRoleName")]
    public class UserController : Controller
    {

        #region Fields

        private readonly IUserService userService;
        private readonly IRoleService roleService;
        private readonly ILogger logger;

        private static readonly int UsersListPageSize;
        private static readonly int DefaultPageSize = 3;

        #endregion

        #region Constractors

        static UserController()
        {
            if (!Int32.TryParse(WebConfigurationManager.AppSettings["UsersListPageSize"],
                out UsersListPageSize))
            {
                UsersListPageSize = DefaultPageSize;
                ((ILogger) DependencyResolver.Current.GetService(typeof (ILogger))).Log(
                    LogLevel.Error,
                    "web.config contains incorrect date for UsersListPageSize. Value: {0}",
                    WebConfigurationManager.AppSettings["UsersListPageSize"]);
            }
        }

        public UserController(IUserService userService, IRoleService roleService, ILogger logger)
        {
            this.userService = userService;
            this.roleService = roleService;
            this.logger = logger;
        }

        #endregion

        #region Action Methods

        public ActionResult Index()
        {
            logger.Log(LogLevel.Trace,"Request users list for admin");

           return View();
        }

        public ActionResult UsersListPage(int? page)
        {
            int pageNumber = page ?? 1;

            return PartialView("_UsersListPage",
                userService.GetAllUsers()
                    .Select(x => x.ToUserPreviewViewModel())
                    .ToPagedList(pageNumber, UsersListPageSize));
        }

        public ActionResult Edit(int id)
        {
            logger.Log(LogLevel.Trace,"Request user edit page for admin id = {0}", id.ToString());
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
