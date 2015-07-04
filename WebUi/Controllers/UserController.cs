using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialNetwork.Bll.Interface.Entity;
using SocialNetwork.Bll.Interface.Services;
using SocialNetwork.Logger.Interface;
using WebUi.Infractracture.Mappers;
using WebUi.Models;


namespace WebUi.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        #region Fields

        private readonly IUserService service;
        private readonly ILogger logger;

        #endregion

        #region Constractors

        public UserController(IUserService service, ILogger logger)
        {
            this.service = service;
            this.logger = logger;
        }

        #endregion

        #region Action Methods

        public ActionResult Index(int? id)
        {
            int currentUserId = service.GetByName(HttpContext.User.Identity.Name).Id;
            int userId = id ?? currentUserId;
            logger.Log(LogLevel.Trace,"Request user page id = {0}. Current user id = {1}", id.ToString(), currentUserId.ToString());

            BllUser user = service.GetById(userId);
            if (user == null) throw new HttpException(404, string.Format("User id = {0} Not found", userId));
            return View(user.ToUserPageViewModel(service, currentUserId));
        }
        
        public ActionResult Avatar(int id)
        {
            logger.Log(LogLevel.Trace,"Request user avatar id = {0}", id.ToString());

            return File(service.GetUserAvatarStream(id),  "image/png");
        }

        [ChildActionOnly]
        [AllowAnonymous]
        public ActionResult SexList(Sex? selected)
        {
            var emptyValue = new SelectListItem()
            {
                Text = String.Empty,
                Value = String.Empty
            };
            var sexItems = new List<SelectListItem> {emptyValue};
            sexItems.AddRange((Enum.GetValues(typeof (Sex))
                .Cast<Sex>()
                .Select( sexValue =>
                        new SelectListItem()
                        {
                            Selected = sexValue == selected,
                            Text = sexValue.ToViewString(),
                            Value = sexValue.ToString()
                        })).ToList());
            return PartialView("_SexSelectList", new SelectList(sexItems, "Value", "Text", selected == null ? "" : selected.ToString()));

        }

        public ActionResult Find()
        {
            logger.Log(LogLevel.Trace,"Request find page");
            return View();
        }

        [HttpPost]
        public ActionResult Find(UserFinViewModel model)
        {
            logger.Log(LogLevel.Trace,
                "Request find result. Name = {0}, Surname = {1}, Sex = {2}, BithdayMin = {3}, BithdayMax = {4}",
                model.Name, model.Surname, model.Sex, model.BirthDayMin, model.BirthDayMax);

            List<UserPreviewViewModel> partialModel =
                service.FindUsers(model.Name, model.Surname, model.BirthDayMin, model.BirthDayMax,
                    model.Sex.ToNullableBllSex()).Select(x => x.ToUserPreviewViewModel()).ToList();

            return PartialView("_FindResult",partialModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToFriend(int id)
        {

            if (!service.IsUserExists(id)) throw new HttpException(404, string.Format("User id = {0} Not found. Add to friend", id));
            int currentUserId = service.GetByName(User.Identity.Name).Id;
            logger.Log(LogLevel.Trace,"Request add to friend id = {0}. Current user id = {1}", id.ToString(), currentUserId.ToString());
            
            service.AddFriend(currentUserId, id);
            return RedirectToAction("Index", new{id = id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveFriend(int id)
        {
            if (!service.IsUserExists(id)) throw new HttpException(404, string.Format("User id = {0} Not found. Delete from friend", id));
            int currentUserId = service.GetByName(User.Identity.Name).Id;
            logger.Log(LogLevel.Trace,"Request add to friend id = {0}. Current user id = {1}", id.ToString(), currentUserId.ToString());

            service.RemoveFriend(service.GetByName(User.Identity.Name).Id, id);
            return RedirectToAction("Index", new { id = id });
        }

        #endregion

    }
}
