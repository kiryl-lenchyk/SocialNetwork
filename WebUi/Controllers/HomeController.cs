using System;
using System.Web.Mvc;

namespace WebUi.Controllers
{
    public class HomeController : Controller
    {
        #region Action Methods

        public ActionResult Index()
        {
            return RedirectToAction("Index","User");
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult ThrowException()
        {
           throw new Exception("Test");
        }

        #endregion

    }
}
