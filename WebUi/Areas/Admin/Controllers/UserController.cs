using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUi.Areas.Admin.Controllers
{ 
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        //
        // GET: /Admin/User/

       
        public ActionResult Index()
        {
            return View();
        }

    }
}
