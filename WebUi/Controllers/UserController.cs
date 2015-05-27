using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SocialNetwork.Bll.Interface.Services;
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

        public ActionResult Index()
        {
            return View(new UserPageViewModel
            {
                Name = "Name",
                Surname = "Surname",
                CanAddToFriends = true,
                CanWriteMessage = true,
                AboutUser = new String('A', 200),
                BirthDay = new DateTime(2000,1,5),
                Sex = Sex.Mail,
                Friends =
                    new List<UserPreviewViewModel>
                    {
                        new UserPreviewViewModel {Id = 1, Name = "Friend1", Surname = "Sur1"},
                        new UserPreviewViewModel {Id = 2, Name = "Friend2", Surname = "Sur2"},
                        new UserPreviewViewModel {Id = 3, Name = "Friend3", Surname = "Sur3"},
                        new UserPreviewViewModel {Id = 4, Name = "Friend4", Surname = "Sur4"}
                    }
            });
        }

        
        public ActionResult Avatar(int id)
        {
            return File(HttpContext.Server.MapPath(@"~/App_Data/Empty.png"), "image/png");
        }

        [ChildActionOnly]
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
            return View(new UserFindModel());
        }

        [HttpPost]
        public ActionResult Find(UserFindModel model)
        {
            List<UserPreviewViewModel> partialModel = new List<UserPreviewViewModel>()
            {
                new UserPreviewViewModel(){Id = 1, BirthDay = model.BirthDayMin,Name = model.Name,Surname = model.Surname,Sex = model.Sex},
                new UserPreviewViewModel(){Id = 2, BirthDay = new DateTime(2015,7,8),Name = "name1",Surname = "Surname1",Sex = Sex.Mail},
                new UserPreviewViewModel(){Id = 3, BirthDay = new DateTime(2015,8,8),Name = "Name2",Surname = "Surname2",Sex = Sex.Third},
            };

            return PartialView("_FindResult",partialModel);
        }

        protected override void Dispose(bool disposing)
        {
            service.Dispose();
            base.Dispose(disposing);
        }
    }
}
