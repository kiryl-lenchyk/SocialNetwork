using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialNetwork.Bll.Interface.Services;
using WebUi.Areas.Admin.Mappers;

namespace WebUi.Areas.Admin.Controllers
{ 
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserService service;

        public UserController(IUserService service)
        {
            this.service = service;
        }
       
        public ActionResult Index()
        {
           return View(service.GetAllUsers().Select(x => x.ToUserPreviewViewModel()));
        }

        protected override void Dispose(bool disposing)
        {
            service.Dispose();
            base.Dispose(disposing);
        }

    }
}
