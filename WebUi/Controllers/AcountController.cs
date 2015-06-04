using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SocialNetwork.Bll.Interface.Entity;
using SocialNetwork.Bll.Interface.Services;
using WebUi.Infractracture.Mappers;
using WebUi.Models;
using WebUi.Providers;

namespace WebUi.Controllers
{
    [AllowAnonymous]
    public class AcountController : Controller
    {
        private readonly IUserService service;

        public AcountController(IUserService service)
        {
            this.service = service;
        }
        

        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                ModelState.AddModelError("", "Incorrect user name or password");
            }

            return View(model);
            
        }

        [Authorize]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (service.IsUserExists(model.UserName))
            {
                ModelState.AddModelError("UserName", "User with this name already exists");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                MembershipUser membershipUser = ((UserMembershipProvider) Membership.Provider)
                    .CreateUser(model.UserName, model.Password, model.Name, model.Surname,
                    model.AboutUser, model.BirthDay, model.Sex == null ? null : (BllSex?)(int) model.Sex);

                if (membershipUser != null)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false);
                    return RedirectToAction("Index", "User");
                }
                ModelState.AddModelError("", "Registration error");
                
            }  
            return View(model);
        }


        [HttpGet]
        [Authorize]
        public ActionResult Edit()
        {
            return View(service.GetByName(User.Identity.Name,-1).ToEdirAccountViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(EdirAccountViewModel model)
        {
           
            if (ModelState.IsValid)
            {
                BllUser newUser = model.ToBllUser();
                BllUser oldUser = service.GetById(model.Id, -1);
                if (oldUser == null) return HttpNotFound();

                newUser.UserName = oldUser.UserName;
                newUser.PasswordHash = oldUser.PasswordHash;
                service.Update(newUser);

                ViewBag.StatusMessage = "User information successfully changed";
                return View(model);
            }
            return View(model);
        }
        
        [HttpGet]
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Membership.Provider.ChangePassword(User.Identity.Name, model.OldPassword,
                    model.NewPassword))
                {

                    ViewBag.StatusMessage = "Password successfully changed";
                    return View(model);
                }
            }
            ModelState.AddModelError("", "Incorrect password");
            return View(model);
        }


        private    ActionResult RedirectToLocal(string url)
        {
            if (Url.IsLocalUrl(url))
            {
                return Redirect(url);
            }
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            service.Dispose();
            base.Dispose(disposing);
        }

    }
}
