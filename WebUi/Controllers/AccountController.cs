using System;
using System.Drawing.Imaging;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SocialNetwork.Bll.Interface.Entity;
using SocialNetwork.Bll.Interface.Services;
using SocialNetwork.Logger.Interface;
using WebUi.Infractracture;
using WebUi.Infractracture.Mappers;
using WebUi.Models;
using WebUi.Providers;

namespace WebUi.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {

        #region Fields

        private readonly IUserService service;
        private readonly ILogger logger;

        private static readonly int CaptchaMinValue = 1111;
        private static readonly int CaptchaMaxValue = 9999;
        private static readonly int CaptchaWidth = 211;
        private static readonly int CaptchaHeight = 50;
        private static readonly string CaptchaFamilyName = "Helvetica";

        #endregion

        #region Constractors

        public AccountController(IUserService service, ILogger logger)
        {
            this.service = service;
            this.logger = logger;
        }

        #endregion

        #region Action Methods

        public ActionResult Login(string returnUrl)
        {
            logger.Log(LogLevel.Trace,"Request login page");
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                logger.Log(LogLevel.Trace,"Try to login userName = {0}", model.UserName);
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                logger.Log(LogLevel.Debug,"Login fault userName = {0}", model.UserName);
                ModelState.AddModelError("", "Incorrect user name or password");
            }

            return View(model);
            
        }

        [Authorize]
        public ActionResult LogOff()
        {
            logger.Log(LogLevel.Trace,"Request logoff");
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Register()
        {
            logger.Log(LogLevel.Trace,"Request register page");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model, HttpPostedFileWrapper avatar)
        {
            if (model.Captcha != (string)Session[CaptchaImage.captchaValueKey])
            {
                logger.Log(LogLevel.Debug,"Invalid captcha. Input value = {0}, correct value = {1}", model.Captcha, Session[CaptchaImage.captchaValueKey]);
                ModelState.AddModelError("Captcha", "Incorrect text from image");
                return View(model);
            }

            if (service.IsUserExists(model.UserName))
            {
                logger.Log(LogLevel.Trace,"Try to register exist user userName = {0}", model.UserName);
                ModelState.AddModelError("UserName", "User with this name already exists");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                logger.Log(LogLevel.Trace,"Try to register userName = {0}", model.UserName);
                MembershipUser membershipUser = ((UserMembershipProvider) Membership.Provider)
                    .CreateUser(model.UserName, model.Password, model.Name, model.Surname,
                    model.AboutUser, model.BirthDay, model.Sex == null ? null : (BllSex?)(int) model.Sex);

                if (membershipUser != null)
                {
                    Roles.AddUserToRole(model.UserName, "User");
                    FormsAuthentication.SetAuthCookie(model.UserName, false);

                    if (avatar != null) service.SetUserAvatar((int)membershipUser.ProviderUserKey, avatar.InputStream);

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
            logger.Log(LogLevel.Trace,"Request edit user page userName = {0}", User.Identity.Name);
            try
            {
                return View(service.GetByName(User.Identity.Name).ToEdirAccountViewModel());
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Fatal, ex.ToString());
                ViewBag.StatusMessage = "User not found. Please try again later.";
                return View((object)null);
            }     
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(EditAccountViewModel model, HttpPostedFileWrapper avatar)
        {
           
            if (ModelState.IsValid)
            {
                logger.Log(LogLevel.Trace,"Edit user page userName = {0}", User.Identity.Name);

                BllUser newUser = model.ToBllUser();
                BllUser oldUser = service.GetById(model.Id);
                if (oldUser == null) throw new HttpException(404, "Not found");

                newUser.UserName = oldUser.UserName;
                newUser.PasswordHash = oldUser.PasswordHash;
                service.Update(newUser);

                if (avatar != null) service.SetUserAvatar(model.Id, avatar.InputStream);

                ViewBag.StatusMessage = "User information successfully changed";
                return View(model);
            }
            return View(model);
        }
        
        [HttpGet]
        [Authorize]
        public ActionResult ChangePassword()
        {
            logger.Log(LogLevel.Trace,"Request ChangePassword page userName = {0}", User.Identity.Name);
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
                    logger.Log(LogLevel.Trace,"Change password  userName = {0}", User.Identity.Name);
                    ViewBag.StatusMessage = "Password successfully changed";
                    return View(model);
                }
                logger.Log(LogLevel.Trace,"Change password fault userName = {0}", User.Identity.Name);
            }
            ModelState.AddModelError("", "Incorrect password");
            return View(model);
        }


        public ActionResult Captcha()
        {
            logger.Log(LogLevel.Trace,"Request Captcha page");
            Session[CaptchaImage.captchaValueKey] =
                new Random(DateTime.Now.Millisecond).Next(CaptchaMinValue, CaptchaMaxValue).ToString(CultureInfo.InvariantCulture);
            
            using(CaptchaImage captcha = 
                new CaptchaImage(Session[CaptchaImage.captchaValueKey].ToString(), CaptchaWidth, CaptchaHeight, CaptchaFamilyName))
            {
                Response.Clear();
                Response.ContentType = "image/jpeg";
                captcha.Image.Save(this.Response.OutputStream, ImageFormat.Jpeg);
            }
            return null;
        }

        #endregion

        #region Private Methods

        private    ActionResult RedirectToLocal(string url)
        {
            if (Url.IsLocalUrl(url))
            {
                return Redirect(url);
            }
            return RedirectToAction("Index", "Home");
        }

        #endregion
        
    }
}
