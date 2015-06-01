using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using System.Web.Security;
using SocialNetwork.Bll.Interface.Entity;
using SocialNetwork.Bll.Interface.Services;
using WebUi.Infractracture.Mappers;
using WebUi.Models;


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
            int userId = id ?? GetCurrentUserId();
            BllUser user = service.GetById(userId, GetCurrentUserId());
            if (user == null) return HttpNotFound();
            return View(user.ToUserPageViewModel(service));
        }

        private int GetCurrentUserId()
        {
            MembershipUser membershipUser = Membership.GetUser(HttpContext.User.Identity.Name);
            if (membershipUser != null)
            {
                object providerUserKey = membershipUser.ProviderUserKey;
                if (providerUserKey != null)
                {
                   return (int) providerUserKey;
                }
            }
            return 1;
        }


        public ActionResult Avatar(int id)
        {
            return File(HttpContext.Server.MapPath(@"~/App_Data/Empty.png"), "image/png");
        }

        [ChildActionOnly]
        [AllowAnonymous]
        public ActionResult SexList()
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
                            Text = sexValue.ToViewString(),
                            Value = sexValue.ToString()
                        })).ToList());
            return PartialView("_SexSelectList", new SelectList(sexItems, "Value", "Text", emptyValue));

        }

        public ActionResult Find()
        {
            return View(new UserFinViewModel());
        }

        [HttpPost]
        public ActionResult Find(UserFinViewModel model)
        {
            List<UserPreviewViewModel> partialModel =
                service.FindUsers(model.Name, model.Surname, model.BirthDayMin, model.BirthDayMax,
                    model.Sex.ToNullableBllSex()).Select(x => x.ToUserPreviewViewModel()).ToList();
          int a =  service.FindUsers(model.Name, model.Surname, model.BirthDayMin, model.BirthDayMax,
                model.Sex.ToNullableBllSex()).Count();
            return PartialView("_FindResult",partialModel);
        }

        protected override void Dispose(bool disposing)
        {
            service.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult AddToFriend(int id)
        {
            if (!service.IsUserExists(id)) return HttpNotFound();
            service.AddFriend(GetCurrentUserId(), id);
            return RedirectToAction("Index", new{id = id});
        }

        public ActionResult RemoveFriend(int id)
        {
            if (!service.IsUserExists(id)) return HttpNotFound();
            service.RemoveFriend(GetCurrentUserId(), id);
            return RedirectToAction("Index", new { id = id });
        }
    }
}
