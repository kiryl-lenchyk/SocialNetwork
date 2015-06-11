using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SocialNetwork.Bll.Interface.Entity;
using SocialNetwork.Bll.Interface.Services;
using WebUi.Infractracture.Mappers;
using WebUi.Models;
using WebUi.Providers;


namespace WebUi.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService service;

        public UserController(IUserService service)
        {
            this.service = service;
        }

        public ActionResult Index(int? id)
        {
            int currentUserId = service.GetByName(HttpContext.User.Identity.Name).Id;
            int userId = id ?? currentUserId;
            BllUser user = service.GetById(userId);
            if (user == null) return HttpNotFound();
            return View(user.ToUserPageViewModel(service, currentUserId));
        }

        


        public ActionResult Avatar(int id)
        {
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
            return View();
        }

        [HttpPost]
        public ActionResult Find(UserFinViewModel model)
        {
            List<UserPreviewViewModel> partialModel =
                service.FindUsers(model.Name, model.Surname, model.BirthDayMin, model.BirthDayMax,
                    model.Sex.ToNullableBllSex()).Select(x => x.ToUserPreviewViewModel()).ToList();

            return PartialView("_FindResult",partialModel);
        }

      public ActionResult AddToFriend(int id)
        {
            if (!service.IsUserExists(id)) return HttpNotFound();
            service.AddFriend(service.GetByName(User.Identity.Name).Id, id);
            return RedirectToAction("Index", new{id = id});
        }

        public ActionResult RemoveFriend(int id)
        {
            if (!service.IsUserExists(id)) return HttpNotFound();
            service.RemoveFriend(service.GetByName(User.Identity.Name).Id, id);
            return RedirectToAction("Index", new { id = id });
        }

       
    }
}
